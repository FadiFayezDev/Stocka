FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Domain/Domain.csproj Domain/
RUN dotnet restore API/API.csproj

FROM restore AS publish
COPY . .
WORKDIR /src/API
RUN dotnet publish API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
