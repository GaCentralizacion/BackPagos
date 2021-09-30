USE [Pagos]
GO

/****** Object:  StoredProcedure [dbo].[SEL_PROG_FIND_BANK_TXT_SP]    Script Date: 14/05/2019 05:13:51 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================  
-- Author: 
-- Create date: 18/05/2016  
-- Description: Procedimeinto que trae la información agrupada por proveedor y referencia  
-- Modified date: 
-- Description: Formar el archivo de pago para banamex
-- ==========================================================================================  
--EXECUTE [SEL_PROG_FIND_BANK_TXT_SP] 8369  
CREATE PROCEDURE [dbo].[SEL_PROG_FIND_BANK_TXT_SP]   
       @idPadre      numeric(18,0) = 0  
AS  
BEGIN  
 SET NOCOUNT ON;  
  BEGIN TRY  
	SELECT  DISTINCT TOP 1 pad_bancoPagador FROM  [dbo].[PAG_PROGRA_PAGOS_DETALLE]  WHERE [pal_id_lote_pago] = @idPadre
  END TRY      
  BEGIN CATCH  
  PRINT ('Error: ' + ERROR_MESSAGE())  
  DECLARE @Mensaje  nvarchar(max),  
  @Componente nvarchar(50) = '[SEL_PROG_FIND_BANK_TXT_SP]'  
  SELECT @Mensaje = ERROR_MESSAGE()  
  EXECUTE [INS_ERROR_SP] @Componente, @Mensaje;   
  --SELECT 0 --Encontro error  
  SELECT 'ERROR EN LA CONSULTA'  
  END CATCH  
END  
GO
