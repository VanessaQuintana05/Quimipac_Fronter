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
    
    public partial class sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos_Result
    {
        public int Id_Contrato { get; set; }
        public string Id_Empresa { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public string Codigo_Interno { get; set; }
        public string Codigo_Cliente { get; set; }
        public string Id_Usuario { get; set; }
        public string Id_Linea { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> Estado { get; set; }
        public Nullable<int> Dia_Plazo { get; set; }
        public Nullable<int> tipo { get; set; }
        public Nullable<int> Contrato_Padre { get; set; }
        public Nullable<decimal> Valor_Referencial { get; set; }
        public Nullable<decimal> monto { get; set; }
        public Nullable<decimal> costo { get; set; }
        public Nullable<int> Responsable { get; set; }
        public Nullable<int> Secuencial { get; set; }
        public string Codigo_Interno_Ant { get; set; }
        public string Observaciones { get; set; }
        public string Codigo_interno_padre { get; set; }
        public Nullable<System.DateTime> Fecha_registro { get; set; }
        public Nullable<System.DateTime> Fecha_modificacion { get; set; }
        public string Localidad { get; set; }
        public Nullable<System.DateTime> Fecha_Aprobacion_Cot { get; set; }
        public string Recepcion_Servicio { get; set; }
        public Nullable<System.DateTime> Fecha_Firma_Conformidad { get; set; }
        public Nullable<System.DateTime> Fecha_Cumplimiento_Inst { get; set; }
        public Nullable<int> Referencia { get; set; }
        public string lineaContrato { get; set; }
        public string nombreCliente { get; set; }
        public string descripcionEstado { get; set; }
        public string CIA_Descripcion { get; set; }
    }
}
