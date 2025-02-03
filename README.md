# Employee Programming Challenge


## Endpoints

* Task 1: Get the reporting structure for an employee
	* HTTP Method: GET
	* URL: localhost:8080/api/reportingstructure/{employeeId}
	* RESPONSE: ReportingStructure


* Task 2: Get the effective compensation record by Employee ID
	* HTTP Method: GET
	* URL: localhost:8080/api/compensation/{employeeId}
	* RESPONSE: Compensation
	
	
* Task 2: Add a compensation record to an employee
	* HTTP Method: POST
	* URL: localhost:8080/api/compensation/
	* PAYLOAD: Compensation (abbreviated)
	* RESPONSE: Compensation
	
The payload here is defined as:

{
  "type":"Compensation",
  "properties": {
	"employeeId": {
	  "type": "string"
	},
	"salary": {
	  "type": "float"
	},
	"effectiveDate": {
		  "type": "DateOnly"
	}
  }
}

For example:

{
	"employeeId": "16a596ae-edd3-4847-99fe-c4518e82c86f",
	"salary": 123.45,
	"effectiveDate": "2025-01-15"
}



## A word on Unit Test Coverage
I've tried to include a representative sample of unit tests for the new major pieces, but I did not strive for 100% case coverage.  My intention is to show my thought process in building the new logic, and illustrate my capabilities in writing tests:

* In some cases I worked to capture all of the edge cases I could think of.  A good example of this are the tests covering CompensationRepository.GetByEmployeeId.

* In other cases, I've left TODOs explaining what should be tested and included direction on how to write those tests.  Most of the time that direction is along the lines of "I need tests here just like I wrote for class ABC".  In one case (CodeChallenge.Helpers.DateOnlyJsonConverter), the direction is "I found an example of how to test this; modify that code to work with this particular class".

