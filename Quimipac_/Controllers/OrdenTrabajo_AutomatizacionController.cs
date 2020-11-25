//codigo

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

using PagedList.Mvc;
using PagedList;
using System.Web.Configuration;
using System.Net.Mail;

namespace Quimipac_.Controllers
{
    public class OrdenTrabajo_AutomatizacionController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        //private int NroRegistro = int.Parse(WebConfigurationManager.AppSettings["TablaNroRegistro"].ToString());

        //MATENIMIENTO DE Orden DE TRABAJO
        #region
        [HttpGet]
        public ActionResult OrdenTrabajo_Automatizacion()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 1).Where(vq => vq.Fecha_asignacion != null).ToList();

                    

                    List<SelectListItem> lkProyecto = new List<SelectListItem>();
                    List<SelectListItem> lkProspecto = new List<SelectListItem>();
                    List<SelectListItem> lkOrdenPadre = new List<SelectListItem>();

                    DataBase_Externo dbe = new DataBase_Externo();
                    var listakOrdenPadre = dbe.ParametroLkOrdenPadre("OrdenPadre", empresa_id.ToString());

                    foreach (var item in listakOrdenPadre)
                    {
                        lkOrdenPadre.Add(new SelectListItem { Value = Convert.ToString(item.Id_OrdenTrabajo), Text = item.Codigo_Cliente });
                    }
                    SelectList selectlkOrdenPadre = new SelectList(lkOrdenPadre, "Value", "Text");

                    var listalkProyecto = dbe.ParametroLkProyecto("Proyecto", empresa_id.ToString());
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    listalkProyecto = listalkProyecto.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();

                    foreach (var item in listalkProyecto)
                    {
                        lkProyecto.Add(new SelectListItem { Value = Convert.ToString(item.Id_Proyecto), Text = item.Codigo_Cliente });
                    }
                    SelectList selectlkProyecto = new SelectList(lkProyecto, "Value", "Text");

                    var listalkProspecto = dbe.ParametroLkContrato("Prospecto", empresa_id.ToString());
                    var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                    listalkProspecto = listalkProspecto.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                    foreach (var item in listalkProspecto)
                    {
                        lkProspecto.Add(new SelectListItem { Value = Convert.ToString(item.Id_Prospecto), Text = item.Codigo_Cliente + " " + item.Nombre });
                    }
                    SelectList selectlkProspecto = new SelectList(lkProspecto, "Value", "Text");


                    ViewBag.listalkTipo_Trabajo = new SelectList(dbe.ParametroLkTipoTrabajo("Tipo_Trabajo", empresa_id.ToString()), "Id_TipoTrabajo", "Descripcion");
                    ViewBag.listalkCampamento = new SelectList(dbe.ParametroLkCampamento("Campamento", empresa_id.ToString()), "Id_Campamento", "Nombre");
                    ViewBag.listalkSucursal = new SelectList(dbe.ParametroLkSucursal("Sucursal", empresa_id.ToString()), "cod_suc", "nombre");
                    ViewBag.listalkEstacion = new SelectList(dbe.ParametroLkEstacion("Estacion", empresa_id.ToString()), "Id_Estacion", "Nombre");


                    //var ContratosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    var ProyectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    var ProspectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();


                    //listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI2.Contains(vq.Id_contrato.ToString())).ToList();
                    listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ProyectosLI2.Contains(vq.Id_Proyecto.ToString()) || ProspectosLI2.Contains(vq.Id_Prospecto.ToString())).ToList();


                    //******************
                    //?
                    var lista_Proceso = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "EN PROCESO").OrderByDescending(vq => vq.Fecha_registro).ToList();// lista_Proceso;
                    var lista_Alerta = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "ALERTA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Alerta;
                    var lista_Caida = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "CAIDA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Caida;

                    //listaOrdenTrabajo = new List<ConsultaMT_OrdenTrabajo_AuxVM>();
                    listaOrdenTrabajo = new List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result>();

                    string orderby = System.Web.HttpContext.Current.Session["OrderBy_OT"] == null ? "DESC" : System.Web.HttpContext.Current.Session["OrderBy_OT"].ToString();
                    //orderby = orderby ?? "CAIDA";
                    orderby = orderby.ToUpper();
                    if (orderby == "ASC")
                    {
                        OrderBy_ListaParametro(lista_Proceso, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Alerta, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Caida, listaOrdenTrabajo);
                    }
                    /*else if (orderby == "ALERTA")
                    {
                        OrderBy_ListaParametro(lista_Alerta, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Caida, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Proceso, listaOrdenTrabajo);
                    }*/
                    else if (orderby == "DESC")
                    {
                        OrderBy_ListaParametro(lista_Caida, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Alerta, listaOrdenTrabajo);
                        OrderBy_ListaParametro(lista_Proceso, listaOrdenTrabajo);
                    }
                    /*
                    foreach (var item in lista_Alerta) {
                        listaOrdenTrabajo.Add(item);
                    }
                    foreach (var item in lista_Proceso) {
                        listaOrdenTrabajo.Add(item);
                    }*/
                    //?

                    //******************
                    ViewBag.listaOrdenTrabajo = listaOrdenTrabajo;

                    var dba = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dba.Item_OpcPermiso();

                    ViewBag.listalkSector = new SelectList(dbe.ParametroLkSector("Sector", empresa_id.ToString()), "Id_Sector", "Nombre");
                    ViewBag.listalkEstado = new SelectList(dbe.ParametroLkEstado("Estado", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");

                    /*ojo verificar como hacer para traer solo los clientes de permisos*/
                    ViewBag.listakClientes = new SelectList(dbe.ParametroLkCliente("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");


                    ViewBag.lista_Proceso = lista_Proceso;//? listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "EN PROCESO").ToList();// lista_Proceso;
                    ViewBag.lista_Alerta = lista_Alerta;//? listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "ALERTA").ToList();//lista_Alerta;
                    ViewBag.lista_Caida = lista_Caida;//? listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "CAIDA").ToList();//lista_Caida;

                    
                    ViewBag.listakOrdenPadre = selectlkOrdenPadre;
                    ViewBag.listalkProyecto = selectlkProyecto;
                    ViewBag.listalkProspecto = selectlkProspecto;

                    ViewBag.mensaje = TempData["mensaje_error2"];
                    ViewBag.listaEtapas = orderby;




                    return View();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

       /* # public List<ConsultaMT_OrdenTrabajo_AuxVM> OrderBy_ListaParametro(List<ConsultaMT_OrdenTrabajo_AuxVM> LI, List<ConsultaMT_OrdenTrabajo_AuxVM> FullOTLI)
        {
            foreach (var item in LI)
            {
                FullOTLI.Add(item);
            }
            return FullOTLI;
        }*/

        public List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> OrderBy_ListaParametro(List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> LI, List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> FullOTLI)
        {
            foreach (var item in LI)
            {
                FullOTLI.Add(item);
            }
            return FullOTLI;
        }


        //filtro
        public List<ParametrosBusquedaOT> OrderBy_ListaParametro_Filtro(List<ParametrosBusquedaOT> LI, List<ParametrosBusquedaOT> FullOTLI)
        {
            foreach (var item in LI)
            {
                FullOTLI.Add(item);
            }
            return FullOTLI;
        }

        //#--public List<ConsultaMT_OrdenTrabajo_AuxVM> OrderBy_ListaParametro_Filtro_2(List<ConsultaMT_OrdenTrabajo_AuxVM> LI, List<ConsultaMT_OrdenTrabajo_AuxVM> FullOTLI)
        public List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> OrderBy_ListaParametro_Filtro_2(List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> LI, List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> FullOTLI)
        {
            foreach (var item in LI)
            {
                FullOTLI.Add(item);
            }
            return FullOTLI;
        }

        public string IsValidParametro(int Orden_trabajoID, int? TipoTrabajoId) //string codigo
        {
            DataBase_Externo dbe = new DataBase_Externo();
            BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
            //var lk_OrdenTrabajoTT = db.MT_OrdenTrabajo.Where(vq => vq.Id_tipo_trabajo_ejecutado == TipoTrabajoId).Take(1).ToList();//.ToList();
            var lk_OrdenTrabajoTT = db.MT_OrdenTrabajo.Where(vq => vq.Id_tipo_trabajo_ejecutado == TipoTrabajoId && vq.Id_OrdenTrabajo == Orden_trabajoID).Take(1).ToList();//.ToList();
            var TipoTrabajoOT = db.MT_TipoTrabajo.Where(vq => vq.Id_TipoTrabajo == TipoTrabajoId).SingleOrDefault();
            DateTime fAsignacion = Convert.ToDateTime(lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion);
            if (lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion != null)
            {
                DateTime fActual = DateTime.Now; //(dbe.ReturnFechaActual_SQL().ElementAt(0).FechaActual_SQL);

                DateTime? DateProceso = null;
                DateTime? DateAlerta = null;
                DateTime? DateCaida = null;

                if (TipoTrabajoOT != null)
                {
                    DateProceso = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.Proceso));
                    DateAlerta = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.Alerta));
                    DateCaida = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.Caida));

                    if (fActual > DateProceso)
                    {
                        if (fActual > DateAlerta)
                        {
                            //CAIDA
                            return "CAIDA";
                        }
                        else
                        {
                            //ALERTA
                            return "ALERTA";
                        }
                    }
                    else { return "EN PROCESO"; }
                }//if temporal
                return null;
            }
            else { return null; }
        }


        //order by ajax
        [HttpPost]
        public ActionResult OrderByEtapa(string OTCriterio)
        {
            var mensaje = new JsonResult();
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                System.Web.HttpContext.Current.Session["OrderBy_OT"] = OTCriterio;


                mensaje.Data = "OK";

                return mensaje;
            }
            else
            {
                mensaje.Data = "ERROR";
                return mensaje;
                //                return RedirectToAction("IniciarSesion", "Home");

            }

        }
        #endregion
        //agregar, editar, import
        #region
        [HttpGet]
        public ActionResult Agregar_OrdenTrabajo()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                try
                {
                    List<SelectListItem> itemsOrdenTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                    List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                    List<SelectListItem> itemstipo_trabajo_recibido = new List<SelectListItem>();
                    List<SelectListItem> itemstipo_trabajo_ejecutado = new List<SelectListItem>();
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemssector = new List<SelectListItem>();
                    List<SelectListItem> itemsEstacion = new List<SelectListItem>();
                    List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                    List<SelectListItem> itemsNivelPrioridad = new List<SelectListItem>();
                    List<SelectListItem> itemsGruposTrabajo = new List<SelectListItem>();
                    List<SelectListItem> itemsProspectos = new List<SelectListItem>();

                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresa = empresa_id.ToString();

                    var listaOrdenes = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa, 1).Where(x => x.Estado == 30 || x.Estado == 31).ToList();
                    foreach (var orden in listaOrdenes)
                    {
                        itemsOrdenTrabajo.Add(new SelectListItem { Value = Convert.ToString(orden.Id_OrdenTrabajo), Text = orden.Codigo_Cliente });
                    }
                    itemsOrdenTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });
                    SelectList selectlistaOrden = new SelectList(itemsOrdenTrabajo, "Value", "Text");

                    var listaProyecto = db.sp_Quimipac_ConsultaMT_Proyecto(empresa).ToList();
                    //aquiiiii agregue
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                    listaProyecto = listaProyecto.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROYECTOS").ToList();
                    listaProyecto = listaProyecto.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                    itemsProyectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var proyecto in listaProyecto)
                    {
                        itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
                    }
                    SelectList selectlistaProyecto = new SelectList(itemsProyectos, "Value", "Text");

                    var listacampamento = db.sp_Quimipac_ConsultaMT_Campamento(empresa).ToList();
                    foreach (var campamento in listacampamento)
                    {
                        itemsCampamento.Add(new SelectListItem { Value = Convert.ToString(campamento.Id_Campamento), Text = campamento.Nombre });
                    }
                    itemsCampamento.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });
                    SelectList selectlistaCampamento = new SelectList(itemsCampamento, "Value", "Text");

                    var listaTrabajorecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre(2, empresa).ToList();
                    if (listaTrabajorecibido.Count <= 0)
                    {
                        var aux_trecibido_Add = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                        foreach (var item in aux_trecibido_Add)
                        {
                            var aux_trecibidoPadre_Add = new sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre_Result
                            {
                                Id_TipoTrabajo = item.Id_TipoTrabajo,
                                Descripcion = item.Descripcion,
                                Codigo = item.Codigo
                            };
                            listaTrabajorecibido.Add(aux_trecibidoPadre_Add);
                        }

                    }
                    itemstipo_trabajo_recibido.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var trabajorecibido in listaTrabajorecibido)
                    {
                        itemstipo_trabajo_recibido.Add(new SelectListItem { Value = Convert.ToString(trabajorecibido.Id_TipoTrabajo), Text = trabajorecibido.Codigo + " | " + trabajorecibido.Descripcion });
                    }
                    SelectList selectlistaTrabajoRecibido = new SelectList(itemstipo_trabajo_recibido, "Value", "Text");

                    var listaTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos(empresa, listaTrabajorecibido.ElementAt(0).Id_TipoTrabajo).ToList();
                    if (listaTrabajoejecutado.Count <= 0)
                    {
                        var aux_tejecutado_Add = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                        foreach (var item in aux_tejecutado_Add)
                        {
                            var aux_tejecutadoPadre_Add = new sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos_Result
                            {
                                Id_TipoTrabajo = item.Id_TipoTrabajo,
                                Descripcion = item.Descripcion,
                                Codigo = item.Codigo
                            };
                            listaTrabajoejecutado.Add(aux_tejecutadoPadre_Add);
                        }

                    }
                    itemstipo_trabajo_ejecutado.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var trabajoejecutado in listaTrabajoejecutado)
                    {
                        itemstipo_trabajo_ejecutado.Add(new SelectListItem { Value = Convert.ToString(trabajoejecutado.Id_TipoTrabajo), Text = trabajoejecutado.Codigo + " | " + trabajoejecutado.Descripcion });
                    }
                    SelectList selectlistaTrabajoEjecutado = new SelectList(itemstipo_trabajo_ejecutado, "Value", "Text");

                    var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoTrabajo(empresa).ToList();
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

                    var listaSector = db.sp_Quimipac_ConsultaMT_Sector(empresa).ToList();
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

                    var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa).ToList();
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

                    var listaProspecto = db.sp_Quimipac_ConsultaMT_Prospecto(empresa).ToList();
                    //aquiiiii agregue
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                    listaProspecto = listaProspecto.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                    var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROSPECTOS").ToList();
                    listaProspecto = listaProspecto.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();
                    itemsProspectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var prospecto in listaProspecto)
                    {
                        itemsProspectos.Add(new SelectListItem { Value = Convert.ToString(prospecto.Id_Prospecto), Text = prospecto.Codigo_Cliente + " " + prospecto.Nombre  });
                    }
                    SelectList selectlistaProspecto = new SelectList(itemsProspectos, "Value", "Text");

                    ViewBag.listaOrdenes = selectlistaOrden;
                    ViewBag.listaProyecto = selectlistaProyecto;
                    ViewBag.listacampamento = selectlistaCampamento;
                    ViewBag.listaTrabajorecibido = selectlistaTrabajoRecibido;
                    ViewBag.listaTrabajoejecutado = selectlistaTrabajoEjecutado;
                    ViewBag.listaEstados = selectlistaEstados;
                    ViewBag.listaSector = selectlistaSector;
                    ViewBag.listaEstacion = selectlistaEstacion;
                    ViewBag.listaSucursal = selectlistaSucursal;
                    ViewBag.listaNivelPrioridad = selectlistaPrioridad;
                    ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;
                    ViewBag.listaProspecto = selectlistaProspecto;
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
        public ActionResult Agregar_OrdenTrabajo([Bind(Include = "Id_Proyecto, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_entrega_orden_trabajo ,Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_asignacion, Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio ,Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_Planilla, Estado_orden_planilla, Codigo_Cliente, Interna, Fecha_maxima_contratista, Desalojo, Id_Prospecto")] MT_OrdenTrabajo mT_OrdenTrabajo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (mT_OrdenTrabajo.Fecha_asignacion != null)
                        {
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            var ordenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 1).ToList().Where(x => x.Codigo_Cliente == mT_OrdenTrabajo.Codigo_Cliente && x.Id_orden_padre == mT_OrdenTrabajo.Id_orden_padre).ToList();
                            if (ordenTrabajo.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Orden Trabajo ya existe";
                                return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                                    //if (mT_OrdenTrabajo.Id_contrato != null || mT_OrdenTrabajo.Id_contrato != 0)
                                    if (mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado != null || mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado != 0)
                                    {
                                        var TipoTrabajoOT = db.MT_TipoTrabajo.Where(vq => vq.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado).ToList();
                                        //var objParametros = db.MT_Contrato_Parametro.Where(vq => vq.Id_Contrato == mT_OrdenTrabajo.Id_contrato).SingleOrDefault();
                                        if (TipoTrabajoOT != null)
                                        {
                                            if (TipoTrabajoOT.ElementAt(0).Proceso != null)
                                            {
                                                DateTime fAsignacion = Convert.ToDateTime(mT_OrdenTrabajo.Fecha_asignacion);
                                                mT_OrdenTrabajo.Fecha_maxima_contratista = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.ElementAt(0).Proceso));
                                                mT_OrdenTrabajo.Fecha_finalizacion_obra = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.ElementAt(0).Proceso));

                                            }
                                        }
                                        else
                                        {
                                            mT_OrdenTrabajo.Fecha_maxima_contratista = null;
                                            mT_OrdenTrabajo.Fecha_finalizacion_obra = null;
                                        }
                                    }

                                    mT_OrdenTrabajo.Id_usuario = user_id.ToString();
                                    mT_OrdenTrabajo.Fecha_registro = DateTime.Now;
                                    mT_OrdenTrabajo.EstadoEditOrden = "A";
                                    mT_OrdenTrabajo.Excel_orden = false;
                                    mT_OrdenTrabajo.Automatizacion = true;
                                    mT_OrdenTrabajo.Fecha_inicio_ejecucion = mT_OrdenTrabajo.Fecha_asignacion;
                                    db.MT_OrdenTrabajo.Add(mT_OrdenTrabajo);
                                    db.SaveChanges();

                                    DataBase_Externo dbe = new DataBase_Externo();
                                    var resp = dbe.InsertarEstadoOrden(mT_OrdenTrabajo.Id_OrdenTrabajo, mT_OrdenTrabajo.Estado);
                                    if (resp > 0)
                                    {
                                        TempData["mensaje_correcto"] = "Orden Trabajo guardado";
                                    }
                                    else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }
                                    return RedirectToAction("OrdenTrabajo_Automatizacion");
                                }
                            }
                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores Incorrectos o asegurese de ingresar la fecha de asignación";
                            return RedirectToAction("OrdenTrabajo_Automatizacion");
                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
                    }
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


        [HttpGet]
        public ActionResult Editar_OrdenTrabajo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                try
                {
                    MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                    System.Web.HttpContext.Current.Session["estado_Orden"] = mT_OrdenTrabajo.Estado;
                    System.Web.HttpContext.Current.Session["FechaRegistro"] = mT_OrdenTrabajo.Fecha_registro;
                    System.Web.HttpContext.Current.Session["Excel_orden"] = mT_OrdenTrabajo.Excel_orden;
                    //System.Web.HttpContext.Current.Session["Id_Contrato"] = mT_OrdenTrabajo.Id_contrato;
                    System.Web.HttpContext.Current.Session["Id_Proyecto"] = mT_OrdenTrabajo.Id_Proyecto;
                    System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] = mT_OrdenTrabajo.Fecha_inicio_ejecucion;
                    System.Web.HttpContext.Current.Session["Automatizacion"] = mT_OrdenTrabajo.Automatizacion;
                    System.Web.HttpContext.Current.Session["FechaAsignacion"] = mT_OrdenTrabajo.Fecha_asignacion;

                    // comente esto porque ella quiso que fecha_inicioejecucion sea igual a fecha_asignacion var mt_OrdenTrabajoActividadOrdenFI = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaInicio").ToList();
                    var mt_OrdenTrabajoActividadOrdenFF = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaFin").ToList();
                    /*if (mt_OrdenTrabajoActividadOrdenFI.Count > 0)
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
                    }*/


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
                        //List<SelectListItem> itemsContratos = new List<SelectListItem>();
                        List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                        List<SelectListItem> itemsCampamento = new List<SelectListItem>();
                        List<SelectListItem> itemstipo_trabajo_recibido = new List<SelectListItem>();
                        List<SelectListItem> itemstipo_trabajo_ejecutado = new List<SelectListItem>();
                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemssector = new List<SelectListItem>();
                        List<SelectListItem> itemsEstacion = new List<SelectListItem>();
                        List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                        List<SelectListItem> itemsNivelPrioridad = new List<SelectListItem>();
                        List<SelectListItem> itemsProspectos = new List<SelectListItem>();

                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 1).Where(x => x.Estado == 30 || x.Estado == 31).ToList();
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

                        //var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                        var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();

                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProyectos = listaProyectos.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        //var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                        listaProyectos = listaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();

                        itemsProyectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var proyecto in listaProyectos)
                        {
                            //if (contrato.Id_Contrato == mT_OrdenTrabajo.Id_contrato)
                            if (proyecto.Id_Proyecto == mT_OrdenTrabajo.Id_Proyecto)
                            {
                                seleccion = true;
                            }
                            itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente, Selected = seleccion });
                        }
                        SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");

                        var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                        itemsSucursal.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
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
                        itemsCampamento.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
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
                        var listaTiposTrabajorecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre(2, empresa_id.ToString()).ToList();

                        if (listaTiposTrabajorecibido.Count <= 0)
                        {
                            var aux_trecibido = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                            foreach (var item in aux_trecibido)
                            {
                                var aux_trecibidoPadre = new sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre_Result
                                {
                                    Id_TipoTrabajo = item.Id_TipoTrabajo,
                                    Descripcion = item.Descripcion,
                                    Codigo = item.Codigo
                                };
                                listaTiposTrabajorecibido.Add(aux_trecibidoPadre);
                            }

                        }

                        itemstipo_trabajo_recibido.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var tipoTrabajo in listaTiposTrabajorecibido)
                        {
                            if (tipoTrabajo.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_recibido)
                            {
                                seleccion = true;
                            }

                            itemstipo_trabajo_recibido.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo + " | " + tipoTrabajo.Descripcion, Selected = seleccion });
                        }
                        SelectList selectlistaTiposTrabajorecibido = new SelectList(itemstipo_trabajo_recibido, "Value", "Text");

                        //var listaTiposTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems(2).ToList();
                        var listaTiposTrabajoejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos(empresa_id.ToString(), listaTiposTrabajorecibido.ElementAt(0).Id_TipoTrabajo).ToList();

                        if (listaTiposTrabajoejecutado.Count <= 0)
                        {
                            var aux_tejecutado = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                            foreach (var item in aux_tejecutado)
                            {
                                var aux_tejecutadoPadre = new sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos_Result
                                {
                                    Id_TipoTrabajo = item.Id_TipoTrabajo,
                                    Descripcion = item.Descripcion,
                                    Codigo = item.Codigo
                                };
                                listaTiposTrabajoejecutado.Add(aux_tejecutadoPadre);
                            }

                        }
                        itemstipo_trabajo_ejecutado.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var tipoTrabajo in listaTiposTrabajoejecutado)
                        {
                            if (tipoTrabajo.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado)
                            {
                                seleccion = true;
                            }
                            itemstipo_trabajo_ejecutado.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo + " | " + tipoTrabajo.Descripcion, Selected = seleccion });
                        }
                        SelectList selectlistaTiposTrabajoejecutado = new SelectList(itemstipo_trabajo_ejecutado, "Value", "Text");

                        var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(9).ToList();//pordefecto al sp de la tabla detalle le envio un parametro en este caso 9 por el id_tabla 
                        itemsEstado.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_OrdenTrabajo.Estado)
                            {
                                seleccion = true;
                                itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                                break;
                            }
                        }
                        SelectList selectlistaEstados = new SelectList(itemsEstado, "Value", "Text");

                        var listaSector = db.sp_Quimipac_ConsultaMT_Sector(empresa_id.ToString()).ToList();
                        itemssector.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

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
                        itemsEstacion.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

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
                        itemsNivelPrioridad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        foreach (var prioridad in listaNivelPrioridad)
                        {
                            if (prioridad.Id_TablaDetalle == mT_OrdenTrabajo.Nivel_prioridad)
                            {
                                seleccion = true;
                            }
                            itemsNivelPrioridad.Add(new SelectListItem { Value = Convert.ToString(prioridad.Id_TablaDetalle), Text = prioridad.Descripcion, Selected = seleccion });
                        }
                        SelectList selectlistaNivelPrioridad = new SelectList(itemsNivelPrioridad, "Value", "Text");

                        var listaProspectos = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).ToList();

                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();


                        listaProspectos = listaProspectos.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                        var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                        listaProspectos = listaProspectos.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                        itemsProspectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        //foreach (var contrato in listaContratos) CAMBIE POR PROYECTO
                        foreach (var prospecto in listaProspectos)
                        {
                            //if (contrato.Id_Contrato == mT_OrdenTrabajo.Id_contrato) cambie por proyecto
                            if (prospecto.Id_Prospecto == mT_OrdenTrabajo.Id_Prospecto)
                            {
                                seleccion = true;
                            }
                            itemsProspectos.Add(new SelectListItem { Value = Convert.ToString(prospecto.Id_Prospecto), Text = prospecto.Codigo_Cliente + " " + prospecto.Nombre, Selected = seleccion });
                        }
                        SelectList selectlistaProspectos = new SelectList(itemsProspectos, "Value", "Text");


                        ViewBag.listaOrdenTrabajo = selectlistaOrdenTrabajo;
                        ViewBag.listaProyectos = selectlistaProyectos;
                        ViewBag.listaCampamento = selectlistaCampamento;
                        ViewBag.listaTiposTrabajorecibido = selectlistaTiposTrabajorecibido;
                        ViewBag.listaTiposTrabajoejecutado = selectlistaTiposTrabajoejecutado;
                        ViewBag.listaEstados = selectlistaEstados;
                        ViewBag.listaSector = selectlistasector;
                        ViewBag.listaEstacion = selectlistaEstacion;
                        ViewBag.listaSucursal = selectlistaSucursal;
                        ViewBag.listaNivelPrioridad = selectlistaNivelPrioridad;
                        ViewBag.tiempo_transcurrido = mT_OrdenTrabajo.Tiempo_transcurrido;
                        ViewBag.OrdenTcod_Edit = mT_OrdenTrabajo.Codigo_Cliente;
                        ViewBag.listaProspectos = selectlistaProspectos;

                        return View(mT_OrdenTrabajo);
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
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
        public ActionResult Editar_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo, Id_Proyecto, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_entrega_orden_trabajo, Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_asignacion, Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_Planilla, Estado_orden_planilla, Codigo_Cliente, Interna, Fecha_maxima_contratista, Desalojo, Id_Prospecto")] MT_OrdenTrabajo mT_OrdenTrabajoedit)
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
                            //mT_OrdenTrabajoedit.Id_contrato = Convert.ToInt32(System.Web.HttpContext.Current.Session["Id_Contrato"]);
                            mT_OrdenTrabajoedit.Id_Proyecto = Convert.ToInt32(System.Web.HttpContext.Current.Session["Id_Proyecto"]);
                            //if (mT_OrdenTrabajoedit.Id_contrato != null || mT_OrdenTrabajoedit.Id_contrato != 0)
                            if (mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado != null || mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado != 0)
                            {
                                var TipoTrabajoOT = db.MT_TipoTrabajo.Where(vq => vq.Id_TipoTrabajo == mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado).ToList();
                                //var objParametros = db.MT_Contrato_Parametro.Where(vq => vq.Id_Contrato == mT_OrdenTrabajoedit.Id_contrato).SingleOrDefault();
                                if (TipoTrabajoOT != null)
                                {
                                    if (TipoTrabajoOT.ElementAt(0).Proceso != null)
                                    {
                                        DateTime fAsignacion = Convert.ToDateTime(mT_OrdenTrabajoedit.Fecha_asignacion);
                                        mT_OrdenTrabajoedit.Fecha_maxima_contratista = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.ElementAt(0).Proceso));
                                        mT_OrdenTrabajoedit.Fecha_finalizacion_obra = fAsignacion.AddHours(Convert.ToInt32(TipoTrabajoOT.ElementAt(0).Proceso));
                                    }

                                }
                                else
                                {
                                    mT_OrdenTrabajoedit.Fecha_maxima_contratista = null;
                                    mT_OrdenTrabajoedit.Fecha_finalizacion_obra = null;
                                }
                            }

                            if (Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaAsignacion"]) == mT_OrdenTrabajoedit.Fecha_asignacion)
                            {
                                if (System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] != null)
                                {
                                    mT_OrdenTrabajoedit.Fecha_inicio_ejecucion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaInicioEjecucion"]);
                                }
                            }
                            else
                            {
                                mT_OrdenTrabajoedit.Fecha_inicio_ejecucion = mT_OrdenTrabajoedit.Fecha_asignacion;

                            }

                            DataBase_Externo databaseOrden = new DataBase_Externo();
                            int retornaTO = databaseOrden.GetTipoTrabajoOrden(mT_OrdenTrabajoedit.Id_OrdenTrabajo, mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado);
                            if (retornaTO == 1)
                            {
                                mT_OrdenTrabajoedit.Id_usuario = user_id.ToString();
                                mT_OrdenTrabajoedit.Fecha_registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);


                                if (System.Web.HttpContext.Current.Session["FechaFinEjecucion"] != null)
                                {
                                    mT_OrdenTrabajoedit.Fecha_fin_ejecucion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucion"]);
                                }

                                mT_OrdenTrabajoedit.EstadoEditOrden = "A";
                                mT_OrdenTrabajoedit.Estado = Convert.ToInt32(System.Web.HttpContext.Current.Session["estado_Orden"]);
                                mT_OrdenTrabajoedit.Excel_orden = Convert.ToBoolean(System.Web.HttpContext.Current.Session["Excel_orden"]);
                                mT_OrdenTrabajoedit.Automatizacion = Convert.ToBoolean(System.Web.HttpContext.Current.Session["Automatizacion"]);
                                db.Entry(mT_OrdenTrabajoedit).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {

                                if (System.Web.HttpContext.Current.Session["FechaFinEjecucion"] != null)
                                {
                                    mT_OrdenTrabajoedit.Fecha_fin_ejecucion = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucion"]);
                                }

                                mT_OrdenTrabajoedit.Excel_orden = Convert.ToBoolean(System.Web.HttpContext.Current.Session["Excel_orden"]);
                                mT_OrdenTrabajoedit.Automatizacion = Convert.ToBoolean(System.Web.HttpContext.Current.Session["Automatizacion"]);
                                mT_OrdenTrabajoedit.Estado = Convert.ToInt32(System.Web.HttpContext.Current.Session["estado_Orden"]);
                                databaseOrden.InsertarOrdenTrabajoEditNueva(mT_OrdenTrabajoedit.Id_OrdenTrabajo, /*mT_OrdenTrabajoedit.Id_contrato*/mT_OrdenTrabajoedit.Id_Proyecto, mT_OrdenTrabajoedit.Id_sucursal,
                                 mT_OrdenTrabajoedit.Id_campamento, mT_OrdenTrabajoedit.Id_tipo_trabajo_recibido, mT_OrdenTrabajoedit.Id_tipo_trabajo_ejecutado,
                                 mT_OrdenTrabajoedit.Estado, mT_OrdenTrabajoedit.Id_sector, mT_OrdenTrabajoedit.Id_orden_padre, mT_OrdenTrabajoedit.Id_estacion, user_id.ToString(),
                                 mT_OrdenTrabajoedit.Id_entrega_orden_trabajo, mT_OrdenTrabajoedit.Nivel_prioridad, mT_OrdenTrabajoedit.Fecha_creacion_cliente,
                                 mT_OrdenTrabajoedit.Fecha_maxima_ejecucion_cliente, mT_OrdenTrabajoedit.Fecha_asignacion_grupo_trabajo, mT_OrdenTrabajoedit.Fecha_asignacion, mT_OrdenTrabajoedit.Fecha_finalizacion_obra,
                                 mT_OrdenTrabajoedit.Fecha_ini_permiso_municipal, mT_OrdenTrabajoedit.Fecha_fin_permiso_municipal, mT_OrdenTrabajoedit.Fecha_entrega,
                                 mT_OrdenTrabajoedit.Fecha_max_legalizacion, mT_OrdenTrabajoedit.Hora_acordada, mT_OrdenTrabajoedit.Id_barrio, mT_OrdenTrabajoedit.Direccion,
                                 mT_OrdenTrabajoedit.Referencia_direccion, mT_OrdenTrabajoedit.Identificacion_suscriptor, mT_OrdenTrabajoedit.Nombre_suscriptor,
                                 mT_OrdenTrabajoedit.Tipo_suscriptor, mT_OrdenTrabajoedit.Telefono_suscriptor, mT_OrdenTrabajoedit.Origen, mT_OrdenTrabajoedit.Comentario,
                                 mT_OrdenTrabajoedit.Porcentaje_avance, mT_OrdenTrabajoedit.Tiempo_transcurrido, mT_OrdenTrabajoedit.Id_planilla,
                                 mT_OrdenTrabajoedit.Estado_orden_planilla, mT_OrdenTrabajoedit.Codigo_Cliente, mT_OrdenTrabajoedit.Interna, mT_OrdenTrabajoedit.Excel_orden, mT_OrdenTrabajoedit.Fecha_maxima_contratista, mT_OrdenTrabajoedit.Desalojo, mT_OrdenTrabajoedit.Automatizacion, mT_OrdenTrabajoedit.Fecha_inicio_ejecucion, mT_OrdenTrabajoedit.Fecha_fin_ejecucion, mT_OrdenTrabajoedit.Id_Prospecto);
                            }
                            TempData["mensaje_actualizado"] = "Orden Trabajo actualizado";
                            return RedirectToAction("OrdenTrabajo_Automatizacion");
                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
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

        public JsonResult GetTiposTrabajo2(int id)
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                List<SelectListItem> itemsTiposTrabajo = new List<SelectListItem>();
                var listaTiposTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos(empresa_id.ToString(), id).ToList();
                if (listaTiposTrabajo.Count <= 0)
                {
                    var aux_tejecutado2 = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(2, empresa_id.ToString()).ToList();
                    foreach (var item in aux_tejecutado2)
                    {
                        var aux_tejecutadoPadre2 = new sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos_Result
                        {
                            Id_TipoTrabajo = item.Id_TipoTrabajo,
                            Descripcion = item.Descripcion,
                            Codigo = item.Codigo
                        };
                        listaTiposTrabajo.Add(aux_tejecutadoPadre2);
                    }

                }
                itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var tipoTrabajo in listaTiposTrabajo)
                {
                    itemsTiposTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipoTrabajo.Id_TipoTrabajo), Text = tipoTrabajo.Codigo + " | " + tipoTrabajo.Descripcion });
                }
                SelectList selectlistaTipoTrabajo = new SelectList(itemsTiposTrabajo, "Value", "Text");

                return Json(selectlistaTipoTrabajo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        public int Obtener_Id(string trabajo)
        {
            return 0;
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile, int contrato_excel, string Origen)
        {
            try
            {
                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    TempData["mensaje_error"] = "Por favor seleccione un archivo";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
                }
                else
                {
                    if (contrato_excel != 0)
                    {
                        if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                        {
                            //TempData["mensaje_correcto"] = "Archivo seleccionado";@"~/Formularios_Actividad/"
                            string path = Server.MapPath(@"~/Excel_OT/" + excelfile.FileName);
                            string pathArchivos = Server.MapPath(@"~/Excel_OT/");

                            if (!Directory.Exists(pathArchivos))
                            {
                                Directory.CreateDirectory(pathArchivos);
                            }

                            var liArchivo = Directory.GetFiles(pathArchivos).ToList();
                            if (liArchivo.Count() != 0)
                            {
                                foreach (var item in liArchivo)
                                {
                                    if (item.Contains(excelfile.FileName))
                                    {
                                        TempData["mensaje_error"] = "Archivo ya existe, suba otro archivo";
                                        return RedirectToAction("OrdenTrabajo_Automatizacion");
                                    }
                                }

                            }

                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            excelfile.SaveAs(path);
                            //read data from excel file

                            var records = new List<OrdenTrabajoAutoma_Excel>();
                            int iContador = 0;
                            int iContadorExistente = 0;
                            int iContadorNoIngresado = 0;
                            int iContadorIngresado = 0;
                            DateTime? fecha_creacion_;
                            DateTime? fecha_asignacion_;
                            DateTime? inicio_ej_;
                            DateTime? fecha_finalizacion_obra;
                            string variable;
                            string variablest = string.Empty;
                            string codigo_orden = string.Empty;

                            var user_id = System.Web.HttpContext.Current.Session["usuario"];
                            string usuario = user_id.ToString();
                            string mensaje = string.Empty;
                            List<OrdenTrabajoAutoma_Excel> lista_ordenes = new List<OrdenTrabajoAutoma_Excel>();
                            List<string> lista_aux_ordenes = new List<string>();
                            List<OrdenTrabajoAutoma_Excel> lista_ordenes_totales = new List<OrdenTrabajoAutoma_Excel>();

                            using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Excel_OT/"), excelfile.FileName), FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    while (reader.Read())
                                    {
                                        if (reader.GetValue(0) != null)
                                        {
                                            if (iContador != 0)
                                            {
                                                lista_ordenes_totales.Add(new OrdenTrabajoAutoma_Excel
                                                {
                                                    oatom_id_aux_total = reader.GetValue(0) == null ? null : reader.GetValue(0).ToString(),
                                                    Estado = reader.GetValue(1) == null ? null : reader.GetValue(1).ToString(),
                                                    Identificador = reader.GetValue(2) == null ? null : reader.GetValue(2).ToString(),
                                                    nombre_actividad = reader.GetValue(3) == null ? null : reader.GetValue(3).ToString(),
                                                    rutinaria_activador = reader.GetValue(11) == null ? null : reader.GetValue(11).ToString(),
                                                    inicio_ej = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString(),
                                                    fin_ej = reader.GetValue(18) == null ? null : reader.GetValue(18).ToString(),
                                                    fecha_creacion = reader.GetValue(19) == null ? null : reader.GetValue(19).ToString(),
                                                    integrantes = reader.GetValue(23) == null ? null : reader.GetValue(23).ToString(),
                                                    responsable = reader.GetValue(27) == null ? null : reader.GetValue(27).ToString(),
                                                    Tipo_de_Trabajo = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString()

                                                });
                                                if (!lista_aux_ordenes.Contains(reader.GetValue(0).ToString()))
                                                {
                                                    lista_aux_ordenes.Add(reader.GetValue(0).ToString());
                                                    /*

                                                        if (!lista_ordenes.Exists(vq => vq.oatom_id_aux == reader.GetValue(0).ToString()))
                                                        {

                                                        lista_ordenes.Add(new OrdenTrabajoAutoma_Excel
                                                        {oatom_id_aux = reader.GetValue(0).ToString()  });
                                                    */

                                                    //records.Add(new OrdenTrabajoAutoma_Excel
                                                    //{

                                                    //    Estado = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                                                    //    Identificador = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                                                    //    fecha_asignacion = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString(),
                                                    //    inicio_ej = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString(),
                                                    //    fecha_creacion = reader.GetValue(19) == null ? null : reader.GetValue(19).ToString(),
                                                    //    Tipo_de_Trabajo = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString()


                                                    //});

                                                    string Estado = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString();
                                                    string id_orden_trabajo = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString();

                                                    //string Identificador_Codigo_Actividad = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                                    //if (Identificador_Codigo_Actividad != "")
                                                    //{

                                                    //var codigo_split = Identificador_Codigo_Actividad.Split('-');
                                                    codigo_orden = id_orden_trabajo; //codigo_split[0].Trim() + "-" + codigo_split[1].Trim() + "-" + codigo_split[2].Trim() + "-" + id_orden_trabajo;
                                                                                     //}

                                                    //             OM-GY-EAP-BBB-MOT-000-2
                                                    string estacion = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                                    //if (estacion != "")
                                                    //{
                                                    //    //int nposs = estacion.IndexOf('-');                                        
                                                    //    //variablest = estacion.Substring(nposs + 1);
                                                    //    //variablest = variablest.TrimStart();
                                                    //    var estacion_split = estacion.Split('-');
                                                    //    variablest = estacion_split[3].Trim();
                                                    //}


                                                    string Fecha_de_Asignacion = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString();
                                                    if (Fecha_de_Asignacion != null)
                                                    {
                                                        fecha_asignacion_ = Convert.ToDateTime(Fecha_de_Asignacion);
                                                    }
                                                    else { fecha_asignacion_ = null; }

                                                    string Inicio_de_Ejecucion = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString();
                                                    if (Inicio_de_Ejecucion != null)
                                                    {
                                                        inicio_ej_ = Convert.ToDateTime(Inicio_de_Ejecucion);
                                                    }
                                                    else { inicio_ej_ = null; }
                                                    string Fecha_finalizacion = reader.GetValue(18) == null ? null : reader.GetValue(18).ToString();
                                                    if (Fecha_finalizacion != null)
                                                    {
                                                        fecha_finalizacion_obra = Convert.ToDateTime(Fecha_finalizacion);
                                                    }
                                                    else { fecha_finalizacion_obra = null; }

                                                    string Fecha_de_Creacion = reader.GetValue(19) == null ? null : reader.GetValue(19).ToString();
                                                    if (Fecha_de_Creacion != null)
                                                    {
                                                        fecha_creacion_ = Convert.ToDateTime(Fecha_de_Creacion);
                                                    }
                                                    else
                                                    {
                                                        fecha_creacion_ = null;
                                                    }

                                                    string tipo_trabajo = reader.GetValue(32) == null ? "" : reader.GetValue(32).ToString();
                                                    if (tipo_trabajo != "")
                                                    {
                                                        //int npos = tipo_trabajo.IndexOf('-');
                                                        variable = tipo_trabajo; //.Substring(npos + 1);
                                                        variable = variable.TrimStart();
                                                    }
                                                    else
                                                    {
                                                        variable = "";
                                                    }





                                                    bool automatizacion = true;

                                                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];


                                                    DataBase_Externo database4 = new DataBase_Externo();
                                                    //var OTRepetido = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString()).ToList().Where(x => x.Codigo_Cliente == codigo_orden && x.Id_contrato == contrato_excel).ToList();
                                                    if (Origen == "Proyecto")
                                                    {
                                                        var OTRepetido = db.MT_OrdenTrabajo.Where(x => x.Codigo_Cliente == codigo_orden && x.Id_Proyecto == contrato_excel && x.EstadoEditOrden == "A").ToList();

                                                        if (OTRepetido.Count() <= 0)
                                                        {
                                                            if (variable != "")
                                                            {
                                                                //set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contrato_excel)                                                    
                                                                //select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato
                                                                var client_contr = db.MT_Proyecto.Where(vq => vq.Id_Proyecto == contrato_excel).FirstOrDefault().Id_Cliente;
                                                                //var client_contr = db.MT_Contrato.Where(vq => vq.Id_Contrato == contrato_excel).FirstOrDefault().Categoria;
                                                                var trabjservplan = from tipotrab in db.MT_TipoTrabajo.Where(vq => vq.Descripcion == variable && vq.Estado == "A" && vq.Id_Cliente == client_contr)
                                                                                    join tiposect in db.MT_Servicio.Where(vq => vq.Id_Empresa == empresa_id.ToString())
                                                                                    on tipotrab.Id_Servicio equals tiposect.Id_Servicio
                                                                                    select new { tipotrab.Id_TipoTrabajo };


                                                                var trabajoservicioplanlist = trabjservplan.ToList();

                                                                if (trabajoservicioplanlist.Count > 0)
                                                                {
                                                                    if (trabajoservicioplanlist.ElementAt(0).Id_TipoTrabajo != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            var ac = database4.IngresarOrdenes_Excel_Aut(codigo_orden, Estado, fecha_creacion_, fecha_asignacion_, inicio_ej_, fecha_finalizacion_obra, variable, variablest, usuario, contrato_excel, empresa_id.ToString(), automatizacion, Origen);
                                                                            if (ac != -1)
                                                                            {
                                                                                iContadorIngresado = iContadorIngresado + 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                                mensaje = mensaje + "La orden " + codigo_orden + "El tipo trabajo y/o estacion no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                            }
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                            mensaje = mensaje + "La orden " + codigo_orden + "El tipo trabajo y/o estacion no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                            System.Diagnostics.Debug.WriteLine("Error insert orden Automat" + e.ToString());
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                        mensaje = mensaje + "En La orden " + codigo_orden + " Los campos tipo trabajo y/o estacion estan vacios en el excel. Favor verificar. \r\n";

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                    mensaje = mensaje + "En La orden " + codigo_orden + " El tipo trabajo y/o estacion no se encuentran registrados. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                }

                                                            }
                                                            else
                                                            {
                                                                iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                mensaje = mensaje + "En La orden " + codigo_orden + " El campo tipo trabajo y/o estacion las columnas estan vacias. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            iContadorExistente = iContadorExistente + 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var OTRepetido = db.MT_OrdenTrabajo.Where(x => x.Codigo_Cliente == codigo_orden && x.Id_Prospecto == contrato_excel && x.EstadoEditOrden == "A").ToList();

                                                        if (OTRepetido.Count() <= 0)
                                                        {
                                                            if (variable != "")
                                                            {
                                                                //set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contrato_excel)                                                    
                                                                //select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato
                                                                var client_contr = db.MT_Prospecto.Where(vq => vq.Id_Prospecto == contrato_excel).FirstOrDefault().Id_Cliente;
                                                                //var client_contr = db.MT_Contrato.Where(vq => vq.Id_Contrato == contrato_excel).FirstOrDefault().Categoria;
                                                                var trabjservplan = from tipotrab in db.MT_TipoTrabajo.Where(vq => vq.Descripcion == variable && vq.Estado == "A" && vq.Id_Cliente == client_contr)
                                                                                    join tiposect in db.MT_Servicio.Where(vq => vq.Id_Empresa == empresa_id.ToString())
                                                                                    on tipotrab.Id_Servicio equals tiposect.Id_Servicio
                                                                                    select new { tipotrab.Id_TipoTrabajo };


                                                                var trabajoservicioplanlist = trabjservplan.ToList();

                                                                if (trabajoservicioplanlist.Count > 0)
                                                                {
                                                                    if (trabajoservicioplanlist.ElementAt(0).Id_TipoTrabajo != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            var ac = database4.IngresarOrdenes_Excel_Aut(codigo_orden, Estado, fecha_creacion_, fecha_asignacion_, inicio_ej_, fecha_finalizacion_obra, variable, variablest, usuario, contrato_excel, empresa_id.ToString(), automatizacion, Origen);
                                                                            if (ac != -1)
                                                                            {
                                                                                iContadorIngresado = iContadorIngresado + 1;
                                                                            }
                                                                            else
                                                                            {
                                                                                iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                                mensaje = mensaje + "La orden " + codigo_orden + "El tipo trabajo y/o estacion no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                            }
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                            mensaje = mensaje + "La orden " + codigo_orden + "El tipo trabajo y/o estacion no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                            System.Diagnostics.Debug.WriteLine("Error insert orden Automat" + e.ToString());
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                        mensaje = mensaje + "En La orden " + codigo_orden + " Los campos tipo trabajo y/o estacion estan vacios en el excel. Favor verificar. \r\n";

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                    mensaje = mensaje + "En La orden " + codigo_orden + " El tipo trabajo y/o estacion no se encuentran registrados. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                }

                                                            }
                                                            else
                                                            {
                                                                iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                mensaje = mensaje + "En La orden " + codigo_orden + " El campo tipo trabajo y/o estacion las columnas estan vacias. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            iContadorExistente = iContadorExistente + 1;
                                                        }
                                                    }

                                                }


                                            }
                                        }
                                        iContador++;
                                    }

                                    //DataBase_Externo dba_Actividades = new DataBase_Externo();

                                    foreach (var item in lista_aux_ordenes)
                                    {
                                        int contador_actividad = 0;
                                        foreach (var item_totales in lista_ordenes_totales.Where(vq => vq.oatom_id_aux_total == item))//.oatom_id_aux))
                                        {
                                            try
                                            {


                                                contador_actividad++;
                                                var id_orden_trabajo = db.MT_OrdenTrabajo.Where(vq => vq.Codigo_Cliente == item).ToList();//.oatom_id_aux).ToList();






                                                if (id_orden_trabajo.Count != 0)
                                                {
                                                    var id_ordentrab_act = id_orden_trabajo.ElementAt(0).Id_OrdenTrabajo;
                                                    var OTRepetido_Act = db.MT_OrdenTrabajo_Actividad.Where(x => x.Id_Orden_Trabajo == id_ordentrab_act).ToList();

                                                    if (OTRepetido_Act.Count() <= 0)
                                                    {




                                                        MT_OrdenTrabajo_Actividad mt_ordentrabajao_actividad_excel = new MT_OrdenTrabajo_Actividad
                                                        {
                                                            Id_Orden_Trabajo = id_orden_trabajo.ElementAt(0).Id_OrdenTrabajo,
                                                            Id_Actividad = 0,
                                                            Estado = 69,
                                                            Orden = contador_actividad,
                                                            Fecha_Ini = Convert.ToDateTime(item_totales.inicio_ej),
                                                            Fecha_Fin = Convert.ToDateTime(item_totales.fin_ej),
                                                            EstadoAct = "A",
                                                            Id_Usuario = usuario,
                                                            Fecha = Convert.ToDateTime(item_totales.inicio_ej),
                                                            Nombre_Actividad = item_totales.nombre_actividad


                                                        };
                                                        db.MT_OrdenTrabajo_Actividad.Add(mt_ordentrabajao_actividad_excel);
                                                        db.SaveChanges();
                                                    }
                                                }
                                            }
                                            catch (Exception e)
                                            {

                                                throw;
                                            }

                                        }
                                    }

                                    //string folder = @"C:\Users\Administrador2\Downloads\Errores_Excel";

                                    string folder = Server.MapPath(@"~/Errores_Excel");
                                    //var folder = Server.MapPath("~/Content/OT_");
                                    if (!Directory.Exists(folder))
                                    {
                                        Directory.CreateDirectory(folder);

                                    }
                                    var f = DateTime.Now.ToString();
                                    f = f.Replace("/", "-"); f = f.Replace(":", "_");
                                    string filePath = System.IO.Path.Combine(folder, "fichero" + f + ".txt");

                                    using (StreamWriter writer = System.IO.File.CreateText(filePath))
                                    {
                                        writer.WriteLine(DateTime.Now + "," + mensaje + " \r\n.");

                                    }

                                }
                            }
                            //TempData["mensaje_error2"] = "Total de registros" +iContador+ ":" +iContadorNoIngresado+ " no ingresados," +iContadorExistente+ " registros existentes, " +iContadorIngresado+ " registrados correctamente";
                            TempData["mensaje_error2"] = "Total de registros" + iContador + ":" + iContadorIngresado + "cargados con éxito," + iContadorNoIngresado + " no cargados, " + iContadorExistente + "existentes";

                            //ViewBag.mensaje = "Fueron" + iContador + " registros :" + iContadorNoIngresado + " no ingresados de los cuales este es el posible error: " + mensaje + ", " + iContadorExistente + " existentes, " + iContadorIngresado + " registrados correctamente";
                            return RedirectToAction("OrdenTrabajo_Automatizacion");
                            //return View("OrdenTrabajo_Automatizacion");
                        }
                        else
                        {
                            TempData["mensaje_error"] = "Tipo de archivo es incorrecto<br>";
                            return RedirectToAction("OrdenTrabajo_Automatizacion");
                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Debe seleccionar un contrato";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "Tipo de archivo es incorrecto, verificar la estructura del archivo";
                //return RedirectToAction("Error", "Errores");
                Console.Write(e.Message); return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
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
                                return RedirectToAction("OrdenTrabajo_Automatizacion", new { id = idOT });
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
                                    string patharchivo = Server.MapPath(ruta_archivo);
                                    if (!Directory.Exists(patharchivo))
                                    {
                                        Directory.CreateDirectory(patharchivo);
                                    }

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
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
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
                        var user_id = System.Web.HttpContext.Current.Session["usuario"];

                        var listaActividades_OrdenTrabajo = db.Sp_MtOrdenTActividad(mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado, mT_OrdenTrabajo.Id_OrdenTrabajo, user_id.ToString()).ToList();
                        //var primer_actividad = db.MT_Actividad.Where(x => x.Id_TipoTrabajo == mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado).Select(o => o.Id_Actividad).First();

                        var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                        ViewBag.listaActividades_OrdenTrabajo = listaActividades_OrdenTrabajo;
                        ViewBag.orden_trabajo = orden_trabajo;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                System.Web.HttpContext.Current.Session["Fecha_creacion"] = mT_OrdenActividad.Fecha;
                System.Web.HttpContext.Current.Session["Id_usuario_creacion"] = mT_OrdenActividad.Id_Usuario;

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
        public ActionResult Editar_Actividades_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Actividad, Id_Orden_Trabajo, Estado,Orden,Comentario, Fecha_Ini, Fecha_Fin,Id_Usuario_Modifica,Fecha_Modifica")] MT_OrdenTrabajo_Actividad mT_OrdenActividad)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_actividad = System.Web.HttpContext.Current.Session["id_actividad"];
                var fecha = System.Web.HttpContext.Current.Session["Fecha_creacion"];
                var Id_usuario_creacion = System.Web.HttpContext.Current.Session["Id_usuario_creacion"];
                var user_id_edit = System.Web.HttpContext.Current.Session["usuario"];


                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                int idMActividad = int.Parse(id_actividad.ToString());

                if (ModelState.IsValid)
                {
                    mT_OrdenActividad.Id_Actividad = idMActividad;
                    mT_OrdenActividad.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenActividad.Fecha = Convert.ToDateTime(fecha);
                    mT_OrdenActividad.Id_Usuario = Id_usuario_creacion.ToString();
                    mT_OrdenActividad.Fecha_Modifica = DateTime.Now;
                    mT_OrdenActividad.Id_Usuario_Modifica = user_id_edit.ToString();

                    var estado_orden = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == idOrdenTrabajo).ToList();

                    if (estado_orden.Count() > 0)
                    {
                        if (estado_orden.LastOrDefault().Estado_Orden_Trabajo != 32 && estado_orden.LastOrDefault().Estado_Orden_Trabajo != 33)
                        {
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

                        }
                        else
                        {
                            TempData["mensaje_actualizado"] = "Orden Actividad no puede editarse porque la orden esta Desestimada y/o Bloqueada";
                            return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                        }
                    }
                    else
                    {
                        TempData["mensaje_actualizado"] = "Orden Actividad no puede editarse porque la orden no cuenta con estados";
                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                    }

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
                var nombre_actividad_desalojo = db.MT_Actividad.Where(x => x.Id_Actividad == mT_OrdenActividad.Id_Actividad).ToList();
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                var grupo_trabajo_orden_inte = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == idOrdenTrabajo).ToList();
                var grupo_trabajo_orden_grupo = db.MT_OrdenTrabajo_Equipo.Where(x => x.Id_Orden_Trabajo == idOrdenTrabajo).ToList();
                var estado_orden = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == idOrdenTrabajo).ToList();
                var fecha_asignacion_grupo = db.MT_OrdenTrabajo.Where(x => x.Id_OrdenTrabajo == idOrdenTrabajo).ToList();

                if (estado_orden.Count() > 0)
                {
                    if (estado_orden.LastOrDefault().Estado_Orden_Trabajo != 32 && estado_orden.LastOrDefault().Estado_Orden_Trabajo != 33)
                    {
                        if (grupo_trabajo_orden_inte.Count() > 0 && grupo_trabajo_orden_grupo.Count() > 0)
                        {
                            if (grupo_trabajo_orden_inte.ElementAt(0).Id_GrupoTrabajo != null && grupo_trabajo_orden_grupo.ElementAt(0).Id_GrupoTrabajo != null)
                            {

                                if (fecha_asignacion_grupo != null)
                                {

                                    if (mT_OrdenActividad.Fecha_Ini == null)
                                    {

                                        mT_OrdenActividad.Fecha_Ini = DateTime.Now;
                                        mT_OrdenActividad.Estado = 68;
                                        //mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;

                                        //MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                        //if (mT_OrdenTrabajo.Fecha_inicio_ejecucion == null)
                                        //{
                                        //    mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;
                                        //    mT_OrdenTrabajo.Estado = 31;
                                        //}

                                        db.Entry(mT_OrdenActividad).State = EntityState.Modified;
                                        //db.Entry(mT_OrdenTrabajo).State = EntityState.Modified;
                                        db.SaveChanges();

                                    }
                                    else
                                    {
                                        TempData["mensaje_actualizado"] = "Orden Actividad Ya fue iniciada";
                                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                                    }
                                }
                                else
                                {
                                    TempData["mensaje_actualizado"] = "Orden Actividad no puede ser iniciada porque no cuenta con una fecha de asignacion grupo de trabajo";
                                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                                }
                            }
                        }
                        else
                        {
                            TempData["mensaje_actualizado"] = "No puede iniciar actividad, debe asignar la Orden a un Grupo de Trabajo";
                            return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                        }
                    }
                    else
                    {
                        TempData["mensaje_actualizado"] = "Orden Actividad no puede iniciarse porque la orden esta Desestimada y/o Bloqueada";
                        return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                    }
                }
                else
                {
                    TempData["mensaje_actualizado"] = "Orden Actividad no puede iniciarse porque la orden no contiene estados";
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

        [HttpPost]
        public ActionResult IniciarActividadOrdenTrabajo_Json(int id)
        {
            try
            {
                var mensaje = new JsonResult();
                MT_OrdenTrabajo_Actividad mT_OrdenActividad = db.MT_OrdenTrabajo_Actividad.Find(id);
                var nombre_actividad_desalojo = db.MT_Actividad.Where(x => x.Id_Actividad == mT_OrdenActividad.Id_Actividad).ToList();
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                var grupo_trabajo_orden_inte = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == idOrdenTrabajo).ToList();
                var grupo_trabajo_orden_grupo = db.MT_OrdenTrabajo_Equipo.Where(x => x.Id_Orden_Trabajo == idOrdenTrabajo).ToList();
                var estado_orden = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == idOrdenTrabajo).ToList();
                var fecha_asignacion_grupo = db.MT_OrdenTrabajo.Where(x => x.Id_OrdenTrabajo == idOrdenTrabajo).ToList();


                if (estado_orden.Count() > 0)
                {
                    if (estado_orden.LastOrDefault().Estado_Orden_Trabajo != 32 && estado_orden.LastOrDefault().Estado_Orden_Trabajo != 33)
                    {
                        if (grupo_trabajo_orden_inte.Count() > 0 && grupo_trabajo_orden_grupo.Count() > 0)
                        {
                            if (grupo_trabajo_orden_inte.ElementAt(0).Id_GrupoTrabajo != null && grupo_trabajo_orden_grupo.ElementAt(0).Id_GrupoTrabajo != null)
                            {
                                if (fecha_asignacion_grupo != null)
                                {


                                    if (mT_OrdenActividad.Fecha_Ini == null)
                                    {

                                        mT_OrdenActividad.Fecha_Ini = DateTime.Now;
                                        mT_OrdenActividad.Estado = 68;
                                        //mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;

                                        //MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                        //if (mT_OrdenTrabajo.Fecha_inicio_ejecucion == null)
                                        //{
                                        //    mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;
                                        //    mT_OrdenTrabajo.Estado = 31;
                                        //}

                                        db.Entry(mT_OrdenActividad).State = EntityState.Modified;
                                        //db.Entry(mT_OrdenTrabajo).State = EntityState.Modified;
                                        db.SaveChanges();

                                    }
                                    else
                                    {
                                        mensaje.Data = "Orden Actividad Ya fue iniciada";
                                        //TempData["mensaje_actualizado"] = "Orden Actividad Ya fue iniciada";
                                        //return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                                        return mensaje;
                                    }
                                }
                                else
                                {
                                    TempData["mensaje_actualizado"] = "Orden Actividad no puede ser iniciada porque no cuenta con una fecha de asignacion grupo de trabajo";
                                    return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                                }

                            }
                        }
                        else
                        {
                            //TempData["mensaje_actualizado"] = "No puede iniciar actividad, debe asignar la Orden a un Grupo de Trabajo";
                            mensaje.Data = "No puede iniciar actividad, debe asignar la Orden a un Grupo de Trabajo";
                            //return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                            return mensaje;
                        }
                    }
                    else
                    {
                        mensaje.Data = "Orden Actividad no puede iniciarse porque la orden esta Desestimada y/o Bloqueada";
                        //TempData["mensaje_actualizado"] = "Orden Actividad no puede iniciarse porque la orden esta Desestimada y/o Bloqueada";
                        //return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                        return mensaje;
                    }
                }
                else
                {
                    mensaje.Data = "Orden Actividad no puede iniciarse porque la orden no contiene estados";
                    //TempData["mensaje_actualizado"] = "Orden Actividad no puede iniciarse porque la orden no contiene estados";
                    //return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                    return mensaje;
                }
                mensaje.Data = "Orden Actividad Iniciada";
                //TempData["mensaje_actualizado"] = "Orden Actividad Iniciada";
                //return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividad.Id_Orden_Trabajo });
                return mensaje;

            }
            catch (Exception)
            {
                //return RedirectToAction("Error", "Errores");
                return Json(Response.StatusCode);
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

                        var estado_orden = db.MT_OrdenTrabajo_Estado.Where(x => x.Id_orden_Trabajo == idOrdenTrabajo).ToList();

                        if (estado_orden.Count() > 0)
                        {
                            if (estado_orden.LastOrDefault().Estado_Orden_Trabajo != 32 && estado_orden.LastOrDefault().Estado_Orden_Trabajo != 33)
                            {
                                var lkProyProrroga = db.MT_OrdenTrabajo_Actividad.Where(x => x.Id_OrdenTrabajo_Actividad == mT_OrdenActividadFinalizada.Id_OrdenTrabajo_Actividad);
                                if (lkProyProrroga.Count() > 0)
                                {
                                    foreach (var itemProrroga in lkProyProrroga)
                                    {
                                        itemProrroga.Estado = mT_OrdenActividadFinalizada.Estado;
                                        itemProrroga.Fecha_Fin = DateTime.Now;
                                        MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                        mT_OrdenTrabajo.Fecha_fin_ejecucion = DateTime.Now;
                                        //var fecha_inicio = mT_OrdenTrabajo.Fecha_inicio_ejecucion;



                                    }
                                    db.SaveChanges();

                                    //***
                                    var liOT = db.MT_OrdenTrabajo.Where(vq => vq.Id_OrdenTrabajo == idOrdenTrabajo).ToList();
                                    foreach (var item in liOT)
                                    {
                                        item.Tiempo_transcurrido = Obtener_Tiempo(idOrdenTrabajo);
                                    }
                                    //db.SaveChanges();
                                    //*****
                                }
                            }
                            else
                            {
                                mT_OrdenActividadFinalizada.Id_Orden_Trabajo = idOrdenTrabajo;
                                TempData["mensaje_actualizado"] = "Actividad no puede finalizarse porque la orden tiene esta desestimada y/o bloqueada";

                                return RedirectToAction("Actividades_OrdenTrabajo", new { id = mT_OrdenActividadFinalizada.Id_Orden_Trabajo });
                            }
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

        [HttpGet]
        public ActionResult Operadores_ActividadOrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo_Actividad mT_OrdenActividadOperador = db.MT_OrdenTrabajo_Actividad.Find(id);
                System.Web.HttpContext.Current.Session["id_actividad"] = mT_OrdenActividadOperador.Id_Actividad;
                //System.Web.HttpContext.Current.Session["id_Orden"] = mT_OrdenActividadOperador.Id_Orden_Trabajo;
                System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] = mT_OrdenActividadOperador.Fecha_Ini;
                System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] = mT_OrdenActividadOperador.Fecha_Fin;

                bool seleccion = false;
                if (mT_OrdenActividadOperador != null)
                {
                    List<SelectListItem> itemsOperadores = new List<SelectListItem>();

                    //var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();
                    var listaOperadores = db.MT_TablaDetalle.Where(x => x.Id_Tabla == 1065).ToList();

                    foreach (var operadores in listaOperadores)
                    {
                        if (operadores.Id_TablaDetalle == mT_OrdenActividadOperador.Estado)
                        {
                            seleccion = true;
                        }

                        itemsOperadores.Add(new SelectListItem { Value = Convert.ToString(operadores.Id_TablaDetalle), Text = operadores.Descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaOperadoresActividadesOrden = new SelectList(itemsOperadores, "Value", "Text");

                    ViewBag.listaOperadores = selectlistaOperadoresActividadesOrden;
                    ViewBag.id_orden = mT_OrdenActividadOperador.Id_Orden_Trabajo;

                    return View(mT_OrdenActividadOperador);
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


        [HttpPost]
        public ActionResult Operadores_ActividadOrdenTrabajo(int Id_OrdenTrabajo_Actividad, List<string> Operadores_LI)
        {
            try
            {
                string operadores = string.Empty;
                for (int i = 0; i < Operadores_LI.Count; i++)
                {
                    string sep = (i == Operadores_LI.Count - 1) ? "" : ";";
                    operadores += Operadores_LI[i] + sep;
                }
                MT_OrdenTrabajo_Actividad mt_Upd_mtOrdenActividadOp = db.MT_OrdenTrabajo_Actividad.Where(x => x.Id_OrdenTrabajo_Actividad == Id_OrdenTrabajo_Actividad).FirstOrDefault();
                mt_Upd_mtOrdenActividadOp.Responsable = operadores;

                db.Entry(mt_Upd_mtOrdenActividadOp).State = EntityState.Modified;
                db.SaveChanges();

                var mensaje = new JsonResult();

                mensaje.Data = "Ok";

                return mensaje;

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        public TimeSpan Obtener_Tiempo(int Id_OrdenTrabajo)
        {
            var liActividad = db.MT_OrdenTrabajo_Actividad.Where(vq => vq.Id_Orden_Trabajo == Id_OrdenTrabajo && vq.Fecha_Fin != null).ToList();
            TimeSpan hf = new TimeSpan(0, 0, 0);
            double horas = 0;
            foreach (var item in liActividad)
            {
                DateTime fi = Convert.ToDateTime(item.Fecha_Ini);
                DateTime ff = Convert.ToDateTime(item.Fecha_Fin);
                horas += (ff - fi).TotalHours;
            }
            return TimeSpan.FromHours(horas);
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
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
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
                    var ordentrabajo_cliente = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;
                    ViewBag.idMTOrdenTrabajo = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    ViewBag.ordentrabajocliente = ordentrabajo_cliente;
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
                                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(idOrdenTrabajo);

                                mT_OrdenTrabajo.Fecha_asignacion_grupo_trabajo = mT_OrdenTrabajoIntegrante.Fecha_Inicio;
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
                var mT_OrdenTrabajoIntegrante = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajo.Id_OrdenTrabajo);
                if (mT_OrdenTrabajoIntegrante != null)
                {
                    System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    foreach (var itemProrroga in mT_OrdenTrabajoIntegrante)
                    {
                        System.Web.HttpContext.Current.Session["id_GrupoTrabajo"] = itemProrroga.Id_GrupoTrabajo;
                    }

                    var listaIntegrantes_OrdenTrabajo = db.sp_Quimipac_ConsultaInsMT_IntegranteOrdenTrabajo(mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaIntegrantes_OrdenTrabajo = listaIntegrantes_OrdenTrabajo;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
                }
            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "No se ha asignado un grupo de trabajo";
                return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                    foreach (var itemProrroga in mT_OrdenTrabajoEquipo)
                    {
                        System.Web.HttpContext.Current.Session["id_GrupoTrabajoE"] = itemProrroga.Id_GrupoTrabajo;
                    }

                    var listaEquiposOrdenTrabajo = db.sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo(mT_OrdenTrabajo.Id_OrdenTrabajo).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaEquiposOrdenTrabajo = listaEquiposOrdenTrabajo;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
                }

            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "No se ha asignado un grupo de trabajo";
                return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    var listaItems_OrdenTrabajo = db.sp_Quimipac_ConsultaInsMt_OrdenTrabajoEgreso(mT_OrdenTrabajo.Id_tipo_trabajo_ejecutado, mT_OrdenTrabajo.Id_OrdenTrabajo, user_id.ToString()).ToList();
                    var orden_trabajo = mT_OrdenTrabajo.Codigo_Cliente;
                    ViewBag.orden_trabajo = orden_trabajo;
                    ViewBag.listaItems_OrdenTrabajo = listaItems_OrdenTrabajo;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                var usuario_crea = System.Web.HttpContext.Current.Session["usuario"];

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
                            mT_OrdenEgreso.Fecha = DateTime.Now;
                            mT_OrdenEgreso.Id_Usuario = usuario_crea.ToString();
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
                System.Web.HttpContext.Current.Session["Fecha"] = mT_OrdenEgreso.Fecha;
                System.Web.HttpContext.Current.Session["Id_usuario_creacion"] = mT_OrdenEgreso.Id_Usuario;

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
        public ActionResult Editar_Items_OrdenTrabajo([Bind(Include = "Id_OrdenTrabajo_Egreso,Fecha,Moneda,Unidad,Cantidad,Precio,Costo,Area,Tipo,Id_Usuario_Modifica,Fecha_Modifica")] MT_OrdenTrabajo_Egreso mT_OrdenItems)
        {
            try
            {
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                var id_Item = System.Web.HttpContext.Current.Session["id_Item"];
                int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                var fecha = System.Web.HttpContext.Current.Session["Fecha"];
                var useri_id_crea = System.Web.HttpContext.Current.Session["Id_usuario_creacion"];
                var usuario_edit = System.Web.HttpContext.Current.Session["usuario"];
                string idItem = id_Item.ToString();
                if (ModelState.IsValid)
                {
                    mT_OrdenItems.Id_Item = idItem;
                    mT_OrdenItems.Id_Orden_Trabajo = idOrdenTrabajo;
                    mT_OrdenItems.EstadoAct = "A";
                    mT_OrdenItems.Fecha = Convert.ToDateTime(fecha);
                    mT_OrdenItems.Id_Usuario = useri_id_crea.ToString();
                    mT_OrdenItems.Id_Usuario_Modifica = usuario_edit.ToString();
                    mT_OrdenItems.Fecha_Modifica = DateTime.Now;
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
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
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
            var tipo_trabajo_ejecutado = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipoTrabajo"]);
            try
            {
                List<SelectListItem> itemsMedida = new List<SelectListItem>();
                var listaMedidas = db.sp_Quimipac_Consulta_TipoTrabajoMedida().Where(x => x.Id_Tipo_Trabajo == tipo_trabajo_ejecutado).ToList();

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

        //Busqueda por parametros Canvas ORDEN TRABAJO 1ER POST
        #region
        [HttpPost]
        public ActionResult OrdenTrabajo_Automatizacion([Bind(Include = "Fecha_asignacion_grupo_trabajo, Fecha_asignacion, Fecha_registro,Origen,Id_tipo_trabajo_ejecutado,Id_campamento,Id_estacion,Id_sector,Id_sucursal,Id_contrato,Estado,cod_cli, Etapa, Id_OrdenTrabajo")] Orden_Trabajo_FiltCli ORDEN)
        {
            try
            {
                var bde = new DataBase_Externo();
                var usuario = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                //var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion_grupo_trabajo, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_contrato, ORDEN.Estado, ORDEN.cod_cli, ORDEN.Etapa, ORDEN.Id_OrdenTrabajo);
                var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_contrato, ORDEN.Estado, ORDEN.cod_cli, ORDEN.Etapa, ORDEN.Id_OrdenTrabajo);

                /**/

                //var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(ORDEN.Id_usuario , empresa_id.ToString(), "CLIENTES").ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                var listakContrato = db.MT_Contrato.Where(vq => ContratosLI.Contains(vq.Id_Cliente)).ToList();
                //listakContrato = listakContrato.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                lkbusqueda = lkbusqueda.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();

                /**/
                //foreach (var item in lkbusqueda)
                //{
                //    //PROCESO \ ALERTA \ CAIDA
                //    item.Estado_P_A_C = IsValidParametro(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);
                //}

                var lista_Proceso = lkbusqueda.Where(vq => vq.Estado_P_A_C == "EN PROCESO").OrderByDescending(vq => vq.Fecha_registro).ToList();// lista_Proceso;
                var lista_Alerta = lkbusqueda.Where(vq => vq.Estado_P_A_C == "ALERTA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Alerta;
                var lista_Caida = lkbusqueda.Where(vq => vq.Estado_P_A_C == "CAIDA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Caida;

                lkbusqueda = new List<ParametrosBusquedaOT>();
                string orderby = System.Web.HttpContext.Current.Session["OrderBy_OT"] == null ? "DESC" : System.Web.HttpContext.Current.Session["OrderBy_OT"].ToString();


                orderby = orderby.ToUpper();
                if (orderby == "ASC")
                {
                    OrderBy_ListaParametro_Filtro(lista_Proceso, lkbusqueda);
                    OrderBy_ListaParametro_Filtro(lista_Alerta, lkbusqueda);
                    OrderBy_ListaParametro_Filtro(lista_Caida, lkbusqueda);
                }
                /*else if (orderby == "ALERTA")
                {
                    OrderBy_ListaParametro(lista_Alerta, listaOrdenTrabajo);
                    OrderBy_ListaParametro(lista_Caida, listaOrdenTrabajo);
                    OrderBy_ListaParametro(lista_Proceso, listaOrdenTrabajo);
                }*/
                else if (orderby == "DESC")
                {
                    OrderBy_ListaParametro_Filtro(lista_Caida, lkbusqueda);
                    OrderBy_ListaParametro_Filtro(lista_Alerta, lkbusqueda);
                    OrderBy_ListaParametro_Filtro(lista_Proceso, lkbusqueda);
                }

                /**/

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
                else if (ORDEN.Origen.Equals("Etapa")) { XParametro = ORDEN.Etapa; }
                else if (ORDEN.Origen.Equals("OrdenPadre")) { XParametro = ORDEN.Id_OrdenTrabajo.ToString(); }

                //ViewBag.ParametroBusqueda = ORDEN.Fecha_asignacion_grupo_trabajo + ";" + ORDEN.Fecha_registro + ";" + ORDEN.Origen;
                //TempData["ParametroBusqueda"] = ORDEN.Fecha_asignacion_grupo_trabajo + " " + ORDEN.Fecha_registro + " " + ORDEN.Origen + " " + XParametro;
                TempData["ParametroBusqueda"] = ORDEN.Fecha_asignacion + " " + ORDEN.Fecha_registro + " " + ORDEN.Origen + " " + XParametro;


                ViewBag.listaEtapas = orderby;
                return RedirectToAction("OrdenTrabajo_Automatizacion");//, "OrdenTrabajo_Automatizacion");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
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
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
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
            string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
            try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }

                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                ViewBag.listaClientes = selectlistaClientes;
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

            return RedirectToAction("Entrega_OrdenTrabajo");
        }


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
                SelectList selectlistaOrdenTrabajo = new SelectList(ordenTrabajo, "Value", "Text");
                return Json(selectlistaOrdenTrabajo, JsonRequestBehavior.AllowGet);
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
                DataBase_Externo dbe = new DataBase_Externo();
                var band = dbe.InsertarEntregaOrden(lkOrden, System.Web.HttpContext.Current.Session["empresa"].ToString(), ValueCliente, ValueFechaEnvio, ValueFechaRecepcion, System.Web.HttpContext.Current.Session["usuario"].ToString(), Comentario, Recibido_Por);
                if (band < 1)
                {
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                    return mensaje;
                }
                mensaje.Data = "Registros guardados correctamente";
                return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        /*
        public class TempEntregaOrden
        {
            public int Id_OrdenTrabajo { get; set; }
            public string Codigo_Cliente { get; set; }

        }
        */
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
                }
                else
                {
                    mensaje.Data = "Registros guardados correctamente";
                }
                return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        public class Temp_EditarCheckTotal
        {
            public int Id_OrdenTrabajo { get; set; }
        }
        public class Temp_EditarCheck
        {
            public int Id_OrdenTrabajoToT { get; set; }
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

                bool seleccion = false;
                if (mt_OrdenEntrega != null)
                {

                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    var listaFecha = db.SP_Quimipac_ConsultaClientesOrdenCont(id, empresa_id.ToString()).ToList();

                    foreach (var items in listaFecha)
                    {
                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(items.Id_Entrega_Orden_Trabajo), Text = items.Fecha_Elaboracion.ToString() });
                    }

                    SelectList selectlistaItemsFecha = new SelectList(itemsEstado, "Value", "Text");

                    ViewBag.listaFecha = selectlistaItemsFecha;
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
                        return RedirectToAction("OrdenTrabajo_Automatizacion", new { id = idMantOrdenTrabajo });
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
                        mT_OrdenTrabajoEntrega.Id_OrdenTrabajo = idOrdenTrabajo;

                        var lkEntregaOrden = db.MT_OrdenTrabajo.Where(x => x.Id_OrdenTrabajo == mT_OrdenTrabajoEntrega.Id_OrdenTrabajo);
                        if (lkEntregaOrden.Count() > 0)
                        {
                            foreach (var itemOrdenEntrega in lkEntregaOrden)
                            {
                                itemOrdenEntrega.Id_entrega_orden_trabajo = mT_OrdenTrabajoEntrega.Id_entrega_orden_trabajo;
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        mT_OrdenTrabajoEntrega.Id_OrdenTrabajo = idOrdenTrabajo;
                        TempData["mensaje_actualizado"] = "Orden Entrega No ha sido iniciada";
                        return RedirectToAction("OrdenTrabajo_Automatizacion", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });
                    }

                    TempData["mensaje_actualizado"] = "Orden entrega actualizada";
                    return RedirectToAction("OrdenTrabajo_Automatizacion", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });
                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("OrdenTrabajo_Automatizacion", new { id = mT_OrdenTrabajoEntrega.Id_OrdenTrabajo });
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

        //Orden Trabajo  Valor
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
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Valor Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
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
                        var codigo_valor_costo = db.MT_TablaDetalle.Where(x => x.Id_TablaDetalle == mT_OrdenTrabajoValor.Tipo_Valor).ToList();
                        mT_OrdenTrabajoValor.Codigo_Valor_Costo = codigo_valor_costo.ElementAt(0).Codigo;
                        var itemOrdenTrabajo = db.MT_OrdenTrabajo_Valor.Where(x => x.Id_Orden_Trabajo == mT_OrdenTrabajoValor.Id_Orden_Trabajo && x.Id_Item == mT_OrdenTrabajoValor.Id_Item && x.Moneda == mT_OrdenTrabajoValor.Moneda && x.Cantidad == mT_OrdenTrabajoValor.Cantidad);
                        if (itemOrdenTrabajo.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Orden Trabajo Valor ya existe";
                            return RedirectToAction("OrdenTrabajo_Valor", new { id = idOrdenTrabajo });
                        }
                        else
                        {
                            var codigo_guardado = db.MT_OrdenTrabajo_Valor.Where(x => x.Tipo_Valor == mT_OrdenTrabajoValor.Tipo_Valor && x.Id_Orden_Trabajo == mT_OrdenTrabajoValor.Id_Orden_Trabajo).ToList().Select(vq => new
                            {
                                vq.Id_Orden_Trabajo,
                                Fecha_Inicio = (vq.Fecha != null ? DateTime.Parse(vq.Fecha.ToString()) : DateTime.Today),
                                vq.Tipo_Valor,
                                vq.Id_Item
                            });


                            int SeqRegistro = 0;
                            int seqregistro2 = 0;

                            if (codigo_guardado.Count() > 0)
                            {
                                codigo_guardado = codigo_guardado.Where(vq => vq.Fecha_Inicio.Year == DateTime.Now.Year).Take(1).OrderByDescending(vq => vq.Fecha_Inicio);
                                SeqRegistro = codigo_guardado.Count() + 1;


                                seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());

                            }
                            else
                            {
                                SeqRegistro = 1;
                                seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                                //Codigo cliente nuevo
                            }
                            mT_OrdenTrabajoValor.Secuencia = seqregistro2;
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
                System.Web.HttpContext.Current.Session["id_usuario_crea"] = mT_ordenTrabajoValor.Id_Usuario;
                System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_ordenTrabajoValor.Fecha;
                System.Web.HttpContext.Current.Session["secuencia"] = mT_ordenTrabajoValor.Secuencia;

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
        public ActionResult Editar_OrdenTrabajo_Valor([Bind(Include = "Id_OrdenTrabajo_Valor,Id_Orden_Trabajo, Id_Usuario, Tipo_Valor, Id_Item,Moneda,Fecha,Unidad,Cantidad,Precio,Costo,IVA,Id_Usuario_Modifica,Fecha_Modifica")] MT_OrdenTrabajo_Valor mT_OrdenTrabajoValor)
        {
            try
            {
                var user_id = System.Web.HttpContext.Current.Session["id_usuario_crea"];
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
                var fecha = System.Web.HttpContext.Current.Session["Fecha"];
                var usuario_edit = System.Web.HttpContext.Current.Session["usuario"];
                var secuencia = System.Web.HttpContext.Current.Session["secuencia"];


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
                        mT_OrdenTrabajoValor.Id_Usuario_Modifica = usuario_edit.ToString();
                        mT_OrdenTrabajoValor.Fecha_Modifica = DateTime.Now;
                        mT_OrdenTrabajoValor.Secuencia = Convert.ToInt32(secuencia.ToString());
                        var codigo_valor_costo = db.MT_TablaDetalle.Where(x => x.Id_TablaDetalle == mT_OrdenTrabajoValor.Tipo_Valor).ToList();
                        mT_OrdenTrabajoValor.Codigo_Valor_Costo = codigo_valor_costo.ElementAt(0).Codigo;
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
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Estado Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
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
        public ActionResult Agregar_OrdenTrabajo_Estado([Bind(Include = "Id_Orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin,Comentario")] MT_OrdenTrabajo_Estado mT_OrdenTrabajoEstado)
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
                            DataBase_Externo dbe = new DataBase_Externo();
                            var resp = dbe.ActualizarOrdenTrabajo(mT_OrdenTrabajoEstado.Id_orden_Trabajo, mT_OrdenTrabajoEstado.Estado_Orden_Trabajo, mT_OrdenTrabajoEstado.Fecha_Ini, mT_OrdenTrabajoEstado.Fecha_Fin, mT_OrdenTrabajoEstado.Comentario);
                            if (resp > 0)
                            {
                                TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                            }
                            else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }
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
        public ActionResult Editar_OrdenTrabajo_Estado([Bind(Include = "Id_OrdenTrabajo_Estado,Id_Orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO, Comentario")] MT_OrdenTrabajo_Estado mT_OrdenTrabajoEstado)
        {
            try
            {
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_Orden"];
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
                        DataBase_Externo dbe = new DataBase_Externo();
                        var resp = dbe.ActualizarOrdenTrabajo(mT_OrdenTrabajoEstado.Id_orden_Trabajo, mT_OrdenTrabajoEstado.Estado_Orden_Trabajo, mT_OrdenTrabajoEstado.Fecha_Ini, mT_OrdenTrabajoEstado.Fecha_Fin, mT_OrdenTrabajoEstado.Comentario);
                        if (resp > 0)
                        {
                            TempData["mensaje_correcto"] = "Orden Trabajo Estado guardado";
                        }
                        else { TempData["mensaje_error"] = "No se guardo correctamente los Estados de la Orden"; }
                        return RedirectToAction("OrdenTrabajo_Estado", new { id = idOrdenTrabajo });
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
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Causa Raiz Orden Trabajo no existe";
                    return RedirectToAction("OrdenTrabajo_Automatizacion");
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

        //Asignar Grupo Orden Trabajo
        #region
        [HttpGet]
        public ActionResult AsignarGrupo_OrdenTrabajo()
        {
            try
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresacod = empresa_id.ToString();
                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                string usuario = user_id.ToString();
                var listagrupoOrdenIntEq = db.sp_Quimipac_ConsultaMT_IntegranteEquipoOrdenGrupoTrabajo(empresacod).ToList();
                ViewBag.listagrupoOrdenIntEq = listagrupoOrdenIntEq;

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        [HttpGet]
        public ActionResult Agregar_AsignarGrupo_OrdenTrabajo()
        {
            string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
            try
            {
                List<SelectListItem> itemsClientes = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                listaClientes = listaClientes.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                foreach (var cliente in listaClientes)
                {
                    itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                }
                SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                List<SelectListItem> itemsGruposTrabajo = new List<SelectListItem>();
                var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo().ToList();
                foreach (var grupotrabajo in listaGrupoTrabajo)
                {
                    itemsGruposTrabajo.Add(new SelectListItem { Value = Convert.ToString(grupotrabajo.Id_GrupoTrabajo), Text = grupotrabajo.Nombre });
                }
                SelectList selectlistaGrupoTrabajo = new SelectList(itemsGruposTrabajo, "Value", "Text");

                ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;
                ViewBag.listaClientes = selectlistaClientes;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_AsignarGrupo_OrdenTrabajo([Bind(Include = "Id_Orden_Trabajo, Fecha_Inicio, Fecha_Fin, Estado, Id_GrupoTrabajo, Id_Cliente")] OrdenGrupo_IntegranteEquipoCliente mT_OrdenTrabajoIntegrante)
        {
            return RedirectToAction("AsignarGrupo_OrdenTrabajo");
        }

        public JsonResult GetOrdenTrabajo_Grupo(string id)
        {
            try
            {
                List<SelectListItem> ordenTrabajo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var listaOrden = db.sp_Quimipac_ConsultaMT_Ordenes_no_Grupos(empresa_id.ToString(), id).ToList();

                foreach (var orden in listaOrden)
                {
                    ordenTrabajo.Add(new SelectListItem { Value = Convert.ToString(orden.Id_OrdenTrabajo), Text = orden.Codigo_Cliente + " | " + orden.Descripcion });
                }
                SelectList selectlistaOrdenGrupos = new SelectList(ordenTrabajo, "Value", "Text");
                return Json(selectlistaOrdenGrupos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        public JsonResult InsertarOrden_Grupo(List<TempEntregaOrden_Grupo> lkOrden, string ValueCliente, DateTime ValueFechaInicio, DateTime ValueFechaFin, string ValueGrupo)
        {
            var user_id = System.Web.HttpContext.Current.Session["usuario"];
            var empresa = System.Web.HttpContext.Current.Session["empresa"];
            //var mensajeDiv = new TempMensaje();
            var mensaje = new JsonResult();
            int nRegistro = 0;
            int valuegrupo_ = Convert.ToInt32(ValueGrupo);
            try
            {
                IQueryable<MT_GrupoTrabajoIntegrante> records = db.MT_GrupoTrabajoIntegrante.Where(o => o.Id_GrupoTrabajo == valuegrupo_);
                var count = records.Count();
                IQueryable<MT_GrupoTrabajoEquipo> records2 = db.MT_GrupoTrabajoEquipo.Where(o => o.Id_GrupoTrabajo == valuegrupo_);
                var count2 = records2.Count();
                if (lkOrden != null)
                {
                    foreach (var item in lkOrden)
                    {
                        var idOrdenTrabajo = item.Id_OrdenTrabajo;
                        var ordengrupotrabajo = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_GrupoTrabajo == valuegrupo_ && x.Fecha_Inicio == ValueFechaInicio && x.Fecha_Fin == ValueFechaFin && x.Id_Orden_Trabajo == item.Id_OrdenTrabajo);
                        if (ordengrupotrabajo.Count() >= 1)
                        {
                            mensaje.Data = "Código ya existe!";
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)
                            {
                                DataBase_Externo database2 = new DataBase_Externo();
                                database2.InsertarIntegranteOrdenTrabajo(idOrdenTrabajo, valuegrupo_, ValueFechaInicio, ValueFechaFin);
                            }
                            DataBase_Externo database3 = new DataBase_Externo();
                            int variable = database3.Verificar_EquipoTrabajoOrden(idOrdenTrabajo, valuegrupo_, ValueFechaInicio, ValueFechaFin);
                            if (variable == 0)
                            {
                                database3.InsertarEquipoOrdenTrabajo(idOrdenTrabajo, count2);

                            }
                            mensaje.Data = "Datos ingresados correctamente";
                        }
                    }
                }

                return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        public class TempEntregaOrden_Grupo
        {
            public int Id_OrdenTrabajo { get; set; }
            public string Codigo_Cliente { get; set; }
        }

        [HttpGet]
        public ActionResult Editar_AsignarGrupo_OrdenTrabajo(int id)
        {
            try
            {
                MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                var mT_OrdenTrabajo_intorden = db.MT_OrdenTrabajo_Integrante.Where(x => x.Id_Orden_Trabajo == id && x.Estado == "A").ToList();
                //var clientes = db.MT_Contrato.Find(mT_OrdenTrabajo.Id_contrato);
                var clientes = db.MT_Contrato.Find(mT_OrdenTrabajo.Id_Proyecto);
                System.Web.HttpContext.Current.Session["Contrato"] = clientes;
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                bool seleccion = false;
                if (mT_OrdenTrabajo != null)
                {/*
                    List<SelectListItem> itemsClientes = new List<SelectListItem>();
                    var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                    foreach (var cliente in listaClientes)
                    {
                        if (clientes.Id_Cliente  == cliente.cod_cli)
                        {
                            seleccion = true;
                        }
                        itemsClientes.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                    }
                    SelectList selectlistaClientes = new SelectList(itemsClientes, "Value", "Text");

                    List<SelectListItem> itemsGruposTrabajo = new List<SelectListItem>();
                    var listaGrupoTrabajo = db.sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo().ToList();
                    foreach (var grupotrabajo in listaGrupoTrabajo)
                    {
                        if (mT_OrdenTrabajo_intorden.FirstOrDefault().Id_GrupoTrabajo ==  grupotrabajo.Id_GrupoTrabajo)
                        {
                            seleccion = true;
                        }
                        itemsGruposTrabajo.Add(new SelectListItem { Value = Convert.ToString(grupotrabajo.Id_GrupoTrabajo), Text = grupotrabajo.Nombre, Selected = seleccion });
                    }
                    SelectList selectlistaGrupoTrabajo = new SelectList(itemsGruposTrabajo, "Value", "Text");
                    */
                    /*ViewBag.listaClientes = selectlistaClientes;
                    ViewBag.listaGrupoTrabajo = selectlistaGrupoTrabajo;*/
                    ViewBag.Id_Cliente = new SelectList(db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList(), "cod_cli", "nom_cli", clientes.Id_Cliente);
                    ViewBag.Id_GrupoTrabajo = new SelectList(db.sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo().ToList(), "Id_GrupoTrabajo", "Nombre", mT_OrdenTrabajo_intorden.FirstOrDefault().Id_GrupoTrabajo);
                    ViewBag.Id_Orden = mT_OrdenTrabajo.Id_OrdenTrabajo;
                    //ViewBag.Id_Cliente = clientes.Id_Cliente;
                    ViewBag.ClienteID = clientes.Id_Cliente;
                    ViewBag.fecha_Inicio = mT_OrdenTrabajo_intorden.ElementAt(0).Fecha_Inicio;
                    ViewBag.fecha_Fin = mT_OrdenTrabajo_intorden.ElementAt(0).Fecha_Fin;

                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("AsignarGrupo_OrdenTrabajo");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }
        //Editar_AsignarGrupo_OrdenTrabajo
        public JsonResult Guardar_Editar_AsignarGrupo_OrdenTrabajo(int? Id_ClienteGrupoOrden, List<int> resultadoChecked, List<int> resultadoCheckTotal, int? Id_Cliente, DateTime? FechaEnvio, DateTime? FechaRecep, int? Id_Grupo)
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
                            cont = dbe.Editar_Grupo_OrdenTrabajo(Id_ClienteGrupoOrden, item, Id_Cliente, FechaEnvio, FechaRecep, "NO", Id_Grupo);
                        }
                        else
                        {
                            cont = dbe.Editar_Grupo_OrdenTrabajo(Id_ClienteGrupoOrden, item, Id_Cliente, FechaEnvio, FechaRecep, "SI", Id_Grupo);
                        }
                    }
                }

                if (cont < 1)
                {
                    mensaje.Data = "Error al ingresar, favor verificar datos ingresados!";
                }
                else
                {
                    mensaje.Data = "Registros guardados correctamente";
                }
                return mensaje;

            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }
        public class Temp_EditarCheckTotal_Grupo
        {
            public int Id_OrdenTrabajo { get; set; }
        }
        public class Temp_EditarCheck_Grupo
        {
            public int Id_OrdenTrabajoToT { get; set; }
        }



        #endregion

        //ALERTA DE ORDENTRABAJO
        #region

        [HttpGet]
        public ActionResult Alerta_OrdenTrabajo(int id)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_OrdenTrabajo mT_OrdenTrabajo = db.MT_OrdenTrabajo.Find(id);
                    if (mT_OrdenTrabajo != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_OrdenTrabajo"] = mT_OrdenTrabajo.Id_OrdenTrabajo;
                        var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(80, empresa_id.ToString(), id).ToList();
                        var ordenTrabajo = mT_OrdenTrabajo.Codigo_Cliente;
                        ViewBag.listaNotificaciones = listaNotificaciones;
                        ViewBag.ordenTrabajo = ordenTrabajo;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
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
            else { return RedirectToAction("IniciarSesion", "Home"); }


        }

        [HttpGet]
        public ActionResult AgregarAlerta()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    List<SelectListItem> itemsPrioridad = new List<SelectListItem>();
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> items_Tipo = new List<SelectListItem>();
                    List<SelectListItem> itemsFrecuencia = new List<SelectListItem>();

                    var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(43).ToList();


                    foreach (var tipo in listaTipos)
                    {
                        if (tipo.Descripcion.Equals("Orden De Trabajo"))
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
                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = true });
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


                    var listaFrecuencia = db.sp_Quimipac_ConsultaMT_TablaDetalle(1066).ToList();
                    foreach (var frecuencia in listaFrecuencia)
                    {

                        itemsFrecuencia.Add(new SelectListItem { Value = Convert.ToString(frecuencia.Id_TablaDetalle), Text = frecuencia.Descripcion });

                    }


                    SelectList selectlistaFrecuencia = new SelectList(itemsFrecuencia, "Value", "Text");

                    ViewBag.listaTipo = selectlistaTipo;
                    ViewBag.listaPrioridad = selectlistaPrioridad;
                    ViewBag.listaEstado = selectlistaEstado;
                    ViewBag.listaTipos = selectlistaTipos;
                    ViewBag.listaFrecuencia = selectlistaFrecuencia;

                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarAlerta([Bind(Include = "Tipo_Notificacion,Id_Codigo_Origen,Id_usuario,Fecha,Prioridad,Asunto,Mensaje,Criterio,Id_Notificacion,Id_Persona,Tipo,Correo, Fecha_Hora,Estado,Frecuencia, Enviado, Fecha_Envio, Reenvio, EmpresaID")] InsertNotificacion mT_Notificacion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_OrdenTrabajo = System.Web.HttpContext.Current.Session["id_OrdenTrabajo"];
                    int idOrdenTrabajo = int.Parse(id_OrdenTrabajo.ToString());
                    var mensaje = new JsonResult();
                    var descripcion_tipo_not = db.MT_TablaDetalle.Where(x => x.Id_TablaDetalle == mT_Notificacion.Tipo_Notificacion).Select(x => x.Descripcion);
                    string descrip_tipo_not = descripcion_tipo_not.FirstOrDefault();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    if (ModelState.IsValid)
                    {//hay que cambiar cuando se den cuentan
                        
                        var Notificaciones = db.MT_Notificaciones.Where(x => x.Asunto == mT_Notificacion.Asunto && x.Mensaje == mT_Notificacion.Mensaje);
                        if (Notificaciones.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Notificacion ya existe";
                            return RedirectToAction("Alerta_OrdenTrabajo", new { id = idOrdenTrabajo });
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
                                var Id_Notificacion = System.Web.HttpContext.Current.Session["Id_Notificacion"];

                                var codigo_frecuencia = db.MT_TablaDetalle.Where(x => x.Id_TablaDetalle == mT_Notificacion.Frecuencia).Select(vq => vq.Codigo);
                                string codigo = codigo_frecuencia.ToString();
                                mT_Notificacion.EmpresaID = empresa_id.ToString();

                                if (codigo != "Ninguna")
                                {
                                    mT_Notificacion.Reenvio = true;
                                }

                                var Id_noti_Nuevo = dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado, idOrdenTrabajo, Convert.ToInt32(Id_Notificacion), mT_Notificacion.Frecuencia, mT_Notificacion.EmpresaID, mT_Notificacion.Reenvio);
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

                                string cadena = mT_Notificacion.Correo;
                                bool bandera = false;
                                if (cadena.Contains(';'))
                                {
                                    cadena = cadena.Replace(";", "; ");
                                }
                                try
                                {
                                    message.From = new MailAddress(cadena, VSMTP[3]);// correo a usuario enviar, correo del que envia 
                                    string[] Separado = cadena.Split(';');
                                    foreach (var item in Separado)
                                    {
                                        message.To.Add(new MailAddress(item));
                                    }
                                    //message.To.Add(cadena);
                                    message.Subject = mT_Notificacion.Asunto;
                                    //... Modificar el cuerpo(campos) que se inserto en la tabla del servidor
                                    string htmlParametro = VSMTP[8];
                                    htmlParametro = htmlParametro.Replace("{Usuario}", "");
                                    htmlParametro = htmlParametro.Replace("{Asunto}", mT_Notificacion.Asunto);
                                    htmlParametro = htmlParametro.Replace("{Correo_Emisor}", VSMTP[6].ToLower());

                                    htmlParametro = htmlParametro.Replace("{Cuerpo}", mT_Notificacion.Mensaje);
                                    htmlParametro = htmlParametro.Replace("{Anio}", DateTime.Today.Year.ToString());

                                    var nameEmpresa = System.Web.HttpContext.Current.Session["empresa_Nombre"];
                                    htmlParametro = htmlParametro.Replace("{Empresa}", nameEmpresa.ToString());

                                    message.Body = htmlParametro;

                                    //client.UseDefaultCredentials = true;
                                    if (Convert.ToInt32(VSMTP[11]) == 0) { message.IsBodyHtml = false; }
                                    else { message.IsBodyHtml = true; }

                                    if (Convert.ToInt32(VSMTP[5]) == 0) { client.EnableSsl = false; }
                                    else { client.EnableSsl = true; }
                                    //
                                    client.Credentials = new System.Net.NetworkCredential(VSMTP[6], VSMTP[7]);
                                    client.Send(message);
                                    bandera = true;
                                }
                                catch (Exception e)
                                {
                                    bandera = false;
                                }


                                //MT_TablaDetalle[] Vsmtp = SMTP.ToArray();
                                TempData["mensaje_correcto"] = "Notificacion guardada";
                                if (bandera == true)
                                {
                                    MT_Notificaciones update_mt_notificaciones = db.MT_Notificaciones.Find(Id_noti_Nuevo);
                                    update_mt_notificaciones.Enviado = true;
                                    update_mt_notificaciones.Fecha_Envio = DateTime.Now;
                                    db.Entry(update_mt_notificaciones).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    MT_Notificaciones update_mt_notificaciones = db.MT_Notificaciones.Find(Id_noti_Nuevo);
                                    update_mt_notificaciones.Enviado = false;
                                    db.Entry(update_mt_notificaciones).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                return RedirectToAction("Alerta_OrdenTrabajo", new { id = idOrdenTrabajo });
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Alerta_OrdenTrabajo", new { id = idOrdenTrabajo });
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
        public ActionResult EliminarAlerta(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            //db.MT_ContratoAlerta.Remove(mT_Alerta);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Archivo eliminado";
                            return RedirectToAction("Alerta_OrdenTrabajo", new { id = id_MTContrato });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Alerta_OrdenTrabajo", new { id = id_MTContrato });
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

        //2do post de orden trabajo

        [HttpPost]
        //Fecha_asignacion_grupo_trabajo, Fecha_asignacion, Fecha_registro,Origen,Id_tipo_trabajo_ejecutado,Id_campamento,Id_estacion,Id_sector,Id_sucursal,Id_contrato,Estado,cod_cli, Etapa, Id_OrdenTrabajo
        public ActionResult OrdenTrabajo_Filter(DateTime? Fecha_Inicio, DateTime? Fecha_registro, List<string> check_parametro, string SelectedAndOr, int? MenuSelectedFecha)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            try
            {

                var usuario = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var bde = new DataBase_Externo();
                int automatizacion = 1;
                var lkbusqueda = bde.execute_filter_OrdenTrabajo(Fecha_Inicio, Fecha_registro, check_parametro, SelectedAndOr, MenuSelectedFecha, automatizacion);

                //var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                //lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                //var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                var listakContrato = db.MT_Contrato.Where(vq => ContratosLI.Contains(vq.Id_Cliente)).ToList();
                //lkbusqueda = lkbusqueda.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();
                lkbusqueda = lkbusqueda.Where(vq => ContratosLI.Contains(vq.Id_Proyecto.ToString())).ToList();

                //DATA GET CONTRATO
                //ViewBag.listaContrato = lkbusqueda;
                //TempData["listaFiltro"] = lkbusqueda;


                //region 2
                //foreach (var item in lkbusqueda)
                //{
                //    //PROCESO \ ALERTA \ CAIDA
                //    item.Estado_P_A_C = IsValidParametro(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);
                //}

                var lista_Proceso = lkbusqueda.Where(vq => vq.Estado_P_A_C == "EN PROCESO").OrderByDescending(vq => vq.Fecha_registro).ToList();// lista_Proceso;
                var lista_Alerta = lkbusqueda.Where(vq => vq.Estado_P_A_C == "ALERTA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Alerta;
                var lista_Caida = lkbusqueda.Where(vq => vq.Estado_P_A_C == "CAIDA").OrderByDescending(vq => vq.Fecha_registro).ToList();//lista_Caida;

                //lkbusqueda = new List<ParametrosBusquedaOT>();
                //lkbusqueda = new List<ConsultaMT_OrdenTrabajo_AuxVM>();
                lkbusqueda = new List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result>();
                string orderby = System.Web.HttpContext.Current.Session["OrderBy_OT"] == null ? "DESC" : System.Web.HttpContext.Current.Session["OrderBy_OT"].ToString();


                orderby = orderby.ToUpper();
                if (orderby == "ASC")
                {
                    OrderBy_ListaParametro_Filtro_2(lista_Proceso, lkbusqueda);
                    OrderBy_ListaParametro_Filtro_2(lista_Alerta, lkbusqueda);
                    OrderBy_ListaParametro_Filtro_2(lista_Caida, lkbusqueda);
                }
                /*else if (orderby == "ALERTA")
                {
                    OrderBy_ListaParametro(lista_Alerta, listaOrdenTrabajo);
                    OrderBy_ListaParametro(lista_Caida, listaOrdenTrabajo);
                    OrderBy_ListaParametro(lista_Proceso, listaOrdenTrabajo);
                }*/
                else if (orderby == "DESC")
                {
                    OrderBy_ListaParametro_Filtro_2(lista_Caida, lkbusqueda);
                    OrderBy_ListaParametro_Filtro_2(lista_Alerta, lkbusqueda);
                    OrderBy_ListaParametro_Filtro_2(lista_Proceso, lkbusqueda);
                }

                /**/

                TempData["listaFiltro"] = lkbusqueda;//"Tipo de Trabajo guardado";
                //



                /*
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();

                ViewBag.listalkTipo = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                ViewBag.listalkClienteC = new SelectList(dbe.ParametroLkClienteC("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");
                ViewBag.listalkContratoPadre = new SelectList(dbe.ParametroLkContratoPadre("Contrato_Padre", empresa_id.ToString()), "Id_Contrato", "Nombre");
                ViewBag.listalkEstadoC = new SelectList(dbe.ParametroLkEstadoC("Estado", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                ViewBag.listalkLinea = new SelectList(dbe.ParametroLkLinea("Unidad_Negocio", empresa_id.ToString()), "codigo", "descripcion");
                ViewBag.listalkCategoria = new SelectList(dbe.ParametroLkCategoria("Categoria", empresa_id.ToString()), "cod_servicio", "nombre");
                ViewBag.listakSubcategoria = new SelectList(dbe.ParametroLkSubcategoria("Subcategoria", empresa_id.ToString()), "codcen", "nombre");
                ViewBag.listalkReferencia = new SelectList(dbe.ParametroLkReferencia("Referencia", empresa_id.ToString()), "Id_Contrato", "Nombre");
                ViewBag.listalkLocalidad = new SelectList(dbe.ParametroLkLocalidad("Localidad", empresa_id.ToString()), "codigo_loc", "descripcion");
                ViewBag.listakResponsable = new SelectList(dbe.ParametroLkResponsable("Responsable", empresa_id.ToString()), "id_persona", "Nombres_Completos");
                */
                //return View();
                /*............List<SelectListItem> lkContrato = new List<SelectListItem>();
                List<SelectListItem> lkOrdenPadre = new List<SelectListItem>();
                DataBase_Externo dbe = new DataBase_Externo();
                var listakOrdenPadre = dbe.ParametroLkOrdenPadre("OrdenPadre", empresa_id.ToString());

                foreach (var item in listakOrdenPadre)
                {
                    lkOrdenPadre.Add(new SelectListItem { Value = Convert.ToString(item.Id_OrdenTrabajo), Text = item.Codigo_Cliente });
                }
                SelectList selectlkOrdenPadre = new SelectList(lkOrdenPadre, "Value", "Text");

                var listakContrato2 = dbe.ParametroLkContrato("Contrato", empresa_id.ToString());
                //ojooooooooooooo Vanessa
                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                listakContrato2 = listakContrato2.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                listakContrato2 = listakContrato2.Where(vq => ContratosLI2.Contains(vq.Id_Contrato.ToString())).ToList();

                foreach (var item in listakContrato2)
                {
                    lkContrato.Add(new SelectListItem { Value = Convert.ToString(item.Id_Contrato), Text = item.Codigo_Cliente + "|" + item.Nombre });
                }
                SelectList selectlkContrato = new SelectList(lkContrato, "Value", "Text");



                ViewBag.listalkTipo_Trabajo = new SelectList(dbe.ParametroLkTipoTrabajo("Tipo_Trabajo", empresa_id.ToString()), "Id_TipoTrabajo", "Descripcion");
                ViewBag.listalkCampamento = new SelectList(dbe.ParametroLkCampamento("Campamento", empresa_id.ToString()), "Id_Campamento", "Nombre");
                ViewBag.listalkSucursal = new SelectList(dbe.ParametroLkSucursal("Sucursal", empresa_id.ToString()), "cod_suc", "nombre");
                ViewBag.listalkEstacion = new SelectList(dbe.ParametroLkEstacion("Estacion", empresa_id.ToString()), "Id_Estacion", "Nombre");
                ViewBag.listalkSector = new SelectList(dbe.ParametroLkSector("Sector", empresa_id.ToString()), "Id_Sector", "Nombre");
                ViewBag.listalkEstado = new SelectList(dbe.ParametroLkEstado("Estado", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                ViewBag.listakClientes = new SelectList(dbe.ParametroLkCliente("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");
                ..........*/
                /*aqui mandamos para el canvas*/
                TempData["lista_Proceso"] = lista_Proceso;
                TempData["lista_Alerta"] = lista_Alerta;
                TempData["lista_Caida"] = lista_Caida;
                //ViewBag.lista_Proceso = lista_Proceso;
                //ViewBag.lista_Alerta = lista_Alerta;
                //ViewBag.lista_Caida = lista_Caida;
                /**/
                ViewBag.listaEtapas = orderby;

                //return View();
                return RedirectToAction("OrdenTrabajo_Automatizacion");//, "OrdenTrabajo_Automatizacion");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


    }
}
