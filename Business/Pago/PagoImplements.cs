using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using Business.Utilerias;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Data.Entity.Core.Objects;

namespace Business.Pago
{
    public class PagoImplements
    {

        PagosEntities iContext;

        public PagoImplements()
        {
            iContext = new PagosEntities();
        }
        public List<SEL_PROGRAMACION_PAGOS_SP_Result> obtienePagos(int idEmpresa)
        {
            return iContext.SEL_PROGRAMACION_PAGOS_SP(idEmpresa).ToList();
        }

        public List<SEL_PROGRAMACION_PAGOSXVENCER_SP_Result> obtienePagosxvencer(int idEmpresa)
        {
            return iContext.SEL_PROGRAMACION_PAGOSXVENCER_SP(idEmpresa).ToList();
        }

        public List<SEL_PROGRAMACION_PAGOSENROJO_SP_Result> obtienePagosenRojo(int idEmpresa)
        {
            return iContext.SEL_PROGRAMACION_PAGOSENROJO_SP(idEmpresa).ToList();
        }

        public List<SEL_AGRUPADORES_SP_Result> obtieneagrupadores(int idEmpresa)
        {
            return iContext.SEL_AGRUPADORES_SP(idEmpresa).ToList();
        }

        public List<SEL_AGRUPADORES_PROVEEDOR_SP_Result> obtieneprovedores(int idEmpresa)
        {
            return iContext.SEL_AGRUPADORES_PROVEEDOR_SP(idEmpresa).ToList();
        }

        public List<SEL_EMPRESAS_SP_Result> obtenempresas(int idEmpleado)
        {
            return iContext.SEL_EMPRESAS_SP(idEmpleado).ToList();
        }

        public List<SEL_ERRORES_CXP_SP_Result> obtenerrores(int idempresa)
        {
            return iContext.SEL_ERRORES_CXP_SP(idempresa).ToList();
        }


        public List<SEL_TRANSFERENCIASXEMPRESA_Result> obtentransferencias(int idEmpresa)
        {
            return iContext.SEL_TRANSFERENCIASXEMPRESA(idEmpresa).ToList();
        }

        public List<SEL_TOTAL_BANCOS_SP_Result> obtentotalxEmpresa(int idEmpresa)
        {
            return iContext.SEL_TOTAL_BANCOS_SP(idEmpresa).ToList();
        }

        public List<SEL_LIBERAR_CXP_SP_Result> liberadocumento(int idLote)
        {
            return iContext.SEL_LIBERAR_CXP_SP(idLote).ToList();
        }

        public SEL_ENCABEZADO_SP_Result obtieneEncabezado(int idEmpresa)
        {
            return iContext.SEL_ENCABEZADO_SP(idEmpresa).FirstOrDefault();
        }

        /// <summary>
        /// LQMA 08040216 envia solicitud de aprobacion
        /// </summary>
        /// <param name="proc_id"></param>
        /// <param name="nodo"></param>
        /// <param name="emp_idempresa"></param>
        /// <param name="idLote"></param>
        /// <returns></returns>
        public int? enviaAprobacion(int proc_id, int nodo, int emp_idempresa, decimal idLote)
        {
            return iContext.INS_APROBACION_SP(proc_id, nodo, emp_idempresa, idLote).FirstOrDefault();
            //return iContext.SEL_ENCABEZADO_SP(idEmpresa).FirstOrDefault();
        }

        /// <summary>
        /// LQMA 09042016 aprueba lote
        /// </summary>
        /// <param name="proc_id"></param>
        /// <param name="nodo"></param>
        /// <param name="emp_idempresa"></param>
        /// <param name="idLote"></param>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        /// 
        public int? apruebaLote(int proc_id, int nodo, int emp_idempresa, decimal idLote, decimal idUsuario, decimal idAprobador, string Observacion, decimal idAprobacion, decimal not_identificador)
        { //LQMA 11042016
            return iContext.INS_APRUEBA_LOTE_SP(proc_id, nodo, emp_idempresa, idLote, idUsuario, idAprobador, Observacion, idAprobacion, not_identificador).FirstOrDefault();
        }

        public int? aplicaLote(int emp_idempresa, decimal idLote, decimal idUsuario)
        { //LQMA 11042016
            return iContext.INS_APLICA_LOTE_SP(emp_idempresa, idLote, idUsuario).FirstOrDefault();
        }

        public string aplicaLoteDirecto(int emp_idempresa, int idLote, int idUsuario)
        { //LQMA 11042016
            return new InlineInserts().aplicaLoteDirectosql(emp_idempresa, idLote, idUsuario);
        }

        public string borraLoteSQL(int idLote)
        { //LQMA 11042016
            return new InlineInserts().borraLoteSQLbusqueda(idLote);
        }


        public string updTransferencia(int idlote, decimal nmonto)
        {
            return iContext.UPD_TRANSFERENCIASXEMPRESA(idlote, nmonto).FirstOrDefault();
        }

        public string aplTransferencia(int idlote, decimal nmonto)
        {
            return iContext.APL_TRANSFERENCIASXEMPRESA(idlote, nmonto).FirstOrDefault();
        }

        public int? guardaPagos(int idEmpleado, int idPadre, string data, string[] parameters, int operacion, string cadenaBulk)
        {

            var datos = data.Split('|');

            DataTable table = JsonConvert.DeserializeObject<DataTable>(datos[0]); //grid


            //  if (table.Rows.Count == 0)
            //      return 1;


            //string connectionString = "Persist Security Info=False;User ID=sa;Password=S0p0rt3;Initial Catalog=Pagos;Server=192.168.20.41"; //System.Configuration.ConfigurationManager.ConnectionStrings["PagosEntities"].ToString();
            using (SqlConnection connection = new SqlConnection(cadenaBulk))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.PAG_TABLA_PASO_POLIZAS";
                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("pal_id_lote_pago", "pal_id_lote_pago");
                    bulkCopy.ColumnMappings.Add("pad_polTipo", "pad_polTipo");
                    bulkCopy.ColumnMappings.Add("pad_polAnnio", "pad_polAnnio");
                    bulkCopy.ColumnMappings.Add("pad_polMes", "pad_polMes");
                    bulkCopy.ColumnMappings.Add("pad_polConsecutivo", "pad_polConsecutivo");
                    bulkCopy.ColumnMappings.Add("pad_polMovimiento", "pad_polMovimiento");
                    bulkCopy.ColumnMappings.Add("pad_fechaPromesaPago", "pad_fechaPromesaPago");
                    bulkCopy.ColumnMappings.Add("pad_saldo", "pad_saldo");
                    bulkCopy.ColumnMappings.Add("tab_revision", "tab_revision");
                    bulkCopy.ColumnMappings.Add("pad_polReferencia", "pad_polReferencia");
                    bulkCopy.ColumnMappings.Add("pad_documento", "pad_documento");
                    bulkCopy.ColumnMappings.Add("pad_agrupamiento", "pad_agrupamiento");
                    bulkCopy.ColumnMappings.Add("pad_bancoPagador", "pad_bancoPagador");
                    bulkCopy.ColumnMappings.Add("pad_bancoDestino", "pad_bancoDestino");
                    try
                    {
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        var msgerr = ex;
                    }
                }
            }

            var sIngresos = datos[1]; //parameters[3].Replace("\\", "");
            //sIngresos = sIngresos.Substring(1, sIngresos.Length - 2);
            var sTransferencias = datos[2]; //parameters[4].Replace("\\", "");
            //sTransferencias = sTransferencias.Substring(1, sTransferencias.Length - 2);
            var sTotales = datos[3]; //parameters[7].Replace("\\", "");
            //sTotales = sTotales.Substring(1, sTotales.Length - 2);

