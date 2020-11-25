using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class ClsNotificacion
    {
        public ClsNotificacion() { }
        public Nullable<int> Id_Tabla { get; set;}
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Id_Padre { get; set; }

        
    }
}