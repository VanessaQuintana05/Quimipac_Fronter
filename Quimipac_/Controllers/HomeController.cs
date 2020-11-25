using Quimipac_.Models;
using Quimipac_.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quimipac_.Controllers
{
    public class HomeController : Controller
    {





        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

        //INICIAR SESION
        #region
        [HttpGet]
        public ActionResult IniciarSesion()


        {
            try
            {
                Login user = new Login();
                List<SelectListItem> items = new List<SelectListItem>();

                var empresas = db.sp_LINK_ConsultaEmpresas().ToList();
                foreach (var empresa in empresas)
                {
                    items.Add(new SelectListItem { Value = Convert.ToString(empresa.CIA_CODIGO), Text = empresa.CIA_DESCRIPCION });
                }
                SelectList selectList = new SelectList(items, "Value", "Text");
                ViewBag.empresas = selectList;

                return View("IniciarSesion", user);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpPost]
        public ActionResult IniciarSesion([Bind(Include = "User_id,User_clave,Id_empresa,Id_moneda")] Login usuario)
        {
            try
            {
                if (ModelState.IsValidField("User_id") && ModelState.IsValidField("User_clave"))
                //sp_LINK_ConsultaCredencialUsuario
                {
                    var persona = db.sp_LINK_ConsultaCredencialUsuario(usuario.User_id, usuario.Id_empresa).FirstOrDefault();

                    if (persona != null)
                    {

                        //Prueba dClave 
                        var dbe = new DataBase_Externo();

                        string claveoriginal_descifrada = dbe.descifrar(usuario.User_clave.ToUpper(), 5);
                        string claveoriginal_cifrada = dbe.cifrar(usuario.User_clave.ToUpper(), 5);
                        string clave_cifrada_10 = string.Empty;
                        if (claveoriginal_cifrada.Length > 10)
                        {
                            clave_cifrada_10 = claveoriginal_cifrada.Substring(0, 10);
                        }
                        else
                        {
                            clave_cifrada_10 = claveoriginal_cifrada;
                        }

                        //if (persona.user_clave == usuario.User_clave)

                        if (persona.user_clave == clave_cifrada_10)
                        {

                            var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(persona.user_id, 0, usuario.Id_empresa).ToList();

                            System.Web.HttpContext.Current.Session["opciones_padres"] = opciones_padres;
                            System.Web.HttpContext.Current.Session["usuario"] = persona.user_id;
                            System.Web.HttpContext.Current.Session["clave"] = persona.user_clave;
                            System.Web.HttpContext.Current.Session["nombres"] = persona.user_descrip;
                            System.Web.HttpContext.Current.Session["empresa"] = usuario.Id_empresa;
                            var nombre_empresa = db.sp_LINK_ConsultaEmpresas_Nombre(usuario.Id_empresa).FirstOrDefault();
                            System.Web.HttpContext.Current.Session["empresa_Nombre"] = nombre_empresa.CIA_DESCRIPCION;


                            //System.Web.HttpContext.Current.Session["Permiso_Btn"] = db.MT_Permiso.Where(vq => vq.Id_Usuario == persona.user_id && vq.Id_Empresa == usuario.Id_empresa && vq.Estado == "A").Take(1).ToList();
                            //System.Web.HttpContext.Current.Session["ContratosxUsuaroLI"] = db.MT_Permiso.Where(vq => vq.Id_Usuario == persona.user_id && vq.Id_Empresa == usuario.Id_empresa && vq.Id_Contrato != null && vq.Estado == "A").ToList();
                            //System.Web.HttpContext.Current.Session["ProyectosxUsuarioLI"] = db.MT_Permiso.Where(vq => vq.Id_Usuario == persona.user_id && vq.Id_Empresa == usuario.Id_empresa && vq.Id_Proyecto != null && vq.Estado == "A").ToList();

                            return RedirectToAction("Default", "Main");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Contraseña no coincide");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Usuario no existe");
                    }
                }

                List<SelectListItem> items = new List<SelectListItem>();
                var empresas = db.sp_LINK_ConsultaEmpresas().ToList();
                foreach (var empresa in empresas)
                {
                    items.Add(new SelectListItem { Value = Convert.ToString(empresa.CIA_CODIGO), Text = empresa.CIA_DESCRIPCION });
                }
                SelectList selectList = new SelectList(items, "Value", "Text");
                ViewBag.empresas = selectList;

                return View(usuario);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //CIERRE DE SESION
        #region
        [HttpGet]
        public ActionResult CierraSesion()
        {
            try
            {
                System.Web.HttpContext.Current.Session["opciones_padres"] = null;
                System.Web.HttpContext.Current.Session["usuario"] = null;
                System.Web.HttpContext.Current.Session["clave"] = null;
                System.Web.HttpContext.Current.Session["nombres"] = null;
                System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = null;
                System.Web.HttpContext.Current.Session["id_MTActividad"] = null;
                return RedirectToAction("IniciarSesion", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        #endregion

        [HttpPost]
        public JsonResult PaginacionSession(int id)
        {
            try
            {
                var mensaje = new JsonResult();
                mensaje.Data = "";
                System.Web.HttpContext.Current.Session["PaginacionID"] = (id > 0) ? id : 0;
                mensaje.Data = id;

                return mensaje;//JsonRequestBehavior();
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}