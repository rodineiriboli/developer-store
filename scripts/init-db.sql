-- Script de inicialização do banco de dados
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Tabela de Sales (será criada pelo EF Core, mas podemos adicionar configurações extras)
-- Esta query verifica se as tabelas já existem para evitar erros
DO $$ 
BEGIN
    -- Você pode adicionar scripts SQL personalizados aqui se necessário
    -- Por exemplo, criar índices extras ou configurações específicas
    RAISE NOTICE 'Database initialization completed';
END $$;