//using Quimipac_.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

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
    public class MantenimientoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

        //MATENIMIENTO DE TIPOS DE TRABAJO
        #region
        [HttpGet]
        public ActionResult MantTipoTrabajo()
        {
            try
            {
                var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo().ToList();
                ViewBag.listaTiposTrabajo = listaTiposTrabajo;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantTipoTrabajo()
        {
            try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                List<SelectListItem> itemsLineas = new List<SelectListItem>();
                List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                List<SelectListItem> itemsServicio = new List<SelectListItem>();

                var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio().ToList();
                foreach (var servicio in listaServicio)
                {
                    itemsServicio.Add(new SelectListItem { Value = Convert.ToString(servicio.Id_Servicio), Text = servicio.Descripcion });
                }

                SelectList selectlistaServicios = new SelectList(itemsServicio, "Value", "Text");

                var listaClientes = db.sp_LINK_ConsultaClientes().ToList();
                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                var listaLineas = db.sp_LINK_ConsultaLineas().ToList();
                foreach (var linea in listaLineas)
                {
                    itemsLineas.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                }

                SelectList selectlistaLineas = new SelectList(itemsLineas, "Value", "Text");


                var listaTipoTrabajo = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();
                foreach (var tipo in listaTipoTrabajo)
                {
                    itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

                ViewBag.listaClientes = selectlistaClientes;
                ViewBag.listaLineas = selectlistaLineas;
                ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo; 
                ViewBag.listaServicio = selectlistaServicios;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantTipoTrabajo([Bind(Include = "Id_Cliente,Codigo,Linea,Descripcion,Tipo,Estado,Control_Item,Control_Equipo,Control_Integrante,Control_Anexo,Control_Medida,Control_Costo, Id_Servicio")] MT_TipoTrabajo mT_TipoTrabajo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tipoTrabajo = db.MT_TipoTrabajo.Where(x => x.Codigo == mT_TipoTrabajo.Codigo);
                    if (tipoTrabajo.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("MantTipoTrabajo");
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
                            mT_TipoTrabajo.Id_Usuario = user_id.ToString();
                            mT_TipoTrabajo.Fecha_Registro = DateTime.Now;
                            mT_TipoTrabajo.Estado = mT_TipoTrabajo.Estado.Substring(0, 1);
                            mT_TipoTrabajo.Control_Item = mT_TipoTrabajo.Control_Item.Substring(0, 1);
                            mT_TipoTrabajo.Control_Equipo = mT_TipoTrabajo.Control_Equipo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Integrante = mT_TipoTrabajo.Control_Integrante.Substring(0, 1);
                            mT_TipoTrabajo.Control_Anexo = mT_TipoTrabajo.Control_Anexo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Medida = mT_TipoTrabajo.Control_Medida.Substring(0, 1);
                            mT_TipoTrabajo.Control_Costo = mT_TipoTrabajo.Control_Costo.Substring(0, 1);

                            db.MT_TipoTrabajo.Add(mT_TipoTrabajo);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Tipo de Trabajo guardado";
                            return RedirectToAction("MantTipoTrabajo");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantTipoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantTipoTrabajo(int id)
        {
            try
            {

                MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                bool seleccion = false;
                if (mT_TipoTrabajo != null)
                {


                    List<SelectListItem> items = new List<SelectListItem>();


                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsLineas = new List<SelectListItem>();
                    List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsServicio = new List<SelectListItem>();

                    var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio().ToList();
                    foreach (var servicio in listaServicio)
                    {
                        if (servicio.Id_Servicio == mT_TipoTrabajo.Id_Servicio)
                        {
                            seleccion = true;
                        }

                        itemsServicio.Add(new SelectListItem { Value = Convert.ToString(servicio.Id_Servicio), Text = servicio.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaServicio = new SelectList(itemsServicio, "Value", "Text");


                    var listaClientes = db.sp_LINK_ConsultaClientes().ToList();
                    foreach (var cliente in listaClientes)
                    {
                        if (cliente.cod_cli == mT_TipoTrabajo.Id_Cliente)
                        {
                            seleccion = true;
                        }

                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                    var listaLineas = db.sp_LINK_ConsultaLineas().ToList();

                    foreach (var linea in listaLineas)
                    {
                        if (linea.codigo == mT_TipoTrabajo.Linea)
                        {
                            seleccion = true;
                        }

                        itemsLineas.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaLineas = new SelectList(itemsLineas, "Value", "Text");


                    var listaTipoTrabajo = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();
                    foreach (var tipo in listaTipoTrabajo)
                    {
                        if (tipo.Id_TablaDetalle == mT_TipoTrabajo.Tipo)
                        {
                            seleccion = true;
                        }

                        itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

                    ViewBag.listaClientes = selectlistaClientes;
                    ViewBag.listaLineas = selectlistaLineas;
                    ViewBag.listaServicio = selectlistaServicio;
                    ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                    return View(mT_TipoTrabajo);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTipoTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantTipoTrabajo([Bind(Include = "Id_TipoTrabajo,Id_Cliente,Codigo,Linea,Descripcion,Tipo,Estado,Control_Item,Control_Equipo,Control_Integrante,Control_Anexo,Control_Medida,Control_Costo,Id_Servicio")] MT_TipoTrabajo mT_TipoTrabajo)
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

                        //var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo().ToList();

                        //foreach (var objt in listaTiposTrabajo)
                        //{
                        //    if (objt.Codigo == mT_TipoTrabajo.Codigo)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if (bandera != 1)
                        //{
                            mT_TipoTrabajo.Id_Usuario = user_id.ToString();
                            mT_TipoTrabajo.Fecha_Registro = DateTime.Now;
                            mT_TipoTrabajo.Estado = mT_TipoTrabajo.Estado.Substring(0, 1);
                            mT_TipoTrabajo.Control_Item = mT_TipoTrabajo.Control_Item.Substring(0, 1);
                            mT_TipoTrabajo.Control_Equipo = mT_TipoTrabajo.Control_Equipo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Integrante = mT_TipoTrabajo.Control_Integrante.Substring(0, 1);
                            mT_TipoTrabajo.Control_Anexo = mT_TipoTrabajo.Control_Anexo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Medida = mT_TipoTrabajo.Control_Medida.Substring(0, 1);
                            mT_TipoTrabajo.Control_Costo = mT_TipoTrabajo.Control_Costo.Substring(0, 1);

                            db.Entry(mT_TipoTrabajo).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Tipo de Trabajo actualizado";

                            return RedirectToAction("MantTipoTrabajo");
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Código ya existe";
                        //    return RedirectToAction("MantTipoTrabajo");
                        //}
                        
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantTipoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarTipoTrabajo(int id)
        {
            try
            {

                MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);

                mT_TipoTrabajo.Estado = "E";

                db.Entry(mT_TipoTrabajo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Tipo Trabajo Eliminado";
                return RedirectToAction("MantTipoTrabajo");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }


        //ITEMS DE TIPO DE TRABAJO
        [HttpGet]
        public ActionResult Items_MantTipoTrabajo(int id)
        {
            try
            {
                MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                if (mT_TipoTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                    var listaItemsTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(id).ToList();
                    ViewBag.listaItemsTipoTrabajo = listaItemsTipoTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTipoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_ItemMantTipoTrabajo()
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                List<SelectListItem> unidades = new List<SelectListItem>();


                var listaItems = db.sp_LINK_ConsultaItems().ToList();
                foreach (var item in listaItems)
                {
                    items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion });
                }

                SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();
                foreach (var unidad in listaUnidades)
                {
                    unidades.Add(new SelectListItem { Value = Convert.ToString(unidad.uni_med), Text = unidad.nombre });
                }

                SelectList selectlistaUnidades = new SelectList(unidades, "Value", "Text");


                ViewBag.listaItems = selectlistaItems;
                ViewBag.listaUnidades = selectlistaUnidades;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ItemMantTipoTrabajo([Bind(Include = "Id_Item,Unidad,Cantidad,Estado")] MT_Items mT_Items)
        {
            try
            {
                var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];

                if (id_MantTipoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_Items.Id_TipoTrabajo = idMantTipoTrabajo;
                        var itemTipoTrabajo = db.MT_Items.Where(x => x.Id_TipoTrabajo == mT_Items.Id_TipoTrabajo && x.Id_Item == mT_Items.Id_Item && x.Unidad == mT_Items.Unidad && x.Cantidad == mT_Items.Cantidad);
                        if (itemTipoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Item ya existe";
                            return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        }
                        else
                        {

                            mT_Items.Id_TipoTrabajo = idMantTipoTrabajo;
                            mT_Items.Estado = mT_Items.Estado.Substring(0, 1);


                            db.MT_Items.Add(mT_Items);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Item guardado";
                            return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });

                        }
                        
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                    }
                }
                
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_ItemMantTipoTrabajo(int id)
        {
            try
            {
                MT_Items mT_Items = db.MT_Items.Find(id);
                bool seleccion = false;
                if (mT_Items != null)
                {

                    List<SelectListItem> items = new List<SelectListItem>();
                    List<SelectListItem> unidades = new List<SelectListItem>();


                    var listaItems = db.sp_LINK_ConsultaItems().ToList();
                    foreach (var item in listaItems)
                    {
                        if (mT_Items.Id_Item == item.cod_item)
                        {
                            seleccion = true;
                        }

                        items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                    var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();
                    foreach (var unidad in listaUnidades)
                    {
                        if (mT_Items.Unidad == unidad.uni_med)
                        {
                            seleccion = true;
                        }

                        unidades.Add(new SelectListItem { Value = Convert.ToString(unidad.uni_med), Text = unidad.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaUnidades = new SelectList(unidades, "Value", "Text");


                    ViewBag.listaItems = selectlistaItems;
                    ViewBag.listaUnidades = selectlistaUnidades;


                    return View(mT_Items);

                }
                else
                {
                    var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];

                    if (id_MantTipoTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
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
        public ActionResult Editar_ItemMantTipoTrabajo([Bind(Include = "Id_ItemTipoTrabajo,Id_TipoTrabajo,Id_Item,Unidad,Cantidad,Estado")] MT_Items mT_Items)
        {
            try
            {
                var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];
               // int bandera = 0;
                if (id_MantTipoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        //var listaItemsTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(idMantTipoTrabajo).ToList();

                        //foreach (var objt in listaItemsTipoTrabajo)
                        //{
                        //    if(objt.Id_MTTipoTrabajo == mT_Items.Id_MTTipoTrabajo && objt.Id_Item == mT_Items.Id_Item)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if(bandera!=1)
                        //{
                            mT_Items.Estado = mT_Items.Estado.Substring(0, 1);


                            db.Entry(mT_Items).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Item de tipo trabajo actualizado";

                            return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Registro ya existe";
                        //    return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}
                        
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                    }
                }  
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        
        //ACTIVIDADES DE TIPO DE TRABAJO
        [HttpGet]
        public ActionResult Actividades_MantTipoTrabajo(int id)
        {
            try
            {
                MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);

                if (mT_TipoTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                    var listaActividades_MantTipoTrabajo = db.sp_Quimipac_ConsultaMT_Actividad(mT_TipoTrabajo.Id_TipoTrabajo).ToList();
                    ViewBag.listaActividades_MantTipoTrabajo = listaActividades_MantTipoTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTipoTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Actividades_MantTipoTrabajo()
        {
            try
            {
                List<SelectListItem> itemsActividad = new List<SelectListItem>();

                var listaActividades = db.MT_Actividad.Where(x => x.Estado == "A").ToList();
                foreach (var actividad in listaActividades)
                {
                    itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Codigo });
                }

                itemsActividad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaActividades = new SelectList(itemsActividad, "Value", "Text");

                ViewBag.listaActividades = selectlistaActividades;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Actividades_MantTipoTrabajo([Bind(Include = "Id_ActividadPadre,Codigo,Descripcion,Tiempo_Estimado,Obligatorio,Peso_Actividad,Orden,Estado")] MT_Actividad mT_Actividad)
        {
            try
            {
                var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];

                if (id_MantTipoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {



                        var tipoTrabajo = db.MT_Actividad.Where(x => x.Codigo == mT_Actividad.Codigo);
                        if (tipoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("Actividades_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        }
                        else
                        {

                            mT_Actividad.Id_TipoTrabajo = idMantTipoTrabajo;

                            mT_Actividad.Obligatorio = mT_Actividad.Obligatorio.Substring(0, 1);

                            if (mT_Actividad.Obligatorio == "N")
                            {
                                mT_Actividad.Peso_Actividad = 0;
                            }

                            mT_Actividad.Estado = mT_Actividad.Estado.Substring(0, 1);


                            db.MT_Actividad.Add(mT_Actividad);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Actividad guardada";
                            return RedirectToAction("Actividades_MantTipoTrabajo", new { id = idMantTipoTrabajo });


                        }


                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Actividades_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                    }

                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Actividades_MantTipoTrabajo(int id)
        {
            try
            {
                MT_Actividad mT_Actividad = db.MT_Actividad.Find(id);
                bool seleccion = false;
                if (mT_Actividad != null)
                {
                    List<SelectListItem> itemsActividad = new List<SelectListItem>();

                    var listaActividades = db.MT_Actividad.Where(x => x.Estado == "A").ToList();

                    itemsActividad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    foreach (var actividad in listaActividades)
                    {

                        if (actividad.Id_ActividadPadre == mT_Actividad.Id_ActividadPadre)
                        {
                            seleccion = true;
                        }


                        itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Codigo, Selected = seleccion });
                    }

                    SelectList selectlistaActividades = new SelectList(itemsActividad, "Value", "Text");


                    if (mT_Actividad.Obligatorio == "N")
                    {
                        mT_Actividad.Peso_Actividad = 0;

                    }

                    ViewBag.listaActividades = selectlistaActividades;

                    return View(mT_Actividad);
                }
                else
                {
                    var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];

                    if (id_MantTipoTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Actividades_MantTipoTrabajo", new { id = idMantTipoTrabajo });
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
        public ActionResult Editar_Actividades_MantTipoTrabajo([Bind(Include = "Id_Actividad,Id_TipoTrabajo,Id_ActividadPadre,Codigo,Descripcion,Tiempo_Estimado,Obligatorio,Peso_Actividad,Orden,Estado")] MT_Actividad mT_Actividad)
        {
            try
            {
                //int bandera = 0;
                if (ModelState.IsValid)
                {
                    //var listaActividades_MantTipoTrabajo = db.sp_Quimipac_ConsultaMT_Actividad(mT_Actividad.Id_MTTipoTrabajo).ToList();

                    //foreach (var objt in listaActividades_MantTipoTrabajo)
                    //{
                    //    if (objt.Codigo == mT_Actividad.Codigo)
                    //    {
                    //        bandera = 1;
                    //    }
                    //}

                    //if (bandera != 1)
                    //{
                        mT_Actividad.Obligatorio = mT_Actividad.Obligatorio.Substring(0, 1);

                        if (mT_Actividad.Obligatorio == "N")
                        {
                            mT_Actividad.Peso_Actividad = 0;
                        }

                        mT_Actividad.Estado = mT_Actividad.Estado.Substring(0, 1);

                        db.Entry(mT_Actividad).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Actividad actualizada";

                        return RedirectToAction("Actividades_MantTipoTrabajo", new { id = mT_Actividad.Id_TipoTrabajo });
                    //}
                    //else
                    //{
                    //    TempData["mensaje_error"] = "Registro ya existe";
                    //    return RedirectToAction("Actividades_MantTipoTrabajo", new { id = mT_Actividad.Id_MTTipoTrabajo });
                    //}

                       
                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Actividades_MantTipoTrabajo", new { id = mT_Actividad.Id_TipoTrabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        //FORMULARIOS DE ACTIVIDAD DE TIPO DE TRABAJO
        [HttpGet]
        public ActionResult Formularios_Actividad(int id)
        {
            try
            {
                MT_Actividad mT_Actividad = db.MT_Actividad.Find(id);


                if (mT_Actividad != null)
                {
                    System.Web.HttpContext.Current.Session["id_MTActividad"] = mT_Actividad.Id_Actividad;
                    var listaFormularios = db.MT_Formulario.Where(x => x.Id_Actividad == mT_Actividad.Id_Actividad && x.Estado == "A").ToList();
                    ViewBag.idMTTipoTrabajo = mT_Actividad.Id_TipoTrabajo;
                    ViewBag.listaFormularios = listaFormularios;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Actividades_MantTipoTrabajo", new { id = mT_Actividad.Id_TipoTrabajo });
                }


            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public FileResult DescargarArchivoFormulario(string nombre)
        {
            try
            {
                var ruta = Server.MapPath("~/Formularios_Actividad/" + nombre);
                var contentType = MimeMapping.GetMimeMapping(nombre);
                FilePathResult archivo = File(ruta, contentType, nombre);
                return archivo;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult AgregarFormulario_Actividad()
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
        public ActionResult AgregarFormulario_Actividad([Bind(Include = "Id_Actividad,Codigo,Descripcion,Enlace_Formulario,NombreArchivo,Fecha_Creacion,Estado")] MT_Formulario mT_Formulario, HttpPostedFileBase NombreArchivo)
        {
            try
            {
                var id_MTActividad = System.Web.HttpContext.Current.Session["id_MTActividad"];

                if (id_MTActividad == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMTActividad = int.Parse(id_MTActividad.ToString());

                    if (ModelState.IsValid)
                    {


                        var formulario = db.MT_Formulario.Where(x => x.Codigo == mT_Formulario.Codigo);
                        if (formulario.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
                        }
                        else
                        {
                            if (NombreArchivo != null)
                            {
                                var ruta_archivo = @"~/Formularios_Actividad/";
                                var ruta_servidor = Path.GetFullPath(ruta_archivo);
                                var extension = Path.GetExtension(NombreArchivo.FileName);

                                int fileSize = NombreArchivo.ContentLength;

                                mT_Formulario.Id_Actividad = idMTActividad;
                                mT_Formulario.NombreArchivo = "";
                                mT_Formulario.Fecha_Creacion = DateTime.Now;
                                mT_Formulario.Estado = mT_Formulario.Estado.Substring(0, 1);
                                mT_Formulario.Enlace_Formulario = ruta_servidor;
                                db.MT_Formulario.Add(mT_Formulario);
                                db.SaveChanges();

                                int scope_identity_id = mT_Formulario.Id_Formulario;

                                string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                                mT_Formulario.NombreArchivo = nombreArchivo;

                                NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                                db.Entry(mT_Formulario).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Formulario de Actividad guardado";
                                return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
                            }
                            else
                            {
                                return View(mT_Formulario);
                            }
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
                    }

                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarArchivoFormulario(int id)
        {
            try
            {


                var id_MTActividad = System.Web.HttpContext.Current.Session["id_MTActividad"];

                if (id_MTActividad == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMTActividad = int.Parse(id_MTActividad.ToString());

                    MT_Formulario mT_Formulario = db.MT_Formulario.Find(id);

                    if (mT_Formulario != null)
                    {
                        mT_Formulario.Estado = "E";
                        db.Entry(mT_Formulario).State = EntityState.Modified;
                        //db.MT_Formulario.Remove(mT_Formulario);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Archivo eliminado";
                        return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
                    }
                    else
                    {
                        TempData["mensaje_correcto"] = "Código no existe";
                        return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
                    }

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        //MEDIDAS TIPO TRABAJO
        [HttpGet]
        public ActionResult Medidas_MantTipoTrabajo(int id)
        {
            try
            {
                MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                if (mT_TipoTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                    var listaMedidasTipoTrabajo = db.sp_Quimipac_ConsultaMt_MedidasTTrabajo(id).ToList();
                    ViewBag.listaMedidasTipoTrabajo = listaMedidasTipoTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTipoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Medidas_MantTipoTrabajo()
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
        public ActionResult Agregar_Medidas_MantTipoTrabajo([Bind(Include = "Codigo,Descripcion,Tipo_Dato,Olbligatorio, Estado")] MT_Tipo_Trabajo_Medida mT_TTMedida)
        {
            try
            {
                var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];

                if (id_MantTipoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_TTMedida.Id_Tipo_Trabajo = idMantTipoTrabajo;
                        var medidaTipoTrabajo = db.MT_Tipo_Trabajo_Medida.Where(x => x.Id_Tipo_Trabajo == mT_TTMedida.Id_Tipo_Trabajo && x.Codigo == mT_TTMedida.Codigo);
                        if (medidaTipoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Medida ya existe";
                            return RedirectToAction("Medidas_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        }
                        else
                        {

                            mT_TTMedida.Id_Tipo_Trabajo = idMantTipoTrabajo;
                            mT_TTMedida.Estado = mT_TTMedida.Estado.Substring(0, 1);


                            db.MT_Tipo_Trabajo_Medida.Add(mT_TTMedida);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Medida guardada";
                            return RedirectToAction("Medidas_MantTipoTrabajo", new { id = idMantTipoTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Medidas_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Medidas_MantTipoTrabajo(int id)
        {
            try
            {

                MT_Tipo_Trabajo_Medida mT_TipoTrabajoMedida = db.MT_Tipo_Trabajo_Medida.Find(id);
                bool seleccion = false;
                if (mT_TipoTrabajoMedida != null)
                {


                    

                    return View(mT_TipoTrabajoMedida);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Medidas_MantTipoTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Medidas_MantTipoTrabajo([Bind(Include = "Id_Tipo_trabajo_Medida,Id_Tipo_Trabajo,Codigo,Descripcion,Tipo_Dato,Olbligatorio, Estado")] MT_Tipo_Trabajo_Medida mT_TipoTrabajoMedida)
        {
            try
            {
                var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];
                // int bandera = 0;
                if (id_MantTipoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {


                        mT_TipoTrabajoMedida.Olbligatorio = mT_TipoTrabajoMedida.Olbligatorio.Substring(0, 1);
                        mT_TipoTrabajoMedida.Estado = mT_TipoTrabajoMedida.Estado.Substring(0, 1);


                        db.Entry(mT_TipoTrabajoMedida).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Medida de tipo trabajo actualizado";

                        return RedirectToAction("Medidas_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Registro ya existe";
                        //    return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Medidas_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        
        #endregion


        //MATENIMIENTO DE PRECIOS REFERENCIALES
        #region
        [HttpGet]
        public ActionResult MantPrecioReferencial()
        {
            try
            {
                var listaPreciosreferProyectos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(1).ToList();
                var listaPreciosreferContratos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(2).ToList();

                foreach (var contrato in listaPreciosreferContratos)
                {
                    listaPreciosreferProyectos.Add(contrato);
                }

                ViewBag.listaPreciosrefer = listaPreciosreferProyectos;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantPrecioReferencial()
        {
            try
            {
                List<SelectListItem> itemsTipos = new List<SelectListItem>();
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                List<SelectListItem> itemsItems = new List<SelectListItem>();
                List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();


                var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

                foreach (var tipo in listaTipos)
                {
                    itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

               // itemsTipos.Insert(0, new SelectListItem() { Value = "", Text = "Seleccione Curso" });

                SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

                var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato().ToList();
                foreach (var contrato in listaContratos)
                {
                    itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente });
                }

                SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto().ToList();
                foreach (var proyecto in listaProyectos)
                {
                    itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
                }

                SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");


                var listaItems = db.sp_LINK_ConsultaItems().ToList();

                foreach (var item in listaItems)
                {
                    itemsItems.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion });
                }

                SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");


                List<SelectListItem> items = new List<SelectListItem>();


                var listaMoneda = db.sp_LINK_ConsultaMonedas().ToList();

                foreach (var moneda in listaMoneda)
                {
                    itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre });
                }

                SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1).ToList(); //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO

                foreach (var tipoTrabajo in listaTiposTrabajo)
                {
                    itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo });
                }

                SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

                ViewBag.listaContratos = selectlistaContratos;
                ViewBag.listaProyectos = selectlistaProyectos;
                ViewBag.listaItems = selectlistaItems;
                ViewBag.listaMoneda = selectlistaMoneda;
                ViewBag.listaTipos = selectlistaTipos;
                ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        public JsonResult GetTiposTrabajo(int id)
        {
            try
            {
                List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id).ToList();

                foreach (var tipoTrabajo in listaTiposTrabajo)
                {
                    itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo });
                }

                SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

               // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaTipoTrabajo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantPrecioReferencial([Bind(Include = "Id_TipoTablaDetalle, Id_Proyecto_Contrato,Id_TipoTrabajo, Id_Item,Fecha_Inicio,Fecha_Fin, Moneda, Precio, Costo, Estado")] MT_PrecioReferencial mT_PrecioReferencial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var precioReferencial = db.MT_PrecioReferencial.Where(x => x.Id_TipoTablaDetalle == mT_PrecioReferencial.Id_TipoTablaDetalle && x.Id_Proyecto_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato && x.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo && x.Id_Item == mT_PrecioReferencial.Id_Item && x.Fecha_Inicio == mT_PrecioReferencial.Fecha_Inicio && x.Fecha_Fin == mT_PrecioReferencial.Fecha_Fin && x.Moneda == mT_PrecioReferencial.Moneda && x.Precio == mT_PrecioReferencial.Precio && x.Costo == mT_PrecioReferencial.Costo);

                    if (precioReferencial.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Precio Referencial ya existe";
                        return RedirectToAction("MantPrecioReferencial");
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
                            mT_PrecioReferencial.Id_Usuario = user_id.ToString();
                            mT_PrecioReferencial.Fecha_Registro = DateTime.Now;
                            mT_PrecioReferencial.Estado = mT_PrecioReferencial.Estado.Substring(0, 1);
                            db.MT_PrecioReferencial.Add(mT_PrecioReferencial);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Precio Referencial guardado";
                            return RedirectToAction("MantPrecioReferencial");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantPrecioReferencial");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
   
        [HttpGet]
        public ActionResult Editar_MantPrecioReferencial(int id)
        {
            try
            {

                MT_PrecioReferencial mT_PrecioReferencial = db.MT_PrecioReferencial.Find(id);
                bool seleccion = false;
                if (mT_PrecioReferencial != null)
                {

                    List<SelectListItem> itemsTipos = new List<SelectListItem>();
                    List<SelectListItem> itemsContratos = new List<SelectListItem>();
                    List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                    List<SelectListItem> itemsItems = new List<SelectListItem>();
                    List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                    List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();


                    var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

                    foreach (var tipo in listaTipos)
                    {
                        if (tipo.Id_TablaDetalle == mT_PrecioReferencial.Id_TipoTablaDetalle)
                             {
                                    seleccion = true;
                             }

                            itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                   

                    SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

                    var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato().ToList();

                    foreach (var contrato in listaContratos)
                    {
                        if (contrato.Id_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato)
                        {
                            seleccion = true;
                        }

                        itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente, Selected = seleccion });
                    }

                    SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto().ToList();
                    foreach (var proyecto in listaProyectos)
                    {
                        if (proyecto.Id_Proyecto == mT_PrecioReferencial.Id_Proyecto_Contrato)
                        {
                            seleccion = true;
                        }
                        itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente, Selected = seleccion });
                    }

                    SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");


                    var listaItems = db.sp_LINK_ConsultaItems().ToList();

                    foreach (var item in listaItems)
                    {
                        if (item.cod_item == mT_PrecioReferencial.Id_Item)
                        {
                            seleccion = true;
                        }
                        itemsItems.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");


                    List<SelectListItem> items = new List<SelectListItem>();


                    var listaMoneda = db.sp_LINK_ConsultaMonedas().ToList();

                    foreach (var moneda in listaMoneda)
                    {
                        if (moneda.codmon == mT_PrecioReferencial.Moneda)
                        {
                            seleccion = true;
                        }

                        itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                    var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1).ToList(); //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO

                    foreach (var tipoTrabajo in listaTiposTrabajo)
                    {
                        if (tipoTrabajo.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo)
                        {
                            seleccion = true;
                        }

                        itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo, Selected = seleccion });
                    }

                    SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

                    ViewBag.listaContratos = selectlistaContratos;
                    ViewBag.listaProyectos = selectlistaProyectos;
                    ViewBag.listaItems = selectlistaItems;
                    ViewBag.listaMoneda = selectlistaMoneda;
                    ViewBag.listaTipos = selectlistaTipos;
                    ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;


                    return View(mT_PrecioReferencial);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantPrecioReferencial");
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }

         

        }

        [ValidateAntiForgeryToken] 
        [HttpPost]
        public ActionResult Editar_MantPrecioReferencial([Bind(Include = "Id_PrecioReferencial,Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,Id_TipoTrabajo,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado")] MT_PrecioReferencial mT_PrecioReferencial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   // int bandera = 0;
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        //var listaPreciosreferProyectos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(1).ToList();
                        //var listaPreciosreferContratos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(2).ToList();

                        //foreach (var contrato in listaPreciosreferContratos)
                        //{
                        //    listaPreciosreferProyectos.Add(contrato);
                        //}

                        //foreach (var objt in listaPreciosreferProyectos)
                        //{
                        //    if(objt.Id_Proyecto_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato && objt.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo && objt.Id_Item == mT_PrecioReferencial.Id_Item)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if(bandera != 1)
                        //{

                            mT_PrecioReferencial.Id_Usuario = user_id.ToString();
                            mT_PrecioReferencial.Fecha_Registro = DateTime.Now;

                            mT_PrecioReferencial.Estado = mT_PrecioReferencial.Estado.Substring(0, 1);

                            db.Entry(mT_PrecioReferencial).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Precio Referencial actualizado";

                            return RedirectToAction("MantPrecioReferencial");
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Registro ya existe";
                        //    return RedirectToAction("MantPrecioReferencial");
                        //}
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantPrecioReferencial");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarPrecioReferencial(int id)
        {
            try
            {

                MT_PrecioReferencial mT_PrecioReferencial = db.MT_PrecioReferencial.Find(id);

                mT_PrecioReferencial.Estado = "E";

                db.Entry(mT_PrecioReferencial).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Precio Referencial Eliminado";
                return RedirectToAction("MantPrecioReferencial");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }
        #endregion


        //MATENIMIENTO DE LUGARES DE MEDICIÓN
        #region
        public ActionResult MantLugarMedicion()
        {
            try
            {
                var listaLugarMedicion = db.sp_Quimipac_ConsultaMT_Estacion().ToList();
                ViewBag.listaLugarMedicion = listaLugarMedicion;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantLugarMedicion()
        {
            try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                List<SelectListItem> itemsProvincias = new List<SelectListItem>();
                List<SelectListItem> itemsCiudades = new List<SelectListItem>();
                List<SelectListItem> itemsMacrosector = new List<SelectListItem>();

                var listaClientes = db.sp_LINK_ConsultaClientes().ToList();
                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                var listaProvincias = db.sp_Quimipac_ConsultaEstacionProvincia().ToList();
                //var listaprovinciaciud = new sp_Quimipac_ConsultaEstacionProvincia_Result();
                foreach (var provincias in listaProvincias)
                {
                    itemsProvincias.Add(new SelectListItem { Value = Convert.ToString(provincias.Id_TablaDetalle), Text = provincias.Descripcion });
                }
                //itemsProvincias.Insert(0, new SelectListItem() { Value = "", Text = "Seleccione Provincia" });

                SelectList selectlistaProvincias = new SelectList(itemsProvincias, "Value", "Text");


                var listaCiudades = db.sp_Quimipac_ConsultaEstacionProvinciaCiudad(listaProvincias[0].Id_TablaDetalle).ToList();
                foreach (var ciudades in listaCiudades)
                {
                    itemsCiudades.Add(new SelectListItem
                    {
                        Value = Convert.ToString(ciudades.Id_TablaDetalle),
                        Text = ciudades.descripcionciu
                    });
                }

                //itemsCiudades.Insert(0, new SelectListItem() { Value = "", Text = "Elija una Ciudad" });

                SelectList selectlistaCiudades = new SelectList(itemsCiudades, "Value", "Text");


                var listaMacrosector = db.sp_Quimipac_ConsultaEstacionMacrosector().ToList();
                foreach (var macrosector in listaMacrosector)
                {
                    itemsMacrosector.Add(new SelectListItem
                    {
                        Value = Convert.ToString(macrosector.Id_TablaDetalle),
                        Text = macrosector.Descripcion
                    });
                }

                //itemsCiudades.Insert(0, new SelectListItem() { Value = "", Text = "Elija una Ciudad" });

                SelectList selectlistaMacrosectores = new SelectList(itemsMacrosector, "Value", "Text");



                ViewBag.listaClientes = selectlistaClientes;
                ViewBag.listaProvincias = selectlistaProvincias;
                ViewBag.listaCiudades = selectlistaCiudades;
                ViewBag.listaMacrosectores = selectlistaMacrosectores;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantLugarMedicion([Bind(Include = "Id_Cliente,Id_Provincia, Id_Ciudad,Id_Macrosector,Nombre, Direccion, Estado, Coordenada_X, Coordenada_Y")] MT_Estacion mT_Estacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lugarMedicion = db.MT_Estacion.Where(x => x.Coordenada_X == mT_Estacion.Coordenada_X && x.Coordenada_Y == mT_Estacion.Coordenada_Y && x.Direccion == mT_Estacion.Direccion);

                    if (lugarMedicion.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("MantLugarMedicion");
                    }
                    else
                    {

                             mT_Estacion.Estado = mT_Estacion.Estado.Substring(0, 1);
                            db.MT_Estacion.Add(mT_Estacion);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Estación guardada";
                            return RedirectToAction("MantLugarMedicion");
                      
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantLugarMedicion");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
		
		[HttpGet]
        public ActionResult Editar_MantLugarMedicion(int id)
        {
            try
            {

                MT_Estacion mT_LugarMedicion = db.MT_Estacion.Find(id);
                bool seleccion = false;
                if (mT_LugarMedicion != null)
                {


                    //List<SelectListItem> items = new List<SelectListItem>();


                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsProvincias = new List<SelectListItem>();
                    List<SelectListItem> itemsCiudades = new List<SelectListItem>();
                    List<SelectListItem> itemsMacroSector = new List<SelectListItem>();



                    var listaClientes = db.sp_LINK_ConsultaClientes().ToList();
                    foreach (var cliente in listaClientes)
                    {
                        if (cliente.cod_cli == mT_LugarMedicion.Id_Cliente)
                        {
                            seleccion = true;
                        }

                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                    var listaProvincias = db.sp_Quimipac_ConsultaEstacionProvincia().ToList();
                    //var listaprovinciaciud = new sp_Quimipac_ConsultaEstacionProvincia_Result();
                    foreach (var provincias in listaProvincias)
                    {

                        if (provincias.Id_TablaDetalle == mT_LugarMedicion.Id_Provincia)
                        {
                            seleccion = true;
                        }

                        itemsProvincias.Add(new SelectListItem { Value = Convert.ToString(provincias.Id_TablaDetalle), Text = provincias.Descripcion, Selected = seleccion });
                    }
                    //itemsProvincias.Insert(0, new SelectListItem() { Value = "", Text = "Seleccione Provincia" });

                    SelectList selectlistaProvincias = new SelectList(itemsProvincias, "Value", "Text");



                    var listaCiudades = db.sp_Quimipac_ConsultaEstacionProvinciaCiudad(listaProvincias[0].Id_TablaDetalle).ToList();
                    foreach (var ciudades in listaCiudades)
                    {
                        if (ciudades.Id_TablaDetalle == mT_LugarMedicion.Id_Ciudad)
                        {
                            seleccion = true;
                        }

                        itemsCiudades.Add(new SelectListItem { Value = Convert.ToString(ciudades.Id_TablaDetalle), Text = ciudades.descripcionciu, Selected = seleccion });
                    }

                    //itemsCiudades.Insert(0, new SelectListItem() { Value = "", Text = "Elija una Ciudad" });

                    SelectList selectlistaCiudades = new SelectList(itemsCiudades, "Value", "Text");

                    var listaMacrosector = db.sp_Quimipac_ConsultaEstacionMacrosector().ToList();
                    foreach (var macrosector in listaMacrosector)
                    {
                        if (macrosector.Id_TablaDetalle == mT_LugarMedicion.Id_Macrosector)
                        {
                            seleccion = true;
                        }

                        itemsMacroSector.Add(new SelectListItem { Value = Convert.ToString(macrosector.Id_TablaDetalle), Text = macrosector.Descripcion, Selected = seleccion });
                    }

                    //itemsCiudades.Insert(0, new SelectListItem() { Value = "", Text = "Elija una Ciudad" });

                    SelectList selectlistaMacrosectores = new SelectList(itemsMacroSector, "Value", "Text");

                    ViewBag.listaClientes = selectlistaClientes;
                    ViewBag.listaProvincias = selectlistaProvincias;
                    ViewBag.listaCiudades = selectlistaCiudades;
                    ViewBag.listaMacrosector = selectlistaMacrosectores;

                    return View(mT_LugarMedicion);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantLugarMedicion");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantLugarMedicion([Bind(Include = "Id_Estacion, Id_Cliente,Id_Provincia, Id_Ciudad, Id_Macrosector,Nombre,Direccion,Coordenada_X, Coordenada_Y, Estado")] MT_Estacion mT_LugarMedicion)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {

                        //mT_LugarMedicion.Id_Usuario = user_id.ToString();
                        //mT_LugarMedicion.Fecha_Registro = DateTime.Now;
                        //mT_LugarMedicion.Id_Cliente = mT_LugarMedicion.Id_Cliente.ToString();
                        //mT_LugarMedicion.Id_Provincia = Convert.ToInt32(mT_LugarMedicion.Id_Provincia.ToString());
                        //mT_LugarMedicion.Id_Ciudad = Convert.ToInt32(mT_LugarMedicion.Id_Ciudad.ToString());
                        //mT_LugarMedicion.Id_Macrosector = Convert.ToInt32(mT_LugarMedicion.Id_Macrosector.ToString());
                        //mT_LugarMedicion.Nombre = mT_LugarMedicion.Nombre.ToString();
                        //mT_LugarMedicion.Direccion = mT_LugarMedicion.Direccion.ToString();
                        //mT_LugarMedicion.Coordenada_X = mT_LugarMedicion.Coordenada_X.ToString();
                        //mT_LugarMedicion.Coordenada_Y = mT_LugarMedicion.Coordenada_Y.ToString();
                        mT_LugarMedicion.Estado = mT_LugarMedicion.Estado.Substring(0, 1);




                        db.Entry(mT_LugarMedicion).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Lugar Medicion actualizado";

                        return RedirectToAction("MantLugarMedicion");
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantLugarMedicion");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarLugaresdeMedicion(int id)
        {
            try
            {

                MT_Estacion mT_LugarMedicion = db.MT_Estacion.Find(id);

                mT_LugarMedicion.Estado = "E";

                db.Entry(mT_LugarMedicion).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Lugar Medicion Eliminado";
                return RedirectToAction("MantLugarMedicion");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        public JsonResult GetCiudades(int id)
        {
            try
            {
               List<SelectListItem> itemsCiudades = new List<SelectListItem>();

                var listaCiudades = db.sp_Quimipac_ConsultaEstacionProvinciaCiudad(id).ToList();

                //string seleccion = false;
                //bool seleccion = false;
                // int contador = 0;
                foreach (var ciudades in listaCiudades)
                {
                    itemsCiudades.Add(new SelectListItem
                    {
                        Value = Convert.ToString(ciudades.Id_TablaDetalle),
                        Text = ciudades.descripcionciu,
                       
                    });
                   
                }

                //itemsCiudades.Insert(0, new SelectListItem() { Value = "", Text = "Elija ciudad" });


                SelectList selectListCiudades = new SelectList(itemsCiudades, "Value", "Text");

                //ViewBag.Ciudades = selectListCiudades;

                return Json(selectListCiudades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }
        #endregion


        //MATENIMIENTO GRUPO TRABAJO
        #region
        public ActionResult MantGrupoTrabajo()
        {
            try
            {
                var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoTrabajo().ToList();
                ViewBag.listaGrupoTrabajo = listaGrupoTrabajo;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantGrupoTrabajo()
        {
            try
            {
                List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                List<SelectListItem> itemsTipoGrupo = new List<SelectListItem>();
                List<SelectListItem> itemsBodega = new List<SelectListItem>();

                var listaCampamentos = db.sp_Quimipac_ConsultaMT_Campamento().ToList();
                foreach (var campamento in listaCampamentos)
                {
                    itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre });
                }

                SelectList selectlistaCampamento = new SelectList(itemsCampamento, "Value", "Text");


                var listaBodegas = db.sp_LINK_ConsultaBodegas().ToList();
                foreach (var bodega in listaBodegas)
                {
                    itemsBodega.Add(new SelectListItem { Value = Convert.ToString(bodega.cod_bod), Text = bodega.nombre });
                }

                SelectList selectlistaBodegas = new SelectList(itemsBodega, "Value", "Text");


                var listaTipoGrupoTrabajo = db.sp_Quimipac_ConsultaMT_TablaDetalle(7).ToList();
                foreach (var tipo in listaTipoGrupoTrabajo)
                {
                    itemsTipoGrupo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                SelectList selectlistaTipoGrupoTrabajo = new SelectList(itemsTipoGrupo, "Value", "Text");

                ViewBag.listaCampamentos = selectlistaCampamento;
                ViewBag.listaBodegas = selectlistaBodegas;
                ViewBag.listaTipoGrupoTrabajo = selectlistaTipoGrupoTrabajo;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantGrupoTrabajo([Bind(Include = "Id_Campamento,Nombre,Tipo,Estado,Bodega")] MT_GrupoTrabajo mT_GrupoTrabajo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var grupoTrabajo = db.MT_GrupoTrabajo.Where(x => x.Id_Campamento == mT_GrupoTrabajo.Id_Campamento && x.Nombre == mT_GrupoTrabajo.Nombre && x.Tipo == mT_GrupoTrabajo.Tipo && x.Bodega == mT_GrupoTrabajo.Bodega);
                    if (grupoTrabajo.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Grupo Trabajo ya existe";
                        return RedirectToAction("MantGrupoTrabajo");
                    }
                    else
                    {
                        mT_GrupoTrabajo.Estado = mT_GrupoTrabajo.Estado.Substring(0, 1);


                        db.MT_GrupoTrabajo.Add(mT_GrupoTrabajo);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Grupo de Trabajo guardado";
                        return RedirectToAction("MantGrupoTrabajo");

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantGrupoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantGrupoTrabajo(int id)
        {
            try
            {

                MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);
                bool seleccion = false;
                if (mT_GrupoTrabajo != null)
                {


                    List<SelectListItem> items = new List<SelectListItem>();


                    List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                    List<SelectListItem> itemsTipoGrupo = new List<SelectListItem>();
                    List<SelectListItem> itemsBodega = new List<SelectListItem>();

                    var listaCampamento = db.sp_Quimipac_ConsultaMT_Campamento().ToList();
                    foreach (var campamento in listaCampamento)
                    {
                        if (campamento.Id_Campamento == mT_GrupoTrabajo.Id_Campamento)
                        {
                            seleccion = true;
                        }

                        itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistaCampamentos = new SelectList(itemsCampamento, "Value", "Text");


                    var listaBodegas = db.sp_LINK_ConsultaBodegas().ToList();

                    foreach (var bodega in listaBodegas)
                    {
                        if (bodega.cod_bod == mT_GrupoTrabajo.Bodega)
                        {
                            seleccion = true;
                        }

                        itemsBodega.Add(new SelectListItem { Value = Convert.ToString(bodega.cod_bod), Text = bodega.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaBodegas = new SelectList(itemsBodega, "Value", "Text");


                    var listaTipoGrupoTrabajo = db.sp_Quimipac_ConsultaMT_TablaDetalle(7).ToList();
                    foreach (var tipo in listaTipoGrupoTrabajo)
                    {
                        if (tipo.Id_TablaDetalle == mT_GrupoTrabajo.Tipo)
                        {
                            seleccion = true;
                        }

                        itemsTipoGrupo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTipoGrupoTrabajo = new SelectList(itemsTipoGrupo, "Value", "Text");

                    ViewBag.listaCampamento = selectlistaCampamentos;
                    ViewBag.listaBodegas = selectlistaBodegas;
                    ViewBag.listaTipoGrupoTrabajo = selectlistaTipoGrupoTrabajo;

                    return View(mT_GrupoTrabajo);

                }
                else
                {
                    TempData["mensaje_error"] = "Grupo Trabajo no existe";
                    return RedirectToAction("MantGrupoTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajo, Id_Campamento,Nombre,Tipo,Estado,Bodega")] MT_GrupoTrabajo mT_GrupoTrabajo)
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

                        //var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo().ToList();

                        //foreach (var objt in listaTiposTrabajo)
                        //{
                        //    if (objt.Codigo == mT_TipoTrabajo.Codigo)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if (bandera != 1)
                        //{

                        mT_GrupoTrabajo.Estado = mT_GrupoTrabajo.Estado.Substring(0, 1);


                        db.Entry(mT_GrupoTrabajo).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Grupo de Trabajo actualizado";

                        return RedirectToAction("MantGrupoTrabajo");
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Código ya existe";
                        //    return RedirectToAction("MantTipoTrabajo");
                        //}

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantGrupoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarGrupoTrabajo(int id)
        {
            try
            {

                MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);

                mT_GrupoTrabajo.Estado = "E";

                db.Entry(mT_GrupoTrabajo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Grupo Trabajo Eliminado";
                return RedirectToAction("MantGrupoTrabajo");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }
       
        //EQUIPOS DE GRUPO DE TRABAJO
        [HttpGet]
        public ActionResult Equipos_MantGrupoTrabajo(int id)
        {
            try
            {
                MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);
                if (mT_GrupoTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"] = mT_GrupoTrabajo.Id_GrupoTrabajo;
                    var listaEquiposGrupoTrabajo = db.sp_Quimipac_ConsultaMT_EquipoGrupoTrabajo(id).ToList();
                    ViewBag.listaEquiposGrupoTrabajo = listaEquiposGrupoTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Equipo no existe";
                    return RedirectToAction("MantGrupoTrabajo");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_EquipoMantGrupoTrabajo()
        {
            try
            {
                List<SelectListItem> equipos = new List<SelectListItem>();



                var listaEquipos = db.sp_Quimipac_ConsultaMt_Equipo().ToList();
                foreach (var equipo in listaEquipos)
                {
                    equipos.Add(new SelectListItem
                    {
                        Value = Convert.ToString(equipo.Id_Equipo),
                        Text = equipo.Descripcion
                    });
                }

                SelectList selectlistaEquipos = new SelectList(equipos, "Value", "Text");

                ViewBag.listaEquipos = selectlistaEquipos;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_EquipoMantGrupoTrabajo([Bind(Include = "Id_Equipo, Estado")] MT_GrupoTrabajoEquipo mT_GrupoTrabajoEquipo)
        {
            try
            {
                var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];

                if (id_MantGrupoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_GrupoTrabajoEquipo.Id_GrupoTrabajo = idMantGrupoTrabajo;
                        var itemGrupoTrabajo = db.MT_GrupoTrabajoEquipo.Where(x => x.Id_GrupoTrabajo == mT_GrupoTrabajoEquipo.Id_GrupoTrabajo && x.Id_Equipo == mT_GrupoTrabajoEquipo.Id_Equipo);
                        if (itemGrupoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Equipo ya existe";
                            return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                        }
                        else
                        {

                            mT_GrupoTrabajoEquipo.Id_GrupoTrabajo = idMantGrupoTrabajo;
                            mT_GrupoTrabajoEquipo.Estado = mT_GrupoTrabajoEquipo.Estado.Substring(0, 1);


                            db.MT_GrupoTrabajoEquipo.Add(mT_GrupoTrabajoEquipo);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Equipo guardado";
                            return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_EquipoMantGrupoTrabajo(int id)
        {
            try
            {
                MT_GrupoTrabajoEquipo mT_GrupoTrabajoEquipo = db.MT_GrupoTrabajoEquipo.Find(id);
                bool seleccion = false;
                if (mT_GrupoTrabajoEquipo != null)
                {

                    List<SelectListItem> equipos = new List<SelectListItem>();



                    var listaEquipos = db.sp_Quimipac_ConsultaMt_Equipo().ToList();
                    foreach (var equipo in listaEquipos)
                    {
                        if (mT_GrupoTrabajoEquipo.Id_Equipo == equipo.Id_Equipo)
                        {
                            seleccion = true;
                        }

                        equipos.Add(new SelectListItem { Value = Convert.ToString(equipo.Id_Equipo), Text = equipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEquipos = new SelectList(equipos, "Value", "Text");


                    ViewBag.listaEquipos = selectlistaEquipos;

                    return View(mT_GrupoTrabajoEquipo);

                }
                else
                {
                    var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];

                    if (id_MantGrupoTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());
                        TempData["mensaje_error"] = "Equipo no existe";
                        return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
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
        public ActionResult Editar_EquipoMantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajoEquipo,Id_GrupoTrabajo,Id_Equipo,Estado")] MT_GrupoTrabajoEquipo mT_GrupoTrabajoEquipo)
        {
            try
            {
                var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];
                // int bandera = 0;
                if (id_MantGrupoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        //var listaItemsTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(idMantTipoTrabajo).ToList();

                        //foreach (var objt in listaItemsTipoTrabajo)
                        //{
                        //    if(objt.Id_MTTipoTrabajo == mT_Items.Id_MTTipoTrabajo && objt.Id_Item == mT_Items.Id_Item)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if(bandera!=1)
                        //{
                        mT_GrupoTrabajoEquipo.Estado = mT_GrupoTrabajoEquipo.Estado.Substring(0, 1);


                        db.Entry(mT_GrupoTrabajoEquipo).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Equipo grupo de trabajo actualizado";

                        return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Registro ya existe";
                        //    return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Equipos_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        //INTEGRANTES DE GRUPO DE TRABAJO
        [HttpGet]
        public ActionResult Integrantes_MantGrupoTrabajo(int id)
        {
            try
            {
                MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);
                if (mT_GrupoTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"] = mT_GrupoTrabajo.Id_GrupoTrabajo;
                    var listaIntegrantesGrupoTrabajo = db.sp_Quimipac_ConsultaMT_IntegranteGrupoTrabajo(id).ToList();
                    ViewBag.listaIntegrantesGrupoTrabajo = listaIntegrantesGrupoTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantGrupoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_IntegranteMantGrupoTrabajo()
        {
            try
            {
                List<SelectListItem> integrantes = new List<SelectListItem>();
                List<SelectListItem> roles = new List<SelectListItem>();


                var listaIntegrantes = db.sp_LINK_ConsultaPersonas().ToList();
                foreach (var integrante in listaIntegrantes)
                {
                    integrantes.Add(new SelectListItem
                    {
                        Value = Convert.ToString(integrante.id_persona),
                        Text = integrante.Nombres_Completos
                    });
                }

                SelectList selectlistaIntegrantes = new SelectList(integrantes, "Value", "Text");

                var listaRol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                foreach (var rol in listaRol)
                {
                    roles.Add(new SelectListItem
                    {
                        Value = Convert.ToString(rol.Id_TablaDetalle),
                        Text = rol.Descripcion
                    });
                }

                SelectList selectlistaRol = new SelectList(roles, "Value", "Text");

                

                ViewBag.listaIntegrantes = selectlistaIntegrantes;
                ViewBag.listaRol = selectlistaRol;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_IntegranteMantGrupoTrabajo([Bind(Include = "Id_Persona, Rol_Usuario, Estado")] MT_GrupoTrabajoIntegrante mT_GrupoTrabajoIntegrante)
        {
            try
            {
                var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];

                if (id_MantGrupoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_GrupoTrabajoIntegrante.Id_GrupoTrabajo = idMantGrupoTrabajo;
                        var itemGrupoTrabajo = db.MT_GrupoTrabajoIntegrante.Where(x => x.Id_GrupoTrabajo == mT_GrupoTrabajoIntegrante.Id_GrupoTrabajo && x.Id_Persona == mT_GrupoTrabajoIntegrante.Id_Persona);
                        if (itemGrupoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Integrante ya existe";
                            return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                        }
                        else
                        {

                            mT_GrupoTrabajoIntegrante.Id_GrupoTrabajo = idMantGrupoTrabajo;
                            mT_GrupoTrabajoIntegrante.Estado = mT_GrupoTrabajoIntegrante.Estado.Substring(0, 1);


                            db.MT_GrupoTrabajoIntegrante.Add(mT_GrupoTrabajoIntegrante);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Integrante guardado";
                            return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_IntegranteMantGrupoTrabajo(int id)
        {
            try
            {
                MT_GrupoTrabajoIntegrante mT_GrupoTrabajoIntegrante = db.MT_GrupoTrabajoIntegrante.Find(id);
                bool seleccion = false;
                if (mT_GrupoTrabajoIntegrante != null)
                {

                    List<SelectListItem> integrantes = new List<SelectListItem>();
                    List<SelectListItem> roles = new List<SelectListItem>();



                    var listaIntegrantes = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var integrante in listaIntegrantes)
                    {
                        if (mT_GrupoTrabajoIntegrante.Id_Persona == integrante.id_persona)
                        {
                            seleccion = true;
                        }

                        integrantes.Add(new SelectListItem { Value = Convert.ToString(integrante.id_persona), Text = integrante.Nombres_Completos, Selected = seleccion });
                    }

                    SelectList selectlistaIntegrantes = new SelectList(integrantes, "Value", "Text");

                    var listaRol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                    foreach (var rol in listaRol)
                    {
                        if (mT_GrupoTrabajoIntegrante.Rol_Usuario == rol.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        roles.Add(new SelectListItem { Value = Convert.ToString(rol.Id_TablaDetalle), Text = rol.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaRol = new SelectList(roles, "Value", "Text");


                    ViewBag.listaIntegrantes = selectlistaIntegrantes;
                    ViewBag.listaRol = selectlistaRol;

                    return View(mT_GrupoTrabajoIntegrante);

                }
                else
                {
                    var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];

                    if (id_MantGrupoTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());
                        TempData["mensaje_error"] = "Integrante no existe";
                        return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
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
        public ActionResult Editar_IntegranteMantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajoIntegrante,Id_GrupoTrabajo,Id_Persona, Rol_Usuario, Estado")] MT_GrupoTrabajoIntegrante mT_GrupoTrabajoIntegrante)
        {
            try
            {
                var id_MantGrupoTrabajo = System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"];
                // int bandera = 0;
                if (id_MantGrupoTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantGrupoTrabajo = int.Parse(id_MantGrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        //var listaItemsTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(idMantTipoTrabajo).ToList();

                        //foreach (var objt in listaItemsTipoTrabajo)
                        //{
                        //    if(objt.Id_MTTipoTrabajo == mT_Items.Id_MTTipoTrabajo && objt.Id_Item == mT_Items.Id_Item)
                        //    {
                        //        bandera = 1;
                        //    }
                        //}

                        //if(bandera!=1)
                        //{
                        mT_GrupoTrabajoIntegrante.Estado = mT_GrupoTrabajoIntegrante.Estado.Substring(0, 1);


                        db.Entry(mT_GrupoTrabajoIntegrante).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Integrante grupo de trabajo actualizado";

                        return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Registro ya existe";
                        //    return RedirectToAction("Items_MantTipoTrabajo", new { id = idMantTipoTrabajo });
                        //}

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Integrantes_MantGrupoTrabajo", new { id = idMantGrupoTrabajo });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion


        //MATENIMIENTO EQUIPO TRABAJO

        #region
        public ActionResult MantEquipoTrabajo()
        {
            try
            {
                var listaEquipo = db.sp_Quimipac_ConsultaMT_EquipoTrabajo().ToList();
                ViewBag.listaEquipo = listaEquipo;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        [HttpGet]
        public ActionResult Agregar_MantEquipoTrabajo()
        {
            try
            {
                List<SelectListItem> itemsTipoEquipo = new List<SelectListItem>();
                List<SelectListItem> itemsPersona = new List<SelectListItem>();

                var listaTipoEquipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(40).ToList();
                foreach (var tipo in listaTipoEquipos)
                {
                    itemsTipoEquipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                SelectList selectlistaTipoEquipo = new SelectList(itemsTipoEquipo, "Value", "Text");


                var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();
                foreach (var Persona in listaPersonas)
                {
                    itemsPersona.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos });
                }

                SelectList selectlistaPersonas = new SelectList(itemsPersona, "Value", "Text");



                ViewBag.listaTipoEquipos = selectlistaTipoEquipo;
                ViewBag.listaPersonas = selectlistaPersonas;



                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantEquipoTrabajo([Bind(Include = "Id_Proveedor, Id_Persona_asignada,Tipo,Codigo,Descripcion, Numero_Serie,Marca,Modelo,Fecha, Estado,Horas_Usadas")] MT_Equipo mT_EquipoTrabajo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var equipo = db.MT_Equipo.Where(x => x.Id_Proveedor == mT_EquipoTrabajo.Id_Proveedor && x.Id_Persona_asignada == mT_EquipoTrabajo.Id_Persona_asignada && x.Tipo == mT_EquipoTrabajo.Tipo && x.Codigo == mT_EquipoTrabajo.Codigo);
                    if (equipo.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Equipo ya existe";
                        return RedirectToAction("MantEquipoTrabajo");
                    }
                    else
                    {
                        mT_EquipoTrabajo.Estado = mT_EquipoTrabajo.Estado.Substring(0, 1);


                        db.MT_Equipo.Add(mT_EquipoTrabajo);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Equipo guardado";
                        return RedirectToAction("MantEquipoTrabajo");

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantGrupoTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantEquipoTrabajo(int id)
        {
            try
            {

                MT_Equipo mT_EquipoTrabajo = db.MT_Equipo.Find(id);
                bool seleccion = false;
                if (mT_EquipoTrabajo != null)
                {


                    List<SelectListItem> itemsTipoEquipo = new List<SelectListItem>();
                    List<SelectListItem> itemsPersona = new List<SelectListItem>();

                    var listaTipoEquipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(40).ToList();
                    foreach (var tipoequipo in listaTipoEquipos)
                    {
                        if (tipoequipo.Id_TablaDetalle == mT_EquipoTrabajo.Tipo)
                        {
                            seleccion = true;
                        }

                        itemsTipoEquipo.Add(new SelectListItem { Value = Convert.ToString(tipoequipo.Id_TablaDetalle), Text = tipoequipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTipoEquipo = new SelectList(itemsTipoEquipo, "Value", "Text");


                    var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();

                    foreach (var Persona in listaPersonas)
                    {
                        if (Persona.id_persona == mT_EquipoTrabajo.Id_Persona_asignada)
                        {
                            seleccion = true;
                        }

                        itemsPersona.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos, Selected = seleccion });
                    }

                    SelectList selectlistaPersonas = new SelectList(itemsPersona, "Value", "Text");



                    ViewBag.listaTipoEquipos = selectlistaTipoEquipo;
                    ViewBag.listaPersonas = selectlistaPersonas;


                    return View(mT_EquipoTrabajo);

                }
                else
                {
                    TempData["mensaje_error"] = "Equipo Trabajo no existe";
                    return RedirectToAction("MantEquipoTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantEquipoTrabajo([Bind(Include = "Id_Equipo, Id_Proveedor, Id_Persona_asignada,Tipo,Codigo,Descripcion, Numero_Serie,Marca,Modelo, Fecha, Estado,Horas_Usadas")] MT_Equipo mT_EquipoTrabajo)
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
                        mT_EquipoTrabajo.Estado = mT_EquipoTrabajo.Estado.Substring(0, 1);
                        mT_EquipoTrabajo.Fecha = DateTime.Now;


                        db.Entry(mT_EquipoTrabajo).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Equipo de Trabajo actualizado";

                        return RedirectToAction("MantEquipoTrabajo");
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantEquipoTrabajo");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarMantEquipoTrabajo(int id)
        {
            try
            {

                MT_Equipo mT_EquipoTrabajo = db.MT_Equipo.Find(id);

                mT_EquipoTrabajo.Estado = "E";

                db.Entry(mT_EquipoTrabajo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Equipo Trabajo Eliminado";
                return RedirectToAction("MantEquipoTrabajo");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }


        #endregion

        //DATOS PERSONALES

        #region
        [HttpGet]
        public ActionResult DatosPersonales()
        {
            try
            {
                var idUsuario = System.Web.HttpContext.Current.Session["usuario"];
                if (idUsuario == null)
                {
                    return RedirectToAction("IniciarSesion", "HomeController");
                }
                else
                {

                    //var bde = new DataBase_Externo();
                    //string usuario = System.Web.HttpContext.Current.Session["usuario"] as string;

                    //var modelo = bde.DatosUsuario(usuario);
                    //ViewBag.modelo = modelo;
                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        #endregion

        //MANT SECTOR
        #region

        [HttpGet]
        public ActionResult MantSector()
        {
            try
            {

                var listaSector = db.sp_Quimipac_ConsultaMT_Sector().ToList();
                ViewBag.listaSector = listaSector;
                return View();
                
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantSector()
        {
            try
            {
                List<SelectListItem> itemsSector = new List<SelectListItem>();

                var listaSector = db.MT_Sector.Where(x => x.Estado != "E").ToList();
                foreach (var sector in listaSector)
                {
                    itemsSector.Add(new SelectListItem { Value = Convert.ToString(sector.Id_Sector), Text = sector.Nombre });
                }

                itemsSector.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaSector = new SelectList(itemsSector, "Value", "Text");

                ViewBag.listaSector = selectlistaSector;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
       public ActionResult Agregar_MantSector([Bind(Include = "Id_Padre_Sector,Nombre,Estado")] MT_Sector mT_Sector)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sector = db.MT_Sector.Where(x => x.Nombre == mT_Sector.Nombre);
                    if (sector.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Sector ya existe";
                        return RedirectToAction("MantSector");
                    }
                    else
                    {
                        mT_Sector.Estado = mT_Sector.Estado.Substring(0, 1);


                        db.MT_Sector.Add(mT_Sector);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Sector guardado";
                        return RedirectToAction("MantSector");

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantSector");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantSector(int id)
        {
            try
            {
                MT_Sector mT_Sector = db.MT_Sector.Find(id);
                bool seleccion = false;
                if (mT_Sector != null)
                {
                    List<SelectListItem> itemsSector = new List<SelectListItem>();

                    var listaSector = db.MT_Sector.Where(x => x.Estado != "E").ToList();

                    itemsSector.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    foreach (var sector in listaSector)
                    {

                        if (sector.Id_Padre_Sector == mT_Sector.Id_Padre_Sector)
                        {
                            seleccion = true;
                        }


                        itemsSector.Add(new SelectListItem { Value = Convert.ToString(sector.Id_Sector), Text = sector.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistaSector = new SelectList(itemsSector, "Value", "Text");
                    

                    ViewBag.listaSector = selectlistaSector;

                    return View(mT_Sector);
                }
                else
                {
                    TempData["mensaje_error"] = "Sector no existe";
                    return RedirectToAction("MantSector");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantSector([Bind(Include = "Id_Sector, Id_Padre_Sector,Nombre,Estado")] MT_Sector mT_Sector)
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

                        mT_Sector.Estado = mT_Sector.Estado.Substring(0, 1);


                        db.Entry(mT_Sector).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Sector actualizado";

                        return RedirectToAction("MantSector");
                        
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantSector");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarMantSector(int id)
        {
            try
            {

                MT_Sector mT_Sector = db.MT_Sector.Find(id);

                mT_Sector.Estado = "E";

                db.Entry(mT_Sector).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Scetor Eliminado";
                return RedirectToAction("MantSector");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        #endregion

        #region
        //CAMPAMENTO
        public ActionResult MantCampamento()
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresaCod = empresa_id.ToString();
                var listaCampamento = db.sp_Quimipac_ConsultaMT_CampamentoGeneral(empresaCod).ToList();
                ViewBag.listaCampamento = listaCampamento;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantCampamento()
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
        public ActionResult Agregar_MantCampamento([Bind(Include = "Id_Empresa,Nombre, Estado")] MT_Campamento mT_Campamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var campamento = db.MT_Campamento.Where(x => x.Nombre == mT_Campamento.Nombre);

                    if (campamento.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("MantCampamento");
                    }
                    else
                    {

                        mT_Campamento.Estado = mT_Campamento.Estado.Substring(0, 1);
                        db.MT_Campamento.Add(mT_Campamento);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Campamento guardado";
                        return RedirectToAction("MantCampamento");

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantCampamento");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantCampamento(int id)
        {
            try
            {

                MT_Campamento mT_Campamento = db.MT_Campamento.Find(id);
                bool seleccion = false;
                if (mT_Campamento != null)
                {





                    return View(mT_Campamento);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantCampamento");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantCampamento([Bind(Include = "Id_Campamento,Id_Empresa,Nombre, Estado")] MT_Campamento mT_Campamento)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        mT_Campamento.Estado = mT_Campamento.Estado.Substring(0, 1);




                        db.Entry(mT_Campamento).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Campamento actualizado";

                        return RedirectToAction("MantCampamento");
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantCampamento");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarMantCampamento(int id)
        {
            try
            {

                MT_Campamento mT_Campamento = db.MT_Campamento.Find(id);

                mT_Campamento.Estado = "E";

                db.Entry(mT_Campamento).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Campamento Eliminado";
                return RedirectToAction("MantCampamento");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }



        #endregion


        //SERVICIO 

        #region

        [HttpGet]
        public ActionResult MantServicios()
        {
            try
            {
                var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio().ToList();
                ViewBag.listaServicio = listaServicio;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Agregar_MantServicios()
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
        public ActionResult Agregar_MantServicios([Bind(Include = "Id_Empresa,Codigo,Descripcion, Estado")] MT_Servicio mT_Servicio)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var Servicios = db.MT_Servicio.Where(x => x.Codigo == mT_Servicio.Codigo);
                    if (Servicios.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("MantServicios");
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

                            mT_Servicio.Estado = mT_Servicio.Estado.Substring(0, 1);
                            db.MT_Servicio.Add(mT_Servicio);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Servicio guardado";
                            return RedirectToAction("MantServicios");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantServicios");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantServicios(int id)
        {
            try
            {

                MT_Servicio mT_Servicio = db.MT_Servicio.Find(id);

                if (mT_Servicio != null)
                {




                    return View(mT_Servicio);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantServicios");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantServicios([Bind(Include = "Id_Servicio, Id_Empresa,Codigo,Descripcion,Estado")] MT_Servicio mT_Servicio)
        {
            try
            {
                if (ModelState.IsValid)

                {

                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {

                        mT_Servicio.Estado = mT_Servicio.Estado.Substring(0, 1);
                        db.Entry(mT_Servicio).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Mantenimiento de Servicio actualizado";

                        return RedirectToAction("MantServicios");
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Código ya existe";
                        //    return RedirectToAction("MantTipoTrabajo");
                        //}

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantServicios");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }



        [HttpGet]
        public ActionResult EliminarServicio(int id)
        {
            try
            {

                MT_Servicio mT_Servicio = db.MT_Servicio.Find(id);

                mT_Servicio.Estado = "E";

                db.Entry(mT_Servicio).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Servicio Eliminado";
                return RedirectToAction("MantServicios");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }



        #endregion

        //MATENIMIENTO DE TABLAS
        #region
        [HttpGet]
        public ActionResult MantTabla()
        {
            try
            {
                var listaTabla = db.sp_Quimipac_ConsultaMT_Tabla().ToList();
                ViewBag.listaTabla = listaTabla;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_MantTabla()
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
        
        public ActionResult Agregar_MantTabla([Bind(Include = "Tabla,Descripcion,Descripcion,Estado")] MT_Tablas mT_Tablas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tablas = db.MT_Tablas.Where(x => x.Tabla == mT_Tablas.Tabla && x.Descripcion == mT_Tablas.Descripcion);
                    if (tablas.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("MantTabla");
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
                           
                            mT_Tablas.Estado = mT_Tablas.Estado.Substring(0, 1);
                            

                            db.MT_Tablas.Add(mT_Tablas);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Registro Tabla guardado";
                            return RedirectToAction("MantTabla");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantTabla");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_MantTabla(int id)
        {
            try
            {

                MT_Tablas mT_Tablas = db.MT_Tablas.Find(id);
                bool seleccion = false;
                if (mT_Tablas != null)
                {

                    return View(mT_Tablas);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTabla");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantTabla([Bind(Include = "Id_Tabla,Tabla,Descripcion,Descripcion,Estado")] MT_Tablas mT_Tablas)
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


                      
                       
                        mT_Tablas.Estado = mT_Tablas.Estado.Substring(0, 1);
                        
                        db.Entry(mT_Tablas).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Registro Tabla actualizado";

                        return RedirectToAction("MantTabla");
                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Código ya existe";
                        //    return RedirectToAction("MantTipoTrabajo");
                        //}

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("MantTabla");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarMantTabla(int id)
        {
            try
            {

                MT_Tablas mT_Tablas = db.MT_Tablas.Find(id);

                mT_Tablas.Estado = "E";

                db.Entry(mT_Tablas).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Registro Tabla Eliminado";
                return RedirectToAction("MantTabla");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }
        
        [HttpGet]
        public ActionResult Detalle_MantTabla(int id)
        {
            try
            {
                MT_Tablas mT_Tabla = db.MT_Tablas.Find(id);

                if (mT_Tabla != null)
                {
                    System.Web.HttpContext.Current.Session["id_Tabla"] = mT_Tabla.Id_Tabla;
                    var listaTablaDetalle = db.sp_Quimipac_ConsultaMT_TablaDetalleGeneral(mT_Tabla.Id_Tabla).ToList();
                    ViewBag.listaTablaDetalle = listaTablaDetalle;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("MantTabla");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        
        [HttpGet]
        public ActionResult Agregar_Detalle_MantTabla()
        {
            try
            {
                List<SelectListItem> itemsDetalleTabla = new List<SelectListItem>();

                var listadetalle = db.MT_TablaDetalle.Where(x => x.Estado == "A" || x.Estado == "I").ToList();
                foreach (var detalle in listadetalle)
                {
                    itemsDetalleTabla.Add(new SelectListItem { Value = Convert.ToString(detalle.Id_TablaDetalle), Text = detalle.Codigo });
                }

                itemsDetalleTabla.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaDetalle = new SelectList(itemsDetalleTabla, "Value", "Text");

                ViewBag.listadetalle = selectlistaDetalle;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Detalle_MantTabla([Bind(Include = "Id_Padre,Codigo,Descripcion, Estado")] MT_TablaDetalle mT_Detalle)
        {
            try
            {
                var id_MantTabla = System.Web.HttpContext.Current.Session["id_MantTabla"];

                if (id_MantTabla == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantTabla = int.Parse(id_MantTabla.ToString());

                    if (ModelState.IsValid)
                    {



                        var tabla = db.MT_TablaDetalle.Where(x => x.Codigo == mT_Detalle.Codigo);
                        if (tabla.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("Detalle_MantTabla", new { id = idMantTabla });
                        }
                        else
                        {

                            mT_Detalle.Id_Tabla = idMantTabla;



                            mT_Detalle.Estado = mT_Detalle.Estado.Substring(0, 1);


                            db.MT_TablaDetalle.Add(mT_Detalle);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Registro Detalle Tabla guardada";
                            return RedirectToAction("Detalle_MantTabla", new { id = idMantTabla });


                        }


                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Detalle_MantTabla", new { id = idMantTabla });
                    }

                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Detalle_MantTabla(int id)
        {
            try
            {
                MT_TablaDetalle mT_Detalle = db.MT_TablaDetalle.Find(id);
                bool seleccion = false;
                if (mT_Detalle != null)
                {
                    List<SelectListItem> itemsDetalle = new List<SelectListItem>();

                    var listaDetalle = db.MT_TablaDetalle.Where(x => x.Estado == "A" || x.Estado == "I").ToList();

                    itemsDetalle.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    foreach (var detalle in listaDetalle)
                    {

                        if (detalle.Id_Padre == mT_Detalle.Id_Padre)
                        {
                            seleccion = true;
                        }


                        itemsDetalle.Add(new SelectListItem { Value = Convert.ToString(detalle.Id_TablaDetalle), Text = detalle.Codigo, Selected = seleccion });
                    }

                    SelectList selectlistaDetalle = new SelectList(itemsDetalle, "Value", "Text");


                   

                    ViewBag.listaDetalle = selectlistaDetalle;

                    return View(mT_Detalle);
                }
                else
                {
                    var id_MantTabla = System.Web.HttpContext.Current.Session["id_MantTabla"];

                    if (id_MantTabla == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantTabla = int.Parse(id_MantTabla.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Detalle_MantTabla", new { id = idMantTabla });
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
        public ActionResult Editar_Detalle_MantTabla([Bind(Include = "Id_TablaDetalle,Id_Tabla,Id_Padre,Codigo,Descripcion, Estado")] MT_TablaDetalle mT_Detalle)
        {
            try
            {
                //int bandera = 0;
                if (ModelState.IsValid)
                {



                    mT_Detalle.Estado = mT_Detalle.Estado.Substring(0, 1);

                    db.Entry(mT_Detalle).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Registro Detalle Tabla actualizado";

                    return RedirectToAction("Detalle_MantTabla", new { id = mT_Detalle.Id_Tabla });
                    
                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Detalle_MantTabla", new { id = mT_Detalle.Id_Tabla });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Solicitud_Materiales()
        {
            try
            {
                var listaSolicitud = db.sp_Quimipac_ConsultaMT_Solicitud().ToList();
                ViewBag.listaSolicitud = listaSolicitud;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //Servicios


        //MATENIMIENTO DE PERMISOS
        #region
        [HttpGet]
        public ActionResult MantPermisos()
        {
            try
            {
                //var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo().ToList();
                //ViewBag.listaTiposTrabajo = listaTiposTrabajo;
                var LkRol = db.Roles.Where(v => v.Estado == "A").ToList();
                var LkUsuario = db.sp_LINK_ConsultaUsuarios("").ToList();
                var LkMenu = db.Menu.Where(v1 =>v1.Estado == "A").ToList();

                ViewBag.LkRol = LkRol;
                ViewBag.LkUsuario = LkUsuario;
                ViewBag.LkMenu = LkMenu;
                /*var LkMenu    =*/
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