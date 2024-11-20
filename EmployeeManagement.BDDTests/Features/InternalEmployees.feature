Feature: InternalEmployeesController

  Scenario: GetInternalEmployees
    When I send a GET request to "/api/internalemployees"
    Then the response status code should be 200
    And the response should contain a list of internal employees

  Scenario: GetInternalEmployee
    Given I have an internal employee with ID "bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"
    When I send a GET request to "/api/internalemployees/bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"
    Then the response status code should be 200
    And the response should contain an internal employee with ID "bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"

  Scenario: CreateInternalEmployee
    Given I have a new internal employee with first name "Megan" and last name "Jones"
    When I send a POST request to "/api/internalemployees"
    Then the response status code should be 201
    And the response should contain an internal employee with first name "Megan" and last name "Jones"
