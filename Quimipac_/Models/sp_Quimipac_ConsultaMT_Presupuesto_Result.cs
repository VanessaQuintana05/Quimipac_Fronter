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
    
    public partial class sp_Quimipac_ConsultaMT_Presupuesto_Result
    {
        public int Id_Presupuesto { get; set; }
        public string Id_Empresa { get; set; }
        public string Id_Sucursal { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Id_Usuario { get; set; }
        public string Comentario { get; set; }
        public Nullable<decimal> Porcentaje_indirectos { get; set; }
        public Nullable<decimal> Monto_Certificacion { get; set; }
        public Nullable<decimal> Iva_Certificacion { get; set; }
        public string Validez { get; set; }
        public string Moneda { get; set; }
        public string CIA_DESCRIPCION { get; set; }
        public string nom_cli { get; set; }
        public string user_id { get; set; }
        public string nombre { get; set; }
    }
}