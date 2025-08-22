#!/bin/sh
# Script para aguardar o banco de dados ficar pronto

set -e

host="$1"
shift
port="$1"
shift
cmd="$@"

until PGPASSWORD=$POSTGRES_PASSWORD psql -h "$host" -p "$port" -U "postgres" -c '\q'; do
  >&2 echo "PostgreSQL is unavailable - sleeping"
  sleep 1
done

>&2 echo "PostgreSQL is up - executing command"
exec $cmd