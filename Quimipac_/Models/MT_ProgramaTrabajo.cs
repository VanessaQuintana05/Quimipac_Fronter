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
    
    public partial class MT_ProgramaTrabajo
    {
        public int Id_ProgramaTrabajo { get; set; }
        public Nullable<int> Id_Contrato { get; set; }
        public string Id_Usuario { get; set; }
        public Nullable<int> Id_TIpo_Trabajo { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Ini { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public Nullable<int> Id_GrupoTrabajo { get; set; }
    }
}
