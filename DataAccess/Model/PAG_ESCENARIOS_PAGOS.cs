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
    
    public partial class PAG_ESCENARIOS_PAGOS
    {
        public decimal pep_idEscenarios { get; set; }
        public bool pep_pagoDirectoPlanta { get; set; }
        public bool pep_pagoDirectoBanco { get; set; }
        public decimal ptrp_idtipoReferenciaPlanta { get; set; }
        public decimal ptrb_idtipoReferenciaBancos { get; set; }
    
        public virtual PAG_TIPO_REFERENCIA_BANCOS PAG_TIPO_REFERENCIA_BANCOS { get; set; }
        public virtual PAG_TIPO_REFERENCIA_PLANTA PAG_TIPO_REFERENCIA_PLANTA { get; set; }
    }
}
