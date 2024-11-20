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
    public class InternalEmployeesSteps
    {
        private readonly ReqNRollClient _client;
        private HttpResponseMessage _response;
        private InternalEmployeeDto _internalEmployee;

        public InternalEmployeesSteps()
        {
            _client = new ReqNRollClient("http://localhost:5000");
        }

        [When(@"I send a GET request to ""(.*)""")]
        public async Task WhenISendAGETRequestTo(string url)
        {
            _response = await _client.GetAsync(url);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"the response should contain a list of internal employees")]
        public async Task ThenTheResponseShouldContainAListOfInternalEmployees()
        {
            var employees = await _response.Content.ReadAsAsync<IEnumerable<InternalEmployeeDto>>();
            employees.Should().NotBeEmpty();
        }

        [Given(@"I have an internal employee with ID ""(.*)""")]
        public void GivenIHaveAnInternalEmployeeWithID(string employeeId)
        {
            _internalEmployee = new InternalEmployeeDto
            {
                Id = Guid.Parse(employeeId),
                FirstName = "John",
                LastName = "Doe"
            };
        }

        [Then(@"the response should contain an internal employee with ID ""(.*)""")]
        public async Task ThenTheResponseShouldContainAnInternalEmployeeWithID(string employeeId)
        {
            var employee = await _response.Content.ReadAsAsync<InternalEmployeeDto>();
            employee.Id.Should().Be(Guid.Parse(employeeId));
        }

        [Given(@"I have a new internal employee with first name ""(.*)"" and last name ""(.*)""")]
        public void GivenIHaveANewInternalEmployeeWithFirstNameAndLastName(string firstName, string lastName)
        {
            _internalEmployee = new InternalEmployeeDto
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        [When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPOSTRequestTo(string url)
        {
            _response = await _client.PostAsync(url, _internalEmployee);
        }

        [Then(@"the response should contain an internal employee with first name ""(.*)"" and last name ""(.*)""")]
        public async Task ThenTheResponseShouldContainAnInternalEmployeeWithFirstNameAndLastName(string firstName, string lastName)
        {
            var employee = await _response.Content.ReadAsAsync<InternalEmployeeDto>();
            employee.FirstName.Should().Be(firstName);
            employee.LastName.Should().Be(lastName);
        }
    }
}
