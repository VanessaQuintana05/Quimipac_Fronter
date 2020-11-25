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
    
    public partial class MT_Proyecto_Actividad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MT_Proyecto_Actividad()
        {
            this.MT_Proyecto_Actividad_Anexo = new HashSet<MT_Proyecto_Actividad_Anexo>();
            this.MT_Proyecto_Actividad_Equipo = new HashSet<MT_Proyecto_Actividad_Equipo>();
            this.MT_Proyecto_Actividad_Integrante = new HashSet<MT_Proyecto_Actividad_Integrante>();
            this.MT_Proyecto_Actividad_Novedad = new HashSet<MT_Proyecto_Actividad_Novedad>();
            this.MT_Proyecto_Actividad_Valor = new HashSet<MT_Proyecto_Actividad_Valor>();
            this.MT_Proyecto_ActividadEgreso = new HashSet<MT_Proyecto_ActividadEgreso>();
        }
    
        public int Id_Proyecto_Actividad { get; set; }
        public Nullable<int> Id_Proyecto { get; set; }
        public Nullable<int> Id_Actividad { get; set; }
        public Nullable<int> Estado { get; set; }
        public Nullable<int> Orden { get; set; }
        public Nullable<System.DateTime> Fecha_Ini { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> Id_Planilla { get; set; }
        public Nullable<int> Estado_Actividad_Plantilla { get; set; }
        public string EstadoAct { get; set; }
    
        public virtual MT_Proyecto MT_Proyecto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_Actividad_Anexo> MT_Proyecto_Actividad_Anexo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_Actividad_Equipo> MT_Proyecto_Actividad_Equipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_Actividad_Integrante> MT_Proyecto_Actividad_Integrante { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_Actividad_Novedad> MT_Proyecto_Actividad_Novedad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_Actividad_Valor> MT_Proyecto_Actividad_Valor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MT_Proyecto_ActividadEgreso> MT_Proyecto_ActividadEgreso { get; set; }
    }
}
