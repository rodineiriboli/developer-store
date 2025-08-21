# DeveloperStore API

## Pré-requisitos
- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code

## Configuração

1. Clone o repositório
2. Configure a string de conexão no arquivo `appsettings.json`
3. Execute as migrações do Entity Framework:
   ```bash
   dotnet ef database update --project src/DeveloperStore.Infrastructure --startup-project src/DeveloperStore.API