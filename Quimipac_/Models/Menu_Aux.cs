using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Menu_Aux
    {
        public int Id_Menu { get; set; }
        public string Descripcion_Menu { get; set; }
        public Nullable<int> Padre { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<int> Nivel_Profundidad { get; set; }
        public string Estado { get; set; }
        //public string Action { get; set; }
        //public string Controlador { get; set; }
        public string Icono { get; set; }
        public bool IsPadre { get; set; }
        public bool IsSelected { get; set; }

        public string nombre_Rol { get; set; }
        
    }
}