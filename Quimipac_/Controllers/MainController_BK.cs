using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;

namespace Quimipac_.Controllers
{
    public class MainController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        //PÁGINA PRINCIPAL DEL DASHBOARD
        #region
        [HttpGet]
        public ActionResult Default()
        {

			return View();
        }
        #endregion

        //pagina auxiliar 
        #region
        [HttpGet]
        public ActionResult LeerNotificacion()
        {

			try
			{
								
				var listaNotificacion = db.sp_Quimipac_Consulta_Notificaciones_Entrada("General",0).ToList();
				ViewBag.listaNotificacion = listaNotificacion;

				return View();
			}
			catch (Exception e)
			{
				return RedirectToAction("Error", "Errores");
			}
		}
        #endregion

       

    }
}