using Quimipac_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quimipac_.Controllers
{
    public class ErroresController : Controller
    {
        BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

        //CONTROL DE ERRORES
        #region
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Error(string msj)
        {
            return View();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}