using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
	public class ParametrosSmtp
	{
		public int Id_TablaDetalle { get; set; }
		public int Id_Tabla { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public string Estado { get; set; }
		public int Id_Padre { get; set; }
	}
}