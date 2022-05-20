Challenge: write a web api that will call an external(3rd party) service that will return past years accounting information for NYC departments.
include unit tests, error handling, caching, dependency injection,  and best practices.

 the api should have endpoints that:
   1. return departments whose expenses meet or exceed their funding
   2. return deparments whose expenses have increased over time by user specified percentage (int) and # of years (int)
   3. return departments whose expenses are a user specified percentage below their funding year over year.
   
   Note: for this challenge the json indexes of importance are:
   9 = fiscal year 
   10 = dept. id
   11 = dept. name
   12 = funds available
   13 = funds used
   14 = remarks


Mock API: https://mockbin.org/bin/20acd654-c45a-4cea-bf6c-ad320a3dc303
