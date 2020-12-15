using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;
using static Quimipac_.Models.CustomChartRender;
using System.Data.SqlClient;


namespace Quimipac_.Controllers
{
    public class MainController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        #region MAIN DASHBOARD
        [HttpGet]
        public ActionResult Default(int? Id_Proyecto = 0)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                var UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaOrdenTrabajoRedes = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 0).Where(vq => vq.Fecha_asignacion != null).ToList();
                var listaOrdenTrabajoAutom = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 1).Where(vq => vq.Fecha_asignacion != null).ToList();

                if (Id_Proyecto != 0)
                {
                    listaOrdenTrabajoRedes = listaOrdenTrabajoRedes.Where(vq=>vq.Id_Proyecto == Id_Proyecto).ToList();
                    listaOrdenTrabajoAutom = listaOrdenTrabajoAutom.Where(vq => vq.Id_Proyecto == Id_Proyecto).ToList();
                }

                var ProyectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                var ProspectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();

                listaOrdenTrabajoRedes = listaOrdenTrabajoRedes.Where(vq => ProyectosLI2.Contains(vq.Id_Proyecto.ToString()) || ProspectosLI2.Contains(vq.Id_Prospecto.ToString())).ToList();
                //******************

                ViewBag.N_OTRedes = listaOrdenTrabajoRedes.Count();// + listaOrdenTrabajoAutom.Count();
                ViewBag.N_OT_AUTOM = listaOrdenTrabajoAutom.Count();

                ViewBag.Nlista_ProcesoRedes = listaOrdenTrabajoRedes.Where(vq => vq.Estado_P_A_C == "EN PROCESO").ToList().Count();// lista_Proceso;
                ViewBag.Nlista_AlertaRedes = listaOrdenTrabajoRedes.Where(vq => vq.Estado_P_A_C == "ALERTA").ToList().Count();//lista_Alerta;
                ViewBag.Nlista_CaidaRedes = listaOrdenTrabajoRedes.Where(vq => vq.Estado_P_A_C == "CAIDA").ToList().Count();//lista_Caida;

                ViewBag.Nlista_Proceso_Autom = listaOrdenTrabajoAutom.Where(vq => vq.Estado_P_A_C == "EN PROCESO").ToList().Count();// lista_Proceso;
                ViewBag.Nlista_Alerta_Autom = listaOrdenTrabajoAutom.Where(vq => vq.Estado_P_A_C == "ALERTA").ToList().Count();//lista_Alerta;
                ViewBag.Nlista_Caida_Autom = listaOrdenTrabajoAutom.Where(vq => vq.Estado_P_A_C == "CAIDA").ToList().Count();//lista_Caida;

                var listaOrdenTrabajoDemo = db.Sp_Dashboard_EstadoOT(empresa_id.ToString()).ToList();
                // var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                //                --listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();

                //COMBO PROYECTOS
                var listaAuth = db.MT_Proyecto.Where(vq => vq.Id_Empresa == empresa_id.ToString() && vq.Estado == "A").ToList();
                var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                listaAuth = listaAuth.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();

                var datacmbProy = listaAuth.Select(vq => new MT_Proyecto
                {
                    Id_Proyecto = vq.Id_Proyecto,
                    Codigo_Cliente = vq.Codigo_Cliente + " | " + vq.Nombre
                }).ToList();
                var oNinguno = new MT_Proyecto { Id_Proyecto = 0, Codigo_Cliente = " - Todos -" };
                datacmbProy.Add(oNinguno); // listaAuth.Add(oNinguno);

                //listaAuth
                ViewBag.Id_Proyecto = new SelectList(datacmbProy.OrderBy(vq => vq.Id_Proyecto).ToList(), "Id_Proyecto", "Codigo_Cliente", Id_Proyecto);

                var fechaDesde = DateTime.Today.AddYears(-3);//.AddMonths(-3);
                //xFechadd
                ViewBag.chartDataPie = getxFecha(listaOrdenTrabajoRedes, fechaDesde, false, Id_Proyecto);// chartDataPie;
                ViewBag.chart_Automatizacion = getxFecha(listaOrdenTrabajoAutom, fechaDesde, true, Id_Proyecto);// chart_Automatizacion.Count() >1 ? chart_Automatizacion:null;

                //REDES
                ViewBag.chart_grupo_redes = setxGrupo(false, Id_Proyecto);
                ViewBag.chart_cliente_redes = setxCliente(false, Id_Proyecto);

                //AUTOMATIZACION
                ViewBag.chart_grupo_automatizacion = setxGrupo(true, Id_Proyecto);
                ViewBag.chart_cliente_automatizacion = setxCliente(true, Id_Proyecto);
                return View();
            }else { 
                return RedirectToAction("IniciarSesion", "Home"); 
            }
        }

        
        private object getxFecha(List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> Lista, DateTime fechaDesde,bool isAutomatizacion,int? Id_Proyecto)
        {
            try
            {
                string titleCabecera = isAutomatizacion? "Orden de Automatización":"Orden de Trabajo Redes" ;
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                if (Id_Proyecto != 0)
                {
                    Lista = Lista.Where(vq => vq.Id_Proyecto == Id_Proyecto).ToList();
                }

                //
                /*var query = Lista.OrderBy(vq => vq.Fecha_asignacion)
                   .GroupBy(p => p.Fecha_asignacion)//.Id_GrupoTrabajo)
                   .Select(g => new {
                       FAsignacion = g.Key,
                       Total = g.Where(v => v.Fecha_asignacion == v.Fecha_asignacion).Count()
                   }).ToList();*/
                
                /*
                 var query = (from p in Lista.OrderBy(vq => vq.Fecha_asignacion)
                            group p by p.Fecha_asignacion into g
                            select new
                            {
                                FAsignacion = g.Key,
                                Total = g.Count()
                            }).ToList();*/

                /*
                 var query = Lista.OrderBy(vq => vq.Fecha_asignacion)
                 .GroupBy(p => p.Fecha_asignacion)//.Id_GrupoTrabajo)
                 .Select(g => new {
                       FAsignacion = g.Key,
                       Total = g.Where(vq => vq.Fecha_asignacion>=fechaDesde.ToUpper() == "CAIDA").Count()
                   }).ToList();
                */
                //
                var N_OTRedes_TotFecha = Lista.Where(vq => vq.Fecha_asignacion >= fechaDesde).OrderBy(vq => vq.Fecha_asignacion).Select(
                vq => new sp_Quimipac_ConsultaMT_OrdenTrabajo_Result
                {
                    Fecha_asignacion = Convert.ToDateTime(vq.Fecha_asignacion?.ToShortDateString()),// Convert.ToDateTime(vq.Fecha_asignacion.ToString("dd/MM/yyyy")),
                    Estado = (Lista.Where(v => v.Fecha_asignacion?.ToString("dd/MM/yyyy") == vq.Fecha_asignacion?.ToString("dd/MM/yyyy")).ToList().Count())
                });
                var query = (from p in N_OTRedes_TotFecha// Lista
                             group p by p.Fecha_asignacion into g
                             select new
                             {
                                 FAsignacion = g.Key,
                                 Total = g.Count()
                             }).ToList();
                /*List<CustomChart> result = new List<CustomChart>();
                foreach (var item in N_OTRedes_TotFecha)// listaOrdenTrabajoDemo)
                {
                    result.Add(new CustomChart
                    {
                        TexItem = item.Fecha_asignacion?.ToString("dd/MM/yyyy"),//Estado,
                        ValueItem = item.Estado ?? 0
                    });
                }
                */


                var chartDataPie = new object[query.Count + 1]; 
                chartDataPie[0] = new object[] { titleCabecera, "Total" };
                int j = 0;
                foreach (var i in query)//result)
                {
                    j++;
                    //chartDataPie[j] = new object[] { i.TexItem.ToString(), i.ValueItem };
                    chartDataPie[j] = new object[] { i.FAsignacion?.ToString("dd/MM/yyyy"), i.Total };
                }
                return chartDataPie;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return null;// Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetChartxGrupo()
        private object setxGrupo(Boolean isAutomatizacion, int? Id_Proyecto)
        {
            try
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                using (var ctx = new BD_QUIMIPACEntities())
                {
                    var listaGrupo = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_GRUPOORDEN_ETAPA @p0,@p1", empresa_id, isAutomatizacion).ToList();

                    if (Id_Proyecto != 0)
                    {
                        listaGrupo = listaGrupo.Where(vq => vq.Id_Proyecto == Id_Proyecto).ToList();
                    }
                    //listaGrupo = listaGrupo.Where(vq=>vq.AUTOMATIZACION == TipoAutomatizacion).ToList();

                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    var listaProspectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();

                    //listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();
                    listaGrupo = listaGrupo.Where(vq => listaProyectos.Contains(vq.Id_Proyecto.ToString()) || listaProspectos.Contains(vq.Id_Prospecto.ToString())).ToList();

                   

                    
                    /*var query = from p in listaGrupo
                    group p by p.name into g
                    select new
                    {
                      name = g.Key,
                      count = g.Count()
                    };*/
                     
                    var query = listaGrupo
                   .GroupBy(p => p.GRUPOTRABAJO)//.Id_GrupoTrabajo)
                   .Select(g => new { 
                       GRUPOTRABAJO = g.Key,
                       nProceso = g.Where(vq=>vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").Count(),
                       nAlerta = g.Where(vq => vq.ESTADO_P_A_C.ToUpper() == "ALERTA").Count(),
                       nCaida = g.Where(vq => vq.ESTADO_P_A_C.ToUpper() == "CAIDA").Count()
                   }).ToList();
                    var chart = new object[query.Count + 1]; //var chartDataPie = new object[(3) + 1];
                    chart[0] = new object[] { "Grupo", "Proceso", "Alerta", "Caida" };

                    int j = 0;
                    foreach (var i in query)//listaGrupo)
                    {
                        j++;
                        chart[j] = new object[] {
                            i.GRUPOTRABAJO.ToString(),
                            i.nProceso,
                            i.nAlerta,
                            i.nCaida

                            /*i.GRUPOTRABAJO.ToString(),
                            listaGrupo.Where(vq=>vq.Id_GrupoTrabajo == i.Id_GrupoTrabajo && vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").ToList().Count(),
                            listaGrupo.Where(vq=>vq.Id_GrupoTrabajo == i.Id_GrupoTrabajo && vq.ESTADO_P_A_C.ToUpper() == "ALERTA").ToList().Count(),
                            listaGrupo.Where(vq=>vq.Id_GrupoTrabajo == i.Id_GrupoTrabajo && vq.ESTADO_P_A_C.ToUpper() == "CAIDA").ToList().Count()*/
                        };
                    }
                    return chart;// Json(chart, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return null;// Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        private object setxCliente(Boolean isAutomatizacion, int? Id_Proyecto)//public JsonResult GetChartxClte()
        {
            try
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                using (var ctx = new BD_QUIMIPACEntities())
                {
                    var listaClte = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_CLIENTE_ORDEN @p0, @p1", empresa_id, isAutomatizacion).ToList();
                    //listaClte = listaClte.Where().T;
                    if (Id_Proyecto != 0)
                    {
                        listaClte = listaClte.Where(vq => vq.Id_Proyecto == Id_Proyecto).ToList();
                    }

                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    var listaProspectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();

                    listaClte = listaClte.Where(vq => listaProyectos.Contains(vq.Id_Proyecto.ToString()) || listaProspectos.Contains(vq.Id_Prospecto.ToString())).ToList();



                    var query = listaClte
                   .GroupBy(p => p.NOMBRECLIENTE)//.Id_GrupoTrabajo)
                   .Select(g => new {
                       NOMBRECLIENTE = g.Key,
                       nProceso = g.Where(vq => vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").Count(),
                       nAlerta = g.Where(vq => vq.ESTADO_P_A_C.ToUpper() == "ALERTA").Count(),
                       nCaida = g.Where(vq => vq.ESTADO_P_A_C.ToUpper() == "CAIDA").Count()
                   }).ToList();

                    var chart = new object[query.Count + 1];
                    chart[0] = new object[] { "Clientes", "Proceso", "Alerta", "Caida" };

                    int j = 0;
                    foreach (var i in query)//listaClte)
                    {
                        j++;
                        chart[j] = new object[] {
                            i.NOMBRECLIENTE.ToString(),
                            i.nProceso,
                            i.nAlerta,
                            i.nCaida
                            /*listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").ToList().Count(),
                            listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "ALERTA").ToList().Count(),
                            listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "CAIDA").ToList().Count()
                            */
                        };
                    }
                    return chart;// Json(chart, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return null;// Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion




        #region LEER NOTIFICACIONES
        [HttpGet]
        public ActionResult LeerNotificacion()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
			    {
                    var user = System.Web.HttpContext.Current.Session["usuario"] as string;
                    //var listaNotificacion = db.sp_Quimipac_Consulta_Notificaciones_Entrada("General",0, user).ToList();
                    var listaNotificacion = db.sp_Quimipac_Consulta_Notificaciones_Entrada("Entrada_Noti", 0, user).ToList();
				    ViewBag.listaNotificacion = listaNotificacion;

				    return View();
			    }
			    catch (Exception e)
			    {
				    return RedirectToAction("Error", "Errores");
			    }
            }else {   return RedirectToAction("IniciarSesion", "Home");
        }
}
        #endregion

        public JsonResult GetChartxGrupo()
        {
            try
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                using (var ctx = new BD_QUIMIPACEntities()) 
                {
                    var listaGrupo = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_GRUPOORDEN_ETAPA @p0,@p1", empresa_id,false).ToList();
                    //var listaGrupo = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_GRUPOORDEN_ETAPA @empresa",
                    //new SqlParameter(("empresa", empresa_id)).ToList();

                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    var listaProspectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();

                    //listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();
                    listaGrupo = listaGrupo.Where(vq => listaProyectos.Contains(vq.Id_Proyecto.ToString()) || listaProspectos.Contains(vq.Id_Prospecto.ToString())).ToList();

                    var chart = new object[listaGrupo.Count + 1]; //var chartDataPie = new object[(3) + 1];
                    chart[0] = new object[] { "Grupo", "Proceso", "Alerta","Caida" };

                    int j = 0;
                    foreach (var i in listaGrupo)
                    {
                        j++;
                        chart[j] = new object[] { 
                            i.GRUPOTRABAJO.ToString(),
                            listaGrupo.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").ToList().Count(),
                            listaGrupo.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "ALERTA").ToList().Count(),
                            listaGrupo.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "CAIDA").ToList().Count()
                        };
                    }
                    return Json(chart, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetChartxClte()
        {
            try
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                using (var ctx = new BD_QUIMIPACEntities())
                {
                    var listaClte = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_CLIENTE_ORDEN @p0,@p1", empresa_id,false).ToList();

                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    var listaProspectos = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();

                    listaClte = listaClte.Where(vq => listaProyectos.Contains(vq.Id_Proyecto.ToString()) || listaProspectos.Contains(vq.Id_Prospecto.ToString())).ToList();

                    var chart = new object[listaClte.Count + 1]; //var chartDataPie = new object[(3) + 1];
                    chart[0] = new object[] { "Clientes", "Proceso", "Alerta", "Caida" };

                    int j = 0;
                    foreach (var i in listaClte)
                    {
                        j++;
                        chart[j] = new object[] {
                            i.NOMBRECLIENTE.ToString(),
                            listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "EN PROCESO").ToList().Count(),
                            listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "ALERTA").ToList().Count(),
                            listaClte.Where(vq=>vq.ID_ORDENTRABAJO_INTEGRANTE == i.ID_ORDENTRABAJO_INTEGRANTE && vq.ESTADO_P_A_C.ToUpper() == "CAIDA").ToList().Count()
                        };
                    }
                    return Json(chart, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //***********
        public JsonResult GetChartPieData_Estado()
        {
            string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
            string empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
            try
            {
                var listaOrdenTrabajo = db.Sp_Dashboard_EstadoOT(empresa_id.ToString()).ToList();
               // var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
//                --listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();

                List<CustomChart> result = new List<CustomChart>();
                foreach (var item in listaOrdenTrabajo)
                {
                result.Add(new CustomChart { TexItem = item.Estado, ValueItem = Convert.ToInt32(item.total)});

                }


                var chartDataPie = new object[result.Count + 1]; //var chartDataPie = new object[(3) + 1];
                chartDataPie[0] = new object[] { "Estado OT", "Total" };
                int j = 0;
                foreach (var i in result)
                {
                    j++;
                    chartDataPie[j] = new object[] { i.TexItem.ToString(), i.ValueItem };
                }



                return Json(chartDataPie, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OT Get" + e.Message);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


    }

    //public class CustomChart
    //{
    //    public string TexItem { set; get; }
    //    public int ValueItem { set; get; }
    //}
    //public class CustomChartxEstado
    //{
    //    public string TexItem { set; get; }
    //    public int nProceso { set; get; }
    //    public int nAlerta { set; get; }
    //    public int nCaida { set; get; }
    //}
}