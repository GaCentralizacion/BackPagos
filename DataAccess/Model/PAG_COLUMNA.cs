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
    
    public partial class PAG_COLUMNA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAG_COLUMNA()
        {
            this.PAG_COLUMNA_USUARIO = new HashSet<PAG_COLUMNA_USUARIO>();
        }
    
        public decimal pac_idColumna { get; set; }
        public string pac_columna { get; set; }
        public string pac_label { get; set; }
        public Nullable<decimal> pac_width { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAG_COLUMNA_USUARIO> PAG_COLUMNA_USUARIO { get; set; }
    }
}
