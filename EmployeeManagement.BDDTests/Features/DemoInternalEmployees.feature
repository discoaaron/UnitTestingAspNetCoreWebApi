Feature: DemoInternalEmployeesController

  Scenario: CreateInternalEmployee
    Given I have a new internal employee with first name "John" and last name "Doe"
    When I send a POST request to "/api/demointernalemployees"
    Then the response status code should be 201
    And the response should contain an internal employee with first name "John" and last name "Doe"

  Scenario: GetProtectedInternalEmployees
    Given I have a valid JWT token
    When I send a GET request to "/api/demointernalemployees" with the token
    Then the response status code should be 302
    And the response should redirect to "/api/internalemployees"
