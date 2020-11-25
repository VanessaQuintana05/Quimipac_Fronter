using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    [MetadataType(typeof(ContratoMetaData))]
    public partial class MT_Contrato
    {
        public string Valor_Referencial_Aux { get; set; }
        public string Monto_Aux { get; set; }
        public string Costo_Aux { get; set; }
        public string lineaContrato { get; set; }
        public string nombre_Categoria { get; set; }
        public string nombre_Subcategoria { get; set; }
        


    }
    public class ContratoMetaData
    {
    }
}