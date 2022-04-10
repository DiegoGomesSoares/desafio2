FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY /Payment/Payment.sln ./
COPY /Payment/Domain/Domain.csproj ./Domain/ 
COPY /Payment/Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY /Payment/UnitTests/UnitTests.csproj ./UnitTests/
COPY /Payment/Payment/Payment.csproj ./Payment/

RUN dotnet restore
COPY . .
WORKDIR /src/Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/Infrastructure
RUN dotnet build -c Release -o /app

#WORKDIR /src/UnitTests
#RUN dotnet build -c Release -o /app

WORKDIR /src/Payment
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Payment.dll"]