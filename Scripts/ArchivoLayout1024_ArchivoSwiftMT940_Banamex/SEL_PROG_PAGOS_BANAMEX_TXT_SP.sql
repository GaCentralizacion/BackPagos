USE [Pagos]
GO

/****** Object:  StoredProcedure [dbo].[SEL_PROG_PAGOS_BANAMEX_TXT_SP]    Script Date: 07/05/2019 04:04:13 p. m. ******/
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
--EXECUTE [SEL_PROG_PAGOS_BANAMEX_TXT_SP] 8909  
CREATE PROCEDURE [dbo].[SEL_PROG_PAGOS_BANAMEX_TXT_SP]   
       @idPadre      numeric(18,0) = 0  
AS  
BEGIN  
 SET NOCOUNT ON;  
  BEGIN TRY  
  
        DECLARE @idEmpresa numeric(18,0) = 0  
         
     SELECT @idEmpresa = [pal_id_empresa]    
   FROM [Pagos].[dbo].[PAG_LOTE_PAGO]  
   WHERE [pal_id_lote_pago] = @idPadre  
     --SELECT @idEmpresa  
     ---------------------------------------------------------------  
  --  Buscamos la  IP del Servidor y  la Base                  --  
  ---------------------------------------------------------------  
  DECLARE @ipServidor    VARCHAR(100);  
  DECLARE @cadIpServidor VARCHAR(100);  
  DECLARE @nombreBase    VARCHAR(100);  
  DECLARE @campos        VARCHAR(max);  
  DECLARE @campos1        VARCHAR(max);  
  DECLARE @campos2        VARCHAR(max);  
  DECLARE @tabla         VARCHAR(max);  
  DECLARE @condicion     VARCHAR(max);  
  DECLARE @consultaSaldo VARCHAR(max);  
  DECLARE @totalSaldo     decimal(18,5);  
  DECLARE @select         VARCHAR(max);  
  
  SELECT @nombreBase = [nombre_base]          
     ,@ipServidor = [ip_servidor]        
  FROM [Centralizacionv2].[dbo].[DIG_CAT_BASES_BPRO]  
  WHERE catemp_nombrecto = (SELECT [emp_nombrecto]  empNombreCto   
         FROM [ControlAplicaciones].[dbo].[cat_empresas]  
        WHERE [emp_idempresa] = @idEmpresa)  
    AND tipo = 2  
  
       -- select @nombreBase, @ipServidor  
  ---------------------------------------------------------------  
  --  Para cambiar la consulta si es local o no                --  
  ---------------------------------------------------------------  
  set @cadIpServidor =' [' + @ipServidor + '].'  
    
  IF (@ipServidor ='192.168.20.29')  
  BEGIN  
  set @cadIpServidor =''  
  END  
  
  
  
  DECLARE @empresa numeric(18,0) = (SELECT pal_id_empresa FROM [dbo].[PAG_LOTE_PAGO] WHERE pal_id_lote_pago = @idPadre)  
  DECLARE @usuario numeric(18,0) = (SELECT pal_id_usuario FROM [dbo].[PAG_LOTE_PAGO] WHERE pal_id_lote_pago = @idPadre)  
  
  --UPDATE Pagos.dbo.[PAG_LOTE_PAGO] SET pal_estatus = 4 WHERE (pal_id_empresa = @empresa AND pal_id_lote_pago = @idPadre)  
  --EXECUTE [INS_APLICA_LOTE_SP] @empresa,@idPadre,@usuario   
      ---------------------------------------------------------------  
  --  Obtenemos la consulta                      --  
  ---------------------------------------------------------------  
  DECLARE     @VariableTabla TABLE (ID INT IDENTITY(1,1)  
                                   ,idPadre           int  
           ,nombreLote        nvarchar(150)  
           ,idUsuario         nvarchar(50)  
           ,fechaRegistro     datetime  
           ,idProveedor       int    
           ,proveedor         nvarchar(250)  
           ,rfc            nvarchar(50)  
           ,polTipo         nvarchar(50)  
           ,annio             numeric  
           ,polMes         numeric  
           ,polConsecutivo numeric  
           ,polMovimiento     numeric  
           ,documento         nvarchar(100)  
           ,ordenCompra  nvarchar(30)  
           ,idEstatus         nvarchar(3)  
           ,cuentaPagadora    nvarchar(50)  
           --  
           ,cuentaOrigen       nvarchar(50)  
           ,numBancoOrigen     nvarchar(4)  
           ,nombreBancoOrigen  nvarchar(50)  
           ,sucursalOrigen     nvarchar(4)  
           ,tipoCtaOrigen      nvarchar(10)  
           ,numCtaOrigen       nvarchar(50)  
           --  
           ,cuentaDestino       nvarchar(50)  
           ,numeroBancoBCO      nvarchar(4)  
           ,numBancoDestino     nvarchar(4)  
           ,convenioCie    nvarchar(10)  
           ,nombreBancoDestino  nvarchar(50)  
           ,sucursalDestino     nvarchar(4)  
           ,tipoCtaDestino      nvarchar(10)  
           ,numCtaDestino       nvarchar(18)  
           ,numCtaClabeDestino  nvarchar(18)  
           ,numRefDestino       nvarchar(30)  
           --  
           ,importe           decimal(13,2)  
           ,importeSP           nvarchar(16)  
           ,ivaImporteSP       nvarchar(16)  
           --  
           ,moneda    nvarchar(5)  
           ,monedaCte          nvarchar(3)  
           ,razonSocial        nvarchar(30)  
           ,motivoPago         nvarchar(30)  
           ,fecha              nvarchar(10)  
           ,hora               nvarchar(4)  
           ,disponibilidad     nvarchar(2)  
           ,plazo              nvarchar(2)  
           ,referencia      nvarchar(30)  
           ,agrupar    int  
           ,autorizacion   int
		   ,direccion1		 nvarchar(35)
		   ,direccion2		 nvarchar(35)
		   ,fechaVencimiento   datetime
		   ,razonSocialEmisora nvarchar(35)
           )  
  
