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
    
    public partial class PAG_FLUJO_INGRESO_BANCOS
    {
        public decimal pib_id { get; set; }
        public decimal pal_id_lote_pago { get; set; }
        public string pib_id_cuenta { get; set; }
        public decimal pib_saldo { get; set; }
        public decimal pib_disponible { get; set; }
        public string pib_cuentaContable { get; set; }
    
        public virtual PAG_LOTE_PAGO PAG_LOTE_PAGO { get; set; }
    }
}
