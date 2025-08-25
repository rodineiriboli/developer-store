# DeveloperStore API

Uma API RESTful para gerenciamento de vendas desenvolvida em .NET 8.0 seguindo os princípios de Domain-Driven Design (DDD) e Clean Architecture.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8.0** - Plataforma de desenvolvimento
- **C# 11** - Linguagem de programação
- **Entity Framework Core 8.0** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados relacional
- **MediatR** - Implementação do padrão Mediator
- **AutoMapper** - Mapeamento objeto-objeto
- **BCrypt.Net-Next** - Criptografia de senhas
- **JWT Bearer Authentication** - Autenticação por tokens
- **xUnit** - Framework de testes unitários
- **NSubstitute** - Framework de mocking para testes

### Padrões e Arquitetura
- **Domain-Driven Design (DDD)** - Design centrado no domínio
- **Clean Architecture** - Separação em camadas
- **CQRS** - Segregação de leitura e escrita
- **Repository Pattern** - Abstração de acesso a dados
- **Unit of Work** - Gerenciamento de transações
- **Value Objects** - Objetos de valor imutáveis
- **Domain Events** - Eventos de domínio

## 📋 Funcionalidades

### Módulos Implementados
- **🔐 Autenticação** - Login, registro e gestão de usuários
- **👥 Users** - CRUD completo de usuários
- **🛍️ Products** - Gestão de produtos e categorias
- **🛒 Carts** - Carrinhos de compras
- **💰 Sales** - Processamento de vendas com regras de desconto

### Regras de Negócio
- Sistema de descontos baseado em quantidade:
  - 4+ itens: 10% de desconto
  - 10-20 itens: 20% de desconto
  - Limite de 20 itens por produto

## 🛠️ Configuração do Ambiente

### Pré-requisitos
- .NET 8.0 SDK
- PostgreSQL 15+
- Visual Studio 2022 ou VS Code
- Git

### 1. Clone o repositório
```bash
git clone https://github.com/rodineiriboli/developer-store.git
cd DeveloperStore
```

### 2. Configure o banco de dados com Docker
```bash
# Subir apenas o PostgreSQL
docker-compose up

# Verificar se o banco está rodando
docker-compose ps
```

### 3. Configure a connection string

Caso precise alterar a string de conexão.

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=DeveloperStore;Username=postgres;Password=postgrespassword;"
  },
  "Jwt": {
    "Secret": "SUA_CHAVE_SECRETA_MUITO_SEGURA_AQUI_MINIMO_32_CARACTERES",
    "ExpiresInHours": 8
  }
}
```

### 4. Aplique as migrações
```bash
# Via Package Manager Console no Visual Studio
Update-Database -Project DeveloperStore.Infrastructure -StartupProject DeveloperStore.API

# Ou via CLI
dotnet ef database update --project src/DeveloperStore.Infrastructure --startup-project src/DeveloperStore.API
```

### 5. Execute a aplicação
```bash
# Desenvolvimento
dotnet run --project src/DeveloperStore.API

# Ou pelo Visual Studio
# Setar DeveloperStore.API como projeto de inicialização e executar
```

## 🧪 Executando Testes

### Testes Unitários
```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~DeveloperStore.Domain.Tests"
dotnet test --filter "FullyQualifiedName~DeveloperStore.Application.Tests"

# Ou Visualmente via janela do Gerenciador de Testes do Visual Studio
```

## 📊 Estrutura do Projeto

```
DeveloperStore/
├── src/
│   ├── DeveloperStore.API/          # Camada de apresentação
│   ├── DeveloperStore.Application/  # Casos de uso e DTOs
│   ├── DeveloperStore.Domain/       # Entidades e regras de negócio
│   └── DeveloperStore.Infrastructure/ # Implementações de infra
└── tests/
    ├── DeveloperStore.Domain.Tests/
    ├── DeveloperStore.Application.Tests/
```

**Nota**: Este projeto utiliza Docker apenas para o banco de dados. As migrações devem ser executadas manualmente via terminal ou Package Manager Console.