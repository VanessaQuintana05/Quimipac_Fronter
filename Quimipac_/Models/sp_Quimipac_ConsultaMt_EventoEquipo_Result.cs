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
    
    public partial class sp_Quimipac_ConsultaMt_EventoEquipo_Result
    {
        public int Id_Equipo_Evento { get; set; }
        public Nullable<int> Id_Equipo { get; set; }
        public string Id_Usuario { get; set; }
        public Nullable<int> Tipo_Evento { get; set; }
        public Nullable<System.DateTime> Fecha_Ini { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public string Proveedor { get; set; }
        public Nullable<decimal> Valor { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> Id_Persona_Origen { get; set; }
        public Nullable<int> Id_Persona_Destino { get; set; }
        public string Descripcion { get; set; }
    }
}
