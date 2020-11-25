using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;
using Quimipac_.Repositorio;
using System.Net.Mail;

namespace Quimipac_.Controllers
{
	public class NotificacionesController : Controller
	{
		private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
		#region 
		[HttpGet]
		public ActionResult Notificaciones()
		{
			try
			{
				var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(82).ToList();
				ViewBag.listaNotificaciones = listaNotificaciones;

				return View();
			}
			catch (Exception e)
			{
				return RedirectToAction("Error", "Errores");
			}
		}
		// agregar notificacion
		[HttpGet]
		public ActionResult Agregar_Notificacion()
		{
			try
			{
				List<SelectListItem> itemsTipo = new List<SelectListItem>();
				List<SelectListItem> itemsPrioridad = new List<SelectListItem>();
				List<SelectListItem> itemsEstado = new List<SelectListItem>();
				List<SelectListItem> items_Tipo = new List<SelectListItem>();

				
				var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(43).ToList();

				
				foreach (var tipo in listaTipos)
				{
                    if (tipo.Descripcion.Equals("General"))
                    {
						itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = true });
					}
				}


				SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");


				var listaPrioridad = db.sp_Quimipac_ConsultaMT_TablaDetalle(44).ToList();
				foreach (var Prioridad in listaPrioridad)
				{
					itemsPrioridad.Add(new SelectListItem { Value = Convert.ToString(Prioridad.Id_TablaDetalle), Text = Prioridad.Descripcion });
				}
				SelectList selectlistaPrioridad = new SelectList(itemsPrioridad, "Value", "Text");

				var listaEstado = db.sp_Quimipac_ConsultaMT_TablaDetalle(48).ToList();
				foreach (var estado in listaEstado)
				{
					if (estado.Descripcion.Equals("No Leido"))
					{
						itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected=true});
						break;
					}
				}


				SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");

				var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(47).ToList();
				foreach (var tipos in listaTipo)
				{
					
						items_Tipo.Add(new SelectListItem { Value = Convert.ToString(tipos.Id_TablaDetalle), Text = tipos.Descripcion });
					
				}


				SelectList selectlistaTipos = new SelectList(items_Tipo, "Value", "Text");

				ViewBag.listaTipo = selectlistaTipo;
				ViewBag.listaPrioridad = selectlistaPrioridad;
				ViewBag.listaEstado = selectlistaEstado;
				ViewBag.listaTipos = selectlistaTipos;

				return View();
			}
			catch (Exception e)
			{
				return RedirectToAction("Error", "Errores");
			}
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult Agregar_Notificacion([Bind(Include = "Tipo_Notificacion,Id_Codigo_Origen,Id_usuario,Fecha,Prioridad,Asunto,Mensaje,Criterio,Id_Notificacion,Id_Persona,Tipo,Correo, Fecha_Hora,Estado")] InsertNotificacion mT_Notificacion)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var Notificaciones = db.MT_Notificaciones.Where(x => x.Asunto == mT_Notificacion.Asunto && x.Mensaje == mT_Notificacion.Mensaje);
					if (Notificaciones.Count() >= 1)
					{
						TempData["mensaje_error"] = "Notificacion ya existe";
						return RedirectToAction("Notificaciones");
					}
					else
					{
						var user_id = System.Web.HttpContext.Current.Session["usuario"];

						if (user_id == null)
						{
							return Redirect(Url.Action("IniciarSesion", "Home"));
						}
						else
						{

							mT_Notificacion.Id_usuario = user_id.ToString();
							mT_Notificacion.Fecha = DateTime.Now;
							//db.MT_Notificaciones.Add(mT_Notificacion);
							//db.SaveChanges();

							var dbe = new DataBase_Externo();
							
							dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado,0);
							// servidor de correo 
								int i = 0;// variable a contar
								var SMTP1 = dbe.LkParametrosSMTP();// variable que trae resultados del repositorio
								int n = SMTP1.Count();// variable que vaa contar los campos
								string[] VSMTP = new string[n]; //declaracion y instanciar arreglo
								foreach (var item in SMTP1)// recorrer el arreglo
								{
									VSMTP[i] = item.Descripcion;//recorre el arreglo en cada posicion y guarda datos del sp en cada posicion
								i++;
								}
								SmtpClient client = new SmtpClient(VSMTP[2], Convert.ToInt32(VSMTP[4]));// servidor y puerto
								MailMessage message = new MailMessage();
								message.From = new MailAddress(mT_Notificacion.Correo, VSMTP[3]);// correo a usuario enviar, correo del que envia 
								message.To.Add(mT_Notificacion.Correo);
								message.Subject = mT_Notificacion.Asunto;

								//... Modificar el cuerpo(campos) que se inserto en la tabla del servidor
								string htmlParametro = VSMTP[8];
								//htmlParametro = htmlParametro.Replace("{Usuario}", mT_Notificacion.Correo);
								htmlParametro = htmlParametro.Replace("{Asunto}", mT_Notificacion.Asunto);
								message.Body = htmlParametro;

								//client.UseDefaultCredentials = true;
								if (Convert.ToInt32(VSMTP[11]) == 0) { message.IsBodyHtml = false; }
								else { message.IsBodyHtml = true; }

								if (Convert.ToInt32(VSMTP[5]) == 0) { client.EnableSsl = false; }
								else { client.EnableSsl = true; }
								//
								client.Credentials = new System.Net.NetworkCredential(VSMTP[6], VSMTP[7]);
								client.Send(message);
								//MT_TablaDetalle[] Vsmtp = SMTP.ToArray();
							TempData["mensaje_correcto"] = "Notificacion guardada";
							return RedirectToAction("Notificaciones");
						}
					
					}

				}
				else
				{
					TempData["mensaje_error"] = "Valores incorrectos";
					return RedirectToAction("Notificaciones");
				}
			}
			catch (Exception e)
			{
				return RedirectToAction("Error", "Errores");
			}
		}
		#endregion
	}

}
