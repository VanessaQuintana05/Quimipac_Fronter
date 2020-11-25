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
    public class ContratoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        //CONTRATO 


        #region
        [HttpGet]
        public ActionResult Contrato()
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresacod = empresa_id.ToString();

                var listaContrato = db.sp_Quimipac_ConsultaMT_ContratoGeneral(empresacod).ToList();
                ViewBag.listaContrato = listaContrato;

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        public ActionResult Agregar_Contrato()
        {
            try
            {
                List<SelectListItem> itemsEstado = new List<SelectListItem>();
                List<SelectListItem> itemsCliente = new List<SelectListItem>();
                List<SelectListItem> itemsLinea = new List<SelectListItem>();
                

                var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                foreach (var tipo in listaEstados)
                {
                    itemsEstado.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }


                SelectList selectlistaEstado= new SelectList(itemsEstado, "Value", "Text");


                var listacliente = db.sp_LINK_ConsultaClientes().ToList();
                foreach (var cliente in listacliente)
                {
                    itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistacliente = new SelectList(itemsCliente, "Value", "Text");


                var listaLinea = db.sp_LINK_ConsultaLineas().ToList();
                foreach (var linea in listaLinea)
                {
                    itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                }


                SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");
                

                ViewBag.listaEstados = selectlistaEstado;
                ViewBag.listacliente = selectlistacliente;
                ViewBag.listaLinea = selectlistaLinea;
                



                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
         }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Contrato([Bind(Include = " Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado")] MT_Contrato mT_Contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contratos = db.MT_Contrato.Where(x => x.Codigo_Cliente == mT_Contrato.Codigo_Cliente && x.Nombre == mT_Contrato.Nombre && x.Categoria == mT_Contrato.Categoria && x.Subcategoria == mT_Contrato.Subcategoria && x.Id_Linea == mT_Contrato.Id_Linea && x.Id_Cliente == mT_Contrato.Id_Cliente);
                    if (contratos.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Contrato ya existe";
                        return RedirectToAction("Contrato");
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

                            mT_Contrato.Id_Usuario = user_id.ToString();
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Contrato.Id_Empresa = empresa_id.ToString();
                            db.MT_Contrato.Add(mT_Contrato);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Contrato guardado";
                        return RedirectToAction("Contrato");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Contrato");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Contrato(int id)
        {
            try
            {

                MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                bool seleccion = false;
                if (mT_Contrato != null)
                {
                   

                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemsCliente = new List<SelectListItem>();
                    List<SelectListItem> itemsLinea = new List<SelectListItem>();


                    var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                    foreach (var tipo in listaEstados)
                    {
                        if (tipo.Id_TablaDetalle == mT_Contrato.Estado)
                        {
                            seleccion = true;
                        }

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");



                   

                    var listacliente = db.sp_LINK_ConsultaClientes().ToList();

                    foreach (var cliente in listacliente)
                    {
                        if (cliente.cod_cli == mT_Contrato.Id_Cliente)
                        {
                            seleccion = true;
                        }

                        itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }

                    SelectList selectlistaCliente = new SelectList(itemsCliente, "Value", "Text");




                    var listaLinea = db.sp_LINK_ConsultaLineas().ToList();

                    foreach (var linea in listaLinea)
                    {
                        if (linea.codigo == mT_Contrato.Id_Linea)
                        {
                            seleccion = true;
                        }

                        itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");

                    ViewBag.listaEstados = selectlistaEstado;
                    ViewBag.listacliente = selectlistaCliente;
                    ViewBag.listaLinea = selectlistaLinea;



                    return View(mT_Contrato);

                }
                else
                {
                    TempData["mensaje_error"] = "Contrato no existe";
                    return RedirectToAction("Contrato");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Contrato([Bind(Include = "Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado")] MT_Contrato mT_Contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //int bandera = 1;
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        mT_Contrato.Id_Empresa = empresa_id.ToString();
                        mT_Contrato.Id_Usuario = user_id.ToString();
                        //mT_Contrato.Fecha_Fin = DateTime.Now;
                        //mT_Contrato.Fecha_Inicio = DateTime.Now;


                        db.Entry(mT_Contrato).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Contrato actualizado";

                        return RedirectToAction("Contrato");
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Contrato");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        #endregion


        //FORMULARIOS DE CONTRATO

        #region
        [HttpGet]
        public ActionResult Formularios_Contrato(int id)
        {
            try
            {
                MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                if (mT_Contrato != null)
                {
                    System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                    var listaFormulariosC = db.sp_Quimipac_ConsultaMT_ContratoDocumentado(id).ToList();
                    ViewBag.listaFormulariosC = listaFormulariosC;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Contrato");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }



        [HttpGet]
        public FileResult DescargarArchivoFormularioC(string nombre)
        {
            try
            {
                var ruta = Server.MapPath("~/Formularios_Actividad/" + nombre);
                var contentType = MimeMapping.GetMimeMapping(nombre);
                FilePathResult archivo = File(ruta, contentType, nombre);
                return archivo;

            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpGet]
        public ActionResult AgregarFormulario_Contrato()
        {
            try
            {
                List<SelectListItem> itemsTipoC = new List<SelectListItem>();
               
                var listaTipoC = db.sp_Quimipac_ConsultaMT_TablaDetalle(45).ToList();
                foreach (var tipo in listaTipoC)
                {
                    itemsTipoC.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                SelectList selectlistaTipoC = new SelectList(itemsTipoC, "Value", "Text");
                

                ViewBag.listaTipoC = selectlistaTipoC;
             
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarFormulario_Contrato([Bind(Include = "Id_Contrato,Descripcion,Enlace,Fecha_Validez,Estado,NombreArchivo,Tipo")] MT_Contrato_Documentado mT_FormularioC, HttpPostedFileBase NombreArchivo)
        {
            try
            {
                var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];
                
                if (id_Contrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }
                else
                {
                  int idContrato = int.Parse(id_Contrato.ToString());

                if (ModelState.IsValid)
                {
                    var FormularioC = db.MT_Contrato_Documentado.Where(x => x.Estado == mT_FormularioC.Estado && x.NombreArchivo == mT_FormularioC.NombreArchivo && x.Tipo == mT_FormularioC.Tipo);
                    if (FormularioC.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Formulario ya existe";
                        return RedirectToAction("Contrato", new { id = idContrato });
                    }
                    else
                    {
                            if (NombreArchivo != null)
                            {
                                var ruta_archivo = @"~/Formularios_Actividad/";
                                var ruta_servidor = Path.GetFullPath(ruta_archivo);
                                var extension = Path.GetExtension(NombreArchivo.FileName);

                                int fileSize = NombreArchivo.ContentLength;

                                mT_FormularioC.Id_Contrato = idContrato;
                                mT_FormularioC.NombreArchivo = "";
                                mT_FormularioC.Fecha_Registro = DateTime.Now;
                                mT_FormularioC.Estado = mT_FormularioC.Estado.Substring(0, 1);
                                mT_FormularioC.Enlace = ruta_servidor;
                                db.MT_Contrato_Documentado.Add(mT_FormularioC);
                                db.SaveChanges();

                                int scope_identity_id = mT_FormularioC.Id_Contrato_Documentado;

                                string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                                mT_FormularioC.NombreArchivo = nombreArchivo;

                                NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                                db.Entry(mT_FormularioC).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Formulario de Contrato guardado";
                                return RedirectToAction("Formularios_Contrato", new { id = idContrato });
                            }
                            else
                            {
                                return View(mT_FormularioC);
                            }

                        }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Formularios_Contrato", new { id = idContrato });
                }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarFormularioContrato(int id)
        {
            try
            {


                var id_MTContrato = System.Web.HttpContext.Current.Session["id_Contrato"];

                if (id_MTContrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMTActividad = int.Parse(id_MTContrato.ToString());

                    MT_Contrato_Documentado mT_Formulario = db.MT_Contrato_Documentado.Find(id);

                    if (mT_Formulario != null)
                    {
                        mT_Formulario.Estado = "E";
                        db.Entry(mT_Formulario).State = EntityState.Modified;
                        //db.MT_Formulario.Remove(mT_Formulario);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Archivo eliminado";
                        return RedirectToAction("Formularios_Contrato", new { id = id_MTContrato });
                    }
                    else
                    {
                        TempData["mensaje_correcto"] = "Código no existe";
                        return RedirectToAction("Formularios_Contrato", new { id = id_MTContrato });
                    }

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        #endregion
                
        //ALERTA DE CONTRATO
        #region

        [HttpGet]
        public ActionResult Alerta_Contrato(int id)
        {
            try
            {
                MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                System.Web.HttpContext.Current.Session["FechaFin"] = mT_Contrato.Fecha_Fin;
                if (mT_Contrato != null)
                {
                    System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                    var listaAlerta = db.sp_Quimipac_ConsultaMT_ContratoAlerta(id).ToList();
                    ViewBag.listaAlerta = listaAlerta;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Contrato");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        [HttpGet]
        public ActionResult AgregarAlerta()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarAlerta([Bind(Include = "Id_Contrato, Id_Usuario, Fecha_Alerta, Mensaje, Repetir, Estado, Correo")] MT_ContratoAlerta mT_Alerta)
        {
            try
            {
                var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];

                if (id_Contrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }
                else
                {
                    int idContrato = int.Parse(id_Contrato.ToString());

                    if (ModelState.IsValid)
                    {
                        var Alerta = db.MT_ContratoAlerta.Where(x => x.Id_Usuario == mT_Alerta.Id_Usuario);
                        if (Alerta.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Alerta ya existe";
                            return RedirectToAction("Alerta_Contrato", new { id = idContrato });
                        }
                        else
                        {

                            mT_Alerta.Id_Contrato = idContrato;
                            mT_Alerta.Fecha_Alerta = DateTime.Now;
                            mT_Alerta.Fecha_Registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFin"]);
                            mT_Alerta.Estado = mT_Alerta.Estado.Substring(0, 1);

                            db.MT_ContratoAlerta.Add(mT_Alerta);

                            //int scope_identity_id = mT_Alerta.Id_Contrato_Alerta;
                            //db.Entry(mT_Alerta).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Alerta  de Contrato guardado";
                            //return View(mT_Alerta);
                            return RedirectToAction("Alerta_Contrato", new { id = idContrato });
                            
                        }

                    }

                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Alerta_Contrato", new { id = idContrato });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        [HttpGet]
        public ActionResult EliminarAlerta(int id)
        {
            try
            {


                var id_MTContrato = System.Web.HttpContext.Current.Session["id_Contrato"];

                if (id_MTContrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMTActividad = int.Parse(id_MTContrato.ToString());

                    MT_ContratoAlerta mT_Alerta = db.MT_ContratoAlerta.Find(id);

                    if (mT_Alerta != null)
                    {
                        mT_Alerta.Estado = "E";
                        db.Entry(mT_Alerta).State = EntityState.Modified;
                        db.MT_ContratoAlerta.Remove(mT_Alerta);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Archivo eliminado";
                        return RedirectToAction("Alerta_Contrato", new { id = id_MTContrato });
                    }
                    else
                    {
                        TempData["mensaje_correcto"] = "Código no existe";
                        return RedirectToAction("Alerta_Contrato", new { id = id_MTContrato });
                    }

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //PARAMETRO CONTRATO
        #region
        [HttpGet]
        public ActionResult Parametro_Contrato(int id)
        {
            try
            {
                MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                if (mT_Contrato != null)
                {
                    System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                    var listaParametroC = db.sp_Quimipac_ConsultaMT_ContratoParametro(id).ToList();
                    ViewBag.listaParametroC = listaParametroC;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Contrato");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult AgregarParametro_Contrato()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarParametro_Contrato([Bind(Include = "Id_Contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Contrato_Parametro mT_ParametroContrato)
        {
            try
            {
                var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];

                if (id_Contrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }
                else
                {
                    int idContrato = int.Parse(id_Contrato.ToString());

                    if (ModelState.IsValid)
                    {
                        var contratoParametro = db.MT_Contrato_Parametro.Where(x => x.Id_Contrato == mT_ParametroContrato.Id_Contrato && x.Parametro == mT_ParametroContrato.Parametro && x.Valor_Inicial == mT_ParametroContrato.Valor_Inicial && x.Valor_Final == mT_ParametroContrato.Valor_Final);
                        if (contratoParametro.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Contrato Parametro ya existe";
                            return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                        }
                        else
                        {

                            mT_ParametroContrato.Id_Contrato = idContrato;
                            mT_ParametroContrato.Estado = mT_ParametroContrato.Estado.Substring(0, 1);
                            db.MT_Contrato_Parametro.Add(mT_ParametroContrato);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Parametro de Contrato guardado";
                            return RedirectToAction("Parametro_Contrato", new { id = idContrato });

                        }

                    }

                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EditarParametro_Contrato(int id)
        {
            try
            {
                MT_Contrato_Parametro mT_ContratoParametro = db.MT_Contrato_Parametro.Find(id);
                bool seleccion = false;
                if (mT_ContratoParametro != null)
                {
                    return View(mT_ContratoParametro);

                }
                else
                {
                    
                    var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];

                    if (id_Contrato == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idContrato = int.Parse(id_Contrato.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditarParametro_Contrato([Bind(Include = "Id_Contrato_Parametro, Id_Contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Contrato_Parametro mT_ParametroContrato)
        {
            try
            {
                var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];
                // int bandera = 0;
                if (id_Contrato == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idContrato = int.Parse(id_Contrato.ToString());

                    if (ModelState.IsValid)
                    {


                        mT_ParametroContrato.Estado = mT_ParametroContrato.Estado.Substring(0, 1);


                        db.Entry(mT_ParametroContrato).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Parametro de Contrato actualizado";

                        return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                       
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion
    }
}