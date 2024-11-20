using ReqNRoll;
using FluentAssertions;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TopLevelManagement;

namespace EmployeeManagement.BDDTests
{
    public class BDDTests
    {
        private readonly ReqNRollClient _client;

        public BDDTests()
        {
            _client = new ReqNRollClient("http://localhost:5000");
        }

        [Fact]
        public async Task CreateInternalEmployee_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var internalEmployeeForCreation = new
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var response = await _client.PostAsync("/api/demointernalemployees", internalEmployeeForCreation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdEmployee = await response.Content.ReadAsAsync<InternalEmployeeDto>();
            createdEmployee.FirstName.Should().Be("John");
            createdEmployee.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task GetProtectedInternalEmployees_ShouldRedirectToGetInternalEmployees()
        {
            // Arrange
            var token = "your-jwt-token";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/demointernalemployees");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.ToString().Should().Contain("/api/internalemployees");
        }

        [Fact]
        public async Task GetInternalEmployees_ShouldReturnEmployees()
        {
            // Act
            var response = await _client.GetAsync("/api/internalemployees");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var employees = await response.Content.ReadAsAsync<IEnumerable<InternalEmployeeDto>>();
            employees.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreatePromotion_ShouldPromoteEmployee()
        {
            // Arrange
            var promotionForCreation = new
            {
                EmployeeId = Guid.NewGuid()
            };

            // Act
            var response = await _client.PostAsync("/api/promotions", promotionForCreation);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var promotionResult = await response.Content.ReadAsAsync<PromotionResultDto>();
            promotionResult.EmployeeId.Should().Be(promotionForCreation.EmployeeId);
            promotionResult.JobLevel.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Pay_ShouldReturnOk()
        {
            // Arrange
            var payDto = new PayDto(5000, 1, new int[] { 1, 2, 3 }, "TX123");

            // Act
            var response = await _client.PostAsync("/api/promotioneligibilities/{employeeId}/pay", payDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
