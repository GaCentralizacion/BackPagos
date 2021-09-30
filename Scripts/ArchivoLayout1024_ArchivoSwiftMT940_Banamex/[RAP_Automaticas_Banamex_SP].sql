-- =============================================
-- Author:		Ing. Alejandro Grijalva Antonio
-- Create date: 2018-01-04
-- Description:	Referencias aplicadas de manera automatica
-- =============================================
CREATE PROCEDURE [dbo].[RAP_Automaticas_Banamex_SP] 
	@idEmpresa INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @FacturaQuery varchar(max)  = '';
	DECLARE @Base VARCHAR(MAX)			= '';
	DECLARE @idDeposito INT				= '';
	DECLARE @idBanco INT				= 1;
	
	DECLARE @countDep INT				= 0;
	DECLARE @rap_folio INT				= 0;

	-- Consulta de las bases de datos y sucursales activas
DECLARE @tableConf  TABLE(idEmpresa INT, idSucursal INT, servidor VARCHAR(250), baseConcentra VARCHAR(250), sqlCmd VARCHAR(8000), cargaDiaria VARCHAR(8000));
DECLARE @tableBancoFactura TABLE(consecutivo INT IDENTITY(1,1), idDeposito INT);
DECLARE @tableBancoCotizacion TABLE(consecutivo INT IDENTITY(1,1), idDeposito INT);
INSERT INTO @tableConf Execute [dbo].[SEL_ACTIVE_DATABASES_SP];
DECLARE @CountSuc INT = (SELECT COUNT(idSucursal) Sucursales FROM @tableConf WHERE idEmpresa = @idEmpresa);
PRINT( '========================= [ RAP_Automaticas_Banamex_SP ] =========================' );
PRINT( 'EMPRESA ' + CONVERT(VARCHAR(2), @idEmpresa) + ': SUCURSALES ' + CONVERT(VARCHAR(3), @CountSuc) );

DECLARE @Current INT = 0, @Max INT = 0;
DECLARE @CurrentBanco INT = 0, @MaxBanco INT = 0;
DECLARE @CurrentBancoCoti INT = 0, @MaxBancoCoti INT = 0;

SELECT @Current = MIN(idSucursal),@Max = MAX(idSucursal) FROM @tableConf WHERE idEmpresa = @idEmpresa;
WHILE(@Current <= @Max )
BEGIN
	-- FUNCIONAMIENTO PARA FACTURAS
	SET @FacturaQuery = '	SELECT
								B.idBanamex
							FROM [referencias].[dbo].[Referencia]				R 
							INNER JOIN [referencias].[dbo].[Banamex]			B	ON R.Referencia = b.SLReferencia
							INNER JOIN [referencias].[dbo].[DetalleReferencia]	DR	ON DR.idReferencia = R.idReferencia
							WHERE R.idEmpresa		= ' + CONVERT( VARCHAR(3), @idEmpresa ) + '
							AND DR.idSucursal		= ' + CONVERT( VARCHAR(3), @Current ) + '
							AND B.estatusRevision	= 1
							AND B.SLCredito			= ''C''
							AND DR.idTipoDocumento	= 1;';

	INSERT INTO @tableBancoFactura
	EXECUTE( @FacturaQuery );

	SET @countDep = (SELECT COUNT(consecutivo) FROM @tableBancoFactura);
	PRINT( '    Sucursal ' + CONVERT( VARCHAR(3), @Current ) + ': '+ CONVERT(VARCHAR(5), @countDep) +' Referencias');
	
	-- VALIDACION PARA NO ESPECIFICAR EL SERVER DE UN PUNTO AL MISMO
	DECLARE @ipLocal VARCHAR(15) = (
		SELECT	dec.local_net_address
		FROM	sys.dm_exec_connections AS dec
		WHERE	dec.session_id = @@SPID
	);

	-- DECLARE @Base VARCHAR(300) = ''
	IF(  @ipLocal = (SELECT ip_servidor FROM Centralizacionv2.dbo.DIG_CAT_BASES_BPRO WHERE emp_idempresa = @idEmpresa AND tipo = 2)  )
		BEGIN
			SET @Base = (SELECT '[' + nombre_base + '].[dbo]' FROM Centralizacionv2.dbo.DIG_CAT_BASES_BPRO WHERE emp_idempresa = @idEmpresa AND tipo = 2);
		END
	ELSE
		BEGIN
			SET @Base = (SELECT '[' + ip_servidor + '].[' + nombre_base + '].[dbo]' FROM Centralizacionv2.dbo.DIG_CAT_BASES_BPRO WHERE emp_idempresa = @idEmpresa AND tipo = 2);
		END
	-- / VALIDACION PARA NO ESPECIFICAR EL SERVER DE UN PUNTO AL MISMO
	
	
	
	
	--select * from @tableBancoFactura
	-- FUNCIONAMIENTO PARA FACTURAS

