using System.Net;
using System.Text.Json;
using EmployeeManagement.Models;
using FluentAssertions;
using ReqNRoll;
using TechTalk.SpecFlow;

namespace EmployeeManagement.BDDTests.StepDefinitions
{
    [Binding]
    public class PromotionsSteps
    {
        private readonly ReqNRollClient _client;
        private HttpResponseMessage _response;
        private PromotionResultDto _promotionResult;

        public PromotionsSteps()
        {
            _client = new ReqNRollClient("http://localhost:5000");
        }

        [Given(@"I have an employee with ID ""(.*)""")]
        public void GivenIHaveAnEmployeeWithID(string employeeId)
        {
            _promotionResult = new PromotionResultDto
            {
                EmployeeId = Guid.Parse(employeeId)
            };
        }

        [When(@"I send a POST request to ""(.*)"" with the employee ID")]
        public async Task WhenISendAPOSTRequestToWithTheEmployeeID(string url)
        {
            var promotionForCreation = new PromotionForCreationDto
            {
                EmployeeId = _promotionResult.EmployeeId
            };
            _response = await _client.PostAsync(url, promotionForCreation);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"the response should contain a promotion result with the employee ID ""(.*)""")]
        public async Task ThenTheResponseShouldContainAPromotionResultWithTheEmployeeID(string employeeId)
        {
            var promotionResult = await _response.Content.ReadAsAsync<PromotionResultDto>();
            promotionResult.EmployeeId.Should().Be(Guid.Parse(employeeId));
        }

        [Then(@"the job level should be greater than (.*)")]
        public async Task ThenTheJobLevelShouldBeGreaterThan(int jobLevel)
        {
            var promotionResult = await _response.Content.ReadAsAsync<PromotionResultDto>();
            promotionResult.JobLevel.Should().BeGreaterThan(jobLevel);
        }
    }
}
