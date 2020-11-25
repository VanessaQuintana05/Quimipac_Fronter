using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class OrdenTrabajo_Excel
    {
        public string Identificador { get; set; }
        public string Numerador { get; set; }
        public string Tipo_de_Trabajo { get; set; }
        public string Estado { get; set; }
        public string Sector_Operativo { get; set; }
        public string Producto { get; set; }
        public string Unidad_de_Trabajo { get; set; }
        public string Estado_Unidad_de_Trabajo { get; set; }
        public DateTime? Fecha_de_Creacion { get; set; }
        public DateTime? Fecha_de_Asignacion { get; set; }
        public string Tipo_de_Asignacion { get; set; }
        public DateTime? Fecha_Estimada_de_Ejecucion { get; set; }
        public DateTime? Fecha_Maxima_para_Legalizacion { get; set; }
        public DateTime? Ultima_Reprogramacion { get; set; }
        public DateTime? Fecha_de_Legalizacion { get; set; }
        public DateTime? Inicio_de_Ejecucion { get; set; }
        public DateTime? Fin_de_Ejecucion { get; set; }
        public string Causal_de_Legalizacion { get; set; }
        public string Personal { get; set; }
        public string Valor_de_la_Orden { get; set; }
        public string Número_de_Veces_Impresa { get; set; }
        public string Intentos_de_Legalizacion { get; set; }
        public string Anula_Otra_Orden { get; set; }
        public string Tipo_de_Trabajo_Planeado { get; set; }
        public string Nivel_de_Prioridad { get; set; }
        public string Tipo_de_Compromiso { get; set; }
        public TimeSpan? Hora_Acordada { get; set; }
        public string Cita_Confirmada { get; set; }
        public string Consecutivo_Ruta { get; set; }
        public string Ruta { get; set; }
        public string Nombre_Ruta { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public string Ubicacion_Geografica { get; set; }
        public string Cliente { get; set; }
        public string Nombre_Suscriptor { get; set; }
        public string Apellido_Suscriptor { get; set; }
        public string Tipo_de_Cliente { get; set; }
        public string Teléfono_Cliente { get; set; }
        public string Puntaje_Cliente { get; set; }
        public string Esfuerzo_de_la_Orden { get; set; }
        public string Comentario { get; set; }
        public string Tipo_de_Comentario { get; set; }
        public string Unidad_Asociada { get; set; }
        public string Ofertada { get; set; }
        public string Proyecto { get; set; }
        public string Etapa { get; set; }
        public string Estado_de_la_Orden { get; set; }
        public string PARENT_ID { get; set; }



        public string fecha_creacion { get; set; }
        public string fecha_asignacion { get; set; }
        public string fecha_estimada_eje { get; set; }
        public string fecha_max_leg { get; set; }
        public string ultimareprog { get; set; }
        public string fecha_leg { get; set; }
        public string inicio_ej { get; set; }
        public string fin_ej { get; set; }
        public string hora_acordadas { get; set; }
    }
}