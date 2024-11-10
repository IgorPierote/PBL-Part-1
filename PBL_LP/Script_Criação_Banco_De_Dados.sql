-- Criação do banco de dados
CREATE DATABASE PBL;
GO

-- Selecionando o banco de dados recém-criado
USE PBL;
GO

-- Criação da tabela Empresa
CREATE TABLE Empresa (
    CNPJ VARCHAR(14) PRIMARY KEY, -- Usando VARCHAR para armazenar o CNPJ
    NomeDaEmpresa VARCHAR(100) NOT NULL,
    Responsavel VARCHAR(100) NOT NULL
);
GO

-- Criação da tabela Sensor
CREATE TABLE Sensor (
    Codigo INT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Tipo VARCHAR(50) NOT NULL,
    ValorDoAluguel DECIMAL(10, 2) NOT NULL
);
GO

-- Criação da tabela Aluguel
CREATE TABLE Aluguel (
    CodigoDoAluguel INT PRIMARY KEY,
    CNPJ VARCHAR(14),
    CodigoSensor INT, -- Renomeando para evitar confusão
    Quantidade INT NOT NULL,
    DataDeInicio DATE NOT NULL,
    DataDeFinalizacao DATE NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CNPJ) REFERENCES Empresa(CNPJ),
    FOREIGN KEY (CodigoSensor) REFERENCES Sensor(Codigo) -- Referencia a coluna correta
);
GO

-- Criação da tabela Usuario
CREATE TABLE Usuario (
    CPF VARCHAR(11) PRIMARY KEY, -- Usando VARCHAR para armazenar o CPF
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Senha VARCHAR(50) NOT NULL,
    Telefone VARCHAR(15) NOT NULL,
    DataDeNascimento DATE NOT NULL,
    FotoCaminho VARCHAR(255)
);
GO
