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
    
    public partial class Mt_Prospecto_Alerta
    {
        public int Id_Prospecto_Alerta { get; set; }
        public Nullable<int> Id_Prospecto { get; set; }
        public string Id_Usuario { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Alerta { get; set; }
        public string Mensaje { get; set; }
        public Nullable<int> Repetir { get; set; }
        public string Estado { get; set; }
        public string Correo { get; set; }
    
        public virtual MT_Prospecto MT_Prospecto { get; set; }
    }
}