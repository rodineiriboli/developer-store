# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["developer-store.sln", "."]
COPY ["src/DeveloperStore.API/DeveloperStore.API.csproj", "src/DeveloperStore.API/"]
COPY ["src/DeveloperStore.Application/DeveloperStore.Application.csproj", "src/DeveloperStore.Application/"]
COPY ["src/DeveloperStore.Domain/DeveloperStore.Domain.csproj", "src/DeveloperStore.Domain/"]
COPY ["src/DeveloperStore.Infrastructure/DeveloperStore.Infrastructure.csproj", "src/DeveloperStore.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "developer-store.sln"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/DeveloperStore.API"
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create SSL certificate directory and set permissions
RUN mkdir -p /https && chmod 700 /https

# Expose ports
EXPOSE 80
EXPOSE 443

# Entry point
ENTRYPOINT ["dotnet", "DeveloperStore.API.dll"]