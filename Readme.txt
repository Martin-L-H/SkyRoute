To run it is currently necessary to make sure the project start is set to infrastructure, do a migration, update the database, 
then run the API layer, and do a NPM START on the terminal within the "ClientApp" directory.

The front-end is an angular signal state based SPA that connects to the back-end using service layers.

The back-end is a layer type C# API that uses Domain, Infrastructure, Application, and API layers to connect to a MSSQL database that is pre-seeded when initialized.
ServiceResponse is used with DTOs for returning error message descriptions back to the user in the case of a badrequest using IActionResult in the controller layer.

Currently the "Providers" are objects, instead of API calls, or JSON objects. To add a new provider a new class for a provider needs to be added with their own price
parameters created. Sadly I did not had time to decouple the ORM-DTO mapping from the providers class,
so every provider would have to also map the record DTO response for the front-end.

The loggers are basic console loggers without monitoring tools, rate limit has been added, Connection string is done from the appsettings.Development.json

Insertion of new countries, cities, airports, and flights must be done by T-SQL directly.

I originally intended to have a docker/Kubernetes compatible service running, however I came across technical difficulties and was not able to get that working in time.

This code also has no "job queuing" for Booking inserts, just race flag conditions. This makes the code insecure and has the possibility of accidental overbooking
if two people make the same request at the same time.