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
    
    public partial class sp_Quimipac_ConsultaMT_EntregaOrdenTrabajo_Result
    {
        public int Id_Entrega_Orden_Trabajo { get; set; }
        public string Id_Empresa { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<System.DateTime> Fecha_Elaboracion { get; set; }
        public Nullable<System.DateTime> Fecha_Envio { get; set; }
        public Nullable<System.DateTime> Fecha_Recepcion { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public string Recibido_Por { get; set; }
        public string CIA_DESCRIPCION { get; set; }
        public string nom_cli { get; set; }
    }
}
