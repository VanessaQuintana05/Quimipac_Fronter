using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Insertar_Contratosysbase
    {

        

        public string cia_codigo { get; set; }
        public string cod_cliente { get; set; }
        public DateTime? fecha_inicial { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string codigo_secuencial_interno { get; set; }
        public string codigo_contrato_asociado { get; set; }
        public string user_id { get; set; }
        public string unidad { get; set; }
        public string cod_servicio { get; set; }
        public string codcen { get; set; }
        public string detalle { get; set; }
        public int id_estado { get; set; }
        public decimal plazo_contrato { get; set; }
        public int cod_tipo { get; set; }
        public decimal monto { get; set; }
        public decimal costo { get; set; }
        public string secuencial { get; set; }
        public string codigo_secuencial_interno_padre { get; set; }
        public string observaciones { get; set; }

    }
}