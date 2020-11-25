using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Sp_Quimipac_ConsultaUsuarios_Rol
    {
        public string User_id { get; set; }
        public string User_clave { get; set; }
        public string User_descrip { get; set; }
        public string User_status { get; set; }
        public string User_dep { get; set; }
        public string User_cargo { get; set; }
        public string User_cheq_nivel { get; set; }
        public string User_cheq_maquina { get; set; }
        public string User_uni_inc { get; set; }
        public string Elimina { get; set; }
        public string Email { get; set; }
        /**/
        public Nullable<Decimal> Id_persona { get; set; }
        public string Primer_nombre { get; set; }
        public string Segundo_nombre { get; set; }
        public string Primer_apellido { get; set; }
        public string Segundo_apellido { get; set; }
        public Nullable<System.DateTime> Fecha_nacimiento { get; set; }
        public string Genero { get; set; }
        public string Estado_civil { get; set; }
        public string Tipo_identifica1 { get; set; }
        public string Nro_identifica1 { get; set; }
        public string Tipo_identifica2 { get; set; }
        public string Nro_identifica2 { get; set; }
        public string Correo { get; set; }
        public string Nacionalidad { get; set; }
        public string Correo_laboral { get; set; }
        public string Tipo_identifica3 { get; set; }
        public string Nro_identifica3 { get; set; }
        /**/
        public Nullable<int> Id_Rol_Usuario { get; set; }
        public string Id_Usuario { get; set; }
        public Nullable<int> Id_Rol { get; set; }
        public string Estado_UsuarioRol { get; set; }
        /**/
        public Nullable<int> Id_roles { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}