------------------  
  SET @campos='D.[pal_id_lote_pago]         AS idPadre  
           ,M.[pal_nombre]               AS nombreLote  
           ,M.[pal_id_usuario]           AS idUsuario  
           ,M.[pal_fecha]                AS fechaRegistro  
     ,D.[pad_idProveedor]          AS idProveedor  
     ,D.[pad_proveedor]   AS proveedor  
     ,(select p.per_rfc  ' +  
     '    from '+@cadIpServidor+'GA_Corporativa.[dbo].[per_personas] p   
     where p.per_idpersona = D.[pad_idProveedor]) as rfc     
     ,D.[pad_polTipo]              AS polTipo  
     ,D.[pad_polAnnio]             AS annio  
     ,D.[pad_polMes]       AS polMes  
     ,D.[pad_polConsecutivo]     AS polConsecutivo  
     ,D.[pad_polMovimiento]     AS polMovimiento  
     ,D.[pad_documento]   AS documento  
     ,D.[pad_ordenCompra]   AS ordenCompra  
     ,D.[pad_idEstatus]   AS idEstatus  
            ,ISNULL(D.[pad_cuentaProveedor],' +''''+'SIN CUENTA PAGADORA' +''''+')  as cuentaPagadora  
           ,ba.par_descrip1     as cuentaOrigen      
     ,ba.par_descrip5     as numBancoOrigen  
     ,case when ba.par_descrip5= ' +''''+'002' +''''+'  then ' +''''+'BANAMEX' +''''+'  
           when ba.par_descrip5= ' +''''+'012' +''''+'  then ' +''''+'BANCOMER' +''''+'  
     when ba.par_descrip5=' +''''+'014' +''''+'  then ' +''''+'SANTANDER' +''''+'  
     when ba.par_descrip5=' +''''+'021' +''''+'  then ' +''''+'HSBC' +''''+'  
     when ba.par_descrip5=' +''''+'072' +''''+'  then ' +''''+'BANORTE' +''''+'  
     else ' +''''+'Otro Banco' +''''+'  
    end    as nombreBancoOrigen          
      ,right(replicate (' +''''+'0' +''''+',4) + cast ( ISNULL(transbco.PAR_DESCRIP2,' +''''+'0000' +''''+') AS varchar(4)) ,4) as sucursalOrigen      
      ,case when transbco.PAR_DESCRIP1=' +''''+'CHEQUES' +''''+'  then ' +''''+'03' +''''+'                     
            else ' +''''+'00' +''''+'  
    end as tipoCtaOrigen                 
               ,right(replicate (' +''''+'0' +''''+',18) + cast((SELECT numeroCuenta FROM referencias.dbo.BancoCuenta WHERE ((cuenta like ' +''''+'%' +''''+' + D.[pad_cuentaProveedor] +' +''''+'%' +''''+') AND  (idEmpresa = M.[pal_id_empresa]))) AS varchar(25)) ,18) as numCtaOrigen           
            ,(select top 1 ba.PAR_DESCRIP1  
          from '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] ba  
      where PAR_IDENPARA= b.BCO_BANCO  
        and ba.par_tipopara = ' +''''+'ba' +''''+'  
       
     )             as cuentaDestino  
      ,b.BCO_BANCO          as numeroBancoBCO,'   
      SET @campos1= '(SELECT   TOP 1   PAG_CAT_BANXICO.pbx_numoficial  
    FROM     '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] AS ba INNER JOIN  
                PAG_CAT_BANXICO ON ba.PAR_DESCRIP5 = PAG_CAT_BANXICO.pbx_numoficial COLLATE SQL_Latin1_General_CP1_CI_AS  
    WHERE        (ba.PAR_IDENPARA = b.BCO_BANCO) AND (ba.PAR_TIPOPARA = ' +''''+'ba' +''''+') and (ba.par_status   = ' +''''+'A' +''''+') ORDER BY PAG_CAT_BANXICO.pbx_numoficial)  
         as numBancoDestino, isnull(b.BCO_CONVENIOCIE,' +''''''+') as convenioCie,  
      (SELECT   TOP 1   ba.PAR_DESCRIP1  
    FROM      '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] AS ba INNER JOIN  
                PAG_CAT_BANXICO ON ba.PAR_DESCRIP5 = PAG_CAT_BANXICO.pbx_numoficial COLLATE SQL_Latin1_General_CP1_CI_AS  
    WHERE        (ba.PAR_IDENPARA = b.BCO_BANCO) AND (ba.PAR_TIPOPARA = ' +''''+'ba' +''''+') and (ba.par_status   = ' +''''+'A' +''''+')  ORDER BY PAG_CAT_BANXICO.pbx_numoficial)  
         as nombreBancoDestino,  
    right(replicate (' +''''+'0' +''''+',4) + cast ( ISNULL(b.BCO_SUCURSAL,' +''''+'0000' +''''+') AS varchar(4)) ,4) as sucursalDestino                     
                 ,CASE WHEN b.bco_clabe is null  THEN ' +''''+'03' +''''+'   
       ELSE  ' +''''+'40' +''''+'                             
      END  as tipoCtaDestino         
    ,right(replicate (' +''''+'0' +''''+',18) + cast ( b.bco_numcuenta AS varchar(25)) ,18) as numCtaDestino  
    ,right(replicate (' +''''+'0' +''''+',18) + cast ( b.bco_clabe     AS varchar(20)) ,18) as numCtaClabeDestino  
    ,b.bco_refernum as numRefDestino                                                            
    ,replicate (' +''''+'0' +''''+',13)+  cast(ROUND(d.[pad_saldo], 2) as decimal(13,2)) as importe   
    ,REPLACE(CAST(CAST(ROUND(d.[pad_saldo], 2)  as decimal(13,2)) as varchar(16)),' +''''+'.' +''''+',' +''''+'' +''''+')                 as importeSP       
    ,REPLACE(CAST(CAST(ROUND((d.[pad_saldo])*0.16, 2) as decimal(13,2)) as varchar(16)),' +''''+'.' +''''+',' +''''+'' +''''+')           as ivaImporteSP   
    ,case when d.[pad_moneda]=' +''''+'PE' +''''+' then ' +''''+'MXP' +''''+'   
      else ' +''''+'Otro' +''''+'  
      end as moneda      
    ,' +''''+'000' +''''+'  as monedaCte            
    ,left(cast ( D.[pad_proveedor] AS varchar(30)) + replicate (' +''''+' ' +''''+',30) ,30)                               as razonSocial  
    ,left(cast ( ISNULL(D.[pad_ordenCompra],replicate (' +''''+' ' +''''+',24)) AS varchar(24)) + replicate (' +''''+' ' +''''+',24) ,24)  as motivoPago   
    ,REPLACE(CONVERT(VARCHAR(10),GETDATE(),5),' +''''+'-' +''''+',' +''''+'' +''''+')                      as fecha       
    ,REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108),' +''''+':' +''''+',' +''''+'' +''''+')                   as hora        
    ,' +''''+'H' +''''+'       as disponibilidad                                 
    ,' +''''+'00' +''''+'      as plazo  
    ,D.pad_polReferencia     as referencia, D.pad_agrupamiento as agrupar,  
    b.BCO_AUTORIZADA as autorizacion
	,cast(P.PER_CALLE1 + '' '' + P.PER_CALLE2 + '' '' + P.PER_CALLE3 + '' '' + P.PER_NUMEXTER +'' ''+P.PER_NUMINER+ '' ''+P.PER_CODPOS as nvarchar(35))   
	,cast(P.PER_COLONIA +'' ''+P.PER_DELEGAC+'' ''+P.PER_CIUDAD as nvarchar(35))   
	,D.pad_fechavencimiento
	,cast(ER.RazonSocial   as nvarchar(35))   '
  
  SET @tabla= ' [dbo].[PAG_LOTE_PAGO] M    
          INNER JOIN [Pagos].[dbo].[PAG_PROGRA_PAGOS_DETALLE] as D ON D.[pal_id_lote_pago] = M.[pal_id_lote_pago] AND D.[pal_id_lote_pago] = '+ cast (@idPadre as varchar(18)) +'
		  INNER JOIN [CentralizacionV2].[dbo].[DIG_CAT_EMPRESA_RAZON] as ER ON ER.IdEmpresa = M.[pal_id_empresa]  
    LEFT OUTER JOIN '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] ba ON ba.par_tipopara = ' +''''+'ba' +''''+' and ba.par_status=' +''''+'A' +''''+' and ba.par_descrip1 = D.[pad_cuentaProveedor] COLLATE Modern_Spanish_CI_AS  
          LEFT OUTER JOIN '+ @cadIpServidor + ' GA_Corporativa.[dbo].[per_personas] p ON p.per_idpersona = D.[pad_idProveedor]  
    LEFT OUTER JOIN '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[CON_BANCOS]   b ON b.bco_idpersona = D.[pad_idProveedor] AND B.BCO_NUMCUENTA = D.pad_cuentaDestino   COLLATE Modern_Spanish_CI_AS   
       LEFT OUTER  JOIN '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] tu ON tu.par_idenpara = b.bco_tipcuenta and tu.par_tipopara     = ' +''''+'tu' +''''+'  
    LEFT OUTER JOIN '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] transbco ON transbco.par_tipopara = ' +''''+'TRANSBCO' +''''+' and  transbco.PAR_DESCRIP5 =(SELECT TOP 1 ccheq.PAR_IDENPARA   
                                     FROM '+ @cadIpServidor + '['+  @nombreBase +'].[dbo].[PNC_PARAMETR] ccheq  
                                     WHERE ccheq.par_tipopara = ' +''''+'ccheq' +''''+'   
                                     AND ccheq.PAR_DESCRIP2 = (SELECT TOP 1 [pcp_claveCuentaPagadora]  
                                             FROM [Pagos].[dbo].[PAG_CAT_PROVEEDORES]  
                                                   WHERE [pcp_idProveedor]  = D.[pad_idProveedor]) COLLATE Modern_Spanish_CI_AS) '  
    
    
    
  SET @condicion= ' b.BCO_AUTORIZADA = 1 '  
  set @select = ' SELECT ' + @campos + @campos1 +'   FROM ' +  @tabla    
          

  print ' SELECT  ' + @campos + @campos1 +  '   FROM ' +  @tabla  +';'                        
  INSERT INTO  @VariableTabla  EXEC ( ' SELECT  ' + @campos + @campos1 +  
                                      '   FROM ' +  @tabla  +';');  

  --      SELECT DISTINCT idPadre,nombreLote,idUsuario,fechaRegistro,idProveedor,proveedor,rfc,  
  --  '0' as idEstatus,cuentaPagadora,cuentaOrigen,numBancoOrigen,nombreBancoOrigen,  
  --  sucursalOrigen,tipoCtaOrigen,numCtaOrigen,cuentaDestino,numeroBancoBCO,  
  --  numBancoDestino,convenioCie, nombreBancoDestino,sucursalDestino,tipoCtaDestino,  
  --  numCtaDestino,numCtaClabeDestino,numRefDestino,  
  --  SUM(importe) AS "importeTotal", '0' as motivoPago,  
  --  moneda,monedaCte,razonSocial,fecha,hora,disponibilidad,plazo,referencia,agrupar  
  --FROM  @VariableTabla AS  V  
  --GROUP BY idPadre,nombreLote,idUsuario,fechaRegistro,idProveedor,proveedor,rfc,  
  --  cuentaPagadora,cuentaOrigen,numBancoOrigen,nombreBancoOrigen,  
  --  sucursalOrigen,tipoCtaOrigen,numCtaOrigen,cuentaDestino,numeroBancoBCO,  
  --  numBancoDestino,convenioCie, nombreBancoDestino,sucursalDestino,tipoCtaDestino,  
  --  numCtaDestino,numCtaClabeDestino,numRefDestino,  
  --  moneda,monedaCte,razonSocial,fecha,hora,disponibilidad,plazo,referencia,agrupar 
  
	DECLARE     @VariableTablaF TABLE (ID INT IDENTITY(1,1)
			,idPadre			int
			,nombreLote   	    nvarchar(150)
			,idUsuario	        nvarchar(50)
			,fechaRegistro	    datetime
			,idProveedor		int  
			,proveedor			nvarchar(250)
			,rfc   				nvarchar(50)
			,idEstatus			nvarchar(3)
			,cuentaPagadora		nvarchar(50)
			,cuentaOrigen       nvarchar(50)
			,numBancoOrigen     nvarchar(4)
			,nombreBancoOrigen  nvarchar(50)
			,sucursalOrigen     nvarchar(4)
			,tipoCtaOrigen      nvarchar(10)
			,numCtaOrigen       nvarchar(50)
			,cuentaDestino      nvarchar(50)
			,numeroBancoBCO     nvarchar(4)
			,numBancoDestino    nvarchar(4)
			,convenioCie		nvarchar(10)
			,nombreBancoDestino nvarchar(50)
			,sucursalDestino    nvarchar(4)
			,tipoCtaDestino     nvarchar(10)
			,numCtaDestino      nvarchar(18)
			,numCtaClabeDestino nvarchar(18)
			,numRefDestino      nvarchar(30)
			,importe	        decimal(13,2)
			,moneda				nvarchar(5)
			,monedaCte          nvarchar(3)
			,razonSocial        nvarchar(30)
			,motivoPago         nvarchar(30)
			,fecha              nvarchar(10)
			,hora               nvarchar(4)
			,disponibilidad     nvarchar(2)
			,plazo              nvarchar(2)
			,referencia			nvarchar(30)
			,agrupar			int
			,direccion1			nvarchar(35)
			,direccion2			nvarchar(35)
			,fechaVencimiento   datetime
			,razonSocialEmisora        nvarchar(35)
			)
	INSERT INTO @VariableTablaF
	SELECT DISTINCT idPadre,nombreLote,idUsuario,fechaRegistro,idProveedor,proveedor,rfc,
					'0' as idEstatus,cuentaPagadora,cuentaOrigen,numBancoOrigen,nombreBancoOrigen,
					sucursalOrigen,tipoCtaOrigen,numCtaOrigen,cuentaDestino,numeroBancoBCO,
					numBancoDestino,convenioCie, nombreBancoDestino,sucursalDestino,tipoCtaDestino,
					numCtaDestino,numCtaClabeDestino,numRefDestino,
					SUM(importe) AS "importeTotal", '0' as motivoPago,
					moneda,monedaCte,razonSocial,fecha,hora,disponibilidad,plazo,referencia,agrupar,direccion1,direccion2,fechaVencimiento,razonSocialEmisora
			FROM  @VariableTabla AS  V
			GROUP BY idPadre,nombreLote,idUsuario,fechaRegistro,idProveedor,proveedor,rfc,
					cuentaPagadora,cuentaOrigen,numBancoOrigen,nombreBancoOrigen,
					sucursalOrigen,tipoCtaOrigen,numCtaOrigen,cuentaDestino,numeroBancoBCO,
					numBancoDestino,convenioCie, nombreBancoDestino,sucursalDestino,tipoCtaDestino,
					numCtaDestino,numCtaClabeDestino,numRefDestino,
					moneda,monedaCte,razonSocial,fecha,hora,disponibilidad,plazo,referencia,agrupar,direccion1,direccion2,fechaVencimiento,razonSocialEmisora

	DECLARE @VariableTablaReporte TABLE(
								id			INT IDENTITY(1,1),
								[1]			NVARCHAR(3),
								[3]			NVARCHAR(3),
								[13]		NVARCHAR(10),
								[12]		NVARCHAR(6),
								[8]			NVARCHAR(3),
								[6]			NVARCHAR(15),
								[2]			NVARCHAR(8),
								[5]			NVARCHAR(20),
								[10]		NVARCHAR(3),
								[16]		NVARCHAR(20),
								[11]		NVARCHAR(15),
								[42]		NVARCHAR(6),
								[44]		NVARCHAR(35),
								[45]		NVARCHAR(35),
								[46]		NVARCHAR(35),
								[47]		NVARCHAR(35),
								[9]			NVARCHAR(2),
								[14]		NVARCHAR(2),
								[20]		NVARCHAR(80),
								[21]		NVARCHAR(35),
								[21b]		NVARCHAR(35),
								[22]		NVARCHAR(15),
								[24]		NVARCHAR(2),
								[23]		NVARCHAR(12),
								[25]		NVARCHAR(16),
								[26]		NVARCHAR(3),
								[27]		NVARCHAR(8),
								[60]		NVARCHAR(35),
								[61]		NVARCHAR(2),
								[28]		NVARCHAR(30),
								[164]		NVARCHAR(17),
								[165]		NVARCHAR(1),
								[167]		NVARCHAR(1),
								[166]		NVARCHAR(2),
								[168]		NVARCHAR(20),
								[34]		NVARCHAR(16),
								[35]		NVARCHAR(20),
								[36]		NVARCHAR(15),
								[62]		NVARCHAR(10),
								[63]		NVARCHAR(2),
								[19]		NVARCHAR(3),
								[37]		NVARCHAR(50),
								[38]		NVARCHAR(5),
								[49]		NVARCHAR(50),
								[64]		NVARCHAR(15),
								[65]		NVARCHAR(1),
								[169]		NVARCHAR(11),
								[150]		NVARCHAR(1),
								[151]		NVARCHAR(1),
								[reservado] NVARCHAR(253)
	)
	INSERT INTO @VariableTablaReporte
	SELECT [1],[3],[13],[12],[8],[6],RIGHT('00000000' + CAST( ROW_NUMBER() OVER(ORDER BY [1] ASC) as varchar) ,8) as [2],[5],[10],[16],[11],[42],[44],[45],[46],[47],[9],[14],[20],[21],[21b],[22],[24],[23],[25],[26],[27],[60],[61],[28],[164],[165],[166],[167],[168],[34],[35],[36],[62],[63],[19],[37],[38],[49],[64],[65],[169],[150],[151],[reservado] FROM (
	SELECT 
			'PAY'																						as [1]			,--Obligatorio--1--tipo de registro
			'485'																						as [3]			,--Obligatorio--3--codigo de pais del cliente
			SPACE(10)																					as [13]			,--13--numero de cuenta cliente (LATAM)
			convert(varchar, getdate(), 12)																as [12]			,--Obligatorio--12--Fecha de transaccion
			'001'																						as [8]			,--Obligatorio--8--codigo de transaccion--el manual tiene un catalogo de los diversos tipos de pagos, cual de todos estos se van a usar?, hay algun diferenciador entre las opciones que nos muestra el manual
			LEFT(Ltrim(Rtrim(referencia))+SPACE(15),15)													as [6]			,--Obligatorio--6--referencia de la transaccion del cliente--del set de datos resultantes del query de fernando se toma el campo de referencia?
	LEFT(''+SPACE(8),8)																					as [2]			,--Obligatorio--2--numero de secuencia de la transaccion
			LEFT(Ltrim(Rtrim(rfc))+SPACE(20),20)														as [5]			,--Obligatorio--5--RFC del beneficiario
			'MXN'																						as [10]			,--Obligatorio--10--moneda del numero
			LEFT( Ltrim(Rtrim(idProveedor))+SPACE(20),20)												as [16]			,--16--codigo del beneficiario--que se utilizara como codigo de beneficiario
			RIGHT('000000000000000'+Ltrim(Rtrim((cast(cast((importe * 100) as int) as varchar)))),15)	as [11]			,--11--importe de la transaccion
			convert(varchar, fechaVencimiento, 12) 														as [42]			,--42--Maturity date--cual es la fecha de expiracion de la orden de campo
			LEFT(RIGHT('0000000'+Ltrim(Rtrim(idPadre)),7)+SPACE(35),35)									as [44]			,--44--Detalles de la transaccion linea 1--que tipo de referencia numerica se colocara
			LEFT(razonSocialEmisora+SPACE(35),35)														as [45]			,--45--Detalles de la transaccion linea 2--que tipo de referencia numerica se colocara
			SPACE(35)																					as [46]			,--46--Detalles de la transaccion linea 3--se llenara este campo?
			SPACE(35)																					as [47]			,--47--Detalles de la transaccion linea 4--se llenara este campo?
			'06'																						as [9]			,--9--codigo de transaccion local--sera con comprobante fiscal o sin comprobante fiscal o como se va disernir sobre este campo
			'01'																						as [14]			,--14--tipo de cuenta del cliente
			LEFT(LEFT((rtrim(ltrim(RIGHT(proveedor,35)))+SPACE(35)  ),35)+SPACE(80),80)					as [20]			,--20--nombre del beneficiario--solo se concideraran 35 posiciones
			LEFT(direccion1+SPACE(35),35)																as [21]			,--21--Direccion del beneficiario 1--de donde obtengo esta informacion
			LEFT(direccion2+SPACE(35),35)																as [21b]		,--21b--Direccion del beneficiario 2--de donde obtengo esta informacion
			SPACE(15)																					as [22]			,--22--Ciudad del beneficiario--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(2)																					as [24]			,--24--codigo estado--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(12)																					as [23]			,--23--codigo postal--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(16)																					as [25]			,--25--numero telefonico del beneficiario
			'000'																						as [26]			,--26--codigo del banco del beneficiario--tenemos un catalogo para los tipos de transaccion y cuando son intgerbancarias como se va a tomar esto o como disernimos de uno de otro
			SPACE(8)																					as [27]			,--27--Agencia del banco del beneficiario
			LEFT(numCtaClabeDestino+SPACE(35),35)														as [60]			,--60--numero de cuenta del beneficiario --necesitamos saber el tipo de transaccion ya que es obligatorio para ciertos casos
			'05'																						as [61]			,--61--tipo de cuenta del beneficiario --que campos se van a uutilizar
			LEFT('BcoBenef'+SPACE(30),30)																as [28]			,--28--direccion del banco --se conoce la direccion del banco o se pondra el valor default?
			'00000000000000000'																			as [164]		,--164--importe del impuesto--de donde se saca esta informacion
			'N'																							as [165]		,--165--bandera de prioridad que tipo de prioridad tendra
			'N'																							as [166]		,--166--confidencial--como se pondran las transacciones como privadas si o jno
			'01'																						as [167]		,--167--fecha de acreditacion al beneficiario--depende del tipo de transaccion, y cual vamos a tener
			LEFT(LEFT((rtrim(ltrim(RIGHT(numctaOrigen,11))))+SPACE(11),11)+SPACE(20),20)				as [168]		,--168--numero de cuenta de debito banamex--
			SPACE(16)																					as [34]			,--34--fax del benificiario
			SPACE(20)																					as [35]			,--35--contacto del fax del Beneficiario
			SPACE(15)																					as [36]			,--36--Departamento del fax del beneficiario
			SPACE(10)																					as [62]			,--62--numero de cuenta del beneficiario
			SPACE(2)																					as [63]			,--63--tipo de cuenta
			SPACE(3)																					as [19]			,--19--metodo de entrega de transacciones--depende del tipo de transaccion definir que se usara en cada caso
			SPACE(50)																					as [37]			,--37--identificacion de un titulo
			'00000'																						as [38]			,--38--codigo de activacion del beneficiario
			SPACE(50)																					as [49]			,--49--correo electronico del beneficiario--de donde se deduce este campo, o si se va activar
			LEFT( '999999999'+SPACE(15),15)																as [64]			,--64--importe maximo por pago--que cantidad se va a configurar
			SPACE(1)																					as [65]			,--65--Actualizacion del registro de pago
			LEFT('NONE' +SPACE(11),11)																	as [169]		,--169--notificacion al beneficiario--se va activar?
			SPACE(1)																					as [150]		,--150--marca del cheque
			SPACE(1)																					as [151]		,--151--bandera del cheque
			SPACE(253)																					as [reservado]	-- --espacios en blanco
	 FROM @VariableTablaF
	 WHERE numBancoOrigen=numBancoDestino

	 UNION ALL
 
	 SELECT 

			'PAY'																							,--Obligatorio--1--tipo de registro
			'485'																							,--Obligatorio--3--codigo de pais del cliente
			SPACE(10)																						,--13--numero de cuenta cliente (LATAM)
			convert(varchar, getdate(), 12)																	,--Obligatorio--12--Fecha de transaccion
			'072'																							,--Obligatorio--8--codigo de transaccion--el manual tiene un catalogo de los diversos tipos de pagos, cual de todos estos se van a usar?, hay algun diferenciador entre las opciones que nos muestra el manual
			LEFT(Ltrim(Rtrim(referencia))+SPACE(15),15)														,--Obligatorio--6--referencia de la transaccion del cliente--del set de datos resultantes del query de fernando se toma el campo de referencia?
	LEFT(''+SPACE(8),8)																						,--Obligatorio--2--numero de secuencia de la transaccion
			LEFT(Ltrim(Rtrim(rfc))+SPACE(20),20)															,--Obligatorio--5--RFC del beneficiario
			'MXN'																							,--Obligatorio--10--moneda del numero
			LEFT( Ltrim(Rtrim(idProveedor))+SPACE(20),20)													,--16--codigo del beneficiario--que se utilizara como codigo de beneficiario
			RIGHT('000000000000000'+Ltrim(Rtrim((cast(cast((importe * 100) as int) as varchar)))),15)		,--11--importe de la transaccion
			convert(varchar, fechaVencimiento, 12) 															,--42--Maturity date--cual es la fecha de expiracion de la orden de campo
			LEFT(RIGHT('0000000'+Ltrim(Rtrim(idPadre)),7)+SPACE(35),35)										,--44--Detalles de la transaccion linea 1--que tipo de referencia numerica se colocara
			LEFT(razonSocialEmisora+SPACE(35),35)															,--45--Detalles de la transaccion linea 2--que tipo de referencia numerica se colocara
			SPACE(35)																						,--46--Detalles de la transaccion linea 3--se llenara este campo?
			SPACE(35)																						,--47--Detalles de la transaccion linea 4--se llenara este campo?
			'06'																							,--9--codigo de transaccion local--sera con comprobante fiscal o sin comprobante fiscal o como se va disernir sobre este campo
			'01'																							,--14--tipo de cuenta del cliente
			LEFT(LEFT((rtrim(ltrim(RIGHT(proveedor,35)))+SPACE(35)  ),35)+SPACE(80),80)						,--20--nombre del beneficiario--solo se concideraran 35 posiciones
			LEFT(direccion1+SPACE(35),35)																	,--21--Direccion del beneficiario 1--de donde obtengo esta informacion
			LEFT(direccion2+SPACE(35),35)																	,--21b--Direccion del beneficiario 2--de donde obtengo esta informacion
			SPACE(15)																						,--22--Ciudad del beneficiario--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(2)																						,--24--codigo estado--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(12)																						,--23--codigo postal--este campo es obligatorio pero depende del codigo de transaccion 
			SPACE(16)																						,--25--numero telefonico del beneficiario
			numBancoDestino																					,--26--codigo del banco del beneficiario--tenemos un catalogo para los tipos de transaccion y cuando son intgerbancarias como se va a tomar esto o como disernimos de uno de otro
			SPACE(8)																						,--27--Agencia del banco del beneficiario
			LEFT(numCtaClabeDestino+SPACE(35),35)															,--60--numero de cuenta del beneficiario --necesitamos saber el tipo de transaccion ya que es obligatorio para ciertos casos
			'05'																							,--61--tipo de cuenta del beneficiario --que campos se van a uutilizar
			LEFT('BcoBenef'+SPACE(30),30)																	,--28--direccion del banco --se conoce la direccion del banco o se pondra el valor default?
			'00000000000000000'																				,--164--importe del impuesto--de donde se saca esta informacion
			'N'																								,--165--bandera de prioridad que tipo de prioridad tendra
			'N'																								,--166--confidencial--como se pondran las transacciones como privadas si o jno
			'01'																							,--167--fecha de acreditacion al beneficiario--depende del tipo de transaccion, y cual vamos a tener
			LEFT(LEFT((rtrim(ltrim(RIGHT(numctaOrigen,11))))+SPACE(11),11)+SPACE(20),20)					,--168--numero de cuenta de debito banamex--
			SPACE(16)																						,--34--fax del benificiario
			SPACE(20)																						,--35--contacto del fax del Beneficiario
			SPACE(15)																						,--36--Departamento del fax del beneficiario
			SPACE(10)																						,--62--numero de cuenta del beneficiario
			SPACE(2)																						,--63--tipo de cuenta
			SPACE(3)																						,--19--metodo de entrega de transacciones--depende del tipo de transaccion definir que se usara en cada caso
			SPACE(50)																						,--37--identificacion de un titulo
			'00000'																							,--38--codigo de activacion del beneficiario
			SPACE(50)																						,--49--correo electronico del beneficiario--de donde se deduce este campo, o si se va activar
			LEFT( '999999999'+SPACE(15),15)																	,--64--importe maximo por pago--que cantidad se va a configurar
			SPACE(1)																						,--65--Actualizacion del registro de pago
			LEFT('NONE' +SPACE(11),11)																		,--169--notificacion al beneficiario--se va activar?
			SPACE(1)																						,--150--marca del cheque
			SPACE(1)																						,--151--bandera del cheque
			SPACE(253)																						-- --espacios en blanco
	 FROM @VariableTablaF
	 WHERE numBancoOrigen<>numBancoDestino
	)A

	DECLARE @reporteFinal TABLE (
		id INT IDENTITY(1,1),
		renglon NVARCHAR(MAX)
	)
	INSERT INTO @reporteFinal
	SELECT 
		[1]+[3]+[13]+[12]+[8]+[6]+[2]+[5]+[10]+[16]+[11]+[42]+[44]+[45]+[46]+[47]+[9]+[14]+[20]+[21]+[21b]+[22]+[24]+[23]+[25]+[26]+[27]+[60]+[61]+[28]+[164]+[165]+[166]+[167]+[168]+[34]+[35]+[36]+[62]+[63]+[19]+[37]+[38]+[49]+[64]+[65]+[169]+[150]+[151]+[reservado]
	FROM @VariableTablaReporte ORDER BY 1
	INSERT INTO @reporteFinal
	SELECT 
		'TRL'
		+RIGHT('000000000000000' + CAST(COUNT(1) AS VARCHAR) ,15)
		+RIGHT('000000000000000' + CAST(SUM(CAST ([11] AS INT)) AS VARCHAR) ,15)
		+'000000000000000'
		+RIGHT('000000000000000' + CAST(COUNT(1) AS VARCHAR) ,15)
		+SPACE(37)
	FROM @VariableTablaReporte


	SELECT renglon FROM @reporteFinal ORDER BY 1
 
  
  END TRY  
    
  BEGIN CATCH  
  PRINT ('Error: ' + ERROR_MESSAGE())  
  DECLARE @Mensaje  nvarchar(max),  
  @Componente nvarchar(50) = '[SEL_PROG_PAGOS_BANAMEX_TXT_SP]'  
  SELECT @Mensaje = ERROR_MESSAGE()  
  EXECUTE [INS_ERROR_SP] @Componente, @Mensaje;   
  --SELECT 0 --Encontro error  
  SELECT 'ERROR EN LA CONSULTA'  
  END CATCH  
END  
  
  
GO


