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
    
    public partial class MT_OrdenTrabajo_Equipo
    {
        public int Id_OrdenTrabajo_Equipo { get; set; }
        public Nullable<int> Id_Orden_Trabajo { get; set; }
        public Nullable<int> Id_Equipo { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public string Estado { get; set; }
        public Nullable<int> Id_GrupoTrabajo { get; set; }
    
        public virtual MT_OrdenTrabajo MT_OrdenTrabajo { get; set; }
    }
}
