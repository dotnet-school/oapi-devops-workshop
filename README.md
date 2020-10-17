Todo 

- [x] Create pre-requisites
- [x] Create a web-api from boilerplate
- [x] Create a healtch check API : send version, remarks
- [ ] Dockerize Web API
- [ ] Crate kubernetes config
- [ ] Run with Kind locally 
- [ ] Create AKS cluster
- [ ] Deploy to AKS from Azure CLI
- [ ] Create a build pipeline
- [ ] Create a release pipeline 



### Setup

Make sure that following tools are available on your machine before starting.

- [ ] Powershell (if windows)
- [ ] Dotnet core (3.1 or 5-preview)
- [ ] Azure CLI - https://docs.microsoft.com/en-us/cli/azure/install-azure-cli 
- [ ] Azure account : http://portal.azure.com/
- [ ] Github account
  - [ ] Create a new repository for this project (completely empty)
- [ ] Docker
- [ ] Kind



### Creating a Web API 

- Dotnet Core comes with some cool boiler-plate generator 

- Type `dotnet new -h` to view some of the boiler plates available: 
  e.g. : 
  
    <table>
  <tr><td> dotnet new cosnole</td><td>Console Application</td>        </tr>
  <tr><td> dotnet new webapi</td><td>Creat a Web API</td>       </tr>
  <tr><td>dotnet new react</td><td>Creates app with React.js</td> </tr>
  <tr><td>dotnet new grpc</td><td>Creates a gRPC Service  </td></tr>
</table>
  
- We will create a simple json service that will return data at a url :

  ```bash
  dotnet new webapi -o OAPI.Service
  ```

- Now run the app

  ```bash
  cd OAPI.Service
  dotnet run
  # info: Microsoft.Hosting.Lifetime[0]
  #       Now listening on: https://localhost:5001
  # info: Microsoft.Hosting.Lifetime[0]
  #       Now listening on: http://localhost:5000
  # info: Microsoft.Hosting.Lifetime[0]
  #       Application started. Press Ctrl+C to shut down.
  # info: Microsoft.Hosting.Lifetime[0]
  #       Hosting environment: Development
  # info: Microsoft.Hosting.Lifetime[0]
  #       Content root path: /Users/dawn/projects/dotnet-school/oapi-devops-workshop/OAPI.Service
  ```

- Open url https://localhost:5001/weatherforecast, you should get a response like :

  ```json
  [
    {
      "date": "2020-10-18T14:59:15.157908+05:30",
      "temperatureC": 10,
      "temperatureF": 49,
      "summary": "Warm"
    },
    {
      "date": "2020-10-19T14:59:15.158209+05:30",
      "temperatureC": -12,
      "temperatureF": 11,
      "summary": "Balmy"
    },
    {
      "date": "2020-10-20T14:59:15.158214+05:30",
      "temperatureC": -2,
      "temperatureF": 29,
      "summary": "Sweltering"
    },
    {
      "date": "2020-10-21T14:59:15.158214+05:30",
      "temperatureC": -6,
      "temperatureF": 22,
      "summary": "Cool"
    },
    {
      "date": "2020-10-22T14:59:15.158215+05:30",
      "temperatureC": 0,
      "temperatureF": 32,
      "summary": "Balmy"
    }
  ]
  ```
- Now lets generate a `.gitignore` before we can commit and push our project.

  ```bash
  cd ..
  dotnet new gitignore
  git add --all
  git commit -m "Create a web api"
  git remote add origin <your-repository-on-github>
  git push origin master
  ```



### Creating a health check API 

- Lets not think about why we need a health check API for now. Lets take this as a practice to add a new endpoint to our Web API.

- Create a file `OAPI.Service/Controllers/HealthCheckController.cs` : 

  ```c#
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Mvc;
  
  namespace OAPI.Service.Controllers
  {
      [ApiController]
      [Route("")]
      public class HealthCheckController : ControllerBase
      {
         [HttpGet]
          public IDictionary<string, object> Get()
          {
              return new Dictionary<string, object>()
              {
                              ["version"] = "1.0", 
                              ["healthy"] = true, 
                              ["message"] = "Up and running", 
              };
          }
      }
  }
  ```

