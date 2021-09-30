using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business.Pago;
using DataAccess.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using static PagoImplements;

    public class PagoApiController : ApiController
    {

        PagoImplements iPago;

        

        public PagoApiController()
        {
            iPago = new PagoImplements();
        }

        public HttpResponseMessage Get(string id)
        {
            var parameters = id.Split('|');
            switch (parameters[0])
            {
                case "1":
                    var pago = iPago.obtienePagos(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_PROGRAMACION_PAGOS_SP_Result>>(HttpStatusCode.OK, pago);
                case "2":
                    var encabezado = iPago.obtieneEncabezado(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<SEL_ENCABEZADO_SP_Result>(HttpStatusCode.OK, encabezado);
                case "3":
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                case "4":

                    JObject json = JObject.Parse(parameters[1]);
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                case "5":
                    var pagosGuardados = iPago.obtienePagosGuardados(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_PROG_PAGOS_GUARDADA_SP_Result>>(HttpStatusCode.OK, pagosGuardados);

                case "6":                
                    var idPadre = (Convert.ToInt32(parameters[4]) == 0)?iPago.insertaPagosPadre(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), parameters[3], Convert.ToInt16(parameters[5]), Convert.ToDecimal(parameters[6])) :Convert.ToInt16(parameters[4]);
                    return Request.CreateResponse<decimal?>(HttpStatusCode.OK, idPadre);
                case "7":
                    var agrupadores = iPago.obtieneagrupadores(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_AGRUPADORES_SP_Result>>(HttpStatusCode.OK, agrupadores);
                case "8":
                    var provedores = iPago.obtieneprovedores(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_AGRUPADORES_PROVEEDOR_SP_Result>>(HttpStatusCode.OK, provedores);
                case "9":
                    var empresas = iPago.obtenempresas(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_EMPRESAS_SP_Result>>(HttpStatusCode.OK, empresas);

                case "10":
                    var totalxEmpresa = iPago.obtentotalxEmpresa(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_TOTAL_BANCOS_SP_Result>>(HttpStatusCode.OK, totalxEmpresa);

                //LQMA 04032016
                case "11":
                    var ingresos = iPago.obtieneIngresos(Convert.ToDecimal(parameters[1]), Convert.ToDecimal(parameters[2]));
                    return Request.CreateResponse<IEnumerable<SEL_CUENTAS_INGRESOS_SP_Result>>(HttpStatusCode.OK, ingresos);

                case "12":
                    var egresos = iPago.obtieneEgresos(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                    return Request.CreateResponse<IEnumerable<SEL_CUENTAS_EGRESOS_SP_Result>>(HttpStatusCode.OK, egresos);

                case "13":
                    var lotes = iPago.obtieneLotes(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]));
                    return Request.CreateResponse<IEnumerable<SEL_LOTE_SP_Result>>(HttpStatusCode.OK, lotes);

                case "14":
                    var transferencias = iPago.obtieneTransferencias(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_CUENTAS_TRANSFERENCIAS_SP_Result>>(HttpStatusCode.OK, transferencias);

                case "15":
                    var otrosIngresos = iPago.obtieneOtrosIngresos(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_INGRESO_OTROS_SP_Result>>(HttpStatusCode.OK, otrosIngresos);
                //FAL Agrupadores y Cuentas

                case "16":
                    var ProvedoresCuenta = iPago.obtenproveedoresCuenta(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_PROVEEDORES_CTA_PAGADORA_SP_Result>>(HttpStatusCode.OK, ProvedoresCuenta);
                case "17":
                    var CuentasPagadoras = iPago.obtenCuentasEgresos(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_CUENTAS_EGRESOS_SP_Result>>(HttpStatusCode.OK, CuentasPagadoras);
                //FAL 10042016 Obtiene los datos para llenar los totales de bancos

                case "18":
                    var transferenciasxbanco  = iPago.obtentransferencias(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_TRANSFERENCIASXEMPRESA_Result>>(HttpStatusCode.OK, transferenciasxbanco);
                //FAL 11222016 Obtiene los datos para llenar los totales de bancos

                case "19":
                    var BancosCompleta = iPago.obtenBancosCompleta(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_TOTAL_BANCOS_P_NP_SP_Result>>(HttpStatusCode.OK, BancosCompleta);

                //FAL 23052016 LLena los parametros de los distintos escenarios.
                case "20":
                    var parametrosEscenarios = iPago.obtenParametrosEscenarios(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<SEL_ESCENARIO_SP_Result>(HttpStatusCode.OK, parametrosEscenarios);

                case "21":
                    var lotesxfecha = iPago.obtieneLotesxFecha(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToString(parameters[3]), Convert.ToString(parameters[4]), Convert.ToInt32(parameters[5]));
                    return Request.CreateResponse<IEnumerable<SEL_LOTESXFECHA_SP_Result>>(HttpStatusCode.OK, lotesxfecha);

                case "22":
                    var trasferxlote = iPago.selTrasnferenciaxlote(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<SEL_TRANSFERENCIASXLOTE_Result>(HttpStatusCode.OK, trasferxlote);

                case "23":
                    var usertransfer = iPago.obtieneUsuarioTransferencia(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse <IEnumerable<SEL_USUARIO_TRANSFERENCIA_Result>>(HttpStatusCode.OK, usertransfer);

                case "24":
                    var pagoxvencer = iPago.obtienePagosxvencer(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_PROGRAMACION_PAGOSXVENCER_SP_Result>>(HttpStatusCode.OK, pagoxvencer);

                case "25":
                    var transferxfecha = iPago.obtienetransferxFecha(Convert.ToInt32(parameters[1]), Convert.ToString(parameters[2]), Convert.ToString(parameters[3]));
                    return Request.CreateResponse<IEnumerable<SEL_TRANSFERENCIASXEMPRESAXFECHA_Result>>(HttpStatusCode.OK, transferxfecha);

                case "26":
                    var pagoenrojo = iPago.obtienePagosenRojo(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_PROGRAMACION_PAGOSENROJO_SP_Result>>(HttpStatusCode.OK, pagoenrojo);

                //FAL Trae los lotes por estatus para su administración.
                //case "21":
                //    var lotesxestatus = iPago.obtieneLotesxestatus(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                //    return Request.CreateResponse<IEnumerable<SEL_LOTE_ADMIN_SP_Result>>(HttpStatusCode.OK, lotesxestatus);
                case "27":
                    var errorescxp = iPago.obtenerrores(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_ERRORES_CXP_SP_Result>>(HttpStatusCode.OK, errorescxp);

                case "28":

                    List<DocumentosPDF_XML> listDocs = new List<DocumentosPDF_XML>();

                    DocumentosPDF_XML pdf = iPago.ConsultaPDF(Convert.ToString(parameters[1]), Convert.ToString(parameters[2]), Convert.ToString(parameters[3]), Convert.ToString(parameters[4]), Convert.ToString(parameters[5]));
                    listDocs.Add(pdf);

                    return Request.CreateResponse<IEnumerable<DocumentosPDF_XML>>(HttpStatusCode.OK, listDocs);

                case "29":
                    var totalxliberar = iPago.liberadocumento(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_LIBERAR_CXP_SP_Result>>(HttpStatusCode.OK, totalxliberar);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Post(string id)
        {
            var parameters = id.Split('|');
            //var total = id.Length;

            switch (parameters[0])
            {
                case "1":

                    var idEmpleado = parameters[1];

                    var ss = Request.Content;
                    string data = ss.ReadAsStringAsync().Result;

                    string cadenaBulkcopy = ConfigurationManager.AppSettings["cadenaBulkcopy"];
                    var idPagosAprob = iPago.guardaPagos(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), data, parameters, Convert.ToInt32(parameters[5]), cadenaBulkcopy);

                    if (idPagosAprob == 0)
                        return Request.CreateResponse<int?>(HttpStatusCode.OK, 0);
                    else
                        return Request.CreateResponse<int?>(HttpStatusCode.InternalServerError, 1);

                case "2":
                    //FAL guardo el json completo de agrupadores
                    int idEmpresa = Convert.ToInt32(parameters[1]);
                    var zz = Request.Content;
                    string dataAgrupadores = zz.ReadAsStringAsync().Result;
                    var borra = iPago.borraAgrupadoresPorveedoresxEmpresa(Convert.ToInt32(parameters[1]));
                    var intAgrupador = iPago.guardaAgrupadores(idEmpresa, dataAgrupadores);
                    return Request.CreateResponse<decimal?>(HttpStatusCode.OK, 1);

                case "3":
                    //FAL guardo el json completo de cuentas provedores
                    int idEmpresaCuentas = Convert.ToInt32(parameters[1]);
                    var qq = Request.Content;
                    string dataCuentas = qq.ReadAsStringAsync().Result;
                    var intCuentas = iPago.guardaCuentas(idEmpresaCuentas, dataCuentas);
                    return Request.CreateResponse<decimal?>(HttpStatusCode.OK, 1);

                case "4":
                    //FAL 11042016 generación de pagos
                    int idPadreLote = 0;
                    if (!String.IsNullOrEmpty(Convert.ToString(parameters[2])))
                    {
                        idPadreLote = Convert.ToInt32(parameters[2]);
                    }

                    int idempresaArchivo = Convert.ToInt32(parameters[1]);
                    string folderName = iPago.ObtenerDato(1); //ConfigurationManager.AppSettings["rutaLayoutsPago"];
                    string folderNameDestino = iPago.ObtenerDato(2);//ConfigurationManager.AppSettings["rutaLayoutsPagoDestino"];
                    string path = iPago.ObtenerDato(5);

                    string nombreArchivo = "";

                    string banco = iPago.ObtenNombreBanco(idPadreLote).FirstOrDefault();

                    string bank = banco.Split(' ')[0];
                    switch (bank)
                    {
                        case "BANAMEX":                            
                            nombreArchivo = iPago.generaArchivoBanamexZip(idempresaArchivo, folderName, idPadreLote, folderNameDestino, path);
                            break;
                        case "BANCOMER":                            
                            nombreArchivo = iPago.generaArchivoZip(idempresaArchivo, folderName, idPadreLote, folderNameDestino);
                            break;
                        default:
                            return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }                    

                    string pathString = System.IO.Path.Combine(folderName, nombreArchivo);

                    if (File.Exists(pathString))
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(nombreArchivo, Encoding.UTF8, "text/html")
                        };
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }

                //LQMA 08042016 envia solicitud de aprobacion
                case "5":
                    var result = iPago.enviaAprobacion(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToDecimal(parameters[4]));
                    return Request.CreateResponse<int?>(HttpStatusCode.OK, result);

                //LQMA 09042016 envia solicitud de aprobacion
                case "6": //LQMA 11042016
                    var resp = iPago.apruebaLote(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToDecimal(parameters[4]), Convert.ToDecimal(parameters[5]), Convert.ToDecimal(parameters[6]), parameters[7], Convert.ToDecimal(parameters[8]), Convert.ToDecimal(parameters[9]));
                    return Request.CreateResponse<int?>(HttpStatusCode.OK, resp);

                case "7":
                    var Aplicacion = iPago.aplicaLote(Convert.ToInt32(parameters[1]), Convert.ToDecimal(parameters[2]), Convert.ToDecimal(parameters[3]));
                    return Request.CreateResponse<int?>(HttpStatusCode.OK, Aplicacion);

                case "8":
                    var UpdTransferencia = iPago.updTransferencia(Convert.ToInt32(parameters[1]), Convert.ToDecimal(parameters[2]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, UpdTransferencia);

                case "9":
                    var APLTransferencia = iPago.aplTransferencia(Convert.ToInt32(parameters[1]), Convert.ToDecimal(parameters[2]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, APLTransferencia);
                case "10":
                    var pruebaData = iPago.UpdateCartera(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, pruebaData.ToString());
                case "11":
                    var AplicacionDirecta = iPago.aplicaLoteDirecto(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, AplicacionDirecta.ToString());
                case "12":
                    var borraLote = iPago.borraLoteSQL(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, borraLote.ToString());

                case "13":

                    var idEmpleado1 = parameters[1];

                    var ss1 = Request.Content;
                    string data1 = ss1.ReadAsStringAsync().Result;

                    string cadenaBulkcopy1 = ConfigurationManager.AppSettings["cadenaBulkcopy"];
                    var idPagosAprob1 = iPago.guardaPagosAutoriza(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), data1, cadenaBulkcopy1);

                    if (idPagosAprob1 == 0)
                        return Request.CreateResponse<int?>(HttpStatusCode.OK, 0);
                    else
                        return Request.CreateResponse<int?>(HttpStatusCode.InternalServerError, 1);

                case "14":
                    var liberaData = iPago.liberaDocumento(Convert.ToInt32(parameters[1]), Convert.ToString(parameters[2]));
                    return Request.CreateResponse<string>(HttpStatusCode.OK, liberaData.ToString());

                case "15":
                    string carpeta = iPago.ObtenerDato(3);
                    string carpetaProcesados = iPago.ObtenerDato(4);
                    string mail1 = iPago.ObtenerDato(6); 
                    string mail2 = iPago.ObtenerDato(7); 
                    var MoverArchivos = iPago.procesarLogBanamex(carpeta, carpetaProcesados, mail1, mail2);
                    return Request.CreateResponse<string>(HttpStatusCode.OK, MoverArchivos.ToString());
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }    
}
