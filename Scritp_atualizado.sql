CREATE DATABASE PBL;

USE [PBL]
GO
/****** Object:  Table [dbo].[Aluguel]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Aluguel](
	[id] [int] NOT NULL,
	[idEmpresa] [int] NOT NULL,
	[Quantidade] [int] NOT NULL,
	[DataDeInicio] [date] NOT NULL,
	[DataDeFinalizacao] [date] NOT NULL,
	[Preco] [decimal](10, 2) NOT NULL,
	[CodigoSensor] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
    [id] [int] NOT NULL,
	[CNPJ] [varchar](20) UNIQUE NOT NULL,
	[NomeDaEmpresa] [varchar](100) NOT NULL,
	[Responsavel] [varchar](100) NOT NULL,
	[TelefoneContato] [varchar](15) NULL,
 CONSTRAINT [PK_EMPRESA] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sensor]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sensor](
	[id] [int]  NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[ValorDoAluguel] [decimal](10, 2) NOT NULL,
	[Tipo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoSensor]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoSensor](
	[id] [int] NOT NULL,
	[Descricao] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create TABLE [dbo].[Usuario](
    [id] [int] NOT NULL,
	[CPF] [varchar](11) UNIQUE NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Senha] [varchar](50) NOT NULL,
	[Telefone] [varchar](15) NOT NULL,
	[DataDeNascimento] [date] NOT NULL,
        [fotoCaminho] [varbinary] (max)
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aluguel]  WITH CHECK ADD FOREIGN KEY([CodigoSensor])
REFERENCES [dbo].[Sensor] ([id])
GO
ALTER TABLE [dbo].[Aluguel]  WITH CHECK ADD  CONSTRAINT [FK_CNPJ_EMPRESA] FOREIGN KEY([IdEmpresa])
REFERENCES [dbo].[Empresa] ([id])
GO
ALTER TABLE [dbo].[Aluguel] CHECK CONSTRAINT [FK_CNPJ_EMPRESA]
GO

ALTER TABLE [dbo].[Sensor] 
WITH CHECK ADD CONSTRAINT [FK_Sensor_TipoSensor] 
FOREIGN KEY ([Tipo])
REFERENCES [dbo].[TipoSensor] ([id]);
GO

ALTER TABLE [dbo].[Sensor] 
CHECK CONSTRAINT [FK_Sensor_TipoSensor];
GO

--Criação das stored procedures

create procedure spDelete
(
@id int ,
@tabela varchar(max)
)
as
begin
declare @sql varchar(max);
set @sql = ' delete ' + @tabela +
' where id = ' + cast(@id as varchar(max))
exec(@sql)
end
GO

create procedure spConsulta
(
@id int ,
@tabela varchar(max)
)
as
begin
declare @sql varchar(max);
set @sql = 'select * from ' + @tabela +
' where id = ' + cast(@id as varchar(max))
exec(@sql)
end
GO

Create PROCEDURE spConsultaCNPJ
    @cnpj NVARCHAR(18) -- Ajuste o tipo de dado ao utilizado na tabela
AS
BEGIN
    SET NOCOUNT ON;

    -- Consulta na tabela Empresa pelo CNPJ informado
    SELECT * 
    FROM Empresa
    WHERE CNPJ = @cnpj;
END;
GO


create procedure spListagem
(
@tabela varchar(max),
@ordem varchar(max))
as
begin
exec('select * from ' + @tabela +
' order by ' + @ordem)
end
GO

create procedure spProximoId
(@tabela varchar(max))
as
begin
exec('select isnull(max(id) +1, 1) as MAIOR from '
+@tabela)
end
GO

create PROCEDURE spInsert_Aluguel
	@id int,
    @idEmpresa INT,
    @Quantidade INT,
    @DataDeInicio DATE,
    @DataDeFinalizacao DATE,
    @Preco DECIMAL(10, 2),
    @CodigoSensor INT
AS
BEGIN
    INSERT INTO Aluguel (id, idEmpresa, Quantidade, DataDeInicio, DataDeFinalizacao, Preco, CodigoSensor)
    VALUES (@id, @idEmpresa, @Quantidade, @DataDeInicio, @DataDeFinalizacao, @Preco, @CodigoSensor);
END;


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

create PROCEDURE spInsert_Empresa
	@id int,
    @CNPJ VARCHAR(20),
    @NomeDaEmpresa VARCHAR(100),
    @Responsavel VARCHAR(100),
    @TelefoneContato VARCHAR(15) = NULL
AS
BEGIN
    INSERT INTO Empresa (id, CNPJ, NomeDaEmpresa, Responsavel, TelefoneContato)
    VALUES (@id, @CNPJ, @NomeDaEmpresa, @Responsavel, @TelefoneContato);
END;


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

create PROCEDURE spInsert_Sensor
	@id int,
    @Nome VARCHAR(100),
    @ValorDoAluguel DECIMAL(10, 2),
    @Tipo INT = NULL
AS
BEGIN
    INSERT INTO Sensor (id,Nome, ValorDoAluguel, Tipo)
    VALUES (@id, @Nome, @ValorDoAluguel, @Tipo);
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

create PROCEDURE spInsert_TipoSensor
	@id int,
    @Descricao VARCHAR(100)
AS
BEGIN
    INSERT INTO TipoSensor (Descricao)
    VALUES (@Descricao);
END;
GO

CREATE PROCEDURE spUpdate_TipoSensor
    @Descricao VARCHAR(100),
	@id int
AS
BEGIN
    UPDATE TipoSensor
    SET Descricao = @Descricao
    WHERE id = @id;
END;
GO

create procedure spInsert_Usuario
	@id int,
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @Email VARCHAR(100),
    @Senha VARCHAR(50),
    @Telefone VARCHAR(15),
    @DataDeNascimento DATE,
    @fotoCaminho VARBINARY (max)
	AS
BEGIN
    INSERT INTO Usuario (id, CPF, Nome, Email, Senha, Telefone, DataDeNascimento, fotoCaminho)
    VALUES (@id,@CPF, @Nome, @Email, @Senha, @Telefone, @DataDeNascimento, @fotoCaminho);
END;

create procedure spUpdate_Usuario
    @id int,
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @Email VARCHAR(100),
    @Senha VARCHAR(50),
    @Telefone VARCHAR(15),
    @DataDeNascimento DATE,
    @fotoCaminho VARBINARY (max)
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

create PROCEDURE sp_consulta_Aluguel
AS
BEGIN
    SELECT 
        * 
    FROM 
        Aluguel
    INNER JOIN 
        Empresa 
    ON 
        Aluguel.idEmpresa = Empresa.id;
END;


insert into TipoSensor(id, Descricao) values(1,'Temperatura')
insert into TipoSensor(id, Descricao) values(2,'Luz')