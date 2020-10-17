# Stage 1: Use an image with SDK (so that we can compile and build app)
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# Run dotnet restore
COPY OAPI.Service/*.csproj .
RUN dotnet restore

# Copy rest of project and publish app int /app directory
COPY ./OAPI.Service .
RUN dotnet publish -c release -o /app --no-restore
