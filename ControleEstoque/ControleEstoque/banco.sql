-- Script do banco (o programa já cria isso sozinho; este arquivo é só para mostrar/usar no Workbench)
CREATE DATABASE IF NOT EXISTS estoque_db CHARACTER SET utf8mb4;
USE estoque_db;

CREATE TABLE IF NOT EXISTS produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    quantidade INT NOT NULL,
    estoque_minimo INT NOT NULL
);

INSERT INTO produtos (nome, preco, quantidade, estoque_minimo) VALUES
('Arroz 5kg', 28.90, 20, 5),
('Feijão 1kg', 8.50, 3, 5),
('Óleo de soja', 7.20, 15, 4);
