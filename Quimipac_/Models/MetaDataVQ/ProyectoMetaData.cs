using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    [MetadataType(typeof(ProyectoMetaData))]
    public partial class MT_Proyecto
    {
        public string EstadoProrroga { get; set; }
        public string Valor_Ajustado_Aux { get; set; }
        public string Valor_Inicial_Aux { get; set; }

    }

    public class ProyectoMetaData
    {
        //[Required, MaxLength(4)]
        //public string EstadoProrroga { get; set; }
    }
}