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
    using System.Collections.Generic;
    
    public partial class MT_Contrato_Documentado
    {
        public int Id_Contrato_Documentado { get; set; }
        public Nullable<int> Id_Contrato { get; set; }
        public string Descripcion { get; set; }
        public string Enlace { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Validez { get; set; }
        public string Estado { get; set; }
        public string NombreArchivo { get; set; }
        public Nullable<int> Tipo { get; set; }
        public Nullable<decimal> Version { get; set; }
    
        public virtual MT_Contrato MT_Contrato { get; set; }
    }
}
