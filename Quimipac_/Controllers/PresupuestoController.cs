using Quimipac_.Models;
using Quimipac_.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Quimipac_.Controllers
{
    public class PresupuestoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        //Presupuesto
        #region
        [HttpGet]
        public ActionResult Presupuesto()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var listaPresupuesto = db.sp_Quimipac_ConsultaMT_Presupuesto(user_id.ToString(), empresa_id.ToString()).ToList();
                ViewBag.listaPresupuesto = listaPresupuesto;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Presupuesto()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                List<SelectListItem> itemsMoneda = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                 var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaClientes = listaClientes.Where(vq => ClienteLI.Contains(vq.cod_cli)).ToList();

                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                foreach (var sucursal in listaSucursal)
                {
                    itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre });
                }

                SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");

                var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                foreach (var moneda in listaMoneda)
                {
                    itemsMoneda.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre });
                }

                SelectList selectlistaMoneda = new SelectList(itemsMoneda, "Value", "Text");

                ViewBag.listaClientes = selectlistaClientes;
                ViewBag.listaSucursal = selectlistaSucursal;
                ViewBag.listaMoneda = selectlistaMoneda;
                

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        } else
            {   return RedirectToAction("IniciarSesion", "Home");
    }
}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Presupuesto([Bind(Include = "Id_Presupuesto,Id_Empresa,Id_Sucursal,Id_Cliente,Fecha,Id_Usuario,Comentario,Porcentaje_indirectos,Monto_Certificacion,Iva_Certificacion,Validez,Moneda")] MT_Presupuesto mT_Presupuesto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                if (ModelState.IsValid)
                {
                    var presupuesto = db.MT_Presupuesto.Where(x => x.Id_Sucursal == mT_Presupuesto.Id_Sucursal && x.Id_Cliente == mT_Presupuesto.Id_Cliente && x.Fecha == mT_Presupuesto.Fecha && x.Monto_Certificacion == mT_Presupuesto.Monto_Certificacion && x.Iva_Certificacion == mT_Presupuesto.Iva_Certificacion);
                    if (presupuesto.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Código ya existe";
                        return RedirectToAction("Presupuesto");
                    }
                    else
                    {
                        var user_id = System.Web.HttpContext.Current.Session["usuario"];
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        if (user_id == null)
                        {
                            return Redirect(Url.Action("IniciarSesion", "Home"));
                        }
                        else
                        {
                            mT_Presupuesto.Id_Empresa = empresa_id.ToString();
                            mT_Presupuesto.Id_Usuario = user_id.ToString();
                            //mT_Presupuesto.Fecha = DateTime.Now;

                            //db.MT_Presupuesto.Add(mT_Presupuesto);
                            //db.SaveChanges();

                            DataBase_Externo db = new DataBase_Externo();
                            var resp =db.InsertarPresupuesto(mT_Presupuesto.Id_Empresa, mT_Presupuesto.Id_Sucursal, mT_Presupuesto.Id_Cliente, mT_Presupuesto.Id_Usuario, mT_Presupuesto.Comentario, mT_Presupuesto.Porcentaje_indirectos, mT_Presupuesto.Monto_Certificacion, mT_Presupuesto.Iva_Certificacion, mT_Presupuesto.Validez, mT_Presupuesto.Moneda);
                            if (resp >0)
                            {
                                TempData["mensaje_correcto"] = "Presupuesto guardado";
                            }
                            else { TempData["mensaje_error"] = "Valores incorrectos"; }
                           // TempData["mensaje_correcto"] = "Presupuesto guardado";
                            return RedirectToAction("Presupuesto");
                        }

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Presupuesto");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [HttpGet]
        public ActionResult Editar_Presupuesto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
            {

                MT_Presupuesto mT_Presupuesto = db.MT_Presupuesto.Find(id);
                System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_Presupuesto.Fecha;
                bool seleccion = false;
                if (mT_Presupuesto != null)
                {

                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                    List<SelectListItem> itemsMoneda = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaClientes = listaClientes.Where(vq => ClienteLI.Contains(vq.cod_cli)).ToList();

                    foreach (var cliente in listaClientes)
                    {
                        if (cliente.cod_cli == mT_Presupuesto.Id_Cliente)
                        {
                            seleccion = true;
                        }

                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                    var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                    foreach (var sucursal in listaSucursal)
                    {
                        if (sucursal.cod_suc == mT_Presupuesto.Id_Sucursal)
                        {
                            seleccion = true;
                        }

                        itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");

                    var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                    foreach (var moneda in listaMoneda)
                    {
                        if (moneda.codmon == mT_Presupuesto.Moneda)
                        {
                            seleccion = true;
                        }

                        itemsMoneda.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaMoneda = new SelectList(itemsMoneda, "Value", "Text");

                    ViewBag.listaClientes = selectlistaClientes;
                    ViewBag.listaSucursal = selectlistaSucursal;
                    ViewBag.listaMoneda = selectlistaMoneda;


                    return View(mT_Presupuesto);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Presupuesto");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        } else
            {   return RedirectToAction("IniciarSesion", "Home");
    }
}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Presupuesto([Bind(Include = "Id_Presupuesto,Id_Empresa,Id_Sucursal, Id_Cliente,Fecha,Id_Usuario,Comentario,Porcentaje_indirectos,Monto_Certificacion,Iva_Certificacion,Validez,Moneda")] MT_Presupuesto mT_Presupuesto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                if (ModelState.IsValid)
                {
                    //int bandera = 1;
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {


                        mT_Presupuesto.Id_Usuario = user_id.ToString();
                        mT_Presupuesto.Id_Empresa = empresa_id.ToString();
                        mT_Presupuesto.Fecha = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);



                        db.Entry(mT_Presupuesto).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Presupuesto actualizado";

                        return RedirectToAction("Presupuesto");

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Presupuesto");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        //[HttpGet]
        //public ActionResult EliminarPresupuesto(int id)
        //{
        //    try
        //    {

        //        MT_Presupuesto mT_Presupuesto = db.MT_Presupuesto.Find(id);

        //        mT_Presupuesto.Estado = "E";

        //        db.Entry(mT_Presupuesto).State = EntityState.Modified;
        //        db.SaveChanges();

        //        TempData["mensaje_actualizado"] = "Presupuesto Eliminado";
        //        return RedirectToAction("Presupuesto");
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }

        //}


        //public JsonResult GetProyecto(string id)
        //{
        //    try
        //    {
        //        List<SelectListItem>proyectos = new List<SelectListItem>();
        //        var listaProyecto = db.sp_Quimipac_ConsultaMT_ProyectoPresupuesto(id).ToList();

        //        foreach (var proyecto in listaProyecto)
        //        {
        //            proyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
        //        }
        //        //itemsitemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        //        SelectList selectlistaProyectoPresupuesto = new SelectList(proyectos, "Value", "Text");

        //        // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

        //        return Json(selectlistaProyectoPresupuesto, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(Response.StatusCode);
        //    }
        //}

        //public JsonResult InsertarProyecto(List<TempProyecto> lkProyecto, string ValueCliente, string ValueSucursal, string Comentario, decimal Porcentaje, decimal Monto_Certificacion, decimal Iva, decimal Validez, string ValueMoneda)
        //{
        //    var user_id = System.Web.HttpContext.Current.Session["usuario"];
        //    var empresa = System.Web.HttpContext.Current.Session["empresa"];
        //    //var mensajeDiv = new TempMensaje();
        //    var mensaje = new JsonResult();
        //    int nRegistro = 0;
        //    try
        //    {

        //        //var objectContext = ((IObjectContextAdapter)db).ObjectContext;
        //        //MT_PrecioReferencial MTPPreferencial;

        //        //foreach (var item in lkOrden)
        //        //{
        //        DataBase_Externo dbe = new DataBase_Externo();

        //        var band = dbe.InsertarProyecto(lkProyecto, System.Web.HttpContext.Current.Session["empresa"].ToString(), ValueCliente, ValueSucursal, System.Web.HttpContext.Current.Session["usuario"].ToString(), Comentario, Porcentaje, Monto_Certificacion, Iva, Validez, ValueMoneda);
        //        // nRegistro++;
        //        //}




        //        if (band < 1)
        //        {
        //            //mensaje.Data = new { mensaje = "Error al ingresar, favor verificar datos ingresados" }; 
        //            mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
        //            return mensaje;
        //            // throw new ApplicationException("Error al ingresar, favor verificar datos ingresados");
        //            // throw new InvalidOperationException();
        //            //  
        //        }
        //        // mensajeDiv.Mensaje = "  Registro Guardado correctamente";
        //        //mensajeDiv.Ok = true;

        //        // objectContext.AddObject("MT_PrecioReferencial", MTPPreferencial);
        //        // db.SaveChanges();


        //        mensaje.Data = "Registros guardados correctamente";
        //        return mensaje;
        //        //  return Json(lkItems, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        //mensajeDiv.Ok = false;
        //        // return resultado;
        //        return Json(Response.StatusCode);
        //    }
        //}

        //public class TempProyecto
        //{
        //    public int Id_Proyecto { get; set; }
        //    public string Codigo_Cliente { get; set; }

        //}

        #endregion

        //DetallePresupuesto 

        #region
        [HttpGet]
        public ActionResult Presupuesto_Detalle(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_Presupuesto mT_Presupuesto = db.MT_Presupuesto.Find(id);
                if (mT_Presupuesto != null)
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    System.Web.HttpContext.Current.Session["id_Presupuesto"] = mT_Presupuesto.Id_Presupuesto;
                    var listaPresupuestoDetalle = db.sp_Quimipac_ConsultaMT_PresupuestoDetalle(id, empresa_id.ToString()).ToList();
                    var presupuesto = System.Web.HttpContext.Current.Session["id_Presupuesto"].ToString();
                    ViewBag.presupuesto = presupuesto;
                    ViewBag.listaPresupuestoDetalle = listaPresupuestoDetalle;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Presupuesto");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        } else
            {   return RedirectToAction("IniciarSesion", "Home");
    }
}

        [HttpGet]
        public ActionResult Agregar_PresupuestoDetalle()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                List<SelectListItem> unidades = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_PresupuestoDetalle([Bind(Include = "Id_Presupuesto,Id_Item,Cantidad,unidad,Precio,Valor,IVA")] MT_Presupuesto_Detalle mT_PresupuestoDetalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var id_Presupuesto = System.Web.HttpContext.Current.Session["id_Presupuesto"];

                if (id_Presupuesto == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPresupuesto = int.Parse(id_Presupuesto.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_PresupuestoDetalle.Id_Presupuesto = idPresupuesto;
                        var presupuestodetalle = db.MT_Presupuesto_Detalle.Where(x => x.Id_Presupuesto == mT_PresupuestoDetalle.Id_Presupuesto && x.Id_Item == mT_PresupuestoDetalle.Id_Item && x.Cantidad == mT_PresupuestoDetalle.Cantidad && x.unidad == mT_PresupuestoDetalle.unidad && x.Precio == mT_PresupuestoDetalle.Precio);
                        if (presupuestodetalle.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Detalle Presupuesto ya existe";
                            return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });
                        }
                        else
                        {

                            mT_PresupuestoDetalle.Id_Presupuesto = idPresupuesto;
                            


                            db.MT_Presupuesto_Detalle.Add(mT_PresupuestoDetalle);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Detalle Presupuesto guardado";
                            return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        } else
            {   return RedirectToAction("IniciarSesion", "Home");
    }
}

        [HttpGet]
        public ActionResult Editar_PresupuestoDetalle(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_Presupuesto_Detalle mT_PresupuestoDetalle = db.MT_Presupuesto_Detalle.Find(id);
                bool seleccion = false;
                if (mT_PresupuestoDetalle != null)
                {

                    List<SelectListItem> items = new List<SelectListItem>();
                    List<SelectListItem> unidades = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                    foreach (var item in listaItems)
                    {
                        if (mT_PresupuestoDetalle.Id_Item == item.cod_item)
                        {
                            seleccion = true;
                        }

                        items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                    var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();
                    foreach (var unidad in listaUnidades)
                    {
                        if (mT_PresupuestoDetalle.unidad == unidad.uni_med)
                        {
                            seleccion = true;
                        }

                        unidades.Add(new SelectListItem { Value = Convert.ToString(unidad.uni_med), Text = unidad.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaUnidades = new SelectList(unidades, "Value", "Text");

                    
                    
                    ViewBag.listaItems = selectlistaItems;
                    ViewBag.listaUnidades = selectlistaUnidades;

                    return View(mT_PresupuestoDetalle);

                }
                else
                {
                    var id_Presupuesto = System.Web.HttpContext.Current.Session["id_Presupesto"];

                    if (id_Presupuesto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idPresupuesto = int.Parse(id_Presupuesto.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_PresupuestoDetalle([Bind(Include = "Id_Presupuesto_detalle,Id_Presupuesto,Id_Item,Cantidad,unidad,Precio,Valor,IVA")] MT_Presupuesto_Detalle mT_PresupuestoDetalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
            try
            {
                var id_Presupuesto = System.Web.HttpContext.Current.Session["id_Presupuesto"];
                // int bandera = 0;
                if (id_Presupuesto == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPresupuesto = int.Parse(id_Presupuesto.ToString());

                    if (ModelState.IsValid)
                    {

                       


                        db.Entry(mT_PresupuestoDetalle).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Presupuesto Detalle actualizado";

                        return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });
                        
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Presupuesto_Detalle", new { id = idPresupuesto });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        } else
            {   return RedirectToAction("IniciarSesion", "Home");
    }
}
        #endregion


    }
}
