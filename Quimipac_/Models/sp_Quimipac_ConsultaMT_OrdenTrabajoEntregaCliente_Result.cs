//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quimipac_.Models
{
    using System;
    
    public partial class sp_Quimipac_ConsultaMT_OrdenTrabajoEntregaCliente_Result
    {
        public int Id_OrdenTrabajo { get; set; }
        public Nullable<int> Id_Proyecto { get; set; }
        public string Id_sucursal { get; set; }
        public Nullable<int> Id_campamento { get; set; }
        public Nullable<int> Id_tipo_trabajo_recibido { get; set; }
        public Nullable<int> Id_tipo_trabajo_ejecutado { get; set; }
        public Nullable<int> Estado { get; set; }
        public Nullable<int> Id_sector { get; set; }
        public Nullable<int> Id_orden_padre { get; set; }
        public Nullable<int> Id_estacion { get; set; }
        public string Id_usuario { get; set; }
        public Nullable<int> Id_entrega_orden_trabajo { get; set; }
        public Nullable<int> Nivel_prioridad { get; set; }
        public Nullable<System.DateTime> Fecha_registro { get; set; }
        public Nullable<System.DateTime> Fecha_creacion_cliente { get; set; }
        public Nullable<System.DateTime> Fecha_maxima_ejecucion_cliente { get; set; }
        public Nullable<System.DateTime> Fecha_asignacion_grupo_trabajo { get; set; }
        public Nullable<System.DateTime> Fecha_inicio_ejecucion { get; set; }
        public Nullable<System.DateTime> Fecha_fin_ejecucion { get; set; }
        public Nullable<System.DateTime> Fecha_finalizacion_obra { get; set; }
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
        public string Porcentaje_avance { get; set; }
        public Nullable<System.TimeSpan> Tiempo_transcurrido { get; set; }
        public Nullable<int> Id_planilla { get; set; }
        public string Estado_orden_planilla { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Interna { get; set; }
        public string EstadoEditOrden { get; set; }
        public Nullable<bool> Excel_orden { get; set; }
        public Nullable<System.DateTime> Fecha_asignacion { get; set; }
        public Nullable<System.DateTime> Fecha_maxima_contratista { get; set; }
        public Nullable<bool> Desalojo { get; set; }
        public Nullable<bool> Automatizacion { get; set; }
        public Nullable<int> Id_Prospecto { get; set; }
        public string Descripcion { get; set; }
    }
}
