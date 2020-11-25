using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class JsonUtil
    {

        public class addContratos_Permisos
        {
            public int Id_Cliente { get; set; }
            public string Nombre_Cliente { get; set; }
            
        }
        public string cod_cli { get; set; }
        public string nom_cli { get; set; }

        public string cod_servicio { get; set; }
        public string nombre { get; set; }
        public string codcen { get; set; }
        public string descripcion { get; set; }
        public string codigo_loc { get; set; }
        
        public decimal id_persona { get; set; }
        public string Nombres_Completos { get; set; }
    }
}