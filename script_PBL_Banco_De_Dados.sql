USE [PBL]
GO
/****** Object:  Table [dbo].[Aluguel]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aluguel](
	[CodigoDoAluguel] [int] NOT NULL,
	[CNPJ] [varchar](20) NOT NULL,
	[Quantidade] [int] NOT NULL,
	[DataDeInicio] [date] NOT NULL,
	[DataDeFinalizacao] [date] NOT NULL,
	[Preco] [decimal](10, 2) NOT NULL,
	[CodigoSensor] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CodigoDoAluguel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[CNPJ] [varchar](20) NOT NULL,
	[NomeDaEmpresa] [varchar](100) NOT NULL,
	[Responsavel] [varchar](100) NOT NULL,
	[TelefoneContato] [varchar](15) NULL,
 CONSTRAINT [PK_EMPRESA] PRIMARY KEY CLUSTERED 
(
	[CNPJ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sensor]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sensor](
	[Codigo] [int] NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[ValorDoAluguel] [decimal](10, 2) NOT NULL,
	[Tipo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoSensor]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoSensor](
	[Codigo] [int] NOT NULL,
	[Descricao] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 27/09/2024 14:01:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[CPF] [varchar](11) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Senha] [varchar](50) NOT NULL,
	[Telefone] [varchar](15) NOT NULL,
	[DataDeNascimento] [date] NOT NULL,
        [fotoCaminho] [varbinary]
PRIMARY KEY CLUSTERED 
(
	[CPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Aluguel]  WITH CHECK ADD FOREIGN KEY([CodigoSensor])
REFERENCES [dbo].[Sensor] ([Codigo])
GO
ALTER TABLE [dbo].[Aluguel]  WITH CHECK ADD  CONSTRAINT [FK_CNPJ_EMPRESA] FOREIGN KEY([CNPJ])
REFERENCES [dbo].[Empresa] ([CNPJ])
GO
ALTER TABLE [dbo].[Aluguel] CHECK CONSTRAINT [FK_CNPJ_EMPRESA]
GO
