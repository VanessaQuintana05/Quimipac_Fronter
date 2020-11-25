using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class InsertarOrdenTrabajo
    {

    public int Id_OrdenTrabajo { get; set; }  
    public int Id_contrato { get; set; }   
    public string Id_sucursal { get; set; }  
    public int Id_campamento { get; set; }  
    public int Id_tipo_trabajo_recibido { get; set; } 
    public int Id_tipo_trabajo_ejecutado { get; set; }  
    public int Estado { get; set; }  
    public int Id_sector { get; set; } 
    public int Id_orden_padre { get; set; } 
    public int Id_estacion { get; set; } 
    public string Id_usuario { get; set; } 
    public int Id_entrega_orden_trabajo { get; set; } 
    public int Nivel_prioridad { get; set; } 
    public DateTime Fecha_creacion_cliente  { get; set; } 
    public DateTime Fecha_maxima_ejecucion_cliente { get; set; } 
    public DateTime Fecha_asignacion_grupo_trabajo  { get; set; } 
    public DateTime Fecha_asignacion  { get; set; } 
    public DateTime Fecha_finalizacion_obra { get; set; } 
    public DateTime Fecha_ini_permiso_municipal { get; set; } 
    public DateTime Fecha_fin_permiso_municipal { get; set; } 
    public DateTime Fecha_entrega  { get; set; } 
    public DateTime Fecha_max_legalizacion { get; set; } 
    public TimeSpan Hora_acordada { get; set; } 
    public int Id_barrio { get; set; } 
    public string Direccion { get; set; } 
    public string Referencia_direccion { get; set; } 
    public string Identificacion_suscriptor { get; set; } 

    public string Nombre_suscriptor { get; set; } 
    public string Tipo_suscriptor { get; set; } 

    public string Telefono_suscriptor { get; set; } 

    public string Origen { get; set; } 
    public string Comentario { get; set; } 
    public string Porcentaje_avance { get; set; } 
    public TimeSpan Tiempo_transcurrido { get; set; } 
    public int Id_planilla { get; set; } 

    public string Estado_orden_planilla { get; set; } 

    public string Codigo_Cliente { get; set; } 

    public string interna { get; set; }

    public bool Excel_orden { get; set; }





    }
}