FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY src/IdentityService/IdentityService.csproj ./src/IdentityService/

RUN dotnet restore ./src/IdentityService/IdentityService.csproj

COPY . . 
RUN dotnet publish ./src/IdentityService/IdentityService.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.dll"]