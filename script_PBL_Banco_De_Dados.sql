CREATE DATABASE PBL;
GO

USE [PBL];
GO

-- Criação da tabela Aluguel com IDENTITY para auto-incremento
CREATE TABLE [dbo].[Aluguel](
    [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [idEmpresa] [int] NOT NULL,
    [Quantidade] [int] NOT NULL,
    [DataDeInicio] [date] NOT NULL,
    [DataDeFinalizacao] [date] NOT NULL,
    [Preco] [decimal](10, 2) NOT NULL,
    [CodigoSensor] [int] NOT NULL
);
GO

-- Criação da tabela Empresa com IDENTITY para auto-incremento
CREATE TABLE [dbo].[Empresa](
    [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [CNPJ] [varchar](20) UNIQUE NOT NULL,
    [NomeDaEmpresa] [varchar](100) NOT NULL,
    [Responsavel] [varchar](100) NOT NULL,
    [TelefoneContato] [varchar](15) NULL
);
GO

-- Criação da tabela Sensor com IDENTITY para auto-incremento
CREATE TABLE [dbo].[Sensor](
    [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [Nome] [varchar](100) NOT NULL,
    [ValorDoAluguel] [decimal](10, 2) NOT NULL,
    [Tipo] [int] NULL
);
GO

-- Criação da tabela TipoSensor com IDENTITY para auto-incremento
CREATE TABLE [dbo].[TipoSensor](
    [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [Descricao] [varchar](100) NOT NULL
);
GO

-- Criação da tabela Usuario com IDENTITY para auto-incremento
CREATE TABLE [dbo].[Usuario](
    [id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [CPF] [varchar](11) UNIQUE NOT NULL,
    [Nome] [varchar](100) NOT NULL,
    [Email] [varchar](100) NOT NULL,
    [Senha] [varchar](50) NOT NULL,
    [Telefone] [varchar](15) NOT NULL,
    [DataDeNascimento] [date] NOT NULL,
    [fotoCaminho] [varbinary](max)
);
GO

-- Adicionando Foreign Keys
ALTER TABLE [dbo].[Aluguel] 
ADD FOREIGN KEY([CodigoSensor]) REFERENCES [dbo].[Sensor]([id]);
GO

ALTER TABLE [dbo].[Aluguel] 
ADD CONSTRAINT [FK_CNPJ_EMPRESA] FOREIGN KEY([idEmpresa]) REFERENCES [dbo].[Empresa]([id]);
GO

ALTER TABLE [dbo].[Sensor] 
ADD CONSTRAINT [FK_Sensor_TipoSensor] FOREIGN KEY ([Tipo]) REFERENCES [dbo].[TipoSensor]([id]);
GO

-- Criação das stored procedures

CREATE PROCEDURE spDelete
    @id int,
    @tabela varchar(max)
AS
BEGIN
    DECLARE @sql varchar(max);
    SET @sql = 'DELETE FROM ' + @tabela + ' WHERE id = ' + CAST(@id AS varchar(max));
    EXEC(@sql);
END;
GO

CREATE PROCEDURE spConsulta
    @id int,
    @tabela varchar(max)
AS
BEGIN
    DECLARE @sql varchar(max);
    SET @sql = 'SELECT * FROM ' + @tabela + ' WHERE id = ' + CAST(@id AS varchar(max));
    EXEC(@sql);
END;
GO

CREATE PROCEDURE spConsultaCNPJ
    @cnpj NVARCHAR(18)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Empresa WHERE CNPJ = @cnpj;
END;
GO

CREATE PROCEDURE spListagem
    @tabela varchar(max),
    @ordem varchar(max)
AS
BEGIN
    EXEC('SELECT * FROM ' + @tabela + ' ORDER BY ' + @ordem);
END;
GO

-- Ajuste: Remover spProximoId se o ID é gerado automaticamente

CREATE PROCEDURE spInsert_Aluguel
    @idEmpresa INT,
    @Quantidade INT,
    @DataDeInicio DATE,
    @DataDeFinalizacao DATE,
    @Preco DECIMAL(10, 2),
    @CodigoSensor INT
AS
BEGIN
    INSERT INTO Aluguel (idEmpresa, Quantidade, DataDeInicio, DataDeFinalizacao, Preco, CodigoSensor)
    VALUES (@idEmpresa, @Quantidade, @DataDeInicio, @DataDeFinalizacao, @Preco, @CodigoSensor);
END;
GO

CREATE PROCEDURE spUpdate_Aluguel
    @id int,
    @idEmpresa INT,
    @Quantidade INT,
    @DataDeInicio DATE,
    @DataDeFinalizacao DATE,
    @Preco DECIMAL(10, 2),
    @CodigoSensor INT
AS
BEGIN
    UPDATE Aluguel
    SET idEmpresa = @idEmpresa,
        Quantidade = @Quantidade,
        DataDeInicio = @DataDeInicio,
        DataDeFinalizacao = @DataDeFinalizacao,
        Preco = @Preco,
        CodigoSensor = @CodigoSensor
    WHERE id = @id;
END;
GO

CREATE PROCEDURE spInsert_Empresa
    @CNPJ VARCHAR(20),
    @NomeDaEmpresa VARCHAR(100),
    @Responsavel VARCHAR(100),
    @TelefoneContato VARCHAR(15) = NULL
AS
BEGIN
    INSERT INTO Empresa (CNPJ, NomeDaEmpresa, Responsavel, TelefoneContato)
    VALUES (@CNPJ, @NomeDaEmpresa, @Responsavel, @TelefoneContato);
END;
GO

CREATE PROCEDURE spUpdate_Empresa
    @id INT,
    @CNPJ VARCHAR(20),
    @NomeDaEmpresa VARCHAR(100),
    @Responsavel VARCHAR(100),
    @TelefoneContato VARCHAR(15)
AS
BEGIN
    UPDATE Empresa
    SET CNPJ = @CNPJ,
        NomeDaEmpresa = @NomeDaEmpresa,
        Responsavel = @Responsavel,
        TelefoneContato = @TelefoneContato
    WHERE id = @id;
END;
GO

CREATE PROCEDURE spInsert_Sensor
    @Nome VARCHAR(100),
    @ValorDoAluguel DECIMAL(10, 2),
    @Tipo INT = NULL
AS
BEGIN
    INSERT INTO Sensor (Nome, ValorDoAluguel, Tipo)
    VALUES (@Nome, @ValorDoAluguel, @Tipo);
END;
GO

CREATE PROCEDURE spUpdate_Sensor
    @id int,
    @Nome VARCHAR(100),
    @ValorDoAluguel DECIMAL(10, 2),
    @Tipo INT = NULL
AS
BEGIN
    UPDATE Sensor
    SET Nome = @Nome,
        ValorDoAluguel = @ValorDoAluguel,
        Tipo = @Tipo
    WHERE id = @id;
END;
GO

CREATE PROCEDURE spInsert_TipoSensor
    @Descricao VARCHAR(100)
AS
BEGIN
    INSERT INTO TipoSensor (Descricao)
    VALUES (@Descricao);
END;
GO

CREATE PROCEDURE spUpdate_TipoSensor
    @id int,
    @Descricao VARCHAR(100)
AS
BEGIN
    UPDATE TipoSensor
    SET Descricao = @Descricao
    WHERE id = @id;
END;
GO

CREATE PROCEDURE spInsert_Usuario
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @Email VARCHAR(100),
    @Senha VARCHAR(50),
    @Telefone VARCHAR(15),
    @DataDeNascimento DATE,
    @fotoCaminho VARBINARY(max)
AS
BEGIN
    INSERT INTO Usuario (CPF, Nome, Email, Senha, Telefone, DataDeNascimento, fotoCaminho)
    VALUES (@CPF, @Nome, @Email, @Senha, @Telefone, @DataDeNascimento, @fotoCaminho);
END;
GO

CREATE PROCEDURE spUpdate_Usuario
    @id int,
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @Email VARCHAR(100),
    @Senha VARCHAR(50),
    @Telefone VARCHAR(15),
    @DataDeNascimento DATE,
    @fotoCaminho VARBINARY(max)
AS
BEGIN
    UPDATE Usuario
    SET CPF = @CPF,
        Nome = @Nome,
        Email = @Email,
        Senha = @Senha,
        Telefone = @Telefone,
        DataDeNascimento = @DataDeNascimento,
        fotoCaminho = @fotoCaminho
    WHERE id = @id;
END;
GO

CREATE PROCEDURE Sp_ConsultaSensor
AS
BEGIN
    SELECT 
        Sensor.id,
        Sensor.ValorDoAluguel,
        Sensor.Nome,
        TipoSensor.Descricao,
        Sensor.Tipo
    FROM 
        Sensor 
    INNER JOIN 
        TipoSensor ON Sensor.Tipo = TipoSensor.id;
END;
GO

-- Insere exemplos de dados
INSERT INTO TipoSensor(Descricao) VALUES ('Temperatura');
INSERT INTO TipoSensor(Descricao) VALUES ('Luz');
GO

-- Consultas para verificar os dados
SELECT * FROM Usuario;
SELECT * FROM Empresa;
SELECT * FROM TipoSensor;
GO
