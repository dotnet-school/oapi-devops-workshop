Create a simple docker file with code : 

```
# Stage 1: Use an image with SDK (so that we can compile and build app)
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# Run dotnet restore
COPY OAPI.Service/*.csproj .
RUN dotnet restore

# Copy rest of project and publish app int /app directory
COPY ./OAPI.Service .
RUN dotnet publish -c release -o /app --no-restore

EXPOSE 80
ENTRYPOINT ["dotnet", "OAPI.Service.dll"]
```

Check if build succeeds 

```
docker build -t oapi-service .
docker run -p 5001:80 oapi-service
```

Check api response.