Todo 

- [ ] Create pre-requisites
  - [ ] Powershell (if windows)
  - [ ] Dotnet core (3.1 or 5-preview)
  - [ ] Azure CLI - https://docs.microsoft.com/en-us/cli/azure/install-azure-cli 
  - [ ] Azure account : http://portal.azure.com/
- [ ] Create a web-api from boilerplate
- [ ] Create a healtch check API : send version, remarks
- [ ] Create docker image
- [ ] Crate kubernetes config
- [ ] Run with Kind locally 
- [ ] Create AKS cluster
- [ ] Deploy to AKS from Azure CLI
- [ ] Create a build pipeline
- [ ] Create a release pipeline 



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




