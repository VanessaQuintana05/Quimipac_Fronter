//using Quimipac_.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Mvc;

using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;
using Quimipac_.Repositorio;
using ExcelDataReader;

namespace Quimipac_.Controllers
{
    public class OrdenTrabajoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

        //MATENIMIENTO DE Orden DE TRABAJO
        //public String isValidParametro(DateTime? fechaAsignacion, int? valorFinal,string parametrostring, int Orden_trabajoID, int? ContratoID)

        #region
        [HttpGet]
        public ActionResult OrdenTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaOrdenTrabajo_2 = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString()).ToList();
                //DataBase_Externo dbeo = new DataBase_Externo();

                //foreach (var item in listaOrdenTrabajo)
                //{
                //     var fecha_Lista = dbeo.ReturnFechaActual_SQL();
                //    //var obj = db.MT_OrdenTrabajo.Where(vq=>vq.Fecha_asignacion_grupo_trabajo)

                //    var obj = db.MT_Contrato_Parametro.Where(vq => vq.Id_Contrato == item.Id_contrato);
                //    isValidParametro(item.Fecha_asignacion_grupo_trabajo, item.Valor_Final, item.Parametro, item.Id_OrdenTrabajo, item.Id_contrato);

                //}


                List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> listaOrdenTrabajo = new List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result>();
                List<string> lkAUX = new List<string>();
                foreach (var item in listaOrdenTrabajo_2) //aquiiiiiiiiiii
                {
                    if (item.Fecha_asignacion_grupo_trabajo != null)
                    {


                        sp_Quimipac_ConsultaMT_OrdenTrabajo_Result o = new sp_Quimipac_ConsultaMT_OrdenTrabajo_Result();
                        o.Id_OrdenTrabajo = item.Id_OrdenTrabajo;
                        o.Id_contrato = item.Id_contrato;
                        o.Id_sucursal = item.Id_sucursal;
                        o.Id_campamento = item.Id_campamento;
                        o.Id_tipo_trabajo_recibido = item.Id_tipo_trabajo_recibido;
                        o.Id_tipo_trabajo_ejecutado = item.Id_tipo_trabajo_ejecutado;
                        o.Estado = item.Estado;
                        o.Id_sector = item.Id_sector;
                        o.Id_orden_padre = item.Id_orden_padre;
                        o.Id_estacion = item.Id_estacion;
                        o.Id_usuario = item.Id_usuario;
                        o.Id_entrega_orden_trabajo = item.Id_entrega_orden_trabajo;
                        o.Nivel_prioridad = item.Nivel_prioridad;
                        o.Fecha_registro = item.Fecha_registro;
                        o.Fecha_creacion_cliente = item.Fecha_creacion_cliente;
                        o.Fecha_maxima_ejecucion_cliente = item.Fecha_maxima_ejecucion_cliente;
                        o.Fecha_asignacion_grupo_trabajo = item.Fecha_asignacion_grupo_trabajo;
                        o.Fecha_inicio_ejecucion = item.Fecha_inicio_ejecucion;
                        o.Fecha_fin_ejecucion = item.Fecha_fin_ejecucion;
                        o.Fecha_finalizacion_obra = item.Fecha_finalizacion_obra;
                        o.Fecha_ini_permiso_municipal = item.Fecha_ini_permiso_municipal;
                        o.Fecha_fin_permiso_municipal = item.Fecha_fin_permiso_municipal;
                        o.Fecha_entrega = item.Fecha_entrega;
                        o.Fecha_max_legalizacion = item.Fecha_max_legalizacion;
                        o.Hora_acordada = item.Hora_acordada;
                        o.Id_barrio = item.Id_barrio;
                        o.Direccion = item.Direccion;
                        o.Referencia_direccion = item.Referencia_direccion;
                        o.Identificacion_suscriptor = item.Identificacion_suscriptor;
                        o.Nombre_suscriptor = item.Nombre_suscriptor;
                        o.Tipo_suscriptor = item.Tipo_suscriptor;
                        o.Telefono_suscriptor = item.Telefono_suscriptor;
                        o.Origen = item.Origen;
                        o.Comentario = item.Comentario;
                        o.Porcentaje_avance = item.Porcentaje_avance;
                        o.Tiempo_transcurrido = item.Tiempo_transcurrido;
                        o.Id_planilla = item.Id_planilla;
                        o.Estado_orden_planilla = item.Estado_orden_planilla;
                        o.Codigo_Cliente = item.Codigo_Cliente;
                        o.Interna = item.Interna;
                        o.CodigoInternoContrato = item.CodigoInternoContrato;
                        o.NombreContrato = item.NombreContrato;
                        o.Parametro = item.Parametro;
                        o.Valor_Inicial = item.Valor_Inicial;
                        o.Valor_Final = item.Valor_Final;
                        if (!lkAUX.Contains(item.Id_contrato + "" + item.Id_tipo_trabajo_ejecutado + "" + item.Codigo_Cliente))
                        {
                            //item.Id_contrato+""+ item.Id_tipo_trabajo_ejecutado
                            listaOrdenTrabajo.Add(o);
                            lkAUX.Add(item.Id_contrato + "" + item.Id_tipo_trabajo_ejecutado + "" + item.Codigo_Cliente);
                        }
                    }//fin != null
                }



                List<SelectListItem> lkTipo_Trabajo = new List<SelectListItem>();
                List<SelectListItem> lkCampamento = new List<SelectListItem>();
                List<SelectListItem> lkSucursal = new List<SelectListItem>();
                List<SelectListItem> lkEstacion = new List<SelectListItem>();
                List<SelectListItem> lkSector = new List<SelectListItem>();
                List<SelectListItem> lkContrato = new List<SelectListItem>();
                List<SelectListItem> lkEstado = new List<SelectListItem>();
                List<SelectListItem> lkCliente = new List<SelectListItem>();
                

                DataBase_Externo dbe = new DataBase_Externo();

                var listalkTipo_Trabajo = dbe.ParametroLkTipoTrabajo("Tipo_Trabajo", empresa_id.ToString());
                foreach (var item in listalkTipo_Trabajo)
                {
                    lkTipo_Trabajo.Add(new SelectListItem { Value = Convert.ToString(item.Id_TipoTrabajo), Text = item.Descripcion });
                }

                var listalkCampamento = dbe.ParametroLkCampamento("Campamento", empresa_id.ToString());
                foreach (var item in listalkCampamento)
                {
                    lkCampamento.Add(new SelectListItem { Value = Convert.ToString(item.Id_Campamento), Text = item.Nombre });
                }

                var listalkSucursal = dbe.ParametroLkSucursal("Sucursal", empresa_id.ToString());
                foreach (var item in listalkSucursal)
                {
                    lkSucursal.Add(new SelectListItem { Value = Convert.ToString(item.cod_suc), Text = item.nombre });
                }

                var listalkEstacion = dbe.ParametroLkEstacion("Estacion", empresa_id.ToString());
                foreach (var item in listalkEstacion)
                {
                    lkEstacion.Add(new SelectListItem { Value = Convert.ToString(item.Id_Estacion), Text = item.Direccion });
                }

                var listalkSector = dbe.ParametroLkSector("Sector", empresa_id.ToString());
                foreach (var item in listalkSector)
                {
                    lkSector.Add(new SelectListItem { Value = Convert.ToString(item.Id_Sector), Text = item.Nombre });
                }

                var listalkContrato = dbe.ParametroLkContrato("Contrato", empresa_id.ToString());
                foreach (var item in listalkContrato)
                {
                    lkContrato.Add(new SelectListItem { Value = Convert.ToString(item.Id_Contrato), Text = item.Nombre });
                }

                var listalkEstado = dbe.ParametroLkEstado("Estado", empresa_id.ToString());
                foreach (var item in listalkEstado)
                {
                    lkEstado.Add(new SelectListItem { Value = Convert.ToString(item.Estado), Text = item.Descripcion });
                }

                    var listakClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    foreach (var item in listakClientes)
                    {
                        lkCliente.Add(new SelectListItem { Value = Convert.ToString(item.cod_cli), Text = item.nom_cli });
                    }
                    SelectList selectlkCliente = new SelectList(listakClientes, "cod_cli", "nom_cli");

                //    var listakClientes = dbe.ParametroLkCliente("Cliente", empresa_id.ToString());
                //    foreach (var item in listakClientes)
                //{
                //    lkCliente.Add(new SelectListItem { Value = Convert.ToString(item.cod_cli), Text = item.nom_cli});
                //}
                


                    //lk itemsContratos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    SelectList selectlkTipo_Trabajo = new SelectList(lkTipo_Trabajo, "Value", "Text");
                SelectList selectlkCampamento = new SelectList(lkCampamento, "Value", "Text");
                SelectList selectlkSucursal = new SelectList(lkSucursal, "Value", "Text");
                SelectList selectlkEstacion = new SelectList(lkEstacion, "Value", "Text");
                SelectList selectlkSector = new SelectList(lkSector, "Value", "Text");
                SelectList selectlkContrato = new SelectList(lkContrato, "Value", "Text");
                SelectList selectlkEstado = new SelectList(lkEstado, "Value", "Text");
                //SelectList selectlkCliente = new SelectList(lkCliente, "Value", "Text");





                   

                ViewBag.listalkTipo_Trabajo = selectlkTipo_Trabajo;
                ViewBag.listalkCampamento = selectlkCampamento;
                ViewBag.listalkSucursal = selectlkSucursal;
                ViewBag.listalkEstacion = selectlkEstacion;
                ViewBag.listalkSector = selectlkSector;
                ViewBag.listalkContrato = selectlkContrato;
                ViewBag.listalkEstado = selectlkEstado;
                ViewBag.listaOrdenTrabajo = listaOrdenTrabajo;
                ViewBag.listakClientes = selectlkCliente;
                   


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
        public ActionResult Agregar_OrdenTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                List<SelectListItem> itemsOrdenTrabajo = new List<SelectListItem>();
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                List<SelectListItem> itemstipo_trabajo_recibido = new List<SelectListItem>();
                List<SelectListItem> itemstipo_trabajo_ejecutado = new List<SelectListItem>();
                List<SelectListItem> itemsEstado = new List<SelectListItem>();
                List<SelectListItem> itemssector = new List<SelectListItem>();
                List<SelectListItem> itemsEstacion = new List<SelectListItem>();
                List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                List<SelectListItem> itemsNivelPrioridad = new List<SelectListItem>();
                List<SelectListItem> itemsGruposTrabajo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaOrdenes = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString()).ToList();
                foreach (var orden in listaOrdenes)
                {
                    itemsOrdenTrabajo.Add(new SelectListItem { Value = Convert.ToString(orden.Id_OrdenTrabajo), Text = orden.Codigo_Cliente });
                }

                itemsOrdenTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaOrden = new SelectList(itemsOrdenTrabajo, "Value", "Text");


                var listaContrato = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                foreach (var contrato in listaContrato)
                {
                    itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Nombre });
                }



                SelectList selectlistaContrato = new SelectList(itemsContratos, "Value", "Text");

                var listacampamento = db.sp_Quimipac_ConsultaMT_Campamento(empresa_id.ToString()).ToList();
                foreach (var campamento in listacampamento)
                {
                    itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre });
                }

                itemsCampamento.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaCampamento = new SelectList(itemsCampamento, "Value", "Text");

                var listaTrabajorecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre(2, empresa_id.ToString()).ToList();
                itemstipo_trabajo_recibido.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var trabajorecibido in listaTrabajorecibido)
                {
                    itemstipo_trabajo_recibido.Add(new SelectListItem { Value = Convert.ToString(trabajorecibido.Id_TipoTrabajo), Text = trabajorecibido.Codigo + " | " + trabajorecibido.Descripcion });
                    }

                SelectList selectlistaTrabajoRecibido = new SelectList(itemstipo_trabajo_recibido, "Value", "Text");

                var listaTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre(2, empresa_id.ToString()).ToList();
                itemstipo_trabajo_ejecutado.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var trabajoejecutado in listaTrabajoejecutado)
                {
                    itemstipo_trabajo_ejecutado.Add(new SelectListItem { Value = Convert.ToString(trabajoejecutado.Id_TipoTrabajo), Text = trabajoejecutado.Codigo + " | " + trabajoejecutado.Descripcion });
                    }

                SelectList selectlistaTrabajoEjecutado = new SelectList(itemstipo_trabajo_ejecutado, "Value", "Text");

                var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoTrabajo(empresa_id.ToString()).ToList();
                foreach (var grupotrabajo in listaGrupoTrabajo)
                {
                    itemsGruposTrabajo.Add(new SelectListItem { Value = Convert.ToString(grupotrabajo.Id_GrupoTrabajo), Text = grupotrabajo.Nombre });
                }

                SelectList selectlistaGrupoTrabajo = new SelectList(itemsGruposTrabajo, "Value", "Text");

                var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(9).ToList();
                foreach (var estado in listaEstados)
                {
                    if (estado.Descripcion.Equals("Registrado"))
                    {
                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = true });

                    }
                }

                SelectList selectlistaEstados = new SelectList(itemsEstado, "Value", "Text");

                //  var listaSector = db.MT_Sector.Where(x => x.Estado == "A").ToList();
                var listaSector = db.sp_Quimipac_ConsultaMT_Sector(empresa_id.ToString()).ToList();
                foreach (var sector in listaSector)
                {
                    itemssector.Add(new SelectListItem { Value = Convert.ToString(sector.Id_Sector), Text = sector.Nombre });
                }

                itemssector.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaSector = new SelectList(itemssector, "Value", "Text");

                var listaEstacion = db.MT_Estacion.Where(x => x.Estado == "A").ToList();
                foreach (var estacion in listaEstacion)
                {
                    itemsEstacion.Add(new SelectListItem { Value = Convert.ToString(estacion.Id_Estacion), Text = estacion.Nombre });
                }

                itemsEstacion.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaEstacion = new SelectList(itemsEstacion, "Value", "Text");

                var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                foreach (var sucursal in listaSucursal)
                {
                    itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre });
                }

                itemsSucursal.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");

                var listaNivelPrioridad = db.sp_Quimipac_ConsultaMT_TablaDetalle(11).ToList();
                foreach (var prioridad in listaNivelPrioridad)
                {
                    itemsNivelPrioridad.Add(new SelectListItem { Value = Convert.ToString(prioridad.Id_TablaDetalle), Text = prioridad.Descripcion });
                }

                itemsNivelPrioridad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                SelectList selectlistaPrioridad = new SelectList(itemsNivelPrioridad, "Value", "Text");


                ViewBag.listaOrdenes = selectlistaOrden;
                ViewBag.listaContrato = selectlistaContrato;
                ViewBag.listacampamento = selectlistaCampamento;
                ViewBag.listaTrabajorecibido = selectlistaTrabajoRecibido;
                ViewBag.listaTrabajoejecutado = selectlistaTrabajoEjecutado;
                ViewBag.listaEstados = selectlistaEstados;
                ViewBag.listaSector = selectlistaSector;
                ViewBag.listaEstacion = selectlistaEstacion;
                ViewBag.listaSucursal = selectlistaSucursal;
                ViewBag.listaNivelPrioridad = selectlistaPrioridad;
                ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_OrdenTrabajo([Bind(Include = "Id_contrato, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_entrega_orden_trabajo ,Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio ,Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_Planilla, Estado_orden_planilla, Codigo_Cliente, Interna")] MT_OrdenTrabajo mT_OrdenTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                if (ModelState.IsValid)
                {

                    if (mT_OrdenTrabajo.Fecha_asignacion_grupo_trabajo != null)
                    {

                        var ordenTrabajo = db.MT_OrdenTrabajo.Where(x => x.Id_orden_padre == mT_OrdenTrabajo.Id_orden_padre && x.Codigo_Cliente == mT_OrdenTrabajo.Codigo_Cliente);

                        if (ordenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Orden Trabajo ya existe";
                            return RedirectToAction("OrdenTrabajo");
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
                                mT_OrdenTrabajo.Id_usuario = user_id.ToString();
                                mT_OrdenTrabajo.Fecha_registro = DateTime.Now;
                                mT_OrdenTrabajo.EstadoEditOrden = "A";

                                db.MT_OrdenTrabajo.Add(mT_OrdenTrabajo);
                                db.SaveChanges();

                                DataBase_Externo dbe = new DataBase_Externo();
                                var resp = dbe.InsertarEstadoOrden(mT_OrdenTrabajo.Id_OrdenTrabajo, mT_OrdenTrabajo.Estado);
                                if (resp > 0)
                                {
                                    TempData["mensaje_correcto"] = "Orden Trabajo guardado";
                                }
                                else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }

                                return RedirectToAction("OrdenTrabajo");
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores Incorrectos o asegurese de ingresar la fecha de asignación";
                        return RedirectToAction("OrdenTrabajo");
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("OrdenTrabajo");
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
        public ActionResult Editar_OrdenTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                System.Web.HttpContext.Current.Session["estado_Orden"] = mT_OrdenTrabajo.Estado;
                System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_OrdenTrabajo.Fecha_registro;

                var mt_OrdenTrabajoActividadOrdenFI = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaInicio").ToList();
                var mt_OrdenTrabajoActividadOrdenFF = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaFin").ToList();
                if (mt_OrdenTrabajoActividadOrdenFI.Count > 0)
                {

                    foreach (var ordentrabajoactividadorden in mt_OrdenTrabajoActividadOrdenFI)
                    {
                        if (ordentrabajoactividadorden.Id_Orden_Trabajo == mT_OrdenTrabajo.Id_OrdenTrabajo)
                        {
                            System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] = ordentrabajoactividadorden.Fecha_Ini;

                        }
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] = null;

                }

                if (mt_OrdenTrabajoActividadOrdenFF.Count > 0)
                {

                    foreach (var ordentrabajoactividadordenFF in mt_OrdenTrabajoActividadOrdenFF)
                    {
                        if (ordentrabajoactividadordenFF.Id_Orden_Trabajo == mT_OrdenTrabajo.Id_OrdenTrabajo)
                        {
                            System.Web.HttpContext.Current.Session["FechaFinEjecucion"] = ordentrabajoactividadordenFF.Fecha_Fin;

                        }
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session["FechaFinEjecucion"] = null;

                }


                bool seleccion = false;
                if (mT_OrdenTrabajo != null)
                {
                    List<SelectListItem> itemsOrdenTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsContratos = new List<SelectListItem>();
                    List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                    List<SelectListItem> itemstipo_trabajo_recibido = new List<SelectListItem>();
                    List<SelectListItem> itemstipo_trabajo_ejecutado = new List<SelectListItem>();
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemssector = new List<SelectListItem>();
                    List<SelectListItem> itemsEstacion = new List<SelectListItem>();
                    List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                    List<SelectListItem> itemsNivelPrioridad = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                        //var listaOrdenTrabajo = db.MT_OrdenTrabajo.Where(x => x.Estado == 30 && x.Estado == 31 && x.Estado == 32 && x.Estado == 33).ToList();
                   var listaOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString()).Where(x=> x.Estado == 30 && x.Estado == 31 && x.Estado == 32 && x.Estado == 33).ToList();


                  itemsOrdenTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                    foreach (var ordentrabajo in listaOrdenTrabajo)
                    {

                        if (ordentrabajo.Id_orden_padre == mT_OrdenTrabajo.Id_orden_padre)
                        {
                            seleccion = true;
                        }


                        itemsOrdenTrabajo.Add(new SelectListItem { Value = Convert.ToString(ordentrabajo.Id_OrdenTrabajo), Text = ordentrabajo.Codigo_Cliente, Selected = seleccion });
                    }

                    SelectList selectlistaOrdenTrabajo = new SelectList(itemsOrdenTrabajo, "Value", "Text");

                    var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();

                    foreach (var contrato in listaContratos)
                    {
                        if (contrato.Id_Contrato == mT_OrdenTrabajo.Id_contrato)
                        {
                            seleccion = true;
                        }

                        itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");

                    var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();

                    foreach (var sucursal in listaSucursal)
                    {
                        if (sucursal.cod_suc == mT_OrdenTrabajo.Id_sucursal)
                        {
                            seleccion = true;
                        }

                        itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");

                    var listaCampamento = db.sp_Quimipac_ConsultaMT_Campamento(empresa_id.ToString()).ToList();

                    foreach (var campamento in listaCampamento)
                    {
                        if (campamento.Id_Campamento == mT_OrdenTrabajo.Id_campamento)
                        {
                            seleccion = true;
                        }

                        itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistaCampamento = new SelectList(itemsCampamento, "Value", "Text");

                    //var listaTiposTrabajorecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems(2).ToList(); 

                    var listaTiposTrabajorecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                    foreach (var tipoTrabajo in listaTiposTrabajorecibido)
                    {
                        if (tipoTrabajo.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_recibido)
                        {
                            seleccion = true;
                        }

                        itemstipo_trabajo_recibido.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTiposTrabajorecibido = new SelectList(itemstipo_trabajo_recibido, "Value", "Text");

                    //var listaTiposTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems(2).ToList();

                    var listaTiposTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                    foreach (var tipoTrabajo in listaTiposTrabajoejecutado)
                    {
                        if (tipoTrabajo.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado)
                        {
                            seleccion = true;
                        }

                        itemstipo_trabajo_ejecutado.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTiposTrabajoejecutado = new SelectList(itemstipo_trabajo_ejecutado, "Value", "Text");

                    var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(9).ToList();//pordefecto al sp de la tabla detalle le envio un parametro en este caso 9 por el id_tabla 

                    foreach (var estado in listaEstados)
                    {
                        if (estado.Id_TablaDetalle == mT_OrdenTrabajo.Estado)
                        {
                            seleccion = true;
                        }

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEstados = new SelectList(itemsEstado, "Value", "Text");

                        //var listaSector = db.MT_Sector.Where(x => x.Estado == "A").ToList();
                    var listaSector = db.sp_Quimipac_ConsultaMT_Sector(empresa_id.ToString()).ToList();
                    foreach (var sector in listaSector)
                    {
                        if (sector.Id_Sector == mT_OrdenTrabajo.Id_sector)
                        {
                            seleccion = true;
                        }

                        itemssector.Add(new SelectListItem { Value = Convert.ToString(sector.Id_Sector), Text = sector.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistasector = new SelectList(itemssector, "Value", "Text");



                    var listaEstacion = db.MT_Estacion.Where(x => x.Estado == "A").ToList();

                    foreach (var estacion in listaEstacion)
                    {
                        if (estacion.Id_Estacion == mT_OrdenTrabajo.Id_estacion)
                        {
                            seleccion = true;
                        }

                        itemsEstacion.Add(new SelectListItem { Value = Convert.ToString(estacion.Id_Estacion), Text = estacion.Nombre, Selected = seleccion });
                    }

                    SelectList selectlistaEstacion = new SelectList(itemsEstacion, "Value", "Text");

                    var listaNivelPrioridad = db.sp_Quimipac_ConsultaMT_TablaDetalle(11).ToList();//pordefecto al sp de la tabla detalle le envio un parametro en este caso 9 por el id_tabla 

                    foreach (var prioridad in listaNivelPrioridad)
                    {
                        if (prioridad.Id_TablaDetalle == mT_OrdenTrabajo.Nivel_prioridad)
                        {
                            seleccion = true;
                        }

                        itemsNivelPrioridad.Add(new SelectListItem { Value = Convert.ToString(prioridad.Id_TablaDetalle), Text = prioridad.Descripcion, Selected = seleccion });
                    }
                    SelectList selectlistaNivelPrioridad = new SelectList(itemsNivelPrioridad, "Value", "Text");


                    ViewBag.listaOrdenTrabajo = selectlistaOrdenTrabajo;
                    ViewBag.listaContratos = selectlistaContratos;
                    ViewBag.listaCampamento = selectlistaCampamento;
                    ViewBag.listaTiposTrabajorecibido = selectlistaTiposTrabajorecibido;
                    ViewBag.listaTiposTrabajoejecutado = selectlistaTiposTrabajoejecutado;
                    ViewBag.listaEstados = selectlistaEstados;
                    ViewBag.listaSector = selectlistasector;
                    ViewBag.listaEstacion = selectlistaEstacion;
                    ViewBag.listaSucursal = selectlistaSucursal;
                    ViewBag.listaNivelPrioridad = selectlistaNivelPrioridad;


                    return View(mT_OrdenTrabajo);
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo, Id_contrato, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_entrega_orden_trabajo, Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_Planilla, Estado_orden_planilla, Codigo_Cliente, Interna")] MT_OrdenTrabajo mT_OrdenTrabajoedit)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                //int bandera = 0;
                if (ModelState.IsValid)
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {

                        DataBase_Externo databaseOrden = new DataBase_Externo();

                        int retornaTO = databaseOrden.GetTipoTrabajoOrden(mT_OrdenTrabajoedit.Id_OrdenTrabajo, mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado);

                        if (retornaTO == 1)
                        {
                            mT_OrdenTrabajoedit.Id_usuario = user_id.ToString();
                            mT_OrdenTrabajoedit.Fecha_registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);

                            if (System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] != null)
                            {
                                mT_OrdenTrabajoedit.Fecha_inicio_ejecucion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaInicioEjecucion"]);
                            }

                            if (System.Web.HttpContext.Current.Session["FechaFinEjecucion"] != null)
                            {
                                mT_OrdenTrabajoedit.Fecha_fin_ejecucion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucion"]);
                            }


                            mT_OrdenTrabajoedit.EstadoEditOrden = "A";
                            mT_OrdenTrabajoedit.Estado = Convert.ToInt32(System.Web.HttpContext.Current.Session["estado_Orden"]);

                            db.Entry(mT_OrdenTrabajoedit).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                        else
                        {
                            databaseOrden.InsertarOrdenTrabajoEditNueva(mT_OrdenTrabajoedit.Id_OrdenTrabajo, mT_OrdenTrabajoedit.Id_contrato, mT_OrdenTrabajoedit.Id_sucursal,
                             mT_OrdenTrabajoedit.Id_campamento, mT_OrdenTrabajoedit.Id_tipo_trabajo_recibido, mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado,
                             mT_OrdenTrabajoedit.Estado, mT_OrdenTrabajoedit.Id_sector, mT_OrdenTrabajoedit.Id_orden_padre, mT_OrdenTrabajoedit.Id_estacion, user_id.ToString(),
                             mT_OrdenTrabajoedit.Id_entrega_orden_trabajo, mT_OrdenTrabajoedit.Nivel_prioridad, mT_OrdenTrabajoedit.Fecha_creacion_cliente,
                             mT_OrdenTrabajoedit.Fecha_maxima_ejecucion_cliente, mT_OrdenTrabajoedit.Fecha_asignacion_grupo_trabajo, mT_OrdenTrabajoedit.Fecha_finalizacion_obra,
                             mT_OrdenTrabajoedit.Fecha_ini_permiso_municipal, mT_OrdenTrabajoedit.Fecha_fin_permiso_municipal, mT_OrdenTrabajoedit.Fecha_entrega,
                             mT_OrdenTrabajoedit.Fecha_max_legalizacion, mT_OrdenTrabajoedit.Hora_acordada, mT_OrdenTrabajoedit.Id_barrio, mT_OrdenTrabajoedit.Direccion,
                             mT_OrdenTrabajoedit.Referencia_direccion, mT_OrdenTrabajoedit.Identificacion_suscriptor, mT_OrdenTrabajoedit.Nombre_suscriptor,
                             mT_OrdenTrabajoedit.Tipo_suscriptor, mT_OrdenTrabajoedit.Telefono_suscriptor, mT_OrdenTrabajoedit.Origen, mT_OrdenTrabajoedit.Comentario,
                             mT_OrdenTrabajoedit.Porcentaje_avance, mT_OrdenTrabajoedit.Tiempo_transcurrido, mT_OrdenTrabajoedit.Id_planilla,
                             mT_OrdenTrabajoedit.Estado_orden_planilla, mT_OrdenTrabajoedit.Codigo_Cliente, mT_OrdenTrabajoedit.Interna);
                        }

                        TempData["mensaje_actualizado"] = "Orden Trabajo actualizado";

                        return RedirectToAction("OrdenTrabajo");
                    }
                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("OrdenTrabajo");
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

        public JsonResult GetTiposTrabajo2(int id)
        {
            try
            {
               
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos(empresa_id.ToString(), id).ToList();

                foreach (var tipoTrabajo in listaTiposTrabajo)
                {
                    itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo + " | " + tipoTrabajo.Descripcion });
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

        public int Obtener_Id(string trabajo)
        {
            //10517 - SONDEOS


            return 0;
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try {
            if (excelfile == null || excelfile.ContentLength == 0)
            {

                TempData["mensaje_error"] = "Please select a excel file<br>";
                return RedirectToAction("OrdenTrabajo");

            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                        //TempData["mensaje_correcto"] = "Archivo seleccionado";@"~/Formularios_Actividad/"
                        string path = Server.MapPath(@"~/Excel_OT/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    //read data from excel file



                    var records = new List<OrdenTrabajo_Excel>();
                    using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Excel_OT/"), excelfile.FileName), FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            int iContador = 0;
                            while (reader.Read())
                            {
                                if (iContador != 0)
                                {
                                    
                                    records.Add(new OrdenTrabajo_Excel
                                    {

                                        Identificador = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString(),
                                        Numerador = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                                        Tipo_de_Trabajo = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                                        Estado = reader.GetValue(3) == null ? "" : reader.GetValue(3).ToString(),
                                        Sector_Operativo = reader.GetValue(4) == null ? "" : reader.GetValue(4).ToString(),
                                        Producto = reader.GetValue(5) == null ? "" : reader.GetValue(5).ToString(),
                                        Unidad_de_Trabajo = reader.GetValue(6) == null ? "" : reader.GetValue(6).ToString(),
                                        Estado_Unidad_de_Trabajo = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString(),
                                        Fecha_de_Creación = reader.GetValue(8) == null ? "" : reader.GetValue(8).ToString(),
                                        Fecha_de_Asignación = reader.GetValue(9) == null ? "" : reader.GetValue(9).ToString(),
                                        Tipo_de_Asignación = reader.GetValue(10) == null ? "" : reader.GetValue(10).ToString(),
                                        Fecha_Estimada_de_Ejecución = reader.GetValue(11) == null ? "" : reader.GetValue(11).ToString(),
                                        Fecha_Máxima_para_Legalización = reader.GetValue(12) == null ? "" : reader.GetValue(12).ToString(),
                                        Última_Reprogramación = reader.GetValue(13) == null ? "" : reader.GetValue(13).ToString(),
                                        Fecha_de_Legalización = reader.GetValue(14) == null ? "" : reader.GetValue(14).ToString(),
                                        Inicio_de_Ejecución = reader.GetValue(15) == null ? "" : reader.GetValue(15).ToString(),
                                        Fin_de_Ejecución = reader.GetValue(16) == null ? "" : reader.GetValue(16).ToString(),
                                        Causal_de_Legalización = reader.GetValue(17) == null ? "" : reader.GetValue(17).ToString(),
                                        Personal = reader.GetValue(18) == null ? "" : reader.GetValue(18).ToString(),
                                        Valor_de_la_Orden = reader.GetValue(19) == null ? "" : reader.GetValue(19).ToString(),
                                        Número_de_Veces_Impresa = reader.GetValue(20) == null ? "" : reader.GetValue(20).ToString(),
                                        Intentos_de_Legalización = reader.GetValue(21) == null ? "" : reader.GetValue(21).ToString(),
                                        Anula_Otra_Orden = reader.GetValue(22) == null ? "" : reader.GetValue(22).ToString(),
                                        Tipo_de_Trabajo_Planeado = reader.GetValue(23) == null ? "" : reader.GetValue(23).ToString(),
                                        Nivel_de_Prioridad = reader.GetValue(24) == null ? "" : reader.GetValue(24).ToString(),
                                        Tipo_de_Compromiso = reader.GetValue(25) == null ? "" : reader.GetValue(25).ToString(),
                                        Hora_Acordada = reader.GetValue(26) == null ? "" : reader.GetValue(26).ToString(),
                                        Cita_Confirmada = reader.GetValue(27) == null ? "" : reader.GetValue(27).ToString(),
                                        Consecutivo_Ruta = reader.GetValue(28) == null ? "" : reader.GetValue(28).ToString(),
                                        Ruta = reader.GetValue(29) == null ? "" : reader.GetValue(29).ToString(),
                                        Nombre_Ruta = reader.GetValue(30) == null ? "" : reader.GetValue(30).ToString(),
                                        Dirección = reader.GetValue(31) == null ? "" : reader.GetValue(31).ToString(),
                                        Barrio = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString(),
                                        Ubicación_Geográfica = reader.GetValue(33) == null ? "" : reader.GetValue(33).ToString(),
                                        Cliente = reader.GetValue(34) == null ? "" : reader.GetValue(34).ToString(),
                                        Nombre_Suscriptor = reader.GetValue(35) == null ? "" : reader.GetValue(35).ToString(),
                                        Apellido_Suscriptor = reader.GetValue(36) == null ? "" : reader.GetValue(36).ToString(),
                                        Tipo_de_Cliente = reader.GetValue(37) == null ? "" : reader.GetValue(37).ToString(),
                                        Teléfono_Cliente = reader.GetValue(38) == null ? "" : reader.GetValue(38).ToString(),
                                        Puntaje_Cliente = reader.GetValue(39) == null ? "" : reader.GetValue(39).ToString(),
                                        Esfuerzo_de_la_Orden = reader.GetValue(40) == null ? "" : reader.GetValue(40).ToString(),
                                        Comentario = reader.GetValue(41) == null ? "" : reader.GetValue(41).ToString(),
                                        Tipo_de_Comentario = reader.GetValue(42) == null ? "" : reader.GetValue(42).ToString(),
                                        Unidad_Asociada = reader.GetValue(43) == null ? "" : reader.GetValue(43).ToString(),
                                        Ofertada = reader.GetValue(44) == null ? "" : reader.GetValue(44).ToString(),
                                        Proyecto = reader.GetValue(45) == null ? "" : reader.GetValue(45).ToString(),
                                        Etapa = reader.GetValue(46) == null ? "" : reader.GetValue(46).ToString(),
                                        Estado_de_la_Orden = reader.GetValue(47) == null ? "" : reader.GetValue(47).ToString(),
                                        PARENT_ID = reader.GetValue(48) == null ? "" : reader.GetValue(48).ToString()

                                    });

                                    string Identificador = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString();
                                    string Numerador = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString();
                                    //string Tipo_de_Trabajo = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                    string Estado = reader.GetValue(3) == null ? "" : reader.GetValue(3).ToString();
                                    //string Sector_Operativo = reader.GetValue(4) == null ? "" : reader.GetValue(4).ToString();
                                    string Producto = reader.GetValue(5) == null ? "" : reader.GetValue(5).ToString();
                                    string Unidad_de_Trabajo = reader.GetValue(6) == null ? "" : reader.GetValue(6).ToString();
                                    string Estado_Unidad_de_Trabajo = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString();
                                    string Fecha_de_Creación = reader.GetValue(8) == null ? "" : reader.GetValue(8).ToString();
                                    string Fecha_de_Asignación = reader.GetValue(9) == null ? "" : reader.GetValue(9).ToString();
                                    string Tipo_de_Asignación = reader.GetValue(10) == null ? "" : reader.GetValue(10).ToString();
                                    string Fecha_Estimada_de_Ejecución = reader.GetValue(11) == null ? "" : reader.GetValue(11).ToString();
                                    string Fecha_Máxima_para_Legalización = reader.GetValue(12) == null ? "" : reader.GetValue(12).ToString();
                                    string Última_Reprogramación = reader.GetValue(13) == null ? "" : reader.GetValue(13).ToString();
                                    string Fecha_de_Legalización = reader.GetValue(14) == null ? "" : reader.GetValue(14).ToString();
                                    string Inicio_de_Ejecución = reader.GetValue(15) == null ? "" : reader.GetValue(15).ToString();
                                    string Fin_de_Ejecución = reader.GetValue(16) == null ? "" : reader.GetValue(16).ToString();
                                    string Causal_de_Legalización = reader.GetValue(17) == null ? "" : reader.GetValue(17).ToString();
                                    string Personal = reader.GetValue(18) == null ? "" : reader.GetValue(18).ToString();
                                    string Valor_de_la_Orden = reader.GetValue(19) == null ? "" : reader.GetValue(19).ToString();
                                    string Número_de_Veces_Impresa = reader.GetValue(20) == null ? "" : reader.GetValue(20).ToString();
                                    string Intentos_de_Legalización = reader.GetValue(21) == null ? "" : reader.GetValue(21).ToString();
                                    string Anula_Otra_Orden = reader.GetValue(22) == null ? "" : reader.GetValue(22).ToString();
                                    //string Tipo_de_Trabajo_Planeado = reader.GetValue(23) == null ? "" : reader.GetValue(23).ToString();
                                    //string Nivel_de_Prioridad = reader.GetValue(24) == null ? "" : reader.GetValue(24).ToString();
                                    string Tipo_de_Compromiso = reader.GetValue(25) == null ? "" : reader.GetValue(25).ToString();
                                    //string Hora_Acordada = reader.GetValue(26) == null ? "" : reader.GetValue(26).ToString();
                                    string Cita_Confirmada = reader.GetValue(27) == null ? "" : reader.GetValue(27).ToString();
                                    string Consecutivo_Ruta = reader.GetValue(28) == null ? "" : reader.GetValue(28).ToString();
                                    string Ruta = reader.GetValue(29) == null ? "" : reader.GetValue(29).ToString();
                                    string Nombre_Ruta = reader.GetValue(30) == null ? "" : reader.GetValue(30).ToString();
                                    string Dirección = reader.GetValue(31) == null ? "" : reader.GetValue(31).ToString();
                                    //string Barrio = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString();
                                    string Ubicación_Geográfica = reader.GetValue(33) == null ? "" : reader.GetValue(33).ToString();
                                    string Cliente = reader.GetValue(34) == null ? "" : reader.GetValue(34).ToString();
                                    string Nombre_Suscriptor = reader.GetValue(35) == null ? "" : reader.GetValue(35).ToString();
                                    string Apellido_Suscriptor = reader.GetValue(36) == null ? "" : reader.GetValue(36).ToString();
                                    //string Tipo_de_Cliente = reader.GetValue(37) == null ? "" : reader.GetValue(37).ToString();
                                    string Teléfono_Cliente = reader.GetValue(38) == null ? "" : reader.GetValue(38).ToString();
                                    string Puntaje_Cliente = reader.GetValue(39) == null ? "" : reader.GetValue(39).ToString();
                                    string Esfuerzo_de_la_Orden = reader.GetValue(40) == null ? "" : reader.GetValue(40).ToString();
                                    string Comentario = reader.GetValue(41) == null ? "" : reader.GetValue(41).ToString();
                                    string Tipo_de_Comentario = reader.GetValue(42) == null ? "" : reader.GetValue(42).ToString();
                                    string Unidad_Asociada = reader.GetValue(43) == null ? "" : reader.GetValue(43).ToString();
                                    string Ofertada = reader.GetValue(44) == null ? "" : reader.GetValue(44).ToString();
                                    string Proyecto = reader.GetValue(45) == null ? "" : reader.GetValue(45).ToString();
                                    string Etapa = reader.GetValue(46) == null ? "" : reader.GetValue(46).ToString();
                                    string Estado_de_la_Orden = reader.GetValue(47) == null ? "" : reader.GetValue(47).ToString();
                                    string PARENT_ID = reader.GetValue(48) == null ? "" : reader.GetValue(48).ToString();

                                    string tipo_trabajo = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                    int npos = tipo_trabajo.IndexOf('-');
                                    string variable = tipo_trabajo.Substring(npos + 1);
                                    variable = variable.TrimStart();

                                    string sector = reader.GetValue(4) == null ? "" : reader.GetValue(4).ToString();
                                    int nposs = sector.IndexOf('-');
                                    string variables = sector.Substring(nposs + 1);
                                    variables = variables.TrimStart();
                                    //string[] trabajo_simplificado = reader.GetValue(2).ToString().Split('-');
                                    //string trabajo_ = trabajo_simplificado[0].ToString();
                                    //mt_Proyecto.Fecha_Inicio = DateTime.Parse(fecha);

                                    string tipo_trabajo_planeado = reader.GetValue(23) == null ? "" : reader.GetValue(23).ToString();
                                    int npostp = tipo_trabajo_planeado.IndexOf('-');
                                    string variabletp = tipo_trabajo.Substring(npostp + 1);
                                    variabletp = variabletp.TrimStart();

                                    string nivel_prioridad = reader.GetValue(24) == null ? "" : reader.GetValue(24).ToString();
                                    int nposnp = nivel_prioridad.IndexOf('-');
                                    string variablenp = nivel_prioridad.Substring(nposnp + 1);
                                    variablenp = variablenp.TrimStart();

                                    string barrio = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString();
                                    int nposb = barrio.IndexOf('-');
                                    string variableb = barrio.Substring(nposb + 1);
                                    variableb = variableb.TrimStart();

                                    string tipo_cliente = reader.GetValue(37) == null ? "" : reader.GetValue(37).ToString();
                                    int nposcl = tipo_cliente.IndexOf('-');
                                    string variablecl = tipo_cliente.Substring(nposcl + 1);
                                    variablecl = variablecl.TrimStart();

                                    string Hora_Acordada = reader.GetValue(26) == null ? "" : reader.GetValue(26).ToString();
                                    int nposh = Hora_Acordada.IndexOf(' ');
                                    string variableh = Hora_Acordada.Substring(nposh + 1);
                                    //variableh = variableh.TrimStart();

                                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                                    string usuario = user_id.ToString();

                                    DataBase_Externo database4 = new DataBase_Externo();

                                    database4.IngresarOrdenes_Excel(Identificador, Numerador, Estado, Producto, Unidad_de_Trabajo, Estado_Unidad_de_Trabajo, Fecha_de_Creación, Fecha_de_Asignación, Tipo_de_Asignación, Fecha_Estimada_de_Ejecución, Fecha_Máxima_para_Legalización, Última_Reprogramación, Fecha_de_Legalización, Inicio_de_Ejecución, Fin_de_Ejecución, Causal_de_Legalización, Personal, Valor_de_la_Orden, Número_de_Veces_Impresa, Intentos_de_Legalización, Anula_Otra_Orden, Tipo_de_Compromiso, Cita_Confirmada, Consecutivo_Ruta, Ruta, Nombre_Ruta, Dirección, Ubicación_Geográfica, Cliente, Nombre_Suscriptor, Apellido_Suscriptor, Teléfono_Cliente, Puntaje_Cliente, Esfuerzo_de_la_Orden, Comentario, Tipo_de_Comentario, Unidad_Asociada, Ofertada, Proyecto, Etapa, Estado_de_la_Orden, PARENT_ID, variable, variables, variabletp, variablenp, variableb, variablecl, variableh, usuario);

                                }

                                iContador++;
                            }

                        }
                    }





                    return RedirectToAction("OrdenTrabajo");
                }
                else
                {

                    TempData["mensaje_error"] = "File type is incorrect<br>";
                    return RedirectToAction("OrdenTrabajo");
                }


            }
        }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "File type is incorrect" +e;
                //return RedirectToAction("Error", "Errores");
                return RedirectToAction("OrdenTrabajo");
                Console.Write(e.Message);

            }


}

        #endregion



        //FORMULARIOS DE ORDENTRABAJO
        #region
        [HttpGet]
        public ActionResult Formularios_OT(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                if (mT_OrdenTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_OT"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    var listaFormulariosOT = db.sp_Quimipac_ConsultaMT_OTAnexo(id).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.listaFormulariosOT = listaFormulariosOT;
                    ViewBag.orden_trabajo = orden_trabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
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
        public ActionResult AgregarFormulario_OT()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                List<SelectListItem> itemsTipoP = new List<SelectListItem>();

                var listaTipoOT = db.sp_Quimipac_ConsultaMT_TablaDetalle(51).ToList();
                foreach (var tipo in listaTipoOT)
                {
                    itemsTipoP.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                SelectList selectlistaTipoOT = new SelectList(itemsTipoP, "Value", "Text");


                ViewBag.listaTipoOT = selectlistaTipoOT;

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
        public ActionResult AgregarFormulario_OT([Bind(Include = "Id_Orden_Trabajo,Tipo_Anexo,Enlace_Anexo,Fecha_Registro,Id_Usuario,Observacion,NombreArchivo")] MT_OrdenTrabajo_Anexo mT_FormularioOT, HttpPostedFileBase NombreArchivo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var id_OT = System.Web.HttpContext.Current.Session["id_OT"];

                if (id_OT == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }
                else
                {
                    int idOT = int.Parse(id_OT.ToString());

                    if (ModelState.IsValid)
                    {
                        var FormularioP = db.MT_OrdenTrabajo_Anexo.Where(x => x.NombreArchivo == mT_FormularioOT.NombreArchivo && x.Tipo_Anexo == mT_FormularioOT.Tipo_Anexo);
                        if (FormularioP.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Formulario ya existe";
                            return RedirectToAction("OrdenTrabajo", new { id = idOT });
                        }
                        else
                        {
                            if (NombreArchivo != null)
                            {
                                var ruta_archivo = @"~/Formularios_Actividad/";
                                var ruta_servidor = Path.GetFullPath(ruta_archivo);
                                var extension = Path.GetExtension(NombreArchivo.FileName);

                                int fileSize = NombreArchivo.ContentLength;

                                mT_FormularioOT.Id_Orden_Trabajo = idOT;
                                mT_FormularioOT.NombreArchivo = "";
                                mT_FormularioOT.Fecha_Registro = DateTime.Now;
                                mT_FormularioOT.Enlace_Anexo = ruta_servidor;
                                db.MT_OrdenTrabajo_Anexo.Add(mT_FormularioOT);
                                db.SaveChanges();

                                int scope_identity_id = mT_FormularioOT.Id_OrdenTrabajo_Anexo;

                                string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                                mT_FormularioOT.NombreArchivo = nombreArchivo;

                                NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                                db.Entry(mT_FormularioOT).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Formulario de Orden Trabajo guardado";
                                return RedirectToAction("Formularios_OT", new { id = idOT });
                            }
                            else
                            {
                                return View(mT_FormularioOT);
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Formularios_OT", new { id = idOT });
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


        //este de aqui no se utiliza ya que mtoanexo no tiene estado 
        [HttpGet]
        public ActionResult EliminarFormularioOT(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {


                var id_MTProyecto = System.Web.HttpContext.Current.Session["id_OT"];

                if (id_MTProyecto == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_MTProyecto.ToString());

                    MT_OrdenTrabajo_Anexo mT_Formulario = db.MT_OrdenTrabajo_Anexo.Find(id);

                    if (mT_Formulario != null)
                    {
                        //mT_Formulario.Estado = "E";
                        //db.Entry(mT_Formulario).State = EntityState.Modified;
                        db.MT_OrdenTrabajo_Anexo.Remove(mT_Formulario);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Archivo eliminado";
                        return RedirectToAction("Formularios_OT", new { id = idOrdenTrabajo });
                    }
                    else
                    {
                        TempData["mensaje_correcto"] = "Código no existe";
                        return RedirectToAction("Formularios_OT", new { id = idOrdenTrabajo });
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

        #endregion


        //ACTIVIDADES DE ORDEN DE TRABAJO
        #region
        [HttpGet]
        public ActionResult Actividades_OrdenTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);


                if (mT_OrdenTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_tipoTrabajo"] = mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado;
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;


                    var listaActividades_OrdenTrabajo = db.Sp_MtOrdenTActividad(mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado, mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.listaActividades_OrdenTrabajo = listaActividades_OrdenTrabajo;
                    ViewBag.orden_trabajo = orden_trabajo;
                    return View();

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
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
        public ActionResult Editar_Actividades_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Actividad mT_OrdenActividad = db.MT_OrdenTrabajo_Actividad.Find(id);
                System.Web.HttpContext.Current.Session["id_actividad"] = mT_OrdenActividad.Id_Actividad;

                System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"] = mT_OrdenActividad.Fecha_Ini;
                System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"] = mT_OrdenActividad.Fecha_Fin;
                System.Web.HttpContext.Current.Session["EstadoAct"] = mT_OrdenActividad.EstadoAct;


                bool seleccion = false;
                if (mT_OrdenActividad != null)
                {

                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemsActividad = new List<SelectListItem>();

                    var listaActividad = db.sp_Quimipac_ConsultaMT_Actividad_OrdenTipoTrabajo().ToList();

                    foreach (var actividad in listaActividad)
                    {
                        if (actividad.Id_Actividad == mT_OrdenActividad.Id_Actividad)
                        {
                            seleccion = true;
                        }

                        itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaActividadesOrden = new SelectList(itemsActividad, "Value", "Text");


                    var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();

                    foreach (var estado in listaEstados)
                    {
                        if (estado.Id_TablaDetalle == mT_OrdenActividad.Estado)
                        {
                            seleccion = true;
                        }

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEstadoActividadesOrden = new SelectList(itemsEstado, "Value", "Text");

                    ViewBag.listaActividad = selectlistaActividadesOrden;
                    ViewBag.listaEstados = selectlistaEstadoActividadesOrden;
                    ViewBag.fechaI = mT_OrdenActividad.Fecha_Ini;
                    ViewBag.fechaF = mT_OrdenActividad.Fecha_Fin;


                    return View(mT_OrdenActividad);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = idMantOrdenTrabajo });
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
        public ActionResult Editar_Actividades_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Actividad, Id_Orden_Trabajo, Estado,Orden,Comentario, Fecha_Ini, Fecha_Fin")] MT_OrdenTrabajo_Actividad mT_OrdenActividad)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_actividad = System.Web.HttpContext.Current.Session["id_actividad"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idMActividad = int.Parse(id_actividad.ToString());


                if (ModelState.IsValid)
                {
                    mT_OrdenActividad.Id_Actividad = idMActividad;
                    mT_OrdenActividad.Id_Orden_Trabajo = idOrdenTrabajo;
                    if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"] != null)
                    {
                        mT_OrdenActividad.Fecha_Ini = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"]);
                    }

                    if (System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"] != null)
                    {
                        mT_OrdenActividad.Fecha_Fin = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"]);
                    }
                    mT_OrdenActividad.EstadoAct = Convert.ToString(System.Web.HttpContext.Current.Session["EstadoAct"]);


                    db.Entry(mT_OrdenActividad).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Actividad actualizada";

                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult IniciarActividadOrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Actividad mT_OrdenActividad = db.MT_OrdenTrabajo_Actividad.Find(id);

                if (mT_OrdenActividad.Fecha_Ini == null)
                {

                    mT_OrdenActividad.Fecha_Ini = DateTime.Now;
                    mT_OrdenActividad.Estado = 68;
                    //mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;

                    var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                    //var fecha = db.MT_OrdenTrabajo.Where(x => x.Fecha_inicio_ejecucion == mT_OrdenTrabajo.Fecha_inicio_ejecucion);

                    //if (fecha.Count() > 0)
                    //{
                    //    mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;
                    //}

                    if (mT_OrdenTrabajo.Fecha_inicio_ejecucion == null)
                    {

                        mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;
                    }


                    db.Entry(mT_OrdenActividad).State = EntityState.Modified;
                    db.Entry(mT_OrdenTrabajo).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    TempData["mensaje_actualizado"] = "Orden Actividad Ya fue iniciada";
                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                }

                TempData["mensaje_actualizado"] = "Orden Actividad Iniciada";
                return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        [HttpGet]
        public ActionResult TerminarActividadOrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Actividad mT_OrdenActividadEstado = db.MT_OrdenTrabajo_Actividad.Find(id);
                System.Web.HttpContext.Current.Session["id_actividad"] = mT_OrdenActividadEstado.Id_Actividad;
                System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] = mT_OrdenActividadEstado.Fecha_Ini;
                System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] = mT_OrdenActividadEstado.Fecha_Fin;

                bool seleccion = false;
                if (mT_OrdenActividadEstado != null)
                {

                    List<SelectListItem> itemsEstado = new List<SelectListItem>();

                    //var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();
                    var listaEstados = db.MT_TablaDetalle.Where(x => x.Id_Padre == 69).ToList();


                    foreach (var estado in listaEstados)
                    {
                        if (estado.Id_TablaDetalle == mT_OrdenActividadEstado.Estado)
                        {
                            seleccion = true;
                        }

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEstadoActividadesOrden = new SelectList(itemsEstado, "Value", "Text");


                    ViewBag.listaEstados = selectlistaEstadoActividadesOrden;
                    //ViewBag.fechaI = mT_OrdenActividad.Fecha_Ini;
                    //ViewBag.fechaF = mT_OrdenActividad.Fecha_Fin;


                    return View(mT_OrdenActividadEstado);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                    }


                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        public ActionResult TerminarActividadOrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Actividad, Id_Orden_Trabajo, Estado")] MT_OrdenTrabajo_Actividad mT_OrdenActividadFinalizada)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_actividad = System.Web.HttpContext.Current.Session["id_actividad"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idMActividad = int.Parse(id_actividad.ToString());


                if (ModelState.IsValid)
                {
                    if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] != null && System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] == null)
                    {
                        mT_OrdenActividadFinalizada.Id_Actividad = idMActividad;
                        mT_OrdenActividadFinalizada.Id_Orden_Trabajo = idOrdenTrabajo;

                        var lkProyProrroga = db.MT_OrdenTrabajo_Actividad.Where(x => x.Id_OrdenTrabajo_Actividad == mT_OrdenActividadFinalizada.Id_OrdenTrabajo_Actividad);
                        if (lkProyProrroga.Count() > 0)
                        {
                            foreach (var itemProrroga in lkProyProrroga)
                            {
                                itemProrroga.Estado = mT_OrdenActividadFinalizada.Estado;
                                itemProrroga.Fecha_Fin = DateTime.Now;
                                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                mT_OrdenTrabajo.Fecha_fin_ejecucion = DateTime.Now;
                            }
                            db.SaveChanges();


                        }
                    }
                    else if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] != null && System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] != null)
                    {
                        mT_OrdenActividadFinalizada.Id_Orden_Trabajo = idOrdenTrabajo;
                        TempData["mensaje_actualizado"] = "Actividad ya fue iniciada y terminada";

                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividadFinalizada.Id_Orden_Trabajo });
                    }
                    else
                    {
                        mT_OrdenActividadFinalizada.Id_Orden_Trabajo = idOrdenTrabajo;
                        TempData["mensaje_actualizado"] = "Actividad No ha sido iniciada";

                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividadFinalizada.Id_Orden_Trabajo });

                    }

                    //db.Entry(mT_OrdenActividadFinalizada).State = EntityState.Modified;
                    //db.SaveChanges();
                    //DataBase_Externo database4 = new DataBase_Externo();

                    //    database4.ActualizarActividadOrden(mT_OrdenActividadFinalizada.Id_OrdenTrabajo_Actividad, mT_OrdenActividadFinalizada.Id_Actividad, mT_OrdenActividadFinalizada.Id_Orden_Trabajo, mT_OrdenActividadFinalizada.Estado);


                    TempData["mensaje_actualizado"] = "Actividad actualizada";

                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividadFinalizada.Id_Orden_Trabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividadFinalizada.Id_Orden_Trabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //FORMULARIOS DE ACTIVIDAD DE OT
        #region
        [HttpGet]
        public ActionResult Formularios_Actividad(int id)
        {
            try
            {
                MT_OrdenTrabajo_Actividad mT_Actividad = db.MT_OrdenTrabajo_Actividad.Find(id);


                if (mT_Actividad != null)
                {
                    System.Web.HttpContext.Current.Session["id_MTActividad"] = mT_Actividad.Id_OrdenTrabajo_Actividad;
                    //var listaFormularios = db.MT_OrdenTrabajo_Formulario.Where(x => x.Id_Orden_Trabajo_Detalle == mT_Actividad.Id_OrdenTrabajo_Actividad).ToList();
                    var listaFormularios = db.sp_Quimipac_ConsultaMT_OrdenTrabajoFormulario(id).ToList();
                    //var actividad_orden = mT_Actividades;
                    ViewBag.idOrdenTrabajo = mT_Actividad.Id_Orden_Trabajo;
                    ViewBag.listaFormularios = listaFormularios;
                    //ViewBag.actividad_orden = actividad_orden;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_Actividad.Id_OrdenTrabajo_Actividad });
                }


            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public FileResult DescargarArchivoFormularioA(string nombre)
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
        public ActionResult AgregarFormulario_Actividad([Bind(Include = "Id_Orden_Trabajo_Detalle,Enlace_Formulario")] MT_OrdenTrabajo_Formulario mT_Formulario, HttpPostedFileBase NombreArchivo)
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


                        var formulario = db.MT_OrdenTrabajo_Formulario.Where(x => x.Id_OrdenTrabajo_Formulario == mT_Formulario.Id_OrdenTrabajo_Formulario && x.Id_Orden_Trabajo_Detalle == mT_Formulario.Id_Orden_Trabajo_Detalle);
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

                                mT_Formulario.Id_Orden_Trabajo_Detalle = idMTActividad;
                                mT_Formulario.NombreArchivo = "";

                                mT_Formulario.Enlace_Formulario = ruta_servidor;
                                db.MT_OrdenTrabajo_Formulario.Add(mT_Formulario);
                                db.SaveChanges();

                                int scope_identity_id = mT_Formulario.Id_OrdenTrabajo_Formulario;

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
            catch (Exception e)
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

                    MT_OrdenTrabajo_Formulario mT_Formulario = db.MT_OrdenTrabajo_Formulario.Find(id);

                    if (mT_Formulario != null)
                    {
                        //mT_Formulario.Estado = "E";
                        //db.Entry(mT_Formulario).State = EntityState.Modified;
                        db.MT_OrdenTrabajo_Formulario.Remove(mT_Formulario);
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
        #endregion

        //ASIGNAR GRUPO TRABAJO
        #region
        [HttpGet]
        public ActionResult Asignar_OrdenTrabajoIntegrante(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);

                if (mT_OrdenTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;

                    List<SelectListItem> itemsGruposTrabajo = new List<SelectListItem>();

                    var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo().ToList();
                    foreach (var grupotrabajo in listaGrupoTrabajo)
                    {
                        itemsGruposTrabajo.Add(new SelectListItem { Value = Convert.ToString(grupotrabajo.Id_GrupoTrabajo), Text = grupotrabajo.Nombre });
                    }


                    SelectList selectlistaGrupoTrabajo = new SelectList(itemsGruposTrabajo, "Value", "Text");

                    ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;
                    ViewBag.idMTOrdenTrabajo = mT_OrdenTrabajo.Id_OrdenTrabajo;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Editar_OrdenTrabajo");

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Asignar_OrdenTrabajoIntegrante([Bind(Include = "Fecha_Inicio, Fecha_Fin, Estado, Id_GrupoTrabajo")] MT_OrdenTrabajo_Integrante mT_OrdenTrabajoIntegrante)
        {

            try
            {
                //var count = context.MyContainer.Where(o => o.ID == '1').SelectMany(o => o.MyTable).Count()
                //var count = db.MT_GrupoTrabajoIntegrante.Where(o => o.Id_GrupoTrabajo == mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo).SelectMany(o => o.Id_GrupoTrabajo).Count()

                IQueryable<MT_GrupoTrabajoIntegrante> records = db.MT_GrupoTrabajoIntegrante.Where(o => o.Id_GrupoTrabajo == mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo);
                var count = records.Count();

                IQueryable<MT_GrupoTrabajoEquipo> records2 = db.MT_GrupoTrabajoEquipo.Where(o => o.Id_GrupoTrabajo == mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo);
                var count2 = records2.Count();


                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        var ordengrupotrabajo = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_GrupoTrabajo == mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo && x.Fecha_Inicio == mT_OrdenTrabajoIntegrante.Fecha_Inicio && x.Fecha_Fin == mT_OrdenTrabajoIntegrante.Fecha_Fin && x.Id_Orden_Trabajo == idOrdenTrabajo);
                        if (ordengrupotrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Código ya existe";
                            return RedirectToAction("Editar_OrdenTrabajo", new { id = idOrdenTrabajo });
                        }
                        else
                        {
                            // Id_OrdenTrabajo_Integrante] ,[Id_Orden_Trabajo],[Id_Persona],[Rol],[Fecha_Inicio],[Fecha_Fin],[Estado],[Id_GrupoTrabajo];
                            for (int i = 0; i < count; i++)
                            {


                                mT_OrdenTrabajoIntegrante.Id_Orden_Trabajo = idOrdenTrabajo;
                                mT_OrdenTrabajoIntegrante.Estado = mT_OrdenTrabajoIntegrante.Estado.Substring(0, 1);
                                db.MT_OrdenTrabajo_Integrante.Add(mT_OrdenTrabajoIntegrante);

                                db.SaveChanges();
                            }
                            DataBase_Externo database3 = new DataBase_Externo();
                            int variable = database3.Verificar_EquipoTrabajoOrden(idOrdenTrabajo, mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo, mT_OrdenTrabajoIntegrante.Fecha_Inicio, mT_OrdenTrabajoIntegrante.Fecha_Fin);
                            if (variable == 0)
                            {
                                database3.InsertarEquipoOrdenTrabajo(idOrdenTrabajo, count2);

                            }

                            return RedirectToAction("Editar_OrdenTrabajo", new { id = idOrdenTrabajo });

                        }



                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Editar_OrdenTrabajo", new { id = idOrdenTrabajo });
                    }

                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion
        //GRUPO TRABAJO ORDEN INTEGRANTE
        #region


        [HttpGet]
        public ActionResult Integrantes_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                //MT_OrdenTrabajo_Integrante mt_OrdenIntegrante = db.MT_OrdenTrabajo_Integrante.Find(id);
                var mT_OrdenTrabajoIntegrante = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajo.Id_OrdenTrabajo);


                if (mT_OrdenTrabajoIntegrante != null)
                {
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    //System.Web.HttpContext.Current.Session["id_GrupoTrabajo"] = mt_OrdenIntegrante.Id_Orden_Trabajo;
                    //var ordengrupotrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];
                    //int idGrupoOrdenTrabajo = int.Parse(ordengrupotrabajo.ToString());

                    foreach (var itemProrroga in mT_OrdenTrabajoIntegrante)
                    {
                        System.Web.HttpContext.Current.Session["id_GrupoTrabajo"] = itemProrroga.Id_GrupoTrabajo;
                    }

                    var listaIntegrantes_OrdenTrabajo = db.sp_Quimipac_ConsultaInsMT_IntegranteOrdenTrabajo(mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaIntegrantes_OrdenTrabajo = listaIntegrantes_OrdenTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
                }

            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "No se ha asignado un grupo de trabajo";
                return RedirectToAction("OrdenTrabajo");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Integrantes_OrdenTrabajo()
        {
            try
            {
                List<SelectListItem> itemsPersona = new List<SelectListItem>();
                List<SelectListItem> itemsRol = new List<SelectListItem>();

                var listaPersona = db.sp_LINK_ConsultaPersonas().ToList();
                foreach (var persona in listaPersona)
                {
                    itemsPersona.Add(new SelectListItem { Value = Convert.ToString(persona.id_persona), Text = persona.Nombres_Completos });
                }

                SelectList selectlistaIntegrante = new SelectList(itemsPersona, "Value", "Text");


                var listaRol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                foreach (var rol in listaRol)
                {
                    itemsRol.Add(new SelectListItem { Value = Convert.ToString(rol.Id_TablaDetalle), Text = rol.Descripcion });
                }

                SelectList selectlistaRol = new SelectList(itemsRol, "Value", "Text");




                ViewBag.listaPersona = selectlistaIntegrante;
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
        public ActionResult Agregar_Integrantes_OrdenTrabajo([Bind(Include = "Id_Persona,Rol,Fecha_Inicio,Fecha_Fin,Estado")] MT_OrdenTrabajo_Integrante mT_OrdenIntegrante)
        {
            try
            {
                var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_GrupoTrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];

                if (id_MantOrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                    int idGrupoTrabajo = int.Parse(id_GrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_OrdenIntegrante.Id_Orden_Trabajo = idMantOrdenTrabajo;
                        mT_OrdenIntegrante.Id_GrupoTrabajo = idGrupoTrabajo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == mT_OrdenIntegrante.Id_Orden_Trabajo && x.Id_GrupoTrabajo == mT_OrdenIntegrante.Id_GrupoTrabajo && x.Id_Persona == mT_OrdenIntegrante.Id_Persona);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Integrante ya existe";
                            return RedirectToAction("Integrantes_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                        }
                        else
                        {

                            mT_OrdenIntegrante.Id_GrupoTrabajo = idGrupoTrabajo;
                            mT_OrdenIntegrante.Id_Orden_Trabajo = idMantOrdenTrabajo;
                            mT_OrdenIntegrante.Estado = mT_OrdenIntegrante.Estado.Substring(0, 1);


                            db.MT_OrdenTrabajo_Integrante.Add(mT_OrdenIntegrante);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Integrante guardado";
                            return RedirectToAction("Integrantes_OrdenTrabajo", new { id = id_MantOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Integrantes_OrdenTrabajo", new { id = id_MantOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Editar_Integrantes_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Integrante mT_OrdenIntegrante = db.MT_OrdenTrabajo_Integrante.Find(id);
                System.Web.HttpContext.Current.Session["id_GrupoTrabajo"] = mT_OrdenIntegrante.Id_GrupoTrabajo;

                bool seleccion = false;
                if (mT_OrdenIntegrante != null)
                {

                    List<SelectListItem> itemsPersona = new List<SelectListItem>();
                    List<SelectListItem> itemsRol = new List<SelectListItem>();

                    var listaPersona = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var persona in listaPersona)
                    {

                        if (persona.id_persona == mT_OrdenIntegrante.Id_Persona)
                        {
                            seleccion = true;
                        }


                        itemsPersona.Add(new SelectListItem { Value = Convert.ToString(persona.id_persona), Text = persona.Nombres_Completos, Selected = seleccion });
                    }

                    SelectList selectlistaIntegrantesOrden = new SelectList(itemsPersona, "Value", "Text");


                    var listaRol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                    foreach (var rol in listaRol)
                    {

                        if (rol.Id_TablaDetalle == mT_OrdenIntegrante.Rol)
                        {
                            seleccion = true;
                        }


                        itemsRol.Add(new SelectListItem { Value = Convert.ToString(rol.Id_TablaDetalle), Text = rol.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaRol = new SelectList(itemsRol, "Value", "Text");

                    ViewBag.listaPersona = selectlistaIntegrantesOrden;
                    ViewBag.listaRol = selectlistaRol;

                    return View(mT_OrdenIntegrante);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Integrantes_OrdenTrabajo", new { id = idMantOrdenTrabajo });
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
        public ActionResult Editar_Integrantes_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Integrante,Id_Persona,Rol,Fecha_Inicio,Fecha_Fin,Estado")] MT_OrdenTrabajo_Integrante mT_OrdenIntegrante)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_GrupoTrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idGrupoTrabajo = int.Parse(id_GrupoTrabajo.ToString());

                //int bandera = 0;
                if (ModelState.IsValid)
                {

                    mT_OrdenIntegrante.Id_GrupoTrabajo = idGrupoTrabajo;
                    mT_OrdenIntegrante.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenIntegrante.Estado = mT_OrdenIntegrante.Estado.Substring(0, 1);

                    db.Entry(mT_OrdenIntegrante).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Integrante actualizado";

                    return RedirectToAction("Integrantes_OrdenTrabajo", new { id = mT_OrdenIntegrante.Id_Orden_Trabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Integrantes_OrdenTrabajo", new { id = mT_OrdenIntegrante.Id_Orden_Trabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Eliminar_IntegranteOrdenTrabajo(int id)
        {
            try
            {

                MT_OrdenTrabajo_Integrante mT_OrdenItegrante = db.MT_OrdenTrabajo_Integrante.Find(id);

                mT_OrdenItegrante.Estado = "E";

                db.Entry(mT_OrdenItegrante).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Integrante Orden Trabajo Eliminado";
                return RedirectToAction("Integrantes_OrdenTrabajo", new { id = mT_OrdenItegrante.Id_Orden_Trabajo });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }
        #endregion

        //GRUPO TRABAJO ORDEN EQUIPO
        #region

        [HttpGet]
        public ActionResult Equipos_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                var mT_OrdenTrabajoEquipo = db.MT_OrdenTrabajo_Equipo.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajo.Id_OrdenTrabajo);


                if (mT_OrdenTrabajoEquipo != null)
                {
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    //var ordengrupotrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];
                    // int idGrupoOrdenTrabajo = int.Parse(ordengrupotrabajo.ToString());

                    foreach (var itemProrroga in mT_OrdenTrabajoEquipo)
                    {
                        System.Web.HttpContext.Current.Session["id_GrupoTrabajoE"] = itemProrroga.Id_GrupoTrabajo;
                    }

                    var listaEquiposOrdenTrabajo = db.sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo(mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaEquiposOrdenTrabajo = listaEquiposOrdenTrabajo;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
                }

            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "No se ha asignado un grupo de trabajo";
                return RedirectToAction("OrdenTrabajo");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Equipos_OrdenTrabajo()
        {
            try
            {
                List<SelectListItem> itemsEquipo = new List<SelectListItem>();


                var listaEquipo = db.sp_Quimipac_ConsultaMt_Equipo().ToList();
                foreach (var equipo in listaEquipo)
                {
                    itemsEquipo.Add(new SelectListItem { Value = Convert.ToString(equipo.Id_Equipo), Text = equipo.Descripcion });
                }

                SelectList selectlistaEquipo = new SelectList(itemsEquipo, "Value", "Text");



                ViewBag.listaEquipo = selectlistaEquipo;


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Equipos_OrdenTrabajo([Bind(Include = "Id_Equipo,Fecha_Inicio,Fecha_Fin,Estado")] MT_OrdenTrabajo_Equipo mT_OrdenEquipo)
        {
            try
            {
                var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_GrupoTrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];

                if (id_MantOrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                    int idGrupoTrabajo = int.Parse(id_GrupoTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_OrdenEquipo.Id_Orden_Trabajo = idMantOrdenTrabajo;
                        mT_OrdenEquipo.Id_GrupoTrabajo = idGrupoTrabajo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Equipo.Where(x => x.Id_Orden_Trabajo == mT_OrdenEquipo.Id_Orden_Trabajo && x.Id_GrupoTrabajo == mT_OrdenEquipo.Id_GrupoTrabajo && x.Id_Equipo == mT_OrdenEquipo.Id_Equipo);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Equipo ya existe";
                            return RedirectToAction("Equipos_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                        }
                        else
                        {

                            mT_OrdenEquipo.Id_GrupoTrabajo = idGrupoTrabajo;
                            mT_OrdenEquipo.Id_Orden_Trabajo = idMantOrdenTrabajo;
                            mT_OrdenEquipo.Estado = mT_OrdenEquipo.Estado.Substring(0, 1);


                            db.MT_OrdenTrabajo_Equipo.Add(mT_OrdenEquipo);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Equipo guardado";
                            return RedirectToAction("Equipos_OrdenTrabajo", new { id = id_MantOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Equipos_OrdenTrabajo", new { id = id_MantOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Equipos_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Equipo mT_OrdenEquipo = db.MT_OrdenTrabajo_Equipo.Find(id);
                System.Web.HttpContext.Current.Session["id_GrupoTrabajo"] = mT_OrdenEquipo.Id_GrupoTrabajo;

                bool seleccion = false;
                if (mT_OrdenEquipo != null)
                {

                    List<SelectListItem> itemsEquipo = new List<SelectListItem>();


                    var listaEquipo = db.sp_Quimipac_ConsultaMt_Equipo().ToList();
                    foreach (var equipo in listaEquipo)
                    {
                        itemsEquipo.Add(new SelectListItem { Value = Convert.ToString(equipo.Id_Equipo), Text = equipo.Descripcion });
                    }

                    foreach (var equipo in listaEquipo)
                    {

                        if (equipo.Id_Equipo == mT_OrdenEquipo.Id_Equipo)
                        {
                            seleccion = true;
                        }


                        itemsEquipo.Add(new SelectListItem { Value = Convert.ToString(equipo.Id_Equipo), Text = equipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEquiposOrden = new SelectList(itemsEquipo, "Value", "Text");

                    ViewBag.listaEquipo = selectlistaEquiposOrden;


                    return View(mT_OrdenEquipo);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Equipos_OrdenTrabajo", new { id = idMantOrdenTrabajo });
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
        public ActionResult Editar_Equipos_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Equipo,Id_Equipo,Fecha_Inicio,Fecha_Fin,Estado")] MT_OrdenTrabajo_Equipo mT_OrdenEquipo)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_GrupoTrabajo = System.Web.HttpContext.Current.Session["id_GrupoTrabajo"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idGrupoTrabajo = int.Parse(id_GrupoTrabajo.ToString());

                if (ModelState.IsValid)
                {

                    mT_OrdenEquipo.Id_GrupoTrabajo = idGrupoTrabajo;
                    mT_OrdenEquipo.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenEquipo.Estado = mT_OrdenEquipo.Estado.Substring(0, 1);

                    db.Entry(mT_OrdenEquipo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Equipo actualizado";

                    return RedirectToAction("Equipos_OrdenTrabajo", new { id = mT_OrdenEquipo.Id_Orden_Trabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Equipos_OrdenTrabajo", new { id = mT_OrdenEquipo.Id_Orden_Trabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Eliminar_EquipoOrdenTrabajo(int id)
        {
            try
            {

                MT_OrdenTrabajo_Equipo mT_OrdenEquipo = db.MT_OrdenTrabajo_Equipo.Find(id);

                mT_OrdenEquipo.Estado = "E";

                db.Entry(mT_OrdenEquipo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Equipos Orden Trabajo Eliminado";
                return RedirectToAction("Equipos_OrdenTrabajo", new { id = mT_OrdenEquipo.Id_Orden_Trabajo });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        #endregion

        //ITEMS ORDEN TRABAJO
        #region

        [HttpGet]
        public ActionResult Items_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);


                if (mT_OrdenTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_tipoTrabajo"] = mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado;
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;


                    var listaItems_OrdenTrabajo = db.sp_Quimipac_ConsultaInsMt_OrdenTrabajoEgreso(mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado, mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaItems_OrdenTrabajo = listaItems_OrdenTrabajo;
                    return View();

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Items_OrdenTrabajo()
        {
            try
            {
                List<SelectListItem> itemsItems = new List<SelectListItem>();
                List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                List<SelectListItem> itemsUnidades = new List<SelectListItem>();
                List<SelectListItem> itemsTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();

                foreach (var items in listaItems)
                {

                    itemsItems.Add(new SelectListItem { Value = Convert.ToString(items.cod_item), Text = items.descripcion });

                }

                SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");

                var listaMonedas = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                foreach (var monedas in listaMonedas)
                {
                    itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(monedas.codmon), Text = monedas.nombre });
                }

                SelectList selectlistaMonedas = new SelectList(itemsMonedas, "Value", "Text");

                var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();

                foreach (var unidades in listaUnidades)
                {

                    itemsUnidades.Add(new SelectListItem { Value = Convert.ToString(unidades.uni_med), Text = unidades.nombre });

                }

                SelectList selectlistaUnidades = new SelectList(itemsUnidades, "Value", "Text");

                var listaTipoEgreso = db.sp_Quimipac_ConsultaMT_TablaDetalle(42).ToList();

                foreach (var tipo in listaTipoEgreso)
                {

                    itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });

                }

                SelectList selectlistaTipoEgreso = new SelectList(itemsTipo, "Value", "Text");


                ViewBag.listaItems = selectlistaItems;
                ViewBag.listaMonedas = selectlistaMonedas;
                ViewBag.listaUnidades = selectlistaUnidades;
                ViewBag.listaTipoEgreso = selectlistaTipoEgreso;


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Items_OrdenTrabajo([Bind(Include = "Id_Item,Fecha,Moneda,Unidad,Cantidad,Precio,Costo,Area,Tipo")] MT_OrdenTrabajo_Egreso mT_OrdenEgreso)
        {
            try
            {
                var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];


                if (id_MantOrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());


                    if (ModelState.IsValid)
                    {
                        mT_OrdenEgreso.Id_Orden_Trabajo = idMantOrdenTrabajo;

                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Egreso.Where(x => x.Id_Orden_Trabajo == mT_OrdenEgreso.Id_Orden_Trabajo && x.Fecha == mT_OrdenEgreso.Fecha && x.Tipo == mT_OrdenEgreso.Tipo && x.Id_Item == mT_OrdenEgreso.Id_Item && x.Moneda == mT_OrdenEgreso.Moneda && x.Precio == mT_OrdenEgreso.Precio);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Egreso ya existe";
                            return RedirectToAction("Items_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                        }
                        else
                        {


                            mT_OrdenEgreso.Id_Orden_Trabajo = idMantOrdenTrabajo;
                            mT_OrdenEgreso.EstadoAct = "A";


                            db.MT_OrdenTrabajo_Egreso.Add(mT_OrdenEgreso);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Egreso guardado";
                            return RedirectToAction("Items_OrdenTrabajo", new { id = id_MantOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Items_OrdenTrabajo", new { id = id_MantOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_Items_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Egreso mT_OrdenEgreso = db.MT_OrdenTrabajo_Egreso.Find(id);
                System.Web.HttpContext.Current.Session["id_Item"] = mT_OrdenEgreso.Id_Item;

                bool seleccion = false;
                if (mT_OrdenEgreso != null)
                {

                    List<SelectListItem> itemsItems = new List<SelectListItem>();
                    List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                    List<SelectListItem> itemsUnidades = new List<SelectListItem>();
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();

                    foreach (var items in listaItems)
                    {

                        if (items.cod_item == mT_OrdenEgreso.Id_Item)
                        {
                            seleccion = true;
                        }


                        itemsItems.Add(new SelectListItem { Value = Convert.ToString(items.cod_item), Text = items.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItemsOrden = new SelectList(itemsItems, "Value", "Text");

                    var listaMonedas = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                    foreach (var monedas in listaMonedas)
                    {
                        if (monedas.codmon == mT_OrdenEgreso.Id_Item)
                        {
                            seleccion = true;
                        }


                        itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(monedas.codmon), Text = monedas.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaMonedasOrden = new SelectList(itemsMonedas, "Value", "Text");

                    var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();

                    foreach (var unidades in listaUnidades)
                    {

                        if (unidades.uni_med == mT_OrdenEgreso.Unidad)
                        {
                            seleccion = true;
                        }


                        itemsUnidades.Add(new SelectListItem { Value = Convert.ToString(unidades.uni_med), Text = unidades.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaUnidadesOrden = new SelectList(itemsUnidades, "Value", "Text");

                    var listaTipoEgreso = db.sp_Quimipac_ConsultaMT_TablaDetalle(42).ToList();

                    foreach (var tipo in listaTipoEgreso)
                    {

                        if (tipo.Id_TablaDetalle == mT_OrdenEgreso.Tipo)
                        {
                            seleccion = true;
                        }


                        itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTipoEgresoOrden = new SelectList(itemsTipo, "Value", "Text");

                    ViewBag.listaItems = selectlistaItemsOrden;
                    ViewBag.listaMonedas = selectlistaMonedasOrden;
                    ViewBag.listaUnidades = selectlistaUnidadesOrden;
                    ViewBag.listaTipoEgreso = selectlistaTipoEgresoOrden;

                    return View(mT_OrdenEgreso);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Items_OrdenTrabajo", new { id = idMantOrdenTrabajo });
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
        public ActionResult Editar_Items_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Egreso,Fecha,Moneda,Unidad,Cantidad,Precio,Costo,Area,Tipo")] MT_OrdenTrabajo_Egreso mT_OrdenItems)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_Item = System.Web.HttpContext.Current.Session["id_Item"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                string idItem = id_Item.ToString();

                if (ModelState.IsValid)
                {

                    mT_OrdenItems.Id_Item = idItem;
                    mT_OrdenItems.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenItems.EstadoAct = "A";

                    db.Entry(mT_OrdenItems).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Egreso actualizado";

                    return RedirectToAction("Items_OrdenTrabajo", new { id = mT_OrdenItems.Id_Orden_Trabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Items_OrdenTrabajo", new { id = mT_OrdenItems.Id_Orden_Trabajo });
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarItemsOrdenTrabajo(int id)
        {
            try
            {

                MT_OrdenTrabajo_Egreso mT_OrdenEgreso = db.MT_OrdenTrabajo_Egreso.Find(id);

                mT_OrdenEgreso.EstadoAct = "E";

                db.Entry(mT_OrdenEgreso).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Egreso Orden Trabajo Eliminado";
                return RedirectToAction("Items_OrdenTrabajo", new { id = mT_OrdenEgreso.Id_Orden_Trabajo });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        #endregion

        //MEDIDAS ORDEN TRABAJO
        #region


        [HttpGet]
        public ActionResult Medidas_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);


                if (mT_OrdenTrabajo != null)
                {
                    System.Web.HttpContext.Current.Session["id_tipoTrabajo"] = mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado;
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaMedidas_OrdenTrabajo = db.sp_Quimipac_ConsultaInsMt_OrdenTrabajoMedida(mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado, mT_OrdenTrabajo.Id_OrdenTrabajo, user_id.ToString(), empresa_id.ToString()).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaMedidas_OrdenTrabajo = listaMedidas_OrdenTrabajo;
                    return View();

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo");
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_Medidas_OrdenTrabajo()
        {
            try
            {

                List<SelectListItem> itemsMedida = new List<SelectListItem>();


                var listaMedidas = db.sp_Quimipac_Consulta_TipoTrabajoMedida().ToList();

                foreach (var medidas in listaMedidas)
                {

                    itemsMedida.Add(new SelectListItem { Value = Convert.ToString(medidas.Id_Tipo_trabajo_Medida), Text = medidas.Descripcion });

                }

                SelectList selectlistaItemsMedida = new SelectList(itemsMedida, "Value", "Text");

                ViewBag.listaMedidas = selectlistaItemsMedida;


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Medidas_OrdenTrabajo([Bind(Include = "Id_Tipo_Trabajo_Medida,Valor_Ini,Valor_Fin")] MT_OrdenTrabajo_Medida mT_OrdenMedida)
        {
            try
            {
                var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                string usuario = user_id.ToString();

                if (id_MantOrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());


                    if (ModelState.IsValid)
                    {
                        mT_OrdenMedida.Id_Orden_Trabajo = idMantOrdenTrabajo;

                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Medida.Where(x => x.Id_Orden_Trabajo == mT_OrdenMedida.Id_Orden_Trabajo && x.Id_Tipo_Trabajo_Medida == mT_OrdenMedida.Id_Tipo_Trabajo_Medida && x.Valor_Ini == mT_OrdenMedida.Valor_Ini && x.Valor_Fin == mT_OrdenMedida.Valor_Fin);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Medida ya existe";
                            return RedirectToAction("Medidas_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                        }
                        else
                        {


                            mT_OrdenMedida.Id_Orden_Trabajo = idMantOrdenTrabajo;
                            //mT_OrdenMedida.EstadoAct = "A";
                            mT_OrdenMedida.Id_Usuario = usuario;



                            db.MT_OrdenTrabajo_Medida.Add(mT_OrdenMedida);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Medida guardado";
                            return RedirectToAction("Medidas_OrdenTrabajo", new { id = id_MantOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Medidas_OrdenTrabajo", new { id = id_MantOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Editar_Medidas_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Medida mT_OrdenMedida = db.MT_OrdenTrabajo_Medida.Find(id);
                System.Web.HttpContext.Current.Session["id_TipoTrabajoMedida"] = mT_OrdenMedida.Id_Tipo_Trabajo_Medida;

                bool seleccion = false;
                if (mT_OrdenMedida != null)
                {
                    List<SelectListItem> itemsMedida = new List<SelectListItem>();


                    var listaMedidas = db.sp_Quimipac_Consulta_TipoTrabajoMedida().ToList();

                    foreach (var medidas in listaMedidas)
                    {

                        if (medidas.Id_Tipo_trabajo_Medida == mT_OrdenMedida.Id_Tipo_Trabajo_Medida)
                        {
                            seleccion = true;
                        }


                        itemsMedida.Add(new SelectListItem { Value = Convert.ToString(medidas.Id_Tipo_trabajo_Medida), Text = medidas.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItemsMedida = new SelectList(itemsMedida, "Value", "Text");

                    ViewBag.listaMedidas = selectlistaItemsMedida;

                    return View(mT_OrdenMedida);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Medidas_OrdenTrabajo", new { id = idMantOrdenTrabajo });
                    }

                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Medidas_OrdenTrabajo([Bind(Include = "Id_Orden_Trabajo_Medida,Valor_Ini,Valor_Fin")] MT_OrdenTrabajo_Medida mT_OrdenMedida)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_TipoTrabajoMedida = System.Web.HttpContext.Current.Session["id_TipoTrabajoMedida"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];

                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idTipoTrabajoMedida = int.Parse(id_TipoTrabajoMedida.ToString());
                //string idItem = id_Item.ToString();
                string usuario = user_id.ToString();

                if (ModelState.IsValid)
                {

                    mT_OrdenMedida.Id_Tipo_Trabajo_Medida = idTipoTrabajoMedida;
                    mT_OrdenMedida.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenMedida.EstadoAct = "A";
                    mT_OrdenMedida.Id_Usuario = usuario;

                    db.Entry(mT_OrdenMedida).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Medida actualizada";

                    return RedirectToAction("Medidas_OrdenTrabajo", new { id = mT_OrdenMedida.Id_Orden_Trabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Medidas_OrdenTrabajo", new { id = mT_OrdenMedida.Id_Orden_Trabajo });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult EliminarMedidasOrdenTrabajo(int id)
        {
            try
            {

                MT_OrdenTrabajo_Medida mT_OrdenMedida = db.MT_OrdenTrabajo_Medida.Find(id);

                mT_OrdenMedida.EstadoAct = "E";

                db.Entry(mT_OrdenMedida).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Egreso Orden Trabajo Eliminado";
                return RedirectToAction("Medidas_OrdenTrabajo", new { id = mT_OrdenMedida.Id_Orden_Trabajo });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        #endregion

        //Busqueda por parametros Canvas
        #region
        /*PARAMETROS COMBO RECIBIDOS */

        //FI Fecha_asignacion_grupo_trabajo
        //FF Fecha_registro
        //CRIT Origen

        //TIPOTRABAJO Id_tipo_trabajo_ejecutado
        //CAMPAMENTO Id_campamento
        //ESTACION Id_estacion
        //SECTOR Id_sector
        //SUCURSAL Id_sucursal


        [HttpPost]
        public ActionResult OrdenTrabajo([Bind(Include = "Fecha_asignacion_grupo_trabajo,Fecha_registro,Origen,Id_tipo_trabajo_ejecutado,Id_campamento,Id_estacion,Id_sector,Id_sucursal,Id_contrato,Estado,cod_cli")]Orden_Trabajo_FiltCli ORDEN)
        {
            try
            {
                var bde = new DataBase_Externo(); //string usuario = System.Web.HttpContext.Current.Session["usuario"] as string;

                var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion_grupo_trabajo, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_contrato, ORDEN.Estado, ORDEN.cod_cli);
                TempData["listaFiltro"] = lkbusqueda;//"Tipo de Trabajo guardado";

                string XParametro = string.Empty;
                if (ORDEN.Origen.Equals("Ninguno")) { XParametro = ""; }
                else if (ORDEN.Origen.Equals("Tipos_Trabajo")) { XParametro = ORDEN.Id_tipo_trabajo_ejecutado.ToString(); }
                else if (ORDEN.Origen.Equals("Campamento")) { XParametro = ORDEN.Id_campamento.ToString(); }
                else if (ORDEN.Origen.Equals("Estacion")) { XParametro = ORDEN.Id_estacion.ToString(); }
                else if (ORDEN.Origen.Equals("Sector")) { XParametro = ORDEN.Id_sector.ToString(); }
                else if (ORDEN.Origen.Equals("Sucursal")) { XParametro = ORDEN.Id_sucursal; }
                else if (ORDEN.Origen.Equals("Contrato")) { XParametro = ORDEN.Id_contrato.ToString(); }
                else if (ORDEN.Origen.Equals("Estado")) { XParametro = ORDEN.Estado.ToString(); }
                else if (ORDEN.Origen.Equals("Cliente")) { XParametro = ORDEN.cod_cli; }
                
                //ViewBag.ParametroBusqueda = ORDEN.Fecha_asignacion_grupo_trabajo + ";" + ORDEN.Fecha_registro + ";" + ORDEN.Origen;
                TempData["ParametroBusqueda"] = ORDEN.Fecha_asignacion_grupo_trabajo + " " + ORDEN.Fecha_registro + " " + ORDEN.Origen + " " + XParametro;
                return RedirectToAction("OrdenTrabajo");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion

        //Entrega Orden Trabajo
        #region
        [HttpGet]
        public ActionResult Entrega_OrdenTrabajo()
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresacod = empresa_id.ToString();
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                string usuario = user_id.ToString();
                var listaentregaorden = db.sp_Quimipac_ConsultaMT_EntregaOrdenTrabajo(usuario, empresacod).ToList();
                ViewBag.listaentregaorden = listaentregaorden;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Agregar_Entrega_OrdenTrabajo()
        {
            try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];



                var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");




                ViewBag.listaClientes = selectlistaClientes;

                //TestModel model = new TestModel();


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_Entrega_OrdenTrabajo([Bind(Include = "Id_Empresa,Id_Cliente,Fecha_Elaboracion,Fecha_Envio,Fecha_Recepcion,Usuario,Comentario,Recibido_Por")] MT_EntregaOrden_Trabajo mT_EntregaOrden)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var entregaorden = db.MT_EntregaOrden_Trabajo.Where(x => x.Id_Empresa == mT_EntregaOrden.Id_Empresa && x.Id_Cliente == mT_EntregaOrden.Id_Cliente && x.Usuario == mT_EntregaOrden.Usuario && x.Fecha_Elaboracion == mT_EntregaOrden.Fecha_Elaboracion);
            //        if (entregaorden.Count() >= 1)
            //        {
            //            TempData["mensaje_error"] = "Código ya existe";
            //            return RedirectToAction("Entrega_OrdenTrabajo");
            //        }
            //        else
            //        {
            //            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            //            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


            //            if (user_id == null)
            //            {
            //                return Redirect(Url.Action("IniciarSesion", "Home"));
            //            }
            //            else
            //            {
            //                mT_EntregaOrden.Usuario = user_id.ToString();
            //                mT_EntregaOrden.Id_Empresa = empresa_id.ToString();
            //                mT_EntregaOrden.Fecha_Elaboracion = DateTime.Now;


            //                db.MT_EntregaOrden_Trabajo.Add(mT_EntregaOrden);
            //                db.SaveChanges();

            //                // DataBase_Externo database3 = new DataBase_Externo();
            //                //int variable = database3.Verificar_EquipoTrabajoOrden(idOrdenTrabajo, mT_OrdenTrabajoIntegrante.Id_GrupoTrabajo, mT_OrdenTrabajoIntegrante.Fecha_Inicio, mT_OrdenTrabajoIntegrante.Fecha_Fin);
            //                //if (variable == 0)
            //                //{
            //                   //database3.InsertarEquipoOrdenTrabajo(idOrdenTrabajo, count2);

            //                //}

            //                TempData["mensaje_correcto"] = "Entrega Orden guardado";
            //                return RedirectToAction("Entrega_OrdenTrabajo");
            //            }

            //        }

            //    }
            //    else
            //    {
            //        TempData["mensaje_error"] = "Valores incorrectos";
            //        return RedirectToAction("Entrega_OrdenTrabajo");
            //    }
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Error", "Errores");
            //}
            return RedirectToAction("Entrega_OrdenTrabajo");
        }

        //[HttpGet]
        // public ActionResult Editar_Entrega_OrdenTrabajo(int id)
        // {
        //    try
        //    {

        //        MT_EntregaOrden_Trabajo mT_EntregaOrden = db.MT_EntregaOrden_Trabajo.Find(id);
        //        System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_EntregaOrden.Fecha_Elaboracion;
        //        bool seleccion = false;
        //        if (mT_EntregaOrden != null)
        //        {

        //            List<SelectListItem> itemsClientes = new List<SelectListItem>();

        //            var listaClientes = db.sp_LINK_ConsultaClientes().ToList();
        //            foreach (var cliente in listaClientes)
        //            {
        //                if (cliente.cod_cli == mT_EntregaOrden.Id_Cliente)
        //                {
        //                    seleccion = true;
        //                }

        //                itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
        //            }

        //            SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

        //            ViewBag.listaClientes = selectlistaClientes;


        //            return View(mT_EntregaOrden);

        //        }
        //        else
        //        {
        //            TempData["mensaje_error"] = "Código no existe";
        //            return RedirectToAction("Entrega_OrdenTrabajo");
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }
        //}

        /*cambio por json*/
        // [ValidateAntiForgeryToken]
        // [HttpPost]
        // public ActionResult Editar_Entrega_OrdenTrabajo([Bind(Include = "Id_Entrega_Orden_Trabajo, Id_Empresa,Id_Cliente,Fecha_Elaboracion,Fecha_Envio,Fecha_Recepcion,Usuario,Comentario,Recibido_Por")] MT_EntregaOrden_Trabajo mT_EntregaOrden)
        // {
        // try
        // {
        // if (ModelState.IsValid)
        // {
        // //int bandera = 1;
        // var user_id = System.Web.HttpContext.Current.Session["usuario"];
        // var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

        // if (user_id == null)
        // {
        // return RedirectToAction("IniciarSesion", "Home");
        // }
        // else
        // {


        // mT_EntregaOrden.Usuario = user_id.ToString();
        // mT_EntregaOrden.Id_Empresa = empresa_id.ToString();
        // mT_EntregaOrden.Fecha_Elaboracion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);



        // db.Entry(mT_EntregaOrden).State = EntityState.Modified;
        // db.SaveChanges();
        // TempData["mensaje_actualizado"] = "Entrega Orden actualizado";

        // return RedirectToAction("Entrega_OrdenTrabajo");

        // }

        // }
        // else
        // {
        // TempData["mensaje_error"] = "Valores incorrectos";
        // return RedirectToAction("Entrega_OrdenTrabajo");
        // }
        // }
        // catch (Exception)
        // {
        // return RedirectToAction("Error", "Errores");
        // }
        // }

        public JsonResult GetOrdenTrabajo(string id)
        {
            try
            {
                List<SelectListItem> ordenTrabajo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaOrden = db.sp_Quimipac_ConsultaMT_OrdenTrabajoEntregaCliente(id, empresa_id.ToString()).ToList();

                foreach (var orden in listaOrden)
                {
                    ordenTrabajo.Add(new SelectListItem { Value = Convert.ToString(orden.Id_OrdenTrabajo), Text = orden.Codigo_Cliente });
                }
                //itemsitemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaItemsTipoTrabajo = new SelectList(ordenTrabajo, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaItemsTipoTrabajo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        public JsonResult InsertarOrden(List<TempEntregaOrden> lkOrden, string ValueCliente, DateTime ValueFechaEnvio, DateTime ValueFechaRecepcion, string Comentario, string Recibido_Por)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa = System.Web.HttpContext.Current.Session["empresa"];
            //var mensajeDiv = new TempMensaje();
            var mensaje = new JsonResult();
            int nRegistro = 0;
            try
            {

                //var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                //MT_PrecioReferencial MTPPreferencial;

                //foreach (var item in lkOrden)
                //{
                DataBase_Externo dbe = new DataBase_Externo();

                var band = dbe.InsertarEntregaOrden(lkOrden, System.Web.HttpContext.Current.Session["empresa"].ToString(), ValueCliente, ValueFechaEnvio, ValueFechaRecepcion, System.Web.HttpContext.Current.Session["usuario"].ToString(), Comentario, Recibido_Por);
                // nRegistro++;
                //}




                if (band < 1)
                {
                    //mensaje.Data = new { mensaje = "Error al ingresar, favor verificar datos ingresados" }; 
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                    return mensaje;
                    // throw new ApplicationException("Error al ingresar, favor verificar datos ingresados");
                    // throw new InvalidOperationException();
                    //  
                }
                // mensajeDiv.Mensaje = "  Registro Guardado correctamente";
                //mensajeDiv.Ok = true;

                // objectContext.AddObject("MT_PrecioReferencial", MTPPreferencial);
                // db.SaveChanges();


                mensaje.Data = "Registros guardados correctamente";
                return mensaje;
                //  return Json(lkItems, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                //mensajeDiv.Ok = false;
                // return resultado;
                return Json(Response.StatusCode);
            }
        }

        public class TempEntregaOrden
        {
            public int Id_OrdenTrabajo { get; set; }
            public string Codigo_Cliente { get; set; }

        }

        [HttpGet]
        public ActionResult Editar_Entrega_OrdenTrabajo(int id)
        {
            try
            {

                MT_EntregaOrden_Trabajo mT_EntregaOrden = db.MT_EntregaOrden_Trabajo.Find(id);
                System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_EntregaOrden.Fecha_Elaboracion;
                System.Web.HttpContext.Current.Session["Cliente"] = mT_EntregaOrden.Id_Cliente;
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                bool seleccion = false;
                if (mT_EntregaOrden != null)
                {

                    List<SelectListItem> itemsClientes = new List<SelectListItem>();

                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    foreach (var cliente in listaClientes)
                    {
                        if (cliente.cod_cli == mT_EntregaOrden.Id_Cliente)
                        {
                            seleccion = true;
                        }

                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }

                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                    ViewBag.listaClientes = selectlistaClientes;


                    return View(mT_EntregaOrden);

                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Entrega_OrdenTrabajo");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        //Editar_Entrega_OrdenTrabajo
        public JsonResult Guardar_Editar_Entrega_OT(int? Id_EntregaOrdenTrabajo, List<int> resultadoChecked, List<int> resultadoCheckTotal, int? Id_Cliente, string comentario, string RecibidoX, DateTime? FechaEnvio, DateTime? FechaRecep)
        {
            try
            {
                var mensaje = new JsonResult();
                var band = 0;
                DataBase_Externo dbe = new DataBase_Externo();
                int cont = 0;
                if (resultadoCheckTotal != null)
                {
                    foreach (var item in resultadoCheckTotal)
                    {
                        var bandCheck = resultadoChecked.Contains(item);// == null ? false : true;
                        if (bandCheck)
                        {
                            cont = dbe.Editar_Entrega_OrdenTrabajo(Id_EntregaOrdenTrabajo, item, Id_Cliente, FechaEnvio, FechaRecep, comentario, RecibidoX, "NO");
                        }
                        else
                        {
                            cont = dbe.Editar_Entrega_OrdenTrabajo(Id_EntregaOrdenTrabajo, item, Id_Cliente, FechaEnvio, FechaRecep, comentario, RecibidoX, "SI");
                        }
                    }
                }

                if (cont < 1)
                {
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                    //mensaje = "Error al ingresar, favor verificar datos ingresados!";

                }
                else
                {
                    mensaje.Data = "Registros guardados correctamente";
                    //mensaje = "Registros guardados correctamente";
                }
                return mensaje;

                // return Json((string)mensaje, JsonRequestBehavior.AllowGet);
                //return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        public class Temp_EditarCheckTotal
        {
            public int Id_OrdenTrabajo { get; set; }
            //public string Codigo_Cliente { get; set; }

        }
        public class Temp_EditarCheck
        {
            public int Id_OrdenTrabajoToT { get; set; }
            //public string Codigo_Cliente { get; set; }

        }

        #endregion

        //ORDEN ENTREGA TRABAJO
        #region
        [HttpGet]
        public ActionResult OrdenEntregaTrabajoOrden(int id)
        {
            try
            {
                MT_OrdenTrabajo mt_OrdenEntrega = db.MT_OrdenTrabajo.Find(id);
                System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mt_OrdenEntrega.Id_OrdenTrabajo;
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                //System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] = mT_OrdenActividadEstado.Fecha_Ini;
                // MT_EntregaOrden_Trabajo mt_OrdenEntregaOrden = db.MT_OrdenTrabajo.ToList();

                bool seleccion = false;
                if (mt_OrdenEntrega != null)
                {

                    List<SelectListItem> itemsEstado = new List<SelectListItem>();

                    //var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();
                    var listaFecha = db.SP_Quimipac_ConsultaClientesOrdenCont(id, empresa_id.ToString()).ToList();

                    //var listaItems = db.sp_LINK_ConsultaItems().ToList();

                    foreach (var items in listaFecha)
                    {

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(items.Id_Entrega_Orden_Trabajo), Text = items.Fecha_Elaboracion.ToString() });

                    }

                    SelectList selectlistaItemsFecha = new SelectList(itemsEstado, "Value", "Text");


                    ViewBag.listaFecha = selectlistaItemsFecha;
                    //ViewBag.fechaI = mT_OrdenActividad.Fecha_Ini;
                    //ViewBag.fechaF = mT_OrdenActividad.Fecha_Fin;
                    ViewBag.idMTOrdenTrabajo = mt_OrdenEntrega.Id_OrdenTrabajo;


                    return View(mt_OrdenEntrega);
                }
                else
                {

                    var id_MantOrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_MantOrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantOrdenTrabajo = int.Parse(id_MantOrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("OrdenTrabajo", new { id = idMantOrdenTrabajo });
                    }

                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        //arreglar ojoooooooooooooooooooooo
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrdenEntregaTrabajoOrden([Bind(Include = "Id_OrdenTrabajo,Id_entrega_orden_trabajo")] MT_OrdenTrabajo mT_OrdenTrabajoEntrega)
        {
            try
            {

                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];


                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());



                if (ModelState.IsValid)
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id != null)
                    {
                        //mT_OrdenTrabajoEntrega.Id_Actividad = idMActividad;
                        mT_OrdenTrabajoEntrega.Id_OrdenTrabajo = idOrdenTrabajo;

                        var lkEntregaOrden = db.MT_OrdenTrabajo.Where(x => x.Id_OrdenTrabajo == mT_OrdenTrabajoEntrega.Id_OrdenTrabajo);
                        if (lkEntregaOrden.Count() > 0)
                        {
                            foreach (var itemOrdenEntrega in lkEntregaOrden)
                            {
                                itemOrdenEntrega.Id_entrega_orden_trabajo = mT_OrdenTrabajoEntrega.Id_entrega_orden_trabajo;
                                //itemProrroga.Fecha_Fin = DateTime.Now;
                                //MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                //mT_OrdenTrabajo.Fecha_fin_ejecucion = DateTime.Now;
                            }
                            db.SaveChanges();


                        }
                    }
                    else
                    {
                        mT_OrdenTrabajoEntrega.Id_OrdenTrabajo = idOrdenTrabajo;
                        TempData["mensaje_actualizado"] = "Orden Entrega No ha sido iniciada";

                        return RedirectToAction("OrdenTrabajo", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });
                    }

                    //db.Entry(mT_OrdenActividadFinalizada).State = EntityState.Modified;
                    //db.SaveChanges();
                    //DataBase_Externo database4 = new DataBase_Externo();

                    //    database4.ActualizarActividadOrden(mT_OrdenActividadFinalizada.Id_OrdenTrabajo_Actividad, mT_OrdenActividadFinalizada.Id_Actividad, mT_OrdenActividadFinalizada.Id_Orden_Trabajo, mT_OrdenActividadFinalizada.Estado);


                    TempData["mensaje_actualizado"] = "Orden entrega actualizada";

                    return RedirectToAction("OrdenTrabajo", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("OrdenTrabajo", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //Orden Trabajo Actividades Valor
        #region
        [HttpGet]
        public ActionResult OrdenTrabajo_Valor(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_Orden = db.MT_OrdenTrabajo.Find(id);
                if (mT_Orden != null)
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    System.Web.HttpContext.Current.Session["id_Orden"] = mT_Orden.Id_OrdenTrabajo;
                    var listaValorOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajoValor(id, empresa_id.ToString()).ToList();
                    var orden_trabajo = mT_Orden.Codigo_Cliente;                    
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaValorOrdenTrabajo = listaValorOrdenTrabajo;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Valor Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_OrdenTrabajo_Valor()
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                List<SelectListItem> tipo_valor = new List<SelectListItem>();
                List<SelectListItem> monedas = new List<SelectListItem>();
                List<SelectListItem> Unidades = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                foreach (var item in listaItems)
                {
                    items.Add(new SelectListItem
                    {
                        Value = Convert.ToString(item.cod_item),
                        Text = item.descripcion
                    });
                }

                SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                var listaTipo_Valor = db.sp_Quimipac_ConsultaMT_TablaDetalle(56).ToList();
                foreach (var tipo in listaTipo_Valor)
                {
                    tipo_valor.Add(new SelectListItem
                    {
                        Value = Convert.ToString(tipo.Id_TablaDetalle),
                        Text = tipo.Descripcion
                    });
                }

                SelectList selectlistaTipo_Valor = new SelectList(tipo_valor, "Value", "Text");

                var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                foreach (var moneda in listaMoneda)
                {
                    monedas.Add(new SelectListItem
                    {
                        Value = Convert.ToString(moneda.codmon),
                        Text = moneda.nombre
                    });
                }

                SelectList selectlistaMoneda = new SelectList(monedas, "Value", "Text");

                var listaUnidad = db.sp_LINK_ConsultaUnidades().ToList();
                foreach (var unidad in listaUnidad)
                {
                    Unidades.Add(new SelectListItem
                    {
                        Value = Convert.ToString(unidad.uni_med),
                        Text = unidad.nombre
                    });
                }

                SelectList selectlistaUnidad = new SelectList(Unidades, "Value", "Text");


                ViewBag.listaItems = selectlistaItems;
                ViewBag.listaTipo_Valor = selectlistaTipo_Valor;
                ViewBag.listaMoneda = selectlistaMoneda;
                ViewBag.listaUnidad = selectlistaUnidad;

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_OrdenTrabajo_Valor([Bind(Include = "Id_Orden_Trabajo, Id_Usuario, Tipo_Valor, Id_Item,Moneda,Fecha,Unidad,Cantidad,Precio,Costo,IVA")] MT_OrdenTrabajo_Valor mT_OrdenTrabajoValor)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];

                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {
                        mT_OrdenTrabajoValor.Id_Usuario = user_id.ToString();
                        mT_OrdenTrabajoValor.Fecha = DateTime.Now;
                        mT_OrdenTrabajoValor.Id_Orden_Trabajo = idOrdenTrabajo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Valor.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajoValor.Id_Orden_Trabajo && x.Id_Item == mT_OrdenTrabajoValor.Id_Item && x.Moneda == mT_OrdenTrabajoValor.Moneda && x.Cantidad == mT_OrdenTrabajoValor.Cantidad);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Orden Trabajo Valor ya existe";
                            return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });
                        }
                        else
                        {

                            mT_OrdenTrabajoValor.Id_Orden_Trabajo = idOrdenTrabajo;


                            db.MT_OrdenTrabajo_Valor.Add(mT_OrdenTrabajoValor);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Orden Trabajo Valor guardado";
                            return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_OrdenTrabajo_Valor(int id)
        {
            try
            {
                MT_OrdenTrabajo_Valor mT_ordenTrabajoValor = db.MT_OrdenTrabajo_Valor.Find(id);
                System.Web.HttpContext.Current.Session["Fecha"] = mT_ordenTrabajoValor.Fecha;
                System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_ordenTrabajoValor.Fecha;
                bool seleccion = false;
                if (mT_ordenTrabajoValor != null)
                {

                    List<SelectListItem> items = new List<SelectListItem>();
                    List<SelectListItem> TipoValor = new List<SelectListItem>();
                    List<SelectListItem> Monedas = new List<SelectListItem>();
                    List<SelectListItem> Unidades = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                    foreach (var item in listaItems)
                    {
                        if (mT_ordenTrabajoValor.Id_Item == item.cod_item)
                        {
                            seleccion = true;
                        }

                        items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                    var listaTipo_Valor = db.sp_Quimipac_ConsultaMT_TablaDetalle(56).ToList();
                    foreach (var tipo in listaTipo_Valor)
                    {
                        if (mT_ordenTrabajoValor.Tipo_Valor == tipo.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        TipoValor.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaTipoValor = new SelectList(TipoValor, "Value", "Text");

                    var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                    foreach (var moneda in listaMoneda)
                    {
                        if (mT_ordenTrabajoValor.Moneda == moneda.codmon)
                        {
                            seleccion = true;
                        }

                        Monedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaMoneda = new SelectList(Monedas, "Value", "Text");

                    var listaUnidad = db.sp_LINK_ConsultaUnidades().ToList();
                    foreach (var unidad in listaUnidad)
                    {
                        if (mT_ordenTrabajoValor.Unidad == unidad.uni_med)
                        {
                            seleccion = true;
                        }

                        Unidades.Add(new SelectListItem { Value = Convert.ToString(unidad.uni_med), Text = unidad.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaUnidad = new SelectList(Unidades, "Value", "Text");

                    ViewBag.listaItems = selectlistaItems;
                    ViewBag.listaTipo_Valor = selectlistaTipoValor;
                    ViewBag.listaMoneda = selectlistaMoneda;
                    ViewBag.listaUnidad = selectlistaUnidad;

                    return View(mT_ordenTrabajoValor);

                }
                else
                {
                    var id_ordentrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_ordentrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idOrdenTrabajo = int.Parse(id_ordentrabajo.ToString());
                        TempData["mensaje_error"] = "Orden Trabajo Valor no existe";
                        return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_OrdenTrabajo_Valor([Bind(Include = "Id_OrdenTrabajo_Valor,Id_Orden_Trabajo, Id_Usuario, Tipo_Valor, Id_Item,Moneda,Fecha,Unidad,Cantidad,Precio,Costo,IVA")] MT_OrdenTrabajo_Valor mT_OrdenTrabajoValor)
        {
            try
            {

                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
                var fecha = System.Web.HttpContext.Current.Session["Fecha"];

                // int bandera = 0;
                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {


                        mT_OrdenTrabajoValor.Id_Usuario = user_id.ToString();
                        mT_OrdenTrabajoValor.Fecha = Convert.ToDateTime(fecha);
                        mT_OrdenTrabajoValor.Id_Orden_Trabajo = idOrdenTrabajo;

                        db.Entry(mT_OrdenTrabajoValor).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Orden Trabajo Valor guardado";

                        return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });


                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion

        //Orden Trabajo Actividades Estado
        #region


        [HttpGet]
        public ActionResult OrdenTrabajo_Estado(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_Orden = db.MT_OrdenTrabajo.Find(id);
                if (mT_Orden != null)
                {
                    System.Web.HttpContext.Current.Session["id_Orden"] = mT_Orden.Id_OrdenTrabajo;
                    var listaEstadoOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajoEstado(id).ToList();
                    var orden_trabajo = mT_Orden.Codigo_Cliente;                    
                    ViewBag.listaEstadoOrdenTrabajo = listaEstadoOrdenTrabajo;
                    ViewBag.orden_trabajo = orden_trabajo;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Estado Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_OrdenTrabajo_Estado()
        {
            try
            {

                List<SelectListItem> estado = new List<SelectListItem>();


                var listaEstado = db.sp_Quimipac_ConsultaMT_TablaDetalle(9).ToList();
                foreach (var tipo in listaEstado)
                {
                    estado.Add(new SelectListItem
                    {
                        Value = Convert.ToString(tipo.Id_TablaDetalle),
                        Text = tipo.Descripcion
                    });
                }

                SelectList selectlistalistaEstado = new SelectList(estado, "Value", "Text");


                ViewBag.listaEstado = selectlistalistaEstado;


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_OrdenTrabajo_Estado([Bind(Include = "Id_Orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin")] MT_OrdenTrabajo_Estado mT_OrdenTrabajoEstado)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];

                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        mT_OrdenTrabajoEstado.Id_orden_Trabajo = idOrdenTrabajo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == mT_OrdenTrabajoEstado.Id_orden_Trabajo && x.Estado_Orden_Trabajo == mT_OrdenTrabajoEstado.Estado_Orden_Trabajo && x.Fecha_Ini == mT_OrdenTrabajoEstado.Fecha_Ini);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Orden Trabajo Estado ya existe";
                            return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });
                        }
                        else
                        {
                            mT_OrdenTrabajoEstado.Id_orden_Trabajo = idOrdenTrabajo;

                            //db.MT_OrdenTrabajo_Estado.Add(mT_OrdenTrabajoEstado);
                            //db.SaveChanges();

                            //var itemOrdenEstado = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == mT_OrdenTrabajoEstado.Id_orden_Trabajo && x.Estado_Orden_Trabajo == 30);
                            //if (itemOrdenEstado.Count() >= 1)
                            //{
                            DataBase_Externo dbe = new DataBase_Externo();
                            var resp = dbe.ActualizarOrdenTrabajo(mT_OrdenTrabajoEstado.Id_orden_Trabajo, mT_OrdenTrabajoEstado.Estado_Orden_Trabajo, mT_OrdenTrabajoEstado.Fecha_Ini, mT_OrdenTrabajoEstado.Fecha_Fin);
                            if (resp > 0)
                            {
                                TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                            }
                            else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }
                            //}
                            //else
                            //{
                            //    TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                            //}

                            return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_OrdenTrabajo_Estado(int id)
        {
            try
            {
                MT_OrdenTrabajo_Estado mT_ordenTrabajoEstado = db.MT_OrdenTrabajo_Estado.Find(id);
                System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_ordenTrabajoEstado.Id_orden_Trabajo;
                bool seleccion = false;
                if (mT_ordenTrabajoEstado != null)
                {


                    List<SelectListItem> Estado = new List<SelectListItem>();

                    var listaEstado = db.sp_Quimipac_ConsultaMT_TablaDetalle(9).ToList();
                    foreach (var tipo in listaEstado)
                    {
                        if (mT_ordenTrabajoEstado.Estado_Orden_Trabajo == tipo.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        Estado.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEstado = new SelectList(Estado, "Value", "Text");


                    ViewBag.listaEstado = selectlistaEstado;

                    return View(mT_ordenTrabajoEstado);

                }
                else
                {
                    var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_OrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Orden Trabajo Valor no existe";
                        return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_OrdenTrabajo_Estado([Bind(Include = "Id_OrdenTrabajo_Estado,Id_Orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO")] MT_OrdenTrabajo_Estado mT_OrdenTrabajoEstado)
        {
            try
            {

                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];


                // int bandera = 0;
                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {



                        mT_OrdenTrabajoEstado.Id_orden_Trabajo = idOrdenTrabajo;


                        //db.Entry(mT_OrdenTrabajoEstado).State = EntityState.Modified;
                        //db.SaveChanges();

                        //var itemOrdenEstado = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == mT_OrdenTrabajoEstado.Id_orden_Trabajo && x.Estado_Orden_Trabajo == 30);
                        //if (itemOrdenEstado.Count() >= 1)
                        //{
                        DataBase_Externo dbe = new DataBase_Externo();
                        var resp = dbe.ActualizarOrdenTrabajo(mT_OrdenTrabajoEstado.Id_orden_Trabajo, mT_OrdenTrabajoEstado.Estado_Orden_Trabajo, mT_OrdenTrabajoEstado.Fecha_Ini, mT_OrdenTrabajoEstado.Fecha_Fin);
                        if (resp > 0)
                        {
                            TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                        }
                        else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }
                        //}
                        //else
                        //{
                        //    TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                        //}

                        return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });


                        //TempData["mensaje_actualizado"] = "Orden Trabajo Estado guardado";

                        //return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });


                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        #endregion

        //Orden Trabajo Causa Raiz 
        #region


        [HttpGet]
        public ActionResult OrdenTrabajo_CausaRaiz(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_Orden = db.MT_OrdenTrabajo.Find(id);
                if (mT_Orden != null)
                {
                    System.Web.HttpContext.Current.Session["id_Orden"] = mT_Orden.Id_OrdenTrabajo;
                    var listaCausaRaizOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajoCausaRaiz(id).ToList();
                    var orden_trabajo = mT_Orden.Codigo_Cliente;
                    ViewBag.listaCausaRaizOrdenTrabajo = listaCausaRaizOrdenTrabajo;                    
                    ViewBag.orden_trabajo = orden_trabajo;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Causa Raiz Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Agregar_OrdenTrabajo_CausaRaiz()
        {
            try
            {

                List<SelectListItem> causaraiz = new List<SelectListItem>();
                List<SelectListItem> hizo = new List<SelectListItem>();
                List<SelectListItem> encontro = new List<SelectListItem>();


                var listaTipo_CausaRaiz = db.sp_Quimipac_ConsultaMT_TablaDetalle(60).ToList();
                foreach (var tipo in listaTipo_CausaRaiz)
                {
                    causaraiz.Add(new SelectListItem
                    {
                        Value = Convert.ToString(tipo.Id_TablaDetalle),
                        Text = tipo.Descripcion
                    });
                }

                SelectList selectlistaTipo_CausaRaiz = new SelectList(causaraiz, "Value", "Text");
                

                var lista_Hizo = db.sp_Quimipac_ConsultaMT_TablaDetalle(61).ToList();
                foreach (var tipoH in lista_Hizo)
                {
                    hizo.Add(new SelectListItem
                    {
                        Value = Convert.ToString(tipoH.Id_TablaDetalle),
                        Text = tipoH.Descripcion
                    });
                }

                SelectList selectlista_Hizo = new SelectList(hizo, "Value", "Text");
                              

                var lista_Encontro = db.sp_Quimipac_ConsultaMT_TablaDetalle(62).ToList();
                foreach (var tipoE in lista_Encontro)
                {
                    encontro.Add(new SelectListItem
                    {
                        Value = Convert.ToString(tipoE.Id_TablaDetalle),
                        Text = tipoE.Descripcion
                    });
                }

                SelectList selectlista_Encontro = new SelectList(encontro, "Value", "Text");
                
                ViewBag.listaTipo_CausaRaiz = selectlistaTipo_CausaRaiz;
                ViewBag.lista_Hizo = selectlista_Hizo;
                ViewBag.lista_Encontro = selectlista_Encontro;


                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_OrdenTrabajo_CausaRaiz([Bind(Include = "Id_Orden_Trabajo, Id_Causa_Raiz, Motivo_Causa, Encontro, Motivo_E, Hizo, Motivo_H")] MT_OrdenTrabajo_CausaRaiz mT_OrdenTrabajoCausa)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
                var user_id = System.Web.HttpContext.Current.Session["usuario"];

                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {

                        mT_OrdenTrabajoCausa.Id_Orden_Trabajo = idOrdenTrabajo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_CausaRaiz.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajoCausa.Id_Orden_Trabajo && x.Id_Causa_Raiz == mT_OrdenTrabajoCausa.Id_Causa_Raiz && x.Hizo == mT_OrdenTrabajoCausa.Hizo && x.Encontro == mT_OrdenTrabajoCausa.Encontro);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Orden Trabajo Causa Raiz ya existe";
                            return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });
                        }
                        else
                        {
                            mT_OrdenTrabajoCausa.Id_Orden_Trabajo = idOrdenTrabajo;

                            db.MT_OrdenTrabajo_CausaRaiz.Add(mT_OrdenTrabajoCausa);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Orden Trabajo Cusa Raiz guardado";
                           
                            return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpGet]
        public ActionResult Editar_OrdenTrabajo_CausaRaiz(int id)
        {
            try
            {
                MT_OrdenTrabajo_CausaRaiz mT_ordenTrabajoCausaRaiz = db.MT_OrdenTrabajo_CausaRaiz.Find(id);
                System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_ordenTrabajoCausaRaiz.Id_Orden_Trabajo;
                bool seleccion = false;
                if (mT_ordenTrabajoCausaRaiz != null)
                {


                    List<SelectListItem> causaraiz = new List<SelectListItem>();
                    List<SelectListItem> hizo = new List<SelectListItem>();
                    List<SelectListItem> encontro = new List<SelectListItem>();

                    var listaCausaRaiz = db.sp_Quimipac_ConsultaMT_TablaDetalle(60).ToList();
                    foreach (var tipo in listaCausaRaiz)
                    {
                        if (mT_ordenTrabajoCausaRaiz.Id_Causa_Raiz == tipo.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        causaraiz.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaCausaRaiz = new SelectList(causaraiz, "Value", "Text");

                    var listaHizo = db.sp_Quimipac_ConsultaMT_TablaDetalle(61).ToList();
                    foreach (var tipoH in listaHizo)
                    {
                        if (mT_ordenTrabajoCausaRaiz.Hizo == tipoH.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        hizo.Add(new SelectListItem { Value = Convert.ToString(tipoH.Id_TablaDetalle), Text = tipoH.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaHizo = new SelectList(hizo, "Value", "Text");

                    var listaEncontro = db.sp_Quimipac_ConsultaMT_TablaDetalle(62).ToList();
                    foreach (var tipoE in listaEncontro)
                    {
                        if (mT_ordenTrabajoCausaRaiz.Encontro == tipoE.Id_TablaDetalle)
                        {
                            seleccion = true;
                        }

                        encontro.Add(new SelectListItem { Value = Convert.ToString(tipoE.Id_TablaDetalle), Text = tipoE.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaEncontro = new SelectList(encontro, "Value", "Text");


                    ViewBag.listaCausaRaiz = selectlistaCausaRaiz;
                    ViewBag.listaHizo = selectlistaHizo;
                    ViewBag.listaEncontro = selectlistaEncontro;

                    return View(mT_ordenTrabajoCausaRaiz);

                }
                else
                {
                    var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];

                    if (id_OrdenTrabajo == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                        TempData["mensaje_error"] = "Orden Trabajo Causa Raiz no existe";
                        return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });
                    }
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_OrdenTrabajo_CausaRaiz([Bind(Include = "Id_OrdenTrabajo_CausaRaiz,Id_Orden_Trabajo, Id_Causa_Raiz, Motivo_Causa, Encontro, Motivo_E, Hizo, Motivo_H")] MT_OrdenTrabajo_CausaRaiz mT_OrdenTrabajoCausaRaiz)
        {
            try
            {

                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];


                // int bandera = 0;
                if (id_OrdenTrabajo == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());

                    if (ModelState.IsValid)
                    {



                        mT_OrdenTrabajoCausaRaiz.Id_Orden_Trabajo = idOrdenTrabajo;

                        db.Entry(mT_OrdenTrabajoCausaRaiz).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Orden Trabajo Causa Raiz guardado";

                        return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });


                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_CausaRaiz", new { id = idOrdenTrabajo });
                    }
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
