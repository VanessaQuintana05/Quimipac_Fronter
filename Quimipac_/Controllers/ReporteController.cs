//using ClosedXML.Excel;
using ClosedXML.Excel;
using Quimipac_.Models;
using Quimipac_.Repositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Quimipac_.Controllers
{
    public class ReporteController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        // GET: Reporte
        public ActionResult reporteContrato()//DateTime? FIni1, DateTime? FIni2)
        {
            
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresacod = empresa_id.ToString();

                ViewBag.FechaIni_ = DateTime.Today.ToString("yyyy-MM-dd");
                ViewBag.FechaFin_ = DateTime.Today.ToString("yyyy-MM-dd");
                ViewBag.ContratosSearch = null;

                //Combos Filtros
                var dbe = new DataBase_Externo();
                //*?ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();

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
                return View();
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
        }
        [HttpPost]
        public ActionResult reporteContrato_Anterior(DateTime? FIni1, DateTime? FIni2)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    if (FIni1 != null && FIni2 != null)
                    {
                        //var contratoLI = db.MT_Contrato.Where(vq => vq.Fecha_Inicio >= FIni1 && vq.Fecha_Inicio <= FIni2 && vq.Id_Empresa == empresa_id.ToString() && vq.Estado != 2156).ToList();
                        var contratoLI = db.MT_Contrato.Where(vq => vq.Fecha_Inicio >= FIni1 && vq.Fecha_Inicio <= FIni2 && vq.Id_Empresa == empresa_id.ToString() && vq.Estado != 1160).ToList();
                        //ViewBag.ContratosSearch = contratoLI;
                        ViewBag.FechaIni_ = FIni1?.ToString("yyyy-MM-dd");
                        ViewBag.FechaFin_ = FIni2?.ToString("yyyy-MM-dd");
                        ViewBag.ContratosSearch = Contratos_AuxLI(contratoLI);
                        return View(contratoLI);
                    }
                    ViewBag.FechaIni_ = DateTime.Today.ToString("yyyy-MM-dd");
                    ViewBag.FechaFin_ = DateTime.Today.ToString("yyyy-MM-dd");
                    ViewBag.ContratosSearch = null;
                    return View();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("reporteContrato:" + e.Message);
                    return RedirectToAction("Error", "Errores");
                }
            }
            else
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

        }

        [HttpPost]
        public ActionResult Contrato_Filter(DateTime? Fecha_Inicio, DateTime? Fecha_registro, List<string> check_parametro, string SelectedAndOr, int? MenuSelectedFecha)
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

                var lkbusqueda = bde.execute_filter_contrato(Fecha_Inicio, Fecha_registro, check_parametro, SelectedAndOr, MenuSelectedFecha);

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                lkbusqueda = lkbusqueda.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                //DATA GET CONTRATO
                //ViewBag.listaContrato = lkbusqueda;
                TempData["listaFiltro"] = lkbusqueda;
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

                //return View(lkbusqueda;


                return RedirectToAction("reporteContrato");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }




        public void TemplateClosedXML()
        {

            //Definimos la plantilla y la utilizamos con la libreria ClosedXML
            var template = Server.MapPath("~/Content/templateReport/CotizMejoraFormatoHYDR.xlsx");
            using (var wb = new XLWorkbook(template))
            {
                //Ponemos algunos valores en el documento
                var fecha = DateTime.Now.ToString("dd/MM/yyy");
                wb.Worksheets.Worksheet(1).Cell(8, 4).Value = fecha;//Fecha Solicitud
                wb.Worksheets.Worksheet(1).Cell(8, 8).Value = fecha;//Nro. Requerimineto
                wb.Worksheets.Worksheet(1).Cell(10, 7).Value = fecha;//Desea Instalacion

                wb.Worksheets.Worksheet(1).Cell(14, 5).Value = fecha;//Tipo Softw Licencia
                wb.Worksheets.Worksheet(1).Cell(16, 5).Value = fecha;//Tipo Softw Otros

                wb.Worksheets.Worksheet(1).Cell(14, 9).Value = fecha;//Drivers

                wb.Worksheets.Worksheet(1).Cell(20, 2).Value = fecha;//Descripcion Solicita
                wb.Worksheets.Worksheet(1).Cell(31, 9).Value = fecha;//Fecha aprobacion

                wb.Worksheets.Worksheet(1).Cell(39, 2).Value = fecha;//Descripcion adqurido

                wb.Worksheets.Worksheet(1).Cell(42, 9).Value = fecha;//Fecha Cumplimiento

                wb.Worksheets.Worksheet(1).Cell(49, 7).Value = fecha;//Fecha Conformidad

                /*
                //Podemos insertar un DataTable
                wb.Worksheets.Worksheet(1).Cell(9, 1).InsertTable(dtReporte);
                //Aplicamos los filtros y formatos a la tabla 
                wb.Worksheets.Worksheet(1).Table("Table1").ShowAutoFilter = true;
                wb.Worksheets.Worksheet(1).Table("Table1").Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;
                wb.Worksheets.Worksheet(1).Columns(2, 2 + dtReporte.Columns.Count).AdjustToContents();

                //Limitamos el ancho de las columnas a 60
                foreach (var column in wb.Worksheets.Worksheet(1).Columns())
                    if (column.Width > 60)
                    {
                        column.Width = 60;
                        column.Style.Alignment.WrapText = true;
                    }

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                */
                //Enviamos el archivo al cliente
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + "Excelnuevo0" + ".xlsx\"");

                using (var myMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(myMemoryStream);
                    myMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        public List<ReporteContrato> Contratos_AuxLI(List<MT_Contrato> itemLI)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

            var ContAUX = new List<ReporteContrato>();
            foreach (var item in itemLI)
            {
                string nomb_cliente = string.Empty;
                string responsable = string.Empty;
                string prorroga_inical = string.Empty;
                string plazo_prorroga = string.Empty;

                string NombreEmp = string.Empty;
                var EmpresaNomb = db.sp_LINK_ConsultaEmpresas_Nombre(empresa_id.ToString()).SingleOrDefault();//.ToList();
                NombreEmp = (EmpresaNomb != null) ? EmpresaNomb.ToString() : "";

                using (var ctx = new BD_QUIMIPACEntities())
                {
                    string sqlquery_ = " SELECT cli.* FROM MT_Contrato c Inner Join [SADATABASE]..[DBA].[clientes] cli on c.Id_Cliente = cli.cod_cli"
                                      + " Where cli.cia_codigo = '" + empresa_id.ToString() + "' and c.Id_Contrato=" + item.Id_Contrato + " and c.Id_Cliente = " + item.Id_Cliente;
                    var ncliente = ctx.Database.SqlQuery<sp_LINK_ConsultaClientes_Result>(sqlquery_).ToList();
                    nomb_cliente = ncliente.Count() > 0 ? ncliente.ElementAt(0).nom_cli : "";

                    string sqlquery = " select p.primer_nombre as Nombres_Completos,ISNULL(p.fecha_nacimiento,'')fecha_nacimiento,p.* FROM MT_Contrato c "
                                + " INNER JOIN [SADATABASE]..[DBA].[rh_persona] p on c.Responsable = p.id_persona"
                                + " WHERE c.Responsable = " + item.Responsable + " and p.estado = '001' and c.Id_Contrato = " + item.Id_Contrato;
                    var Nrespons = ctx.Database.SqlQuery<sp_LINK_ConsultaPersonas_Result>(sqlquery).ToList();
                    responsable = Nrespons.Count() > 0 ? Nrespons.ElementAt(0).Nombres_Completos : "";
                }
                //var auxProrroga = db.MT_Contrato_Prorroga.Where(vq=>vq.);
                var listProrroga = db.MT_Contrato_Prorroga.Where(vq => vq.Id_Contrato == item.Id_Contrato).OrderBy(vq => vq.Fecha_Prorroga).Take(1).ToList();// item.pro;
                if (listProrroga.Count() > 0)
                {
                    prorroga_inical = listProrroga.ElementAt(0).Fecha_Prorroga?.ToString("dd/MM/yyyy");
                    plazo_prorroga = listProrroga.ElementAt(0).Dia_Prorroga.ToString();
                }

                var nuevoObj = new ReporteContrato
                {
                    ci_emp = item.Codigo_Interno.Length > 3 ? item.Codigo_Interno.Substring(0, 3) : "",
                    ci_cliente = item.Codigo_Interno.Substring(3, 3),
                    nomb_cliente = nomb_cliente,
                    ci_unidad = item.Codigo_Interno.Substring(6, 3),
                    ci_proy = item.Codigo_Interno.Substring(9, 3),
                    ci_secuencia = item.Codigo_Interno.Substring(12, 3),
                    ci_anio = item.Codigo_Interno.Substring(15, 4),

                    cod_sec_interno = item.Codigo_Interno ?? "",
                    cod_sec_interno_padre = item.Codigo_interno_padre ?? "",
                    cod_contrato_asociado = item.Codigo_Cliente ?? "",//.Contrato_Padre != null 
                    fecha_inicial = item.Fecha_Inicio != null ? item.Fecha_Inicio?.ToString("dd/MM/yyyy") : "",
                    plazo_contrato = item.Dia_Plazo != null ? item.Dia_Plazo.ToString() : "",
                    fecha_fin = item.Fecha_Fin != null ? item.Fecha_Fin?.ToString("dd/MM/yyyy") : "",
                    monto = item.monto != null ? item.monto.ToString() : "",

                    responsable = responsable,
                    costo = item.costo != null ? item.costo.ToString() : "",
                    detalle = item.Nombre ?? "",
                    cod_sec_interno_migrado = "",// item.Codigo_Interno_Ant;
                    prorroga_inical = prorroga_inical,
                    plazo_prorroga = plazo_prorroga,
                    observaciones = item.Observaciones ?? "",
                    NombreEmpresa = NombreEmp
                };
                ContAUX.Add(nuevoObj);
            }
            return ContAUX;
        }

        [HttpPost]
        public FileResult ExportExcel()//DateTime? FInicial1, DateTime? FInicial2)//string fi,string ff)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                //var ContratosLI = System.Web.HttpContext.Current.Session["contratosLK"] as List<ReporteContrato>;// db.MT_Contrato.Where(vq =>vq.Id_Empresa == empresa_id.ToString()).ToList();
                var ContratosLI = System.Web.HttpContext.Current.Session["contratosLK"] as List<sp_Quimipac_ConsultaMT_ContratoGeneral_Result>;// db.MT_Contrato.Where(vq =>vq.Id_Empresa == empresa_id.ToString()).ToList();
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);

                //DataTable dt = new DataTable("Registros");
                DataTable dt = new DataTable("Registro");
                dt.Columns.AddRange(new DataColumn[22] {
                                    new DataColumn("N°"),
                                    new DataColumn("Empresa"),
                                    new DataColumn("Cliente"),
                                    new DataColumn("Nom_cli"),
                                    new DataColumn("Unidad"),
                                    new DataColumn("Proyecto"),
                                    new DataColumn("Secuencial"),
                                    new DataColumn("Anio"),
                                    new DataColumn("Codigo_secuencial_interno"),
                                    new DataColumn("Codigo_secuencial_interno_padre"),
                                    new DataColumn("Codigo_contrato_asociado"),
                                    new DataColumn("Fecha_inicial"),
                                    new DataColumn("Plazo_contrato"),
                                    new DataColumn("Fecha_fin"),
                                    new DataColumn("Monto"),
                                    new DataColumn("Responsable"),
                                    new DataColumn("Costo"),
                                    new DataColumn("Detalle"),
                                    new DataColumn("Codigo_secuencial_interno_migrado"),
                                    new DataColumn("Prorroga_inicial"),
                                    new DataColumn("Plazo_prorroga"),
                                    new DataColumn("Observaciones")
                });

                int nro = 1;
                foreach (var item in ContratosLI)
                {
                    //string auxCodInterno = item.. .Codigo_Interno;
                    /*
                    string ci_emp = auxCodInterno.Length > 3 ? auxCodInterno.Substring(0,3):"";
                    string ci_cliente = auxCodInterno.Substring(3,3);
                    string nomb_cliente = string.Empty;
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        string sqlquery_ = " SELECT cli.* FROM MT_Contrato c Inner Join [SADATABASE]..[DBA].[clientes] cli on c.Id_Cliente = cli.cod_cli"
                                          +" Where cli.cia_codigo = '" + empresa_id.ToString()+"' and c.Id_Contrato="+item.Id_Contrato+" and c.Id_Cliente = "+ item.Id_Cliente;
                        var ncliente = ctx.Database.SqlQuery<sp_LINK_ConsultaClientes_Result>(sqlquery_).ToList();
                        nomb_cliente = ncliente.Count() > 0 ? ncliente .ElementAt(0).nom_cli:"";
                    }

                    string ci_unidad = auxCodInterno.Substring(6,3);
                    string ci_proy = auxCodInterno.Substring(9,3);
                    string ci_secuencia = auxCodInterno.Substring(12,3);
                    string ci_anio = auxCodInterno.Substring(15,4);

                    string cod_sec_interno = item.Codigo_Interno;
                    string cod_sec_interno_padre = item.Codigo_interno_padre;
                    string cod_contrato_asociado = item.Codigo_Cliente;//.Contrato_Padre != null ? item.Contrato_Padre.ToString():"";//?
                    string fecha_inicial = item.Fecha_Inicio !=  null ? item.Fecha_Inicio?.ToString("dd/MM/yyyy"):"";
                    string plazo_contrato = item.Dia_Plazo != null ? item.Dia_Plazo.ToString() : "";
                    string fecha_fin = item.Fecha_Fin != null ? item.Fecha_Fin?.ToString("dd/MM/yyyy"):"";
                    string monto = item.monto.ToString();
                    string responsable = string.Empty;// item.Responsable.ToString();

                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        string sqlquery = " select p.primer_nombre as Nombres_Completos,ISNULL(p.fecha_nacimiento,'')fecha_nacimiento,p.* FROM MT_Contrato c "
                                        + " INNER JOIN [SADATABASE]..[DBA].[rh_persona] p on c.Responsable = p.id_persona"
                                        + " WHERE c.Responsable = "+ item.Responsable+" and p.estado = '001' and c.Id_Contrato = "+item.Id_Contrato;
                        var Nrespons = ctx.Database.SqlQuery<sp_LINK_ConsultaPersonas_Result>(sqlquery).ToList();
                        responsable = Nrespons.Count() > 0 ? Nrespons.ElementAt(0).Nombres_Completos : "";
                    }

                    string costo = item.costo.ToString();
                    string detalle = item.Nombre;// item.Observaciones;
                    string cod_sec_interno_migrado = "";// item.Codigo_Interno_Ant;// "";

                    string prorroga_inical = string.Empty;// item.pro
                    string plazo_prorroga = string.Empty;
                    //var auxProrroga = db.MT_Contrato_Prorroga.Where(vq=>vq.);
                    var listProrroga  = db.MT_Contrato_Prorroga.Where(vq => vq.Id_Contrato == item.Id_Contrato).OrderBy(vq => vq.Fecha_Prorroga).Take(1).ToList();// item.pro;
                    if (listProrroga.Count()>0)
                    {
                        prorroga_inical = listProrroga.ElementAt(0).Fecha_Prorroga?.ToString("dd/MM/yyyy");
                        plazo_prorroga = listProrroga.ElementAt(0).Dia_Prorroga.ToString();
                    }
                    string observaciones = item.Observaciones;
                    */
                    dt.Rows.Add(nro++, item.Codigo_Interno.Length > 3 ? item.Codigo_Interno.Substring(0, 3) : "", item.abreviatura_cliente, item.nombreCliente, item.Id_Linea, item.Tipo_Contrato, item.Secuencial, item.Codigo_Interno.Substring(15, 4), item.Codigo_Interno, item.Codigo_interno_padre, item.Codigo_Cliente, item.Fecha_Inicio?.ToString("dd/MM/yyyy"), item.Dia_Plazo, item.Fecha_Fin?.ToString("dd/MM/yyyy"), item.monto, item.Nombres_Completos, item.costo, item.Nombre, item.Codigo_Interno_Ant, item.Fecha_proIni?.ToString("dd/MM/yyyy"), item.Dia_ProIn, item.Observaciones);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Style.Font.SetFontSize(10);
                    wb.Worksheets.Add(dt);
                    wb.Worksheets.ForEach(ws => ws.Columns().AdjustToContents());

                    //wb.Worksheets.Columns().AdjustToContents();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        /*var worksheet = wb.AddWorksheet("Test");
                        worksheet.Range("A1:V15")
                            .SetValue("Hello Hello Hello Hello Hello Hello Name!");//.Style.Border.SetOutsideBorder(XLBorderStyleValues.Dotted);
                        worksheet.Range("A1:V" + ContratosLI.Count() + "").Style
                        .Font.SetFontSize(10);//.Font.SetFontColor(XLColor.Gray).Font.SetItalic(true);
                        //worksheet.Style.Border.SetOutsideBorder(XLBorderStyleValues.DashDot);
                        //worksheet.Rows(2, 20).AdjustToContents();
                        worksheet.Cell(3, 2).Value = "Hello Hello Hello Hello Hello Hello Name";
                        worksheet.Cell(3, 2).Style.Alignment.WrapText = true;
                        //worksheet.Columns(3, 4).AdjustToContents();
                        worksheet.Columns().AdjustToContents();*/

                        wb.SaveAs(stream);

                        string stfi = System.Web.HttpContext.Current.Session["fini_contrato"].ToString();
                        string stff = System.Web.HttpContext.Current.Session["ffin_contrato"]?.ToString();
                        stfi = stfi.Replace("/", "");
                        stff = stff.Replace("/", "");

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Contrato " + stfi + "_" + stff + "" + ".xlsx");
                    }
                }
            }
            else
            {
                return null;// ("IniciarSesion", "Home");
            }
        }


        public JsonResult setDataContrato(List<sp_Quimipac_ConsultaMT_ContratoGeneral_Result> listContrato, DateTime fini_contrato, DateTime ffin_contrato)
        {
            var mensaje = new JsonResult();
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                System.Web.HttpContext.Current.Session["contratosLK"] = listContrato;
                System.Web.HttpContext.Current.Session["fini_contrato"] = fini_contrato.ToString("dd/MM/yyyy");
                System.Web.HttpContext.Current.Session["ffin_contrato"] = ffin_contrato.ToString("dd/MM/yyyy");
                try
                {

                    var lk = System.Web.HttpContext.Current.Session["contratosLK"];
                    mensaje.Data = "OK";
                }
                catch (Exception)
                {
                    mensaje.Data = "";
                    return Json(Response.StatusCode);
                }
            }
            else
            {
                mensaje.Data = "sessionCaducada";
            }
            return mensaje;
        }


        

    }

    public class ReporteContrato
    {
        public string nomb_cliente { get; set; }
        public string responsable { get; set; }
        public string prorroga_inical { get; set; }
        public string plazo_prorroga { get; set; }

        public string ci_emp { get; set; }
        public string ci_cliente { get; set; }
        public string ci_unidad { get; set; }
        public string ci_proy { get; set; }
        public string ci_secuencia { get; set; }

        public string ci_anio { get; set; }
        public string cod_sec_interno { get; set; }
        public string cod_sec_interno_padre { get; set; }
        public string cod_contrato_asociado { get; set; }
        public string fecha_inicial { get; set; }

        public string plazo_contrato { get; set; }
        public string fecha_fin { get; set; }
        public string monto { get; set; }
        public string costo { get; set; }
        public string detalle { get; set; }

        public string cod_sec_interno_migrado { get; set; }
        public string observaciones { get; set; }

        public string NombreEmpresa { get; set; }

    }



}