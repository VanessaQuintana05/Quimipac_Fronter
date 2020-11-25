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
        public ActionResult Default()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                var UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaOrdenTrabajo = db.sp_Quimipac_ConsultaMT_OrdenTrabajo(empresa_id.ToString(), 0).Where(vq => vq.Fecha_asignacion != null).ToList();

                var ProyectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                var ProspectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();



                listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ProyectosLI2.Contains(vq.Id_Proyecto.ToString()) || ProspectosLI2.Contains(vq.Id_Prospecto.ToString())).ToList();

                //******************
                //?
                ViewBag.N_OT = listaOrdenTrabajo.Count();
                ViewBag.Nlista_Proceso = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "EN PROCESO").ToList().Count();// lista_Proceso;
                ViewBag.Nlista_Alerta = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "ALERTA").ToList().Count();//lista_Alerta;
                ViewBag.Nlista_Caida = listaOrdenTrabajo.Where(vq => vq.Estado_P_A_C == "CAIDA").ToList().Count();//lista_Caida;



                var listaOrdenTrabajoDemo = db.Sp_Dashboard_EstadoOT(empresa_id.ToString()).ToList();
                // var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                //                --listaOrdenTrabajo = listaOrdenTrabajo.Where(vq => ContratosLI.Contains(vq.Id_contrato.ToString())).ToList();

                List<CustomChart> result = new List<CustomChart>();
                foreach (var item in listaOrdenTrabajoDemo)
                {
                    result.Add(new CustomChart { 
                        TexItem = item.Estado
                       ,ValueItem = Convert.ToInt32(item.total)
                    });

                }


                var chartDataPie = new object[result.Count + 1]; //var chartDataPie = new object[(3) + 1];
                chartDataPie[0] = new object[] { "Estado OT", "Total" };
                int j = 0;
                foreach (var i in result)
                {
                    j++;
                    chartDataPie[j] = new object[] { i.TexItem.ToString(), i.ValueItem };
                }

                ViewBag.chartDataPie = chartDataPie;//.ToList();
                return View();
                }else { return RedirectToAction("IniciarSesion", "Home"); }
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
                    var listaGrupo = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_GRUPOORDEN_ETAPA @p0", empresa_id).ToList();
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
                    var listaClte = ctx.Database.SqlQuery<CustomChartSelect>("SP_DASHBOARD_CLIENTE_ORDEN @p0", empresa_id).ToList();

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