/*********************************************************************************************************************************/

	-- FUNCIONAMIENTO PARA COTIZACIONES
	SET @FacturaQuery = '	SELECT
								B.idBanamex
							FROM [referencias].[dbo].[Referencia]				R 
							INNER JOIN [referencias].[dbo].[Banamex]			B	ON R.Referencia = b.SLReferencia
							INNER JOIN [referencias].[dbo].[DetalleReferencia]	DR	ON DR.idReferencia = R.idReferencia
							WHERE R.idEmpresa		= ' + CONVERT( VARCHAR(3), @idEmpresa ) + '
							AND DR.idSucursal		= ' + CONVERT( VARCHAR(3), @Current ) + '
							AND B.estatusRevision	= 1
							AND B.SLCredito			= ''C''
							AND DR.idTipoDocumento	= 2;';

	INSERT INTO @tableBancoCotizacion
	EXECUTE( @FacturaQuery );
	

	SELECT @CurrentBancoCoti = MIN(consecutivo),@MaxBancoCoti = MAX(consecutivo) FROM @tableBancoCotizacion;
	WHILE(@CurrentBancoCoti <= @MaxBancoCoti )
		BEGIN
			-- Funcionamiento de meter en cxc_refantypag
			-- Funcionamiento de meter en cxc_refantypag
			BEGIN TRY
				SET @idDeposito			  = ( SELECT TOP 1 idDeposito FROM @tableBancoCotizacion WHERE consecutivo = @CurrentBancoCoti );
				SELECT @Base,@idDeposito
				--SELECT @FacturaQuery	  = [dbo].[fnReferenciaBancomerCotizacion]( @Base, @idDeposito );
				--EXECUTE( @FacturaQuery ); 
						
				--IF( @@ROWCOUNT > 0 )
				--	BEGIN
				--		SET @rap_folio = @@IDENTITY;
				--		INSERT INTO [referencias].[dbo].[RAPDeposito](idEmpresa, idSucursal, rap_folio,idBanco,idDeposito,idOrigenReferencia, fecha) VALUES (@idEmpresa, @Current, @rap_folio, @idBanco, @idDeposito,'Procesos Automáticos | Cotización', GETDATE());
				--		UPDATE [referencias].[dbo].[Bancomer] SET estatusRevision = 2 WHERE idBmer = @idDeposito;
				--	END
						
				--PRINT( '        SUCCESS: Depósito ['+ CONVERT(VARCHAR(10), @idDeposito) +'] COTIZACIÓN: Inserción exitosa con rap_folio ' + CONVERT(VARCHAR(10), @rap_folio) );
			END TRY
			BEGIN CATCH	
				--INSERT INTO LogRAP(log_error, log_origen, log_fecha, idBanco, idDeposito, idEmpresa, idSucursal) VALUES( ERROR_MESSAGE(), 'Procesos Automáticos | Cotización', GETDATE(), @idBanco, @idDeposito, @idEmpresa, @Current );
				--PRINT( '        ERROR: Depósito ['+ CONVERT(VARCHAR(10), @idDeposito) +'] FACTURA: ' + ERROR_MESSAGE() );
			END CATCH
			-- Funcionamiento de meter en cxc_refantypag
			-- Funcionamiento de meter en cxc_refantypag
		SET	@CurrentBancoCoti = @CurrentBancoCoti + 1;
		END	
			
			
	PRINT('');
	-- FUNCIONAMIENTO PARA COTIZACIONES


	DELETE FROM @tableBancoCotizacion;
	DELETE FROM @tableBancoFactura;
	SET	@Current = @Current + 1;
		END 
END