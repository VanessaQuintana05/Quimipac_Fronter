//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quimipac_.Models
{
    using System;
    
    public partial class sp_LINK_ConsultaContratos_Result
    {
        public string empresa { get; set; }
        public string cliente { get; set; }
        public string unidad { get; set; }
        public string proyecto { get; set; }
        public string secuencial { get; set; }
        public string anio { get; set; }
        public string codigo_secuencial_interno { get; set; }
        public string codigo_secuencial_interno_padre { get; set; }
        public string codigo_contrato_asociado { get; set; }
        public Nullable<System.DateTime> fecha_inicial { get; set; }
        public Nullable<decimal> plazo_contrato { get; set; }
        public Nullable<System.DateTime> fecha_fin { get; set; }
        public Nullable<decimal> monto { get; set; }
        public string responsable { get; set; }
        public Nullable<decimal> costo { get; set; }
        public string detalle { get; set; }
        public string codigo_secuencial_interno_migrado { get; set; }
        public string unidad_migrada { get; set; }
        public string cod_servicio { get; set; }
        public string codcen { get; set; }
        public string estado { get; set; }
        public Nullable<System.DateTime> prorroga_inicial { get; set; }
        public Nullable<decimal> plazo_prorroga { get; set; }
        public Nullable<System.DateTime> prorroga_final { get; set; }
        public string observaciones { get; set; }
        public string cia_codigo { get; set; }
    }
}
