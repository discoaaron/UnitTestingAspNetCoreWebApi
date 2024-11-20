using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.BDDTests
{
    public class IntegrationTestBase : IAsyncLifetime
    {
        private IContainerService _container;
        protected HttpClient _client;
        protected IServiceProvider _serviceProvider;

        public async Task InitializeAsync()
        {
            // Start Docker container
            _container = new Builder()
                .UseContainer()
                .UseImage("your-docker-image")
                .ExposePort(5000, 80)
                .Build()
                .Start()
                .WaitForPort("80/tcp", 30000);

            // Create HttpClient
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            // Create service provider with mock dependencies
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        public Task DisposeAsync()
        {
            _client.Dispose();
            _container.Dispose();
            return Task.CompletedTask;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            // Add mock dependencies
            var mockService = Substitute.For<IMyService>();
            services.AddSingleton(mockService);
        }
    }
}
