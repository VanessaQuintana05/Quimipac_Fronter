using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class CustomChartRender
    {
        public class CustomChart
        {
            public string TexItem { set; get; }
            public int ValueItem { set; get; }
        }
        public class CustomChartxEstado
        {
            public string TexItem { set; get; }
            public int nProceso { set; get; }
            public int nAlerta { set; get; }
            public int nCaida { set; get; }
        }
        public class CustomChartSelect
        {
            public string CODIGO_CLIENTE { set; get; }
            public string ESTADO_P_A_C { set; get; }
            public string GRUPOTRABAJO { set; get; }
            public string NOMBRECLIENTE { set; get; }
            public int? ID_ORDENTRABAJO_INTEGRANTE { get; set; }
            public int? Id_GrupoTrabajo { get; set; }
            public int? ID_ORDEN_TRABAJO { get; set; }
            public int? Id_Proyecto { get; set; }
            public int? Id_Prospecto { get; set; }
            public bool AUTOMATIZACION { get; set; }
        }

    }
}