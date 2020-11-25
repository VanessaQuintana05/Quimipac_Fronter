using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class Permisos_AccionUsuario
    {
        public Permisos_AccionUsuario()
        {
            Consultar = false; Modificar = false; 
            Crear = false; Eliminar = false; Aprobar = false;
        }
        public bool Consultar{ get; set; }
        public bool Modificar { get; set; }
        public bool Crear { get; set; }
        public bool Eliminar { get; set; }
        public bool Aprobar { get; set; }
    }
        public class Permisos_Usuario
    {
        //sp_LINK_ConsultaUsuarios_Result
        public string user_id { get; set; }
        public string user_clave { get; set; }
        public string user_descrip { get; set; }
        public Nullable<System.DateTime> user_fec_crea { get; set; }
        public Nullable<System.DateTime> user_fec_vigen { get; set; }
        public Nullable<System.DateTime> user_fec_termi { get; set; }
        public string user_status { get; set; }
        public Nullable<System.DateTime> user_fec_vclave { get; set; }
        public int user_tiempo_cclave { get; set; }
        public string user_mult_log { get; set; }
        public string user_dep { get; set; }
        public string user_cargo { get; set; }
        public string user_cheq_nivel { get; set; }
        public string user_cheq_maquina { get; set; }
        public string user_uni_inc { get; set; }
        public Nullable<System.TimeSpan> hora_inicio { get; set; }
        public Nullable<System.TimeSpan> hora_fin { get; set; }
        public string elimina { get; set; }
        public string email { get; set; }
        public Nullable<decimal> id_persona { get; set; }
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        public Nullable<System.DateTime> fecha_nacimiento { get; set; }
        public string genero { get; set; }
        public string estado_civil { get; set; }
        public string tipo_identifica1 { get; set; }
        public string nro_identifica1 { get; set; }
        public string tipo_identifica2 { get; set; }
        public string nro_identifica2 { get; set; }
        public string correo { get; set; }
        public byte[] curriculum_pdf { get; set; }
        public string nacionalidad { get; set; }
        public string correo_laboral { get; set; }
        public string tipo_identifica3 { get; set; }
        public string nro_identifica3 { get; set; }
        public Nullable<System.DateTime> fecha_expedicion { get; set; }
        public Nullable<System.DateTime> fecha_caducidad { get; set; }
        //MT_Permiso
        public int Id_Permiso { get; set; }
        public string Id_Usuario { get; set; }
        public string Id_Cliente { get; set; }
        public Nullable<int> Id_Contrato { get; set; }
        public Nullable<int> Id_Proyecto { get; set; }
        public Nullable<int> Id_Tipo_Trabajo { get; set; }
        public string Id_Linea { get; set; }
        public string Consultar { get; set; }
        public string Modificar { get; set; }
        public string Crear { get; set; }
        public string Eliminar { get; set; }
        public string Aprobar { get; set; }
        public string Usuario { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public string Estado { get; set; }
    }
}