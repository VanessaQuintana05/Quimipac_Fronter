using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Insertar_ContratoParametro
    {
        public int Id_Contrato_Parametro { get; set; }
        public int Id_Contrato { get; set; }
        public string Parametro { get; set; }
        public string Descripcion { get; set; }
        public string Tipo_Dato { get; set; }
        public int Valor_Inicial { get; set; }
        public string Estado { get; set; }
        public int Valor_Final { get; set; }

    }
}