using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class PlanilllaDetalle_Excel
    {
        public string Código_Acta { get; set; }
        public string Base_Administrativa { get; set; }
        public string Descripción_de_Acta { get; set; }
        public string Código_Periodo { get; set; }
        public string Descripción_de_Periodo { get; set; }
        public string Código_Tipo_de_Acta { get; set; }
        public string Descripción_de_Tipo_de_Acta { get; set; }
        public DateTime? Fecha_Inicial_Periodo { get; set; }
        public DateTime? Fecha_Final_Periodo { get; set; }
        public DateTime? Fecha_Cierre_Acta { get; set; }
        public string Estado_Acta { get; set; }
        public string Descripción_de_Estado_Acta { get; set; }
        public string Orden_Raiz { get; set; }
        public string No_Orden_Padre { get; set; }
        public string No_Orden { get; set; }
        public string Código_Detalle_Acta { get; set; }
        public string Código_Items { get; set; }
        public string Descripción_de_Items { get; set; }
        public string Cantidad { get; set; }
        public string Código_Unidad { get; set; }
        public string Unidad { get; set; }
        public string Valor_Unitario { get; set; }
        public string Valor_Total { get; set; }
        public string Código_Listado_de_Costo { get; set; }
        public string Descripción_Listado_de_Costo { get; set; }
        public string Tipo_Generación { get; set; }
        public string Código_de_Plan_de_Condición { get; set; }
        public string Tipo_de_Trabajo { get; set; }
        public string Descripción_de_Tipo_de_Trabajo { get; set; }
        public string Actividad { get; set; }
        public DateTime? Fecha_Asignación { get; set; }
        public DateTime? Fecha_Fin_Permiso_Municipal_Calc { get; set; }
        public DateTime? Fecha_Asignación_OT { get; set; }
        public DateTime? Fecha_Ejecución { get; set; }
        public DateTime? Fecha_Legalización { get; set; }
        public DateTime? Fecha_Ejecucion_OT_Padre { get; set; }
        public string Tiempo_Promedio_Inc_Desc_OT { get; set; }
        public string Tiempo_Promedio_Desc_Descarga { get; set; }
        public string Tiempo_Transcurrido_HORAS { get; set; }
        public string Tiempo_Optimo_Incentivo { get; set; }
        public string Tiempo_Máximo_Incentivo { get; set; }
        public string Porcentaje_Incentivo { get; set; }
        public string Tiempo_Optimo_Desc_Atención { get; set; }
        public string Tiempo_Máximo_Desc_Atención { get; set; }
        public string Porcentaje_de_Desc_Atención { get; set; }
        public string Tiempo_Máximo_Desc_Descarga { get; set; }
        public string Tiempo_Excede_Desc_Descarga { get; set; }
        public string Porcentaje_Desc_Descarga { get; set; }
        public string Valor_Aplica_Desc_Atención_OT { get; set; }
        public string Código_Contrato { get; set; }
        public string Descripción_Contrato { get; set; }
        public string Tipo_de_Contrato { get; set; }
        public string Descripción_de_Tipo_de_Contrato { get; set; } 
        public string Contratista { get; set; }
        public string Descripción_Contratista { get; set; }
        public string Porcentaje_Costo_Indirecto_Contrato { get; set; }
        public string Porcentaje_Fondo_Garantia_Contrato { get; set; }
        public string Porcentaje_Amortización_Contrato { get; set; }
        public string Descuento_General { get; set; }
        public string Código_Unidad_Operativa { get; set; }
        public string Descripción_Unidad_Operativa { get; set; }
        public string Terminal { get; set; }
        public string Valor_Base { get; set; }
        public string Cumplimiento { get; set; }
        public string Tiempo_Contractual_HORAS { get; set; }
        public string Tiempo_de_Permiso_Municipal_HORAS { get; set; }
        public string Tipo_Clasifica { get; set; }
        public string Inventario { get; set; }
        public string Tiene_Caso_Especial { get; set; }
        public string Tipoe_de_Relación { get; set; }
        public string Nro_Contrato { get; set; }
        public string Nro_Producto { get; set; }
        public string Código { get; set; }
        public string Tipo_de_Irregularidad { get; set; }
        public string Descripción_de_Proyecto { get; set; }
        public string Usuario_Finalizo { get; set; }
        public string Código_Tipo_Comentario { get; set; }
        public string Comentario { get; set; }
        public string Observación_Orden_Actividad { get; set; }
        public DateTime? Fecha_Pago_Sistema_Externo { get; set; }
        public string Número_Factura_Sistema_Externo { get; set; }
        public string Cod_estado_orden { get; set; }

        public string fecha_Inicial_Periodo { get; set; }
        public string fecha_Final_Periodo { get; set; }
        public string fecha_Cierre_Acta { get; set; }
        public string fecha_Asignación { get; set; }
        public string fecha_Fin_Permiso_Municipal_Calc { get; set; }
        public string fecha_Asignación_OT { get; set; }
        public string fecha_Ejecución { get; set; }
        public string fecha_Legalización { get; set; }
        public string fecha_Ejecucion_OT_Padre { get; set; }
        public string fecha_Pago_Sistema_Externo { get; set; }

    }
}