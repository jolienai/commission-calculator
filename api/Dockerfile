# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and project files
COPY . .

# Restore dependencies
RUN dotnet restore FCamara.CommissionCalculator.sln

# Publish the main API project
RUN dotnet publish ./FCamara.Commission.Api/FCamara.Commission.Api.csproj -c Release -o /app/publish

# Stage 2: Runtimels -l
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish ./

# Expose the API on port 5000
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "FCamara.Commission.Api.dll"]