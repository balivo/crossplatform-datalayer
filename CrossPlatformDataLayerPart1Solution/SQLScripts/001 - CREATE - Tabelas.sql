IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_UnidadeFederacao_Pais]') AND parent_object_id = OBJECT_ID(N'[UnidadeFederacao]'))
ALTER TABLE [UnidadeFederacao] DROP CONSTRAINT [FK_UnidadeFederacao_Pais]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Cidade_UnidadeFederacao]') AND parent_object_id = OBJECT_ID(N'[Cidade]'))
ALTER TABLE [Cidade] DROP CONSTRAINT [FK_Cidade_UnidadeFederacao]
GO

/****** Object:  Table [UnidadeFederacao]    Script Date: 22/07/2015 13:04:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UnidadeFederacao]') AND type in (N'U'))
DROP TABLE [UnidadeFederacao]
GO

/****** Object:  Table [Pais]    Script Date: 22/07/2015 13:04:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Pais]') AND type in (N'U'))
DROP TABLE [Pais]
GO

/****** Object:  Table [Cidade]    Script Date: 22/07/2015 13:04:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Cidade]') AND type in (N'U'))
DROP TABLE [Cidade]
GO

/****** Object:  Table [Cidade]    Script Date: 22/07/2015 13:04:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Cidade]') AND type in (N'U'))
BEGIN
CREATE TABLE [Cidade](
	[CidadeId] [int] NOT NULL,
	[UnidadeFederacaoId] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[DDD] [varchar](3) NULL,
	[CEPInicial] [char](8) NULL,
	[CEPFinal] [char](8) NULL,
	[Classe] [varchar](5) NULL,
 CONSTRAINT [PK_Cidade] PRIMARY KEY CLUSTERED 
(
	[CidadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [Pais]    Script Date: 22/07/2015 13:04:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Pais]') AND type in (N'U'))
BEGIN
CREATE TABLE [Pais](
	[PaisId] [int] NOT NULL,
	[ISO2] [char](2) NULL,
	[ISO3] [char](3) NULL,
	[NumCode] [int] NULL,
	[Nome] [varchar](255) NOT NULL,
	[DDI] [varchar](5) NULL,
 CONSTRAINT [PK_Pais] PRIMARY KEY CLUSTERED 
(
	[PaisId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [UnidadeFederacao]    Script Date: 22/07/2015 13:04:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UnidadeFederacao]') AND type in (N'U'))
BEGIN
CREATE TABLE [UnidadeFederacao](
	[UnidadeFederacaoId] [int] NOT NULL,
	[PaisId] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Sigla] [char](2) NOT NULL,
 CONSTRAINT [PK_UnidadeFederacao] PRIMARY KEY CLUSTERED 
(
	[UnidadeFederacaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Cidade_UnidadeFederacao]') AND parent_object_id = OBJECT_ID(N'[Cidade]'))
ALTER TABLE [Cidade]  WITH CHECK ADD  CONSTRAINT [FK_Cidade_UnidadeFederacao] FOREIGN KEY([UnidadeFederacaoId])
REFERENCES [UnidadeFederacao] ([UnidadeFederacaoId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Cidade_UnidadeFederacao]') AND parent_object_id = OBJECT_ID(N'[Cidade]'))
ALTER TABLE [Cidade] CHECK CONSTRAINT [FK_Cidade_UnidadeFederacao]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_UnidadeFederacao_Pais]') AND parent_object_id = OBJECT_ID(N'[UnidadeFederacao]'))
ALTER TABLE [UnidadeFederacao]  WITH CHECK ADD  CONSTRAINT [FK_UnidadeFederacao_Pais] FOREIGN KEY([PaisId])
REFERENCES [Pais] ([PaisId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_UnidadeFederacao_Pais]') AND parent_object_id = OBJECT_ID(N'[UnidadeFederacao]'))
ALTER TABLE [UnidadeFederacao] CHECK CONSTRAINT [FK_UnidadeFederacao_Pais]
GO
