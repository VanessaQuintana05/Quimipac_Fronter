using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class ClsDatosUsuario
    {

        //public ClsDatosUsuario() { }


        public string user_Id { get; set; }
        public string user_Clave { get; set; }
        public string user_descrip { get; set; }
        public Nullable<System.DateTime> user_fec_crea { get; set; }
        public Nullable<System.DateTime> user_fec_virgen { get; set; }
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
        public TimeSpan hora_inicio { get; set; }
        public TimeSpan hora_fin { get; set; }
        public string elimina { get; set; }
        public string cia_codigo { get; set; }
        public string email { get; set; }
        public decimal id_persona { get; set; }
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
        public string curriculum_pdf { get; set; }
        public string nacionalidad { get; set; }
        public string correo_laboral { get; set; }
        public string tipo_identifica3 { get; set; }
        public string nro_identifica3 { get; set; }
        public Nullable<System.DateTime> fecha_expedicion { get; set; }
        public Nullable<System.DateTime> fecha_caducidad { get; set; }


        //public string USRUserId { get; set; }
        //public string USRUserClave { get; set; }
        //public string USRUserDescripcion { get; set; }
        //public string PERNombres { get; set; }
        //public string PERApellidos { get; set; }
        //public Nullable<System.DateTime> PERFechaNaci { get; set; }
        //public string PERGenero { get; set; }
        //public string PEREmail { get; set; }
    }
}