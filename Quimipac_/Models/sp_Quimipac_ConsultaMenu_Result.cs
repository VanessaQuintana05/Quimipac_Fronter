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
    
    public partial class sp_Quimipac_ConsultaMenu_Result
    {
        public int Id_Menu { get; set; }
        public string Menu { get; set; }
        public Nullable<int> Padre { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<int> Nivel_Profundidad { get; set; }
        public string Estado { get; set; }
        public string Action { get; set; }
        public string controlador { get; set; }
        public string Icono { get; set; }
    }
}