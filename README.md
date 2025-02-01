# Appointment Booking Challenge

## Notes to Reviewers

- Please correct 'Requirements' section in the requirements.pdf file. Expected output says that the format of `start_date`
field should `2024-05-03T12:00:00.00Z`, two zeros before `Z`, which is not usual, however the e2e tests expect `start_date` 
to be `2024-05-03T12:00:00.000Z`, three zeros before `Z`, which is common format for UTC dates. Formally speaking, 
the document must be a source of truth for me and I must implement the solution according to it but in this case the test 
suite will fail.

- In the requirements.pdf following is mentioned: 'We might ALSO run additional tests, such as loading thousands of records 
in the database to assert the application is performant enough' and 'Efficiency and performance of the api endpoint'. 
But because there is no definition of what is 'performant enough' for the that particular case, I chose to implement the 
solution in the most straightforward and simple for understanding way. Additional techniques and optimization can be 
implemented but only with clear requirements regarding expected performance.

### Database

- Database form the initial task archive copied to the [database](./database) folder.
- Two indexes where added to the [init.sql](./database/init.sql) file comparing to the initial task archive.

## Solution

### Pre-requisites

The following software needs to be installed on your machine.
Further steps provide installation examples to be run in the `pwsh` and using `winget` utility from Microsoft for Windows 11+.

- [.NET SDK 9](https://dotnet.microsoft.com/en-us/download/dotnet) in order to build & run the solution

    Installation on Windows11+:

        winget install Microsoft.DotNet.SDK.8

- Docker desktop in order to run Postgres.

    Installation on Windows11+:

        winget install Docker.DockerDesktop

### To start db container:

From root of the repository run:

```
cd ./database
docker build -t enpal-coding-challenge-db .
docker run --name enpal-coding-challenge-db -p 5432:5432 -d enpal-coding-challenge-db
```

### To run the api

From root of the repository run:

```
dotnet run -c Release --project ./src/Enplt.Services.Api/Enplt.Services.Api.csproj
```

### To run provided e2e tests

From root of the repository run:

```
cd ./tests
npm i
npm run test
```
