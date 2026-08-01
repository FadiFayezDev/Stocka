FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY MVC/MVC.csproj MVC/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Domain/Domain.csproj Domain/
RUN dotnet restore MVC/MVC.csproj

FROM restore AS publish
COPY . .
WORKDIR /src/MVC
RUN dotnet publish MVC.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.dll"]
