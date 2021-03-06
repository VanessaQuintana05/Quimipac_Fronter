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
    using System.Collections.Generic;
    
    public partial class MT_Planilla_Detalle
    {
        public int Id_Planilla_Detalle { get; set; }
        public Nullable<int> Id_Planilla { get; set; }
        public string Codigo_acta { get; set; }
        public string Base_Admnistrativa { get; set; }
        public string Descripcion_de_Acta { get; set; }
        public Nullable<decimal> Codigo_Periodo { get; set; }
        public string Descripcion_de_Periodo { get; set; }
        public string Codigo_Tipo_de_Acta { get; set; }
        public string Descripcion_de_Tipo_de_Acta { get; set; }
        public Nullable<System.DateTime> Fecha_Inicial_Periodo { get; set; }
        public Nullable<System.DateTime> Fecha_Final_Periodo { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre_Acta { get; set; }
        public string Estado_Acta { get; set; }
        public string Descripcion_de_Estado_Acta { get; set; }
        public string Orden_Raiz { get; set; }
        public string No_Orden_Padre { get; set; }
        public string No_Orden { get; set; }
        public string Codigo_Detalle_Acta { get; set; }
        public string Código_Items { get; set; }
        public string Descripcion_de_Items { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public string Codigo_Unidad { get; set; }
        public string Unidad { get; set; }
        public Nullable<decimal> Valor_Unitario { get; set; }
        public Nullable<decimal> Valor_Total { get; set; }
        public string Codigo_Listado_de_Costo { get; set; }
        public string Descripcion_Listado_de_Costo { get; set; }
        public string Tipo_Generacion { get; set; }
        public string Codigo_de_Plan_de_Condicion { get; set; }
        public string Tipo_de_Trabajo { get; set; }
        public string Descripcion_de_Tipo_de_Trabajo { get; set; }
        public string Actividad { get; set; }
        public Nullable<System.DateTime> Fecha_Asignacion { get; set; }
        public Nullable<System.DateTime> Fecha_Fin_Permiso_Municipal_Calc { get; set; }
        public Nullable<System.DateTime> Fecha_Asignacion_OT { get; set; }
        public Nullable<System.DateTime> Fecha_Ejecucion { get; set; }
        public Nullable<System.DateTime> Fecha_Legalizacion { get; set; }
        public Nullable<System.DateTime> Fecha_Ejecucion_OT_Padre { get; set; }
        public Nullable<decimal> Tiempo_Promedio_Inc_Desc_OT { get; set; }
        public Nullable<decimal> Tiempo_Promedio_Desc_Descarga { get; set; }
        public Nullable<decimal> Tiempo_Transcurrido_HORAS { get; set; }
        public Nullable<decimal> Tiempo_Optimo_Incentivo { get; set; }
        public Nullable<decimal> Tiempo_Máximo_Incentivo { get; set; }
        public Nullable<decimal> Porcentaje_Incentivo { get; set; }
        public Nullable<decimal> Tiempo_Optimo_Desc_Atencion { get; set; }
        public Nullable<decimal> Tiempo_Máximo_Desc_Atencion { get; set; }
        public Nullable<decimal> Porcentaje_de_Desc_Atencion { get; set; }
        public Nullable<decimal> Tiempo_Máximo_Desc_Descarga { get; set; }
        public Nullable<decimal> Tiempo_Excede_Desc_Descarga { get; set; }
        public Nullable<decimal> Porcentaje_Desc_Descarga { get; set; }
        public Nullable<decimal> Valor_Aplica_Desc_Atencion_OT { get; set; }
        public string Codigo_Contrato { get; set; }
        public string Descripcion_Contrato { get; set; }
        public string Tipo_de_Contrato { get; set; }
        public string Descripcion_de_Tipo_de_Contrato { get; set; }
        public string Contratista { get; set; }
        public string Descripcion_Contratista { get; set; }
        public Nullable<decimal> Porcentaje_Costo_Indirecto_Contrato { get; set; }
        public Nullable<decimal> Porcentaje_Fondo_Garantia_Contrato { get; set; }
        public Nullable<decimal> Porcentaje_Amortizacion_Contrato { get; set; }
        public Nullable<decimal> Descuento_General { get; set; }
        public string Codigo_Unidad_Operativa { get; set; }
        public string Descripcion_Unidad_Operativa { get; set; }
        public string Terminal { get; set; }
        public string Valor_Base { get; set; }
        public string Cumplimiento { get; set; }
        public Nullable<decimal> Tiempo_Contractual_HORAS { get; set; }
        public Nullable<decimal> Tiempo_de_Permiso_Municipal_HORAS { get; set; }
        public string Tipo_Clasifica { get; set; }
        public string Inventario { get; set; }
        public string Tiene_Caso_Especial { get; set; }
        public string Tipoe_de_Relacion { get; set; }
        public string Nro_Contrato { get; set; }
        public string Nro_Producto { get; set; }
        public string Codigo { get; set; }
        public string Tipo_de_Irregularidad { get; set; }
        public string Descripcion_de_Proyecto { get; set; }
        public string Usuario_Finalizo { get; set; }
        public string Codigo_Tipo_Comentario { get; set; }
        public string Comentario { get; set; }
        public string Observacion_Orden_Actividad { get; set; }
        public string Fecha_Pago_Sistema_Externo { get; set; }
        public string Numero_Factura_Sistema_Externo { get; set; }
        public string Cod_estado_orden { get; set; }
    }
}
