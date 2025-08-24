#!/bin/bash
set -e

echo "Aguardando banco de dados ficar pronto..."
# Aguardar o PostgreSQL ficar disponível
max_attempts=30
attempt=1

while ! nc -z postgres 5432; do
  if [ $attempt -ge $max_attempts ]; then
    echo "Erro: Banco de dados não ficou pronto após $max_attempts tentativas"
    exit 1
  fi
  echo "Tentativa $attempt/$max_attempts - Banco ainda não pronto, aguardando..."
  sleep 2
  attempt=$((attempt + 1))
done

echo "Banco de dados pronto! Executando migrações..."

# Verificar se as ferramentas EF estão instaladas
if ! command -v dotnet-ef &> /dev/null; then
    echo "Instalando dotnet-ef tool..."
    dotnet tool install --global dotnet-ef --version 8.0.0
    export PATH="$PATH:/root/.dotnet/tools"
fi

# Executar migrações
echo "Executando: dotnet ef database update..."
dotnet ef database update --project ../DeveloperStore.Infrastructure/DeveloperStore.Infrastructure.csproj --startup-project .

if [ $? -eq 0 ]; then
    echo "Migrações concluídas com sucesso!"
else
    echo "Erro ao executar migrações. Verifique se:"
    echo "1. O projeto Infrastructure existe no caminho correto"
    echo "2. As dependências estão restauradas"
    echo "3. A connection string está correta"
    exit 1
fi

echo "Iniciando aplicação..."
exec "$@"