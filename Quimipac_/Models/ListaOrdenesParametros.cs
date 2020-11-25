using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class ListaOrdenesParametros
    {
        //public ListaOrdenesParametros() { }
        //public string Codigo_Cliente { get; set; }
        //public string Nombre_Completo { get; set; }
        //public string Parametro { get; set; }
        //public Nullable<System.DateTime> Fecha_Asignacion { get; set; }

        public Nullable<int> Id_OrdenTrabajo { get; set; }
        public string Codigo_Interno { get; set; }
        public string Nombre_Completo { get; set; }
        public string Parametro { get; set; }
        public string Fecha_Asignacion { get; set; }
        public string Dtrabajo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fecha_registro { get; set; }
        public string Porcentaje_avance { get; set; }
        public Nullable<TimeSpan> Tiempo_transcurrido { get; set; }

    }
}