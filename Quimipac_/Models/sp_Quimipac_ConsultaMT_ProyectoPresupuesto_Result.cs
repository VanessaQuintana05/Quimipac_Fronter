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
    
    public partial class sp_Quimipac_ConsultaMT_ProyectoPresupuesto_Result
    {
        public int Id_Proyecto { get; set; }
        public string Id_Empresa { get; set; }
        public string Id_Sucursal { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<int> Id_TipoTrabajo { get; set; }
        public Nullable<int> Id_Presupuesto { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public Nullable<System.DateTime> Fecha_Prorroga { get; set; }
        public string Estado { get; set; }
        public string Codigo_Interno { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public string Comentario { get; set; }
        public string Porcentaje_avance { get; set; }
        public Nullable<decimal> Valor_Inicial { get; set; }
        public Nullable<decimal> Valor_Ajustado { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public Nullable<System.DateTime> Fecha_Anticipo { get; set; }
        public Nullable<int> Id_contrato { get; set; }
        public Nullable<int> Id_Prospecto { get; set; }
        public Nullable<int> Proyecto_Padre { get; set; }
        public Nullable<int> Dia_Plazo { get; set; }
        public Nullable<int> Secuencial { get; set; }
    }
}
