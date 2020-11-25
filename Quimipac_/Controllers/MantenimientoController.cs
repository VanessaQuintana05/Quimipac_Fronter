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
using System.Data.Entity.Infrastructure;
using Quimipac_.Repositorio;

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
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    //var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo(empresa_id).ToList();
                    //ViewBag.listaTiposTrabajo = listaTiposTrabajo;
                    var listaobj = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>();
                    using (var ctx = new BD_QUIMIPACEntities())
                    { //{empresa_id},{0},{"SinPadre"},{0},{0}
                        string sql =$"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + 0 + ",'SinPadre'," + 0 +","+0;
                        var hijTB = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql).ToList();                       

                        foreach (var item in hijTB)
                        {
                            listaobj.Add(item);

                            string sql2 = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + item.Id_TipoTrabajo + ",'Hijo',"+ item.Id_TipoTrabajo + "," + 0;
                            var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql2).ToList();

                            //var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>($"sp_Quimipac_Padre_Hijo_XTTRABAJO @p0,@p1,@p2,@p3,@p4",empresa_id,item.Id_TipoTrabajo,"Hijo",item.Id_Padre,0).ToList();

                            foreach (var item2 in hijTC)
                                {
                                    listaobj.Add(item2);

                                string sql3 = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + item.Id_TipoTrabajo + ",'Nieto'," + item.Id_TipoTrabajo + "," + 0;
                                var hijTD = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql3).ToList();

                                //var hijTD = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>($"sp_Quimipac_Padre_Hijo_XTTRABAJO @p0,@p1,@p2,@p3,@p4", empresa_id,item.Id_TipoTrabajo,"Nieto",item.Id_Padre,0).ToList();
                                    foreach (var item3 in hijTD)
                                    {
                                        listaobj.Add(item3);
                                    }
                                }
                            }
                    }

                    ViewBag.listaTiposTrabajo = listaobj;
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
        public ActionResult Agregar_MantTipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {
                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsLineas = new List<SelectListItem>();
                    List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsServicio = new List<SelectListItem>();
                    List<SelectListItem> itemsTrabajoPadre = new List<SelectListItem>();
                    List<SelectListItem> itemsClasificacion = new List<SelectListItem>();

                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio(empresa_id.ToString()).ToList();
                    foreach (var servicio in listaServicio)
                    {
                        itemsServicio.Add(new SelectListItem { Value = Convert.ToString(servicio.Id_Servicio), Text = servicio.Descripcion });
                    }

                    SelectList selectlistaServicios = new SelectList(itemsServicio, "Value", "Text");

                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                    
                    foreach (var cliente in listaClientes)
                    {
                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                    var listaLineas = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
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

                    //

                    //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1, empresa_id.ToString()).Where(vq=> vq.Id_TipoTrabajo  NOT IN(SELECT DISTINCT Id_Padre FROM MT_TipoTrabajo)).ToList();
                    //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1, empresa_id.ToString()).ToList();
                    //var lista_nonietos = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef_Result>();

                    ////Obtener nietos
                    //var lista_trabajo_nietos = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef(1, empresa_id.ToString()).ToList();
                    //if (lista_trabajo_nietos.Count() > 0)
                    //{

                    //    foreach (var item in lista_trabajo_nietos)
                    //    {
                    //        if (lista_trabajo_nietos.Any(vq => vq.Id_Padre == item.Id_Padre && vq.Id_Padre == 0))
                    //        {
                    //            lista_nonietos.Add(item);
                    //        }
                    //    }
                    //}

                    //sin Nietos
                    //var listaTrabajoPadre = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre_Result>();
                    //foreach (var itemAux in listaTrabajoPadreAux)
                    //{
                    //    if (lista_nonietos.Any(vq => vq.Id_Padre == itemAux.Id_Padre))
                    //    {
                    //        listaTrabajoPadre.Add(itemAux);
                    //    }
                    //}

                    var listaobjAgregar = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>();
                    using (var ctx = new BD_QUIMIPACEntities())
                    { //{empresa_id},{0},{"SinPadre"},{0},{0}
                        string sql = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + 0 + ",'SinPadre'," + 0 + "," + 0;
                        var hijTB = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql).Where(vq=> vq.Tipo == 1).ToList();

                        foreach (var item in hijTB)
                        {
                            listaobjAgregar.Add(item);

                            string sql2 = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + item.Id_TipoTrabajo + ",'Hijo'," + item.Id_TipoTrabajo + "," + 0;
                            var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql2).Where(vq => vq.Tipo == 1).ToList();

                            //var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>($"sp_Quimipac_Padre_Hijo_XTTRABAJO @p0,@p1,@p2,@p3,@p4",empresa_id,item.Id_TipoTrabajo,"Hijo",item.Id_Padre,0).ToList();

                            foreach (var item2 in hijTC)
                            {
                                listaobjAgregar.Add(item2);

                                
                            }
                        }
                    }

                    //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO
                    itemsTrabajoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var trabajoPadre in listaobjAgregar)
                    {
                        itemsTrabajoPadre.Add(new SelectListItem { Value = Convert.ToString(trabajoPadre.Id_TipoTrabajo), Text = trabajoPadre.Codigo + " | " + trabajoPadre.Descripcion }); //+ "-" + trabajoPadre.Descripcion
                    }

                    SelectList selectlistaTrabajoPadre = new SelectList(itemsTrabajoPadre, "Value", "Text");

                    var listaClasificacion = db.sp_Quimipac_ConsultaMT_TablaDetalle(1064).ToList();
                    foreach (var clasificacion in listaClasificacion)
                    {
                        itemsClasificacion.Add(new SelectListItem { Value = Convert.ToString(clasificacion.Id_TablaDetalle), Text = clasificacion.Descripcion });
                    }

                    SelectList selectlistaClasificacion = new SelectList(itemsClasificacion, "Value", "Text");


                    ViewBag.listaTrabajoPadre = selectlistaTrabajoPadre;

                    ViewBag.listaClientes = selectlistaClientes;
                    ViewBag.listaLineas = selectlistaLineas;
                    ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;
                    ViewBag.listaServicio = selectlistaServicios;
                    ViewBag.listaClasificacion = selectlistaClasificacion;

                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantTipoTrabajo([Bind(Include = "Id_Cliente,Codigo,Linea,Descripcion,Tipo,Estado,Control_Item,Control_Equipo,Control_Integrante,Control_Anexo,Control_Medida,Control_Costo, Id_Servicio, Id_Padre, Control_Raiz, Proceso, Alerta, Caida,Clasificacion")] MT_TipoTrabajo mT_TipoTrabajo)
        //public JsonResult Agregar_MantTipoTrabajo(MT_TipoTrabajo mT_TipoTrabajo) //
        {
            //var mensaje = new JsonResult();


            try
            {
                if (System.Web.HttpContext.Current.Session["usuario"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        var tipoTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo(empresa_id.ToString()).ToList().Where(x => x.Codigo == mT_TipoTrabajo.Codigo && x.Id_Cliente == mT_TipoTrabajo.Id_Cliente);
                        //var tipoTrabajo = db.MT_TipoTrabajo.Where(x => x.Codigo == mT_TipoTrabajo.Codigo);



                        if (tipoTrabajo.Count() >= 1)
                        {
                            /* esto fue la primera prueba 
                             * foreach (var item in tipoTrabajo)
                             {
                                 if (item.Descripcion == mT_TipoTrabajo.Descripcion && item.Codigo == mT_TipoTrabajo.Codigo)
                                 {
                                     TempData["mensaje_error"] = "Código ya existe";
                                     //return RedirectToAction("mT_TipoTrabajo");

                                 }
                             }*/
                            //return View(mT_TipoTrabajo);//mT_TipoTrabajo  null;
                            //RedirectToAction("MantTipoTrabajo");
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("MantTipoTrabajo");
                            //return View(mT_TipoTrabajo);
                        }


                        /* esto es lo que estaba probando de lo 2do q si  var n = (tipoTrabajo.Any(vq => vq.Descripcion == mT_TipoTrabajo.Descripcion && vq.Codigo == mT_TipoTrabajo.Codigo)) ? "Código ya existe" : "";

                         if (n != "")
                         {
                             //TempData["mensaje_error"] = "Código ya existe";
                             //        return PartialView("Agregar_MantTipoTrabajo");
                             mensaje.Data = "Código ya existe";
                             return mensaje;
                         } */
                        else
                        {
                            var user_id = System.Web.HttpContext.Current.Session["usuario"];

                            if (user_id == null)
                            {
                               return RedirectToAction("IniciarSesion", "Home");
                                /* esto tbn es de lo 2do mensaje.Data = "Verifique";
                                return mensaje;*/
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
                                mT_TipoTrabajo.Control_Raiz = mT_TipoTrabajo.Control_Raiz.Substring(0, 1);


                                db.MT_TipoTrabajo.Add(mT_TipoTrabajo);
                                db.SaveChanges();
                             TempData["mensaje_correcto"] = "Tipo de Trabajo guardado";
                             return RedirectToAction("MantTipoTrabajo");
                                /* esto t6bn es de lo 2do mensaje.Data = "Trabajo Guardado";
                                return mensaje;*/
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        //return RedirectToAction("MantTipoTrabajo");
                        return View(mT_TipoTrabajo);
                       /* esto tbn es de lo 2do mensaje.Data = "Valor Incorrecto";
                        return mensaje;*/
                    }
                }
                else
                {
                    /* esto tbn de lo 2do mensaje.Data = "Fin Sesion";*/
                     return RedirectToAction("IniciarSesion", "Home");
                }
                /* esto tbn de lo 2do return mensaje;*/
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
                /*return Json(Response.StatusCode);*/
            }


        }

        [HttpGet]
        public ActionResult Editar_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {

                    MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                    System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_TipoTrabajo.Fecha_Registro;
                    bool seleccion = false;
                    if (mT_TipoTrabajo != null)
                    {


                        List<SelectListItem> items = new List<SelectListItem>();


                        List<SelectListItem> itemsClientes = new List<SelectListItem>();
                        List<SelectListItem> itemsLineas = new List<SelectListItem>();
                        List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                        List<SelectListItem> itemsServicio = new List<SelectListItem>();
                        List<SelectListItem> itemsTrabajoPadre = new List<SelectListItem>();
                        List<SelectListItem> itemsClasificacion = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio(empresa_id.ToString()).ToList();
                        foreach (var servicio in listaServicio)
                        {
                            if (servicio.Id_Servicio == mT_TipoTrabajo.Id_Servicio)
                            {
                                seleccion = true;
                            }

                            itemsServicio.Add(new SelectListItem { Value = Convert.ToString(servicio.Id_Servicio), Text = servicio.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaServicio = new SelectList(itemsServicio, "Value", "Text");


                        var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                        foreach (var cliente in listaClientes)
                        {
                            if (cliente.cod_cli == mT_TipoTrabajo.Id_Cliente)
                            {
                                seleccion = true;
                            }

                            itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                        }

                        SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                        var listaLineas = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();

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




                        //var listaTrabajoPadre = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(mT_TipoTrabajo.Tipo, empresa_id.ToString()).ToList();
                        var listaobjAgregar = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>();
                        using (var ctx = new BD_QUIMIPACEntities())
                        { //{empresa_id},{0},{"SinPadre"},{0},{0}
                            string sql = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + 0 + ",'SinPadre'," + 0 + "," + 0;
                            var hijTB = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql).Where(vq => vq.Tipo == mT_TipoTrabajo.Tipo).ToList();

                            foreach (var item in hijTB)
                            {
                                listaobjAgregar.Add(item);

                                string sql2 = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + item.Id_TipoTrabajo + ",'Hijo'," + item.Id_TipoTrabajo + "," + 0;
                                var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql2).Where(vq => vq.Tipo == mT_TipoTrabajo.Tipo).ToList();

                                //var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>($"sp_Quimipac_Padre_Hijo_XTTRABAJO @p0,@p1,@p2,@p3,@p4",empresa_id,item.Id_TipoTrabajo,"Hijo",item.Id_Padre,0).ToList();

                                foreach (var item2 in hijTC)
                                {
                                    listaobjAgregar.Add(item2);


                                }
                            }
                        }

                        itemsTrabajoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                        foreach (var trabajo_Padre in listaobjAgregar)
                        {

                            if (trabajo_Padre.Id_Padre == mT_TipoTrabajo.Id_Padre)
                            {
                                seleccion = true;
                            }
                                                        
                            //itemsTrabajoPadre.Add(new SelectListItem { Value = Convert.ToString(trabajo_Padre.Id_TipoTrabajo), Text = trabajo_Padre.Descripcion, Selected = seleccion });
                            itemsTrabajoPadre.Add(new SelectListItem { Value = Convert.ToString(trabajo_Padre.Id_TipoTrabajo), Text = trabajo_Padre.Codigo + " | " + trabajo_Padre.Descripcion, Selected = seleccion });

                        }

                        SelectList selectlistaTrabajoPadre = new SelectList(itemsTrabajoPadre, "Value", "Text");

                        var listaClasificacion = db.sp_Quimipac_ConsultaMT_TablaDetalle(1064).ToList();
                        itemsClasificacion.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var clasificacion in listaClasificacion)
                        {
                            if (clasificacion.Id_TablaDetalle == mT_TipoTrabajo.Clasificacion)
                            {
                                seleccion = true;
                            }

                            itemsClasificacion.Add(new SelectListItem { Value = Convert.ToString(clasificacion.Id_TablaDetalle), Text = clasificacion.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaClasificacion = new SelectList(itemsClasificacion, "Value", "Text");


                        ViewBag.listaTrabajoPadre = selectlistaTrabajoPadre;

                        ViewBag.listaClientes = selectlistaClientes;
                        ViewBag.listaLineas = selectlistaLineas;
                        ViewBag.listaServicio = selectlistaServicio;
                        ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;
                        ViewBag.listaClasificacion = selectlistaClasificacion;

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantTipoTrabajo([Bind(Include = "Id_TipoTrabajo,Id_Cliente,Codigo,Linea,Descripcion,Tipo,Estado,Control_Item,Control_Equipo,Control_Integrante,Control_Anexo,Control_Medida,Control_Costo,Id_Servicio, Id_Padre, Control_Raiz, Proceso, Alerta, Caida,Clasificacion")] MT_TipoTrabajo mT_TipoTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            mT_TipoTrabajo.Fecha_Registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);
                            mT_TipoTrabajo.Estado = mT_TipoTrabajo.Estado.Substring(0, 1);
                            mT_TipoTrabajo.Control_Item = mT_TipoTrabajo.Control_Item.Substring(0, 1);
                            mT_TipoTrabajo.Control_Equipo = mT_TipoTrabajo.Control_Equipo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Integrante = mT_TipoTrabajo.Control_Integrante.Substring(0, 1);
                            mT_TipoTrabajo.Control_Anexo = mT_TipoTrabajo.Control_Anexo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Medida = mT_TipoTrabajo.Control_Medida.Substring(0, 1);
                            mT_TipoTrabajo.Control_Costo = mT_TipoTrabajo.Control_Costo.Substring(0, 1);
                            mT_TipoTrabajo.Control_Raiz = mT_TipoTrabajo.Control_Raiz.Substring(0, 1);

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        public JsonResult GetTiposTrabajo(int id)
        {
            try
            {
                List<SelectListItem> itemsTiposTrabajos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
                var listaobjAgregar = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>();
                using (var ctx = new BD_QUIMIPACEntities())
                { //{empresa_id},{0},{"SinPadre"},{0},{0}
                    string sql = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + 0 + ",'SinPadre'," + 0 + "," + 0;
                    var hijTB = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql).Where(vq => vq.Tipo == id).ToList();

                    foreach (var item in hijTB)
                    {
                        listaobjAgregar.Add(item);

                        string sql2 = $"sp_Quimipac_Padre_Hijo_XTTRABAJO '" + empresa_id + "'," + item.Id_TipoTrabajo + ",'Hijo'," + item.Id_TipoTrabajo + "," + 0;
                        var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>(sql2).Where(vq => vq.Tipo == id).ToList();

                        //var hijTC = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_TipoTrabajo_Result>($"sp_Quimipac_Padre_Hijo_XTTRABAJO @p0,@p1,@p2,@p3,@p4",empresa_id,item.Id_TipoTrabajo,"Hijo",item.Id_Padre,0).ToList();

                        foreach (var item2 in hijTC)
                        {
                            listaobjAgregar.Add(item2);


                        }
                    }
                }
                //var lista_nonietos = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef_Result>();

                //Obtener nietos
                //var lista_trabajo_nietos = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef(id, empresa_id.ToString()).ToList();
                //if (lista_trabajo_nietos.Count() > 0)
                //{

                //    foreach (var item in lista_trabajo_nietos)
                //    {
                //        if (lista_trabajo_nietos.Any(vq => vq.Id_Padre == item.Id_Padre && vq.Id_Padre == 0))
                //        {
                //            lista_nonietos.Add(item);
                //        }
                //    }
                //}

                ////sin Nietos
                //var listaTrabajoPadre = new List<sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre_Result>();
                //foreach (var itemAux in listaTrabajoPadreAux)
                //{
                //    if (lista_nonietos.Any(vq => vq.Id_Padre == itemAux.Id_Padre))
                //    {
                //        listaTrabajoPadre.Add(itemAux);
                //    }
                //}



                //var listaTiposTrabajos = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa).ToList();
                itemsTiposTrabajos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var tipoTrabajos in listaobjAgregar)
                {
                    itemsTiposTrabajos.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajos.Id_TipoTrabajo), Text = tipoTrabajos.Codigo + " | " + tipoTrabajos.Descripcion });
                }
                // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaTipoTrabajos = new SelectList(itemsTiposTrabajos, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaTipoTrabajos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }
        #endregion

        //ITEMS DE TIPO DE TRABAJO
        #region
        [HttpGet]
        public ActionResult Items_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                    if (mT_TipoTrabajo != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                        var listaItemsTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(id, empresa_id.ToString()).ToList();
                        var tipo_trabajo = mT_TipoTrabajo.Descripcion;
                        ViewBag.listaItemsTipoTrabajo = listaItemsTipoTrabajo;
                        ViewBag.tipo_trabajo = tipo_trabajo;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("MantTipoTrabajo");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_ItemMantTipoTrabajo()
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ItemMantTipoTrabajo([Bind(Include = "Id_Item,Unidad,Cantidad,Estado")] MT_Items mT_Items)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            //var itemTipoTrabajo = db.MT_Items.Where(x => x.Id_TipoTrabajo == mT_Items.Id_TipoTrabajo && x.Id_Item == mT_Items.Id_Item && x.Unidad == mT_Items.Unidad);
                            var itemTipoTrabajo = db.sp_Quimipac_ConsultaMT_Items(mT_Items.Id_TipoTrabajo, empresa_id.ToString()).Where(x=> x.Unidad == mT_Items.Unidad).ToList();

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_ItemMantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Items mT_Items = db.MT_Items.Find(id);
                    bool seleccion = false;
                    if (mT_Items != null)
                    {

                        List<SelectListItem> items = new List<SelectListItem>();
                        List<SelectListItem> unidades = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_ItemMantTipoTrabajo([Bind(Include = "Id_ItemTipoTrabajo,Id_TipoTrabajo,Id_Item,Unidad,Cantidad,Estado")] MT_Items mT_Items)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        #endregion

        //ACTIVIDADES DE TIPO DE TRABAJO
        #region
        [HttpGet]
        public ActionResult Actividades_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);

                    if (mT_TipoTrabajo != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                        var listaActividades_MantTipoTrabajo = db.sp_Quimipac_ConsultaMT_Actividad(mT_TipoTrabajo.Id_TipoTrabajo, empresa_id.ToString()).ToList();
                        var tipo_trabajo = mT_TipoTrabajo.Descripcion;
                        ViewBag.listaActividades_MantTipoTrabajo = listaActividades_MantTipoTrabajo;                                                
                        ViewBag.tipo_trabajo = tipo_trabajo;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("MantTipoTrabajo");
                    }

                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Actividades_MantTipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsActividad = new List<SelectListItem>();
                    List<SelectListItem> itemsTipoActividad = new List<SelectListItem>();
                    var id_tipo_trabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"].ToString();
                    var id_trabajo = Convert.ToInt32(id_tipo_trabajo);
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    var listaActividades = db.sp_Quimipac_ConsultaMT_Actividad(id_trabajo, empresa_id).ToList();
                    foreach (var actividad in listaActividades)
                    {
                        itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Codigo });
                    }

                    itemsActividad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    SelectList selectlistaActividades = new SelectList(itemsActividad, "Value", "Text");

                    var listaTipoActividad = db.sp_Quimipac_ConsultaMT_TablaDetalle(58).ToList();
                    foreach (var tipo in listaTipoActividad)
                    {
                        itemsTipoActividad.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }

                    SelectList selectlistaTipoActividad = new SelectList(itemsTipoActividad, "Value", "Text");

                    ViewBag.listaActividades = selectlistaActividades;
                    ViewBag.listaTipoActividad = selectlistaTipoActividad;

                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Actividades_MantTipoTrabajo([Bind(Include = "Id_ActividadPadre,Codigo,Descripcion,Tiempo_Estimado,Obligatorio,Peso_Actividad,Orden,Estado, Tipo")] MT_Actividad mT_Actividad)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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



                            var tipoTrabajo = db.sp_Quimipac_ConsultaMT_Actividad(idMantTipoTrabajo, empresa_id.ToString()).ToList().Where(x=> x.Descripcion == mT_Actividad.Descripcion && x.Codigo == mT_Actividad.Codigo);
                           if (tipoTrabajo.Count() >= 1)
                            {
                                //foreach (var item in tipoTrabajo)
                                //{
                                //    if (item.Descripcion == mT_Actividad.Descripcion && item.Codigo == mT_Actividad.Codigo)
                                //    {
                                    TempData["mensaje_error"] = "Código ya existe";
                                    
                                //    }
                                //}
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Actividades_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Actividad mT_Actividad = db.MT_Actividad.Find(id);
                    bool seleccion = false;
                    if (mT_Actividad != null)
                    {
                        List<SelectListItem> itemsActividad = new List<SelectListItem>();
                        List<SelectListItem> itemsTipoActividad = new List<SelectListItem>();

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

                        var listaTipoActividad = db.sp_Quimipac_ConsultaMT_TablaDetalle(58).ToList();
                        foreach (var tipo in listaTipoActividad)
                        {
                            if (tipo.Id_TablaDetalle == mT_Actividad.Tipo)
                            {
                                seleccion = true;
                            }

                            itemsTipoActividad.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipoActividad = new SelectList(itemsTipoActividad, "Value", "Text");


                        if (mT_Actividad.Obligatorio == "N")
                        {
                            mT_Actividad.Peso_Actividad = 0;

                        }

                        ViewBag.listaActividades = selectlistaActividades;
                        ViewBag.listaTipoActividad = selectlistaTipoActividad;

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Actividades_MantTipoTrabajo([Bind(Include = "Id_Actividad,Id_TipoTrabajo,Id_ActividadPadre,Codigo,Descripcion,Tiempo_Estimado,Obligatorio,Peso_Actividad,Orden,Estado, Tipo")] MT_Actividad mT_Actividad)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        #endregion

        //FORMULARIOS DE ACTIVIDAD DE TIPO DE TRABAJO
        #region
        [HttpGet]
        public ActionResult Formularios_Actividad(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public FileResult DescargarArchivoFormulario(string nombre)
        {
            // if (System.Web.HttpContext.Current.Session["usuario"] != null){
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
            // }else {   return RedirectToAction("IniciarSesion", "Home");  }
        }
        [HttpGet]
        public ActionResult AgregarFormulario_Actividad()
        {
            //if (System.Web.HttpContext.Current.Session["usuario"] != null){
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
            // } else {   return RedirectToAction("IniciarSesion", "Home"); }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarFormulario_Actividad([Bind(Include = "Id_Actividad,Codigo,Descripcion,Enlace_Formulario,NombreArchivo,Fecha_Creacion,Estado")] MT_Formulario mT_Formulario, HttpPostedFileBase NombreArchivo)
        {
            //if (System.Web.HttpContext.Current.Session["usuario"] != null){
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
            // } else {   return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarArchivoFormulario(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        #endregion

        //MEDIDAS TIPO TRABAJO
        #region
        [HttpGet]
        public ActionResult Medidas_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);
                    if (mT_TipoTrabajo != null)
                    {
                        System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"] = mT_TipoTrabajo.Id_TipoTrabajo;
                        var listaMedidasTipoTrabajo = db.sp_Quimipac_ConsultaMt_MedidasTTrabajo(id).ToList();
                        var tipo_trabajo = mT_TipoTrabajo.Descripcion;                       
                        ViewBag.tipo_trabajo = tipo_trabajo;
                        ViewBag.listaMedidasTipoTrabajo = listaMedidasTipoTrabajo;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("MantTipoTrabajo");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Medidas_MantTipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Medidas_MantTipoTrabajo([Bind(Include = "Codigo,Descripcion,Tipo_Dato,Olbligatorio, Estado")] MT_Tipo_Trabajo_Medida mT_TTMedida)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                                mT_TTMedida.Olbligatorio = mT_TTMedida.Olbligatorio.Substring(0, 1);

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Medidas_MantTipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Medidas_MantTipoTrabajo([Bind(Include = "Id_Tipo_trabajo_Medida,Id_Tipo_Trabajo,Codigo,Descripcion,Tipo_Dato,Olbligatorio, Estado")] MT_Tipo_Trabajo_Medida mT_TipoTrabajoMedida)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        #endregion


        //MATENIMIENTO DE PRECIOS REFERENCIALES
        #region
        [HttpGet]
        public ActionResult MantPrecioReferencial()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaPreciosreferProyectos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(1, empresa_id.ToString()).ToList();
                    
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    listaPreciosreferProyectos = listaPreciosreferProyectos.Where(vq => ContratosLI.Contains(vq.Id_Proyecto_Contrato.ToString())).ToList();
                    
                    var listaPreciosreferContratos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(2, empresa_id.ToString()).ToList();
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    listaPreciosreferContratos = listaPreciosreferContratos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto_Contrato.ToString())).ToList();

                    foreach (var contrato in listaPreciosreferContratos)
                    {
                        listaPreciosreferProyectos.Add(contrato);
                    }

                    ViewBag.listaPreciosrefer = listaPreciosreferProyectos;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();

                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantPrecioReferencial()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {
                    List<SelectListItem> itemsTipos = new List<SelectListItem>();
                    List<SelectListItem> itemsContratos = new List<SelectListItem>();
                    List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                    List<SelectListItem> itemsItems = new List<SelectListItem>();
                    List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                    List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                    List<SelectListItem> items = new List<SelectListItem>();

                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

                    foreach (var tipo in listaTipos)
                    {
                        itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }

                    // itemsTipos.Insert(0, new SelectListItem() { Value = "", Text = "Seleccione Curso" });

                    SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

                    var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaContratos = listaContratos.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    listaContratos = listaContratos.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();


                    foreach (var contrato in listaContratos)
                    {
                        itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente });
                    }

                    SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaProyectos = listaProyectos.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    listaProyectos = listaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                    foreach (var proyecto in listaProyectos)
                    {
                        itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
                    }

                    SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");


                    var listaItems = db.sp_Quimipac_ConsultaMT_Items(1, empresa_id.ToString()).ToList();//por defecto q venga los del tt 1 

                    foreach (var item in listaItems)
                    {
                        itemsItems.Add(new SelectListItem { Value = Convert.ToString(item.Id_Item), Text = item.DescripcionItem });
                    }

                    SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");





                    var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

                    foreach (var moneda in listaMoneda)
                    {
                        itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre });
                    }

                    SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                    var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef(1, empresa_id.ToString()).ToList(); //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO
                    itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var tipoTrabajo in listaTiposTrabajo)
                    {
                        itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo + " | " + tipoTrabajo.Descripcion });
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
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
       
        public JsonResult GetTiposTrabajoPrecioRef(int id)
        {
            try
            {
                List<SelectListItem> itemsTiposTrabajos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                var listaTiposTrabajos = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef(id, empresa).ToList();
                itemsTiposTrabajos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var tipoTrabajos in listaTiposTrabajos)
                {
                    itemsTiposTrabajos.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajos.Id_TipoTrabajo), Text = tipoTrabajos.Codigo + " | " + tipoTrabajos.Descripcion });
                }
                // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaTipoTrabajos = new SelectList(itemsTiposTrabajos, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaTipoTrabajos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }


        public JsonResult GetItems(int id)
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                List<SelectListItem> itemsitemsTiposTrabajo = new List<SelectListItem>();
                var listaItemsTT = db.sp_Quimipac_ConsultaMT_Items(id, empresa_id.ToString()).ToList();

                foreach (var tipoTrabajo in listaItemsTT)
                {
                    itemsitemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_Item), Text = tipoTrabajo.DescripcionItem });
                }
                //itemsitemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaItemsTipoTrabajo = new SelectList(itemsitemsTiposTrabajo, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaItemsTipoTrabajo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }
        //public JsonResult GetItemsTabla(int id)
        //{
        //    try
        //    {
        //        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        //        List<SelectListItem> itemsitemsTiposTrabajo = new List<SelectListItem>();
        //        var listaItemsTT = db.sp_Quimipac_ConsultaMT_Items(id, empresa_id.ToString()).ToList();

        //        //foreach (var tipoTrabajo in listaItemsTT)
        //        //{
        //        //    itemsitemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_Item), Text = tipoTrabajo.DescripcionItem });
        //        //}

        //        //SelectList selectlistaItemsTipoTrabajo = new SelectList(itemsitemsTiposTrabajo, "Value", "Text");

        //        return Json(listaItemsTT, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(Response.StatusCode);
        //    }
        //}

        public JsonResult GetItemsTabla(int idTipoTrabajo, int IdProyContrato)//, string Unidad)
        {
            try
            {
                //List<SelectListItem> itemsitemsTiposTrabajo = new List<SelectListItem>();
                //var listaItemsTT = db.sp_Quimipac_ConsultaMT_Items(idTipoTrabajo).ToList();

                DataBase_Externo dbe = new DataBase_Externo();
                var listaItemsTT = dbe.Obtener_Id_Ingresados(idTipoTrabajo, IdProyContrato);


                //foreach (var tipoTrabajo in listaItemsTT)
                //{
                //    itemsitemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_Item), Text = tipoTrabajo.DescripcionItem });
                //}

                //SelectList selectlistaItemsTipoTrabajo = new SelectList(itemsitemsTiposTrabajo, "Value", "Text");

                return Json(listaItemsTT, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //[HttpGet]
        //public JsonResult InsertarItems(List<TempItems> valores) var user_id = System.Web.HttpContext.Current.Session["usuario"];

        //public JsonResult InsertarItems(List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        //{
        //    var user_id = System.Web.HttpContext.Current.Session["usuario"];
        //    //var mensajeDiv = new TempMensaje();
        //    var mensaje = new JsonResult();
        //    int nRegistro = 0;
        //    try
        //    {

        //        //var objectContext = ((IObjectContextAdapter)db).ObjectContext;
        //        //MT_PrecioReferencial MTPPreferencial;

        //        foreach (var item in lkItems)
        //        {
        //            //if ((item.Fecha_Inicio != null && item.Fecha_Fin != null && item.Id_Item != null && item.Precio != null && item.Costo!= null) || (!item.Fecha_Inicio.Equals("") && !item.Fecha_Fin.Equals("") && !item.Id_Item.Equals("") && !item.Precio.Equals("") && !item.Costo.Equals("")))
        //            if(item.Fecha_Inicio !=null )
        //            {
        //                //Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,
        //                //Id_TipoTrabajo,Id_Usuario,Fecha_Registro,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado

        //                DataBase_Externo dbe = new DataBase_Externo();

        //                var band = dbe.InsertarPrecioRef(ValueTipo, ValueProyCont, item.Id_Item, ValueTipoTrab, System.Web.HttpContext.Current.Session["usuario"].ToString(), Convert.ToDateTime(item.Fecha_Inicio), Convert.ToDateTime(item.Fecha_Fin), ValueMoneda, Convert.ToDecimal(item.Precio), Convert.ToDecimal(item.Costo), "A");

        //                //MTPPreferencial = new MT_PrecioReferencial()
        //                //{
        //                //    Id_PrecioReferencial =0
        //                //   ,Id_TipoTablaDetalle  = ValueTipo
        //                //   ,Id_Proyecto_Contrato = ValueProyCont
        //                //   ,Id_Item = item.Id_Item
        //                //   ,Id_TipoTrabajo = ValueTipoTrab
        //                //   ,Id_Usuario     = System.Web.HttpContext.Current.Session["usuario"].ToString()
        //                //   ,Fecha_Registro = DateTime.Now
        //                //   ,Fecha_Inicio = Convert.ToDateTime(item.Fecha_Inicio)
        //                //   ,Fecha_Fin = Convert.ToDateTime(item.Fecha_Fin)
        //                //   ,Moneda = ValueMoneda
        //                //   ,Precio = item.Precio
        //                //   ,Costo = item.Costo
        //                //   ,Estado = "A"
        //                //};            
        //                //// db.MT_PrecioReferencial.Add(MTPPreferencial);
        //                //objectContext.AddObject("MT_PrecioReferencial", MTPPreferencial);
        //                //db.SaveChanges();

        //                nRegistro++;
        //            }
        //            else { }

        //        }




        //        if (nRegistro < 1)
        //        {
        //            //mensaje.Data = new { mensaje = "Error al ingresar, favor verificar datos ingresados" }; 
        //            mensaje.Data =  "";
        //            return mensaje;
        //            // throw new ApplicationException("Error al ingresar, favor verificar datos ingresados");
        //           // throw new InvalidOperationException();
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


        public JsonResult InsertarItems(List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            //var mensajeDiv = new TempMensaje();
            var mensaje = new JsonResult();
            int nRegistro = 0;
            try
            {

                //var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                //MT_PrecioReferencial MTPPreferencial;

                foreach (var item in lkItems)
                {
                    //if ((item.Fecha_Inicio != null && item.Fecha_Fin != null && item.Id_Item != null && item.Precio != null && item.Costo!= null) || (!item.Fecha_Inicio.Equals("") && !item.Fecha_Fin.Equals("") && !item.Id_Item.Equals("") && !item.Precio.Equals("") && !item.Costo.Equals("")))
                    if (item.Fecha_Inicio != null)
                    {
                        //Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,
                        //Id_TipoTrabajo,Id_Usuario,Fecha_Registro,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado

                        DataBase_Externo dbe = new DataBase_Externo();

                        var band = dbe.InsertarPrecioRef(ValueTipo, ValueProyCont, item.Id_Item, ValueTipoTrab, System.Web.HttpContext.Current.Session["usuario"].ToString(), Convert.ToDateTime(item.Fecha_Inicio), Convert.ToDateTime(item.Fecha_Fin), ValueMoneda, Convert.ToDecimal(item.Precio), Convert.ToDecimal(item.Costo), "A", item.Unidad);

                        //MTPPreferencial = new MT_PrecioReferencial()
                        //{
                        //    Id_PrecioReferencial =0
                        //   ,Id_TipoTablaDetalle  = ValueTipo
                        //   ,Id_Proyecto_Contrato = ValueProyCont
                        //   ,Id_Item = item.Id_Item
                        //   ,Id_TipoTrabajo = ValueTipoTrab
                        //   ,Id_Usuario     = System.Web.HttpContext.Current.Session["usuario"].ToString()
                        //   ,Fecha_Registro = DateTime.Now
                        //   ,Fecha_Inicio = Convert.ToDateTime(item.Fecha_Inicio)
                        //   ,Fecha_Fin = Convert.ToDateTime(item.Fecha_Fin)
                        //   ,Moneda = ValueMoneda
                        //   ,Precio = item.Precio
                        //   ,Costo = item.Costo
                        //   ,Estado = "A"
                        //};            
                        //// db.MT_PrecioReferencial.Add(MTPPreferencial);
                        //objectContext.AddObject("MT_PrecioReferencial", MTPPreferencial);
                        //db.SaveChanges();

                        nRegistro++;
                    }
                    else { }

                }




                if (nRegistro < 1)
                {
                    //mensaje.Data = new { mensaje = "Error al ingresar, favor verificar datos ingresados" }; 
                    mensaje.Data = "";
                    return mensaje;
                    // throw new ApplicationException("Error al ingresar, favor verificar datos ingresados");
                    // throw new InvalidOperationException();
                    //  
                }
                else
                {
                    mensaje.Data = "Registros guardados correctamente";
                    return mensaje;
                }
                // mensajeDiv.Mensaje = "  Registro Guardado correctamente";
                //mensajeDiv.Ok = true;

                // objectContext.AddObject("MT_PrecioReferencial", MTPPreferencial);
                // db.SaveChanges();



                //  return Json(lkItems, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                //mensajeDiv.Ok = false;
                // return resultado;
                return Json(Response.StatusCode);
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantPrecioReferencial([Bind(Include = "Id_TipoTablaDetalle, Id_Proyecto_Contrato,Id_TipoTrabajo, Id_Item,Fecha_Inicio,Fecha_Fin, Moneda, Precio, Costo, Estado")] MT_PrecioReferencial mT_PrecioReferencial)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        //[HttpGet]
        //public ActionResult Editar_MantPrecioReferencial(int id)
        //{
        //    try
        //    {

        //        MT_PrecioReferencial mT_PrecioReferencial = db.MT_PrecioReferencial.Find(id);
        //        System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_PrecioReferencial.Fecha_Registro;
        //        bool seleccion = false;
        //        if (mT_PrecioReferencial != null)
        //        {

        //            List<SelectListItem> itemsTipos = new List<SelectListItem>();
        //            List<SelectListItem> itemsContratos = new List<SelectListItem>();
        //            List<SelectListItem> itemsProyectos = new List<SelectListItem>();
        //            List<SelectListItem> itemsItems = new List<SelectListItem>();
        //            List<SelectListItem> itemsMonedas = new List<SelectListItem>();
        //            List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
        //            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

        //            var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

        //            foreach (var tipo in listaTipos)
        //            {
        //                if (tipo.Id_TablaDetalle == mT_PrecioReferencial.Id_TipoTablaDetalle)
        //                     {
        //                            seleccion = true;
        //                     }

        //                    itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
        //            }



        //            SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

        //            var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();

        //            foreach (var contrato in listaContratos)
        //            {
        //                if (contrato.Id_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato)
        //                {
        //                    seleccion = true;
        //                }

        //                itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente, Selected = seleccion });
        //            }

        //            SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


        //            var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
        //            foreach (var proyecto in listaProyectos)
        //            {
        //                if (proyecto.Id_Proyecto == mT_PrecioReferencial.Id_Proyecto_Contrato)
        //                {
        //                    seleccion = true;
        //                }
        //                itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente, Selected = seleccion });
        //            }

        //            SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");


        //            var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();

        //            foreach (var item in listaItems)
        //            {
        //                if (item.cod_item == mT_PrecioReferencial.Id_Item)
        //                {
        //                    seleccion = true;
        //                }
        //                itemsItems.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
        //            }

        //            SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");


        //            List<SelectListItem> items = new List<SelectListItem>();


        //            var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

        //            foreach (var moneda in listaMoneda)
        //            {
        //                if (moneda.codmon == mT_PrecioReferencial.Moneda)
        //                {
        //                    seleccion = true;
        //                }

        //                itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
        //            }

        //            SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

        //            var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1,empresa_id.ToString()).ToList(); //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO

        //            foreach (var tipoTrabajo in listaTiposTrabajo)
        //            {
        //                if (tipoTrabajo.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo)
        //                {
        //                    seleccion = true;
        //                }

        //                itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo, Selected = seleccion });
        //            }

        //            SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

        //            ViewBag.listaContratos = selectlistaContratos;
        //            ViewBag.listaProyectos = selectlistaProyectos;
        //            ViewBag.listaItems = selectlistaItems;
        //            ViewBag.listaMoneda = selectlistaMoneda;
        //            ViewBag.listaTipos = selectlistaTipos;
        //            ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;


        //            return View(mT_PrecioReferencial);

        //        }
        //        else
        //        {
        //            TempData["mensaje_error"] = "Código no existe";
        //            return RedirectToAction("MantPrecioReferencial");
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }



        //}

        //[ValidateAntiForgeryToken] 
        //[HttpPost]
        //public ActionResult Editar_MantPrecioReferencial([Bind(Include = "Id_PrecioReferencial,Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,Id_TipoTrabajo,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado")] MT_PrecioReferencial mT_PrecioReferencial)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //           // int bandera = 0;
        //            var user_id = System.Web.HttpContext.Current.Session["usuario"];

        //            if (user_id == null)
        //            {
        //                return RedirectToAction("IniciarSesion", "Home");
        //            }
        //            else
        //            {
        //                //var listaPreciosreferProyectos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(1).ToList();
        //                //var listaPreciosreferContratos = db.sp_Quimipac_ConsultaMT_PrecioReferencial(2).ToList();

        //                //foreach (var contrato in listaPreciosreferContratos)
        //                //{
        //                //    listaPreciosreferProyectos.Add(contrato);
        //                //}

        //                //foreach (var objt in listaPreciosreferProyectos)
        //                //{
        //                //    if(objt.Id_Proyecto_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato && objt.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo && objt.Id_Item == mT_PrecioReferencial.Id_Item)
        //                //    {
        //                //        bandera = 1;
        //                //    }
        //                //}

        //                //if(bandera != 1)
        //                //{

        //                    mT_PrecioReferencial.Id_Usuario = user_id.ToString();
        //                mT_PrecioReferencial.Fecha_Registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);


        //                    mT_PrecioReferencial.Estado = mT_PrecioReferencial.Estado.Substring(0, 1);

        //                    db.Entry(mT_PrecioReferencial).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                    TempData["mensaje_actualizado"] = "Precio Referencial actualizado";

        //                    return RedirectToAction("MantPrecioReferencial");
        //                //}
        //                //else
        //                //{
        //                //    TempData["mensaje_error"] = "Registro ya existe";
        //                //    return RedirectToAction("MantPrecioReferencial");
        //                //}
        //            }

        //        }
        //        else
        //        {
        //            TempData["mensaje_error"] = "Valores incorrectos";
        //            return RedirectToAction("MantPrecioReferencial");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }
        //}


        [HttpGet]
        public ActionResult Editar_MantPrecioReferencial(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {

                    MT_PrecioReferencial mT_PrecioReferencial = db.MT_PrecioReferencial.Find(id);
                    System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_PrecioReferencial.Fecha_Registro;
                    bool seleccion = false;
                    if (mT_PrecioReferencial != null)
                    {

                        List<SelectListItem> itemsTipos = new List<SelectListItem>();
                        List<SelectListItem> itemsContratos = new List<SelectListItem>();
                        List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                        List<SelectListItem> itemsItems = new List<SelectListItem>();
                        List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                        List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

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

                        var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id).ToList();
                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratos = listaContratos.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratos = listaContratos.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                        foreach (var contrato in listaContratos)
                        {
                            if (contrato.Id_Contrato == mT_PrecioReferencial.Id_Proyecto_Contrato)
                            {
                                seleccion = true;
                            }

                            itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente, Selected = seleccion });
                        }

                        SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                        var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProyectos = listaProyectos.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                        var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                        listaProyectos = listaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                        foreach (var proyecto in listaProyectos)
                        {
                            if (proyecto.Id_Proyecto == mT_PrecioReferencial.Id_Proyecto_Contrato)
                            {
                                seleccion = true;
                            }
                            itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente, Selected = seleccion });
                        }

                        SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");
                        /*

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


                        //.....................*/


                        List<SelectListItem> items = new List<SelectListItem>();


                        var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id).ToList();

                        foreach (var moneda in listaMoneda)
                        {
                            if (moneda.codmon == mT_PrecioReferencial.Moneda)
                            {
                                seleccion = true;
                            }

                            itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                        var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef(1, empresa_id).ToList(); //POR DEFECTO QUE MUESTRE LOS TIPOS DE TRABAJO DE PROYECTO

                        foreach (var tipoTrabajo in listaTiposTrabajo)
                        {
                            if (tipoTrabajo.Id_TipoTrabajo == mT_PrecioReferencial.Id_TipoTrabajo)
                            {
                                seleccion = true;
                            }

                            itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo, Selected = seleccion });
                        }

                        SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");




                        var lkItems = db.sp_Quimipac_ConsultaMT_PrecioReferencial_Items(id, empresa_id).ToList();

                        ViewBag.listaContratos = selectlistaContratos;
                        ViewBag.listaProyectos = selectlistaProyectos;
                        //ViewBag.listaItems = selectlistaItems;
                        ViewBag.listaItems = lkItems;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }



        }


        //***********************
        
        public JsonResult EditarPreReferencial(List<sp_Quimipac_ConsultaMT_PrecioReferencial_Items_Result> lkItems, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var mensaje = new JsonResult();
            int nRegistro = 0;
            try
            {
                foreach (var item in lkItems)
                {
                    if (item.Fecha_Inicio != null)
                    {
                        DataBase_Externo dbe = new DataBase_Externo();
                        var band = dbe.ActualizarPreRef(item.Id_PrecioReferencial, System.Web.HttpContext.Current.Session["usuario"].ToString(), Convert.ToDateTime(item.Fecha_Inicio), Convert.ToDateTime(item.Fecha_Fin), Convert.ToDecimal(item.Precio), Convert.ToDecimal(item.Costo), item.Estado, ValueMoneda);

                        nRegistro++;
                    }
                    else { }
                }
                if (nRegistro < 1)
                {
                    mensaje.Data = "Error al actualizar, favor verificar datos ingresados!";
                    //return mensaje;
                }
                else { mensaje.Data = "Registros guardados correctamente"; }

                return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        //***********************
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantPrecioReferencial([Bind(Include = "Id_PrecioReferencial,Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,Id_TipoTrabajo,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado")] MT_PrecioReferencial mT_PrecioReferencial)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            mT_PrecioReferencial.Fecha_Registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);


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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [HttpGet]
        public ActionResult EliminarPrecioReferencial(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }
        #endregion


        //MATENIMIENTO DE LUGARES DE MEDICIÓN
        #region
        public ActionResult MantLugarMedicion()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaLugarMedicion = db.sp_Quimipac_ConsultaMT_Estacion(empresa_id.ToString()).ToList();
                    ViewBag.listaLugarMedicion = listaLugarMedicion;
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantLugarMedicion()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {
                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsProvincias = new List<SelectListItem>();
                    List<SelectListItem> itemsCiudades = new List<SelectListItem>();
                    List<SelectListItem> itemsMacrosector = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                    
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


                    var listaMacrosector = db.sp_Quimipac_ConsultaMT_Sector(empresa_id.ToString()).ToList();
                    foreach (var macrosector in listaMacrosector)
                    {
                        itemsMacrosector.Add(new SelectListItem
                        {
                            Value = Convert.ToString(macrosector.Id_Sector),
                            Text = macrosector.Nombre
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantLugarMedicion([Bind(Include = "Id_Cliente,Id_Provincia, Id_Ciudad,Id_Macrosector,Nombre, Direccion, Estado, Coordenada_X, Coordenada_Y")] MT_Estacion mT_Estacion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var lugarMedicion = db.MT_Estacion.Where(x => x.Coordenada_X == mT_Estacion.Coordenada_X && x.Coordenada_Y == mT_Estacion.Coordenada_Y);

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantLugarMedicion(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
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

                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                        

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

                        var listaMacrosector = db.sp_Quimipac_ConsultaMT_Sector(empresa_id.ToString()).ToList();
                        foreach (var macrosector in listaMacrosector)
                        {
                            if (macrosector.Id_Sector == mT_LugarMedicion.Id_Macrosector)
                            {
                                seleccion = true;
                            }

                            itemsMacroSector.Add(new SelectListItem { Value = Convert.ToString(macrosector.Id_Sector), Text = macrosector.Nombre, Selected = seleccion });
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantLugarMedicion([Bind(Include = "Id_Estacion, Id_Cliente,Id_Provincia, Id_Ciudad, Id_Macrosector,Nombre,Direccion,Coordenada_X, Coordenada_Y, Estado")] MT_Estacion mT_LugarMedicion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarLugaresdeMedicion(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

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
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoTrabajo(empresa_id.ToString()).ToList();
                    ViewBag.listaGrupoTrabajo = listaGrupoTrabajo;
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantGrupoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                    List<SelectListItem> itemsTipoGrupo = new List<SelectListItem>();
                    List<SelectListItem> itemsBodega = new List<SelectListItem>();
                    List<SelectListItem> itemsVehiculo = new List<SelectListItem>();

                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaCampamentos = db.sp_Quimipac_ConsultaMT_Campamento(empresa_id.ToString()).ToList();
                    foreach (var campamento in listaCampamentos)
                    {
                        itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre });
                    }

                    SelectList selectlistaCampamento = new SelectList(itemsCampamento, "Value", "Text");


                    var listaBodegas = db.sp_LINK_ConsultaBodegas(empresa_id.ToString()).ToList();
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

                    var listaVehiculos = db.sp_LINK_ConsultaVehiculos().ToList();
                    foreach (var vehiculos in listaVehiculos)
                    {
                        itemsVehiculo.Add(new SelectListItem { Value = Convert.ToString(vehiculos.COD_VEHICULO), Text = vehiculos.DESCRIPCION });
                    }

                    SelectList selectlistaVehiculos = new SelectList(itemsVehiculo, "Value", "Text");

                    ViewBag.listaCampamentos = selectlistaCampamento;
                    ViewBag.listaBodegas = selectlistaBodegas;
                    ViewBag.listaTipoGrupoTrabajo = selectlistaTipoGrupoTrabajo;
                    ViewBag.listaVehiculos = selectlistaVehiculos;


                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantGrupoTrabajo([Bind(Include = "Id_Campamento,Nombre,Tipo,Estado,Bodega,Id_Vehiculo")] MT_GrupoTrabajo mT_GrupoTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                        var grupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoTrabajo(empresa_id.ToString()).ToList().Where(x => x.Id_Campamento == mT_GrupoTrabajo.Id_Campamento && x.Nombre == mT_GrupoTrabajo.Nombre && x.Tipo == mT_GrupoTrabajo.Tipo && x.Bodega == mT_GrupoTrabajo.Bodega);
                        //var grupoTrabajo = db.MT_GrupoTrabajo.Where(x => x.Id_Campamento == mT_GrupoTrabajo.Id_Campamento && x.Nombre == mT_GrupoTrabajo.Nombre && x.Tipo == mT_GrupoTrabajo.Tipo && x.Bodega == mT_GrupoTrabajo.Bodega);
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                        List<SelectListItem> itemsVehiculo = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaCampamento = db.sp_Quimipac_ConsultaMT_Campamento(empresa_id.ToString()).ToList();
                        foreach (var campamento in listaCampamento)
                        {
                            if (campamento.Id_Campamento == mT_GrupoTrabajo.Id_Campamento)
                            {
                                seleccion = true;
                            }

                            itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre, Selected = seleccion });
                        }

                        SelectList selectlistaCampamentos = new SelectList(itemsCampamento, "Value", "Text");


                        var listaBodegas = db.sp_LINK_ConsultaBodegas(empresa_id.ToString()).ToList();

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

                        var listaVehiculos = db.sp_LINK_ConsultaVehiculos().ToList();

                        foreach (var vehiculos in listaVehiculos)
                        {
                            if (vehiculos.COD_VEHICULO == mT_GrupoTrabajo.Id_Vehiculo)
                            {
                                seleccion = true;
                            }

                            itemsVehiculo.Add(new SelectListItem { Value = Convert.ToString(vehiculos.COD_VEHICULO), Text = vehiculos.DESCRIPCION, Selected = seleccion });
                        }

                        SelectList selectlistaVehiculos = new SelectList(itemsVehiculo, "Value", "Text");

                        ViewBag.listaCampamento = selectlistaCampamentos;
                        ViewBag.listaBodegas = selectlistaBodegas;
                        ViewBag.listaTipoGrupoTrabajo = selectlistaTipoGrupoTrabajo;
                        ViewBag.listaVehiculos = selectlistaVehiculos;
                        ViewBag.id_Vehiculo_Editar = id;


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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajo, Id_Campamento,Nombre,Tipo,Estado,Bodega,Id_Vehiculo")] MT_GrupoTrabajo mT_GrupoTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        public JsonResult GetVehiculo(int id)
        {
            try
            {
                List<SelectListItem> itemsVehiculos = new List<SelectListItem>();
                //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                //string empresa = empresa_id.ToString();
                var nombre_campamento = db.MT_Campamento.Where(x => x.Id_Campamento == id).Select(z => z.Nombre).FirstOrDefault();
                var cod_campamento_sysbase = db.sp_LINK_ConsultaCampamento().Where(x => x.DESCRIPCION == nombre_campamento).Select(z => z.COD_CAMPAMENTO).FirstOrDefault();
                
                var listaVehiculos = db.sp_LINK_ConsultaVehiculos().Where(x => x.COD_CAMPAMENTO == cod_campamento_sysbase).ToList();

                itemsVehiculos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var vehiculos in listaVehiculos)
                {
                    itemsVehiculos.Add(new SelectListItem { Value = Convert.ToString(vehiculos.COD_VEHICULO), Text = vehiculos.DESCRIPCION });
                }
                SelectList selectlistaVehiculos = new SelectList(itemsVehiculos, "Value", "Text");

                return Json(selectlistaVehiculos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        

        //get editar contrato y default
        public JsonResult GetVehiculoDefauledit(int? id_Campamento, int? id_grupo_Editar)
        {
            try
            {
                List<SelectListItem> itemsVehiculo = new List<SelectListItem>();
                //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                //string empresa = empresa_id.ToString();
                var nombre_campamento = db.MT_Campamento.Where(x => x.Id_Campamento == id_Campamento).Select(z => z.Nombre).FirstOrDefault();
                var cod_campamento_sysbase = db.sp_LINK_ConsultaCampamento().Where(x => x.DESCRIPCION == nombre_campamento).Select(z => z.COD_CAMPAMENTO).FirstOrDefault();

                var contratoVehiculo = db.sp_LINK_ConsultaVehiculos().Where(x =>  x.COD_CAMPAMENTO == cod_campamento_sysbase).Select(u => new { u.COD_CAMPAMENTO, u.COD_VEHICULO, u.DESCRIPCION }).ToList();


                return Json(contratoVehiculo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        #endregion



        //EQUIPOS DE GRUPO DE TRABAJO
        #region
        [HttpGet]
        public ActionResult Equipos_MantGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);
                    if (mT_GrupoTrabajo != null)
                    {
                        System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"] = mT_GrupoTrabajo.Id_GrupoTrabajo;
                        var listaEquiposGrupoTrabajo = db.sp_Quimipac_ConsultaMT_EquipoGrupoTrabajo(id).ToList();
                        var grupo_trabajo = mT_GrupoTrabajo.Nombre;
                        ViewBag.listaEquiposGrupoTrabajo = listaEquiposGrupoTrabajo;
                        ViewBag.grupo_trabajo = grupo_trabajo;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_EquipoMantGrupoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_EquipoMantGrupoTrabajo([Bind(Include = "Id_Equipo, Estado")] MT_GrupoTrabajoEquipo mT_GrupoTrabajoEquipo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_EquipoMantGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_EquipoMantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajoEquipo,Id_GrupoTrabajo,Id_Equipo,Estado")] MT_GrupoTrabajoEquipo mT_GrupoTrabajoEquipo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        #endregion



        //INTEGRANTES DE GRUPO DE TRABAJO

        #region
        [HttpGet]
        public ActionResult Integrantes_MantGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_GrupoTrabajo mT_GrupoTrabajo = db.MT_GrupoTrabajo.Find(id);
                    if (mT_GrupoTrabajo != null)
                    {
                        System.Web.HttpContext.Current.Session["id_MantGrupoTrabajo"] = mT_GrupoTrabajo.Id_GrupoTrabajo;
                        var listaIntegrantesGrupoTrabajo = db.sp_Quimipac_ConsultaMT_IntegranteGrupoTrabajo(id).ToList();
                        var grupo_trabajo = mT_GrupoTrabajo.Nombre;
                        ViewBag.listaIntegrantesGrupoTrabajo = listaIntegrantesGrupoTrabajo;
                        ViewBag.grupo_trabajo = grupo_trabajo;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_IntegranteMantGrupoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_IntegranteMantGrupoTrabajo([Bind(Include = "Id_Persona, Rol_Usuario, Estado")] MT_GrupoTrabajoIntegrante mT_GrupoTrabajoIntegrante)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_IntegranteMantGrupoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_IntegranteMantGrupoTrabajo([Bind(Include = "Id_GrupoTrabajoIntegrante,Id_GrupoTrabajo,Id_Persona, Rol_Usuario, Estado")] MT_GrupoTrabajoIntegrante mT_GrupoTrabajoIntegrante)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        #endregion


        //MATENIMIENTO EQUIPO TRABAJO

        #region
        public ActionResult MantEquipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        [HttpGet]
        public ActionResult Agregar_MantEquipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantEquipoTrabajo([Bind(Include = "Id_Proveedor, Id_Persona_asignada,Tipo,Codigo,Descripcion, Numero_Serie,Marca,Modelo,Fecha, Estado,Horas_Usadas")] MT_Equipo mT_EquipoTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                        return RedirectToAction("MantEquipoTrabajo");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantEquipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Equipo mT_EquipoTrabajo = db.MT_Equipo.Find(id);
                    System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_EquipoTrabajo.Fecha;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantEquipoTrabajo([Bind(Include = "Id_Equipo, Id_Proveedor, Id_Persona_asignada,Tipo,Codigo,Descripcion, Numero_Serie,Marca,Modelo, Fecha, Estado,Horas_Usadas")] MT_Equipo mT_EquipoTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            //mT_EquipoTrabajo.Fecha = DateTime.Now;

                            mT_EquipoTrabajo.Fecha = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarMantEquipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }
        #endregion

        //EventoEquipo
        #region

        [HttpGet]
        public ActionResult EventoMantEquipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Equipo mT_Equipo = db.MT_Equipo.Find(id);
                    if (mT_Equipo != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Equipo"] = mT_Equipo.Id_Equipo;
                        var listaEventoEquipo = db.sp_Quimipac_ConsultaMt_EventoEquipo(id, empresa_id.ToString()).ToList();
                        var equipo = mT_Equipo.Descripcion;
                        ViewBag.listaEventoEquipo = listaEventoEquipo;
                        ViewBag.equipo = equipo;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("MantEquipoTrabajo");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_EventoMantEquipoTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemstipo = new List<SelectListItem>();
                    List<SelectListItem> itemspersonadestino = new List<SelectListItem>();
                    List<SelectListItem> itemspersonaorigen = new List<SelectListItem>();


                    var listaTipoEventosEquipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(54).ToList();
                    foreach (var evento in listaTipoEventosEquipo)
                    {
                        itemstipo.Add(new SelectListItem { Value = Convert.ToString(evento.Id_TablaDetalle), Text = evento.Descripcion });
                    }

                    SelectList selectlistaTipoEventos = new SelectList(itemstipo, "Value", "Text");

                    var listaPersonasdestino = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var personadestino in listaPersonasdestino)
                    {
                        itemspersonadestino.Add(new SelectListItem { Value = Convert.ToString(personadestino.id_persona), Text = personadestino.Nombres_Completos });
                    }

                    SelectList selectlistaPersonaDestino = new SelectList(itemspersonadestino, "Value", "Text");

                    var listaPersonasorigen = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var personaorigen in listaPersonasorigen)
                    {
                        itemspersonaorigen.Add(new SelectListItem { Value = Convert.ToString(personaorigen.id_persona), Text = personaorigen.Nombres_Completos });
                    }

                    SelectList selectlistaPersonaOrigen = new SelectList(itemspersonaorigen, "Value", "Text");


                    ViewBag.listaTipoEventosEquipo = selectlistaTipoEventos;
                    ViewBag.listaPersonasdestino = selectlistaPersonaDestino;
                    ViewBag.listaPersonasorigen = selectlistaPersonaOrigen;


                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_EventoMantEquipoTrabajo([Bind(Include = "Id_Equipo,Id_Usuario,Tipo_Evento,Fecha_Ini,Fecha_Fin,Proveedor,Valor,Comentario,Id_Persona_Origen,Id_Persona_Destino")] MT_Equipo_Evento mT_EventoEquipo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var id_Equipo = System.Web.HttpContext.Current.Session["id_Equipo"];

                    if (id_Equipo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idEquipo = int.Parse(id_Equipo.ToString());

                        if (ModelState.IsValid)
                        {

                            mT_EventoEquipo.Id_Equipo = idEquipo;
                            var eventoEquipo = db.MT_Equipo_Evento.Where(x => x.Id_Equipo == mT_EventoEquipo.Id_Equipo && x.Tipo_Evento == mT_EventoEquipo.Tipo_Evento && x.Proveedor == mT_EventoEquipo.Proveedor && x.Fecha_Ini == mT_EventoEquipo.Fecha_Ini && x.Fecha_Fin == mT_EventoEquipo.Fecha_Fin && x.Valor == mT_EventoEquipo.Valor);
                            if (eventoEquipo.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Evento Equipo ya existe";
                                return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });
                            }
                            else
                            {
                                mT_EventoEquipo.Id_Usuario = user_id.ToString();
                                mT_EventoEquipo.Id_Equipo = idEquipo;
                                //mT_EventoEquipo.Estado = mT_Items.Estado.Substring(0, 1);


                                db.MT_Equipo_Evento.Add(mT_EventoEquipo);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Evento Equipo guardado";
                                return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });
                        }
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_EventoMantEquipoTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Equipo_Evento mT_EventoEquipo = db.MT_Equipo_Evento.Find(id);
                    bool seleccion = false;
                    if (mT_EventoEquipo != null)
                    {

                        List<SelectListItem> itemstipo = new List<SelectListItem>();
                        List<SelectListItem> itemspersonadestino = new List<SelectListItem>();
                        List<SelectListItem> itemspersonaorigen = new List<SelectListItem>();


                        var listaTipoEventosEquipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(54).ToList();
                        foreach (var eventoequipo in listaTipoEventosEquipo)
                        {
                            if (mT_EventoEquipo.Tipo_Evento == eventoequipo.Id_TablaDetalle)
                            {
                                seleccion = true;
                            }

                            itemstipo.Add(new SelectListItem { Value = Convert.ToString(eventoequipo.Id_TablaDetalle), Text = eventoequipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEventoEquipo = new SelectList(itemstipo, "Value", "Text");

                        var listaPersonasdestino = db.sp_LINK_ConsultaPersonas().ToList();
                        foreach (var personadestino in listaPersonasdestino)
                        {
                            if (mT_EventoEquipo.Id_Persona_Destino == personadestino.id_persona)
                            {
                                seleccion = true;
                            }

                            itemspersonadestino.Add(new SelectListItem { Value = Convert.ToString(personadestino.id_persona), Text = personadestino.Nombres_Completos, Selected = seleccion });

                        }

                        SelectList selectlistaPersonaDestino = new SelectList(itemspersonadestino, "Value", "Text");


                        var listaPersonasorigen = db.sp_LINK_ConsultaPersonas().ToList();
                        foreach (var personaorigen in listaPersonasorigen)
                        {
                            if (mT_EventoEquipo.Id_Persona_Origen == personaorigen.id_persona)
                            {
                                seleccion = true;
                            }

                            itemspersonaorigen.Add(new SelectListItem { Value = Convert.ToString(personaorigen.id_persona), Text = personaorigen.Nombres_Completos, Selected = seleccion });

                        }

                        SelectList selectlistaPersonaOrigen = new SelectList(itemspersonaorigen, "Value", "Text");


                        ViewBag.listaTipoEventosEquipo = selectlistaEventoEquipo;
                        ViewBag.listaPersonasdestino = selectlistaPersonaDestino;
                        ViewBag.listaPersonasorigen = selectlistaPersonaOrigen;

                        return View(mT_EventoEquipo);

                    }
                    else
                    {
                        var id_Equipo = System.Web.HttpContext.Current.Session["id_Equipo"];

                        if (id_Equipo == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idEquipo = int.Parse(id_Equipo.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });
                        }
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_EventoMantEquipoTrabajo([Bind(Include = "Id_Equipo_Evento,Id_Equipo,Id_Usuario,Tipo_Evento,Fecha_Ini,Fecha_Fin,Proveedor,Valor,Comentario,Id_Persona_Origen,Id_Persona_Destino")] MT_Equipo_Evento mT_EquipoEvento)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var id_Equipo = System.Web.HttpContext.Current.Session["id_Equipo"];
                    // int bandera = 0;
                    if (id_Equipo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idEquipo = int.Parse(id_Equipo.ToString());

                        if (ModelState.IsValid)
                        {

                            mT_EquipoEvento.Id_Equipo = idEquipo;
                            //mT_EquipoEvento.Estado = mT_Items.Estado.Substring(0, 1);
                            mT_EquipoEvento.Id_Usuario = user_id.ToString();

                            db.Entry(mT_EquipoEvento).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Evento Equipo actualizado";

                            return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("EventoMantEquipoTrabajo", new { id = idEquipo });
                        }
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        #endregion


        //DATOS PERSONALES

        #region
        [HttpGet]
        public ActionResult DatosPersonales()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        #endregion

        //MANT SECTOR
        #region

        [HttpGet]
        public ActionResult MantSector()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();
                    var listaSector = db.sp_Quimipac_ConsultaMT_SectorGeneral(empresacod).ToList();
                    ViewBag.listaSector = listaSector;
                    return View();

                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantSector()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    List<SelectListItem> itemsSector = new List<SelectListItem>();

                    var listaSector = db.MT_Sector.Where(x => x.Estado != "E" && x.Id_Empresa == empresa_id).ToList();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantSector([Bind(Include = "Id_Padre_Sector,Nombre,Estado")] MT_Sector mT_Sector)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    if (ModelState.IsValid)
                    {
                        var sector = db.MT_Sector.Where(x => x.Nombre == mT_Sector.Nombre && x.Id_Empresa == empresa_id && x.Id_Padre_Sector == mT_Sector.Id_Padre_Sector);
                        if (sector.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Sector ya existe";
                            return RedirectToAction("MantSector");
                        }
                        else
                        {
                            //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Sector.Id_Empresa = empresa_id;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantSector(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    MT_Sector mT_Sector = db.MT_Sector.Find(id);
                    bool seleccion = false;
                    if (mT_Sector != null)
                    {
                        List<SelectListItem> itemsSector = new List<SelectListItem>();

                        var listaSector = db.MT_Sector.Where(x => x.Estado != "E" && x.Id_Empresa == empresa_id).ToList();

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantSector([Bind(Include = "Id_Sector, Id_Padre_Sector,Nombre,Estado")] MT_Sector mT_Sector)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            mT_Sector.Id_Empresa = empresa_id.ToString();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarMantSector(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        #endregion


        //CAMPAMENTO

        #region
        public ActionResult MantCampamento()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();
                    var listaCampamento = db.sp_Quimipac_ConsultaMT_CampamentoGeneral(empresacod).ToList();
                    ViewBag.listaCampamento = listaCampamento;
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantCampamento()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantCampamento([Bind(Include = "Nombre, Estado")] MT_Campamento mT_Campamento)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var campamento = db.MT_Campamento.Where(x => x.Nombre == mT_Campamento.Nombre && x.Id_Empresa == mT_Campamento.Id_Empresa);

                        if (campamento.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("MantCampamento");
                        }
                        else
                        {
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Campamento.Id_Empresa = empresa_id.ToString();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantCampamento(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantCampamento([Bind(Include = "Id_Campamento,Nombre, Estado")] MT_Campamento mT_Campamento)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Campamento.Id_Empresa = empresa_id.ToString();



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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarMantCampamento(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }



        #endregion


        //SERVICIO 

        #region

        [HttpGet]
        public ActionResult MantServicios()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();
                    var listaServicio = db.sp_Quimipac_ConsultaMT_ServicioGeneral(empresacod).ToList();
                    ViewBag.listaServicio = listaServicio;
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [HttpGet]
        public ActionResult Agregar_MantServicios()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_MantServicios([Bind(Include = "Codigo,Descripcion, Estado")] MT_Servicio mT_Servicio)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        var Servicios = db.MT_Servicio.Where(x => x.Codigo == mT_Servicio.Codigo && x.Id_Empresa == mT_Servicio.Id_Empresa);
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
                                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                mT_Servicio.Id_Empresa = empresa_id.ToString();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantServicios(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantServicios([Bind(Include = "Id_Servicio,Codigo,Descripcion,Estado")] MT_Servicio mT_Servicio)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Servicio.Id_Empresa = empresa_id.ToString();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }



        [HttpGet]
        public ActionResult EliminarServicio(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }



        #endregion

        //MATENIMIENTO DE TABLAS
        #region
        [HttpGet]
        public ActionResult MantTabla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_MantTabla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult Agregar_MantTabla([Bind(Include = "Tabla,Descripcion,Descripcion,Estado")] MT_Tablas mT_Tablas)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                                mT_Tablas.Tabla = mT_Tablas.Tabla.ToUpper();

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_MantTabla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_MantTabla([Bind(Include = "Id_Tabla,Tabla,Descripcion,Descripcion,Estado")] MT_Tablas mT_Tablas)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarMantTabla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        #endregion

        //Tabla Detalle
        #region

        [HttpGet]
        public ActionResult Detalle_MantTabla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Tablas mT_Tabla = db.MT_Tablas.Find(id);

                    if (mT_Tabla != null)
                    {
                        System.Web.HttpContext.Current.Session["id_Tabla"] = mT_Tabla.Id_Tabla;
                        var listaTablaDetalle = db.sp_Quimipac_ConsultaMT_TablaDetalleGeneral(mT_Tabla.Id_Tabla).ToList();
                        var tabla = mT_Tabla.Descripcion;
                        ViewBag.listaTablaDetalle = listaTablaDetalle;
                        ViewBag.tabla = tabla;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Detalle_MantTabla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Detalle_MantTabla([Bind(Include = "Id_Padre,Codigo,Descripcion, Estado")] MT_TablaDetalle mT_Detalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    //var id_MantTabla = System.Web.HttpContext.Current.Session["id_MantTabla"];
                    var id_MantTabla = System.Web.HttpContext.Current.Session["id_Tabla"];

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Detalle_MantTabla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                        //var id_MantTabla = System.Web.HttpContext.Current.Session["id_MantTabla"];
                        var id_MantTabla = System.Web.HttpContext.Current.Session["id_Tabla"];

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult Editar_Detalle_MantTabla([Bind(Include = "Id_TablaDetalle,Id_Tabla,Id_Padre,Codigo,Descripcion, Estado")] MT_TablaDetalle mT_Detalles)
        //{
        //    if (System.Web.HttpContext.Current.Session["usuario"] != null)
        //    {
        //        try
        //        {
        //            //int bandera = 0;
        //            if (ModelState.IsValid)
        //            {


        //                MT_TablaDetalle mT_Detalle_Editar = db.MT_TablaDetalle.Find(mT_Detalles.Id_TablaDetalle);

        //                var codigo_Editar = mT_Detalle_Editar.Codigo;
        //                if (codigo_Editar != mT_Detalles.Codigo)

        //                {
        //                    TempData["mensaje_error"] = "Código es incorrecto";
        //                    return RedirectToAction("Detalle_MantTabla", new { id = mT_Detalles.Id_Tabla });
        //                }
        //                else
        //                {



        //                    mT_Detalles.Estado = mT_Detalles.Estado.Substring(0, 1);

        //                    db.Entry(mT_Detalles).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                    TempData["mensaje_actualizado"] = "Registro Detalle Tabla actualizado";

        //                    return RedirectToAction("Detalle_MantTabla", new { id = mT_Detalles.Id_Tabla });
        //                }

        //            }
        //            else
        //            {
        //                TempData["mensaje_error"] = "Valores incorrectos";
        //                return RedirectToAction("Detalle_MantTabla", new { id = mT_Detalles.Id_Tabla });
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            return RedirectToAction("Error", "Errores");
        //        }
        //    }
        //    else { return RedirectToAction("IniciarSesion", "Home"); }
        //}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Detalle_MantTabla([Bind(Include = "Id_TablaDetalle,Id_Tabla,Id_Padre,Codigo,Descripcion, Estado")] MT_TablaDetalle mT_Detalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var usuario = System.Web.HttpContext.Current.Session["usuario"];
                    // int bandera = 0;
                    if (usuario == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {

                        var idTABLADETALLE = mT_Detalle.Id_Tabla;

                        if (ModelState.IsValid)
                        {

                            mT_Detalle.Estado = mT_Detalle.Estado.Substring(0, 1);


                            db.Entry(mT_Detalle).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Detalle de tabla actualizado";

                            return RedirectToAction("Detalle_MantTabla", new { id = idTABLADETALLE });
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
                            return RedirectToAction("Detalle_MantTabla", new { id = idTABLADETALLE });
                        }
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        #endregion

        //SOLICITUD MATERIALES
        #region

        [HttpGet]
        public ActionResult Solicitud()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaSolicitud = db.sp_Quimipac_ConsultaMT_Solicitud(user_id.ToString(), empresa_id.ToString()).ToList();
                    ViewBag.listaSolicitud = listaSolicitud;
                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Solicitud()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                    foreach (var sucursal in listaSucursal)
                    {
                        itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre });
                    }

                    SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");

                    ViewBag.listaSucursal = selectlistaSucursal;

                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Solicitud([Bind(Include = "semana, dia,codpro,estado, hora, observacion, usuario, fecha_pro_comp, observ_compras, codcen, dias_prov, fecha_disp, observ_disp, observ_pago, usuario_compra, cod_suc, categoria, elemento, num_ped_cotiz, usuario_aprobador")] InsertarSolicitud mT_Solicitud)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        //var solicitud = db.MT_Solicitud.Where(x => x.Id_OrdenTrabajo == mT_Solicitud.Id_OrdenTrabajo && x.BodegaRetirar == mT_Solicitud.BodegaRetirar && x.tipo == mT_Solicitud.tipo && x.fecha == mT_Solicitud.fecha);
                        var solicitud = db.sp_Quimipac_ConsultaMT_Solicitud("", "").Where(x => x.dia == mT_Solicitud.dia && x.estado == mT_Solicitud.estado && x.cod_suc == mT_Solicitud.cod_suc && x.semana == mT_Solicitud.semana && x.cia_codigo == mT_Solicitud.cia_codigo);

                        if (solicitud.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Solicitud ya existe";
                            return RedirectToAction("Solicitud");
                        }
                        else
                        {
                            //mT_Solicitud.Estado = mT_Solicitud.Estado.Substring(0, 1);
                            var user_id = System.Web.HttpContext.Current.Session["usuario"];
                            mT_Solicitud.usuario = user_id.ToString();
                            var empresa = System.Web.HttpContext.Current.Session["empresa"];
                            mT_Solicitud.cia_codigo = empresa.ToString();


                            //db.MT_Solicitud.Add(mT_Solicitud);
                            //db.SaveChanges();

                            var dbe = new DataBase_Externo();


                            dbe.InsertarSolicitudWeb(mT_Solicitud.dia, mT_Solicitud.codpro, mT_Solicitud.estado, mT_Solicitud.hora, mT_Solicitud.observacion, mT_Solicitud.usuario, mT_Solicitud.fecha_pro_comp, mT_Solicitud.observ_compras, mT_Solicitud.codcen, mT_Solicitud.dias_prov, mT_Solicitud.fecha_disp, mT_Solicitud.observ_disp, mT_Solicitud.observ_pago, mT_Solicitud.usuario_compra, mT_Solicitud.cod_suc, mT_Solicitud.categoria, mT_Solicitud.elemento, mT_Solicitud.num_ped_cotiz, mT_Solicitud.usuario_aprobador, mT_Solicitud.cia_codigo);

                            TempData["mensaje_correcto"] = "Solicitud guardada";
                            return RedirectToAction("Solicitud");

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Solicitud");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Solicitud(string id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    // var solicitud = db.sp_Quimipac_ConsultaMT_Solicitud().Where(x => x.dia == mT_Solicitud.dia && x.estado == mT_Solicitud.estado && x.cod_suc == mT_Solicitud.cod_suc && x.semana == mT_Solicitud.semana);

                    MT_Solicitud mT_Solicitud = db.MT_Solicitud.Find(id);

                    // InsertarSolicitud mT_Solicitud = db.sp_Quimipac_ConsultaMT_Solicitud().Where(x => x.seman == id)
                    //System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_Solicitud.Fecha;
                    bool seleccion = false;
                    if (mT_Solicitud != null)
                    {


                        List<SelectListItem> itemsOrden = new List<SelectListItem>();
                        List<SelectListItem> itemsBodega = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaBodega = db.sp_LINK_ConsultaBodegas(empresa_id.ToString()).ToList();
                        foreach (var bodega in listaBodega)
                        {
                            if (bodega.cod_bod == mT_Solicitud.BodegaRetirar)
                            {
                                seleccion = true;
                            }

                            itemsBodega.Add(new SelectListItem { Value = Convert.ToString(bodega.cod_bod), Text = bodega.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaBodega = new SelectList(itemsBodega, "Value", "Text");


                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(50).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_Solicitud.tipo)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaOrden = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(),0).ToList();
                        foreach (var orden in listaOrden)
                        {
                            if (orden.Id_OrdenTrabajo == mT_Solicitud.Id_OrdenTrabajo)
                            {
                                seleccion = true;
                            }

                            itemsOrden.Add(new SelectListItem { Value = Convert.ToString(orden.Id_OrdenTrabajo), Text = orden.Codigo_Cliente, Selected = seleccion });
                        }

                        SelectList selectlistaOrden = new SelectList(itemsOrden, "Value", "Text");



                        ViewBag.listaBodega = selectlistaBodega;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaOrden = selectlistaOrden;


                        return View(mT_Solicitud);

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Solicitud no existe";
                        return RedirectToAction("Solicitud_Materiales");
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Solicitud([Bind(Include = "Id_Solicitud, Id_OrdenTrabajo, fecha,BodegaRetirar,retira,solicitante, tipo")] MT_Solicitud mT_Solicitud)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            //mT_Solicitud.Estado = mT_Solicitud.Estado.Substring(0, 1);
                            //mT_EquipoTrabajo.Fecha = DateTime.Now;

                            // mT_Solicitud.Fecha = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);
                            db.Entry(mT_Solicitud).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Solicitud actualizada";

                            return RedirectToAction("Solicitud_Materiales");
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Solicitud_Materiales");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarSolicitud(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Solicitud mT_Solicitud = db.MT_Solicitud.Find(id);

                    mT_Solicitud.Estado = "E";

                    db.Entry(mT_Solicitud).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["mensaje_actualizado"] = "Solicitud Eliminada";
                    return RedirectToAction("Solicitud_Materiales");
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        #endregion

        //Solicitud Detalle faltaaaaaaaaaaaaaa
        

        //MATERIALES SOLICITUD
        #region

        [HttpGet]
        public ActionResult Materiales_Solicitud(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Solicitud mt_Solicitud = db.MT_Solicitud.Find(id);
                    if (mt_Solicitud != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Solicitud"] = mt_Solicitud.Id_Solicitud;
                        var listaMaterialesSolicitud = db.sp_Quimipac_ConsultaMT_SolicitudMateriales(id, empresa_id.ToString()).ToList();
                        var solicitud = mt_Solicitud.Id_Solicitud;
                        ViewBag.listaMaterialesSolicitud = listaMaterialesSolicitud;
                        ViewBag.solicitud = solicitud;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Solicitud_Materiales");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_Materiales_Solicitud()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> items = new List<SelectListItem>();

                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                    foreach (var item in listaItems)
                    {
                        items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion });
                    }

                    SelectList selectlistaItems = new SelectList(items, "Value", "Text");




                    ViewBag.listaItems = selectlistaItems;


                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Materiales_Solicitud([Bind(Include = "Id_Solicitud,fecha,Id_Item,Cantidad,Estado")] MT_Materiales_Solicitud mT_MaterialesSolicitud)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    //var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_MantTipoTrabajo"];
                    var id_Solicitud = System.Web.HttpContext.Current.Session["id_Solicitud"];

                    if (id_Solicitud == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idSolicitud = int.Parse(id_Solicitud.ToString());

                        if (ModelState.IsValid)
                        {
                            mT_MaterialesSolicitud.Id_Solicitud = idSolicitud;
                            var materialSolicitud = db.MT_Materiales_Solicitud.Where(x => x.Id_Solicitud == mT_MaterialesSolicitud.Id_Solicitud && x.Id_Item == mT_MaterialesSolicitud.Id_Item && x.Cantidad == mT_MaterialesSolicitud.Cantidad);
                            if (materialSolicitud.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Material de Solicitud ya existe";
                                return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });
                            }
                            else
                            {

                                mT_MaterialesSolicitud.Id_Solicitud = idSolicitud;
                                mT_MaterialesSolicitud.Estado = mT_MaterialesSolicitud.Estado.Substring(0, 1);


                                db.MT_Materiales_Solicitud.Add(mT_MaterialesSolicitud);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Material de Solicitud guardado";
                                return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });
                        }
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Materiales_Solicitud(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Materiales_Solicitud mT_MaterialSolicitud = db.MT_Materiales_Solicitud.Find(id);
                    bool seleccion = false;
                    if (mT_MaterialSolicitud != null)
                    {

                        List<SelectListItem> items = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                        var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                        foreach (var item in listaItems)
                        {
                            if (mT_MaterialSolicitud.Id_Item == item.cod_item)
                            {
                                seleccion = true;
                            }

                            items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaItems = new SelectList(items, "Value", "Text");


                        ViewBag.listaItems = selectlistaItems;


                        return View(mT_MaterialSolicitud);

                    }
                    else
                    {
                        var id_Solicitud = System.Web.HttpContext.Current.Session["id_Solicitud"];

                        if (id_Solicitud == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idSolicitud = int.Parse(id_Solicitud.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });
                        }
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Materiales_Solicitud([Bind(Include = "Id_Material_Solicitud,Id_Solicitud,fecha,Id_Item,Cantidad,Estado")] MT_Materiales_Solicitud mT_MaterialesSolicitud)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Solicitud = System.Web.HttpContext.Current.Session["id_Solicitud"];
                    // int bandera = 0;
                    if (id_Solicitud == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idSolicitud = int.Parse(id_Solicitud.ToString());

                        if (ModelState.IsValid)
                        {
                            mT_MaterialesSolicitud.Id_Solicitud = idSolicitud;
                            mT_MaterialesSolicitud.Estado = mT_MaterialesSolicitud.Estado.Substring(0, 1);


                            db.Entry(mT_MaterialesSolicitud).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Material de Solicitud actualizado";

                            return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Materiales_Solicitud", new { id = idSolicitud });
                        }
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarSolicitudMateriales(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Materiales_Solicitud mT_SolicitudMateriales = db.MT_Materiales_Solicitud.Find(id);

                    mT_SolicitudMateriales.Estado = "E";

                    db.Entry(mT_SolicitudMateriales).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["mensaje_actualizado"] = "Solicitud Materiales Eliminada";
                    return RedirectToAction("Materiales_Solicitud", new { id = mT_SolicitudMateriales.Id_Solicitud });
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }

        #endregion


        //PROGRAMA TRABAJO
        #region
        [HttpGet]
        public ActionResult ProgramaTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {


                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("MantTipoTrabajo", "Mantenimiento");
                    }

                    else
                    {

                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string usuario = user_id.ToString();

                        var HorarioActividad = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo(usuario, empresa_id.ToString()).ToList();


                        HorarioActividad = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo(usuario, empresa_id.ToString()).ToList();

                        foreach (var horario in HorarioActividad)
                        {

                            DateTime horaInicioUTC = DateTime.SpecifyKind(DateTime.Parse(horario.Fecha_Registro.ToString()), DateTimeKind.Local);
                            DateTime horaInicioLocalVersion = horaInicioUTC.ToLocalTime();

                            horario.Fecha_Registro = horaInicioLocalVersion;


                        }

                        List<SelectListItem> itemsContrato = new List<SelectListItem>();
                        List<SelectListItem> itemsTipoTrabajo = new List<SelectListItem>();
                        List<SelectListItem> itemsGrupoTrabajo = new List<SelectListItem>();

                        string empresacod = empresa_id.ToString();

                        var listaContrato = db.sp_Quimipac_ConsultaMT_ContratoGeneral(empresacod).ToList();
                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContrato = listaContrato.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContrato = listaContrato.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                        foreach (var contrato in listaContrato)
                        {
                            itemsContrato.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Nombre });
                        }

                        SelectList selectlistaContrato = new SelectList(itemsContrato, "Value", "Text");

                        var listaTipoTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems(2, empresa_id.ToString()).ToList();
                        foreach (var tptrabajo in listaTipoTrabajo)
                        {
                            itemsTipoTrabajo.Add(new SelectListItem { Value = Convert.ToString(tptrabajo.Id_TipoTrabajo), Text = tptrabajo.Descripcion });
                        }

                        SelectList selectlistaTipoTrabajo = new SelectList(itemsTipoTrabajo, "Value", "Text");

                        var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo().ToList();
                        foreach (var gptrabajo in listaGrupoTrabajo)
                        {
                            itemsGrupoTrabajo.Add(new SelectListItem { Value = Convert.ToString(gptrabajo.Id_GrupoTrabajo), Text = gptrabajo.Nombre });
                        }

                        SelectList selectlistaGrupoTrabajo = new SelectList(itemsGrupoTrabajo, "Value", "Text");


                        //. . . 
                        var lkcontratoProg = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos(empresacod).ToList();
                        //var lkcontratosNoOt = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo_ContratosNoOT(empresa_id.ToString()).ToList();
                        var lkOrdenTrabajo = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo_OT(empresa_id.ToString()).ToList();







                        ViewBag.ProgramaTrabajo = HorarioActividad;
                        ViewBag.listaContrato = selectlistaContrato;
                        ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;
                        ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;

                        //....
                        ViewBag.lkcontratoProg = lkcontratoProg;
                        //ViewBag.lkcontratosNoOt = lkcontratosNoOt;
                        ViewBag.lkOrdenTrabajo = lkOrdenTrabajo;

                        return View();
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_ProgramaTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
                {
                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    List<SelectListItem> itemsLineas = new List<SelectListItem>();
                    List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsServicio = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                    var listaServicio = db.sp_Quimipac_ConsultaMT_Servicio(empresa_id.ToString()).ToList();
                    foreach (var servicio in listaServicio)
                    {
                        itemsServicio.Add(new SelectListItem { Value = Convert.ToString(servicio.Id_Servicio), Text = servicio.Descripcion });
                    }

                    SelectList selectlistaServicios = new SelectList(itemsServicio, "Value", "Text");

                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                    foreach (var cliente in listaClientes)
                    {
                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");


                    var listaLineas = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ProgramaTrabajo([Bind(Include = "Id_Contrato,Id_TIpo_Trabajo,Fecha_Ini,Fecha_Fin,Fecha_Registro,Descripcion,Direccion,Ubicacion,Estado,Id_GrupoTrabajo")] MT_ProgramaTrabajo mT_ProgramaTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var tipoTrabajo = db.MT_ProgramaTrabajo.Where(x => x.Id_Contrato == mT_ProgramaTrabajo.Id_Contrato && x.Id_TIpo_Trabajo == mT_ProgramaTrabajo.Id_TIpo_Trabajo && x.Fecha_Registro == mT_ProgramaTrabajo.Fecha_Registro && x.Fecha_Ini == mT_ProgramaTrabajo.Fecha_Ini && x.Fecha_Fin == mT_ProgramaTrabajo.Fecha_Fin);
                        //var ot = db.MT_OrdenTrabajo.Where(x => x.Id_contrato == mT_ProgramaTrabajo.Id_Contrato && x.Id_tipo_trabajo_ejecutado == mT_ProgramaTrabajo.Id_TIpo_Trabajo && x.EstadoEditOrden == "A");
                        var ot = db.MT_OrdenTrabajo.Where(x => x.Id_Proyecto == mT_ProgramaTrabajo.Id_Contrato && x.Id_tipo_trabajo_ejecutado == mT_ProgramaTrabajo.Id_TIpo_Trabajo && x.EstadoEditOrden == "A");


                        if (tipoTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("ProgramaTrabajo");
                        }
                        else
                        {

                            if (ot.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Contrato ya tiene Orden de Trabajo Generada, favor verificar!";
                                return RedirectToAction("ProgramaTrabajo");
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
                                    mT_ProgramaTrabajo.Id_Usuario = user_id.ToString();
                                    mT_ProgramaTrabajo.Estado = mT_ProgramaTrabajo.Estado.Substring(0, 1);



                                    var dbe = new DataBase_Externo();
                                    int n = dbe.GuardarProgramaTrabajo(mT_ProgramaTrabajo.Id_Contrato, mT_ProgramaTrabajo.Id_Usuario, mT_ProgramaTrabajo.Id_TIpo_Trabajo, mT_ProgramaTrabajo.Fecha_Registro, mT_ProgramaTrabajo.Fecha_Ini, mT_ProgramaTrabajo.Fecha_Fin, mT_ProgramaTrabajo.Descripcion, mT_ProgramaTrabajo.Direccion, mT_ProgramaTrabajo.Ubicacion, mT_ProgramaTrabajo.Estado, mT_ProgramaTrabajo.Id_GrupoTrabajo);

                                    /*db.MT_ProgramaTrabajo.Add(mT_ProgramaTrabajo);
                                    db.SaveChanges();*/
                                    if (n <= 0)
                                    {
                                        TempData["mensaje_error"] = "Valores incorrectos";
                                        return RedirectToAction("ProgramaTrabajo");
                                    }
                                    else
                                    {

                                        TempData["mensaje_correcto"] = "Registro guardado correctamente";
                                        return RedirectToAction("ProgramaTrabajo");
                                    }

                                }
                            }



                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("ProgramaTrabajo");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }



        public JsonResult GetEventsCalendar()
        {
            using (BD_QUIMIPACEntities dc = new BD_QUIMIPACEntities())
            {
                var events = db.MT_ProgramaTrabajo.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public JsonResult SaveEvent(MT_ProgramaTrabajo e)
        {

            var status = false;
            using (BD_QUIMIPACEntities dc = new BD_QUIMIPACEntities())
            {
                if (e.Id_ProgramaTrabajo > 0)
                {
                    //update
                    var v = dc.MT_ProgramaTrabajo.Where(a => a.Id_ProgramaTrabajo == e.Id_ProgramaTrabajo).FirstOrDefault();
                    if (v != null)
                    {
                        v.Id_Contrato = e.Id_Contrato;
                        v.Id_GrupoTrabajo = e.Id_GrupoTrabajo;
                        v.Id_TIpo_Trabajo = e.Id_TIpo_Trabajo;
                        var user_id = System.Web.HttpContext.Current.Session["usuario"];
                        v.Id_Usuario = user_id.ToString();
                        v.Ubicacion = e.Ubicacion;
                        v.Descripcion = e.Descripcion;
                        v.Direccion = e.Direccion;
                        v.Estado = e.Estado.Substring(0, 1);
                        v.Fecha_Ini = e.Fecha_Ini;
                        v.Fecha_Fin = e.Fecha_Fin;
                        v.Fecha_Registro = DateTime.Now;

                    }
                }
                else
                {
                    dc.MT_ProgramaTrabajo.Add(e);

                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        public JsonResult DeleteEvent(int eventId)
        {
            var status = false;

            using (BD_QUIMIPACEntities dc = new BD_QUIMIPACEntities())
            {
                var v = dc.MT_ProgramaTrabajo.Where(a => a.Id_ProgramaTrabajo == eventId).FirstOrDefault();
                if (v != null)
                {
                    var t = dc.MT_ProgramaTrabajo.Find(v);
                    t.Estado = "E";
                    dc.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };

        }

        public JsonResult GetEventosMT_ProgrmaTrab()
        {

            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var lk = db.sp_Quimipac_ConsultaMt_ProgramaTrabajo(user_id.ToString(), empresa_id.ToString()).ToList();

                return Json(lk, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }

            //using (var ctx = new BD_QUIMIPACEntities()) { }


            // var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


        }


        //Parametros Para crear OT
        public JsonResult GetDataMT_ProgTrabajo(int? Id_ProgramaTrab)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var mensaje = new JsonResult();
            int nreg = 0;
            try
            {

                var registros = db.MT_ProgramaTrabajo.Where(vq => vq.Id_ProgramaTrabajo == Id_ProgramaTrab);
                foreach (var item in registros)
                {
                    var dbe = new DataBase_Externo();
                    nreg = dbe.Generar_OT_ProgramaTrabajo(Id_ProgramaTrab, item.Id_Contrato, item.Id_TIpo_Trabajo, item.Direccion, user_id.ToString(), item.Id_GrupoTrabajo, item.Fecha_Ini, item.Fecha_Fin);

                }
                if (nreg < 1)
                {
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                }
                else { mensaje.Data = "Registros guardados correctamente"; }

                return mensaje;
                //  return Json(lkItems, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }

        }

        //upd_FechaEvento Actualizar fechas Arrastrar y soltar
        public JsonResult UPD_FechaEvento(int IdProgTrabajo, DateTime Fini, DateTime Ffin)
        {
            //  var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var mensaje = new JsonResult();
            int nreg = 0;
            try
            {
                //Ffin=Ffin.Date.Add(new TimeSpan(23, 59, 58));//,997));
                var dbe = new DataBase_Externo();
                nreg = dbe.UPD_FechaEvento_ProgTrab(IdProgTrabajo, Fini, Ffin);


                if (nreg < 1)
                {
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                }
                else { mensaje.Data = "Registros guardados correctamente"; }

                return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        #endregion

        //MATENIMIENTO DE PERMISOS
        #region
        [HttpGet]
        public ActionResult MantPermisos()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    //var lkUsuarios = db.sp_Quimipac_ConsultaUsuarios_Permisos("").ToList();
                    string empresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();



                    var liUsuarios = db.sp_LINK_ConsultaUsuarios("ALL", empresaID).ToList().OrderBy(vq => vq.user_id);
                    //liUsuarios = liUsuarios.Where(vq=>vq.user_status=="A");

                    var LiUsuariosTabla = new List<Permisos_Usuario>();

                    foreach (var item in liUsuarios.Where(vq => vq.user_status == "A"))
                    {
                        var liPermisosUsuarios = db.MT_Permiso.Where(vq => vq.Estado == "A" && vq.Id_Usuario == item.user_id).OrderByDescending(vq => vq.Fecha_Registro).ToList();
                        var obPermiso = new Permisos_Usuario
                        {
                            user_id = item.user_id
                           ,
                            Id_Usuario = item.primer_nombre + " " + item.segundo_nombre + " " + item.primer_apellido + " " + item.segundo_apellido
                           ,
                            Consultar = (liPermisosUsuarios.Count() > 0) ? liPermisosUsuarios.ElementAt(0).Consultar : ""
                           ,
                            Modificar = (liPermisosUsuarios.Count() > 0) ? liPermisosUsuarios.ElementAt(0).Modificar : ""
                           ,
                            Crear = (liPermisosUsuarios.Count() > 0) ? liPermisosUsuarios.ElementAt(0).Crear : ""
                           ,
                            Eliminar = (liPermisosUsuarios.Count() > 0) ? liPermisosUsuarios.ElementAt(0).Eliminar : ""
                           ,
                            Aprobar = (liPermisosUsuarios.Count() > 0) ? liPermisosUsuarios.ElementAt(0).Aprobar : ""
                           ,
                            Estado = (liPermisosUsuarios.Count() > 0) ? "A" : "I"
                        };
                        LiUsuariosTabla.Add(obPermiso);
                    }
                    //var lkUsuarios = db.sp_Quimipac_ConsultaUsuarios_Permisos("", empresaID).ToList();
                    ViewBag.LiUsuariosTabla = LiUsuariosTabla;

                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }



        //AGREGAR CLIENTES AL MODAL
        public JsonResult AgregarClientes_Modal(List<string> liSeleccionadoCLTE)//List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            try
            {
                using (var ctx = new BD_QUIMIPACEntities())
                {
                    var EmpresaID = System.Web.HttpContext.Current.Session["empresa"];
                    var lksql = (ctx.Database.SqlQuery<sp_LINK_ConsultaClientes_Result>("sp_LINK_ConsultaClientes @p0", EmpresaID).ToList());

                    if (liSeleccionadoCLTE != null)
                    {
                        if (liSeleccionadoCLTE.Count() > 0)
                        {
                            var liClientes = new List<sp_LINK_ConsultaClientes_Result>();
                            foreach (var item in lksql)
                            {
                                if (!liSeleccionadoCLTE.Contains(item.cod_cli))
                                {
                                    liClientes.Add(item);
                                }
                            }
                            return Json(liClientes, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //else{ return Json(lksql, JsonRequestBehavior.AllowGet);}
                    return Json(lksql, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }



        [HttpGet]
        public ActionResult Agregar_MantPermiso_Usuario()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {

                        return View();
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [HttpGet]
        public ActionResult Agregar_ManPermiso_UsuarioClientes(string id)//
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    System.Web.HttpContext.Current.Session["usuario_Permiso"] = id;
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        List<SelectListItem> itemsClientes = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var LkClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                        foreach (var vq in LkClientes)
                        {
                            itemsClientes.Add(new SelectListItem { Value = Convert.ToString(vq.cod_cli), Text = vq.nom_cli });
                        }

                        SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                        ViewBag.LkClientes = selectlistaClientes;

                        return View();
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        //*1*

        public JsonResult AgregarClientes_Permisos(List<TempClientes> lkItems, string Usuario)//List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            try
            {
                return Json(lkItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        public class TempClientes
        {
            public int Id_Cliente { get; set; }
            public string Nombre_Cliente { get; set; }

        }

        //1**

        //Llenar modal contratosXClientes time
        public JsonResult AddContratosClientes_Modal(string IdClienteBoton, List<int> LiTablaContrato)//id)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

            var mensaje = new JsonResult();
            try
            {
                //aqui cambie el sp
                //var lkContratos = db.sp_Quimipac_ConsultaMT_Permisos_XContratos(empresa_id.ToString(), IdClienteBoton).ToList();
                var lkContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).Where(x=>x.Id_Cliente == IdClienteBoton).ToList();

                //sp_Quimipac_ConsultaMT_Permisos_XContratos_Result o = new sp_Quimipac_ConsultaMT_Permisos_XContratos_Result();
                //List<sp_Quimipac_ConsultaMT_Permisos_XContratos_Result> liContrato = new List<sp_Quimipac_ConsultaMT_Permisos_XContratos_Result>();
                List<sp_Quimipac_ConsultaMT_Contrato_Result> liContrato = new List<sp_Quimipac_ConsultaMT_Contrato_Result>();

                if (lkContratos != null)
                {
                    foreach (var item in lkContratos)
                    {
                        if (LiTablaContrato != null)
                        {
                            if (!LiTablaContrato.Contains(item.Id_Contrato))
                            {
                                liContrato.Add(item);
                            }
                        }
                        else { liContrato.Add(item); }
                    }
                }
                return Json(liContrato, JsonRequestBehavior.AllowGet); //return Json(lkContratos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        //*1*


        //Llenar TablaContratos seleccionados en el Modal
        public JsonResult AddTablaContratosClientes(List<TempContratoClientes> lkItems)//, string Usuario---List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


            //var mensajeDiv = new TempMensaje();
            var mensaje = new JsonResult();
            try
            {
                //  var lkContratos = db.sp_Quimipac_ConsultaMT_Permisos_XContratos(empresa_id.ToString(), id).ToList();
                //JsonUtil dataSet = JsonConvert.DeserializeObject<JsonUtil>(lkContratos);

                return Json(lkItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //mensajeDiv.Ok = false;
                // return resultado;
                return Json(Response.StatusCode);
            }
        }

        public class TempContratoClientes
        {
            public int Id_Contrato { get; set; }
            public string Nombre { get; set; }

        }



        //*1*
        //Llenar TablaContratos seleccionados en el Modal    AddTablaProyectosClientes
        public JsonResult AddTablaProyectosClientes(List<TempProyectosClientes> lkItems)//, string Usuario---List<TempItems> lkItems, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


            //var mensajeDiv = new TempMensaje();
            var mensaje = new JsonResult();
            try
            {
                //  var lkContratos = db.sp_Quimipac_ConsultaMT_Permisos_XContratos(empresa_id.ToString(), id).ToList();
                //JsonUtil dataSet = JsonConvert.DeserializeObject<JsonUtil>(lkContratos);

                return Json(lkItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //mensajeDiv.Ok = false;
                // return resultado;
                return Json(Response.StatusCode);
            }
        }



        public class TempProyectosClientes
        {
            public int Id_Proyecto { get; set; }
            public string Codigo_Cliente { get; set; }
            //public string Codigo_Interno { get; set; }

        }//   

        //*1*

        public JsonResult GuardarPermisos(List<MT_Permiso> lkItemsPermisos)//, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var mensaje = new JsonResult();
            int nRegistro = 0;
            var EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();

            try
            {
                foreach (var item in lkItemsPermisos)
                {
                    var obPermiso = new MT_Permiso
                    {
                        //Id_Permiso = item,
                        Id_Usuario = item.Id_Usuario,
                        Id_Cliente = item.Id_Cliente,
                        Id_Contrato = item.Id_Contrato,
                        Id_Proyecto = item.Id_Proyecto,
                        Id_Tipo_Trabajo = item.Id_Tipo_Trabajo,
                        Id_Linea = item.Id_Linea,
                        Consultar = item.Consultar,
                        Modificar = item.Modificar,
                        Crear = item.Crear,
                        Eliminar = item.Eliminar,
                        Aprobar = item.Aprobar,
                        Usuario = user_id.ToString(),
                        Fecha_Registro = DateTime.Now,
                        Estado = "A",
                        Id_Empresa = EmpresaID
                    };
                    db.MT_Permiso.Add(obPermiso);
                    db.SaveChanges();

                    //DataBase_Externo dbe = new DataBase_Externo();
                    //var band = dbe.GuardarMTPermisos(item.Id_Usuario, item.Id_Cliente, item.Id_Contrato, item.Id_Proyecto, item.Consultar, item.Modificar, item.Crear, item.Eliminar, item.Aprobar, System.Web.HttpContext.Current.Session["usuario"].ToString(), DateTime.Now, "A");
                    nRegistro++;
                }
                mensaje.Data = (nRegistro < 1) ? "Error al ingresar, favor verificar datos ingresados!" : "Registros guardados correctamente";
                return mensaje;
                //  return Json(lkItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }



        /*
        public class MT_Permiso_Guardar
        {
            //public int Id_Permiso { get; set; }
            public string Id_Usuario { get; set; }
            public string Id_Cliente { get; set; }
            public Nullable<int> Id_Contrato { get; set; }
            public Nullable<int> Id_Proyecto { get; set; }
            //public Nullable<int> Id_Tipo_Trabajo { get; set; }
            //public string Id_Linea { get; set; }
            public string Consultar { get; set; }
            public string Modificar { get; set; }
            public string Crear { get; set; }
            public string Eliminar { get; set; }
            public string Usuario { get; set; }
            public Nullable<System.DateTime> Fecha_Registro { get; set; }
            public string Estado { get; set; }
        }*/

        public JsonResult AddProyectosClientes_Modal(string IdClienteBoton, List<int> LiTablaProyecto)//int id)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

            var mensaje = new JsonResult();
            try
            {
                //reemplace por el otro sp pues esos estan demas
                //var lkProyectos = db.sp_Quimipac_ConsultaMT_Permisos_XProyectos(empresa_id.ToString(), id).ToList();
                //var lkProyectos = db.sp_Quimipac_ConsultaMT_Permisos_XProyectos(empresa_id.ToString(), IdClienteBoton).ToList();
                var lkProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
                List<sp_Quimipac_ConsultaMT_Proyecto_Result> liProyecto = new List<sp_Quimipac_ConsultaMT_Proyecto_Result>();

                if (lkProyectos != null)
                {
                    foreach (var item in lkProyectos)
                    {
                        if (LiTablaProyecto != null)
                        {
                            if (!LiTablaProyecto.Contains(item.Id_Proyecto))
                            {
                                liProyecto.Add(item);
                            }
                        }
                        else { liProyecto.Add(item); }
                    }
                }//JsonUtil dataSet = JsonConvert.DeserializeObject<JsonUtil>(lkContratos);

                return Json(liProyecto, JsonRequestBehavior.AllowGet);//lkProyectos
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }



       
        //Editar Permisos
        [HttpGet]
        public ActionResult Editar_MantPermiso_Usuario(string user_id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    // var dbe = new DataBase_Externo();
                    //ViewBag.lkPermiso = dbe.ObtenerPermisosXUsuario(IdUsuario,System.Web.HttpContext.Current.Session["empresa"].ToString());

                    ViewBag.EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpPost]
        public JsonResult ActualizarPermisos(List<MT_Permiso> lkEditarPermisos, string Usuario)//, int ValueTipo, int ValueProyCont, int ValueTipoTrab, string ValueMoneda)
        {
            var user_id = (System.Web.HttpContext.Current.Session["usuario"] != null) ? System.Web.HttpContext.Current.Session["usuario"].ToString() : null;
            var EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();
            var mensaje = new JsonResult();
            int nRegistro = 0;


            try
            {
                if (user_id != null)
                {
                    var liPermisoUPD = db.MT_Permiso.Where(vq => vq.Id_Usuario == Usuario && vq.Estado == "A").ToList();
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        var nn = ctx.Database.ExecuteSqlCommand("DELETE FROM MT_PERMISO WHERE ID_USUARIO ='"+Usuario+"'");
                    }
                    /*if (liPermisoUPD.Count() != 0)
                    {
                        foreach (var item in liPermisoUPD)
                            item.Estado = "I";// item.Estado.Replace("", "");
                        db.SaveChanges();
                    }
                    */

                    foreach (var item in lkEditarPermisos)
                    {
                        //DataBase_Externo dbe = new DataBase_Externo();
                        //var band = dbe.ActualizarMTPermisos(item.Id_Permiso, item.Consultar, item.Modificar, item.Crear, item.Eliminar, item.Aprobar, System.Web.HttpContext.Current.Session["usuario"].ToString(), item.Estado);
                        //


                        MT_Permiso lp = new MT_Permiso()
                        {
                            Id_Usuario = item.Id_Usuario,
                            Id_Cliente = item.Id_Cliente,
                            Id_Contrato = item.Id_Contrato,
                            Id_Proyecto = item.Id_Proyecto,
                            Consultar = item.Consultar,
                            Modificar = item.Modificar,
                            Crear = item.Crear,
                            Eliminar = item.Eliminar,
                            Aprobar = item.Aprobar,
                            Usuario = user_id,
                            Fecha_Registro = DateTime.Now,
                            Estado = "A",
                            Id_Empresa = EmpresaID
                        };

                        db.MT_Permiso.Add(lp);
                        db.SaveChanges();
                        nRegistro++;
                    }


                    mensaje.Data = (nRegistro < 1) ? "Error al ingresar, favor verificar datos ingresados!" : "Registros Actualizado correctamente";

                    //if (nRegistro < 1)
                    //{
                    //    mensaje.Data = ;
                    //    return mensaje;
                    //}

                    //mensaje.Data = "Registros Actualizado correctamente";

                    //  return Json(lkItems, JsonRequestBehavior.AllowGet);
                }
                return mensaje;
            }
            catch (Exception e)
            {
                //mensajeDiv.Ok = false;
                // return resultado;
                return Json(Response.StatusCode);
            }
        }
        //Eliminar  EliminarPermisoUsuario
        [HttpGet]
        public ActionResult EliminarPermisoUsuario(string id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    //MT_TipoTrabajo mT_TipoTrabajo = db.MT_TipoTrabajo.Find(id);

                    //mT_TipoTrabajo.Estado = "E";

                    //db.Entry(mT_TipoTrabajo).State = EntityState.Modified;
                    //db.SaveChanges();
                    var lkPermisos = db.MT_Permiso.Where(vq => vq.Id_Usuario == id && vq.Estado == "A");
                    if (lkPermisos.Count() > 0)
                    {
                        foreach (var item in lkPermisos)
                        {
                            if (item.Estado.Equals("A"))
                            {
                                item.Estado = item.Estado.Replace("A", "E");
                                item.Estado = "E";
                                //db.Entry(mT_TipoTrabajo).State = EntityState.Modified;
                            }

                        }
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Permisos eliminados a Usuario";
                        return RedirectToAction("MantPermisos");
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Error al eliminar permisos a Usuario";
                        return RedirectToAction("MantPermisos");
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }



        #endregion

        //INGRESO MATERIALES
        #region

        [HttpGet]
        public ActionResult IngresoMateriales()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    var listaIngresoMateriales = db.sp_Quimipac_ConsultaMT_ItemsBodega(empresa_id.ToString()).ToList();
                    ViewBag.listaIngresoMateriales = listaIngresoMateriales;
                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Agregar_IngresoMateriales()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsOrden = new List<SelectListItem>();
                    List<SelectListItem> itemsBodega = new List<SelectListItem>();
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaBodega = db.sp_LINK_ConsultaBodegas(empresa_id.ToString()).ToList();
                    foreach (var bodega in listaBodega)
                    {
                        itemsBodega.Add(new SelectListItem { Value = Convert.ToString(bodega.cod_bod), Text = bodega.nombre });
                    }

                    SelectList selectlistaBodega = new SelectList(itemsBodega, "Value", "Text");






                    ViewBag.listaBodega = selectlistaBodega;




                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_IngresoMateriales([Bind(Include = "Bodega, Descripcion,Cantidad")] MT_ItemsBodega mT_IngresoMateriales)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var solicitud = db.MT_ItemsBodega.Where(x => x.Bodega == mT_IngresoMateriales.Bodega && x.Descripcion == mT_IngresoMateriales.Descripcion && x.Cantidad == mT_IngresoMateriales.Cantidad);
                        if (solicitud.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Material ya existe";
                            return RedirectToAction("IngresoMateriales");
                        }
                        else
                        {
                            //mT_Solicitud.Estado = mT_Solicitud.Estado.Substring(0, 1);


                            db.MT_ItemsBodega.Add(mT_IngresoMateriales);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Material guardada";
                            return RedirectToAction("IngresoMateriales");

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("IngresoMateriales");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_IngresoMateriales(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_ItemsBodega mT_ItemsBodega = db.MT_ItemsBodega.Find(id);
                    //System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_Solicitud.Fecha;
                    bool seleccion = false;
                    if (mT_ItemsBodega != null)
                    {



                        List<SelectListItem> itemsBodega = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaBodega = db.sp_LINK_ConsultaBodegas(empresa_id.ToString()).ToList();
                        foreach (var bodega in listaBodega)
                        {
                            if (bodega.cod_bod == mT_ItemsBodega.Bodega)
                            {
                                seleccion = true;
                            }

                            itemsBodega.Add(new SelectListItem { Value = Convert.ToString(bodega.cod_bod), Text = bodega.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaBodega = new SelectList(itemsBodega, "Value", "Text");






                        ViewBag.listaBodega = selectlistaBodega;



                        return View(mT_ItemsBodega);

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Material no existe";
                        return RedirectToAction("IngresoMateriales");
                    }

                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_IngresoMateriales([Bind(Include = "Id_Solicitud, Id_OrdenTrabajo, fecha,BodegaRetirar,retira,solicitante, tipo")] MT_Solicitud mT_Solicitud)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            //mT_Solicitud.Estado = mT_Solicitud.Estado.Substring(0, 1);
                            //mT_EquipoTrabajo.Fecha = DateTime.Now;

                            // mT_Solicitud.Fecha = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);
                            db.Entry(mT_Solicitud).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Solicitud actualizada";

                            return RedirectToAction("Solicitud_Materiales");
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Solicitud_Materiales");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarIngresoMateriales(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Solicitud mT_Solicitud = db.MT_Solicitud.Find(id);

                    mT_Solicitud.Estado = "E";

                    db.Entry(mT_Solicitud).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["mensaje_actualizado"] = "Solicitud Eliminada";
                    return RedirectToAction("Solicitud_Materiales");
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }

        }
        #endregion

        //Consultar Stock
        #region
        [HttpGet]
        public ActionResult ConsultaStock()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    //var listaStock = db.sp_LINK_ConsultaStock().ToList();
                    //ViewBag.listaStock = listaStock;
                    return View();
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        #endregion


        [HttpPost]
        public ActionResult Item_OpcPermiso(string PersonaID, string EmpresaID, string Criterio)
        {
            try
            {
                var PermisiItemLI = db.MT_Permiso.Where(vq => vq.Id_Usuario == PersonaID && vq.Id_Empresa == EmpresaID && vq.Estado == "A").Take(1).ToList();
                if (Criterio == "PermisoContrato") 
                {
                    PermisiItemLI = db.MT_Permiso.Where(vq => vq.Id_Usuario == PersonaID && vq.Id_Empresa == EmpresaID && vq.Id_Contrato != null && vq.Estado == "A").ToList();
                }
                else if (Criterio == "PermisoProyecto") 
                {
                    PermisiItemLI = db.MT_Permiso.Where(vq => vq.Id_Usuario == PersonaID && vq.Id_Empresa == EmpresaID && vq.Id_Proyecto != null && vq.Estado == "A").ToList();
                }
                
                return Json(PermisiItemLI, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Item_OpcPermiso E:" +e.Message);
                return Json(Response.StatusCode);
            }
        }

        #region MenuxROl

        //agregar Rol
        [HttpPost]
        public ActionResult AgregarRol(string Rol)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string paramRol = Rol.ToLower().Trim();
                        var AuxRol = db.Roles.Where(vq => vq.Descripcion.ToLower() == paramRol).ToList();
                        if (AuxRol.Count() >0)
                        {
                            string msn = string.Empty;
                            if (AuxRol.Any(vq => vq.Estado == "I"))
                            {
                                msn = ", por favor habilitar el estado.";
                            }
                            TempData["mensaje_error"] = "Rol ya existe" + msn; 
                            return RedirectToAction("MantMenuDRol", "Mantenimiento");
                        }
                        else
                        {
                            //var objRol = new Roles { 
                            //    Descripcion = Rol.Trim(),
                            //    Estado = "A"
                            //};

                            db.Roles.Add(new Roles { Descripcion = Rol.Trim(), Estado = "A" });
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Registro guardado correctamente";
                            return RedirectToAction("MantMenuDRol", "Mantenimiento");
                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("MantMenuDRol", "Mantenimiento");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("AgregarRol:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [HttpGet]
        public ActionResult MantMenuDRol()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        /*var registros = ctx.Database.SqlQuery<RolMenu>("SP_QUIMIPAC_MENU_X_ROL @p0", "TBGENERAL").ToList();
                        List<Roles> RolesLI = new List<Roles>();
                        RolesLI.Add(new Roles { Descripcion = ""});
                        foreach (var item in db.Roles.Where(vq => vq.Estado == "A").OrderBy(vq=>vq.Descripcion))
                        {
                            RolesLI.Add(new Roles { Id_Rol = item.Id_Rol,Descripcion = item.Descripcion });
                        }
                        */
                        //ViewBag.RolesLI = new SelectList(RolesLI, "Id_Rol","Descripcion");

                        //ViewBag.RolesLI = db.Roles.Where(vq => vq.Estado == "A").OrderBy(vq => vq.Descripcion);

                        return View(db.Roles.Where(vq => vq.Estado == "A").OrderBy(vq => vq.Descripcion));
                    }

                    //return View();

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("MantRol_Menu:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
        }

        //public void getItem_Menu(List<Menu_Aux> FullMenu,Menu itemMenu,int RolID)
        public List<Menu_Aux> getItem_Menu(int RolID)
        {
            var MenuLI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
            //var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, 0, EmpresaID).ToList();
            var opcPermitidas = db.MenuRol.Where(vq => vq.Estado == "A" && vq.Id_Rol == RolID).ToList();

            var MenuUsuario = new List<Menu_Aux>();
            foreach (var item in MenuLI)
            {
                bool IsPAdre = item.Action == "" ? true : false;
                bool IsSelected = false;
                if (item.Action == "")
                {
                    var objMenu0 = new Menu_Aux
                    {
                        Id_Menu = item.Id_Menu,
                        Descripcion_Menu = item.Menu1,
                        Icono = item.Icono,
                        IsPadre = true,
                        IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true),
                        Nivel_Profundidad = 0,
                        Orden = item.Orden,
                        Padre = item.Padre,
                        Estado = item.Estado
                    };
                    MenuUsuario.Add(objMenu0);
                    //var opciones_nivel2 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item.Id_Menu, EmpresaID).ToList();
                    var opciones_nivel2 = db.Menu.Where(vq => vq.Padre == item.Id_Menu && vq.Estado == "A").ToList();
                    if (opciones_nivel2.Count() != 0)
                    {
                        foreach (var item2 in opciones_nivel2)
                        {
                            //var opciones_nivel3 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item2.Id_Menu, EmpresaID).ToList();
                            var opciones_nivel3 = db.Menu.Where(vq => vq.Padre == item2.Id_Menu && vq.Estado == "A").ToList();
                            if (opciones_nivel3.Count() != 0)
                            {
                                foreach (var item3 in opciones_nivel3)
                                {
                                    //al mommento no ay 3 niveles
                                }
                            }
                            else
                            {
                                var objMenu2 = new Menu_Aux
                                {
                                    Id_Menu = item2.Id_Menu,
                                    Descripcion_Menu = item2.Menu1,
                                    Icono = item2.Icono,
                                    IsPadre = false,
                                    IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item2.Id_Menu) != true ? false : true),
                                    Nivel_Profundidad = 1,
                                    Orden = item2.Orden,
                                    Padre = item2.Padre,
                                    Estado = item2.Estado
                                };
                                MenuUsuario.Add(objMenu2);
                                //< a href = "@Url.Action(item2.Action, item2.controlador)" id = "@item2.Action" >< i class="@item2.Icono"></i> <span>@item2.Menu</span></a>
                            }
                        }
                    }
                }
                else
                {
                    var objMenu1 = new Menu_Aux
                    {
                        Id_Menu = item.Id_Menu,
                        Descripcion_Menu = item.Menu1,
                        Icono = item.Icono,
                        IsPadre = false,
                        IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true),
                        Nivel_Profundidad = 0,
                        Orden = item.Orden,
                        Padre = item.Padre,
                        Estado = item.Estado
                    };
                    MenuUsuario.Add(objMenu1);
                    //< a href = "@Url.Action(item.Action, item.controlador)" id = "@item.Action" > < i class="@item.Icono"></i> <span id = "color" > @item.Menu </ span ></ a >
                }
            }

            var menuiten = MenuUsuario.OrderBy(vq => vq.Orden);
            return MenuUsuario;

            /*foreach (var item in FullMenu)
            {
                var objMenu0 = new Menu_Aux
                {
                    Id_Menu = item.Id_Menu,
                    Descripcion_Menu = item.Descripcion_Menu,
                    Icono = item.Icono,
                    IsPadre = true,
                    IsSelected = false,
                    Nivel_Profundidad = 0,
                    Orden = item.Orden,
                    Padre = item.Padre,
                    Estado = item.Estado
                };
                FullMenu.Add(objMenu0);
            }*/
            //return objMenu0;// FullMenu;
        }
        [HttpGet]
        public ActionResult MantMenuDRol_Editar(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                var MenuFull_Tot = new List<Menu_Aux>();
                string usuarioID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();
                try
                {
                    var MenuPadre0_LI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
                    var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, 0, EmpresaID).ToList();

                    //var MenuUsuario = db.;
                    var opcPermitidas = db.MenuRol.Where(vq => vq.Estado == "A" && vq.Id_Rol == id).ToList();
                    foreach (var item in MenuPadre0_LI)
                    {
                        if (item.Action == null)//"")
                        {
                            var o0 = new Menu_Aux();
                            o0.Id_Menu = item.Id_Menu;
                            o0.Descripcion_Menu = item.Menu1;
                            o0.Icono = item.Icono;
                            o0.IsPadre = true;
                            o0.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true;
                            o0.Nivel_Profundidad = 0;
                            o0.Orden = item.Orden;
                            o0.Padre = item.Padre;
                            o0.Estado = item.Estado;

                            MenuFull_Tot.Add(o0);

                            var opciones_nivel2 = db.Menu.Where(vq => vq.Padre == item.Id_Menu && vq.Estado == "A").OrderBy(vq => vq.Orden).ToList();
                            if (opciones_nivel2.Count() != 0)
                            {
                                /*var o2 = new Menu_Aux();
                                o2.Id_Menu = item2.Id_Menu;
                                o2.Descripcion_Menu = item2.Menu1;
                                o2.Icono = item2.Icono;
                                o2.IsPadre = true;
                                o2.IsSelected = false;
                                o2.Nivel_Profundidad = 2;
                                o2.Orden = item2.Orden;
                                o2.Padre = item2.Padre;
                                o2.Estado = item2.Estado;

                                MenuFull_Tot.Add(o2);*/
                                foreach (var item2 in opciones_nivel2)
                                {
                                    var opciones_nivel3 = db.Menu.Where(vq => vq.Padre == item2.Id_Menu && vq.Estado == "A").ToList();
                                    if (opciones_nivel3.Count() != 0)
                                    {
                                        /*var o2 = new Menu_Aux();
                                        o2.Id_Menu = item2.Id_Menu;
                                        o2.Descripcion_Menu = item2.Menu1;
                                        o2.Icono = item2.Icono;
                                        o2.IsPadre = true;
                                        o2.IsSelected = false;
                                        o2.Nivel_Profundidad =2;
                                        o2.Orden = item2.Orden;
                                        o2.Padre = item2.Padre;
                                        o2.Estado = item2.Estado;

                                        MenuFull_Tot.Add(o2);*/
                                        foreach (var item3 in opciones_nivel3)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        var o2 = new Menu_Aux();
                                        o2.Id_Menu = item2.Id_Menu;
                                        o2.Descripcion_Menu = item2.Menu1;
                                        o2.Icono = item2.Icono;
                                        o2.IsPadre = false;
                                        o2.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true;
                                        o2.Nivel_Profundidad = 2;
                                        o2.Orden = item2.Orden;
                                        o2.Padre = item2.Padre;
                                        o2.Estado = item2.Estado;

                                        MenuFull_Tot.Add(o2);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var o0 = new Menu_Aux();
                            o0.Id_Menu = item.Id_Menu;
                            o0.Descripcion_Menu = item.Menu1;
                            o0.Icono = item.Icono;
                            o0.IsPadre = false;
                            o0.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true;
                            o0.Nivel_Profundidad = 1;
                            o0.Orden = item.Orden;
                            o0.Padre = item.Padre;
                            o0.Estado = item.Estado;

                            MenuFull_Tot.Add(o0);
                        }
                    }
                    //getItem_Menu(MenuFull_Tot,item);

                    var menuiten = MenuFull_Tot.OrderBy(vq => vq.Orden);
                    ViewBag.RolID = id;
                    ViewBag.NombreRol = db.Roles.Where(vq => vq.Id_Rol == id).SingleOrDefault().Descripcion;
                    ViewBag.opcPermitidas = opcPermitidas;                    

                    return View(MenuFull_Tot);// RedirectToAction("MantRol_Menu", "Mantenimiento");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("MenuDRol_Editar:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        [HttpGet]
        public ActionResult MantMenuDRol_Editar_2(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string usuarioID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();
                try
                {
                    var MenuLI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
                    var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, 0, EmpresaID).ToList();

                    var MenuUsuario = new List<Menu_Aux>();
                    foreach (var item in MenuLI)
                    {
                        bool IsPAdre = item.Action == "" ? true : false;
                        bool IsSelected = false;
                        if (item.Action == "")
                        {
                            var objMenu0 = new Menu_Aux
                            {
                                Id_Menu = item.Id_Menu,
                                Descripcion_Menu = item.Menu1,
                                Icono = item.Icono,
                                IsPadre = true,
                                IsSelected = false,
                                Nivel_Profundidad = 0,
                                Orden = item.Orden,
                                Padre = item.Padre,
                                Estado = item.Estado
                            };
                            MenuUsuario.Add(objMenu0);
                            //var opciones_nivel2 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item.Id_Menu, EmpresaID).ToList();
                            var opciones_nivel2 = db.Menu.Where(vq => vq.Padre == item.Id_Menu && vq.Estado == "A").ToList();
                            if (opciones_nivel2.Count() != 0)
                            {
                                foreach (var item2 in opciones_nivel2)
                                {
                                    //var opciones_nivel3 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item2.Id_Menu, EmpresaID).ToList();
                                    var opciones_nivel3 = db.Menu.Where(vq => vq.Padre == item2.Id_Menu && vq.Estado == "A").ToList();
                                    if (opciones_nivel3.Count() != 0)
                                    {
                                        foreach (var item3 in opciones_nivel3)
                                        {
                                            //al mommento no ay 3 niveles
                                            /*var obj3 = new Menu_Aux
                                            {
                                                Id_Menu = item3.Id_Menu,
                                                Nivel_Profundidad = 3
                                            };*/
                                        }
                                    }
                                    else
                                    {
                                        var objMenu2 = new Menu_Aux
                                        {
                                            Id_Menu = item2.Id_Menu,
                                            Descripcion_Menu = item2.Menu1,
                                            Icono = item2.Icono,
                                            IsPadre = true,
                                            IsSelected = false,
                                            Nivel_Profundidad = 1,
                                            //Orden = item2.,
                                            Padre = item2.Padre,
                                            //Estado = item2.Estado
                                        };
                                        MenuUsuario.Add(objMenu2);
                                        //< a href = "@Url.Action(item2.Action, item2.controlador)" id = "@item2.Action" >< i class="@item2.Icono"></i> <span>@item2.Menu</span></a>
                                    }
                                }
                            }
                        }
                        else
                        {
                            var objMenu1 = new Menu_Aux
                            {
                                Id_Menu = item.Id_Menu,
                                Descripcion_Menu = item.Menu1,
                                Icono = item.Icono,
                                IsPadre = true,
                                IsSelected = false,
                                Nivel_Profundidad = 0,
                                Orden = item.Orden,
                                Padre = item.Padre,
                                Estado = item.Estado
                            };
                            MenuUsuario.Add(objMenu1);
                            //< a href = "@Url.Action(item.Action, item.controlador)" id = "@item.Action" > < i class="@item.Icono"></i> <span id = "color" > @item.Menu </ span ></ a >
                        }
                    }

                    var menuiten = MenuUsuario.OrderBy(vq => vq.Orden);
                    return View(MenuUsuario);// RedirectToAction("MantRol_Menu", "Mantenimiento");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("MenuDRol_Editar:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        [HttpGet]
        public ActionResult MantMenuDRol_Eliminar(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {                   
                    string menu_rol_eliminar = "delete from MenuRol where Id_Rol=" + id;
                    string roles_usuario_eliminar = "delete from Roles_Usuario where Id_Rol=" + id;
                    string rol_eliminar = "delete from Roles where Id_Rol=" + id;

                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                       var o1= ctx.Database.ExecuteSqlCommand(menu_rol_eliminar);
                        var o2 = ctx.Database.ExecuteSqlCommand(roles_usuario_eliminar);
                        var o3 = ctx.Database.ExecuteSqlCommand(rol_eliminar);
                    }

                    TempData["mensaje_actualizado"] = "Item eliminado del rol";
                    //return RedirectToAction("Materiales_Solicitud", new { id = mT_SolicitudMateriales.Id_Solicitud }); /Mantenimiento/
                    return RedirectToAction("MantMenuDRol", "Mantenimiento");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("MenuDRol_Eliminar:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        //Guardar
        [HttpPost]
        public ActionResult GuardarMenuSelected(string[] MenuSelectedLI, int RolID, string nombre_Rol)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        using (var ctx = new BD_QUIMIPACEntities())
                        {
                            var nreg = ctx.Database.ExecuteSqlCommand("DELETE FROM MENUROL WHERE ID_ROL ="+RolID);

                            for (int i = 0; i < MenuSelectedLI.Length; i++)
                            {
                                var objMR = new MenuRol
                                {
                                    Id_Menu = int.Parse(MenuSelectedLI[i]),
                                    Id_Rol = RolID,
                                    Estado = "A",
                                    Id_MenuRol = 0
                                };
                                db.MenuRol.Add(objMR);
                                db.SaveChanges();
                            }
                            var Roles_ex = db.Roles.Find(RolID);//.Where(vq => vq.Id_Rol == RolID).ToList();
                            if (Roles_ex != null)//.Count() > 0)
                            {
                                Roles_ex.Descripcion = nombre_Rol;
                                db.Entry(Roles_ex).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        TempData["mensaje_correcto"] = "Registro guardado correctamente";
                        return RedirectToAction("MantMenuDRol", "Mantenimiento");
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("MantMenuDRol","Mantenimiento");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("AgregarRol:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        //?
        [HttpPost]
        public ActionResult EditRol(int RolID_Edit, string NombreRolEdit)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string paramRol = NombreRolEdit.ToLower().Trim();
                        if (paramRol == "")
                        {
                            TempData["mensaje_error"] = "Nombre rol incorrectos";
                        }
                        else
                        {
                            var AuxRol = db.Roles.Where(vq => vq.Descripcion.ToLower() == paramRol).ToList();
                            if (AuxRol.Count() > 0)
                            {
                                string msn = string.Empty;
                                if (AuxRol.Any(vq => vq.Estado == "I"))
                                {
                                    msn = ", por favor habilitar el estado.";
                                }
                                TempData["mensaje_error"] = "Rol ya existe" + msn;
                                //return RedirectToAction("MantMenuDRol", "Mantenimiento");
                            }
                            else
                            {
                                //var objRol = new Roles { 
                                //    Descripcion = Rol.Trim(),
                                //    Estado = "A"
                                //};
                                var rol = db.Roles.Find(RolID_Edit);
                                if (rol != null)
                                {
                                    rol.Descripcion = NombreRolEdit;
                                    db.Entry(rol).State = EntityState.Modified;
                                    db.SaveChanges();
                                    TempData["mensaje_correcto"] = "Registro guardado correctamente";
                                }
                                //return RedirectToAction("MantMenuDRol", "Mantenimiento");
                            }

                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("MantTabla");
                    }
                    return RedirectToAction("MantMenuDRol", "Mantenimiento");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("AgregarRol:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        //ROLES USUARIOS **
        [HttpGet]
        //MantMenuRolFronter
        public ActionResult MantRolUsuario(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var idUsuario = System.Web.HttpContext.Current.Session["usuario"];
                    if (idUsuario != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string EmpresaID = empresa_id.ToString();
                        /*
                        var query = from ru in db.Roles_Usuario
                                    join r in db.Roles on ru.Id_Rol equals r.Id_Rol
                                    where r.Estado == "A" && ru.Estado == "A"
                                    orderby r.Descripcion, ru.Id_Usuario
                                    select new Permisos_Rol
                                    {
                                        Descripcion = r.Descripcion,
                                        Id_Rol = r.Id_Rol,
                                        Estado = r.Estado,
                                        Estado_Usuario = ru.Estado,
                                        Id_Rol_Usuario = ru.Id_Rol_Usuario,
                                        Id_Rol_InUsuario = ru.Id_Rol_Usuario,
                                        Id_Usuario = ru.Id_Usuario
                                        //rolesusuarioli =
                                    };


                        var TotalUsuario_LI = db.sp_LINK_ConsultaUsuarios("ALL", EmpresaID).ToList();
                        var RolesUsuarioID_LI = db.Roles_Usuario.Where(vq => vq.Estado == "A").Select(vq => vq.Id_Usuario);// new { user_id = vq.Id_Usuario.ToString() }).ToList();

                        var UsuariosConRol = TotalUsuario_LI.Where(vq => RolesUsuarioID_LI.Contains(vq.user_id)).ToList();
                        var UsuariosSinRol = TotalUsuario_LI.Where(vq => !RolesUsuarioID_LI.Contains(vq.user_id)).ToList();

                        */


                        /*var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();
                        listacliente = listacliente.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                        */

                        /*
                        ViewBag.RolesLI = db.Roles.OrderBy(vq => vq.Descripcion).ToList();
                        ViewBag.TablaGeneralHtml_LI = query.ToList();
                        */
                        var TodosUsu = db.sp_LINK_ConsultaUsuarios("ALL", EmpresaID).ToList();

                        var UsuarioNoDisponible = db.Roles_Usuario.Where(vq => vq.Estado == "A").Select(vq => vq.Id_Usuario).ToList();
                        var UsuarioConRol_xID = db.Roles_Usuario.Where(vq => vq.Estado == "A" && vq.Id_Rol == id).Select(vq => vq.Id_Usuario).ToList();

                        var UsuarioConRol = TodosUsu.Where(vq => UsuarioConRol_xID.Contains(vq.user_id)).ToList();
                        var UsuarioSinRol = TodosUsu.Where(vq => !UsuarioNoDisponible.Contains(vq.user_id)).ToList();


                        ViewBag.UsuarioID = new SelectList(UsuarioSinRol, "user_id", "user_descrip");
                        ViewBag.UsuarioConRol_LI = UsuarioConRol;

                        ViewBag.RolID = id;
                        ViewBag.NombreRol = db.Roles.Where(vq => vq.Id_Rol == id).SingleOrDefault().Descripcion;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("IniciarSesion", "HomeController");

                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        //POST
        [HttpPost]
        public ActionResult MantRolUsuario(int id, string UsuarioID)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var idUsuario = System.Web.HttpContext.Current.Session["usuario"];
                    if (idUsuario != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string EmpresaID = empresa_id.ToString();


                        var rolUsuario = new Roles_Usuario
                        {
                            Id_Rol_Usuario = 0,
                            Estado = "A",
                            Id_Rol = id,
                            Id_Usuario = UsuarioID
                        };
                        db.Roles_Usuario.Add(rolUsuario);
                        db.SaveChanges();

                        //return View();
                        return RedirectToAction("MantRolUsuario", "Mantenimiento");
                    }
                    else
                    {
                        return RedirectToAction("IniciarSesion", "Home");

                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
        //
        [HttpGet]
        public ActionResult MantRolUsuario_Eliminar(int id, string UserID)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        string sql = String.Format("DELETE FROM ROLES_USUARIO WHERE ID_ROL ={0} AND ID_USUARIO ='{1}'", id, UserID);
                        var nReg = db.Database.ExecuteSqlCommand(sql);
                        if (nReg > 0)
                        {
                            TempData["mensaje_actualizado"] = "Registro Eliminado";
                        }
                    }
                    return RedirectToAction("MantRolUsuario", "Mantenimiento", new { id = id });
                }
                catch (Exception)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
        }

        #endregion

        //Guardar
        [HttpPost]
        public ActionResult Fotousuario(string FotoBase64)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string user_id = System.Web.HttpContext.Current.Session["usuario"].ToString();
                        using (var ctx = new BD_QUIMIPACEntities())
                        {
                            // var nreg = ctx.Database.ExecuteSqlCommand("DELETE FROM MENUROL WHERE ID_ROL ={RolID} ");

                        }
                        TempData["mensaje_correcto"] = "Registro guardado correctamente";
                        return RedirectToAction("DatosPersonales", "Mantenimiento");
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("MantMenuDRol", "Mantenimiento");
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("AgregarRol:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }
    }

    /*Mante JSON Prec Referencial*/
    public class TempItems
    {
        /*public int Id_PrecioReferencial { get; set; }
        public Nullable<int> Id_TipoTablaDetalle { get; set; }
        public Nullable<int> Id_Proyecto_Contrato { get; set; }*/

        //public Nullable<int> Id_TipoTrabajo { get; set; }
        /*public string Id_Usuario { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }*/


        public string Id_Item { get; set; }
        public string Unidad { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Fin { get; set; }
        //public string Moneda { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<decimal> Costo { get; set; }
        // public string Estado { get; set; }

    }
    public class TempMensaje
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; }
    }

    /*public class TempMPrePref
    {
        public Nullable<int> Id_TipoTablaDetalle { get; set; }
        public Nullable<int> Id_Proyecto_Contrato { get; set; }
        public Nullable<int> Id_TipoTrabajo { get; set; }
        public string Id_Usuario { get; set; }
        public List<TempItems> Valores { get; set; }
        // public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public string Estado { get; set; }
    }*/
}