            XmlDocument xmlIngresos = JsonConvert.DeserializeXmlNode("{\"ingresos\":" + sIngresos + "}", "root");
            XmlDocument xmlTransfer = JsonConvert.DeserializeXmlNode("{\"transferencias\":" + sTransferencias + "}", "root");
            var caja = parameters[3]; //parameters[5];
            var otros = parameters[4]; //parameters[6];
            XmlDocument xmlTotal = JsonConvert.DeserializeXmlNode("{\"totales\":" + sTotales + "}", "root");

            return iContext.INS_PROG_PAGOS_SP(idEmpleado, idPadre, xmlIngresos.InnerXml, xmlTransfer.InnerXml, xmlTotal.InnerXml, Convert.ToDecimal(string.IsNullOrEmpty(caja) ? "0" : caja), Convert.ToDecimal(otros), operacion).FirstOrDefault();
        }

        public int? guardaPagosAutoriza(int idEmpleado, int idPadre, string data, string cadenaBulk)
        {

            var datos = data.Split('|');

            DataTable table = JsonConvert.DeserializeObject<DataTable>(datos[0]); //grid


            //  if (table.Rows.Count == 0)
            //      return 1;


            //string connectionString = "Persist Security Info=False;User ID=sa;Password=S0p0rt3;Initial Catalog=Pagos;Server=192.168.20.41"; //System.Configuration.ConfigurationManager.ConnectionStrings["PagosEntities"].ToString();
            using (SqlConnection connection = new SqlConnection(cadenaBulk))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.PAG_TABLA_PASO_POLIZAS";
                    bulkCopy.ColumnMappings.Clear();
                    bulkCopy.ColumnMappings.Add("pal_id_lote_pago", "pal_id_lote_pago");
                    bulkCopy.ColumnMappings.Add("pad_polTipo", "pad_polTipo");
                    bulkCopy.ColumnMappings.Add("pad_polAnnio", "pad_polAnnio");
                    bulkCopy.ColumnMappings.Add("pad_polMes", "pad_polMes");
                    bulkCopy.ColumnMappings.Add("pad_polConsecutivo", "pad_polConsecutivo");
                    bulkCopy.ColumnMappings.Add("pad_polMovimiento", "pad_polMovimiento");
                    bulkCopy.ColumnMappings.Add("pad_fechaPromesaPago", "pad_fechaPromesaPago");
                    bulkCopy.ColumnMappings.Add("pad_saldo", "pad_saldo");
                    bulkCopy.ColumnMappings.Add("tab_revision", "tab_revision");
                    bulkCopy.ColumnMappings.Add("pad_polReferencia", "pad_polReferencia");
                    bulkCopy.ColumnMappings.Add("pad_documento", "pad_documento");
                    bulkCopy.ColumnMappings.Add("pad_agrupamiento", "pad_agrupamiento");
                    bulkCopy.ColumnMappings.Add("pad_bancoPagador", "pad_bancoPagador");
                    bulkCopy.ColumnMappings.Add("pad_bancoDestino", "pad_bancoDestino");
                    try
                    {
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        var msgerr = ex;
                    }
                }
            }
            return iContext.INS_PROG_PAGOS_AUTORIZA_SP(idEmpleado, idPadre).FirstOrDefault();
        }


        public decimal? insertaPagosPadre(int idEmpresa, int idUsuario, string nombreLote, short esDirecto, decimal cifraControl)
        {
            return iContext.INS_PAG_PROG_PAGOS_SP(idEmpresa, idUsuario, nombreLote, (short)1, esDirecto, cifraControl).FirstOrDefault();
        }

        public List<SEL_PROG_PAGOS_GUARDADA_SP_Result> obtienePagosGuardados(int idPadre)
        {
            return iContext.SEL_PROG_PAGOS_GUARDADA_SP(idPadre).ToList();
        }

        /*****LQMA consulta datos de ingresos y gresos*******/
        public List<SEL_CUENTAS_INGRESOS_SP_Result> obtieneIngresos(decimal idEmpresa, decimal numLote)
        {
            return iContext.SEL_CUENTAS_INGRESOS_SP(idEmpresa, numLote).ToList();
        }
        public List<SEL_CUENTAS_EGRESOS_SP_Result> obtieneEgresos(int idEmpresa, int numLote)
        {
            return iContext.SEL_CUENTAS_EGRESOS_SP(idEmpresa, numLote).ToList();
        }
        public List<SEL_LOTE_SP_Result> obtieneLotes(int idEmpresa, int idUsuario, int borraLotes, int idLote)
        {
            return iContext.SEL_LOTE_SP(idEmpresa, idUsuario, borraLotes, idLote).ToList();
        }

        public List<SEL_LOTESXFECHA_SP_Result> obtieneLotesxFecha(int idEmpresa, int idUsuario, string fechaini, string fechafin, int estatus)
        {
            return iContext.SEL_LOTESXFECHA_SP(idEmpresa, idUsuario, fechaini, fechafin, estatus).ToList();
        }

        public List<SEL_TRANSFERENCIASXEMPRESAXFECHA_Result> obtienetransferxFecha(int idEmpresa, string fechaini, string fechafin)
        {
            return iContext.SEL_TRANSFERENCIASXEMPRESAXFECHA(idEmpresa, fechaini, fechafin).ToList();
        }
        //public List<SEL_LOTE_ADMIN_SP_Result> obtieneLotesxestatus(int idEmpresa, int idEstatus)
        //{
        //    return iContext.SEL_LOTE_ADMIN_SP(idEmpresa, idEstatus).ToList();
        //}

        public List<SEL_CUENTAS_TRANSFERENCIAS_SP_Result> obtieneTransferencias(int idLote)
        {
            return iContext.SEL_CUENTAS_TRANSFERENCIAS_SP(idLote).ToList();
        }

        public List<SEL_USUARIO_TRANSFERENCIA_Result> obtieneUsuarioTransferencia(int idUser)
        {
            return iContext.SEL_USUARIO_TRANSFERENCIA(idUser).ToList();
        }

        public List<SEL_INGRESO_OTROS_SP_Result> obtieneOtrosIngresos(int idLote)
        {
            return iContext.SEL_INGRESO_OTROS_SP(idLote).ToList();
        }

        /***********/
        //FAL store que borra los agrupadores de una empresa
        public int? borraAgrupadoresPorveedoresxEmpresa(int idEmpresa)
        {
            return iContext.DEL_AGRUPADOR_PROVEEDOR_SP(idEmpresa).FirstOrDefault();
        }
        //FAL Inserta los agrupadores con sus provedores en el orden indicado
        public int? insertaAgrupadores(int idAgrupador, int idProveddor, int orden)
        {
            return iContext.INS_AGRUPADOR_PROVEEDOR_SP(idAgrupador, idProveddor, orden).FirstOrDefault();
        }

        //FAL trae cada uno de los agrupadores
        public decimal? guardaAgrupadores(int idEmpresa, string data)
        {
            dynamic dynAgrupadores = JsonConvert.DeserializeObject(data);
            dynamic dynAgrupador = "";
            int i = 0;
            int p = 0;
            foreach (var item in dynAgrupadores)
            {
                p = guardaAgrupador(Convert.ToInt32(item.pca_idAgrupador), Convert.ToString(item.lista), Convert.ToInt32(i));
                i++;
            }
            return i;
        }
        //FAL llena cada lista
        public int guardaAgrupador(int idEmpresa, string data, int numLista)
        {
            dynamic dynAgrupador = JsonConvert.DeserializeObject(data);
            int orden = 1;
            string datos = "";
            int z = 0;
            foreach (var item in dynAgrupador)
            {
                datos = item.value;
                var elementos = datos.Split('-');
                z = Convert.ToInt32(insertaAgrupadores(Convert.ToInt32(numLista), Convert.ToInt32(elementos[1]), Convert.ToInt32(orden)));
                orden++;
            }
            return 0;
        }
        //FAL TRAE LA LISTA DE PROVEDORES CON SUS CUENTAS PAGADORAS
        public List<SEL_PROVEEDORES_CTA_PAGADORA_SP_Result> obtenproveedoresCuenta(int idEmpresa)
        {
            return iContext.SEL_PROVEEDORES_CTA_PAGADORA_SP(idEmpresa).ToList();
        }
        //FAL TRAE LAS CUENTAS pagadoras
        public List<SEL_CUENTAS_EGRESOS_SP_Result> obtenCuentasEgresos(int idEmpresa)
        {
            return iContext.SEL_CUENTAS_EGRESOS_SP(idEmpresa, 0).ToList();
        }

        //FAL BORRA LOS PROVEEDORES RELACIONADOS CON LAS CUENTAS PAGADORAS
        public int? borraCuentasPorveedoresxEmpresa(int idEmpresa, int idProveedor, string Cuenta)
        {
            var j = iContext.DEL_PROVEEDOR_CUENTA_SP(Convert.ToInt32(idEmpresa), Convert.ToInt32(idProveedor), Convert.ToString(Cuenta)).FirstOrDefault();
            return 0;
        }
        //FAL GUARDA CADA UNA DE LAS CUENTAS
        public decimal? guardaCuentas(int idEmpresa, string data)
        {
            dynamic dynAgrupadores = JsonConvert.DeserializeObject(data);
            dynamic dynAgrupador = "";
            int i = 0;
            int p = 0;
            foreach (var item in dynAgrupadores)
            {
                p = guardaProvedorCuentas(Convert.ToInt32(idEmpresa), Convert.ToString(item.lista), Convert.ToString(item.cuenta));
                i++;
            }
            return i;
        }
        //FAL LLENA LAS CUENTAS CON LOS PROVEEDORES
        public int guardaProvedorCuentas(int idEmpresa, string data, string NumCuenta)
        {
            dynamic dynAgrupador = JsonConvert.DeserializeObject(data);
            int orden = 1;
            string datos = "";

            foreach (var item in dynAgrupador)
            {
                datos = item.value;
                var elementos = datos.Split('-');
                var z = borraCuentasPorveedoresxEmpresa(Convert.ToInt32(idEmpresa), Convert.ToInt32(elementos[0]), Convert.ToString(NumCuenta));
                orden++;
            }
            return 0;
        }

        //FAL TRAE LOS BANCOS COMPLETOS
        public List<SEL_TOTAL_BANCOS_P_NP_SP_Result> obtenBancosCompleta(int idEmpresa)
        {
            return iContext.SEL_TOTAL_BANCOS_P_NP_SP(idEmpresa).ToList();
        }

        //FAL TRAE EL LOTE CON LOS CAMPOS NECESARIOS PARA LOS LAYOUTS
        public List<SEL_PROG_PAGOS_TXT_SP_Result> ObtenLayoutBancomerBancomer(int idLote)
        {
            return iContext.SEL_PROG_PAGOS_TXT_SP(idLote).ToList();
        }
        //FAL GENERA ARCHIVO.
        public string generaArchivoZip(int idEmpresa, string folderName, int idPadreLote, string folderNameDestino)
        {
            PagoImplements iPago;
            iPago = new PagoImplements();

            var dataSQL = iPago.ObtenLayoutBancomerBancomer(Convert.ToInt32(idPadreLote));

            int lote = dataSQL[0].idPadre ?? -1;
            if (idPadreLote > 0)
            {
                lote = idPadreLote;
            }
            if (lote > 0)
            {

                DateTime dtPago = DateTime.Today;
                string strPago = String.Format("{0:ddMMyy}", dtPago);
                string folderzip = folderName + "\\" + strPago;
                string nomArchivoZip = dataSQL[0].nombreLote + ".zip";
                string nomCompletozip = folderName + "\\" + nomArchivoZip;
                string nomArchivoBanco = "";

                if (System.IO.File.Exists(nomCompletozip))
                {
                    File.Delete(nomCompletozip);
                }

                System.IO.Directory.CreateDirectory(folderzip);
                {
                    var text = "";
                    nomArchivoBanco = strPago + lote + ("BANCOMER").Trim() + ".txt";
                    string fileName = System.IO.Path.Combine(folderzip, nomArchivoBanco);

                    foreach (var item in dataSQL)
                    {
                        /*
                        var text = "";
                        nomArchivoBanco = strPago + lote + (Convert.ToString(item.nombreBancoOrigen)).Trim() + ".txt";
                        string fileName = System.IO.Path.Combine(folderzip, nomArchivoBanco);
                        */
                        if (!System.IO.File.Exists(fileName))
                        {


                            // using (System.IO.FileStream file = System.IO.File.Create(@fileName))
                            //file.Close();
                            if (item.convenioCie != "")
                            {

                                text = BancomerCIE(item);
                            }
                            else
                            {
                                if (item.numBancoDestino == "012")
                                {
                                    text = BancomerCuentas(item);

                                }
                                else
                                {
                                    text = BancomerClabe(item);

                                }
                            }

                            //switch (item.numBancoOrigen)
                            //    {
                            //        case "012":
                            //            if (item.numBancoDestino == "012")
                            //            {
                            //                text = BancomerCuentas(item);
                            //                break;
                            //            }
                            //            else
                            //            {
                            //                text = BancomerClabe(item);
                            //                break;
                            //            }
                            //        case "002":
                            //            if (item.numBancoDestino == "002")
                            //            {

                            //                text = BancomerCuentas(item);
                            //                break;
                            //            }
                            //            else
                            //            {
                            //                text = BancomerCuentas(item);
                            //                break;
                            //            }

                            //        default:
                            //            text = BancomerCuentas(item);
                            //            break;
                            //    }

                            //System.IO.StreamWriter WriteReportFile = System.IO.File.AppendText(@fileName);
                            //WriteReportFile.WriteLine(text);
                            //WriteReportFile.Close();

                            using (TextWriter tw = new StreamWriter(@fileName, true, Encoding.GetEncoding(1252)))
                                tw.WriteLine(text);


                        }
                        else
                        {

                            if (item.convenioCie != "")
                            {

                                text = BancomerCIE(item);
                            }
                            else
                            {
                                if (item.numBancoDestino == "012")
                                {
                                    text = BancomerCuentas(item);

                                }
                                else
                                {
                                    text = BancomerClabe(item);

                                }
                            }
                            //switch (item.numBancoOrigen)
                            //{
                            //    case "012":
                            //        if (item.numBancoDestino == "012")
                            //        {
                            //            text = BancomerCuentas(item);
                            //            break;
                            //        }
                            //        else
                            //        {
                            //            text = BancomerClabe(item);
                            //            break;
                            //        }

                            //    case "002":
                            //        if (item.numBancoDestino == "002")
                            //        {
                            //            text = BancomerCuentas(item);
                            //            break;
                            //        }
                            //        else
                            //        {
                            //            text = BancomerCuentas(item);
                            //            break;
                            //        }

                            //    default:
                            //        text = BancomerCuentas(item);
                            //        break;
                            //}
                            //System.IO.StreamWriter WriteReportFile = System.IO.File.AppendText(@fileName);
                            //WriteReportFile.WriteLine(text);
                            //WriteReportFile.Close();

                            using (TextWriter tw = new StreamWriter(@fileName, true, Encoding.GetEncoding(1252)))
                                tw.WriteLine(text);

                        }
                    }
                }
                string[] txtList = Directory.GetFiles(folderzip, "*.txt");
                ZipFile.CreateFromDirectory(folderzip, nomCompletozip);

                //FAL borra archivos para que no se sobreescriban.
                foreach (string f in txtList)
                {
                    File.Delete(f);
                }
                File.Copy(Path.Combine(folderName, nomArchivoZip), Path.Combine(folderNameDestino, nomArchivoZip), true);
                return nomArchivoZip;
            }
            else
            {
                return "sin archivo";
            }

        }

        public string generaArchivoBanamexZip(int idEmpresa, string folderName, int idPadreLote, string folderNameDestino, string path)
        {
            PagoImplements iPago;
            iPago = new PagoImplements();

            var dataSQL = iPago.ObtenLayoutBanamex(Convert.ToInt32(idPadreLote));

            //int lote = dataSQL[0].idPadre ?? -1;
            //if (idPadreLote > 0)
            //{
            //    lote = idPadreLote;
            //}
            if (dataSQL.Count > 0)
            {

                DateTime dtPago = DateTime.Today;
                string strPago = String.Format("{0:ddMMyy}", dtPago);
                string folderzip = folderName + "\\" + strPago;
                string nomArchivoZip = iPago.ObtenNombreLote(idPadreLote).FirstOrDefault() + ".zip";
                string nomCompletozip = folderName + "\\" + nomArchivoZip;
                string nomArchivoBanco = "";

                if (System.IO.File.Exists(nomCompletozip))
                {
                    File.Delete(nomCompletozip);
                }

                System.IO.Directory.CreateDirectory(folderzip);
                {
                    var text = "";
                    nomArchivoBanco = ("BanamexPago").Trim() + strPago + idPadreLote + ".txt";
                    string fileName = System.IO.Path.Combine(folderzip, nomArchivoBanco);

                    foreach (var item in dataSQL)
                    {

                        if (!System.IO.File.Exists(fileName))
                        {

                            text = item;

                            using (TextWriter tw = new StreamWriter(@fileName, true, Encoding.GetEncoding(1252)))
                                tw.WriteLine(text);
                        }
                        else
                        {

                            text = item;

                            using (TextWriter tw = new StreamWriter(@fileName, true, Encoding.GetEncoding(1252)))
                                tw.WriteLine(text);
                        }
                    }
                }
                Sftp mySftp = new Sftp();
                //mySftp.host = "192.168.20.111";
                //mySftp.password = "B4n4m3x";
                //mySftp.port = 22;
                //mySftp.uploadfile = Path.Combine(folderzip, nomArchivoBanco);
                //mySftp.username = "sftpbnmx";
                //mySftp.workingdirectory = "/upload";

                //Business.Properties.Settings.

                SEL_FORMATO_SFTP_Result FTPValues = new SEL_FORMATO_SFTP_Result();
                FTPValues = this.GetSFTPValores(2);//id Banamex

                mySftp.host = FTPValues.FTPServer;

                mySftp.username = FTPValues.FTPUser;
                
                mySftp.password = FTPValues.FTPPassword;

                mySftp.port =FTPValues.FTPPort.Value ; //Properties.Settings.Default.PortBanamex;

                mySftp.uploadfile = Path.Combine(folderzip, nomArchivoBanco);

                mySftp.workingdirectory = FTPValues.FTPWorkFolder;

                mySftp.subirArchivoRutaFisica();

                string[] txtList = Directory.GetFiles(folderzip, "*.txt");
                ZipFile.CreateFromDirectory(folderzip, nomCompletozip);

                //FAL borra archivos para que no se sobreescriban.
                foreach (string f in txtList)
                {
                    File.Delete(f);
                }

                if(Directory.Exists(folderNameDestino))
                    File.Copy(Path.Combine(folderName, nomArchivoZip), Path.Combine(folderNameDestino, nomArchivoZip), true);
                else
                {
                    Directory.CreateDirectory(folderNameDestino);
                    File.Copy(Path.Combine(folderName, nomArchivoZip), Path.Combine(folderNameDestino, nomArchivoZip), true);
                }

                string monthName = DateTime.Now.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es")).Substring(0, 3).ToUpper();
                string year = DateTime.Now.ToString("yyyy");
                string dirName = path + year + monthName + "\\";

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);
                else
                {
                    File.Copy(Path.Combine(folderName, nomArchivoZip), Path.Combine(dirName, nomArchivoZip), true);
                }


                return nomArchivoZip;
            }
            else
            {
                return "sin archivo";
            }

        }

        //FAL Funcion para rellenar la cadena por la izquierda o por la derecha de acuerdo al layout
        public string rellenaCadena(int lugares, string cadena, string relleno, bool left)
        {
            string newCadena = cadena;
            int tamCadena = cadena.Length;
            int repite = lugares - tamCadena;
            for (int i = 1; i <= repite; i++)
            {
                if (left) { newCadena = relleno + newCadena; } else { newCadena = newCadena + relleno; }
            }
            return newCadena;
        }
        //FAL funcion que formatea  la fecha de acuerdo al layout
        public string fechaFormato(string fecha)
        {
            string miFecha = fecha;
            char[] MyChar = { '/', ' ' };
            string newFecha = miFecha.TrimEnd(MyChar);
            return newFecha;
        }
        //FAL devuelve un renglon en el formato del layout de cuentas a tercero bancomer
        public string BancomerCuentas(dynamic registro)
        {
            string strPrefijo = "PTC";
            string strcuentaAbono = registro.numCtaOrigen == null ? "".PadLeft(18, '0') : registro.numCtaOrigen.ToString().PadLeft(18, '0');
            string strcuentaCargo = registro.numCtaDestino == null ? "".PadLeft(18, '0') : registro.numCtaDestino.ToString().PadLeft(18, '0');
            string strDivisa = "MXP";
            string strMonto = registro.importeTotal == null ? "".PadLeft(16, '0') : registro.importeTotal.ToString().PadLeft(16, '0');
            string strMotivo = registro.referencia == null ? "".PadRight(30, ' ') : registro.referencia.ToString().PadRight(30, ' ');
            return strPrefijo + strcuentaCargo + strcuentaAbono + strDivisa + strMonto.Replace(',', '.') + strMotivo;

        }
        //FAL devuelve un renglon en el formato del layout de cuentas Clabe bancomer
        public string BancomerClabe(dynamic registro)
        {
            string strPrefijo = "PSC";
            string strcuentaAbono = registro.numCtaOrigen == null ? "".PadLeft(18, '0') : registro.numCtaOrigen.ToString().PadLeft(18, '0');
            string strcuentaCargo = registro.numCtaClabeDestino == null ? "".PadLeft(18, '0') : registro.numCtaClabeDestino.ToString().PadLeft(18, '0');
            string strDivisa = "MXP";
            string strMonto = registro.importeTotal == null ? "".PadLeft(16, '0') : registro.importeTotal.ToString().PadLeft(16, '0');
            string idProvedor = registro.idProveedor == null ? "".ToString() : registro.idProveedor.ToString();
            string nombreProvedor = registro.proveedor == null ? "" : registro.proveedor;
            string nombrecompuesto = idProvedor + nombreProvedor;
            nombrecompuesto = (nombrecompuesto.PadRight(30, ' ')).Substring(0, 30);
            string strTipoCuenta = registro.tipoCtaDestino == null ? "".PadLeft(2, '0') : registro.tipoCtaDestino.PadLeft(2, '0');
            string strNumBanco = registro.numBancoDestino == null ? "".PadLeft(3, '0') : registro.numBancoDestino.PadLeft(3, '0');
            string strMotivo = registro.referencia == null ? "".PadRight(30, ' ') : registro.referencia.ToString().PadRight(30, ' ');
            string strRefNum = "1234567";
            string strDisponibilidad = "H";


            return strPrefijo + strcuentaCargo + strcuentaAbono + strDivisa + strMonto.Replace(',', '.') + nombrecompuesto +
                strTipoCuenta + strNumBanco + strMotivo + strRefNum + strDisponibilidad;
        }
        //FAL devuelve un renglon en el formato del layout de cuentas Clabe bancomer
        public string BancomerCIE(dynamic registro)
        {
            //string strPrefijo = "CIL" + registro.numRefDestino.ToString();
            //string txtconceptoCie = strPrefijo.PadRight(33, ' ');
            //string txtconveninioCIE = registro.convenioCie == null ? "".PadLeft(7, '0') : registro.convenioCie.ToString().PadLeft(7, '0');
            //string txtasuntoOrdenante = registro.numCtaOrigen == null ? "".PadLeft(18, '0') : registro.numCtaOrigen.ToString().PadLeft(18, '0');
            //string txtimporte = registro.importeTotal == null ? "".PadLeft(16, '0') : registro.importeTotal.ToString().PadLeft(16, '0');
            //string txtMotivoPago = registro.numRefDestino == null ? "".PadRight(30, ' ') : registro.numRefDestino.ToString().PadRight(30, ' '); ;
            //string txtReferenciaCIE = registro.numRefDestino == null ? "".PadRight(20, ' ') : registro.numRefDestino.ToString().PadRight(20, ' ');
            //return txtconceptoCie + txtconveninioCIE + txtasuntoOrdenante + txtimporte.Replace(',', '.') + txtMotivoPago + txtReferenciaCIE.Substring(0, 20);
            string strPrefijo = "CIL" + registro.referencia.ToString();
            string txtconceptoCie = strPrefijo.PadRight(33, ' ');
            string txtconveninioCIE = registro.convenioCie == null ? "".PadLeft(7, '0') : registro.convenioCie.ToString().PadLeft(7, '0');
            string txtasuntoOrdenante = registro.numCtaOrigen == null ? "".PadLeft(18, '0') : registro.numCtaOrigen.ToString().PadLeft(18, '0');
            string txtimporte = registro.importeTotal == null ? "".PadLeft(16, '0') : registro.importeTotal.ToString().PadLeft(16, '0');
            string txtMotivoPago = registro.referencia == null ? "".PadRight(30, ' ') : registro.referencia.ToString().PadRight(30, ' '); ;
            string txtReferenciaCIE = registro.referencia == null ? "".PadRight(20, ' ') : registro.referencia.ToString().PadRight(20, ' ');
            return txtconceptoCie + txtconveninioCIE + txtasuntoOrdenante + txtimporte.Replace(',', '.') + txtMotivoPago + txtReferenciaCIE.Substring(0, 20);


        }
        //FAL devuelve un renglon en el formato del layout de cuentas a terceros banamex
        public string BanamexCuentas(dynamic registro)
        {
            string strTipoTrans = "03";
            string strTipoCuentaOrigen = "03";
            string strSucursalCuentaOrigen = rellenaCadena(4, "001", "0", true);
            string strcuentaOrigen = rellenaCadena(20, Convert.ToString(registro.cuenta), "0", true);
            string strTipoCuentaDestino = "03";
            string strSucursalCuentaDestino = rellenaCadena(4, "001", "0", true);
            string strcuentaDestino = rellenaCadena(20, Convert.ToString(registro.cuenta), "0", true);
            decimal decSaldo = decimal.Round(Convert.ToDecimal(registro.saldo), 2);
            string strMonto = rellenaCadena(16, Convert.ToString(decSaldo), "0", true);
            string tipoMoneda = "001";
            string strdescripcion = "PAGO A DOCUMENTO: " + Convert.ToString(registro.documento);
            strdescripcion = rellenaCadena(24, strdescripcion, "0", true);
            string strConcepto = Convert.ToString(registro.referencia);
            strConcepto = rellenaCadena(34, strConcepto, "0", true);
            string strReferencia = "1234567891";
            string strMoneda = "000";
            DateTime dtPago = DateTime.Today;
            string strPago = String.Format("{0:ddMMyyhhmm}", dtPago);
            return strTipoTrans + strTipoCuentaOrigen + strSucursalCuentaOrigen + strcuentaOrigen +
                strTipoCuentaDestino + strSucursalCuentaDestino + strcuentaDestino + strMonto + tipoMoneda +
                strdescripcion + strConcepto + strReferencia + strMoneda + strPago;
        }
        //FAL devuelve un renglon en el formato del layout de cuentas a Clabe banamex
        public string BanamexClabe(dynamic registro)
        {
            string strTipoTrans = "09";
            string strTipoCuentaOrigen = "03";
            string strSucursalCuentaOrigen = rellenaCadena(4, "001", "0", true);
            string strcuentaOrigen = rellenaCadena(20, Convert.ToString(registro.cuenta), "0", true);
            decimal decSaldo = decimal.Round(Convert.ToDecimal(registro.saldo), 2);
            string strMonto = rellenaCadena(16, Convert.ToString(decSaldo), "0", true);
            string tipoMoneda = "001";
            string strTipoCuentaDestino = "03";
            string strSucursalCuentaDestino = rellenaCadena(4, "001", "0", true);
            string strcuentaDestino = rellenaCadena(20, Convert.ToString(registro.cuenta), "0", true);
            string strdescripcion = "PAGO A DOCUMENTO: " + Convert.ToString(registro.documento);
            strdescripcion = rellenaCadena(24, strdescripcion, "0", true);
            string strConcepto = Convert.ToString(registro.referencia);
            strConcepto = rellenaCadena(34, strConcepto, "0", true);
            string strReferencia = "1234567891";
            string strMoneda = "000";
            DateTime dtPago = DateTime.Today;
            string strPago = String.Format("{0:ddMMyyhhmm}", dtPago);
            return strTipoTrans + strTipoCuentaOrigen + strSucursalCuentaOrigen + strcuentaOrigen +
                strMonto + tipoMoneda + strTipoCuentaDestino + strSucursalCuentaDestino + strcuentaDestino +
                strdescripcion + strConcepto + strReferencia + strMoneda + strPago;
        }

        public string Left(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength ? value : value.Substring(0, maxLength));
        }

        public SEL_ESCENARIO_SP_Result obtenParametrosEscenarios(int idEmpresa)
        {
            return iContext.SEL_ESCENARIO_SP(idEmpresa).FirstOrDefault();
        }

        public SEL_TRANSFERENCIASXLOTE_Result selTrasnferenciaxlote(int idlote)
        {
            return iContext.SEL_TRANSFERENCIASXLOTE(idlote).FirstOrDefault();
        }

        public string UpdateCartera(int idEmpresa)
        {
            return new InlineInserts().ActualizaCartera(idEmpresa);
        }

        public string liberaDocumento(int idlote, string documento)
        {
            return new InlineInserts().LiberaDocument(idlote, documento);
        }

        public string InsertarBitacoraPrograPagos(string numeroTransaccion, int idLotePago)
        {
            return iContext.INS_BITACORA_POGRA_PAGOS_DET(numeroTransaccion, idLotePago).FirstOrDefault();
        }

       
        public string procesarLogBanamex(string carpeta, string carpetaProcesados, string mail1, string mail2)
        {
            try
            {

                Console.WriteLine("Comenzamos a analizar archivos");
                //string carpeta = ConfigurationManager.AppSettings["RutaArchivosLog"];
                //string carpetaProcesados = ConfigurationManager.AppSettings["RutaArchivosLogProcesados"];

                string[] archivos = null;
                archivos = Directory.GetFiles(carpeta);
                StreamReader lector = null;
                StreamReader segundoStream = null;

                foreach (string archivo in archivos)
                {

                    bool existeError = false;
                    List<string> elementosErroneos = new List<string>();
                    List<string> elementosCorrectos = new List<string>();
                    string loteId = "";

                    try
                    {

                        lector = new StreamReader(archivo);
                        string sLine = "";
                        string sLine2 = "";
                        //string trans = null;
                        int i = 0;

                        Console.WriteLine("Leyendo archivo: " + archivo);

                        while (sLine != null)
                        {
                            sLine = lector.ReadLine();

                            if (sLine == null)
                                break;
                            sLine = sLine.Replace("\t", "");
                            if (sLine != "")
                            {
                                if (sLine.Substring(0, sLine.IndexOf(" : ")) == "Transaction Reference Number")
                                {
                                    i = i + 1;
                                }
                            }


                        }
                        lector.Close();

                        segundoStream = new StreamReader(archivo);

                        Console.WriteLine("Ingresando en base de datos: ");


                        List<PAG_LogBanamex> logBan = new List<PAG_LogBanamex>();
                        int j = 0;

                        while (sLine2 != null)
                        {
                            sLine2 = segundoStream.ReadLine();

                            if (sLine2 != null)
                            {


                                sLine2 = sLine2.Replace("\t", "");
                                if (sLine2 != "")
                                {
                                    if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Profile Name")
                                    {
                                        for (int k = 0; k < i; k++)
                                        {
                                            PAG_LogBanamex log = new PAG_LogBanamex();
                                            log.ProfileName = sLine2.Substring(sLine2.IndexOf(" : ") + 3);
                                            logBan.Add(log);
                                        }
                                    }

                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Profile Description")
                                    {
                                        for (int k = 0; k < i; k++)
                                        {
                                            logBan[k].ProfileDescripcion = sLine2.Substring(sLine2.IndexOf(" : ") + 3);
                                        }
                                    }

                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Run ID")
                                    {
                                        for (int k = 0; k < i; k++)
                                        {
                                            logBan[k].RunID = int.Parse(sLine2.Substring(sLine2.IndexOf(" : ") + 3));
                                        }
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Run Date/Time")
                                    {
                                        for (int k = 0; k < i; k++)
                                        {
                                            logBan[k].RunDate = DateTime.Parse(sLine2.Substring(sLine2.IndexOf(" : ") + 3));
                                        }
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "File Name")
                                    {
                                        loteId = sLine2.Substring(sLine2.IndexOf(" : ") + 3);

                                        char rango = '.';
                                        int range = loteId.IndexOf(rango);


                                        loteId = loteId.Substring(17, range - 17);


                                        for (int k = 0; k < i; k++)
                                        {
                                            logBan[k].SourceFileName = sLine2.Substring(sLine2.IndexOf(" : ") + 3);
                                            logBan[k].DestinationFileName = archivo.Substring(archivo.LastIndexOf("\\") + 1);
                                        }
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Transaction Reference Number")
                                    {

                                        logBan[j].NumeroReferenciadeTransaccion = sLine2.Substring(sLine2.IndexOf(" : ") + 3);

                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Beneficiary or Debit Party Name")
                                    {
                                        logBan[j].Beneficiario = sLine2.Substring(sLine2.IndexOf(" : ") + 3);
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Payment Amount")
                                    {
                                        logBan[j].MontoPago = decimal.Parse(sLine2.Substring(sLine2.IndexOf(" : ") + 3));
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Value Date")
                                    {
                                        logBan[j].ValueDate = DateTime.ParseExact(sLine2.Substring(sLine2.IndexOf(" : ") + 3, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                        int result = InsertarCorrectos(logBan[j]);
                                        List<SEL_PAGOS_DETALLES_BITACORA_Result> detalles = this.getDetallesBitacoraPagos(logBan[j].NumeroReferenciadeTransaccion, Int32.Parse(logBan[j].NumeroReferenciadeTransaccion.Substring(0, logBan[j].NumeroReferenciadeTransaccion.IndexOf("-"))), result);

                                        string detallesBitacoraPagos = "";

                                        foreach (SEL_PAGOS_DETALLES_BITACORA_Result detalleBitacoraResult in detalles)
                                        {
                                            detallesBitacoraPagos = detallesBitacoraPagos + "<p> Proveedor: " + detalleBitacoraResult.pad_proveedor + " </p>" 
                                                + "<p> Documento: " + detalleBitacoraResult.pad_documento + " </p>";
                                        }

                                        elementosCorrectos.Add("<p>No.Lote: " + logBan[j].NumeroReferenciadeTransaccion.Substring(0, logBan[j].NumeroReferenciadeTransaccion.IndexOf("-")) + "</p><p> No. de proveedor: "
                                            + logBan[j].NumeroReferenciadeTransaccion.Substring(logBan[j].NumeroReferenciadeTransaccion.IndexOf("-") + 1, logBan[j].NumeroReferenciadeTransaccion.LastIndexOf("-") - logBan[j].NumeroReferenciadeTransaccion.IndexOf("-") - 1)
                                            + "</p><p> Consecutivo: " + logBan[j].NumeroReferenciadeTransaccion.Substring(logBan[j].NumeroReferenciadeTransaccion.LastIndexOf("-") + 1) + "</p>"
                                            + detallesBitacoraPagos);                                         
                                        j = j + 1;
                                    }
                                    else if (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Reject Reason" || sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Error Detail")
                                    {
                                        logBan[j].RazonRechazo = sLine2.Substring(sLine2.IndexOf(" : ") + 3);
                                        int result = InsertarIncorrectos(logBan[j], (sLine2.Substring(0, sLine2.IndexOf(" : ")) == "Reject Reason") ? 2 : 3);
                                        string enviar = "";

                                        string detallesBitacoraPagos = "";

                                        string sourceName = logBan[j].SourceFileName;
                                        char rango = '.';
                                        int range = sourceName.IndexOf(rango);


                                        sourceName = sourceName.Substring(17, range - 17);

                                        List<SEL_PAGOS_DETALLES_BITACORA_Result> detalles = this.getDetallesBitacoraPagos(logBan[j].NumeroReferenciadeTransaccion, Int32.Parse( sourceName) , result);
                                        
                                        


                                        foreach (SEL_PAGOS_DETALLES_BITACORA_Result detalleBitacoraResult in detalles)
                                        {
                                            detallesBitacoraPagos = detallesBitacoraPagos + "<p> Proveedor: " + detalleBitacoraResult.pad_proveedor + " </p>"
                                                + "<p> Documento: " + detalleBitacoraResult.pad_documento + " </p>"
                                                 + "<p> Lote: " + detalleBitacoraResult.pal_id_lote_pago + " </p>"
                                                  + "<p> fecha y hora: " + logBan[j].RunDate + " </p>"
                                                   + "<p> Id Persona: " + detalleBitacoraResult.pad_idProveedor + " </p>";
                                        }



                                        if (logBan[j].NumeroReferenciadeTransaccion.IndexOf("-") == -1)
                                        {
                                            enviar = detallesBitacoraPagos + "<p> " + logBan[j].RazonRechazo + "</p>";
                                        }
                                        else
                                        {
                                            enviar =

                                            /*"<p>No.Lote: " + logBan[j].NumeroReferenciadeTransaccion.Substring(0, logBan[j].NumeroReferenciadeTransaccion.IndexOf("-")) + "</p><p> No. de proveedor: "
                                        + logBan[j].NumeroReferenciadeTransaccion.Substring(logBan[j].NumeroReferenciadeTransaccion.IndexOf("-") + 1, logBan[j].NumeroReferenciadeTransaccion.LastIndexOf("-") - logBan[j].NumeroReferenciadeTransaccion.IndexOf("-") - 1)
                                        + "</p><p> Consecutivo: " + logBan[j].NumeroReferenciadeTransaccion.Substring(logBan[j].NumeroReferenciadeTransaccion.LastIndexOf("-") + 1) + " </p><p> " + logBan[j].RazonRechazo + "</p>"
                                        */
                                            detallesBitacoraPagos + "<p> " + logBan[j].RazonRechazo + "</p>";
                                            
                                        }
                                        existeError = true;
                                        elementosErroneos.Add(enviar);
                                        j = j + 1;
                                    }
                                }
                            }
                        }

                        segundoStream.Close();


                    }

                    catch (System.Exception e)
                    {
                        return e.Message.ToString();

                    }
                    finally
                    {
                        lector.Close();
                        segundoStream.Close();
                    }

                    if (existeError)
                    {
                        EnviarCorreoElectronico(1, elementosErroneos, loteId, mail1, mail2);
                        if (elementosCorrectos.Count() > 0)
                            EnviarCorreoElectronico(2, elementosCorrectos, loteId, mail1, mail2);
                    }
                    else
                        EnviarCorreoElectronico(2, elementosCorrectos, loteId, mail1, mail2);

                    if (System.IO.File.Exists(carpetaProcesados + archivo.Substring(archivo.LastIndexOf("\\"))))
                    {
                        System.IO.File.Delete(carpetaProcesados + archivo.Substring(archivo.LastIndexOf("\\")));
                    }
                        


                    System.IO.File.Move(archivo, carpetaProcesados + archivo.Substring(archivo.LastIndexOf("\\")));



                }


                return "exitoso";
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: " + e.ToString());
                System.Threading.Thread.Sleep(1000);
                return e.ToString();
            }
        }

        public int InsertarCorrectos(PAG_LogBanamex Log)
        {
            string conexion = ConfigurationManager.AppSettings["cadenaBulkcopyReferencias"];

            SqlConnection sourceConnection = new SqlConnection(conexion);
            sourceConnection.Open();
            SqlCommand sql = new SqlCommand();
            SqlCommand existSql = new SqlCommand();
            int totalRows = 0;

            sql.Connection = sourceConnection;
            existSql.Connection = sourceConnection;

            existSql.CommandText = "Select count(1) from dbo.LogBanamex where SourceFileName =@SourceFileName and NumeroReferenciadeTransaccion =@NumeroReferenciadeTransaccion ";
            existSql.Parameters.AddWithValue("@SourceFileName", Log.SourceFileName);
            existSql.Parameters.AddWithValue("@NumeroReferenciadeTransaccion", Log.NumeroReferenciadeTransaccion);

            SqlDataReader TotalLogs = existSql.ExecuteReader();

            if (TotalLogs.HasRows)
            {
                TotalLogs.Read();
                totalRows = TotalLogs.GetInt32(0);
                TotalLogs.Close();
            }

            if (totalRows == 0)
            {

                sql.CommandText = "insert into dbo.LogBanamex (ProfileName, ProfileDescripcion,RunID, RunDate, SourceFileName, DestinationFileName, EstatusDeTransaccion, NumeroReferenciadeTransaccion, Beneficiario, MontoPago ,ValueDate) " +
                    " values (@ProfileName, @ProfileDescripcion, @RunID, @RunDate, @SourceFileName, @DestinationFileName, @EstatusDeTransaccion, @NumeroReferenciadeTransaccion, @Beneficiario, @MontoPago, @ValueDate)";
                sql.Parameters.AddWithValue("@ProfileName", Log.ProfileName);
                sql.Parameters.AddWithValue("@ProfileDescripcion", Log.ProfileDescripcion);
                sql.Parameters.AddWithValue("@RunID", Log.RunID);
                sql.Parameters.AddWithValue("@RunDate", Log.RunDate);
                sql.Parameters.AddWithValue("@SourceFileName", Log.SourceFileName);
                sql.Parameters.AddWithValue("@DestinationFileName", Log.DestinationFileName);
                sql.Parameters.AddWithValue("@EstatusDeTransaccion", 1);
                sql.Parameters.AddWithValue("@NumeroReferenciadeTransaccion", Log.NumeroReferenciadeTransaccion);
                sql.Parameters.AddWithValue("@Beneficiario", Log.Beneficiario);
                sql.Parameters.AddWithValue("@MontoPago", Log.MontoPago);
                sql.Parameters.AddWithValue("@ValueDate", Log.ValueDate);

                sql.ExecuteNonQuery();
                Console.WriteLine("Insertando registro correcto : " + Log.NumeroReferenciadeTransaccion);
            }
            sourceConnection.Close();


            System.Threading.Thread.Sleep(1000);

            return 1;
        }

        public int InsertarIncorrectos(PAG_LogBanamex Log, int tipoError)
        {

            string conexion = ConfigurationManager.AppSettings["cadenaBulkcopyReferencias"];

            SqlConnection sourceConnection = new SqlConnection(conexion);
            sourceConnection.Open();
            SqlCommand sql = new SqlCommand();
            SqlCommand existSql = new SqlCommand();
            int totalRows = 0;


            sql.Connection = sourceConnection;

            existSql.Connection = sourceConnection;

            existSql.CommandText = "Select count(1) as total from dbo.LogBanamex where SourceFileName =@SourceFileName and NumeroReferenciadeTransaccion =@NumeroReferenciadeTransaccion ";
            existSql.Parameters.AddWithValue("@SourceFileName", Log.SourceFileName);
            existSql.Parameters.AddWithValue("@NumeroReferenciadeTransaccion", Log.NumeroReferenciadeTransaccion);

            SqlDataReader TotalLogs = existSql.ExecuteReader();

            if (TotalLogs.HasRows)
            {
                TotalLogs.Read();
                totalRows = TotalLogs.GetInt32(0);
                TotalLogs.Close();
            }

            if (totalRows == 0)
            {

                sql.CommandText = "insert into dbo.LogBanamex (ProfileName, ProfileDescripcion,RunID, RunDate, SourceFileName,DestinationFileName, EstatusDeTransaccion, NumeroReferenciadeTransaccion, Beneficiario, RazonRechazo) " +
                    " values (@ProfileName, @ProfileDescripcion, @RunID, @RunDate, @SourceFileName,@DestinationFileName, @EstatusDeTransaccion, @NumeroReferenciadeTransaccion, @Beneficiario, @RazonRechazo)";
                sql.Parameters.AddWithValue("@ProfileName", Log.ProfileName);
                sql.Parameters.AddWithValue("@ProfileDescripcion", Log.ProfileDescripcion);
                sql.Parameters.AddWithValue("@RunID", Log.RunID);
                sql.Parameters.AddWithValue("@RunDate", Log.RunDate);
                sql.Parameters.AddWithValue("@SourceFileName", Log.SourceFileName);
                sql.Parameters.AddWithValue("@DestinationFileName", Log.DestinationFileName);
                sql.Parameters.AddWithValue("@EstatusDeTransaccion", tipoError);
                sql.Parameters.AddWithValue("@NumeroReferenciadeTransaccion", Log.NumeroReferenciadeTransaccion);
                sql.Parameters.AddWithValue("@Beneficiario", (Log.Beneficiario == null) ? "" : Log.Beneficiario);
                sql.Parameters.AddWithValue("@RazonRechazo", Log.RazonRechazo);

                sql.ExecuteNonQuery();

                string sourceName = Log.SourceFileName;
                char rango = '.';
                int range = sourceName.IndexOf(rango);


                sourceName = sourceName.Substring(17, range - 17);


                this.InsertarBitacoraPrograPagos(Log.NumeroReferenciadeTransaccion, Int32.Parse(sourceName));
                Console.WriteLine("Insertando registro incorrecto : " + Log.NumeroReferenciadeTransaccion);


            }

            sourceConnection.Close();



            //System.Threading.Thread.Sleep(1000);

            return tipoError;
        }


        public string EnviarCorreoElectronico(int tipoCorreo, List<string> elementos, string loteId, string mail1, string mail2)
        {
            string bodyMail = "";
            string subject = "";

            string colocarElementos = "";

            foreach (string elemento in elementos)
            {
                colocarElementos = colocarElementos + "<br><p>" + elemento + "</p>"; 
            }


            if (tipoCorreo == 1)
            {
                bodyMail = "<html><body><p>Del reciente lote " +
                    loteId +
                    " enviado a pago a trav&eacute;s de Banamex, no se han podido efectuar las siguientes transacciones por el motivo " +
                    "que se documenta por parte del banco.</p>" +
                    colocarElementos +
                     "</body></html>";
                subject = "Banamex - Pago No Realizado";
            } else if (tipoCorreo == 2)
            {
                bodyMail = "<html><body><p> Como parte del lote " + loteId+
                    ", se han realizado los siguientes Pagos  a través de Banamex." +
                    " </p>" +
                    colocarElementos +
                    "</body></html>";
                subject = "Banamex - Pagos realizados";
            }
            string result = "";
            MailMessage email = new MailMessage();
            string toMail1 = mail1;
            string toMail2 = mail2;
            string host = ConfigurationManager.AppSettings["host"];
            string port = ConfigurationManager.AppSettings["port"];
            string fromMail = ConfigurationManager.AppSettings["MailAddress"];
            string passwordMail = ConfigurationManager.AppSettings["passwordMail"];
            string userM = ConfigurationManager.AppSettings["userM"];
            //string smtpClient = ConfigurationManager.AppSettings["smtpClient"].ToString();

            email.To.Add(new MailAddress(toMail1));

            if (tipoCorreo == 1)
            {
                email.To.Add(new MailAddress(toMail2));
            }
            email.From = new MailAddress(fromMail);
            email.Subject = subject;
            email.Body = bodyMail;
            email.IsBodyHtml = true;



            SmtpClient smtp = new SmtpClient
            {
                Host = host,
                Port = Int32.Parse(port),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = true,
                EnableSsl = false,
                Credentials = new NetworkCredential(userM, passwordMail)
                
            };
            /*
            smtp.Host =host;
            smtp.Port = Int32.Parse(port);
            //smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail, passwordMail);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;*/
            try
            {
                smtp.Send(email);
                email.Dispose();
                result = "Corre electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                result = "Error enviando correo electrónico: " + ex.Message;
            }

            return result;
        }

         public class PAG_LogBanamex
        {
            public int Id { get; set; }
            public string ProfileName { get; set; }
            public string ProfileDescripcion { get; set; }
            public Nullable<int> RunID { get; set; }
            public Nullable<System.DateTime> RunDate { get; set; }
            public Nullable<int> EstatusDeTransaccion { get; set; }

            public string NumeroReferenciadeTransaccion { get; set; }
            public string Beneficiario { get; set; }
            public Nullable<decimal> MontoPago { get; set; }
            public Nullable<System.DateTime> ValueDate { get; set; }
            public string RazonRechazo { get; set; }
            public string SourceFileName { get; set; }
            public string DestinationFileName { get; set; }
        }

        //FAL declaro 
        public partial class DocumentosPDF_XML
        {
            public string pathXML { get; set; }
            public string pathPDF { get; set; }
            public string byteOC { get; set; }
            public byte[] arrayB { get; set; }
        }

        public DocumentosPDF_XML ConsultaPDF(string polTipo, string annio, string polMes, string polConsecutivo, string idEmpresa)
        {
            //Crea una instancia de la clase DocumentosPDF_XML
            DocumentosPDF_XML doctos = new DocumentosPDF_XML();

            WebReference.WS_Gen_Pdf ws = new WebReference.WS_Gen_Pdf();

            byte[] doctoByte = ws.GeneraPdfPolizaCompra(polTipo, annio, polMes, polConsecutivo, idEmpresa);
            //byte[] doctoByte = ws.GenerarPdf("OCO", "AU-AT-MIR-RE-PE-10", "3");

            doctos.arrayB = doctoByte;

            return doctos;
        }   //Termina agregado LMS

        public List<string> ObtenLayoutBanamex(int idLote)
        {
            return iContext.SEL_PROG_PAGOS_BANAMEX_TXT_SP(idLote).ToList();
        }

        public List<string> ObtenNombreBanco(int idLote)
        {
            return iContext.SEL_PROG_FIND_BANK_TXT_SP(idLote).ToList();                
        }

        public List<string> ObtenNombreLote(int idLote)
        {
            return iContext.SEL_PROG_FIND_NAMELOTE_SP(idLote).ToList();
        }

        public string ObtenerDato(int id)
        {
           List<SEL_RUTA_CONFIGURACION_Result> result = new List<SEL_RUTA_CONFIGURACION_Result>();
            result = iContext.SEL_RUTA_CONFIGURACION(id).ToList();
            return result[0].descripcion;
        }

        public List<SEL_PAGOS_DETALLES_BITACORA_Result> getDetallesBitacoraPagos(string numeroTransaccion, Nullable<int> idLotePago, Nullable<int> estatusTransaccion)
        {
            List<SEL_PAGOS_DETALLES_BITACORA_Result> result = new List<SEL_PAGOS_DETALLES_BITACORA_Result>();
            return iContext.SEL_PAGOS_DETALLES_BITACORA(numeroTransaccion, idLotePago, estatusTransaccion).ToList();
        }

        public SEL_FORMATO_SFTP_Result GetSFTPValores(int idBanco)
        {
            SEL_FORMATO_SFTP_Result result = new SEL_FORMATO_SFTP_Result();
            result = iContext.SEL_FORMATO_SFTP(idBanco).First();
            return result;
        }


    }
}
