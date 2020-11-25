using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    [MetadataType(typeof(OrdenTrabajo_EtapaMetaData))]
    public class OrdenTrabajo_EtapaMetaData
    {
    }

    public partial class sp_Quimipac_ConsultaMT_OrdenTrabajo_Result
    {
        //public string Estado_P_A_C { get; set; }
        //aumente todo hacia abajo
        public string codAux_Contr_TipTrab_CodCli { get; set; }

        
        public string DtrabajoEJE { get; set; }
        public string Parametro { get; set; }

        //de Fitcli
        public string cod_cli { get; set; }
        public string Etapa { get; set; }

        //permiso
        public string UsuarioPermiso { get; set; }



        
        public Nullable<int> Id_entrega_orden_trabajo { get; set; }
        public Nullable<int> Nivel_prioridad { get; set; }
        
        public Nullable<System.DateTime> Fecha_creacion_cliente { get; set; }
        public Nullable<System.DateTime> Fecha_maxima_ejecucion_cliente { get; set; }
        public Nullable<System.DateTime> Fecha_asignacion_grupo_trabajo { get; set; }
        
        public Nullable<System.DateTime> Fecha_ini_permiso_municipal { get; set; }
        public Nullable<System.DateTime> Fecha_fin_permiso_municipal { get; set; }
        public Nullable<System.DateTime> Fecha_entrega { get; set; }
        public Nullable<System.DateTime> Fecha_max_legalizacion { get; set; }
        public Nullable<System.TimeSpan> Hora_acordada { get; set; }
        public Nullable<int> Id_barrio { get; set; }
        public string Direccion { get; set; }
        public string Referencia_direccion { get; set; }
        public string Identificacion_suscriptor { get; set; }
        public string Nombre_suscriptor { get; set; }
        public string Tipo_suscriptor { get; set; }
        public string Telefono_suscriptor { get; set; }
        public string Origen { get; set; }
        public string Comentario { get; set; }
       
        public Nullable<int> Id_planilla { get; set; }
        public string Estado_orden_planilla { get; set; }
        
        public string Interna { get; set; }
        

    }

}