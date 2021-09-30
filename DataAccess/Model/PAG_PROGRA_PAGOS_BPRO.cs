//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PAG_PROGRA_PAGOS_BPRO
    {
        public Nullable<decimal> pbp_id_lote_pago { get; set; }
        public decimal pbp_id { get; set; }
        public Nullable<decimal> pbp_idProveedor { get; set; }
        public string pbp_polTipo { get; set; }
        public decimal pbp_polAnnio { get; set; }
        public decimal pbp_polMes { get; set; }
        public decimal pbp_polConsecutivo { get; set; }
        public decimal pbp_polMovimiento { get; set; }
        public Nullable<System.DateTime> pbp_polFechaOperacion { get; set; }
        public string pbp_cuenta { get; set; }
        public string pbp_proveedor { get; set; }
        public string pbp_documento { get; set; }
        public string pbp_tipo { get; set; }
        public string pbp_tipoDocto { get; set; }
        public string pbp_cartera { get; set; }
        public Nullable<decimal> pbp_monto { get; set; }
        public Nullable<decimal> pbp_saldo { get; set; }
        public Nullable<decimal> pbp_saldoPorcentaje { get; set; }
        public string pbp_moneda { get; set; }
        public Nullable<System.DateTime> pbp_fechaVencimiento { get; set; }
        public Nullable<System.DateTime> pbp_fechaPromesaPago { get; set; }
        public Nullable<System.DateTime> pbp_fechaRecepcion { get; set; }
        public Nullable<System.DateTime> pbp_fechaFactura { get; set; }
        public string pbp_ordenCompra { get; set; }
        public string pbp_idEstatus { get; set; }
        public string pbp_estatus { get; set; }
        public Nullable<decimal> pbp_anticipo { get; set; }
        public Nullable<decimal> pbp_anticipoAplicado { get; set; }
        public Nullable<decimal> pbp_annio { get; set; }
        public string pbp_proveedorBloqueado { get; set; }
        public string pbp_ordenBloqueada { get; set; }
        public Nullable<decimal> pbp_diasCobro { get; set; }
        public string pbp_aprobado { get; set; }
        public Nullable<decimal> pbp_contReprog { get; set; }
        public string pbp_documentoPagable { get; set; }
        public Nullable<int> pbp_aPagar { get; set; }
        public string pbp_nombreAgrupador { get; set; }
        public Nullable<int> pbp_ordenAgrupador { get; set; }
        public Nullable<int> pbp_ordenProveedor { get; set; }
        public string pbp_cuentaPagadora { get; set; }
        public string pbp_cuentaProveedor { get; set; }
        public Nullable<int> pbp_empresa { get; set; }
        public Nullable<System.DateTime> pbp_fechaCreacionRegistro { get; set; }
        public string pbp_cuentaDestino { get; set; }
        public string pbp_seleccionable { get; set; }
        public string pbp_numeroSerie { get; set; }
        public string pbp_facturaProveedor { get; set; }
        public Nullable<decimal> pbp_consCartera { get; set; }
        public string pbp_esBanco { get; set; }
        public Nullable<int> pbp_tipoCartera { get; set; }
        public Nullable<int> pbp_loteExterno { get; set; }
        public string pbp_convenioCIE { get; set; }
        public Nullable<int> pbp_autorizado { get; set; }
        public string pbp_cuentaDestinoArr { get; set; }
        public string No_CotizacionSISCO { get; set; }
        public string No_OrdenServicioSISCO { get; set; }
        public Nullable<int> Id_EstatusSISCO { get; set; }
        public string EstatusSISCO { get; set; }
        public Nullable<System.DateTime> FechaFacturaSISCO { get; set; }
        public string OperacionSISCO { get; set; }
        public Nullable<System.DateTime> FechaCargaCopade { get; set; }
        public Nullable<System.DateTime> FechaRecepcionCopade { get; set; }
        public string pbp_obsGenerales { get; set; }
        public Nullable<int> EstatusDiasPago { get; set; }
    }
}
