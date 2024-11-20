using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using EmployeeManagement.Models;
using FluentAssertions;
using ReqNRoll;
using TechTalk.SpecFlow;

namespace EmployeeManagement.BDDTests.StepDefinitions
{
    [Binding]
    public class DemoInternalEmployeesSteps
    {
        private readonly ReqNRollClient _client;
        private HttpResponseMessage _response;
        private InternalEmployeeForCreationDto _internalEmployeeForCreation;
        private string _token;

        public DemoInternalEmployeesSteps()
        {
            _client = new ReqNRollClient("http://localhost:5000");
        }

        [Given(@"I have a new internal employee with first name ""(.*)"" and last name ""(.*)""")]
        public void GivenIHaveANewInternalEmployeeWithFirstNameAndLastName(string firstName, string lastName)
        {
            _internalEmployeeForCreation = new InternalEmployeeForCreationDto
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        [When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPostRequestTo(string url)
        {
            _response = await _client.PostAsync(url, _internalEmployeeForCreation);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"the response should contain an internal employee with first name ""(.*)"" and last name ""(.*)""")]
        public async Task ThenTheResponseShouldContainAnInternalEmployeeWithFirstNameAndLastName(string firstName, string lastName)
        {
            var createdEmployee = await _response.Content.ReadAsAsync<InternalEmployeeDto>();
            createdEmployee.FirstName.Should().Be(firstName);
            createdEmployee.LastName.Should().Be(lastName);
        }

        [Given(@"I have a valid JWT token")]
        public void GivenIHaveAValidJwtToken()
        {
            _token = "your-jwt-token";
        }

        [When(@"I send a GET request to ""(.*)"" with the token")]
        public async Task WhenISendAGetRequestToWithTheToken(string url)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _response = await _client.GetAsync(url);
        }

        [Then(@"the response should redirect to ""(.*)""")]
        public void ThenTheResponseShouldRedirectTo(string url)
        {
            _response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            _response.Headers.Location.ToString().Should().Contain(url);
        }
    }
}
