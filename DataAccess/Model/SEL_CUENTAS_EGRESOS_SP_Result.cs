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
    
    public partial class SEL_CUENTAS_EGRESOS_SP_Result
    {
        public Nullable<decimal> id { get; set; }
        public string banco { get; set; }
        public Nullable<decimal> MontoMinimo { get; set; }
        public string cuenta { get; set; }
        public Nullable<decimal> saldo { get; set; }
        public Nullable<decimal> aTransferir { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> excedente { get; set; }
        public Nullable<decimal> totalPagar { get; set; }
        public Nullable<decimal> saldoIngreso { get; set; }
        public Nullable<int> ingreso { get; set; }
        public Nullable<int> egreso { get; set; }
        public string cuentaContable { get; set; }
    }
}