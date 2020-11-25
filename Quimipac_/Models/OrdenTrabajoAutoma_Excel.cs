using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class OrdenTrabajoAutoma_Excel
    {
        public string Identificador { get; set; } //codigo completo es codigo de la actividad
        
        public string Tipo_de_Trabajo { get; set; }
        public string Estado { get; set; }
        
        public DateTime? Fecha_de_Creacion { get; set; }
        public DateTime? Fecha_de_Asignacion { get; set; }
        
        public DateTime? Inicio_de_Ejecucion { get; set; }
        public DateTime? Fin_de_Ejecucion { get; set; }
        
        
        //campo auxiliar
        public string oatom_id_aux { get; set; }
        public string oatom_id_aux_total { get; set; }

        public string fecha_creacion { get; set; }
        public string fecha_asignacion { get; set; }
        
        public string inicio_ej { get; set; }
        public string fin_ej { get; set; }

        //campos para la actividad
        public string nombre_actividad { get; set; } // activo
        public string rutinaria_activador { get; set; }

        //campos de grupo de trabajo
        public string integrantes { get; set; } // recursos humanos
        public string responsable { get; set; } // respinsable jefe




    }
}