//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PAG_PROGRA_PAGOS_DETA
    {
        public decimal id { get; set; }
        public decimal id_proveedor { get; set; }
        public decimal id_cartera { get; set; }
        public string documento { get; set; }
        public string tipo { get; set; }
        public Nullable<decimal> importe { get; set; }
        public Nullable<decimal> pago { get; set; }
        public Nullable<System.DateTime> fecha_promesa { get; set; }
    }
}
