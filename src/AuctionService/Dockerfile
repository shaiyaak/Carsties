FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
EXPOSE 80

COPY Carsties.sln Carsties.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

#restore package deps
RUN dotnet restore Carsties.sln

#copy the app folders
COPY src/AuctionService src/AuctionService
COPY src/Contracts src/Contracts
WORKDIR /app/src/AuctionService
RUN dotnet publish -c Release -o /app/src/out

#build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime 
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet","AuctionService.dll" ]