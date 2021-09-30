USE [Pagos]
GO
/****** Object:  StoredProcedure [dbo].[INS_DOCTOS_BANCOMER_SP]    Script Date: 08/05/2019 12:53:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  EXECUTE [INS_DOCTOS_BANAMEX_SP]                  
CREATE PROCEDURE [dbo].[INS_DOCTOS_BANAMEX_SP]
	
AS
BEGIN
--Esta comentado por si se desea hacer transaccional
--SET NOCOUNT ON;
--BEGIN TRY	

DECLARE @ipServidor   VARCHAR(100) = '';
DECLARE @cadIpServidor VARCHAR(100)= '';
DECLARE @nombreBase    VARCHAR(100)= '';
DECLARE @vlConcepto nvarchar(50)
DECLARE @referencia nvarchar(50)
DECLARE @tiporeferencia nvarchar(50)
DECLARE @refcompleta nvarchar(50)
DECLARE @importe numeric (18,6)
DECLARE @importetotal numeric (18,6)
DECLARE @importepagado numeric (18,6)
DECLARE @cuenta varchar(50)
DECLARE @agrupamiento int
DECLARE @idLote numeric (18,0)
DECLARE @idPersona numeric(18,0)
DECLARE @IdBmer int 
DECLARE @IdBmerDetalle int
DECLARE @iddocumento nvarchar(100)  
DECLARE @conscartera numeric(18,0)
DECLARE @idempresa int
DECLARE @numCuentaDestinosumada nvarchar(50)
DECLARE @numCuentaDestino nvarchar(50)
DECLARE @convenioCIE nvarchar(20)
DECLARE @dpa_iddoctopagado INT
DECLARE @varidpersona INT
DECLARE @sSQL2016 nvarchar(500) = '';
DECLARE @sSQL2017 nvarchar(500) = '';
DECLARE @sSQL2018 nvarchar(500) = '';
DECLARE @sSQL2019 nvarchar(500) = '';
DECLARE @sSQL2019Docs nvarchar(500) = '';
DECLARE @sSQL2018Docs nvarchar(500) = '';
DECLARE @sSQL2017Docs nvarchar(500) = '';
DECLARE @sSQL2016Docs nvarchar(500) = '';


DECLARE @auxBusqueda   INT = 1
DECLARE @VariableBusquedaTabla TABLE (ID INT IDENTITY(1,1), idBmerTemp int, concepto nvarchar(50), refAmpliadaTemp nvarchar(50), importeTemp numeric (18,6), fechaValorTemp datetime, noCuenta varchar(50), idempresa int)
				
INSERT INTO @VariableBusquedaTabla (idBmerTemp,concepto,refAmpliadaTemp,importeTemp,fechaValorTemp,noCuenta) 
		
SELECT 
	mov.idBanamex, mov.SLTransaccion, mov.AODescripcion, mov.SLMonto, mov.SLFechaTransaccion, mov.noCuenta
FROM referencias.dbo.Banamex as mov
INNER JOIN referencias.dbo.BancoCuenta as banco on mov.noCuenta = banco.numeroCuenta 
where ((mov.AODescripcion LIKE '%REF:%') AND ((mov.estatus = 1) AND (mov.SLCredito = 'D'))  AND (datediff (day,mov.SLFechaTransaccion,getdate()) <= 5))
UNION
SELECT 
	mov.idBanamex, mov.SLTransaccion, mov.AODescripcion, mov.SLMonto, mov.SLFechaTransaccion, mov.noCuenta
FROM referencias.dbo.Banamex as mov
INNER JOIN referencias.DBO.BancoCuenta as banco on mov.noCuenta = banco.numeroCuenta 
where (((mov.SLTransaccion LIKE 'SPEI%' AND mov.AODescripcion LIKE '1234567%') OR (mov.SLTransaccion LIKE 'TRASPASO%' ))  AND ((mov.estatus = 1) AND (mov.SLCredito = 'C'))  AND (datediff (day,mov.SLFechaTransaccion,getdate()) <= 5))


DECLARE @totalBusqueda INT = (SELECT count(1) from @VariableBusquedaTabla)


WHILE(@auxBusqueda <=  @totalBusqueda)
				BEGIN
				
				--separo la referncia.
				
					SELECT @vlConcepto= concepto, @refcompleta = refAmpliadaTemp, @importe = importeTemp, @cuenta = noCuenta, @IdBmer = idBmerTemp, @idempresa =  idempresa  FROM @VariableBusquedaTabla WHERE ID = @auxBusqueda
					
					set @referencia =
					CASE (LEFT(@vlConcepto,4))
						WHEN 'SPEI' THEN RTRIM(SUBSTRING(@refcompleta, 8, 20)) 
						WHEN 'TRAS' THEN RTRIM(SUBSTRING(@refcompleta, 1, LEN(@refcompleta)- 7))  
						ELSE  RTRIM(SUBSTRING(@refcompleta, 5, 21))
					END 
					
					SET @referencia = LTRIM(@referencia)
					SET @referencia = RTRIM(@referencia)
					SET @referencia = REPLACE(LTRIM(REPLACE(@referencia, '0', ' ')),' ', '0') 
					
					set @tiporeferencia =
					CASE (LEFT(@vlConcepto,4))
						WHEN 'SPEI' THEN 'SPEI' 
						WHEN 'TRAS' THEN 'TRAS' 
						ELSE  'CIE'
					END 
				
				--Buscamos que el agrupado este en el detalle
				
				--SELECT @referencia
				
				select @referencia,@cuenta,@importe
				
				if ( EXISTS(SELECT  DETALLE.pal_id_lote_pago, DETALLE.pad_idProveedor,sum (pad_saldo) as importeDetalle,DETALLE.pad_agrupamiento,DETALLE.pad_polReferencia,detalle.pad_bancoPagador, BANCO.numeroCuenta,DETALLE.pad_cuentaDestino
						FROM [Pagos].[dbo].[PAG_PROGRA_PAGOS_DETALLE] AS DETALLE
						INNER JOIN referencias.DBO.BancoCuenta AS BANCO ON DETALLE.pad_bancoPagador = BANCO.cuenta
						WHERE DETALLE.pad_polReferencia LIKE '%' + @referencia + '%' and BANCO.numeroCuenta = @cuenta AND DETALLE.pad_estatus_proceso <> 1
						group by DETALLE.pal_id_lote_pago, DETALLE.pad_idProveedor, DETALLE.pad_polReferencia, DETALLE.pad_agrupamiento, detalle.pad_bancoPagador, BANCO.numeroCuenta,DETALLE.pad_cuentaDestino
						HAVING sum (pad_saldo) = @importe ))
				
				begin
						--select @referencia,@cuenta,@importe
						
						SELECT @agrupamiento = DETALLE.pad_agrupamiento, @idLote = DETALLE.pal_id_lote_pago, @idPersona = DETALLE.pad_idProveedor, @numCuentaDestinosumada = DETALLE.pad_cuentaDestino
								FROM [Pagos].[dbo].[PAG_PROGRA_PAGOS_DETALLE] AS DETALLE
								INNER JOIN referencias.DBO.BancoCuenta AS BANCO ON DETALLE.pad_bancoPagador = BANCO.cuenta
								WHERE DETALLE.pad_polReferencia LIKE '%' + @referencia + '%' and BANCO.numeroCuenta = @cuenta AND DETALLE.pad_estatus_proceso <> 1
								group by DETALLE.pal_id_lote_pago, DETALLE.pad_idProveedor, DETALLE.pad_polReferencia, DETALLE.pad_agrupamiento, detalle.pad_bancoPagador, BANCO.numeroCuenta, DETALLE.pad_cuentaDestino
								HAVING sum (pad_saldo) = @importe 
								
						--borro el registro temporal que aparta el id del lote
						
						DELETE FROM   cuentasxpagar.dbo.cxp_doctospagados WHERE dpa_lote = @idLote and dpa_conscartera = 0
									
						--comienza la insercion en doctos pagados temporal
						
						SELECT @vlConcepto, @referencia, @refcompleta, @importe, @cuenta, @agrupamiento, @idlote, @idPersona, @numCuentaDestinosumada

						---	INICIO INGRESO DOCTOS PAGADOS
							
						DECLARE @VariableTabla TABLE (ID INT IDENTITY(1,1), conscartera numeric(18, 0), iddocumento nvarchar(25), idpersona numeric(18, 0), cuentapagadora nvarchar(50), cuentabeneficiario nvarchar(50), importetotal decimal(18, 5),importepagado decimal(18, 5), folioorden nvarchar(50) NULL, pagoaplicado decimal(18, 5), lote int, idempresa int, convenioCIE nvarchar(50),idBmerdetalle int)
												
						INSERT INTO @VariableTabla (conscartera,iddocumento,idpersona,cuentapagadora,cuentabeneficiario,folioorden,importepagado,pagoaplicado,lote,  idempresa, convenioCIE, idBmerdetalle) 
						
						SELECT   PAG_PROGRA_PAGOS_DETALLE.pbp_consCartera, PAG_PROGRA_PAGOS_DETALLE.pad_documento, PAG_PROGRA_PAGOS_DETALLE.pad_idProveedor, @cuenta, 
								 PAG_PROGRA_PAGOS_DETALLE.pad_cuentaDestino, PAG_PROGRA_PAGOS_DETALLE.pad_documento, PAG_PROGRA_PAGOS_DETALLE.pad_monto, PAG_PROGRA_PAGOS_DETALLE.pad_saldo,
								 @idLote,PAG_LOTE_PAGO.pal_id_empresa, PAG_PROGRA_PAGOS_DETALLE.pad_convenioCIE,@IdBmer
						FROM     PAG_PROGRA_PAGOS_DETALLE with (NOLOCK) INNER JOIN
								 PAG_LOTE_PAGO with (NOLOCK) ON PAG_PROGRA_PAGOS_DETALLE.pal_id_lote_pago = PAG_LOTE_PAGO.pal_id_lote_pago
						WHERE        (PAG_PROGRA_PAGOS_DETALLE.pal_id_lote_pago = @idLote) AND (PAG_PROGRA_PAGOS_DETALLE.pad_polReferencia LIKE '%' + @referencia + '%' and  PAG_PROGRA_PAGOS_DETALLE.pad_agrupamiento = @agrupamiento and PAG_PROGRA_PAGOS_DETALLE.pad_cuentaDestino = @numCuentaDestinosumada)  

				END 
	
				SET @auxBusqueda = @auxBusqueda + 1	
			END
			
			--Ya esta todo en doctos pagados temporal
			DECLARE @total INT = (SELECT COUNT(*) FROM @VariableTabla )
											
		    DECLARE @aux   INT = 1
					
						WHILE(@aux <=  @total)
								BEGIN
	
								SELECT @conscartera = conscartera, @iddocumento = iddocumento, @idempresa = idempresa, @numCuentaDestino = cuentabeneficiario,  @convenioCIE = convenioCIE, @varidpersona = idpersona, @IdBmerDetalle = idBmerdetalle
								FROM        @VariableTabla
								WHERE ID = @aux
								--Valido que no este ya el documento
								IF NOT EXISTS
									(
									SELECT 1
									FROM [cuentasxpagar].[dbo].[cxp_doctospagados] with (NOLOCK)
									WHERE dpa_lote = @idLote AND dpa_iddocumento = @iddocumento	AND dpa_conscartera = @conscartera
									)
								BEGIN
							-- Marco el detalle para no volver a procesarlo
							
							UPDATE pagos.dbo. PAG_PROGRA_PAGOS_DETALLE
							SET pad_estatus_proceso = 1 WHERE pal_id_lote_pago = @idlote and pad_documento = @iddocumento and pbp_consCartera = @conscartera and pad_idProveedor = @varidpersona
								
							-- Inserto en doctos pagados
								INSERT INTO [cuentasxpagar].[dbo].[cxp_doctospagados]
								(dpa_conscartera,dpa_iddocumento,dpa_idpersona,dpa_cuentapagadora,dpa_cuentabeneficiario,dpa_importepagado,dpa_folioorden,dpa_pagoaplicado,
								 dpa_lote, dpa_idempresa, dpa_conveniocie)
								SELECT conscartera, iddocumento, idpersona, cuentapagadora, cuentabeneficiario, pagoaplicado, folioorden, 0,lote,idempresa, convenioCIE FROM @VariableTabla WHERE ID = @aux
							
							--Actualizo el tipo de lote a por txt

							UPDATE pagos.dbo.PAG_LOTE_PAGO SET pal_id_tipoLotePago = 1 WHERE pal_id_lote_pago = @idlote	
								
							--ENCUENTRO EL SERVER 	
									SELECT @nombreBase = [nombre_base]        
										  ,@ipServidor = [ip_servidor]      
									FROM [Centralizacionv2].[dbo].[DIG_CAT_BASES_BPRO] with (NOLOCK)
									WHERE catemp_nombrecto = (SELECT [emp_nombrecto]  empNombreCto 
																FROM [ControlAplicaciones].[dbo].[cat_empresas] with (NOLOCK)
															WHERE [emp_idempresa] = @idempresa)
									  AND tipo = 2
									  
						DELETE FROM [cuentasxpagar].[dbo].[cxp_doctospagados] WHERE dpa_lote = @idLote AND dpa_pagoaplicado = 3 
							
<<<<<<< HEAD
<<<<<<< HEAD
						update   referencias.dbo.Banamex  SET estatus = 3	WHERE idBanamex = @IdBmerDetalle
=======
						update   referencias.dbo.Banamex  SET estatus = 3	WHERE idBmer = @IdBmerDetalle
>>>>>>> create [INS_DOCTOS_BANAMEX_SP]
=======
						update   referencias.dbo.Banamex  SET estatus = 3	WHERE idBanamex = @IdBmerDetalle
>>>>>>> Update [INS_DOCTOS_BANAMEX_SP].sql
							
						UPDATE [Centralizacionv2].[dbo].[DIG_EXPNODO_DOC] 
							SET Fecha_Creacion = GetDate()
							WHERE Doc_Id = 67  AND Folio_Operacion = @iddocumento
						--cambio requerido del 66 a peticion de Lau	17042019
						UPDATE [Centralizacionv2].[dbo].[DIG_EXPNODO_DOC] 
							SET Fecha_Creacion = GetDate()
							WHERE Doc_Id = 66  AND Folio_Operacion = @iddocumento
							--PRINT ('Se proceso con éxito' + @iddocumento)
								
							SELECT 'IDPERSONA', @varidpersona		  
									
									set @cadIpServidor =' [' + @ipServidor + '].'
							
										IF (@ipServidor ='192.168.20.29')
										BEGIN
										set @cadIpServidor =''
										END
										
						--	SELECT @idempresa, @cadIpServidor,@nombreBase
						
						--ERROR DE DIFERNECIA 0
						
									SET @sSQL2019 = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012019 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''FAC'''

									BEGIN TRY
										exec(@sSQL2019)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2018 '
									END CATCH		
																	
									SET @sSQL2018 = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012018 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''FAC'''

									BEGIN TRY
										exec(@sSQL2018)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2018 '
									END CATCH

									SET @sSQL2017 = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012017 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''FAC'''
									
									BEGIN TRY
										exec(@sSQL2017)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2017 '
									END CATCH


									SET @sSQL2016 = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012016 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''FAC'''

									BEGIN TRY
										exec(@sSQL2016)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2016 '
									END CATCH


									SET @sSQL2016Docs = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012016 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''DOCDOC'''

									BEGIN TRY
										exec(@sSQL2016Docs)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2016Docs '
									END CATCH

									SET @sSQL2017Docs = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012017 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''DOCDOC'''

									BEGIN TRY
										exec(@sSQL2017Docs)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @sSQL2017Docs '
									END CATCH

									SET @sSQL2018Docs = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012018 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''DOCDOC'''

									BEGIN TRY
										exec(@sSQL2018Docs)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @@sSQL2018Docs '
									END CATCH
									
									SET @sSQL2019Docs = 'UPDATE ' + @cadIpServidor + '[' + @nombreBase + '].DBO.CON_CAR012018 SET CCP_IMPORTEMON = CCP_ABONO WHERE CCP_IDPERSONA = ''' + CONVERT(varchar(50), @varidpersona) + ''' AND CCP_IMPORTEMON = 0 AND CCP_CARGO = 0 AND CCP_TIPODOCTO = ''DOCDOC'''

									BEGIN TRY
										exec(@sSQL2019Docs)
									END TRY
									BEGIN CATCH
										PRINT 'ERROR EN UPDATE: @@sSQL2019Docs '
									END CATCH
									
						--Encuentro su  iddocto pagado
							
								SELECT @dpa_iddoctopagado = dpa_iddoctopagado
									FROM [cuentasxpagar].[dbo].[cxp_doctospagados] with (NOLOCK)
									WHERE dpa_lote = @idLote AND dpa_iddocumento = @iddocumento	AND dpa_conscartera = @conscartera
									
							--Meto su relacion con idbmer
							
								DELETE FROM PAG_REL_DOCTOS_BANCOS  WHERE  idBanco_Registro = @IdBmerDetalle AND dpa_iddoctopagado = @dpa_iddoctopagado
									INSERT INTO PAG_REL_DOCTOS_BANCOS (dpa_iddoctopagado, idBanco_Registro, idBanco)
											VALUES(@dpa_iddoctopagado, @IdBmerDetalle, 1)
									
							
						END
								
						
						SET @aux = @aux + 1
							
						END
			
			SELECT * FROM @VariableBusquedaTabla		

--Esta comentado por si se desea hacer transaccional	
	
--END TRY
--BEGIN CATCH
--     PRINT ('Error: ' + ERROR_MESSAGE())
--	 DECLARE @Mensaje  nvarchar(max),
--	 @Componente nvarchar(50) = '[INS_DOCTOS_BANCOMER_SP]'
--	 SELECT @Mensaje = ERROR_MESSAGE()
--	 EXECUTE [INS_ERROR_SP] @Componente, @Mensaje; 
--	 SELECT 0 
--END CATCH	
	     
END
			
