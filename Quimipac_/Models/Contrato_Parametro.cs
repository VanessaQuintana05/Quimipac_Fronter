using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Contrato_Parametro
    {
        public int Id_Contrato_Parametro { get; set; }
        public Nullable<int> Id_Contrato { get; set; }
        public string Parametro { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Tipo_Dato { get; set; }
        public string Estado { get; set; }
        public string Valor_Inicial_number { get; set; }        
        public string Valor_Final_number { get; set; }
        public string Valor_Inicial_varchar { get; set; }
        public string Valor_Final_varchar { get; set; }
        public string Valor_Inicial_datetime { get; set; }
        public string Valor_Final_datetime { get; set; }
        public string Valor_Inicial_decimal { get; set; }
        public string Valor_Final_decimal { get; set; }

    }
}