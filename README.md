# PaymentGateway





This project is a submission for the 'Build a Payment Gateway' take home coding test.

The application has been developed as a MVP to meet the specification. Some of the suggestsions from the 'Extra mile bonus points' section have been implemented to varying levels of completion.



## Endpoints

The application has 2 endpoints as per the spec, technical details of these can be found within the [swagger documentation](https://paymentgatewaycheckout.azurewebsites.net/).


## Authenticaton
Both endpoints are protected by basic authentication, this isn't a suitable approach for production as it's inherintly insecure as the credentials are sent in clear text. 

## Users
2 user accounts are registered within the application, the details of these are contained in the following table. Note that both username and password are **case sensitive**. Each request must include user credentials as basic auth, eg 
`Authorization = base64(username:password)`

| Username     | Password     |
| :------------- | :----------: |
| tombid | test  |
| testuser   | test |

Each user account can *only* retreive transactions created by their own account.


## How to build and run

There are 2 ways to build and run the solution locally, either via the dotnet cli or docker.
 ### Dotnet cli

 NB Ensure you have dotnet sdk >= 3.1.200 installed, this is the lowest version that testing has taken place with
- Clone the repository
- Navigate to root of repository
- run `dotnet restore`
- run `dotnet build`
- run `dotnet run --project 
- run `dotnet run --project .\src\PaymentGateway.Api\`

The application should now be available at http://localhost:35718

### Docker 

NB If running on windows, ensure that you are running on linux containers or WSL2

- Clone the repository
- Navigate to root of repository
- run `docker-compose up --build`

The application should now be available on http://localhost:8080
______________

### Extras

- Applicaiton Logging
  - Basic application logging has been implemented, by default the logs are written to the console. The default log level is information so it can be quite noisy. In a production scenario this would need ot be changed
- Containerization 
  - The application has been containerized for local dev/deployment, currently the application/db reside in the same conntainer as the DB is Sqlite. In a production scenario, the Db would either be a seperate container or a PaaS offering.
- Authentication 
  - As per the readme, basic auth has been implemented. This isn't suitable for a production scenario and is purely for demonstration purposes.
- CI
  - Github actions have been implemented to build/deploy/test the applicaiton. Currently these are implemented as 3 seperate workflows that all run on pushes to master. Going forward, these would need to be refactored to run on non master branches & potentially deploy to multiple environments for testing etc. Some of the steps in workflows are duplicated and could be removed.
- DataStorage
  - The application uses EntityFrameworkCore with Sqlite as the underlying DB, in a production scenario this would likely be inadequate but for the purposes of the demonstration it is sufficent.
- Testing
  - Test reports can be found by clicking the following links, they should be updated after each build. There are some unit tests and some integration tests. Going forward, the variety and quality of these would need to be improved perhaps through the use of a BDD framework.
  - [![Unit Tests](https://gist.githubusercontent.com/tombiddulph/afbcbf12ee578e4eb681df53142d1dc8/raw/c10e7a6c074bad90815eadc9ef6465b03a7e71a3/payment_gateway_unit_tests.md_badge.svg)](https://gist.github.com/tombiddulph/afbcbf12ee578e4eb681df53142d1dc8?short_path=bd77657#file-payment_gateway_unit_tests-md)
  - [![Integraion Tests](https://gist.githubusercontent.com/tombiddulph/71144c6db784490009ee5a252bb0e724/raw/028b9ca70308441251d7e9f2614d35658009732f/payment_gateway_integration_tests.md_badge.svg)](https://gist.github.com/tombiddulph/71144c6db784490009ee5a252bb0e724#file-payment_gateway_integration_tests-md)
- Deployment
  - The application is automatically deployed as a container to an Azure webapp, as part of the build an image is created & pushed to a private Azure container repository. This image is then used to spin up the app. The application can be found here - https://paymentgatewaycheckout.azurewebsites.net/index.html

