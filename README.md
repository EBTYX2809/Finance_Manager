<h1 align="center">It's backend server for finance manager.</h1>

### Technical details:
- Backend architecture is N-Tier Layered.
- Works with !MS SQL Server database via ORM Entity Framework Core.
- Performs all basic CRUD operations with additional scenarios.
- Services are covered by modular and integration tests;
- Implemented REST API for all necessary requests with documentation via Swagger;
- Uses logging (Serilog), mapping (AutoMapper), input data validation (FluentValidation);
- Added fully modern registration/authentication, authorization via JWT access token and refresh mechanic;
- Works with user balance via database transactions.
- Interaction with a open API for currency conversion.
- Caching of exchange rate data.
- A global exception handler added as middleware;
- Deployment and CI/CD updates for the production version on the server have been configured using Docker and GitHub Actions.

<div align="center">
  <h3>Schema of classes in project</h3>
  <img src="https://github.com/EBTYX2809/Finance_Manager/blob/master/Schema%20of%20classes.jpg">
</div>
