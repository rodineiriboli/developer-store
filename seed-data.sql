-- Inserir dados fictícios na tabela Users
INSERT INTO "Users" ("Id", "Email", "Username", "PasswordHash", "FirstName", "LastName", "City", "Street", "Number", "ZipCode", "GeoLat", "GeoLong", "Phone", "Status", "Role") VALUES
('a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'admin@developerstore.com', 'admin', '$2a$11$rLcJk3k4S5t6X7y8Z9A/BeCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG', 'Admin', 'User', 'São Paulo', 'Av. Paulista', 1000, '01310-100', '-23.5614', '-46.6558', '+5511999999999', 'active', 'admin'),
('b2c3d4e5-f6g7-8901-bcde-f23456789012', 'john.doe@email.com', 'johndoe', '$2a$11$rLcJk3k4S5t6X7y8Z9A/BeCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG', 'John', 'Doe', 'Rio de Janeiro', 'Rua Copacabana', 200, '22050-001', '-22.9707', '-43.1820', '+5521987654321', 'active', 'customer'),
('c3d4e5f6-g7h8-9012-cdef-345678901234', 'jane.smith@email.com', 'janesmith', '$2a$11$rLcJk3k4S5t6X7y8Z9A/BeCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG', 'Jane', 'Smith', 'Belo Horizonte', 'Av. Afonso Pena', 1500, '30130-007', '-19.9167', '-43.9345', '+5531999999999', 'active', 'customer');

-- Inserir dados fictícios na tabela Products
INSERT INTO "Products" ("Id", "Title", "Price", "Description", "Category", "Image", "RatingRate", "RatingCount") VALUES
('d4e5f6g7-h8i9-0123-defg-456789012345', 'iPhone 15 Pro', 999.99, 'Latest iPhone with advanced camera system', 'electronics', 'https://example.com/iphone15.jpg', 4.8, 120),
('e5f6g7h8-i9j0-1234-efgh-567890123456', 'Samsung Galaxy S24', 899.99, 'Powerful Android smartphone with AMOLED display', 'electronics', 'https://example.com/galaxys24.jpg', 4.6, 95),
('f6g7h8i9-j0k1-2345-fghi-678901234567', 'MacBook Pro 16"', 2399.99, 'Professional laptop for developers and creatives', 'electronics', 'https://example.com/macbookpro.jpg', 4.9, 80),
('g7h8i9j0-k1l2-3456-ghij-789012345678', 'Nike Air Jordan', 199.99, 'Classic basketball shoes with modern comfort', 'clothing', 'https://example.com/airjordan.jpg', 4.7, 65),
('h8i9j0k1-l2m3-4567-hijk-890123456789', 'Adidas Ultraboost', 179.99, 'Running shoes with responsive cushioning', 'clothing', 'https://example.com/ultraboost.jpg', 4.5, 110),
('i9j0k1l2-m3n4-5678-ijkl-901234567890', 'JavaScript: The Good Parts', 29.99, 'Essential guide to JavaScript programming', 'books', 'https://example.com/jsgoodparts.jpg', 4.8, 200),
('j0k1l2m3-n4o5-6789-jklm-012345678901', 'Clean Code: A Handbook of Agile Software Craftsmanship', 39.99, 'Software craftsmanship principles and patterns', 'books', 'https://example.com/cleancode.jpg', 4.9, 350),
('k1l2m3n4-o5p6-7890-klmn-123456789012', 'Sony WH-1000XM5 Headphones', 349.99, 'Noise-cancelling wireless headphones', 'electronics', 'https://example.com/sonyheadphones.jpg', 4.7, 180),
('l2m3n4o5-p6q7-8901-lmno-234567890123', 'Logitech MX Master 3', 99.99, 'Advanced wireless mouse for productivity', 'electronics', 'https://example.com/mxmaster.jpg', 4.6, 140),
('m3n4o5p6-q7r8-9012-mnop-345678901234', 'Kindle Paperwhite', 129.99, 'Waterproof e-reader with built-in light', 'electronics', 'https://example.com/kindle.jpg', 4.5, 95);

-- Inserir dados fictícios na tabela Carts
INSERT INTO "Carts" ("Id", "UserId", "Date") VALUES
('n4o5p6q7-r8s9-0123-nopq-456789012345', 1, '2024-01-15 10:30:00+00'),
('o5p6q7r8-s9t0-1234-opqr-567890123456', 2, '2024-01-15 11:45:00+00');

-- Inserir dados fictícios na tabela CartItems
INSERT INTO "CartItems" ("Id", "ProductId", "Quantity", "CartId") VALUES
('p6q7r8s9-t0u1-2345-pqrs-678901234567', 1, 2, 'n4o5p6q7-r8s9-0123-nopq-456789012345'),
('q7r8s9t0-u1v2-3456-qrst-789012345678', 3, 1, 'n4o5p6q7-r8s9-0123-nopq-456789012345'),
('r8s9t0u1-v2w3-4567-rstu-890123456789', 5, 1, 'o5p6q7r8-s9t0-1234-opqr-567890123456');

-- Inserir dados fictícios na tabela Sales
INSERT INTO "Sales" ("Id", "SaleNumber", "SaleDate", "CustomerId", "CustomerName", "CustomerEmail", "TotalAmount", "BranchId", "BranchName", "BranchLocation", "IsCancelled") VALUES
('s9t0u1v2-w3x4-5678-stuv-901234567890', 'SALE-00001', '2024-01-10 14:30:00+00', 2, 'John Doe', 'john.doe@email.com', 1239.98, 1, 'São Paulo Store', 'Shopping Paulista, São Paulo', false),
('t0u1v2w3-x4y5-6789-tuvw-012345678901', 'SALE-00002', '2024-01-12 16:45:00+00', 3, 'Jane Smith', 'jane.smith@email.com', 179.99, 2, 'Rio de Janeiro Store', 'Shopping Rio Sul, Rio de Janeiro', false);

-- Inserir dados fictícios na tabela SaleItems
INSERT INTO "SaleItems" ("Id", "ProductId", "ProductName", "ProductDescription", "Quantity", "UnitPrice", "Discount", "SaleId") VALUES
('u1v2w3x4-y5z6-7890-uvwx-123456789012', 1, 'iPhone 15 Pro', 'Latest iPhone with advanced camera system', 1, 999.99, 0, 's9t0u1v2-w3x4-5678-stuv-901234567890'),
('v2w3x4y5-z6a7-8901-vwxy-234567890123', 4, 'Nike Air Jordan', 'Classic basketball shoes with modern comfort', 1, 199.99, 20.00, 's9t0u1v2-w3x4-5678-stuv-901234567890'),
('w3x4y5z6-a7b8-9012-wxyz-345678901234', 5, 'Adidas Ultraboost', 'Running shoes with responsive cushioning', 1, 179.99, 0, 't0u1v2w3-x4y5-6789-tuvw-012345678901');