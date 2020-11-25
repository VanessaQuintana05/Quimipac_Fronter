using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    /*public class Permisos_Rol
    {
        public int Id_Rol { get; set; }
        public string Descripcion { get; set; }
        public string EstadoRol { get; set; }

        public int Id_Permiso { get; set; }
        //public string Id_Usuario { get; set; }
        public string[] Id_Usuario { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<int> Id_Contrato { get; set; }
        public Nullable<int> Id_Proyecto { get; set; }
        public Nullable<int> Id_Tipo_Trabajo { get; set; }
        public string Id_Linea { get; set; }
        public string Consultar { get; set; }
        public string Modificar { get; set; }
        public string Crear { get; set; }
        public string Eliminar { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public string Estado { get; set; }
    
    }*/

    public class Permisos_Rol
    {
        public int Id_Rol { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public int Id_Rol_Usuario { get; set; }
        public string Id_Usuario { get; set; }
        public int Id_Rol_InUsuario { get; set; }
        public string Estado_Usuario { get; set; }

        public List<UsuarioyRoles> RolesUsuarioLI { get; set; }
       
    }
    public class UsuarioyRoles
    {
        public int Id_Rol_Usuario { get; set; }
        public string Id_Usuario { get; set; }
        public int Id_Rol_InUsuario { get; set; }
        public string Estado_Usuario { get; set; }
        public string DescripcionRol { get; set; }
    }

}