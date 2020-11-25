using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Insertar_ContratoProrroga
    {

        public Nullable<int> Id_Contrato { get; set; }        
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Dia_Prorroga { get; set; }
    }
}