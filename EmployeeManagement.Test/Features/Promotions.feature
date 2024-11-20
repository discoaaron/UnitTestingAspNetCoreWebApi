Feature: PromotionsController

  Scenario: CreatePromotion
    Given I have an employee with ID "72f2f5fe-e50c-4966-8420-d50258aefdcb"
    When I send a POST request to "/api/promotions" with the employee ID
    Then the response status code should be 200
    And the response should contain a promotion result with the employee ID "72f2f5fe-e50c-4966-8420-d50258aefdcb"
    And the job level should be greater than 0
