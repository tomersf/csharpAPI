FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /api
COPY "api.csproj" .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY .env .
COPY --from=build /app .
ENTRYPOINT ["dotnet", "api.dll"]