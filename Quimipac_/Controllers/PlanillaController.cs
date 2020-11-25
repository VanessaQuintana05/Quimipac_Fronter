using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web;
using System.IO;
using ExcelDataReader;
using Quimipac_.Repositorio;

namespace Quimipac_.Controllers
{
    public class PlanillaController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
                
        //MATENIMIENTO DE PRECIOS REFERENCIALES
        #region
        [HttpGet]
        public ActionResult Planilla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
            {
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var listaPlanillaProyectos = db.sp_Quimipac_ConsultaMT_Planilla(1,empresa_id.ToString()).ToList();                
                var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                listaPlanillaProyectos = listaPlanillaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_PoyectoContr.ToString())).ToList();
                var listaPlanillaContratos = db.sp_Quimipac_ConsultaMT_Planilla(2, empresa_id.ToString()).ToList();
                var ProyectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                listaPlanillaContratos = listaPlanillaContratos.Where(vq => ProyectosLI2.Contains(vq.Id_PoyectoContr.ToString())).ToList();

                foreach (var contrato in listaPlanillaContratos)
                {
                    listaPlanillaProyectos.Add(contrato);
                }

                    var listaContrato = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                    //aquiiiii agregue
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaContrato = listaContrato.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    listaContrato = listaContrato.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                    itemsContratos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var contrato in listaContrato)
                    {
                        itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Nombre });
                    }
                    SelectList selectlistaContrato = new SelectList(itemsContratos, "Value", "Text");

                    ViewBag.listaPlanilla = listaPlanillaProyectos;
                    ViewBag.listalkContrato = selectlistaContrato;
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
        public ActionResult Agregar_Planilla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
            {
                List<SelectListItem> itemsTipos = new List<SelectListItem>();
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                List<SelectListItem> itemsEstado = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

                foreach (var tipo in listaTipos)
                {
                    itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                }

                // itemsTipos.Insert(0, new SelectListItem() { Value = "", Text = "Seleccione Curso" });

                SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

                //var listaContratos = db.sp_Quimipac_ConsultaMT_ContratoOrdenPlanilla(empresa_id.ToString()).ToList();
                var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaContratos = listaContratos.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    listaContratos = listaContratos.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                foreach (var contrato in listaContratos)
                {
                    itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente });
                }

                SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                listaProyectos = listaProyectos.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                listaProyectos = listaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                foreach (var proyecto in listaProyectos)
                {
                    itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
                }

                SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");
                                
                var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

                foreach (var moneda in listaMoneda)
                {
                    itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre });
                }

                SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                
                var listaEstado = db.sp_Quimipac_ConsultaMT_TablaDetalle(8).ToList();

                foreach (var estado in listaEstado)
                {
                    if (estado.Descripcion != "Aprobada por cliente")
                    {
                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion });
                    }
                }

                SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");


                ViewBag.listaContratos = selectlistaContratos;
                ViewBag.listaProyectos = selectlistaProyectos;
                ViewBag.listaMoneda = selectlistaMoneda;
                ViewBag.listaTipos = selectlistaTipos;
                ViewBag.listaEstado = selectlistaEstado;

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
        public ActionResult Agregar_Planilla(HttpPostedFileBase excelfile, int Tipo, int? ContratoID, int? ProyectoID)//[Bind(Include = "Id_PoyectoContr, Fecha_Planilla,Fecha_Inicio, Fecha_Fin,Usuario,Estado, moneda, Tipo")] MT_Planilla mT_Planilla)
        {
            
            try
            {
                if (excelfile == null || excelfile.ContentLength == 0)
                {
                    TempData["mensaje_error"] = "Por favor seleccione un archivo";
                    return RedirectToAction("Planilla");
                }
                else
                {
                    if (Tipo != 0)
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
                                        return RedirectToAction("Planilla");
                                    }
                                }

                            }

                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            excelfile.SaveAs(path);
                            //read data from excel file

                            var records = new List<OrdenTrabajo_Excel>();
                            int iContador = 0;
                            int iContadorExistente = 0;
                            int iContadorNoIngresado = 0;
                            int iContadorIngresado = 0;
                            DateTime? fecha_asignacion_;
                            DateTime? fecha_leg_;
                            DateTime? inicio_ej_;
                            DateTime? fin_ej_;

                            string variableh;
                            DateTime? fecha_creacion_;                            
                            DateTime? fecha_estimada_eje_;
                            DateTime? fecha_max_leg_;
                            DateTime? ultimareprog_;
                            TimeSpan? hora_acordada_;
                            string variable;
                            string variables;
                            string variabletp;
                            string variablenp;
                            string variableb;
                            string variablecl;
                            string mensaje = string.Empty;

                            using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Excel_OT/"), excelfile.FileName), FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    while (reader.Read())
                                    {
                                        if (iContador != 0)
                                        {

                                            records.Add(new OrdenTrabajo_Excel
                                            {

                                                Identificador = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString(),
                                                Tipo_de_Trabajo = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                                                Unidad_de_Trabajo = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                                                fecha_asignacion = reader.GetValue(3) == null ? null : reader.GetValue(3).ToString(),                                                
                                                fecha_leg = reader.GetValue(4) == null ? null : reader.GetValue(4).ToString(),
                                                inicio_ej = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString(),
                                                fin_ej = reader.GetValue(6) == null ? null : reader.GetValue(6).ToString(),
                                                Ubicacion_Geografica = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString()
                                                
                                                
                                            });

                                            string Identificador = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString();
                                            string Unidad_de_Trabajo = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                            
                                            string Fecha_de_Asignacion = reader.GetValue(3) == null ? null : reader.GetValue(3).ToString();
                                            if (Fecha_de_Asignacion != null)
                                            {
                                                fecha_asignacion_ = Convert.ToDateTime(Fecha_de_Asignacion);
                                            }
                                            else
                                            {

                                                fecha_asignacion_ = null;
                                            }
                                                                                        
                                            string Fecha_de_Legalizacion = reader.GetValue(4) == null ? null : reader.GetValue(4).ToString();
                                            if (Fecha_de_Legalizacion != null)
                                            {
                                                fecha_leg_ = Convert.ToDateTime(Fecha_de_Legalizacion);
                                            }
                                            else { fecha_leg_ = null; }
                                            
                                            string Inicio_de_Ejecucion = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString();
                                            if (Inicio_de_Ejecucion != null)
                                            {
                                                inicio_ej_ = Convert.ToDateTime(Inicio_de_Ejecucion);
                                            }
                                            else { inicio_ej_ = null; }
                                            
                                            string Fin_de_Ejecucion = reader.GetValue(6) == null ? null : reader.GetValue(6).ToString();
                                            if (Fin_de_Ejecucion != null)
                                            {
                                                fin_ej_ = Convert.ToDateTime(Fin_de_Ejecucion);
                                            }
                                            else { fin_ej_ = null; }
                                            
                                            string Ubicacion_Geográfica = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString();
                                            

                                            string tipo_trabajo = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString();
                                            if (tipo_trabajo != "")
                                            {
                                                int npos = tipo_trabajo.IndexOf('-');
                                                variable = tipo_trabajo.Substring(npos + 1);
                                                variable = variable.TrimStart();
                                            }
                                            else
                                            {
                                                variable = "";
                                            }

                                            
                                            var user_id = System.Web.HttpContext.Current.Session["usuario"];
                                            string usuario = user_id.ToString();

                                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                            DataBase_Externo database4 = new DataBase_Externo();
                                            var OTRepetido = db.MT_Planilla.Where(x => x.Identificador == Identificador).ToList();

                                            if (OTRepetido.Count() <= 0)
                                            {
                                                if (variable != "")
                                                {
                                                    int? contrato_proyecto;
                                                    var client_contr = "";
                                                    if (Tipo == 1)
                                                    {
                                                        contrato_proyecto = ProyectoID;
                                                        client_contr = db.MT_Proyecto.Where(vq => vq.Id_Proyecto == contrato_proyecto).FirstOrDefault().Id_Cliente;

                                                    }
                                                    else
                                                    {
                                                        contrato_proyecto = ContratoID;
                                                        client_contr = db.MT_Contrato.Where(vq => vq.Id_Contrato == contrato_proyecto).FirstOrDefault().Id_Cliente;

                                                    }
                                                    //set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contrato_excel)                                                    
                                                    //select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato
                                                    var trabjservplan = from tipotrab in db.MT_TipoTrabajo.Where(vq => vq.Descripcion == variable && vq.Estado == "A" && vq.Id_Cliente == client_contr)
                                                                        join tiposect in db.MT_Servicio.Where(vq => vq.Id_Empresa == empresa_id.ToString())
                                                                        on tipotrab.Id_Servicio equals tiposect.Id_Servicio
                                                                        select new { tipotrab.Id_TipoTrabajo };

                                                   


                                                    //if (trabjservplan != null && trabjservejec != null)
                                                    //{
                                                    var trabajoservicioplanlist = trabjservplan.ToList();   

                                                    if (trabajoservicioplanlist.Count > 0)
                                                    {
                                                        if (trabajoservicioplanlist.ElementAt(0).Id_TipoTrabajo != null)
                                                        {
                                                            var ac = database4.IngresarPlanilla_Cab(Identificador, Unidad_de_Trabajo, fecha_asignacion_, fecha_leg_, inicio_ej_, fin_ej_,  Ubicacion_Geográfica,  variable,  usuario, contrato_proyecto, empresa_id.ToString(), Tipo);
                                                            if (ac != -1)
                                                            {
                                                                iContadorIngresado = iContadorIngresado + 1;
                                                            }
                                                            else
                                                            {
                                                                iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                mensaje = mensaje + "La orden " + Identificador + "El tipo trabajo planeado y/o tipo trabajo ejecutado no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                            }
                                                        }
                                                        else

                                                        {
                                                            iContadorNoIngresado = iContadorNoIngresado + 1;
                                                            mensaje = mensaje + "En La orden " + Identificador + " Los campos tipo trabajo planeado y/o ejecutado estan vacios en el excel. Favor verificar. \r\n";

                                                        }
                                                    }
                                                    else
                                                    {
                                                        iContadorNoIngresado = iContadorNoIngresado + 1;
                                                        mensaje = mensaje + "En La orden " + Identificador + " El tipo trabajo planeado y/o tipo trabajo ejecutado no se encuentran registrados. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                    }

                                                }
                                                else
                                                {
                                                    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                    mensaje = mensaje + "En La orden " + Identificador + " El campo tipo trabajo planeado y/o tipo trabajo ejecutado las columnas estan vacias. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                }
                                            }
                                            else
                                            {
                                                iContadorExistente = iContadorExistente + 1;
                                            }

                                        }

                                        iContador++;
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
                            return RedirectToAction("Planilla");
                            //return View("OrdenTrabajo");
                        }
                        else
                        {
                            TempData["mensaje_error"] = "Tipo de archivo es incorrecto<br>";
                            return RedirectToAction("Planilla");
                        }
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Debe seleccionar un contrato o un proyecto";
                        return RedirectToAction("Planilla");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["mensaje_error"] = "Tipo de archivo es incorrecto, verificar la estructura del archivo";
                //return RedirectToAction("Error", "Errores");
                Console.Write(e.Message); return RedirectToAction("Planilla");
            }

        }

        [HttpGet]
        public ActionResult Editar_Planilla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                try
            {

                MT_Planilla mT_Planilla = db.MT_Planilla.Find(id);
                //System.Web.HttpContext.Current.Session["Fecha_Planilla"] = mT_Planilla.Fecha_Planilla;
                bool seleccion = false;
                if (mT_Planilla != null)
                {

                    List<SelectListItem> itemsTipos = new List<SelectListItem>();
                    List<SelectListItem> itemsContratos = new List<SelectListItem>();
                    List<SelectListItem> itemsProyectos = new List<SelectListItem>();
                    List<SelectListItem> itemsMonedas = new List<SelectListItem>();
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaTipos = db.sp_Quimipac_ConsultaMT_TablaDetalle(1).ToList();

                    foreach (var tipo in listaTipos)
                    {
                        //if (tipo.Id_TablaDetalle == mT_Planilla.Tipo)
                        {
                            seleccion = true;
                        }

                        itemsTipos.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                    }



                    SelectList selectlistaTipos = new SelectList(itemsTipos, "Value", "Text");

                    var listaContratos = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratos = listaContratos.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratos = listaContratos.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                    foreach (var contrato in listaContratos)
                    {
                        if (contrato.Id_Contrato == mT_Planilla.Id_PoyectoContr)
                        {
                            seleccion = true;
                        }

                        itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Codigo_Cliente, Selected = seleccion });
                    }

                    SelectList selectlistaContratos = new SelectList(itemsContratos, "Value", "Text");


                    var listaProyectos = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaProyectos = listaProyectos.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                    listaProyectos = listaProyectos.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                    foreach (var proyecto in listaProyectos)
                    {
                        if (proyecto.Id_Proyecto == mT_Planilla.Id_PoyectoContr)
                        {
                            seleccion = true;
                        }
                        itemsProyectos.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente, Selected = seleccion });
                    }

                    SelectList selectlistaProyectos = new SelectList(itemsProyectos, "Value", "Text");

                    
                    var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

                    foreach (var moneda in listaMoneda)
                    {
                        //if (moneda.codmon == mT_Planilla.moneda)
                        {
                            seleccion = true;
                        }

                        itemsMonedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                    }

                    SelectList selectlistaMoneda = new SelectList(itemsMonedas, "Value", "Text");

                    var listaEstado = db.sp_Quimipac_ConsultaMT_TablaDetalle(8).ToList();

                    foreach (var estado in listaEstado)
                    {
                        if (estado.Id_TablaDetalle == mT_Planilla.Estado)
                        {
                            seleccion = true;
                        }

                        if (estado.Descripcion != "Aprobada por cliente")
                        {

                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                        }
                    }

                     SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");


                    ViewBag.listaContratos = selectlistaContratos;
                    ViewBag.listaProyectos = selectlistaProyectos;
                    ViewBag.listaMoneda = selectlistaMoneda;
                    ViewBag.listaTipos = selectlistaTipos;
                    ViewBag.listaEstado = selectlistaEstado;


                    return View(mT_Planilla);

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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Planilla([Bind(Include = "Id_Planilla,Id_PoyectoContr, Fecha_Planilla,Fecha_Inicio, Fecha_Fin,Usuario,Estado, moneda, Tipo")] MT_Planilla mT_Planilla)
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

                        mT_Planilla.Usuario = user_id.ToString();
                        //mT_Planilla.Fecha_Planilla = Convert.ToDateTime(System.Web.HttpContext.Current.Session["Fecha_Planilla"]);


                       db.Entry(mT_Planilla).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Planilla actualizado";

                        return RedirectToAction("Planilla");
                        
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Planilla");
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

        //esto no se utiliza porq no tengo estado
        [HttpGet]
        public ActionResult EliminarPlanilla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {

                MT_Planilla mT_Planilla = db.MT_Planilla.Find(id);

                mT_Planilla.Estado = 1; //aqui esta mal porq este estado es enero de tabladetalle

                db.Entry(mT_Planilla).State = EntityState.Modified;
                db.SaveChanges();

                TempData["mensaje_actualizado"] = "Planilla Eliminado";
                return RedirectToAction("Planilla");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
            }
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Aprobar_Planilla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_Planilla mT_Planilla = db.MT_Planilla.Find(id);


                if (mT_Planilla.Estado != 26)
                {
                    mT_Planilla.Estado = 26;


                    db.Entry(mT_Planilla).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    TempData["mensaje_error"] = "Planilla ya esta aprobada";
                    return RedirectToAction("Planilla", new { id = mT_Planilla.Id_Planilla });
                }
                
                TempData["mensaje_actualizado"] = "Estado Planilla Aprobada";
                return RedirectToAction("Planilla");
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

        #region
        [HttpGet]
        public ActionResult Detalle_Planilla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_Planilla mT_Planilla = db.MT_Planilla.Find(id);

                if (mT_Planilla != null)
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    System.Web.HttpContext.Current.Session["id_Planilla"] = mT_Planilla.Id_Planilla;
                    var listaPlanillaDetalle = db.sp_Quimipac_ConsultaMT_DetallePlanilla(id,empresa_id.ToString()).ToList();
                    //var planilla = mT_Planilla.Id_Planilla;
                    ViewBag.planilla = System.Web.HttpContext.Current.Session["id_Planilla"].ToString();
                    ViewBag.listaPlanillaDetalle = listaPlanillaDetalle;
                    return View();
                }
                else
                {
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Planilla");
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

        [HttpGet]
        public ActionResult Agregar_Detalle_Planilla()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                //List<SelectListItem> items = new List<SelectListItem>();
                //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                //var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                //foreach (var item in listaItems)
                //{
                //    items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion });
                //}
                
                //SelectList selectlistaItem = new SelectList(items, "Value", "Text");

                //ViewBag.listaItems = selectlistaItem;

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
        public ActionResult Agregar_Detalle_Planilla(HttpPostedFileBase excelfile)//[Bind(Include = "Id_Planilla,Id_Item,Cantidad, Precio,Valor,IVA")] MT_Planilla_Detalle mT_Detalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var id_Planilla = System.Web.HttpContext.Current.Session["id_Planilla"];

                if (id_Planilla == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPlanilla = int.Parse(id_Planilla.ToString());

                        //if (ModelState.IsValid)
                        //{



                        //    //var planillade = db.MT_Planilla_Detalle.Where(x => x.Id_Item == mT_Detalle.Id_Item && x.Cantidad == mT_Detalle.Cantidad && x.Precio == mT_Detalle.Precio && x.Valor == mT_Detalle.Valor && x.IVA == mT_Detalle.IVA);
                        //    //if (planillade.Count() >= 1)
                        //    //{
                        //        TempData["mensaje_error"] = "Código ya existe";
                        //        return RedirectToAction("Detalle_Planilla", new { id = idPlanilla });
                        //    //}
                        //    //else
                        //    //{

                        //        mT_Detalle.Id_Planilla = idPlanilla;

                        //        db.MT_Planilla_Detalle.Add(mT_Detalle);
                        //        db.SaveChanges();
                        //        TempData["mensaje_correcto"] = "Detalle Planilla guardada";
                        //        return RedirectToAction("Detalle_Planilla", new { id = idPlanilla });


                        //    //}


                        //}
                        //else
                        //{
                        //    TempData["mensaje_error"] = "Valores incorrectos";
                        //    return RedirectToAction("Detalle_Planilla", new { id = idPlanilla });
                        //}

                        if (excelfile == null || excelfile.ContentLength == 0)
                        {
                            TempData["mensaje_error"] = "Por favor seleccione un archivo";
                            return RedirectToAction("Detalle_Planilla");
                        }
                        else
                        {
                            //if (Tipo != 0)
                            //{
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
                                                return RedirectToAction("Detalle_Planilla");
                                            }
                                        }

                                    }

                                    if (System.IO.File.Exists(path))
                                        System.IO.File.Delete(path);
                                    excelfile.SaveAs(path);
                                    //read data from excel file

                                    var records = new List<PlanilllaDetalle_Excel>();
                                    int iContador = 0;
                                    int iContadorExistente = 0;
                                    int iContadorNoIngresado = 0;
                                    int iContadorIngresado = 0;
                                    DateTime? fecha_Inicial_Periodo_;
                                    DateTime? fecha_Final_Periodo_;
                                    DateTime? fecha_Cierre_Acta_;
                                    DateTime? fecha_Asignación_;

                                    
                                    DateTime? fecha_Fin_Permiso_Municipal_Calc_;
                                    DateTime? fecha_Asignación_OT_;
                                    DateTime? fecha_Ejecución_;
                                    DateTime? fecha_Legalización_;
                                    DateTime? fecha_Ejecucion_OT_Padre_;
                                    DateTime? fecha_Pago_Sistema_Externo_;

                                string mensaje = string.Empty;

                                    using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Excel_OT/"), excelfile.FileName), FileMode.Open, FileAccess.Read))
                                    {
                                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                                        {
                                            while (reader.Read())
                                            {
                                                if (iContador != 0)
                                                {

                                                    records.Add(new PlanilllaDetalle_Excel
                                                    {

                                                        Código_Acta = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString(),
                                                        Base_Administrativa = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                                                        Descripción_de_Acta = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                                                        Código_Periodo = reader.GetValue(3) == null ? null : reader.GetValue(3).ToString(),
                                                        Descripción_de_Periodo = reader.GetValue(4) == null ? null : reader.GetValue(4).ToString(),
                                                        Código_Tipo_de_Acta = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString(),
                                                        Descripción_de_Tipo_de_Acta = reader.GetValue(6) == null ? null : reader.GetValue(6).ToString(),
                                                        fecha_Inicial_Periodo = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString(),
                                                        fecha_Final_Periodo = reader.GetValue(8) == null ? "" : reader.GetValue(8).ToString(),
                                                        fecha_Cierre_Acta = reader.GetValue(9) == null ? "" : reader.GetValue(9).ToString(),
                                                        Estado_Acta = reader.GetValue(10) == null ? null : reader.GetValue(10).ToString(),
                                                        Descripción_de_Estado_Acta = reader.GetValue(11) == null ? null : reader.GetValue(11).ToString(),
                                                        Orden_Raiz = reader.GetValue(12) == null ? null : reader.GetValue(12).ToString(),
                                                        No_Orden_Padre = reader.GetValue(13) == null ? null : reader.GetValue(13).ToString(),
                                                        No_Orden = reader.GetValue(14) == null ? "" : reader.GetValue(14).ToString(),
                                                        Código_Detalle_Acta = reader.GetValue(15) == null ? "" : reader.GetValue(15).ToString(),
                                                        Código_Items = reader.GetValue(16) == null ? "" : reader.GetValue(16).ToString(),
                                                        Descripción_de_Items = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString(),
                                                        Cantidad = reader.GetValue(18) == null ? null : reader.GetValue(18).ToString(),
                                                        Código_Unidad = reader.GetValue(19) == null ? null : reader.GetValue(19).ToString(),
                                                        Unidad = reader.GetValue(20) == null ? null : reader.GetValue(20).ToString(),
                                                        Valor_Unitario = reader.GetValue(21) == null ? "" : reader.GetValue(21).ToString(),
                                                        Valor_Total = reader.GetValue(22) == null ? "" : reader.GetValue(22).ToString(),
                                                        Código_Listado_de_Costo = reader.GetValue(23) == null ? "" : reader.GetValue(23).ToString(),
                                                        Descripción_Listado_de_Costo = reader.GetValue(24) == null ? null : reader.GetValue(24).ToString(),
                                                        Tipo_Generación = reader.GetValue(25) == null ? null : reader.GetValue(25).ToString(),
                                                        Código_de_Plan_de_Condición = reader.GetValue(26) == null ? null : reader.GetValue(26).ToString(),
                                                        Tipo_de_Trabajo = reader.GetValue(27) == null ? null : reader.GetValue(27).ToString(),
                                                        Descripción_de_Tipo_de_Trabajo = reader.GetValue(28) == null ? "" : reader.GetValue(28).ToString(),
                                                        Actividad = reader.GetValue(29) == null ? "" : reader.GetValue(29).ToString(),
                                                        fecha_Asignación = reader.GetValue(30) == null ? "" : reader.GetValue(30).ToString(),
                                                        fecha_Fin_Permiso_Municipal_Calc = reader.GetValue(31) == null ? null : reader.GetValue(31).ToString(),
                                                        fecha_Asignación_OT = reader.GetValue(32) == null ? null : reader.GetValue(32).ToString(),
                                                        fecha_Ejecución = reader.GetValue(33) == null ? null : reader.GetValue(33).ToString(),
                                                        fecha_Legalización = reader.GetValue(34) == null ? null : reader.GetValue(34).ToString(),
                                                        fecha_Ejecucion_OT_Padre = reader.GetValue(35) == null ? "" : reader.GetValue(35).ToString(),
                                                        Tiempo_Promedio_Inc_Desc_OT = reader.GetValue(36) == null ? "" : reader.GetValue(36).ToString(),
                                                        Tiempo_Promedio_Desc_Descarga = reader.GetValue(37) == null ? "" : reader.GetValue(37).ToString(),
                                                        Tiempo_Transcurrido_HORAS = reader.GetValue(38) == null ? null : reader.GetValue(38).ToString(),
                                                        Tiempo_Optimo_Incentivo = reader.GetValue(39) == null ? null : reader.GetValue(39).ToString(),
                                                        Tiempo_Máximo_Incentivo = reader.GetValue(40) == null ? null : reader.GetValue(40).ToString(),
                                                        Porcentaje_Incentivo = reader.GetValue(41) == null ? null : reader.GetValue(41).ToString(),
                                                        Tiempo_Optimo_Desc_Atención = reader.GetValue(42) == null ? "" : reader.GetValue(42).ToString(),
                                                        Tiempo_Máximo_Desc_Atención = reader.GetValue(43) == null ? "" : reader.GetValue(43).ToString(),
                                                        Porcentaje_de_Desc_Atención = reader.GetValue(44) == null ? "" : reader.GetValue(44).ToString(),
                                                        Tiempo_Máximo_Desc_Descarga = reader.GetValue(45) == null ? null : reader.GetValue(45).ToString(),
                                                        Tiempo_Excede_Desc_Descarga = reader.GetValue(46) == null ? null : reader.GetValue(46).ToString(),
                                                        Porcentaje_Desc_Descarga = reader.GetValue(47) == null ? null : reader.GetValue(47).ToString(),
                                                        Valor_Aplica_Desc_Atención_OT = reader.GetValue(48) == null ? null : reader.GetValue(48).ToString(),
                                                        Código_Contrato = reader.GetValue(49) == null ? "" : reader.GetValue(49).ToString(),
                                                        Descripción_Contrato = reader.GetValue(50) == null ? "" : reader.GetValue(50).ToString(),
                                                        Tipo_de_Contrato = reader.GetValue(51) == null ? "" : reader.GetValue(51).ToString(),
                                                        Descripción_de_Tipo_de_Contrato = reader.GetValue(52) == null ? null : reader.GetValue(52).ToString(),
                                                        Contratista = reader.GetValue(53) == null ? null : reader.GetValue(53).ToString(),
                                                        Descripción_Contratista = reader.GetValue(54) == null ? null : reader.GetValue(54).ToString(),
                                                        Porcentaje_Costo_Indirecto_Contrato = reader.GetValue(55) == null ? null : reader.GetValue(55).ToString(),
                                                        Porcentaje_Fondo_Garantia_Contrato = reader.GetValue(56) == null ? "" : reader.GetValue(56).ToString(),
                                                        Porcentaje_Amortización_Contrato = reader.GetValue(57) == null ? "" : reader.GetValue(57).ToString(),
                                                        Descuento_General = reader.GetValue(58) == null ? "" : reader.GetValue(58).ToString(),
                                                        Código_Unidad_Operativa = reader.GetValue(59) == null ? null : reader.GetValue(59).ToString(),
                                                        Descripción_Unidad_Operativa = reader.GetValue(60) == null ? null : reader.GetValue(60).ToString(),
                                                        Terminal = reader.GetValue(61) == null ? null : reader.GetValue(61).ToString(),
                                                        Valor_Base = reader.GetValue(62) == null ? null : reader.GetValue(62).ToString(),
                                                        Cumplimiento = reader.GetValue(63) == null ? "" : reader.GetValue(63).ToString(),
                                                        Tiempo_Contractual_HORAS = reader.GetValue(64) == null ? "" : reader.GetValue(64).ToString(),
                                                        Tiempo_de_Permiso_Municipal_HORAS = reader.GetValue(65) == null ? "" : reader.GetValue(65).ToString(),
                                                        Tipo_Clasifica = reader.GetValue(66) == null ? null : reader.GetValue(66).ToString(),
                                                        Inventario = reader.GetValue(67) == null ? null : reader.GetValue(67).ToString(),
                                                        Tiene_Caso_Especial = reader.GetValue(68) == null ? null : reader.GetValue(68).ToString(),
                                                        Tipoe_de_Relación = reader.GetValue(69) == null ? null : reader.GetValue(69).ToString(),
                                                        Nro_Contrato = reader.GetValue(70) == null ? "" : reader.GetValue(70).ToString(),
                                                        Nro_Producto = reader.GetValue(71) == null ? "" : reader.GetValue(71).ToString(),
                                                        Código = reader.GetValue(72) == null ? "" : reader.GetValue(72).ToString(),
                                                        Tipo_de_Irregularidad = reader.GetValue(73) == null ? null : reader.GetValue(73).ToString(),
                                                        Descripción_de_Proyecto = reader.GetValue(74) == null ? null : reader.GetValue(74).ToString(),
                                                        Usuario_Finalizo = reader.GetValue(75) == null ? null : reader.GetValue(75).ToString(),
                                                        Código_Tipo_Comentario = reader.GetValue(76) == null ? null : reader.GetValue(76).ToString(),
                                                        Comentario = reader.GetValue(77) == null ? "" : reader.GetValue(77).ToString(),
                                                        Observación_Orden_Actividad = reader.GetValue(78) == null ? "" : reader.GetValue(78).ToString(),
                                                        fecha_Pago_Sistema_Externo = reader.GetValue(79) == null ? "" : reader.GetValue(79).ToString(),
                                                        Número_Factura_Sistema_Externo = reader.GetValue(80) == null ? null : reader.GetValue(80).ToString(),
                                                        Cod_estado_orden = reader.GetValue(81) == null ? null : reader.GetValue(81).ToString()




                                                    });

                                                string Código_Acta = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString();
                                                string Base_Administrativa = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString();
                                                string Descripción_de_Acta = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString();
                                                string Código_Periodo = reader.GetValue(3) == null ? null : reader.GetValue(3).ToString();
                                                string Descripción_de_Periodo = reader.GetValue(4) == null ? null : reader.GetValue(4).ToString();
                                                string Código_Tipo_de_Acta = reader.GetValue(5) == null ? null : reader.GetValue(5).ToString();
                                                string Descripción_de_Tipo_de_Acta = reader.GetValue(6) == null ? null : reader.GetValue(6).ToString();
                                                string fecha_Inicial_Periodo = reader.GetValue(7) == null ? "" : reader.GetValue(7).ToString();
                                                if (fecha_Inicial_Periodo != null)
                                                {
                                                    fecha_Inicial_Periodo_ = Convert.ToDateTime(fecha_Inicial_Periodo);
                                                }
                                                else
                                                {
                                                    fecha_Inicial_Periodo_ = null;
                                                }
                                                string fecha_Final_Periodo = reader.GetValue(8) == null ? "" : reader.GetValue(8).ToString();
                                                if (fecha_Final_Periodo != null)
                                                {
                                                    fecha_Final_Periodo_ = Convert.ToDateTime(fecha_Final_Periodo);
                                                }
                                                else
                                                {
                                                    fecha_Final_Periodo_ = null;
                                                }
                                                string fecha_Cierre_Acta = reader.GetValue(9) == null ? "" : reader.GetValue(9).ToString();
                                                if (fecha_Cierre_Acta != null)
                                                {
                                                    fecha_Cierre_Acta_ = Convert.ToDateTime(fecha_Cierre_Acta);
                                                }
                                                else
                                                {
                                                    fecha_Cierre_Acta_ = null;
                                                }
                                                string Estado_Acta = reader.GetValue(10) == null ? null : reader.GetValue(10).ToString();
                                                string Descripción_de_Estado_Acta = reader.GetValue(11) == null ? null : reader.GetValue(11).ToString();
                                                string Orden_Raiz = reader.GetValue(12) == null ? null : reader.GetValue(12).ToString();
                                                string No_Orden_Padre = reader.GetValue(13) == null ? null : reader.GetValue(13).ToString();
                                                string No_Orden = reader.GetValue(14) == null ? "" : reader.GetValue(14).ToString();
                                                string Código_Detalle_Acta = reader.GetValue(15) == null ? "" : reader.GetValue(15).ToString();
                                                string Código_Items = reader.GetValue(16) == null ? "" : reader.GetValue(16).ToString();
                                                string Descripción_de_Items = reader.GetValue(17) == null ? null : reader.GetValue(17).ToString();
                                                string Cantidad = reader.GetValue(18) == null ? null : reader.GetValue(18).ToString();
                                                string Código_Unidad = reader.GetValue(19) == null ? null : reader.GetValue(19).ToString();
                                                string Unidad = reader.GetValue(20) == null ? null : reader.GetValue(20).ToString();
                                                string Valor_Unitario = reader.GetValue(21) == null ? "" : reader.GetValue(21).ToString();
                                                string Valor_Total = reader.GetValue(22) == null ? "" : reader.GetValue(22).ToString();
                                                string Código_Listado_de_Costo = reader.GetValue(23) == null ? "" : reader.GetValue(23).ToString();
                                                string Descripción_Listado_de_Costo = reader.GetValue(24) == null ? null : reader.GetValue(24).ToString();
                                                string Tipo_Generación = reader.GetValue(25) == null ? null : reader.GetValue(25).ToString();
                                                string Código_de_Plan_de_Condición = reader.GetValue(26) == null ? null : reader.GetValue(26).ToString();
                                                string Tipo_de_Trabajo = reader.GetValue(27) == null ? null : reader.GetValue(27).ToString();
                                                string Descripción_de_Tipo_de_Trabajo = reader.GetValue(28) == null ? "" : reader.GetValue(28).ToString();
                                                string Actividad = reader.GetValue(29) == null ? "" : reader.GetValue(29).ToString();
                                                string fecha_Asignación = reader.GetValue(30) == null ? "" : reader.GetValue(30).ToString();
                                                if (fecha_Asignación != null)
                                                {
                                                    fecha_Asignación_ = Convert.ToDateTime(fecha_Asignación);
                                                }
                                                else
                                                {
                                                    fecha_Asignación_ = null;
                                                }
                                                string fecha_Fin_Permiso_Municipal_Calc = reader.GetValue(31) == null ? null : reader.GetValue(31).ToString();
                                                if (fecha_Fin_Permiso_Municipal_Calc != null)
                                                {
                                                    fecha_Fin_Permiso_Municipal_Calc_ = Convert.ToDateTime(fecha_Fin_Permiso_Municipal_Calc);
                                                }
                                                else
                                                {
                                                    fecha_Fin_Permiso_Municipal_Calc_ = null;
                                                }
                                                string fecha_Asignación_OT = reader.GetValue(32) == null ? null : reader.GetValue(32).ToString();
                                                if (fecha_Asignación_OT != null)
                                                {
                                                    fecha_Asignación_OT_ = Convert.ToDateTime(fecha_Asignación_OT);
                                                }
                                                else
                                                {
                                                    fecha_Asignación_OT_ = null;
                                                }
                                                string fecha_Ejecución = reader.GetValue(33) == null ? null : reader.GetValue(33).ToString();
                                                if (fecha_Ejecución != null)
                                                {
                                                    fecha_Ejecución_ = Convert.ToDateTime(fecha_Ejecución);
                                                }
                                                else
                                                {
                                                    fecha_Ejecución_ = null;
                                                }
                                                string fecha_Legalización = reader.GetValue(34) == null ? null : reader.GetValue(34).ToString();
                                                if (fecha_Legalización != null)
                                                {
                                                    fecha_Legalización_ = Convert.ToDateTime(fecha_Legalización);
                                                }
                                                else
                                                {
                                                    fecha_Legalización_ = null;
                                                }
                                                string fecha_Ejecucion_OT_Padre = reader.GetValue(35) == null ? "" : reader.GetValue(35).ToString();
                                                if (fecha_Ejecucion_OT_Padre != null)
                                                {
                                                    fecha_Ejecucion_OT_Padre_ = Convert.ToDateTime(fecha_Ejecucion_OT_Padre);
                                                }
                                                else
                                                {
                                                    fecha_Ejecucion_OT_Padre_ = null;
                                                }
                                                string Tiempo_Promedio_Inc_Desc_OT = reader.GetValue(36) == null ? "" : reader.GetValue(36).ToString();
                                                string Tiempo_Promedio_Desc_Descarga = reader.GetValue(37) == null ? "" : reader.GetValue(37).ToString();
                                                string Tiempo_Transcurrido_HORAS = reader.GetValue(38) == null ? null : reader.GetValue(38).ToString();
                                                string Tiempo_Optimo_Incentivo = reader.GetValue(39) == null ? null : reader.GetValue(39).ToString();
                                                string Tiempo_Máximo_Incentivo = reader.GetValue(40) == null ? null : reader.GetValue(40).ToString();
                                                string Porcentaje_Incentivo = reader.GetValue(41) == null ? null : reader.GetValue(41).ToString();
                                                string Tiempo_Optimo_Desc_Atención = reader.GetValue(42) == null ? "" : reader.GetValue(42).ToString();
                                                string Tiempo_Máximo_Desc_Atención = reader.GetValue(43) == null ? "" : reader.GetValue(43).ToString();
                                                string Porcentaje_de_Desc_Atención = reader.GetValue(44) == null ? "" : reader.GetValue(44).ToString();
                                                string Tiempo_Máximo_Desc_Descarga = reader.GetValue(45) == null ? null : reader.GetValue(45).ToString();
                                                string Tiempo_Excede_Desc_Descarga = reader.GetValue(46) == null ? null : reader.GetValue(46).ToString();
                                                string Porcentaje_Desc_Descarga = reader.GetValue(47) == null ? null : reader.GetValue(47).ToString();
                                                string Valor_Aplica_Desc_Atención_OT = reader.GetValue(48) == null ? null : reader.GetValue(48).ToString();
                                                string Código_Contrato = reader.GetValue(49) == null ? "" : reader.GetValue(49).ToString();
                                                string Descripción_Contrato = reader.GetValue(50) == null ? "" : reader.GetValue(50).ToString();
                                                string Tipo_de_Contrato = reader.GetValue(51) == null ? "" : reader.GetValue(51).ToString();
                                                string Descripción_de_Tipo_de_Contrato = reader.GetValue(52) == null ? null : reader.GetValue(52).ToString();
                                                string Contratista = reader.GetValue(53) == null ? null : reader.GetValue(53).ToString();
                                                string Descripción_Contratista = reader.GetValue(54) == null ? null : reader.GetValue(54).ToString();
                                                string Porcentaje_Costo_Indirecto_Contrato = reader.GetValue(55) == null ? null : reader.GetValue(55).ToString();
                                                string Porcentaje_Fondo_Garantia_Contrato = reader.GetValue(56) == null ? "" : reader.GetValue(56).ToString();
                                                string Porcentaje_Amortización_Contrato = reader.GetValue(57) == null ? "" : reader.GetValue(57).ToString();
                                                string Descuento_General = reader.GetValue(58) == null ? "" : reader.GetValue(58).ToString();
                                                string Código_Unidad_Operativa = reader.GetValue(59) == null ? null : reader.GetValue(59).ToString();
                                                string Descripción_Unidad_Operativa = reader.GetValue(60) == null ? null : reader.GetValue(60).ToString();
                                                string Terminal = reader.GetValue(61) == null ? null : reader.GetValue(61).ToString();
                                                string Valor_Base = reader.GetValue(62) == null ? null : reader.GetValue(62).ToString();
                                                string Cumplimiento = reader.GetValue(63) == null ? "" : reader.GetValue(63).ToString();
                                                string Tiempo_Contractual_HORAS = reader.GetValue(64) == null ? "" : reader.GetValue(64).ToString();
                                                string Tiempo_de_Permiso_Municipal_HORAS = reader.GetValue(65) == null ? "" : reader.GetValue(65).ToString();
                                                string Tipo_Clasifica = reader.GetValue(66) == null ? null : reader.GetValue(66).ToString();
                                                string Inventario = reader.GetValue(67) == null ? null : reader.GetValue(67).ToString();
                                                string Tiene_Caso_Especial = reader.GetValue(68) == null ? null : reader.GetValue(68).ToString();
                                                string Tipoe_de_Relación = reader.GetValue(69) == null ? null : reader.GetValue(69).ToString();
                                                string Nro_Contrato = reader.GetValue(70) == null ? "" : reader.GetValue(70).ToString();
                                                string Nro_Producto = reader.GetValue(71) == null ? "" : reader.GetValue(71).ToString();
                                                string Código = reader.GetValue(72) == null ? "" : reader.GetValue(72).ToString();
                                                string Tipo_de_Irregularidad = reader.GetValue(73) == null ? null : reader.GetValue(73).ToString();
                                                string Descripción_de_Proyecto = reader.GetValue(74) == null ? null : reader.GetValue(74).ToString();
                                                string Usuario_Finalizo = reader.GetValue(75) == null ? null : reader.GetValue(75).ToString();
                                                string Código_Tipo_Comentario = reader.GetValue(76) == null ? null : reader.GetValue(76).ToString();
                                                string Comentario = reader.GetValue(77) == null ? "" : reader.GetValue(77).ToString();
                                                string Observación_Orden_Actividad = reader.GetValue(78) == null ? "" : reader.GetValue(78).ToString();
                                                string fecha_Pago_Sistema_Externo = reader.GetValue(79) == null ? "" : reader.GetValue(79).ToString();
                                                if (fecha_Pago_Sistema_Externo != null)
                                                {
                                                    fecha_Pago_Sistema_Externo_ = Convert.ToDateTime(fecha_Pago_Sistema_Externo);
                                                }
                                                else
                                                {
                                                    fecha_Pago_Sistema_Externo_ = null;
                                                }
                                                string Número_Factura_Sistema_Externo = reader.GetValue(80) == null ? null : reader.GetValue(80).ToString();
                                                string Cod_estado_orden = reader.GetValue(81) == null ? null : reader.GetValue(81).ToString();

                                                var user_id = System.Web.HttpContext.Current.Session["usuario"];
                                                string usuario = user_id.ToString();

                                                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                                    DataBase_Externo database4 = new DataBase_Externo();
                                                    //var OTRepetido = db.MT_Planilla.Where(x => x.Identificador == Identificador).ToList();

                                                    //if (OTRepetido.Count() <= 0)
                                                    //{
                                                        //if (variable != "")
                                                        //{
                                                            int? contrato_proyecto;
                                                            var client_contr = "";
                                                //if (Tipo == 1)
                                                //{
                                                //    contrato_proyecto = ProyectoID;
                                                //    client_contr = db.MT_Proyecto.Where(vq => vq.Id_Proyecto == contrato_proyecto).FirstOrDefault().Id_Cliente;

                                                //}
                                                //else
                                                //{
                                                //    contrato_proyecto = ContratoID;
                                                //    client_contr = db.MT_Contrato.Where(vq => vq.Id_Contrato == contrato_proyecto).FirstOrDefault().Id_Cliente;

                                                //}

                                                //var trabjservplan = from tipotrab in db.MT_TipoTrabajo.Where(vq => vq.Descripcion == variable && vq.Estado == "A" && vq.Id_Cliente == client_contr)
                                                // join tiposect in db.MT_Servicio.Where(vq => vq.Id_Empresa == empresa_id.ToString())
                                                // on tipotrab.Id_Servicio equals tiposect.Id_Servicio
                                                // select new { tipotrab.Id_TipoTrabajo };


                                                //var trabajoservicioplanlist = trabjservplan.ToList();

                                                //if (trabajoservicioplanlist.Count > 0)
                                                //{
                                                //if (trabajoservicioplanlist.ElementAt(0).Id_TipoTrabajo != null)
                                                //{
                                                var ac = database4.IngresarPlanilla_Detalle(Código_Acta,	Base_Administrativa,	Descripción_de_Acta,	Código_Periodo,	Descripción_de_Periodo,	Código_Tipo_de_Acta,	Descripción_de_Tipo_de_Acta,	fecha_Inicial_Periodo_,	fecha_Final_Periodo_,	fecha_Cierre_Acta_,	Estado_Acta,	Descripción_de_Estado_Acta,	Orden_Raiz,	No_Orden_Padre,	No_Orden,	Código_Detalle_Acta,	Código_Items,	Descripción_de_Items,	Cantidad,	Código_Unidad,	Unidad,	Valor_Unitario,	Valor_Total,	Código_Listado_de_Costo,	Descripción_Listado_de_Costo,	Tipo_Generación,	Código_de_Plan_de_Condición,	Tipo_de_Trabajo,	Descripción_de_Tipo_de_Trabajo,	Actividad,	fecha_Asignación_,	fecha_Fin_Permiso_Municipal_Calc_,	fecha_Asignación_OT_,	fecha_Ejecución_,	fecha_Legalización_,	fecha_Ejecucion_OT_Padre_,	Tiempo_Promedio_Inc_Desc_OT,	Tiempo_Promedio_Desc_Descarga,	Tiempo_Transcurrido_HORAS,	Tiempo_Optimo_Incentivo,	Tiempo_Máximo_Incentivo,	Porcentaje_Incentivo,	Tiempo_Optimo_Desc_Atención,	Tiempo_Máximo_Desc_Atención,	Porcentaje_de_Desc_Atención,	Tiempo_Máximo_Desc_Descarga,	Tiempo_Excede_Desc_Descarga,	Porcentaje_Desc_Descarga,	Valor_Aplica_Desc_Atención_OT,	Código_Contrato,	Descripción_Contrato,	Tipo_de_Contrato,	Descripción_de_Tipo_de_Contrato,	Contratista,	Descripción_Contratista,	Porcentaje_Costo_Indirecto_Contrato,	Porcentaje_Fondo_Garantia_Contrato,	Porcentaje_Amortización_Contrato,	Descuento_General,	Código_Unidad_Operativa,	Descripción_Unidad_Operativa,	Terminal,	Valor_Base,	Cumplimiento,	Tiempo_Contractual_HORAS,	Tiempo_de_Permiso_Municipal_HORAS,	Tipo_Clasifica,	Inventario,	Tiene_Caso_Especial,	Tipoe_de_Relación,	Nro_Contrato,	Nro_Producto,	Código,	Tipo_de_Irregularidad,	Descripción_de_Proyecto,	Usuario_Finalizo,	Código_Tipo_Comentario,	Comentario,	Observación_Orden_Actividad,	fecha_Pago_Sistema_Externo_,	Número_Factura_Sistema_Externo,	Cod_estado_orden, usuario, empresa_id.ToString());
                                                                    if (ac != -1)
                                                                    {
                                                                        iContadorIngresado = iContadorIngresado + 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                        //mensaje = mensaje + "La orden " + Identificador + "El tipo trabajo planeado y/o tipo trabajo ejecutado no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                    mensaje = mensaje + "La orden El tipo trabajo planeado y/o tipo trabajo ejecutado no existen guardadas. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                                    }
                                                                //}
                                                                //else

                                                                //{
                                                                //    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                                //    mensaje = mensaje + "En La orden " + Identificador + " Los campos tipo trabajo planeado y/o ejecutado estan vacios en el excel. Favor verificar. \r\n";

                                                                //}
                                                            //}
                                                            //else
                                                            //{
                                                            //    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                            //    mensaje = mensaje + "En La orden " + Identificador + " El tipo trabajo planeado y/o tipo trabajo ejecutado no se encuentran registrados. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                            //}

                                                        //}
                                                        //else
                                                        //{
                                                        //    iContadorNoIngresado = iContadorNoIngresado + 1;
                                                        //    mensaje = mensaje + "En La orden " + Identificador + " El campo tipo trabajo planeado y/o tipo trabajo ejecutado las columnas estan vacias. Favor revise y/o ingrese el tipo trabajo que deseee utilizar. \r\n";
                                                        //}
                                                    //}
                                                    //else
                                                    //{
                                                    //    iContadorExistente = iContadorExistente + 1;
                                                    //}

                                                }

                                                iContador++;
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
                                    return RedirectToAction("Detalle_Planilla");
                                    //return View("OrdenTrabajo");
                                }
                                else
                                {
                                    TempData["mensaje_error"] = "Tipo de archivo es incorrecto<br>";
                                    return RedirectToAction("Detalle_Planilla");
                                }
                            //}
                            //else
                            //{
                            //    TempData["mensaje_error"] = "Debe seleccionar un contrato o un proyecto";
                            //    return RedirectToAction("Planilla");
                            //}
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

        [HttpGet]
        public ActionResult Editar_Detalle_Planilla(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                MT_Planilla_Detalle mT_Detalle = db.MT_Planilla_Detalle.Find(id);
                bool seleccion = false;
                if (mT_Detalle != null)
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                    foreach (var item in listaItems)
                    {
                        //if (mT_Detalle.Id_Item == item.cod_item)
                        //{
                            seleccion = true;
                        //}

                        items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                    }

                    SelectList selectlistaItems = new SelectList(items, "Value", "Text");
                    
                    ViewBag.listaItems = selectlistaItems;

                    return View(mT_Detalle);
                }
                else
                {
                    var id_Planilla = System.Web.HttpContext.Current.Session["id_Planilla"];

                    if (id_Planilla == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idPlanilla = int.Parse(id_Planilla.ToString());
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Detalle_Planilla", new { id = idPlanilla });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Detalle_Planilla([Bind(Include = "Id_Planilla_Detalle,Id_Planilla,Id_Item,Cantidad, Precio,Valor,IVA")] MT_Planilla_Detalle mT_Detalle)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
            {
                var id_Planilla = System.Web.HttpContext.Current.Session["id_Planilla"];
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
                        
                        db.Entry(mT_Detalle).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Detalle Planilla actualizado";

                        return RedirectToAction("Detalle_Planilla", new { id = id_Planilla });

                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Detalle_Planilla", new { id = id_Planilla });
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
        #endregion

        


    }
}
