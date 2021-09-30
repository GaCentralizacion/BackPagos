USE [referencias]
GO

/****** Object:  Table [dbo].[Banamex]    Script Date: 07/05/2019 04:10:11 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Banamex](
	[idBanamex] [int] IDENTITY(1,1) NOT NULL,
	[estatus] [int] NULL,
	[idBanco] [int] NULL,
	[FechaRegistro] [datetime] NULL,
	[archivo] [varchar](500) NULL,
	[noCuenta] [varchar](500) NULL,
	[Consecutivo] [bigint] NULL,
	[OBCredito] [varchar](500) NULL,
	[OBFechaApertura] [varchar](500) NULL,
	[OBMoneda] [varchar](500) NULL,
	[OBMontoApertura] [numeric](18, 4) NULL,
	[SLFechaTransaccion] [date] NULL,
	[SLFechaEntrada] [varchar](500) NULL,
	[SLCredito] [varchar](500) NULL,
	[SLMonedaTC] [varchar](500) NULL,
	[SLMonto] [numeric](18, 4) NULL,
	[SLRazon] [varchar](500) NULL,
	[SLTipoReferencia] [varchar](500) NULL,
	[SLReferencia] [varchar](500) NULL,
	[SLCodigoTransaccion] [varchar](500) NULL,
	[SLTransaccion] [varchar](500) NULL,
	[AOTipoProductoID] [varchar](500) NULL,
	[AOTipoProducto] [varchar](500) NULL,
	[AODescripcion] [varchar](500) NULL,
	[CBCredito] [varchar](500) NULL,
	[CBFechaReserva] [date] NULL,
	[CBMoneda] [varchar](500) NULL,
	[CBMonto] [numeric](18, 4) NULL,
	[CABCredito] [varchar](500) NULL,
	[CABFechaReserva] [date] NULL,
	[CABMoneda] [varchar](500) NULL,
	[CABMonto] [numeric](18, 4) NULL,
	[estatusRevision] [int] NULL
) ON [PRIMARY]
GO


