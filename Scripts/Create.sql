--CREATE DATABASE Payment

-- tabela de teste remover
--use Payment
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].Pessoas(
	[idPessoa] [int] IDENTITY(1,1) NOT NULL,
	[nome] [nvarchar](256) NOT NULL,
	[cpf] [varchar](14) NOT NULL,
	[dataNascimento] [datetime] NOT NULL
CONSTRAINT [PK_Pessoas] PRIMARY KEY CLUSTERED 
(
	[IdPessoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


--Contas
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].Contas(
	[idConta] [int] IDENTITY(1,1) NOT NULL,
	[idPessoa] [int]  NOT NULL,
	[saldo] [money] NOT NULL,
	[limiteSaqueDiario] [money] NULL,
	[flagAtivo] [bit] NOT NULL DEFAULT 0,
	[tipoConta] [tinyint]  NOT NULL DEFAULT 0,
	[dataCriacao] [datetime] NOT NULL 
CONSTRAINT [PK_Contas] PRIMARY KEY CLUSTERED 
(
	[idConta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE Contas
ADD CONSTRAINT FK_Contas_Pessoa FOREIGN KEY (idPessoa) REFERENCES Pessoas (idPessoa)

--transacoes
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].Transacoes(
	[idTransacao] [int] IDENTITY(1,1) NOT NULL,
	[idConta] [int]  NOT NULL,
	[valor] [money] NOT NULL,
	[dataTransacao] [datetime] NOT NULL 
CONSTRAINT [PK_Transacoes] PRIMARY KEY CLUSTERED 
(
	[idTransacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE Contas
ADD CONSTRAINT FK_Transacoes_Contas FOREIGN KEY (idConta) REFERENCES Contas (idConta)
