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
    
    public partial class MT_OrdenTrabajo_Formulario
    {
        public int Id_OrdenTrabajo_Formulario { get; set; }
        public Nullable<int> Id_Orden_Trabajo_Detalle { get; set; }
        public string Enlace_Formulario { get; set; }
        public string NombreArchivo { get; set; }
    
        public virtual MT_OrdenTrabajo MT_OrdenTrabajo { get; set; }
    }
}
