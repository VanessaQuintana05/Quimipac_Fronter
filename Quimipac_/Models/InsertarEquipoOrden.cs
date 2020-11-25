using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class InsertarEquipoOrden
    {

      
      public int Id_Orden_Trabajo { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Estado { get; set; }
      public int Id_GrupoTrabajo { get; set; }
    }
}