- Now run the project

  ```
  dotnet run
  ```

- In browser, open url: https://localhost:5001/

  ```json
  {
    "version": "1.0",
    "healthy": true,
    "message": "Up and running"
  }
  ```




### Dockerizing Web API

- Create a `.dockerignore` in project root. It works just like `.gitignore` for docker. It tells docker what files to ignore in our project.

  ```yaml
  # Directories
  **/bin/
  **/obj/
  **/out/
  
  # IDEs
  **/.idea
  **/.vscode
  
  # Files
  Dockerfile*
  README.md
  ```

- Create a `Dockerfile` in our project root and we will use it to compile our project

  ```dockerfile
  # Stage 1: Use an image with SDK (so that we can compile and build app)
  FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
  WORKDIR /source
  
  # Run dotnet restore
  COPY OAPI.Service/*.csproj .
  RUN dotnet restore
  
  # Copy rest of project and publish app int /app directory
  COPY ./OAPI.Service .
  RUN dotnet publish -c release -o /app --no-restore
  ```

- Now lets build and check our docker configuration to compile project

  ```bash
  docker build -t oapi-service .
  
  # Sending build context to Docker daemon  188.4kB
  # Step 1/6 : FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
  #  ---> c4155a9104a8
  # Step 2/6 : WORKDIR /source
  #  ---> Using cache
  #  ---> e3c14feb38e6
  # Step 3/6 : COPY OAPI.Service/*.csproj .
  #  ---> Using cache
  #  ---> a0c5001dbcd5
  # Step 4/6 : RUN dotnet restore
  #  ---> Using cache
  #  ---> df30f3657cbc
  # Step 5/6 : COPY ./OAPI.Service .
  #  ---> Using cache
  #  ---> 86d437cabddd
  # Step 6/6 : RUN dotnet publish -c release -o /app --no-restore
  #  ---> Using cache
  #  ---> 2b0e6f7d8866
  # Successfully built 2b0e6f7d8866
  # Successfully tagged oapi-service:latest
  ```

- Right now our docker image only compiles the project using .net sdk.

- Now, lets make the docker image runt the app. Add following to end of `Dockerfile` : 

  ```dockerfile
  # Stage 2: We do not need the sdk at runtime (smaller efficient image)
  FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
  WORKDIR /app
  
  # Copy files form previous stage 
  COPY --from=build /app .
  
  EXPOSE 80
  ENTRYPOINT ["dotnet", "OAPI.Service.dll"]
  ```

- Lets build and run the docker image now : 

  ```bash
  docker build -t oapi-service .
  
  # Run our docker image as a container
  # Map our machines port 5001 to container's port 80
  docker run -p 5001:80 oapi-service
  ```

- Now open url http://localhost:5001/, it should return our health check as expected : 

  ```json
  {
    "version": "1.0",
    "healthy": true,
    "message": "Up and running"
  }
  ```



- Now lets see if we understand docker, lets try to answer :

  - Why did we copy `.csproj` file before restoring project and then copied the rest of files ?

    ```dockerfile
    # Stage 1: Use an image with SDK (so that we can compile and build app)
    FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
    WORKDIR /source
    
    # Run dotnet restore
    COPY OAPI.Service/*.csproj .
    RUN dotnet restore
    
    # Copy rest of project and publish app int /app directory
    COPY ./OAPI.Service .
    RUN dotnet publish -c release -o /app --no-restore
    
    #....
    ```

    

  - Why did we use two different images in the same `Dockerfile` ? 

    ```dockerfile
    # Stage 1: Use an image with SDK (so that we can compile and build app)
    FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
    WORKDIR /source
    
    # ...
    
    # Stage 2: We do not need the sdk at runtime (smaller efficient image)
    FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
    WORKDIR /app
    
    # ...
    ```

    

