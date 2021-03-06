USE [referencias]
GO
/****** Object:  UserDefinedFunction [dbo].[fnReferenciaBanamexCotizacion]    Script Date: 06/05/2019 04:29:32 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[fnReferenciaBanamexCotizacion](

	@CurrentBase varchar(50),	
	@idDeposito NUMERIC(18)		
)  
RETURNS varchar(max)   
AS    
BEGIN  
		DECLARE @ISUN VARCHAR(MAX) = 
		'ISNULL' + char(13) + 
		'		(' + char(13) + 
		'			(' + char(13) + 
		'				SELECT top 1 ucu_foliocotizacion COLLATE Modern_Spanish_CS_AS ' + char(13) + 
		'				FROM cuentasporcobrar.dbo.uni_cotizacionuniversal ' + char(13) + 
		'				WHERE ucu_idempresa = R.idEmpresa ' + char(13) + 
		'				AND ucu_idsucursal = DR.idSucursal ' + char(13) + 
		'				AND ucu_iddepartamento = DR.idDepartamento ' + char(13) + 
		'				AND ucu_idcliente = DR.idCliente ' + char(13) + 
		'				--AND ucu_idcotizacion  = convert(int,substring (right(rtrim(concepto),8) ,0,8 ))' + char(13) + 
		'			),' + char(13) + 
		'			(' + char(13) + 
		'/*				SELECT UPE_IDPEDI ' + char(13) + 
		'				FROM '+@CurrentBase+'.UNI_PEDIDO ' + char(13) + 
		'				WHERE UPE_IDCLIENTE = DR.idCliente ' + char(13) + 
		'				AND UPE_IDPEDI  = DR.documento*/' + char(13) + 
		'				DR.documento ' + char(13) + 
		'			)' + char(13) + 
		'		)' + char(13) + 
		'' 


		DECLARE @ISUS VARCHAR(MAX) = 
		'ISNULL' + char(13) + 
		'		(' + char(13) + 
		'			(' + char(13) + 
		'				SELECT ucu_foliocotizacion COLLATE Modern_Spanish_CS_AS ' + char(13) + 
		'				FROM cuentasporcobrar.dbo.uni_cotizacionuniversal ' + char(13) + 
		'				WHERE ucu_idempresa = R.idEmpresa ' + char(13) + 
		'				AND ucu_idsucursal = DR.idSucursal ' + char(13) + 
		'				AND ucu_iddepartamento = DR.idDepartamento ' + char(13) + 
		'				AND ucu_idcliente = DR.idCliente ' + char(13) + 
		'				AND ucu_idcotizacion   = DR.documento' + char(13) + 
		'			),' + char(13) + 
		'			(' + char(13) + 
		'/*				SELECT PMS_NUMPEDIDO ' + char(13) + 
		'				FROM '+@CurrentBase+'.USN_PEDIDO ' + char(13) + 
		'				WHERE PMS_IDPERSONA = DR.idCliente ' + char(13) + 
		'				AND PMS_NUMPEDIDO  = DR.documento */' + char(13) + 
		'				DR.documento ' + char(13) + 
		'			)' + char(13) + 
		'		)' + char(13) + 
		'' 


		DECLARE @ISRE VARCHAR(MAX) = 
		'		ISNULL('+ char(13) + 
		'			/*(SELECT PMM_NUMERO + '+char(39)+'-'+char(39)+' + PMM_IDALMA + '+char(39)+'-'+char(39)+' + PMM_COTPED' + char(13) + 
		'			FROM '+@CurrentBase+'.PAR_PEDMOST ' + char(13) + 
		'			WHERE PMM_IDCLIENTE = DR.idCliente' + char(13) + 
		'			AND PMM_COTPED = '+char(39)+'COTIZACION'+char(39)+' ' + char(13) + 
		'			AND PMM_IDALMA = '+char(39)+'GEN'+char(39)+'    ' + char(13) + 
		'			AND PMM_NUMERO   = DR.documento)*/ DR.documento,' + char(13) + 
		'			'+char(39)+' ' + char(39) + ')'
		

		DECLARE @QueryA VARCHAR(MAX) = 
		'	(select ' + char(13) + 
		'		CASE  [dep_nombrecto]  ' + char(13) + 
		'		WHEN '+char(39)+'UN'+char(39)+' THEN '+ @ISUN  + char(13) + 
		'		WHEN '+char(39)+'US'+char(39)+' THEN '+ @ISUS  + char(13) + 
		'		WHEN '+char(39)+'RE'+char(39)+' THEN '+ @ISRE  + char(13) + 
		'		ELSE '+char(39)+''+char(39)+'	' + char(13) + 
		'		END referencia	' + char(13) + 
		'	from [ControlAplicaciones].[dbo].[cat_departamentos] ' + char(13) + 
		'	where  dep_iddepartamento = DR.idDepartamento)' + char(13) + 
		'' 
		
		DECLARE @DepartamentosNissan VARCHAR(MAX) = 
		'CASE R.idEmpresa 
								WHEN 2 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 4 THEN 16
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 3 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 5 THEN 21
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 4 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 6 THEN 26
																			WHEN 7 THEN 31
																			WHEN 8 THEN 36
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 5 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 9 THEN 41
																			WHEN 10 THEN 46
																			WHEN 11 THEN 51
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 6 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 12 THEN 56
																			WHEN 13 THEN 61
																			WHEN 14 THEN 66
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 8 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 17 THEN 81
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 9 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 18 THEN 86
																			WHEN 19 THEN 91
																			WHEN 20 THEN 96
																			WHEN 21 THEN 105
																		END
																	)
													WHEN ''US'' THEN (
														CASE DR.idSucursal
															WHEN 18 THEN 86
															WHEN 19 THEN 91
															WHEN 20 THEN 96
															WHEN 21 THEN 105
														END
													)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								WHEN 10 THEN (
												CASE (SELECT dep_nombrecto from [ControlAplicaciones].[dbo].[cat_departamentos] where  dep_iddepartamento = DR.idDepartamento)
													WHEN ''OT'' THEN (
																		CASE DR.idSucursal
																			WHEN 22 THEN 110
																			WHEN 23 THEN 115
																		END
																	)
													ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
												END
											)
								ELSE CONVERT(VARCHAR(18),DR.idDepartamento)
							END';
							
							
		DECLARE @Consecutivo VARCHAR(MAX) = '( SELECT
													CASE WHEN MAX(RAP_NumDeposito) IS NULL
														THEN 1
													ELSE (MAX(RAP_NumDeposito) + 1 ) END
												FROM GA_Corporativa.DBO.cxc_refantypag
												WHERE rap_referenciabancaria = R.referencia)';

		DECLARE @queryText VARCHAR(MAX) = 
		
		--'--INSERT INTO GA_Corporativa.dbo.cxc_refantypag( rap_idempresa, rap_idsucursal, rap_iddepartamento, rap_idpersona, rap_cobrador, rap_moneda, rap_tipocambio, rap_referencia, rap_iddocto, rap_cotped, rap_consecutivo, rap_importe, rap_formapago, rap_numctabanc, rap_fecha, rap_idusuari, rap_idstatus, rap_banco, rap_referenciabancaria, rap_anno, RAP_AplicaPago, RAP_NumDeposito )'+ char(13) + -- DESCOMENTAR LINEA
		'SELECT' + char(13) + 
		'	rap_idempresa = R.idEmpresa,' + char(13) + 
		'	rap_idsucursal = DR.idSucursal,' + char(13) + 		
		'	rap_iddepartamento = ' + @DepartamentosNissan + ',' + char(13) + 
		'	rap_idpersona = DR.idCliente,' + char(13) + 
		'	rap_cobrador = '+char(39)+'MMK'+char(39)+',' + char(13) + 
		'	rap_moneda = '+char(39)+'PE'+char(39)+',' + char(13) + 
		'	rap_tipocambio = 1,' + char(13) + 
		'	rap_referencia = ' + @QueryA + ',' + char(13) + 
		'	rap_iddocto = '+char(39)+''+char(39)+',' + char(13) + 
		'	rap_cotped = '+char(39)+'COTIZACION UNIVERSAL'+char(39)+',' + char(13) + 
		'	rap_consecutivo = '+char(39)+'0'+char(39)+',' + char(13) + 
		'	rap_importe = convert(numeric(18,2),b.SLMonto),' + char(13) + 
		'	rap_formapago = ''0'',' + char(13) + 
		'	rap_numctabanc = noCuenta,' + char(13) + 
		'	rap_fecha = GETDATE(),' + char(13) + 
		'	rap_idusuari = (SELECT usu_idusuario FROM ControlAplicaciones..cat_usuarios WHERE usu_nombreusu = '+char(39)+'GMI'+char(39)+'),' + char(13) + 
		'	rap_idstatus = '+char(39)+'1'+char(39)+',' + char(13) + 		
		'	rap_banco = (SELECT idBancoBPRO FROM referencias.dbo.BancoCuenta WHERE idEmpresa = R.idEmpresa AND idBanco = B.idBanco AND numeroCuenta = B.noCuenta),' + char(13) + 
		'	rap_referenciabancaria = R.referencia,' + char(13) + 
		'	rap_anno = year(getdate()), ' + char(13) + 
		'	RAP_AplicaPago = convert(numeric(18,2),b.SLMonto),' + char(13) + 
		'	RAP_NumDeposito = ' + @Consecutivo + ' ' + char(13) + 
		'FROM Referencia R ' + char(13) + 
		'INNER JOIN Banamex B ON R.Referencia = b.SLReferencia' + char(13) + 
		'INNER JOIN Centralizacionv2..DIG_CAT_BASES_BPRO BP ON R.idEmpresa = BP.emp_idempresa ' + char(13) + 
		'INNER JOIN DetalleReferencia DR ON  DR.idReferencia = R.idReferencia AND DR.idSucursal = BP.suc_idsucursal' + char(13) + 
		'WHERE B.estatusRevision = 1 ' + char(13) + 
		'	   AND B.SLCredito = ''C'' ' + char(13) + 
		'	   AND DR.idTipoDocumento = 2' + char(13) + 
		'	   AND B.IdBanco = 2' + char(13) 
    --print @queryText
	RETURN @queryText
END;  