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
    
    public partial class PAG_LOTE_PAGO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAG_LOTE_PAGO()
        {
            this.PAG_FLUJO_INGRESO_BANCOS = new HashSet<PAG_FLUJO_INGRESO_BANCOS>();
            this.PAG_PROGRA_PAGOS_DETALLE = new HashSet<PAG_PROGRA_PAGOS_DETALLE>();
        }
    
        public decimal pal_id_lote_pago { get; set; }
        public decimal pal_id_empresa { get; set; }
        public decimal pal_id_usuario { get; set; }
        public System.DateTime pal_fecha { get; set; }
        public string pal_nombre { get; set; }
        public short pal_estatus { get; set; }
        public short pal_esAplicacionDirecta { get; set; }
        public Nullable<int> pal_id_tipoLotePago { get; set; }
        public Nullable<decimal> pal_cifraControl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAG_FLUJO_INGRESO_BANCOS> PAG_FLUJO_INGRESO_BANCOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAG_PROGRA_PAGOS_DETALLE> PAG_PROGRA_PAGOS_DETALLE { get; set; }
    }
}
