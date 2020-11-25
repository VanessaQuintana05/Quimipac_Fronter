using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
	public class InsertNotificacion
	{
		public int Tipo_Notificacion { get; set; }
		//public string Id_Codigo_Origen { get; set; }
		public string Id_usuario { get; set; }
		public DateTime Fecha { get; set; }
		public int Prioridad { get; set; }
		public string Asunto { get; set; }
		public string Mensaje { get; set; }
		public int Criterio { get; set; }
		//tabla notificacion persona 
		public int Id_Notificacion { get; set; }
		public int Id_Persona { get; set; }
		public int Tipo { get; set; }
		public string Correo { get; set; }
		public int Estado { get; set; }
		public int Frecuencia { get; set; }
		public int Enviado { get; set; }
		public DateTime Fecha_Envio { get; set; }
		public bool Reenvio { get; set; } 
		public string EmpresaID { get; set; }

	}
}