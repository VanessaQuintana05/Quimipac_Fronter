using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;
using Quimipac_.Repositorio;
using System.Net.Mail;

namespace Quimipac_.Controllers
{
    public class ContratoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        
        //CONTRATO General .
        #region
        [HttpGet]
        public ActionResult Contrato()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();


                    var ctx = new BD_QUIMIPACEntities();
                    var listaContrato = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ContratoGeneral_Result>("sp_Quimipac_ConsultaMT_ContratoGeneral @p0", empresacod).ToList();
                    
                    //var listaContrato = db.sp_Quimipac_ConsultaMT_ContratoGeneral(empresacod).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "CLIENTES").ToList();

                    listaContrato = listaContrato.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "CONTRATOS").ToList();
                    listaContrato = listaContrato.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                    ViewBag.listaContrato = listaContrato;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT =dbe.Item_OpcPermiso();                
                    

                    var listaEstadoTipoFilter = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                    ViewBag.listalkTipo = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                    ViewBag.listalkClienteC = new SelectList(dbe.ParametroLkClienteC("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");
                    ViewBag.listalkContratoPadre = new SelectList(dbe.ParametroLkContratoPadre("Contrato_Padre", empresa_id.ToString()), "Id_Contrato", "Nombre");
                    ViewBag.listalkEstadoC = new SelectList(dbe.ParametroLkEstadoC(empresa_id.ToString(), listaEstadoTipoFilter.ElementAt(0).Value), "Id_TablaDetalle", "Descripcion");
                    //ViewBag.listalkLinea = new SelectList(dbe.ParametroLkLinea("Unidad_Negocio", empresa_id.ToString()), "codigo", "descripcion");
                    //ViewBag.listalkCategoria = new SelectList(dbe.ParametroLkCategoria("Categoria", empresa_id.ToString()), "cod_servicio", "nombre");
                    //ViewBag.listakSubcategoria = new SelectList(dbe.ParametroLkSubcategoria("Subcategoria", empresa_id.ToString()), "codcen", "nombre");
                    ViewBag.listalkReferencia = new SelectList(dbe.ParametroLkReferencia("Referencia", empresa_id.ToString()), "Id_Contrato", "Nombre");
                    ViewBag.listalkLocalidad = new SelectList(dbe.ParametroLkLocalidad("Localidad", empresa_id.ToString()), "codigo_loc", "descripcion");
                    ViewBag.listakResponsable = new SelectList(dbe.ParametroLkResponsable("Responsable", empresa_id.ToString()), "id_persona", "Nombres_Completos");

                    return View();
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

        public ActionResult Agregar_Contrato(int? id_contrato_editar, int? ctr_pry, string cliente_edit, string linea_edit, string tipserv_edit, string centrocos_edit, string localidad_edit, string valor_ref_edit, string monto_edit, string costo_edit)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
           
                try
                {
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemsCliente = new List<SelectListItem>();
                    //List<SelectListItem> itemsLinea = new List<SelectListItem>();
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    List<SelectListItem> itemsContratoPadre = new List<SelectListItem>();
                    List<SelectListItem> itemsResponsable = new List<SelectListItem>();
                    //List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                    //List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                    List<SelectListItem> itemsLocalidad = new List<SelectListItem>();
                    List<SelectListItem> itemsReferenciaCtr = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    

                    var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle==144 || x.Id_TablaDetalle == 1152).ToList();
                    //itemsTipo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var tipo in listaTipo)
                    {
                        itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }


                    SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");
                    //?                   

                    var listaEstados = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa_id.ToString(), listaTipo.ElementAt(0).Id_TablaDetalle).ToList();

                    if (ctr_pry != null)
                    {
                        listaEstados = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa_id.ToString(), ctr_pry).ToList();
                    }

                    //var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                    //itemsEstado.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var estado in listaEstados)
                    {
                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion });
                    }


                    SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");


                    var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                    var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listacliente = listacliente.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();                    

                    //itemsCliente.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var cliente in listacliente)
                    {
                        itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                    }

                    SelectList selectlistacliente = new SelectList(itemsCliente, "Value", "Text");

                    string empresa = empresa_id.ToString();
                    int id_tipo = listaTipo.ElementAt(0).Id_TablaDetalle;
                    string cliente_id = listacliente.ElementAt(0).cod_cli;

                    var listaContratoPadre = db.sp_Quimipac_ConsultaMT_Contrato_TipoPadre(empresa, id_tipo, cliente_id).ToList();
                                       
                    //var listaContratoPadre = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();

                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaContratoPadre = listaContratoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                    listaContratoPadre = listaContratoPadre.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                    //itemsContratoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var contratoPadre in listaContratoPadre)
                    {
                        itemsContratoPadre.Add(new SelectListItem { Value = Convert.ToString(contratoPadre.Id_Contrato), Text = contratoPadre.Codigo_Interno + " | " + contratoPadre.Nombre });
                    }



                    SelectList selectlistaContratoPadre = new SelectList(itemsContratoPadre, "Value", "Text");


                    //var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
                    ////itemsLinea.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    //foreach (var linea in listaLinea)
                    //{
                    //    itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                    //}


                    //SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");

                    

                    var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var Persona in listaPersonas)
                    {
                        itemsResponsable.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos });
                    }

                    SelectList selectlistaPersonas = new SelectList(itemsResponsable, "Value", "Text");

                    //var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), listaLinea.ElementAt(0).codigo).ToList();
                    //itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                    //foreach (var categoria in listaCategoria)
                    //{
                    //    itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre });
                    //}


                    //SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                    //var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), listaLinea.ElementAt(0).codigo).ToList();
                    //itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    //foreach (var subcategoria in listaSubCategoria)
                    //{
                    //    itemsSubcategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre });
                    //}


                    //SelectList selectlistaSubCategoria = new SelectList(itemsSubcategoria, "Value", "Text");

                    var listaLocalidad = db.sp_LINK_Consulta_Localidad(empresa_id.ToString()).ToList();
                    itemsLocalidad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var localidad in listaLocalidad)
                    {
                        itemsLocalidad.Add(new SelectListItem { Value = Convert.ToString(localidad.codigo_loc), Text = localidad.descripcion });
                    }


                    SelectList selectlistaLocalidad = new SelectList(itemsLocalidad, "Value", "Text");

                    
                    var listaContratoRef = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).Where(vq=> vq.tipo == 145 || vq.tipo == 146 || vq.tipo == 1167).ToList();

                    var ClienteLI_R = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                    listaContratoRef = listaContratoRef.Where(vq => ClienteLI_R.Contains(vq.Id_Cliente)).ToList();
                    var ContratosLIRef = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                    listaContratoRef = listaContratoRef.Where(vq => ContratosLIRef.Contains(vq.Id_Prospecto.ToString())).ToList();

                    itemsReferenciaCtr.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var contratoRef in listaContratoRef)
                    {
                        itemsReferenciaCtr.Add(new SelectListItem { Value = Convert.ToString(contratoRef.Id_Prospecto), Text = contratoRef.Codigo_Interno + " | " + contratoRef.Nombre });
                    }



                    SelectList selectlistaContratoRef = new SelectList(itemsReferenciaCtr, "Value", "Text");


                    ViewBag.listaEstados = selectlistaEstado;
                    ViewBag.listacliente = selectlistacliente;
                    //ViewBag.listaLinea = selectlistaLinea;
                    ViewBag.listaTipo = selectlistaTipo;
                    ViewBag.listaContratoPadre = selectlistaContratoPadre;
                    ViewBag.listaPersonas = selectlistaPersonas;
                    //ViewBag.listaCategoria = selectlistaCategoria;
                    //ViewBag.listaSubcategoria = selectlistaSubCategoria;
                    ViewBag.listaLocalidad = selectlistaLocalidad;
                    ViewBag.listaContratoRef = selectlistaContratoRef;
                    ViewBag.id_contrato_edit = id_contrato_editar;
                    ViewBag.ctr_pry = ctr_pry;
                    ViewBag.cliente_edit = cliente_edit;
                    //ViewBag.linea_edit = linea_edit;
                    //ViewBag.tipserv_edit = tipserv_edit;
                    //ViewBag.centrocos_edit = centrocos_edit;
                    ViewBag.localidad_edit = localidad_edit;
                    ViewBag.valor_ref_edit = valor_ref_edit;
                    ViewBag.monto_edit = monto_edit;
                    ViewBag.costo_edit = costo_edit;


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
        public ActionResult Agregar_Contrato([Bind(Include = " Id_Cliente,Fecha_Inicio,Codigo_Interno, Codigo_Cliente,Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Fin,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst,Referencia")] MT_Contrato mT_Contrato) /*Id_Linea,Categoria,Subcategoria,*/
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (mT_Contrato.Monto_Aux != null) { 
                    string cadMonto = mT_Contrato.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_Contrato.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_Contrato.Valor_Referencial_Aux != null) { 
                    string cadValorReferencial = mT_Contrato.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_Contrato.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_Contrato.Costo_Aux != null) { 
                    string cadCosto = mT_Contrato.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_Contrato.costo = decimal.Parse(cadCosto);
                    }

                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        var contratos = db.MT_Contrato.Where(x => x.Codigo_Cliente == mT_Contrato.Codigo_Cliente && x.Nombre == mT_Contrato.Nombre  && x.Id_Cliente == mT_Contrato.Id_Cliente && x.Id_Empresa == empresa_id.ToString()).ToList(); /*&& x.Categoria == mT_Contrato.Categoria && x.Subcategoria == mT_Contrato.Subcategoria && x.Id_Linea == mT_Contrato.Id_Linea*/
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
                                int? secuencial = 0;
                                mT_Contrato.Id_Usuario = user_id.ToString();
                                

                                var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                                mT_Contrato.Id_Empresa = empresa_id.ToString();

                                /*ojooo verificar como se puede traer los clientes segun permiso*/
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == mT_Contrato.Id_Cliente).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == mT_Contrato.tipo).SingleOrDefault();
                                var codigo_guardado = db.MT_Contrato.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == mT_Contrato.Id_Cliente /*&& x.Id_Linea == mT_Contrato.Id_Linea*/ && x.tipo == mT_Contrato.tipo).ToList().Select(vq => new
                                {
                                    vq.Id_Contrato,
                                    Fecha_Inicio = (vq.Fecha_Inicio != null ? DateTime.Parse(vq.Fecha_Inicio.ToString()) : DateTime.Today),//(vq.Fecha_Fin != null) ?(DateTime.Parse(vq.Fecha_Fin?.ToString())):null
                                    vq.Codigo_Interno,
                                    vq.Secuencial
                                });


                                int? SeqRegistro = 0;
                                int seqregistro2 = 0;
                                codigo_guardado = codigo_guardado.Where(vq => vq.Fecha_Inicio.Year == DateTime.Now.Year).OrderByDescending(vq => vq.Fecha_Inicio).Take(1).ToList();
                                if (codigo_guardado.Count() > 0)
                                {
                                    //codigo_guardado = codigo_guardado.Where(vq => vq.Fecha_Inicio.Year == DateTime.Now.Year).ToList().OrderByDescending(vq => vq.Fecha_Inicio).Take(1);
                                    secuencial = (codigo_guardado.ElementAt(0).Secuencial);
                                    //SeqRegistro = codigo_guardado.Count() + 1;
                                    SeqRegistro = secuencial + 1;



                                    seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                                    //seqString = codigo_guardado.ElementAt(0).Codigo_Interno;
                                    //var aux = (seqString.Substring(13, 15));
                                    //SeqRegistro = int.Parse(aux);
                                }
                                else
                                {
                                    SeqRegistro = 1;
                                    seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                                    //Codigo cliente nuevo
                                }

                                string auxSeq = (seqregistro2.ToString()).PadLeft(3, '0');
                                //codigo_guardado = codigo_guardado.Where(vq=>vq.Fecha_Inicio.);



                                mT_Contrato.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + /*mT_Contrato.Id_Linea +*/ listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Contrato.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + /*mT_Contrato.Id_Linea +*/ listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                //mT_Contrato.Codigo_Interno = nom_empresa.Substring(0, 3) + mT_Contrato.Codigo_Cliente.Substring(0, 3) + listacliente.abreviatura_cliente + mT_Contrato.Id_Linea + listaTipo.Codigo;
                                mT_Contrato.Secuencial = seqregistro2;
                                if (mT_Contrato.Contrato_Padre != 0)
                                {
                                    var codigo_iterno_padre = db.MT_Contrato.Where(x => x.Id_Contrato == mT_Contrato.Contrato_Padre).ToList();
                                    mT_Contrato.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                }

                                //var lista_contrato = db.sp_LINK_ConsultaContratos(empresa_id.ToString()).ToList();
                                //foreach (var item in lista_contrato)
                                //{
                                //    var abreviatura_cliente = item.cliente;
                                //    var cod_clientes = db.sp_LINK_ConsultaClientes(item.cia_codigo).ToList().Where(x=> x.abreviatura_cliente == abreviatura_cliente);
                                //    string cod_cliente = Convert.ToString(cod_clientes.ElementAt(0).cod_cli);
                                //    var abreviatura_tipo = item.proyecto;
                                //    var cod_tipo = db.MT_TablaDetalle.Where(x => x.Codigo == abreviatura_tipo).FirstOrDefault();
                                //    var codestado = item.estado;
                                //    var id_estado = db.MT_TablaDetalle.Where(x => x.Codigo == codestado).FirstOrDefault();
                                //    //int id_tipo = cod_tipo.ElementAt(0).Id_TablaDetalle;
                                //    DataBase_Externo databaseOrden = new DataBase_Externo();
                                //    //databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaContrato.Id_Contrato, mT_ProrrogaContrato.Estado, mT_ProrrogaContrato.Dia_Prorroga, mT_ProrrogaContrato.Descripcion);
                                //    databaseOrden.InsertarContrato(item.cia_codigo, cod_cliente, item.fecha_inicial, item.fecha_fin, item.codigo_secuencial_interno_migrado, item.codigo_contrato_asociado, user_id.ToString(), item.unidad_migrada, item.cod_servicio, item.codcen, item.detalle, Convert.ToInt32(id_estado.Id_TablaDetalle), Convert.ToDecimal(item.plazo_contrato), Convert.ToInt32(cod_tipo.Id_TablaDetalle), Convert.ToDecimal(item.monto), Convert.ToDecimal(item.costo), item.secuencial, item.codigo_secuencial_interno_padre, item.observaciones, item.codigo_secuencial_interno);
                                //}

                                mT_Contrato.Fecha_registro = DateTime.Now;
                                db.MT_Contrato.Add(mT_Contrato);
                                db.SaveChanges();

                                var ProrrogaInicial = new MT_Contrato_Prorroga
                                {
                                    Id_Contrato = mT_Contrato.Id_Contrato,
                                    Descripcion = "Prorroga Inicial",
                                    Dia_Prorroga = mT_Contrato.Dia_Plazo,
                                    Fecha_Prorroga = mT_Contrato.Fecha_Fin,
                                    Estado = "A",
                                    Id_Contrato_Prorroga = 0
                                };
                                db.MT_Contrato_Prorroga.Add(ProrrogaInicial);
                                db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_Contrato.Id_Contrato, "A", mT_Contrato.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_Contrato.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato =mT_Contrato.Id_Contrato,// db.MT_Contrato.Where(x=>x.)
                                    Id_Permiso =0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();

                                var ProyectoContrato = new MT_Proyecto();

                                if (mT_Contrato.tipo == 1152 || mT_Contrato.tipo == 1165)
                                {

                                    ProyectoContrato = new MT_Proyecto
                                    {
                                        Id_Empresa = mT_Contrato.Id_Empresa,
                                        Id_Cliente = mT_Contrato.Id_Cliente,
                                        Fecha_Inicio = mT_Contrato.Fecha_Inicio,
                                        Fecha_Fin = mT_Contrato.Fecha_Fin,
                                        Codigo_Interno = mT_Contrato.Codigo_Interno,
                                        Codigo_Cliente = mT_Contrato.Codigo_Cliente,
                                        Linea = mT_Contrato.Id_Linea,
                                        Categoria = mT_Contrato.Categoria,
                                        Subcategoria = mT_Contrato.Subcategoria,
                                        Valor_Inicial = mT_Contrato.Valor_Referencial,
                                        Valor_Ajustado = mT_Contrato.monto,
                                        Comentario = mT_Contrato.Observaciones,
                                        Fecha_Registro = mT_Contrato.Fecha_registro,
                                        Id_contrato = mT_Contrato.Id_Contrato,
                                        Estado = "A"
                                };
                                db.MT_Proyecto.Add(ProyectoContrato);
                                db.SaveChanges();


                                    var obPermisoProyecto = new MT_Permiso
                                    {
                                        Aprobar = "S",
                                        Consultar = "S",
                                        Modificar = "S",
                                        Crear = "S",
                                        Eliminar = "N",
                                        Estado = "A",
                                        Fecha_Registro = DateTime.Now,
                                        Id_Cliente = mT_Contrato.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoContrato.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_Contrato.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();
                                }


                                

                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_ContratoParametro(mT_Contrato.Id_Contrato);
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
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult Editar_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);

                    mT_Contrato.Valor_Referencial_Aux = mT_Contrato.Valor_Referencial.ToString();
                    mT_Contrato.Costo_Aux = mT_Contrato.costo.ToString();
                    mT_Contrato.Monto_Aux = mT_Contrato.monto.ToString();


                    
                    System.Web.HttpContext.Current.Session["contrato_interno"] = mT_Contrato.Codigo_Interno;
                    System.Web.HttpContext.Current.Session["codigo_interno_anterior"] = mT_Contrato.Codigo_Interno_Ant;
                    System.Web.HttpContext.Current.Session["secuencial"] = mT_Contrato.Secuencial;
                    System.Web.HttpContext.Current.Session["fecha_registro_cont"] = mT_Contrato.Fecha_registro;
                    System.Web.HttpContext.Current.Session["id_cliente"] = mT_Contrato.Id_Cliente;
                    //System.Web.HttpContext.Current.Session["id_linea"] = mT_Contrato.Id_Linea;
                    System.Web.HttpContext.Current.Session["id_tipo"] = mT_Contrato.tipo;

                    //System.Web.HttpContext.Current.Session["tipo_servicio_editagg"] = mT_Contrato.Categoria;
                    //System.Web.HttpContext.Current.Session["centro_costo_editagg"] = mT_Contrato.Subcategoria;
                    System.Web.HttpContext.Current.Session["Localidad_editagg"] = mT_Contrato.Localidad;
                    //System.Web.HttpContext.Current.Session["valor_referencial_edit"] = mT_Contrato.Valor_Referencial;
                    //System.Web.HttpContext.Current.Session["valor_presupuestado_edit"] = mT_Contrato.monto;
                    //System.Web.HttpContext.Current.Session["valor_ofertado_edit"] = mT_Contrato.costo;


                    bool seleccion = false;
                    if (mT_Contrato != null)
                    {


                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente = new List<SelectListItem>();
                        //List<SelectListItem> itemsLinea = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoPadres = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoEsponsable = new List<SelectListItem>();
                        //List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                        //List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsLocalidad = new List<SelectListItem>();
                        List<SelectListItem> itemsReferenciaCtrs = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                        //var listaContratoPadres = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();

                        int id_tipo = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipo"].ToString());
                        string cliente_id = System.Web.HttpContext.Current.Session["id_cliente"].ToString();

                        var listaContratoPadres = db.sp_Quimipac_ConsultaMT_Contrato_TipoPadre(empresa_id.ToString(), id_tipo, cliente_id).ToList();


                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratoPadres = listaContratoPadres.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratoPadres = listaContratoPadres.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                        itemsContratoPadres.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        foreach (var contratoPadre in listaContratoPadres)
                        {

                            if (contratoPadre.Id_Contrato == mT_Contrato.Contrato_Padre)
                            {
                                seleccion = true;
                                itemsContratoPadres.Add(new SelectListItem { Value = Convert.ToString(contratoPadre.Id_Contrato), Text = contratoPadre.Codigo_Interno + " | " + contratoPadre.Nombre, Selected = seleccion });
                                break;
                            }


                            
                        }

                        SelectList selectlistaContratoPadres = new SelectList(itemsContratoPadres, "Value", "Text");



                        var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_Contrato.Estado)
                            {
                                seleccion = true;
                            }

                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");





                        var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listacliente = listacliente.Where(vq => ClienteLI2.Contains(vq.cod_cli)).ToList();
                        
                        //itemsCliente.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var cliente in listacliente)
                        {
                            if (cliente.cod_cli == mT_Contrato.Id_Cliente)
                            {
                                seleccion = true;
                            }

                            itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                        }

                        SelectList selectlistaCliente = new SelectList(itemsCliente, "Value", "Text");




                        //var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();

                        //foreach (var linea in listaLinea)
                        //{
                        //    if (linea.codigo == mT_Contrato.Id_Linea)
                        //    {
                        //        seleccion = true;
                        //    }

                        //    itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion, Selected = seleccion });
                        //}

                        //SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");


                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle==144 || x.Id_TablaDetalle==1152).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_Contrato.tipo)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();

                        foreach (var Persona in listaPersonas)
                        {
                            if (Persona.id_persona == mT_Contrato.Responsable)
                            {
                                seleccion = true;
                            }

                            itemsContratoEsponsable.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos, Selected = seleccion });
                        }

                        SelectList selectlistaPersonas = new SelectList(itemsContratoEsponsable, "Value", "Text");

                        //var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), mT_Contrato.Id_Linea).ToList();
                        ////itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        //foreach (var categoria in listaCategoria)
                        //{
                        //    if (categoria.cod_servicio == mT_Contrato.Categoria)
                        //    {
                        //        seleccion = true;
                        //    }

                        //    itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre, Selected = seleccion });
                        //}

                        //SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                        //var listaSubcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), mT_Contrato.Id_Linea).ToList();
                        ////itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        //foreach (var subcategoria in listaSubcategoria)
                        //{
                        //    if (subcategoria.codcen == mT_Contrato.Subcategoria)
                        //    {
                        //        seleccion = true;
                        //    }

                        //    itemsSubcategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre, Selected = seleccion });
                        //}

                        //SelectList selectlistaSubcategoria = new SelectList(itemsSubcategoria, "Value", "Text");

                        var listaLocalidad = db.sp_LINK_Consulta_Localidad(empresa_id.ToString()).ToList();
                        itemsLocalidad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var localidad in listaLocalidad)
                        {
                            if (localidad.codigo_loc == mT_Contrato.Localidad)
                            {
                                seleccion = true;
                            }

                            itemsLocalidad.Add(new SelectListItem { Value = Convert.ToString(localidad.codigo_loc), Text = localidad.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaLocalidad = new SelectList(itemsLocalidad, "Value", "Text");

                        string nombreUsuario = System.Web.HttpContext.Current.Session["nombres"] as string;
                        var nombre_fiscalizador = db.MT_Fiscalizador.Where(vq => vq.Id_Proyecto_Contrato == id && vq.Estado == "A").OrderByDescending(vq=>vq.Id_Fiscalizador).Take(1).ToList();
                        string nombre_Fiscalizador_Contr = "";
                        if (nombre_fiscalizador.Count > 0)
                        {
                            nombre_Fiscalizador_Contr = nombre_fiscalizador.ElementAt(0).Nombre;
                        }
                        else
                        {
                            nombre_Fiscalizador_Contr = "";
                        }

                        
                        var listaRefeCtr = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).Where(vq => vq.tipo == 145 || vq.tipo == 146 || vq.tipo == 1167).ToList();

                        var ClienteLI_R = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaRefeCtr = listaRefeCtr.Where(vq => ClienteLI_R.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLIRef = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                        listaRefeCtr = listaRefeCtr.Where(vq => ContratosLIRef.Contains(vq.Id_Prospecto.ToString())).ToList();




                        itemsReferenciaCtrs.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var refectr in listaRefeCtr)
                        {
                            if (refectr.Id_Prospecto == mT_Contrato.Referencia)
                            {
                                seleccion = true;
                            }
                            itemsReferenciaCtrs.Add(new SelectListItem { Value = Convert.ToString(refectr.Id_Prospecto), Text = refectr.Codigo_Interno + " | " + refectr.Nombre, Selected = seleccion });
                        }

                        SelectList selectlistaRefCtr = new SelectList(itemsReferenciaCtrs, "Value", "Text");

                        ViewBag.listaEstados = selectlistaEstado;
                        ViewBag.listacliente = selectlistaCliente;
                        //ViewBag.listaLinea = selectlistaLinea;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaContratoPadres = selectlistaContratoPadres;
                        ViewBag.listaPersonas = selectlistaPersonas;
                        //ViewBag.listaCategoria = selectlistaCategoria;
                        //ViewBag.listaSubcategoria = selectlistaSubcategoria;
                        ViewBag.id_contrato_Editar = id;
                        ViewBag.listaLocalidad = selectlistaLocalidad;
                        ViewBag.NombreUsuario = nombreUsuario;
                        ViewBag.NombreFiscalizador = nombre_Fiscalizador_Contr;
                        ViewBag.listaRefeCtr = selectlistaRefCtr;
                        ViewBag.Cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"];
                        //ViewBag.Linea_edit = System.Web.HttpContext.Current.Session["id_linea"];
                        //ViewBag.tiposervicio_edit = System.Web.HttpContext.Current.Session["tipo_servicio_editagg"];
                        //ViewBag.centrocosto_edit = System.Web.HttpContext.Current.Session["centro_costo_editagg"];
                        ViewBag.Localidad_edit = System.Web.HttpContext.Current.Session["Localidad_editagg"];
                        ViewBag.valor_ref_edit = mT_Contrato.Valor_Referencial;
                        ViewBag.costo_edit = mT_Contrato.costo;
                        ViewBag.monto_edit = mT_Contrato.monto;
                        var contrato_codedit = mT_Contrato.Codigo_Cliente;
                        if (contrato_codedit== null || contrato_codedit == "")
                        {
                            contrato_codedit = mT_Contrato.Nombre;
                        }
                        ViewBag.contrato_codedit = contrato_codedit;
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Contrato([Bind(Include = "Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente, Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst,Referencia")] MT_Contrato mT_Contrato) /*Id_Linea,Categoria,Subcategoria,*/
        {
            var mT_Contratoporroga = db.MT_Contrato_Prorroga.Where(x => x.Id_Contrato == mT_Contrato.Id_Contrato && x.Estado == "A");
            var codigointernocontrato = System.Web.HttpContext.Current.Session["contrato_interno"].ToString();
            var codigointerno_anterior = System.Web.HttpContext.Current.Session["codigo_interno_anterior"].ToString();
            int secuencial = Convert.ToInt32(System.Web.HttpContext.Current.Session["secuencial"].ToString());
            var fecha_registro = System.Web.HttpContext.Current.Session["fecha_registro_cont"];
            var id_cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"].ToString();
            //var id_linea_edit = System.Web.HttpContext.Current.Session["id_linea"].ToString();
            int id_tipo_edit = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipo"].ToString());
            

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (mT_Contrato.Monto_Aux != null) { 
                    string cadMonto = mT_Contrato.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_Contrato.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_Contrato.Valor_Referencial_Aux!=null) { 
                    string cadValorReferencial = mT_Contrato.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_Contrato.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_Contrato.Costo_Aux!=null) { 
                    string cadCosto = mT_Contrato.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_Contrato.costo = decimal.Parse(cadCosto);
                    }

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

                            var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            var id_cliente_nuevo = mT_Contrato.Id_Cliente;
                            //var id_linea_nuevo = mT_Contrato.Id_Linea;
                            var id_tipo_nuevo = mT_Contrato.tipo;
                            //var categoria_nueva = mT_Contrato.Categoria;
                            //var subcategoria_nueva = mT_Contrato.Subcategoria;

                            if (mT_Contrato.Id_Cliente == id_cliente_edit /*&& mT_Contrato.Id_Linea == id_linea_edit*/ && mT_Contrato.tipo == id_tipo_edit)
                            {
                                mT_Contrato.Id_Empresa = empresa_id.ToString();
                                mT_Contrato.Id_Usuario = user_id.ToString();
                                mT_Contrato.Secuencial = secuencial;
                                mT_Contrato.Codigo_Interno_Ant = codigointerno_anterior;

                                if (mT_Contrato.Contrato_Padre != 0)
                                {
                                    var codigo_iterno_padre = db.MT_Contrato.Where(x => x.Id_Contrato == mT_Contrato.Contrato_Padre).ToList();
                                    mT_Contrato.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                }
                                mT_Contrato.Fecha_registro = Convert.ToDateTime(fecha_registro);
                                mT_Contrato.Fecha_modificacion = DateTime.Now;
                                DataBase_Externo databaseOrden = new DataBase_Externo();
                                databaseOrden.UpdateContrato(mT_Contrato.Id_Contrato, mT_Contrato.Id_Cliente, mT_Contrato.Fecha_Inicio, mT_Contrato.Fecha_Fin, mT_Contrato.Codigo_Interno, mT_Contrato.Codigo_Cliente, /*mT_Contrato.Id_Linea, mT_Contrato.Categoria, mT_Contrato.Subcategoria,*/ mT_Contrato.Nombre, mT_Contrato.Estado, mT_Contrato.Dia_Plazo, mT_Contrato.tipo, mT_Contrato.Contrato_Padre, mT_Contrato.Valor_Referencial, mT_Contrato.monto, mT_Contrato.costo, mT_Contrato.Responsable, mT_Contrato.Secuencial, mT_Contrato.Codigo_Interno_Ant, mT_Contrato.Observaciones, mT_Contrato.Codigo_interno_padre, mT_Contrato.Fecha_registro, mT_Contrato.Fecha_modificacion, mT_Contrato.Localidad, mT_Contrato.Fecha_Aprobacion_Cot, mT_Contrato.Recepcion_Servicio, mT_Contrato.Fecha_Firma_Conformidad, mT_Contrato.Fecha_Cumplimiento_Inst, mT_Contrato.Referencia);
                                //Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones
                                TempData["mensaje_actualizado"] = "Contrato actualizado";
                            }
                            else
                            {

                                
                                MT_Contrato mt_contratoEditar = db.MT_Contrato.Find(mT_Contrato.Id_Contrato);
                                mt_contratoEditar.Estado = 1160;
                                db.Entry(mt_contratoEditar).State = EntityState.Modified;
                                db.SaveChanges();
                                int? secuencial_nuevo = 0;
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == id_cliente_nuevo).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == id_tipo_nuevo).SingleOrDefault();
                                var codigo_guardado = db.MT_Contrato.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == id_cliente_nuevo && /*x.Id_Linea == id_linea_nuevo &&*/ x.tipo == id_tipo_nuevo).ToList().Select(vq => new
                                {
                                    vq.Id_Contrato,
                                    Fecha_Inicio = (vq.Fecha_Inicio != null ? DateTime.Parse(vq.Fecha_Inicio.ToString()) : DateTime.Today),//(vq.Fecha_Fin != null) ?(DateTime.Parse(vq.Fecha_Fin?.ToString())):null
                                    vq.Codigo_Interno,
                                    vq.Secuencial
                                });


                                int? SeqRegistro = 0;
                                int seqregistro2 = 0;
                                codigo_guardado = codigo_guardado.Where(vq => vq.Fecha_Inicio.Year == DateTime.Now.Year).OrderByDescending(vq => vq.Fecha_Inicio).Take(1).ToList();
                                if (codigo_guardado.Count() > 0)
                                {
                                    secuencial_nuevo = (codigo_guardado.ElementAt(0).Secuencial);
                                    SeqRegistro = secuencial_nuevo + 1;



                                    seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                                }
                                else
                                {
                                    SeqRegistro = 1;
                                    seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                                }

                                string auxSeq = (seqregistro2.ToString()).PadLeft(3, '0');



                                mT_Contrato.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente /*+ id_linea_nuevo*/ + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Contrato.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente /*+ id_linea_nuevo*/ + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Contrato.Secuencial = seqregistro2;
                                if (mT_Contrato.Contrato_Padre != 0)
                                {
                                    var codigo_iterno_padre = db.MT_Contrato.Where(x => x.Id_Contrato == mT_Contrato.Contrato_Padre).ToList();
                                    mT_Contrato.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                }

                                
                                mT_Contrato.Fecha_registro = DateTime.Now;
                                mT_Contrato.Id_Empresa = empresa_id.ToString();
                                mT_Contrato.Id_Usuario = user_id.ToString();
                                mT_Contrato.Id_Cliente = id_cliente_nuevo;
                                //mT_Contrato.Id_Linea = id_linea_nuevo;
                                mT_Contrato.tipo = id_tipo_nuevo;
                                mT_Contrato.Estado = 75;
                                //mT_Contrato.Categoria = categoria_nueva;
                                //mT_Contrato.Subcategoria= subcategoria_nueva;
                                db.MT_Contrato.Add(mT_Contrato);
                                db.SaveChanges();

                                var ProrrogaInicial = new MT_Contrato_Prorroga
                                {
                                    Id_Contrato = mT_Contrato.Id_Contrato,
                                    Descripcion = "Prorroga Inicial",
                                    Dia_Prorroga = mT_Contrato.Dia_Plazo,
                                    Fecha_Prorroga = mT_Contrato.Fecha_Fin,
                                    Estado = "A",
                                    Id_Contrato_Prorroga = 0
                                };
                                db.MT_Contrato_Prorroga.Add(ProrrogaInicial);
                                db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_Contrato.Id_Contrato, "A", mT_Contrato.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso
                                {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_Contrato.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato = mT_Contrato.Id_Contrato,// db.MT_Contrato.Where(x=>x.)
                                    Id_Permiso = 0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();
                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_ContratoParametro(mT_Contrato.Id_Contrato);

                                var ProyectoContrato = new MT_Proyecto();
                                if (mT_Contrato.tipo == 1152 || mT_Contrato.tipo == 1165) { 
                                    ProyectoContrato = new MT_Proyecto
                                {
                                    Id_Empresa = mT_Contrato.Id_Empresa,
                                    Id_Cliente = mT_Contrato.Id_Cliente,
                                    Fecha_Inicio = mT_Contrato.Fecha_Inicio,
                                    Fecha_Fin = mT_Contrato.Fecha_Fin,
                                    Codigo_Interno = mT_Contrato.Codigo_Interno,
                                    Codigo_Cliente = mT_Contrato.Codigo_Cliente,
                                    Linea = mT_Contrato.Id_Linea,
                                    Categoria = mT_Contrato.Categoria,
                                    Subcategoria = mT_Contrato.Subcategoria,
                                    Valor_Inicial = mT_Contrato.Valor_Referencial,
                                    Valor_Ajustado = mT_Contrato.monto,
                                    Comentario = mT_Contrato.Observaciones,
                                    Fecha_Registro = mT_Contrato.Fecha_registro,
                                    Id_contrato = mT_Contrato.Id_Contrato
                                };
                                db.MT_Proyecto.Add(ProyectoContrato);
                                db.SaveChanges();

                                    var obPermisoProyecto = new MT_Permiso
                                    {
                                        Aprobar = "S",
                                        Consultar = "S",
                                        Modificar = "S",
                                        Crear = "S",
                                        Eliminar = "N",
                                        Estado = "A",
                                        Fecha_Registro = DateTime.Now,
                                        Id_Cliente = mT_Contrato.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoContrato.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_Contrato.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();

                                }

                                

                                TempData["mensaje_correcto"] = "Contrato anterior eliminado porque cambio el cliente, la linea o el tipo, Contrato nuevo guardado";

                            }

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EliminarContrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        var nreg = ctx.Database.SqlQuery<MT_Contrato>("select * from MT_Contrato c where c.Id_Contrato in(SELECT DISTINCT Contrato_Padre " + 
                    "FROM MT_Contrato) and Id_Contrato =" + id).ToList();
                        if (nreg.Count > 0)
                        {
                            TempData["mensaje_error"] = "Contrato no se puede eliminar porque este contrato tiene hijos";
                            return RedirectToAction("Contrato");
                        }
                        else {
                            mT_Contrato.Estado = 1160;//2156;

                            db.Entry(mT_Contrato).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Contrato eliminado";
                            return RedirectToAction("Contrato");
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

        public JsonResult GetContrato(string id)
        {
            try
            {
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                
                System.Web.HttpContext.Current.Session["id_Cliente"] = id;

                //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
                var listaContratoPadre = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).Where(x => x.Id_Cliente == id).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaContratoPadre = listaContratoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CONTRATOS").ToList();
                listaContratoPadre = listaContratoPadre.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                itemsContratos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var contratos in listaContratoPadre)
                {
                    itemsContratos.Add(new SelectListItem { Value = Convert.ToString(contratos.Id_Contrato), Text = contratos.Codigo_Interno + " | " + contratos.Nombre });
                }
                // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaContrato = new SelectList(itemsContratos, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaContrato, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //get editar contrato y default
        public JsonResult GetContratoDefauledit(string id_Cliente, int id_contrato_Editar)
        {
            try
            {
                List<SelectListItem> itemsContratos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                //2156 mentcol
                //var contratoPadre = db.MT_Contrato.Where(x => x.Id_Empresa == empresa && x.Estado != 2156 && x.Id_Cliente == id_Cliente).Select(u=> new { u.Id_Contrato, u.Id_Cliente, u.Nombre, u.Contrato_Padre}).ToList();
                var contratoPadre = db.MT_Contrato.Where(x => x.Id_Empresa == empresa && x.Estado != 1160 && x.Id_Cliente == id_Cliente).Select(u => new { u.Id_Contrato, u.Id_Cliente, u.Nombre, u.Contrato_Padre }).ToList();
                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                contratoPadre = contratoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CONTRATOS").ToList();
                contratoPadre = contratoPadre.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                return Json(contratoPadre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //

        public JsonResult GetContratoTipo(string id_cliente_Tipo, int id_contrato_Editar, int id_tipo_Tipo)
        {
    try
    {
        List<SelectListItem> itemsContratosTipo = new List<SelectListItem>();
        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();
        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
        var listaContratoPadreTipo = db.sp_Quimipac_ConsultaMT_Contrato_TipoPadre(empresa, id_tipo_Tipo, id_cliente_Tipo).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaContratoPadreTipo = listaContratoPadreTipo.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CONTRATOS").ToList();
                listaContratoPadreTipo = listaContratoPadreTipo.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();

                return Json(listaContratoPadreTipo, JsonRequestBehavior.AllowGet);
    }
    catch (Exception e)
    {
        return Json(Response.StatusCode);
    }
}

        

        public JsonResult GetCategoria(string id)
{
    try
    {
                id = id.Replace("ampersand","&");

        List<SelectListItem> itemsCategoria = new List<SelectListItem>();
        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();

        //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
        var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), id).ToList();

        itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        foreach (var categoria in listaCategoria)
        {
            itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre });
        }
        // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

        // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

        return Json(selectlistaCategoria, JsonRequestBehavior.AllowGet);
    }
    catch (Exception e)
    {
        return Json(Response.StatusCode);
    }
}

        //public JsonResult GetCategoria_Edit(string id, int id_contrato_Editar, string id_Linea)
        

        public JsonResult GetCategoria_Edit(int id_contrato_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Contrato mt_contrato = db.MT_Contrato.Find(id_contrato_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), id_Linea).ToList();
                var listaCateAux = new List<sp_LINK_ConsultaTipoServicio_Cat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                {
                    cia_codigo = empresa,
                    cod_linea = (mt_contrato.Categoria == "0" ?"OK":""),
                    cod_servicio = "0",
                    nombre = "Ninguno"
                });

                foreach (var categoria in listaCategoria)
                {
                    listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                    {
                        cia_codigo = empresa,
                        cod_linea = (categoria.cod_servicio == mt_contrato.Categoria)? "OK" : "",
                        cod_servicio = categoria.cod_servicio,
                        nombre = categoria.nombre
                    });
                   
                }

                return Json(listaCateAux, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AjaxEdirCategoria:" + e.Message);
                return Json(Response.StatusCode);
            }
        }
        public JsonResult GetSubCategoria(string id)
{
    try
    {
        id = id.Replace("ampersand","&");
        List<SelectListItem> itemsSubCategoria = new List<SelectListItem>();
        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();

        //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
        var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id).ToList();

        itemsSubCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        foreach (var subcategoria in listaSubCategoria)
        {
            itemsSubCategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre });
        }
        // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        SelectList selectlistaSubCategoria = new SelectList(itemsSubCategoria, "Value", "Text");

        // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

        return Json(selectlistaSubCategoria, JsonRequestBehavior.AllowGet);
    }
    catch (Exception e)
    {
        return Json(Response.StatusCode);
    }
}

        //public JsonResult GetSubCategoria_Edit(int id_contrato_Editar, string id_Linea)
        //{
        //    try
        //    {
        //        //id = id.Replace("ampersand", "&");
        //        id_Linea = id_Linea.Replace("ampersand", "&");


        //        bool seleccion = false;
        //        MT_Contrato mt_contrato = db.MT_Contrato.Find(id_contrato_Editar);

        //        List<SelectListItem> itemsSubCategoria_edit = new List<SelectListItem>();
        //        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        //        string empresa = empresa_id.ToString();

        //        //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
        //        var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id_Linea).ToList();

        //        itemsSubCategoria_edit.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        //        //foreach (var categoria in listaCategoria)
        //        //{
        //        //    itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre });
        //        //}

        //        foreach (var subcategoria in listaSubCategoria)
        //        {
        //            if (subcategoria.codcen == mt_contrato.Subcategoria)
        //            {
        //                seleccion = true;



        //            }
        //            else
        //            {
        //                seleccion = false;
        //            }

        //            itemsSubCategoria_edit.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre, Selected = seleccion });

        //        }

        //        // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
        //        SelectList selectlistaSubCategoria_edit = new SelectList(itemsSubCategoria_edit, "Value", "Text");

        //        // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

        //        return Json(selectlistaSubCategoria_edit, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(Response.StatusCode);
        //    }
        //}

        public JsonResult GetSubCategoria_Edit(int id_contrato_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Contrato mt_contrato = db.MT_Contrato.Find(id_contrato_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id_Linea).ToList();
                var listaSubCateAux = new List<sp_LINK_ConsultaCentroConsumo_SubCat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                {
                    cia_codigo = empresa,
                    quimi_linea = (mt_contrato.Subcategoria == "0" ? "OK" : ""),
                    codcen = "0",
                    nombre = "Ninguno"
                });

                foreach (var subcategoria in listaSubCategoria)
                {
                    listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                    {
                        cia_codigo = empresa,
                        quimi_linea = (subcategoria.codcen == mt_contrato.Subcategoria) ? "OK" : "",
                        codcen = subcategoria.codcen,
                        nombre = subcategoria.nombre
                    });

                }

                return Json(listaSubCateAux, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AjaxEdirCategoria:" + e.Message);
                return Json(Response.StatusCode);
            }
        }

        public JsonResult GetContratoPadreMonto(int id_ContratoPadre_Change, int id_contrato_Editar)
        {
            try
            {
                //List<SelectListItem> itemsContratosTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                
                var mensaje = new JsonResult();
                var lista_camp_ContratoPadre = db.MT_Contrato.Where(x=>x.Id_Contrato == id_ContratoPadre_Change).ToList().Select(vq => new
                {
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria,
                    vq.Valor_Referencial
                });
                var valr_ref_ = lista_camp_ContratoPadre.ElementAt(0).Valor_Referencial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");


                var listaCampos_CtrPadre = new List<MT_Contrato>();

                //cod_linea  lo utilizo com ocampo auxiliar
                var id_linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_ContratoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ContratoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();
                listaCampos_CtrPadre.Add(new MT_Contrato
                {
                    Id_Linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea,
                    Categoria = lista_camp_ContratoPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_ContratoPadre.ElementAt(0).Subcategoria,
                    Valor_Referencial_Aux = auxValor,
                    lineaContrato = nombre_linea.ToString(),
                    nombre_Categoria = nombre_categoria.ToString(),
                    nombre_Subcategoria = nombre_Subcategoria.ToString()
                });
                
                //mensaje.Data = auxValor;// Valor_Referencial_ContratoPadre;
                //return Json(mensaje, JsonRequestBehavior.AllowGet);

                return Json(listaCampos_CtrPadre, JsonRequestBehavior.AllowGet);


                //return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        public JsonResult GetProspectoPadreMonto(int id_ContratoPadre_Change, int id_contrato_Editar)
        {
            try
            {
                //List<SelectListItem> itemsContratosTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var mensaje = new JsonResult();
                var lista_camp_ContratoPadre = db.MT_Prospecto.Where(x => x.Id_Prospecto == id_ContratoPadre_Change).ToList().Select(vq => new
                {
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria,
                    //vq.Valor_Referencial
                });
                //var valr_ref_ = lista_camp_ContratoPadre.ElementAt(0).Valor_Referencial;
                //string auxValor = valr_ref_.ToString();
                //auxValor = auxValor.Replace(".", ",");


                var listaCampos_CtrPadre = new List<MT_Contrato>();

                //cod_linea  lo utilizo com ocampo auxiliar
                var id_linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_ContratoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ContratoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();
                listaCampos_CtrPadre.Add(new MT_Contrato
                {
                    Id_Linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea,
                    Categoria = lista_camp_ContratoPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_ContratoPadre.ElementAt(0).Subcategoria,
                    //Valor_Referencial_Aux = auxValor,
                    lineaContrato = nombre_linea.ToString(),
                    nombre_Categoria = nombre_categoria.ToString(),
                    nombre_Subcategoria = nombre_Subcategoria.ToString()
                });

                //mensaje.Data = auxValor;// Valor_Referencial_ContratoPadre;
                //return Json(mensaje, JsonRequestBehavior.AllowGet);

                return Json(listaCampos_CtrPadre, JsonRequestBehavior.AllowGet);


                //return mensaje;
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }




        #endregion

        //FORMULARIOS DE CONTRATO
        #region


        [HttpGet]
public ActionResult Formularios_Contrato(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
            if (mT_Contrato != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                var listaFormulariosC = db.sp_Quimipac_ConsultaMT_ContratoDocumentado(id, empresa_id.ToString()).ToList();
                var contrato = mT_Contrato.Codigo_Cliente;
                ViewBag.listaFormulariosC = listaFormulariosC;
                ViewBag.contrato = contrato;
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
public FileResult DescargarArchivoFormularioC(string nombre)
{
    // if (System.Web.HttpContext.Current.Session["usuario"] != null){
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
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
    else { return RedirectToAction("IniciarSesion", "Home"); }
}



[ValidateAntiForgeryToken]
[HttpPost]
public ActionResult AgregarFormulario_Contrato([Bind(Include = "Id_Contrato,Descripcion,Enlace,Fecha_Registro,Fecha_Validez,Estado,NombreArchivo,Tipo,Version")] MT_Contrato_Documentado mT_FormularioC, HttpPostedFileBase NombreArchivo)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
    else { return RedirectToAction("IniciarSesion", "Home"); }
}


[HttpGet]
public ActionResult EliminarFormularioContrato(int id)
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
                int idContrato = int.Parse(id_MTContrato.ToString());

                MT_Contrato_Documentado mT_Formulario = db.MT_Contrato_Documentado.Find(id);

                if (mT_Formulario != null)
                {
                    mT_Formulario.Estado = "E";
                    db.Entry(mT_Formulario).State = EntityState.Modified;
                    //db.MT_Formulario.Remove(mT_Formulario);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Archivo eliminado";
                    return RedirectToAction("Formularios_Contrato", new { id = idContrato });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Formularios_Contrato", new { id = idContrato });
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

        //ALERTA DE CONTRATO
        #region



        [HttpGet]
        public ActionResult Alerta_Contrato(int id)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                    if (mT_Contrato != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                        var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(119, empresa_id.ToString(), id).ToList();
                        var contrato = mT_Contrato.Codigo_Cliente;
                        ViewBag.listaNotificaciones = listaNotificaciones;
                        ViewBag.contrato = contrato;
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
                        if (tipo.Descripcion.Equals("Contrato"))
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

                        itemsFrecuencia.Add(new SelectListItem { Value = Convert.ToString(frecuencia.Id_TablaDetalle), Text = frecuencia.Codigo });

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
                    var id_Contrato = System.Web.HttpContext.Current.Session["id_Contrato"];
                    int idContrato = int.Parse(id_Contrato.ToString());
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
                            return RedirectToAction("Alerta_Contrato", new { id = idContrato });
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

                                var Id_noti_Nuevo = dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado, idContrato, Convert.ToInt32(Id_Notificacion), mT_Notificacion.Frecuencia, mT_Notificacion.EmpresaID, mT_Notificacion.Reenvio);
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
                                return RedirectToAction("Alerta_Contrato", new { id = idContrato });
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Alerta_Contrato", new { id = idContrato });
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

                        MT_Notificaciones mT_Alerta = db.MT_Notificaciones.Find(id);

                        if (mT_Alerta != null)
                        {
                            mT_Alerta.Estado = false;
                            db.Entry(mT_Alerta).State = EntityState.Modified;
                            //db.MT_ContratoAlerta.Remove(mT_Alerta);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Alerta eliminado";
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        #endregion

        //PARAMETRO CONTRATO
        #region
        /*
        [HttpGet]
        public ActionResult Parametro_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                    if (mT_Contrato != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                        var listaParametroC = db.sp_Quimipac_ConsultaMT_ContratoParametro(id, empresa_id.ToString()).ToList();
                        var contrato = mT_Contrato.Codigo_Cliente;
                        ViewBag.listaParametroC = listaParametroC;
                        ViewBag.contrato = contrato;
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
        public ActionResult AgregarParametro_Contrato()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(64).ToList();
                    foreach (var tipo in listaTipo)
                    {
                        itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }


                    SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                    ViewBag.listaTipo = selectlistaTipo;

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
        public ActionResult AgregarParametro_Contrato([Bind(Include = "Id_Contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Contrato_Parametro mT_ParametroContrato)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [HttpGet]
        public ActionResult EditarParametro_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato_Parametro mT_ContratoParametro = db.MT_Contrato_Parametro.Find(id);
                    bool seleccion = false;
                    if (mT_ContratoParametro != null)
                    {
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();

                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(64).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_ContratoParametro.Tipo_Dato)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        ViewBag.listaTipo = selectlistaTipo;

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
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditarParametro_Contrato([Bind(Include = "Id_Contrato_Parametro, Id_Contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Contrato_Parametro mT_ParametroContrato)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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

                            //var contratoParametro = db.MT_Contrato_Parametro.Where(x => x.Id_Contrato == mT_ParametroContrato.Id_Contrato && x.Parametro == mT_ParametroContrato.Parametro);
                            //if (contratoParametro.Count() >= 1)
                            //{


                            mT_ParametroContrato.Estado = mT_ParametroContrato.Estado.Substring(0, 1);


                            db.Entry(mT_ParametroContrato).State = EntityState.Modified;
                            db.SaveChanges();

                            //}

                            //else
                            //{
                            //    DataBase_Externo databaseOrden = new DataBase_Externo();
                            //    databaseOrden.InsertarParametroContratoEditNuevo(mT_ParametroContrato.Id_Contrato_Parametro, mT_ParametroContrato.Id_Contrato, mT_ParametroContrato.Parametro, mT_ParametroContrato.Descripcion, mT_ParametroContrato.Tipo_Dato, mT_ParametroContrato.Valor_Inicial, mT_ParametroContrato.Estado, mT_ParametroContrato.Valor_Final);
                            //}
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
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Errores");
                }
            }
            else { return RedirectToAction("IniciarSesion", "Home"); }
        }

        public ActionResult EliminarParametroContrato(int id)
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
                        int idContrato = int.Parse(id_MTContrato.ToString());

                        MT_Contrato_Parametro mT_parametro_ctr = db.MT_Contrato_Parametro.Find(id);

                        if (mT_parametro_ctr != null)
                        {
                            mT_parametro_ctr.Estado = "E";
                            db.Entry(mT_parametro_ctr).State = EntityState.Modified;
                            //db.MT_Formulario.Remove(mT_Formulario);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Parametro eliminado";
                            return RedirectToAction("Parametro_Contrato", new { id = idContrato });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Parametro_Contrato", new { id = idContrato });
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
        */
        #endregion 

        //Prorroga CONTRATO
        #region
        [HttpGet]
        public ActionResult Prorroga_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                    if (mT_Contrato != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                        var listaProrrogaCont = db.sp_Quimipac_ConsultaMT_ContratoProrroga(id, empresa_id.ToString()).ToList();
                        var contrato = mT_Contrato.Codigo_Cliente;
                        ViewBag.listaProrrogaCont = listaProrrogaCont;
                        ViewBag.contrato = contrato;
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
        public ActionResult AgregarProrroga_Contrato()
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
        public ActionResult AgregarProrroga_Contrato([Bind(Include = "Id_Contrato, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_Contrato_Prorroga mT_ProrrogaContrato)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                        var feha_prorroga = db.MT_Contrato_Prorroga.Where(x => x.Id_Contrato == idContrato && x.Estado == mT_ProrrogaContrato.Estado.Substring(0, 1)).ToList();
                        MT_Contrato mT_ContratoPro = db.MT_Contrato.Find(idContrato);
                        if (ModelState.IsValid)
                        {

                            var fecha_fin_Contrato = db.MT_Contrato.Where(x => x.Id_Contrato == idContrato).SingleOrDefault();

                            if (fecha_fin_Contrato.Fecha_Fin != null)
                            {

                                if (feha_prorroga.Count() == 0)
                                {
                                    mT_ProrrogaContrato.Fecha_Prorroga = (Convert.ToDateTime(fecha_fin_Contrato.Fecha_Fin)).AddDays(Convert.ToInt32(mT_ProrrogaContrato.Dia_Prorroga));
                                    mT_ProrrogaContrato.Id_Contrato = idContrato;
                                    mT_ProrrogaContrato.Estado = mT_ProrrogaContrato.Estado.Substring(0, 1);
                                    mT_ContratoPro.Fecha_Fin = mT_ProrrogaContrato.Fecha_Prorroga;
                                    db.MT_Contrato_Prorroga.Add(mT_ProrrogaContrato);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    mT_ProrrogaContrato.Id_Contrato = idContrato;
                                    mT_ProrrogaContrato.Estado = mT_ProrrogaContrato.Estado.Substring(0, 1);
                                    DataBase_Externo databaseOrden = new DataBase_Externo();
                                    databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaContrato.Id_Contrato, mT_ProrrogaContrato.Estado, mT_ProrrogaContrato.Dia_Prorroga, mT_ProrrogaContrato.Descripcion);
                                    //mT_ProrrogaContrato.Fecha_Prorroga = (Convert.ToDateTime(feha_prorroga)).AddDays(Convert.ToInt32(mT_ProrrogaContrato.Dia_Prorroga));

                                }

                                TempData["mensaje_correcto"] = "Contrato Prorroga guardado";
                                return RedirectToAction("Prorroga_Contrato", new { id = idContrato });

                            }
                            else
                            {
                                TempData["mensaje_error"] = "No hay fecha Fin, no se puede dar fecha prorroga";
                                return RedirectToAction("Prorroga_Contrato", new { id = idContrato });

                            }

                            //var contratoProrroga = db.MT_Contrato_Prorroga.Where(x => x.Id_Contrato == mT_ProrrogaContrato.Id_Contrato && x.Fecha_Prorroga == mT_ProrrogaContrato.Fecha_Prorroga && x.Estado == mT_ProrrogaContrato.Estado);
                            //if (contratoProrroga.Count() >= 1)
                            //{
                            //    TempData["mensaje_error"] = "Contrato Prorroga ya existe";
                            //    return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
                            //}
                            //else
                            //{


                            //}

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
                        }
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
        public ActionResult EditarProrroga_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato_Prorroga mT_ContratoProrroga = db.MT_Contrato_Prorroga.Find(id);
                    bool seleccion = false;
                    if (mT_ContratoProrroga != null)
                    {
                        return View(mT_ContratoProrroga);

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
                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
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
        public ActionResult EditarProrroga_Contrato([Bind(Include = "Id_Contrato_Prorroga, Id_Contrato, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_Contrato_Prorroga mT_ProrrogaContrato)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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


                            mT_ProrrogaContrato.Estado = mT_ProrrogaContrato.Estado.Substring(0, 1);


                            db.Entry(mT_ProrrogaContrato).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Contrato Prorroga actualizado";

                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
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

        public ActionResult EliminarProrrogaContrato(int id)
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
                        int idContrato = int.Parse(id_MTContrato.ToString());

                        MT_Contrato_Prorroga mT_Prorroga_elim = db.MT_Contrato_Prorroga.Find(id);

                        if (mT_Prorroga_elim != null)
                        {
                            mT_Prorroga_elim.Estado = "E";
                            db.Entry(mT_Prorroga_elim).State = EntityState.Modified;
                            db.SaveChanges();

                            var ultimafechainact = db.MT_Contrato_Prorroga.Where(vq => vq.Id_Contrato == idContrato && vq.Estado == "I").OrderByDescending(vq => vq.Fecha_Prorroga).Take(1).ToList();
                            MT_Contrato_Prorroga mT_Prorroga_habili = db.MT_Contrato_Prorroga.Find(ultimafechainact.ElementAt(0).Id_Contrato_Prorroga);
                            mT_Prorroga_habili.Estado = "A";                            
                            db.Entry(mT_Prorroga_habili).State = EntityState.Modified;
                            db.SaveChanges();


                            MT_Contrato mt_contrato_edit_ffin = db.MT_Contrato.Find(idContrato);
                            mt_contrato_edit_ffin.Fecha_Fin = ultimafechainact.ElementAt(0).Fecha_Prorroga;
                            db.Entry(mt_contrato_edit_ffin).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Prorroga eliminada";
                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Prorroga_Contrato", new { id = idContrato });
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

        //Fiscalizador CONTRATO
        #region
        [HttpGet]
        public ActionResult Fiscalizador(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                    if (mT_Contrato != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Contrato"] = mT_Contrato.Id_Contrato;
                        var listaFiscalizadorC = db.sp_Quimipac_ConsultaMT_ContratoFiscalizador(id, "Contrato", empresa_id.ToString()).ToList();
                        var contrato = mT_Contrato.Codigo_Cliente;
                        ViewBag.listaFiscalizadorC = listaFiscalizadorC;
                        ViewBag.contrato = contrato;
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
        public ActionResult AgregarFiscalizador_Contrato()
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
        public ActionResult AgregarFiscalizador_Contrato([Bind(Include = "Id_Proyecto_Contrato, Nombre, Estado")] MT_Fiscalizador mT_Fiscalizador)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
                            var contratoFiscalizador = db.MT_Fiscalizador.Where(x => x.Id_Proyecto_Contrato == mT_Fiscalizador.Id_Proyecto_Contrato && x.Nombre == mT_Fiscalizador.Nombre && x.Estado == mT_Fiscalizador.Estado);
                            if (contratoFiscalizador.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Contrato Fiscalizador ya existe";
                                return RedirectToAction("Fiscalizador", new { id = idContrato });
                            }
                            else
                            {
                                mT_Fiscalizador.Tipo = "Contrato";
                                mT_Fiscalizador.Id_Proyecto_Contrato = idContrato;
                                mT_Fiscalizador.Estado = mT_Fiscalizador.Estado.Substring(0, 1);
                                db.MT_Fiscalizador.Add(mT_Fiscalizador);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Fiscalizador de Contrato guardado";
                                return RedirectToAction("Fiscalizador", new { id = idContrato });

                            }

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idContrato });
                        }
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
        public ActionResult EditarFiscalizador_Contrato(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Fiscalizador mT_ContratoFiscalizador = db.MT_Fiscalizador.Find(id);
                    bool seleccion = false;
                    if (mT_ContratoFiscalizador != null)
                    {
                        return View(mT_ContratoFiscalizador);

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
                            return RedirectToAction("Fiscalizador_Contrato", new { id = idContrato });
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
        public ActionResult EditarFiscalizador_Contrato([Bind(Include = "Id_Fiscalizador, Id_Proyecto_Contrato, Nombre, Estado")] MT_Fiscalizador mT_FiscalizadorContrato)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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


                            mT_FiscalizadorContrato.Estado = mT_FiscalizadorContrato.Estado.Substring(0, 1);
                            mT_FiscalizadorContrato.Tipo = "Contrato";

                            db.Entry(mT_FiscalizadorContrato).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Fiscalizador de Contrato actualizado";

                            return RedirectToAction("Fiscalizador", new { id = idContrato });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idContrato });
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

        public ActionResult EliminarFiscalizadorContrato(int id)
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
                        int idContrato = int.Parse(id_MTContrato.ToString());

                        MT_Fiscalizador mT_Fiscalizador = db.MT_Fiscalizador.Find(id);

                        if (mT_Fiscalizador != null)
                        {
                            mT_Fiscalizador.Estado = "E";
                            db.Entry(mT_Fiscalizador).State = EntityState.Modified;
                            //db.MT_Formulario.Remove(mT_Formulario);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Fiscalizador eliminado";
                            return RedirectToAction("Fiscalizador", new { id = idContrato });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Fiscalizador", new { id = idContrato });
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

        [HttpPost]
        public ActionResult Insertar_ContratoSYSBASE(int ContratoID)
        {
            try
            {
                int id = ContratoID;
                MT_Contrato mT_Contrato = db.MT_Contrato.Find(id);
                DataBase_Externo databaseOrden = new DataBase_Externo();
                
                databaseOrden.InsertarContrato(mT_Contrato.Id_Contrato,mT_Contrato.Id_Empresa, mT_Contrato.Id_Cliente, mT_Contrato.Fecha_Inicio, mT_Contrato.Fecha_Fin, mT_Contrato.Codigo_Interno,
                mT_Contrato.Codigo_Cliente, mT_Contrato.Id_Usuario, mT_Contrato.Id_Linea, mT_Contrato.Categoria, mT_Contrato.Subcategoria, mT_Contrato.Nombre, mT_Contrato.Estado, mT_Contrato.Dia_Plazo, mT_Contrato.tipo, mT_Contrato.Contrato_Padre, mT_Contrato.Valor_Referencial,
                mT_Contrato.monto, mT_Contrato.costo, mT_Contrato.Responsable, mT_Contrato.Secuencial, mT_Contrato.Codigo_Interno_Ant, mT_Contrato.Observaciones, mT_Contrato.Codigo_interno_padre, mT_Contrato.Fecha_registro, mT_Contrato.Fecha_modificacion, mT_Contrato.Localidad, mT_Contrato.Fecha_Aprobacion_Cot, mT_Contrato.Recepcion_Servicio, mT_Contrato.Fecha_Firma_Conformidad, mT_Contrato.Fecha_Cumplimiento_Inst);


                TempData["mensaje_correcto"] = "Contrato guardado";
                return RedirectToAction("Contrato");

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }



        public JsonResult GetContratoTipo_Estado(int id_tipo_Tipo)
        {
            try
            {
                
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                
                var listaEstadoTipo = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa, id_tipo_Tipo).ToList();
                                              

                return Json(listaEstadoTipo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        public JsonResult GetContratoTipo_Estado_Edit(int id_contrato_editar, int id_tipo_Tipo)
        {
            try
            {
                List<SelectListItem> itemsEstado_edit = new List<SelectListItem>();
                bool seleccion = false;
                MT_Contrato mt_contrato = db.MT_Contrato.Find(id_contrato_editar);
                var nombre_estado_selected = db.MT_TablaDetalle.Find(mt_contrato.Estado).Descripcion;

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaEstadoTipo = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa, id_tipo_Tipo).ToList();

                //itemsEstado_edit.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                
                foreach (var estado in listaEstadoTipo)
                {
                    if (estado.Id_TablaDetalle == mt_contrato.Estado)
                    {
                        seleccion = true;
                        
                        //break;

                    }
                    else
                    {
                        seleccion = false;
                        

                    }
                    itemsEstado_edit.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });

                }
                int cont = 0;
                foreach (var item_selected in itemsEstado_edit)
                {
                    
                    if (item_selected.Selected == true)
                    {

                        cont = cont + 1;
                    }                    
                }
                if(mt_contrato.tipo == id_tipo_Tipo)
                { 
                if (cont ==0)
                {
                    seleccion = true;
                    var obj = new MT_TablaDetalle
                    {
                        Id_TablaDetalle = Convert.ToInt32(mt_contrato.Estado),
                        Descripcion = nombre_estado_selected

                    };
                    itemsEstado_edit.Add(new SelectListItem { Value = Convert.ToString(obj.Id_TablaDetalle), Text = obj.Descripcion, Selected = seleccion });

                }
                }


                SelectList selectlistaEstado_edit = new SelectList(itemsEstado_edit, "Value", "Text");

                


                return Json(selectlistaEstado_edit, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(Response.StatusCode);
            }
        }

        //Busqueda por parametros
        #region
        [HttpPost]
        public ActionResult Contrato_Verifar_funcion([Bind(Include = "Fecha_Inicio, Fecha_registro,Fecha_Fin, Origen,Tipo,Id_Cliente,Contrato_Padre,Estado,Id_Linea,Categoria,Subcategoria,Referencia, Localidad, Responsable")]sp_Quimipac_ConsultaMT_ContratoGeneral_Result CONTRATO)
        {
            try
            {
                var bde = new DataBase_Externo();
                var usuario = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var fecha_Inicio = CONTRATO.Fecha_Inicio?.ToString("yyyy-MM-dd");
                var fecha_Registro = CONTRATO.Fecha_registro?.ToString("yyyy-MM-dd");

                //var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion_grupo_trabajo, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_contrato, ORDEN.Estado, ORDEN.cod_cli, ORDEN.Etapa, ORDEN.Id_OrdenTrabajo);
                var lkbusqueda = bde.BusquedaXParametro_Contrato(fecha_Inicio, fecha_Registro, CONTRATO.Fecha_Fin, CONTRATO.Origen, CONTRATO.tipo, CONTRATO.Id_Cliente, CONTRATO.Contrato_Padre, CONTRATO.Estado, CONTRATO.Id_Linea, CONTRATO.Categoria, CONTRATO.Subcategoria, CONTRATO.Referencia, CONTRATO.Localidad, CONTRATO.Responsable);

                //var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                //var listakContrato = db.MT_Contrato.Where(vq => ContratosLI.Contains(vq.Id_Cliente)).ToList();
                ////listakContrato = listakContrato.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                ///

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CONTRATOS").ToList();
                lkbusqueda = lkbusqueda.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();


                TempData["listaFiltro"] = lkbusqueda;//"Tipo de Trabajo guardado";

                string XParametro = string.Empty;
                if (CONTRATO.Origen.Equals("Ninguno")) { XParametro = ""; }
                else if (CONTRATO.Origen.Equals("Tipo")) { XParametro = CONTRATO.tipo.ToString(); }
                else if (CONTRATO.Origen.Equals("Cliente")) { XParametro = CONTRATO.Id_Cliente; }
                else if (CONTRATO.Origen.Equals("Contrato_Padre")) { XParametro = CONTRATO.Contrato_Padre.ToString(); }
                else if (CONTRATO.Origen.Equals("Estado")) { XParametro = CONTRATO.Estado.ToString(); }
                else if (CONTRATO.Origen.Equals("Id_Linea")) { XParametro = CONTRATO.Id_Linea; }
                else if (CONTRATO.Origen.Equals("Categoria")) { XParametro = CONTRATO.Categoria; }
                else if (CONTRATO.Origen.Equals("Subcategoria")) { XParametro = CONTRATO.Subcategoria; }
                else if (CONTRATO.Origen.Equals("Referencia")) { XParametro = CONTRATO.Referencia.ToString(); }
                else if (CONTRATO.Origen.Equals("Localidad")) { XParametro = CONTRATO.Localidad; }
                else if (CONTRATO.Origen.Equals("Responsable")) { XParametro = CONTRATO.Responsable.ToString(); }

               
                TempData["ParametroBusqueda"] = CONTRATO.Fecha_Inicio + " " + CONTRATO.Fecha_registro + " " + CONTRATO.Origen + " " + XParametro;


                
                return RedirectToAction("Contrato");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpPost]
        public ActionResult Contrato_Filter(DateTime? Fecha_Inicio, DateTime? Fecha_registro, List<string> check_parametro, string SelectedAndOr, int? MenuSelectedFecha)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] == null )
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
                ViewBag.listalkReferencia = new SelectList(dbe.ParametroLkReferencia("Referencia", empresa_id.ToString()), "Id_Prospecto", "Nombre");
                ViewBag.listalkLocalidad = new SelectList(dbe.ParametroLkLocalidad("Localidad", empresa_id.ToString()), "codigo_loc", "descripcion");
                ViewBag.listakResponsable = new SelectList(dbe.ParametroLkResponsable("Responsable", empresa_id.ToString()), "id_persona", "Nombres_Completos");

                //return View();


                return RedirectToAction("Contrato");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

    }
}