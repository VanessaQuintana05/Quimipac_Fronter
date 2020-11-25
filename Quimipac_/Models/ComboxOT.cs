using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class ComboxOT
    {
        public int Id_TipoTrabajo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string cia_codigo { get; set; }
        public int Id_Campamento { get; set; }
        public string Id_Empresa { get; set; }
        public string Nombre { get; set; }
        
        public string cod_suc { get; set; }
        public int Id_Estacion { get; set; }
        public string Direccion { get; set; }
        public int Id_Sector { get; set; }
        public int Id_Contrato { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Codigo_Interno { get; set; }
        public int Id_TablaDetalle { get; set; }
        public string Codigo { get; set; }
        public string Id_Cliente { get; set; }

        public int Id_Proyecto { get; set; }
        public int Id_Prospecto { get; set; }

    }
}