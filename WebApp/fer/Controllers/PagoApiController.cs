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
                    var idPadre = (Convert.ToInt32(parameters[4]) == 0)?iPago.insertaPagosPadre(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), parameters[3], Convert.ToInt32(parameters[5])) :Convert.ToInt32(parameters[4]);
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
                case "19":
                    var BancosCompleta = iPago.obtenBancosCompleta(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<IEnumerable<SEL_TOTAL_BANCOS_P_NP_SP_Result>>(HttpStatusCode.OK, BancosCompleta);

                //FAL 23052016 LLena los parametros de los distintos escenarios.
                case "20":
                    var parametrosEscenarios = iPago.obtenParametrosEscenarios(Convert.ToInt32(parameters[1]));
                    return Request.CreateResponse<SEL_ESCENARIO_SP_Result>(HttpStatusCode.OK, parametrosEscenarios);

                //FAL Trae los lotes por estatus para su administración.
                //case "21":
                //    var lotesxestatus = iPago.obtieneLotesxestatus(Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]));
                //    return Request.CreateResponse<IEnumerable<SEL_LOTE_ADMIN_SP_Result>>(HttpStatusCode.OK, lotesxestatus);
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
                    string folderName = ConfigurationManager.AppSettings["rutaLayoutsPago"];
                    string folderNameDestino = ConfigurationManager.AppSettings["rutaLayoutsPagoDestino"];

                    string nombreArchivo = iPago.generaArchivoZip(idempresaArchivo,folderName, idPadreLote, folderNameDestino);
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

                case "7": //LQMA 11042016
                    var Aplicacion = iPago.aplicaLote(Convert.ToInt32(parameters[1]),Convert.ToDecimal(parameters[2]),Convert.ToDecimal(parameters[3]));
                    return Request.CreateResponse<int?>(HttpStatusCode.OK, Aplicacion);

            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }    
}
