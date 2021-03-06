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
    
    public partial class SEL_PROGRAMACION_PAGOSENROJO_SP_Result
    {
        public string polTipo { get; set; }
        public decimal annio { get; set; }
        public decimal polMes { get; set; }
        public decimal polConsecutivo { get; set; }
        public decimal polMovimiento { get; set; }
        public Nullable<System.DateTime> polFechaOperacion { get; set; }
        public string documento { get; set; }
        public string cuenta { get; set; }
        public Nullable<decimal> idProveedor { get; set; }
        public string proveedor { get; set; }
        public string tipoDocto { get; set; }
        public string cartera { get; set; }
        public Nullable<decimal> monto { get; set; }
        public Nullable<decimal> saldo { get; set; }
        public Nullable<decimal> saldoPorcentaje { get; set; }
        public string moneda { get; set; }
        public Nullable<System.DateTime> fechaVencimiento { get; set; }
        public Nullable<System.DateTime> fechaPromesaPago { get; set; }
        public Nullable<System.DateTime> fechaRecepcion { get; set; }
        public Nullable<System.DateTime> fechaFactura { get; set; }
        public string ordenCompra { get; set; }
        public string estatus { get; set; }
        public string idEstatus { get; set; }
        public Nullable<decimal> anticipo { get; set; }
        public Nullable<decimal> anticipoAplicado { get; set; }
        public string proveedorBloqueado { get; set; }
        public string ordenBloqueada { get; set; }
        public Nullable<decimal> diasCobro { get; set; }
        public string aprobado { get; set; }
        public Nullable<decimal> contReprog { get; set; }
        public string documentoPagable { get; set; }
        public Nullable<int> aPagar { get; set; }
        public string nombreAgrupador { get; set; }
        public Nullable<int> ordenAgrupador { get; set; }
        public Nullable<int> ordenProveedor { get; set; }
        public string cuentaPagadora { get; set; }
        public string cuentaProveedor { get; set; }
        public Nullable<int> empresa { get; set; }
        public string cuentaDestino { get; set; }
        public string seleccionable { get; set; }
        public string numeroSerie { get; set; }
        public string facturaProveedor { get; set; }
        public string esBanco { get; set; }
        public Nullable<decimal> consCartera { get; set; }
    }
}
