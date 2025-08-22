#!/bin/bash
set -e

echo "Aguardando banco de dados ficar pronto..."
# Aguardar o PostgreSQL ficar disponível
while ! nc -z postgres 5432; do
  sleep 1
done

echo "Banco de dados pronto! Executando migrações..."

# Executar migrações
dotnet ef database update --project ../DeveloperStore.Infrastructure/DeveloperStore.Infrastructure.csproj --startup-project .

echo "Migrações concluídas. Iniciando aplicação..."
exec "$@"