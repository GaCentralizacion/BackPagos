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
    
    public partial class PAG_FLUJO_EGRESO_BANCOS
    {
        public decimal peb_id { get; set; }
        public decimal pal_id_lote_pago { get; set; }
        public string peb_id_cuenta { get; set; }
        public Nullable<decimal> peb_saldo { get; set; }
        public Nullable<decimal> peb_aTransferir { get; set; }
        public Nullable<decimal> peb_total { get; set; }
        public Nullable<decimal> peb_excedente { get; set; }
        public string peb_cuentaContable { get; set; }
    }
}