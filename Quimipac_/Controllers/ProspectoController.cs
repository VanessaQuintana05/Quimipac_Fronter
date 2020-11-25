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
    public class ProspectoController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        
        //Prospecto General .
        #region
        [HttpGet]
        public ActionResult Prospecto()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();


                    var ctx = new BD_QUIMIPACEntities();
                    var listaProspecto = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ProspectoGeneral_Result>("sp_Quimipac_ConsultaMT_ProspectoGeneral @p0", empresacod).ToList();
                    
                    //var listaProspecto = db.sp_Quimipac_ConsultaMT_ProspectoGeneral(empresacod).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "CLIENTES").ToList();

                    listaProspecto = listaProspecto.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "PROSPECTOS").ToList();
                    listaProspecto = listaProspecto.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                    ViewBag.listaProspecto = listaProspecto;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT =dbe.Item_OpcPermiso();                
                    

                    var listaEstadoTipoFilter = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                    ViewBag.listalkTipo = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                    ViewBag.listalkClienteC = new SelectList(dbe.ParametroLkClienteC("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");
                    ViewBag.listalkEstadoC = new SelectList(dbe.ParametroLkEstadoC(empresa_id.ToString(), listaEstadoTipoFilter.ElementAt(0).Value), "Id_TablaDetalle", "Descripcion");
                    ViewBag.listalkLinea = new SelectList(dbe.ParametroLkLinea("Unidad_Negocio", empresa_id.ToString()), "codigo", "descripcion");
                    ViewBag.listalkCategoria = new SelectList(dbe.ParametroLkCategoria("Categoria", empresa_id.ToString()), "cod_servicio", "nombre");
                    ViewBag.listakSubcategoria = new SelectList(dbe.ParametroLkSubcategoria("Subcategoria", empresa_id.ToString()), "codcen", "nombre");
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

        public ActionResult Agregar_Prospecto(int? id_Prospecto_editar, int? ctr_pry, string cliente_edit, string linea_edit, string tipserv_edit, string centrocos_edit, string localidad_edit, string valor_ref_edit, string monto_edit, string costo_edit)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
           
                try
                {
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    List<SelectListItem> itemsCliente = new List<SelectListItem>();
                    List<SelectListItem> itemsLinea = new List<SelectListItem>();
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    List<SelectListItem> itemsResponsable = new List<SelectListItem>();
                    List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                    List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                    List<SelectListItem> itemsLocalidad = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    

                    var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle == 145 || x.Id_TablaDetalle==1167).ToList();
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

                    
                    var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
                    //itemsLinea.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var linea in listaLinea)
                    {
                        itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                    }


                    SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");

                    

                    var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var Persona in listaPersonas)
                    {
                        itemsResponsable.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos });
                    }

                    SelectList selectlistaPersonas = new SelectList(itemsResponsable, "Value", "Text");

                    var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), listaLinea.ElementAt(0).codigo).ToList();
                    itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                    foreach (var categoria in listaCategoria)
                    {
                        itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre });
                    }


                    SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                    var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), listaLinea.ElementAt(0).codigo).ToList();
                    itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var subcategoria in listaSubCategoria)
                    {
                        itemsSubcategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre });
                    }


                    SelectList selectlistaSubCategoria = new SelectList(itemsSubcategoria, "Value", "Text");

                    var listaLocalidad = db.sp_LINK_Consulta_Localidad(empresa_id.ToString()).ToList();
                    itemsLocalidad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var localidad in listaLocalidad)
                    {
                        itemsLocalidad.Add(new SelectListItem { Value = Convert.ToString(localidad.codigo_loc), Text = localidad.descripcion });
                    }


                    SelectList selectlistaLocalidad = new SelectList(itemsLocalidad, "Value", "Text");

                    
                    

                    ViewBag.listaEstados = selectlistaEstado;
                    ViewBag.listacliente = selectlistacliente;
                    ViewBag.listaLinea = selectlistaLinea;
                    ViewBag.listaTipo = selectlistaTipo;
                    ViewBag.listaPersonas = selectlistaPersonas;
                    ViewBag.listaCategoria = selectlistaCategoria;
                    ViewBag.listaSubcategoria = selectlistaSubCategoria;
                    ViewBag.listaLocalidad = selectlistaLocalidad;
                    ViewBag.id_Prospecto_edit = id_Prospecto_editar;
                    ViewBag.ctr_pry = ctr_pry;
                    ViewBag.cliente_edit = cliente_edit;
                    ViewBag.linea_edit = linea_edit;
                    ViewBag.tipserv_edit = tipserv_edit;
                    ViewBag.centrocos_edit = centrocos_edit;
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
        public ActionResult Agregar_Prospecto([Bind(Include = " Id_Cliente,Fecha_Inicio,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Fin,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst")] MT_Prospecto mT_Prospecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (mT_Prospecto.Monto_Aux != null) { 
                    string cadMonto = mT_Prospecto.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_Prospecto.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_Prospecto.Valor_Referencial_Aux != null) { 
                    string cadValorReferencial = mT_Prospecto.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_Prospecto.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_Prospecto.Costo_Aux != null) { 
                    string cadCosto = mT_Prospecto.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_Prospecto.costo = decimal.Parse(cadCosto);
                    }

                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        var prospectos = db.MT_Prospecto.Where(x => x.Codigo_Cliente == mT_Prospecto.Codigo_Cliente && x.Nombre == mT_Prospecto.Nombre && x.Categoria == mT_Prospecto.Categoria && x.Subcategoria == mT_Prospecto.Subcategoria && x.Id_Linea == mT_Prospecto.Id_Linea && x.Id_Cliente == mT_Prospecto.Id_Cliente && x.Id_Empresa == empresa_id.ToString()).ToList();
                        if (prospectos.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Prospecto ya existe";
                            return RedirectToAction("Prospecto");
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
                                mT_Prospecto.Id_Usuario = user_id.ToString();
                                //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                //if (mT_Prospecto.Dia_Plazo == null)
                                //{
                                //    mT_Prospecto.Fecha_Fin = (Convert.ToDateTime(mT_Prospecto.Fecha_Inicio)).AddDays(Convert.ToInt32(mT_Prospecto.Dia_Plazo));
                                //}
                                //else
                                //{
                                //}

                                var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                                mT_Prospecto.Id_Empresa = empresa_id.ToString();

                                /*ojooo verificar como se puede traer los clientes segun permiso*/
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == mT_Prospecto.Id_Cliente).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == mT_Prospecto.tipo).SingleOrDefault();
                                var codigo_guardado = db.MT_Prospecto.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == mT_Prospecto.Id_Cliente && x.Id_Linea == mT_Prospecto.Id_Linea && x.tipo == mT_Prospecto.tipo).ToList().Select(vq => new
                                {
                                    vq.Id_Prospecto,
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



                                mT_Prospecto.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + mT_Prospecto.Id_Linea + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Prospecto.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + mT_Prospecto.Id_Linea + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                //mT_Prospecto.Codigo_Interno = nom_empresa.Substring(0, 3) + mT_Prospecto.Codigo_Cliente.Substring(0, 3) + listacliente.abreviatura_cliente + mT_Prospecto.Id_Linea + listaTipo.Codigo;
                                mT_Prospecto.Secuencial = seqregistro2;
                                //if (mT_Prospecto.Prospecto_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_Prospecto.Where(x => x.Id_Prospecto == mT_Prospecto.Prospecto_Padre).ToList();
                                //    mT_Prospecto.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}

                                //var lista_Prospecto = db.sp_LINK_ConsultaProspectos(empresa_id.ToString()).ToList();
                                //foreach (var item in lista_prospecto)
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
                                //    //databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaProspecto.Id_Prospecto, mT_ProrrogaProspecto.Estado, mT_ProrrogaProspecto.Dia_Prorroga, mT_ProrrogaProspecto.Descripcion);
                                //    databaseOrden.InsertarProspecto(item.cia_codigo, cod_cliente, item.fecha_inicial, item.fecha_fin, item.codigo_secuencial_interno_migrado, item.codigo_Prospecto_asociado, user_id.ToString(), item.unidad_migrada, item.cod_servicio, item.codcen, item.detalle, Convert.ToInt32(id_estado.Id_TablaDetalle), Convert.ToDecimal(item.plazo_Prospecto), Convert.ToInt32(cod_tipo.Id_TablaDetalle), Convert.ToDecimal(item.monto), Convert.ToDecimal(item.costo), item.secuencial, item.codigo_secuencial_interno_padre, item.observaciones, item.codigo_secuencial_interno);
                                //}

                                mT_Prospecto.Fecha_registro = DateTime.Now;
                                db.MT_Prospecto.Add(mT_Prospecto);
                                db.SaveChanges();

                                //var ProrrogaInicial = new MT_Prospecto_Prorroga
                                //{
                                //    Id_Prospecto = mT_Prospecto.Id_Prospecto,
                                //    Descripcion = "Prorroga Inicial",
                                //    Dia_Prorroga = mT_Prospecto.Dia_Plazo,
                                //    Fecha_Prorroga = mT_Prospecto.Fecha_Fin,
                                //    Estado = "A",
                                //    Id_Prospecto_Prorroga = 0
                                //};
                                //db.MT_Prospecto_Prorroga.Add(ProrrogaInicial);
                                //db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_Prospecto.Id_Prospecto, "A", mT_Prospecto.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_Prospecto.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato =null,// db.MT_Prospecto.Where(x=>x.)
                                    Id_Prospecto = mT_Prospecto.Id_Prospecto,
                                    Id_Permiso =0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();
                                /*
                                var ProyectoProspecto = new MT_Proyecto();

                                if (mT_Prospecto.tipo == 1152 || mT_Prospecto.tipo == 1165)
                                {

                                    ProyectoProspecto = new MT_Proyecto
                                    {
                                        Id_Empresa = mT_Prospecto.Id_Empresa,
                                        Id_Cliente = mT_Prospecto.Id_Cliente,
                                        Fecha_Inicio = mT_Prospecto.Fecha_Inicio,
                                        Fecha_Fin = mT_Prospecto.Fecha_Fin,
                                        Codigo_Interno = mT_Prospecto.Codigo_Interno,
                                        Codigo_Cliente = mT_Prospecto.Codigo_Cliente,
                                        Linea = mT_Prospecto.Id_Linea,
                                        Categoria = mT_Prospecto.Categoria,
                                        Subcategoria = mT_Prospecto.Subcategoria,
                                        Valor_Inicial = mT_Prospecto.Valor_Referencial,
                                        Valor_Ajustado = mT_Prospecto.monto,
                                        Comentario = mT_Prospecto.Observaciones,
                                        Fecha_Registro = mT_Prospecto.Fecha_registro,
                                        Id_Prospecto = mT_Prospecto.Id_Prospecto,
                                        Estado = "A"
                                };
                                db.MT_Proyecto.Add(ProyectoProspecto);
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
                                        Id_Cliente = mT_Prospecto.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoProspecto.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_Prospecto.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();
                                }
                                */

                                

                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_ProspectoParametro(mT_Prospecto.Id_Prospecto);
                                TempData["mensaje_correcto"] = "Prospecto guardado";
                                return RedirectToAction("Prospecto");
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Prospecto");
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
        public ActionResult Editar_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);

                    mT_Prospecto.Valor_Referencial_Aux = mT_Prospecto.Valor_Referencial.ToString();
                    mT_Prospecto.Costo_Aux = mT_Prospecto.costo.ToString();
                    mT_Prospecto.Monto_Aux = mT_Prospecto.monto.ToString();


                    
                    System.Web.HttpContext.Current.Session["Prospecto_interno"] = mT_Prospecto.Codigo_Interno;
                    System.Web.HttpContext.Current.Session["codigo_interno_anterior"] = mT_Prospecto.Codigo_Interno_Ant;
                    System.Web.HttpContext.Current.Session["secuencial"] = mT_Prospecto.Secuencial;
                    System.Web.HttpContext.Current.Session["fecha_registro_cont"] = mT_Prospecto.Fecha_registro;
                    System.Web.HttpContext.Current.Session["id_cliente"] = mT_Prospecto.Id_Cliente;
                    System.Web.HttpContext.Current.Session["id_linea"] = mT_Prospecto.Id_Linea;
                    System.Web.HttpContext.Current.Session["id_tipo"] = mT_Prospecto.tipo;

                    System.Web.HttpContext.Current.Session["tipo_servicio_editagg"] = mT_Prospecto.Categoria;
                    System.Web.HttpContext.Current.Session["centro_costo_editagg"] = mT_Prospecto.Subcategoria;
                    System.Web.HttpContext.Current.Session["Localidad_editagg"] = mT_Prospecto.Localidad;
                    //System.Web.HttpContext.Current.Session["valor_referencial_edit"] = mT_Prospecto.Valor_Referencial;
                    //System.Web.HttpContext.Current.Session["valor_presupuestado_edit"] = mT_Prospecto.monto;
                    //System.Web.HttpContext.Current.Session["valor_ofertado_edit"] = mT_Prospecto.costo;


                    bool seleccion = false;
                    if (mT_Prospecto != null)
                    {


                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente = new List<SelectListItem>();
                        List<SelectListItem> itemsLinea = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        List<SelectListItem> itemsProspectoPadres = new List<SelectListItem>();
                        List<SelectListItem> itemsProspectoEsponsable = new List<SelectListItem>();
                        List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsLocalidad = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                        //var listaProspectoPadres = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).ToList();

                        int id_tipo = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipo"].ToString());
                        string cliente_id = System.Web.HttpContext.Current.Session["id_cliente"].ToString();

                        


                        var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_Prospecto.Estado)
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
                            if (cliente.cod_cli == mT_Prospecto.Id_Cliente)
                            {
                                seleccion = true;
                            }

                            itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                        }

                        SelectList selectlistaCliente = new SelectList(itemsCliente, "Value", "Text");




                        var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();

                        foreach (var linea in listaLinea)
                        {
                            if (linea.codigo == mT_Prospecto.Id_Linea)
                            {
                                seleccion = true;
                            }

                            itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");


                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle == 145 || x.Id_TablaDetalle == 1167).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_Prospecto.tipo)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();

                        foreach (var Persona in listaPersonas)
                        {
                            if (Persona.id_persona == mT_Prospecto.Responsable)
                            {
                                seleccion = true;
                            }

                            itemsProspectoEsponsable.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos, Selected = seleccion });
                        }

                        SelectList selectlistaPersonas = new SelectList(itemsProspectoEsponsable, "Value", "Text");

                        var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), mT_Prospecto.Id_Linea).ToList();
                        //itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        foreach (var categoria in listaCategoria)
                        {
                            if (categoria.cod_servicio == mT_Prospecto.Categoria)
                            {
                                seleccion = true;
                            }

                            itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                        var listaSubcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), mT_Prospecto.Id_Linea).ToList();
                        //itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var subcategoria in listaSubcategoria)
                        {
                            if (subcategoria.codcen == mT_Prospecto.Subcategoria)
                            {
                                seleccion = true;
                            }

                            itemsSubcategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaSubcategoria = new SelectList(itemsSubcategoria, "Value", "Text");

                        var listaLocalidad = db.sp_LINK_Consulta_Localidad(empresa_id.ToString()).ToList();
                        itemsLocalidad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var localidad in listaLocalidad)
                        {
                            if (localidad.codigo_loc == mT_Prospecto.Localidad)
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

                        
                        var listaRefeCtr = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).Where(vq => vq.tipo == 145 || vq.tipo == 146).ToList();

                        var ClienteLI_R = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        

                        ViewBag.listaEstados = selectlistaEstado;
                        ViewBag.listacliente = selectlistaCliente;
                        ViewBag.listaLinea = selectlistaLinea;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaPersonas = selectlistaPersonas;
                        ViewBag.listaCategoria = selectlistaCategoria;
                        ViewBag.listaSubcategoria = selectlistaSubcategoria;
                        ViewBag.id_prospecto_Editar = id;
                        ViewBag.listaLocalidad = selectlistaLocalidad;
                        ViewBag.NombreUsuario = nombreUsuario;
                        ViewBag.NombreFiscalizador = nombre_Fiscalizador_Contr;
                        ViewBag.Cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"];
                        ViewBag.Linea_edit = System.Web.HttpContext.Current.Session["id_linea"];
                        ViewBag.tiposervicio_edit = System.Web.HttpContext.Current.Session["tipo_servicio_editagg"];
                        ViewBag.centrocosto_edit = System.Web.HttpContext.Current.Session["centro_costo_editagg"];
                        ViewBag.Localidad_edit = System.Web.HttpContext.Current.Session["Localidad_editagg"];
                        ViewBag.valor_ref_edit = mT_Prospecto.Valor_Referencial;
                        ViewBag.costo_edit = mT_Prospecto.costo;
                        ViewBag.monto_edit = mT_Prospecto.monto;
                        var prospecto_codedit = mT_Prospecto.Codigo_Cliente;
                        if (prospecto_codedit== null || prospecto_codedit == "")
                        {
                            prospecto_codedit = mT_Prospecto.Nombre;
                        }
                        ViewBag.prospecto_codedit = prospecto_codedit;
                        return View(mT_Prospecto);

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Prospecto no existe";
                        return RedirectToAction("Prospecto");
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
        public ActionResult Editar_Prospecto([Bind(Include = "Id_Prospecto, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst")] MT_Prospecto mT_Prospecto)
        {
            //var mT_Prospectoporroga = db.MT_Prospecto_Prorroga.Where(x => x.Id_Prospecto == mT_Prospecto.Id_Prospecto && x.Estado == "A");
            var codigointernoprospecto = System.Web.HttpContext.Current.Session["prospecto_interno"].ToString();
            var codigointerno_anterior = System.Web.HttpContext.Current.Session["codigo_interno_anterior"].ToString();
            int secuencial = Convert.ToInt32(System.Web.HttpContext.Current.Session["secuencial"].ToString());
            var fecha_registro = System.Web.HttpContext.Current.Session["fecha_registro_cont"];
            var id_cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"].ToString();
            var id_linea_edit = System.Web.HttpContext.Current.Session["id_linea"].ToString();
            int id_tipo_edit = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipo"].ToString());
            

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (mT_Prospecto.Monto_Aux != null) { 
                    string cadMonto = mT_Prospecto.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_Prospecto.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_Prospecto.Valor_Referencial_Aux!=null) { 
                    string cadValorReferencial = mT_Prospecto.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_Prospecto.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_Prospecto.Costo_Aux!=null) { 
                    string cadCosto = mT_Prospecto.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_Prospecto.costo = decimal.Parse(cadCosto);
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

                            //if (mT_Prospecto.Fecha_Fin == null)//{
                            //    mT_Prospecto.Fecha_Fin = (Convert.ToDateTime(mT_Prospecto.Fecha_Inicio)).AddDays(Convert.ToInt32(mT_Prospecto.Dia_Plazo));
                            //}//else//{//    if (mT_Prospectoporroga == null)
                            //    {//        mT_Prospecto.Fecha_Fin = (Convert.ToDateTime(mT_Prospecto.Fecha_Fin)).AddDays(Convert.ToInt32(mT_Prospecto.Dia_Plazo));
                            //    }//    else //    {//}//}                            

                            var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            var id_cliente_nuevo = mT_Prospecto.Id_Cliente;
                            var id_linea_nuevo = mT_Prospecto.Id_Linea;
                            var id_tipo_nuevo = mT_Prospecto.tipo;
                            var categoria_nueva = mT_Prospecto.Categoria;
                            var subcategoria_nueva = mT_Prospecto.Subcategoria;

                            if (mT_Prospecto.Id_Cliente == id_cliente_edit && mT_Prospecto.Id_Linea == id_linea_edit && mT_Prospecto.tipo == id_tipo_edit)
                            {
                                mT_Prospecto.Id_Empresa = empresa_id.ToString();
                                mT_Prospecto.Id_Usuario = user_id.ToString();
                                mT_Prospecto.Secuencial = secuencial;
                                mT_Prospecto.Codigo_Interno_Ant = codigointerno_anterior;

                                //if (mT_Prospecto.Prospecto_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_Prospecto.Where(x => x.Id_Prospecto == mT_Prospecto.Prospecto_Padre).ToList();
                                //    mT_Prospecto.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}
                                mT_Prospecto.Fecha_registro = Convert.ToDateTime(fecha_registro);
                                mT_Prospecto.Fecha_modificacion = DateTime.Now;
                                DataBase_Externo databaseOrden = new DataBase_Externo();
                                databaseOrden.UpdateProspecto(mT_Prospecto.Id_Prospecto, mT_Prospecto.Id_Cliente, mT_Prospecto.Fecha_Inicio, mT_Prospecto.Fecha_Fin, mT_Prospecto.Codigo_Interno, mT_Prospecto.Codigo_Cliente, mT_Prospecto.Id_Linea, mT_Prospecto.Categoria, mT_Prospecto.Subcategoria, mT_Prospecto.Nombre, mT_Prospecto.Estado, mT_Prospecto.Dia_Plazo, mT_Prospecto.tipo, mT_Prospecto.Valor_Referencial, mT_Prospecto.monto, mT_Prospecto.costo, mT_Prospecto.Responsable, mT_Prospecto.Secuencial, mT_Prospecto.Codigo_Interno_Ant, mT_Prospecto.Observaciones, mT_Prospecto.Codigo_interno_padre, mT_Prospecto.Fecha_registro, mT_Prospecto.Fecha_modificacion, mT_Prospecto.Localidad, mT_Prospecto.Fecha_Aprobacion_Cot, mT_Prospecto.Recepcion_Servicio, mT_Prospecto.Fecha_Firma_Conformidad, mT_Prospecto.Fecha_Cumplimiento_Inst);
                                //Id_Prospecto, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Prospecto_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones
                                TempData["mensaje_actualizado"] = "Prospecto actualizado";
                            }
                            else
                            {

                                //mT_Prospecto.Id_Cliente = id_cliente_edit;
                                //mT_Prospecto.Id_Linea = id_linea_edit;
                                //mT_Prospecto.tipo = id_tipo_edit;
                                //mT_Prospecto.Categoria = categoria_edit;
                                //mT_Prospecto.Subcategoria = subcategoria_edit;
                                //mT_Prospecto.Id_Empresa = empresa_id.ToString();
                                //mT_Prospecto.Id_Usuario = user_id.ToString();
                                //mT_Prospecto.Secuencial = secuencial;
                                //mT_Prospecto.Codigo_Interno_Ant = codigointerno_anterior;                                

                                //if (mT_Prospecto.Prospecto_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_Prospecto.Where(x => x.Id_Prospecto == mT_Prospecto.Prospecto_Padre).ToList();
                                //    mT_Prospecto.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}
                                //mT_Prospecto.Fecha_registro = Convert.ToDateTime(fecha_registro);
                                //mT_Prospecto.Fecha_modificacion = DateTime.Now;
                                //mT_Prospecto.Estado = 1160;
                                MT_Prospecto mt_prospectoEditar = db.MT_Prospecto.Find(mT_Prospecto.Id_Prospecto);
                                mt_prospectoEditar.Estado = 1160;
                                db.Entry(mt_prospectoEditar).State = EntityState.Modified;
                                db.SaveChanges();
                                int? secuencial_nuevo = 0;
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == id_cliente_nuevo).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == id_tipo_nuevo).SingleOrDefault();
                                var codigo_guardado = db.MT_Prospecto.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == id_cliente_nuevo && x.Id_Linea == id_linea_nuevo && x.tipo == id_tipo_nuevo).ToList().Select(vq => new
                                {
                                    vq.Id_Prospecto,
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



                                mT_Prospecto.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + id_linea_nuevo + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Prospecto.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + id_linea_nuevo + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_Prospecto.Secuencial = seqregistro2;
                                //if (mT_Prospecto.Prospecto_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_Prospecto.Where(x => x.Id_Prospecto == mT_Prospecto.Prospecto_Padre).ToList();
                                //    mT_Prospecto.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}

                                
                                mT_Prospecto.Fecha_registro = DateTime.Now;
                                mT_Prospecto.Id_Empresa = empresa_id.ToString();
                                mT_Prospecto.Id_Usuario = user_id.ToString();
                                mT_Prospecto.Id_Cliente = id_cliente_nuevo;
                                mT_Prospecto.Id_Linea = id_linea_nuevo;
                                mT_Prospecto.tipo = id_tipo_nuevo;
                                mT_Prospecto.Estado = 75;
                                mT_Prospecto.Categoria = categoria_nueva;
                                mT_Prospecto.Subcategoria= subcategoria_nueva;
                                db.MT_Prospecto.Add(mT_Prospecto);
                                db.SaveChanges();

                                //var ProrrogaInicial = new MT_Prospecto_Prorroga
                                //{
                                //    Id_Prospecto = mT_Prospecto.Id_Prospecto,
                                //    Descripcion = "Prorroga Inicial",
                                //    Dia_Prorroga = mT_Prospecto.Dia_Plazo,
                                //    Fecha_Prorroga = mT_Prospecto.Fecha_Fin,
                                //    Estado = "A",
                                //    Id_Prospecto_Prorroga = 0
                                //};
                                //db.MT_Prospecto_Prorroga.Add(ProrrogaInicial);
                                //db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_Prospecto.Id_Prospecto, "A", mT_Prospecto.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso
                                {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_Prospecto.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato = null,// db.MT_Prospecto.Where(x=>x.)
                                    Id_Prospecto = mT_Prospecto.Id_Prospecto,
                                    Id_Permiso = 0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();
                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_ProspectoParametro(mT_Prospecto.Id_Prospecto);
                                /*
                                var ProyectoProspecto = new MT_Proyecto();
                                if (mT_Prospecto.tipo == 1152 || mT_Prospecto.tipo == 1165) { 
                                    ProyectoProspecto = new MT_Proyecto
                                {
                                    Id_Empresa = mT_Prospecto.Id_Empresa,
                                    Id_Cliente = mT_Prospecto.Id_Cliente,
                                    Fecha_Inicio = mT_Prospecto.Fecha_Inicio,
                                    Fecha_Fin = mT_Prospecto.Fecha_Fin,
                                    Codigo_Interno = mT_Prospecto.Codigo_Interno,
                                    Codigo_Cliente = mT_Prospecto.Codigo_Cliente,
                                    Linea = mT_Prospecto.Id_Linea,
                                    Categoria = mT_Prospecto.Categoria,
                                    Subcategoria = mT_Prospecto.Subcategoria,
                                    Valor_Inicial = mT_Prospecto.Valor_Referencial,
                                    Valor_Ajustado = mT_Prospecto.monto,
                                    Comentario = mT_Prospecto.Observaciones,
                                    Fecha_Registro = mT_Prospecto.Fecha_registro,
                                    Id_Prospecto = mT_Prospecto.Id_Prospecto
                                };
                                db.MT_Proyecto.Add(ProyectoProspecto);
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
                                        Id_Cliente = mT_Prospecto.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoProspecto.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_Prospecto.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();

                                }
                                */
                                

                                TempData["mensaje_correcto"] = "Prospecto anterior eliminado porque cambio el cliente, la linea o el tipo, Prospecto nuevo guardado";

                            }

                            return RedirectToAction("Prospecto");
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Prospecto");
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
        public ActionResult EliminarProspecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        var nreg = ctx.Database.SqlQuery<MT_Prospecto>("select * from MT_Prospecto c where c.Id_Prospecto in(SELECT DISTINCT Prospecto_Padre " + 
                    "FROM MT_Prospecto) and Id_Prospecto =" + id).ToList();
                        if (nreg.Count > 0)
                        {
                            TempData["mensaje_error"] = "Prospecto no se puede eliminar porque este Prospecto tiene hijos";
                            return RedirectToAction("Prospecto");
                        }
                        else {
                            mT_Prospecto.Estado = 1160;//2156;

                            db.Entry(mT_Prospecto).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Prospecto eliminado";
                            return RedirectToAction("Prospecto");
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

        public JsonResult GetProspecto(string id)
        {
            try
            {
                List<SelectListItem> itemsProspectos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                
                System.Web.HttpContext.Current.Session["id_Cliente"] = id;

                //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
                var listaProspectoPadre = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).Where(x => x.Id_Cliente == id).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaProspectoPadre = listaProspectoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROSPECTOS").ToList();
                listaProspectoPadre = listaProspectoPadre.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                itemsProspectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var Prospectos in listaProspectoPadre)
                {
                    //itemsProspectos.Add(new SelectListItem { Value = Convert.ToString(Prospectos.Id_Prospecto), Text = Prospectos.Codigo_Interno + " | " + prospectos.Nombre });
                }
                // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaProspecto = new SelectList(itemsProspectos, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaProspecto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //get editar Prospecto y default
        public JsonResult GetProspectoDefauledit(string id_Cliente, int id_prospecto_Editar)
        {
            try
            {
                List<SelectListItem> itemsProspectos = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                //2156 mentcol
                //var ProspectoPadre = db.MT_Prospecto.Where(x => x.Id_Empresa == empresa && x.Estado != 2156 && x.Id_Cliente == id_Cliente).Select(u=> new { u.Id_Prospecto, u.Id_Cliente, u.Nombre, u.Prospecto_Padre}).ToList();
                var prospectoPadre = db.MT_Prospecto.Where(x => x.Id_Empresa == empresa && x.Estado != 1160 && x.Id_Cliente == id_Cliente).Select(u => new { u.Id_Prospecto, u.Id_Cliente, u.Nombre }).ToList();
                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                prospectoPadre = prospectoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROSPECTOS").ToList();
                prospectoPadre = prospectoPadre.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                return Json(prospectoPadre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //

        public JsonResult GetProspectoTipo(string id_cliente_Tipo, int id_prospecto_Editar, int id_tipo_Tipo)
        {
    try
    {
        List<SelectListItem> itemsProspectosTipo = new List<SelectListItem>();
        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();
        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
        var listaProspectoPadreTipo = db.sp_Quimipac_ConsultaMT_Prospecto_TipoPadre(empresa, id_tipo_Tipo, id_cliente_Tipo).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaProspectoPadreTipo = listaProspectoPadreTipo.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROSPECTOS").ToList();
                listaProspectoPadreTipo = listaProspectoPadreTipo.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                return Json(listaProspectoPadreTipo, JsonRequestBehavior.AllowGet);
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

        //public JsonResult GetCategoria_Edit(string id, int id_Prospecto_Editar, string id_Linea)
        

        public JsonResult GetCategoria_Edit(int id_prospecto_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Prospecto mt_prospecto = db.MT_Prospecto.Find(id_prospecto_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), id_Linea).ToList();
                var listaCateAux = new List<sp_LINK_ConsultaTipoServicio_Cat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                {
                    cia_codigo = empresa,
                    cod_linea = (mt_prospecto.Categoria == "0" ?"OK":""),
                    cod_servicio = "0",
                    nombre = "Ninguno"
                });

                foreach (var categoria in listaCategoria)
                {
                    listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                    {
                        cia_codigo = empresa,
                        cod_linea = (categoria.cod_servicio == mt_prospecto.Categoria)? "OK" : "",
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

        //public JsonResult GetSubCategoria_Edit(int id_prospecto_Editar, string id_Linea)
        //{
        //    try
        //    {
        //        //id = id.Replace("ampersand", "&");
        //        id_Linea = id_Linea.Replace("ampersand", "&");


        //        bool seleccion = false;
        //        MT_Prospecto mt_prospecto = db.MT_Prospecto.Find(id_prospecto_Editar);

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
        //            if (subcategoria.codcen == mt_prospecto.Subcategoria)
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

        public JsonResult GetSubCategoria_Edit(int id_prospecto_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Prospecto mt_prospecto = db.MT_Prospecto.Find(id_prospecto_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id_Linea).ToList();
                var listaSubCateAux = new List<sp_LINK_ConsultaCentroConsumo_SubCat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                {
                    cia_codigo = empresa,
                    quimi_linea = (mt_prospecto.Subcategoria == "0" ? "OK" : ""),
                    codcen = "0",
                    nombre = "Ninguno"
                });

                foreach (var subcategoria in listaSubCategoria)
                {
                    listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                    {
                        cia_codigo = empresa,
                        quimi_linea = (subcategoria.codcen == mt_prospecto.Subcategoria) ? "OK" : "",
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

        public JsonResult GetProspectoPadreMonto(int id_ProspectoPadre_Change, int id_prospecto_Editar)
        {
            try
            {
                //List<SelectListItem> itemsProspectosTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                //string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                //var listaProspectoPadreTipo = db.sp_Quimipac_ConsultaMT_Prospecto_TipoPadre(empresa, id_tipo_Tipo, id_cliente_Tipo).ToList();

                //var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                //listaProspectoPadreTipo = listaProspectoPadreTipo.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                //var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PROSPECTOS").ToList();
                //listaProspectoPadreTipo = listaProspectoPadreTipo.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();
                var mensaje = new JsonResult();
                // Valor_Referencial_ProspectoPadre = db.MT_Prospecto.Where(x=>x.Id_Prospecto == id_ProspectoPadre_Change).SingleOrDefault().Valor_Referencial;
                var lista_camp_ProspectoPadre = db.MT_Prospecto.Where(x=>x.Id_Prospecto == id_ProspectoPadre_Change).ToList().Select(vq => new
                {
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria,
                    vq.Valor_Referencial
                });
                var valr_ref_ = lista_camp_ProspectoPadre.ElementAt(0).Valor_Referencial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");


                var listaCampos_CtrPadre = new List<MT_Prospecto>();

                //cod_linea  lo utilizo com ocampo auxiliar
                var id_linea = lista_camp_ProspectoPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_ProspectoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ProspectoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();
                listaCampos_CtrPadre.Add(new MT_Prospecto
                {
                    Id_Linea = lista_camp_ProspectoPadre.ElementAt(0).Id_Linea,
                    Categoria = lista_camp_ProspectoPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_ProspectoPadre.ElementAt(0).Subcategoria,
                    Valor_Referencial_Aux = auxValor,
                    lineaProspecto = nombre_linea.ToString(),
                    nombre_Categoria = nombre_categoria.ToString(),
                    nombre_Subcategoria = nombre_Subcategoria.ToString()
                });
                
                //mensaje.Data = auxValor;// Valor_Referencial_ProspectoPadre;
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

        //FORMULARIOS DE PROSPECTO
        #region


        [HttpGet]
public ActionResult Formularios_Prospecto(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
            if (mT_Prospecto != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_Prospecto"] = mT_Prospecto.Id_Prospecto;
                var listaFormulariosC = db.sp_Quimipac_ConsultaMT_ProspectoDocumentado(id, empresa_id.ToString()).ToList();
                var prospecto = mT_Prospecto.Codigo_Cliente;
                ViewBag.listaFormulariosC = listaFormulariosC;
                ViewBag.prospecto = prospecto;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("Prospecto");
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
public ActionResult AgregarFormulario_Prospecto()
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
public ActionResult AgregarFormulario_Prospecto([Bind(Include = "Id_Prospecto,Descripcion,Enlace,Fecha_Registro,Fecha_Validez,Estado,NombreArchivo,Tipo,Version")] MT_Prospecto_Documentado mT_FormularioC, HttpPostedFileBase NombreArchivo)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

            if (id_Prospecto == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                int idProspecto = int.Parse(id_Prospecto.ToString());

                if (ModelState.IsValid)
                {
                    var FormularioC = db.MT_Prospecto_Documentado.Where(x => x.Estado == mT_FormularioC.Estado && x.NombreArchivo == mT_FormularioC.NombreArchivo && x.Tipo == mT_FormularioC.Tipo);
                    if (FormularioC.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Formulario ya existe";
                        return RedirectToAction("Prospecto", new { id = idProspecto });
                    }
                    else
                    {
                        if (NombreArchivo != null)
                        {
                            var ruta_archivo = @"~/Formularios_Actividad/";
                            var ruta_servidor = Path.GetFullPath(ruta_archivo);
                            var extension = Path.GetExtension(NombreArchivo.FileName);

                            int fileSize = NombreArchivo.ContentLength;

                            mT_FormularioC.Id_Prospecto = idProspecto;
                            mT_FormularioC.NombreArchivo = "";
                            mT_FormularioC.Fecha_Registro = DateTime.Now;
                            mT_FormularioC.Estado = mT_FormularioC.Estado.Substring(0, 1);
                            mT_FormularioC.Enlace = ruta_servidor;
                            db.MT_Prospecto_Documentado.Add(mT_FormularioC);
                            db.SaveChanges();

                            int scope_identity_id = mT_FormularioC.Id_Prospecto_Documentado;

                            string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                            mT_FormularioC.NombreArchivo = nombreArchivo;

                            NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                            db.Entry(mT_FormularioC).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Formulario de Prospecto guardado";
                            return RedirectToAction("Formularios_Prospecto", new { id = idProspecto });
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
                    return RedirectToAction("Formularios_Prospecto", new { id = idProspecto });
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
public ActionResult EliminarFormularioProspecto(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {


            var id_MTProspecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

            if (id_MTProspecto == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idProspecto = int.Parse(id_MTProspecto.ToString());

                MT_Prospecto_Documentado mT_Formulario = db.MT_Prospecto_Documentado.Find(id);

                if (mT_Formulario != null)
                {
                    mT_Formulario.Estado = "E";
                    db.Entry(mT_Formulario).State = EntityState.Modified;
                    //db.MT_Formulario.Remove(mT_Formulario);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Archivo eliminado";
                    return RedirectToAction("Formularios_Prospecto", new { id = idProspecto });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Formularios_Prospecto", new { id = idProspecto });
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

        //ALERTA DE PROSPECTO
        #region

        [HttpGet]
        public ActionResult Alerta_Prospecto(int id)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                    if (mT_Prospecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Prospecto"] = mT_Prospecto.Id_Prospecto;
                        var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(119, empresa_id.ToString(), id).ToList();
                        var prospecto = mT_Prospecto.Codigo_Cliente;
                        ViewBag.listaNotificaciones = listaNotificaciones;
                        ViewBag.prospecto = prospecto;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Prospecto");
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
                        if (tipo.Descripcion.Equals("Prospecto"))
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
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];
                    int idProspecto = int.Parse(id_Prospecto.ToString());
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
                            return RedirectToAction("Alerta_Prospecto", new { id = idProspecto });
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

                                var Id_noti_Nuevo = dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado, idProspecto, Convert.ToInt32(Id_Notificacion), mT_Notificacion.Frecuencia, mT_Notificacion.EmpresaID, mT_Notificacion.Reenvio);
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
                                return RedirectToAction("Alerta_Prospecto", new { id = idProspecto });
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Alerta_Prospecto", new { id = idProspecto });
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


                    var id_MTProspecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_MTProspecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMTActividad = int.Parse(id_MTProspecto.ToString());

                        MT_Notificaciones mT_Alerta = db.MT_Notificaciones.Find(id);
                        
                        if (mT_Alerta != null)
                        {
                            mT_Alerta.Estado = false;
                            db.Entry(mT_Alerta).State = EntityState.Modified;
                            //db.MT_ProspectoAlerta.Remove(mT_Alerta);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Alerta eliminado";
                            return RedirectToAction("Alerta_Prospecto", new { id = id_MTProspecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Alerta_Prospecto", new { id = id_MTProspecto });
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

        //PARAMETRO PROSPECTO
        #region
        /*
        [HttpGet]
        public ActionResult Parametro_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                    if (mT_Prospecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Prospecto"] = mT_Prospecto.Id_Prospecto;
                        var listaParametroC = db.sp_Quimipac_ConsultaMT_ProspectoParametro(id, empresa_id.ToString()).ToList();
                        var prospecto = mT_Prospecto.Codigo_Cliente;
                        ViewBag.listaParametroC = listaParametroC;
                        ViewBag.prospecto = prospecto;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Prospecto");
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
        public ActionResult AgregarParametro_Prospecto()
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
        public ActionResult AgregarParametro_Prospecto([Bind(Include = "Id_Prospecto, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Prospecto_Parametro mT_ParametroProspecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var prospectoParametro = db.MT_Prospecto_Parametro.Where(x => x.Id_Prospecto == mT_ParametroProspecto.Id_Prospecto && x.Parametro == mT_ParametroProspecto.Parametro && x.Valor_Inicial == mT_ParametroProspecto.Valor_Inicial && x.Valor_Final == mT_ParametroProspecto.Valor_Final);
                            if (prospectoParametro.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Prospecto Parametro ya existe";
                                return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
                            }
                            else
                            {

                                mT_ParametroProspecto.Id_Prospecto = idProspecto;
                                mT_ParametroProspecto.Estado = mT_ParametroProspecto.Estado.Substring(0, 1);
                                db.MT_Prospecto_Parametro.Add(mT_ParametroProspecto);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Parametro de Prospecto guardado";
                                return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });

                            }

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
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
        public ActionResult EditarParametro_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto_Parametro mT_ProspectoParametro = db.MT_Prospecto_Parametro.Find(id);
                    bool seleccion = false;
                    if (mT_ProspectoParametro != null)
                    {
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();

                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(64).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_ProspectoParametro.Tipo_Dato)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        ViewBag.listaTipo = selectlistaTipo;

                        return View(mT_ProspectoParametro);

                    }
                    else
                    {

                        var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                        if (id_Prospecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProspecto = int.Parse(id_Prospecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
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
        public ActionResult EditarParametro_Prospecto([Bind(Include = "Id_Prospecto_Parametro, Id_Prospecto, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_Prospecto_Parametro mT_ParametroProspecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];
                    // int bandera = 0;
                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());

                        if (ModelState.IsValid)
                        {

                            //var prospectoParametro = db.MT_Prospecto_Parametro.Where(x => x.Id_Prospecto == mT_ParametroProspecto.Id_Prospecto && x.Parametro == mT_ParametroProspecto.Parametro);
                            //if (prospectoParametro.Count() >= 1)
                            //{


                            mT_ParametroProspecto.Estado = mT_ParametroProspecto.Estado.Substring(0, 1);


                            db.Entry(mT_ParametroProspecto).State = EntityState.Modified;
                            db.SaveChanges();

                            //}

                            //else
                            //{
                            //    DataBase_Externo databaseOrden = new DataBase_Externo();
                            //    databaseOrden.InsertarParametroProspectoEditNuevo(mT_ParametroProspecto.Id_Prospecto_Parametro, mT_ParametroProspecto.Id_Prospecto, mT_ParametroProspecto.Parametro, mT_ParametroProspecto.Descripcion, mT_ParametroProspecto.Tipo_Dato, mT_ParametroProspecto.Valor_Inicial, mT_ParametroProspecto.Estado, mT_ParametroProspecto.Valor_Final);
                            //}
                            TempData["mensaje_actualizado"] = "Parametro de Prospecto actualizado";

                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
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

        public ActionResult EliminarParametroProspecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {


                    var id_MTProspecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_MTProspecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_MTProspecto.ToString());

                        MT_Prospecto_Parametro mT_parametro_ctr = db.MT_Prospecto_Parametro.Find(id);

                        if (mT_parametro_ctr != null)
                        {
                            mT_parametro_ctr.Estado = "E";
                            db.Entry(mT_parametro_ctr).State = EntityState.Modified;
                            //db.MT_Formulario.Remove(mT_Formulario);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Parametro eliminado";
                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Parametro_Prospecto", new { id = idProspecto });
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

        //Prorroga PROSPECTO
        #region
        /*
         * [HttpGet]
        public ActionResult Prorroga_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                    if (mT_Prospecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Prospecto"] = mT_Prospecto.Id_Prospecto;
                        var listaProrrogaCont = db.sp_Quimipac_ConsultaMT_ProspectoProrroga(id, empresa_id.ToString()).ToList();
                        var prospecto = mT_Prospecto.Codigo_Cliente;
                        ViewBag.listaProrrogaCont = listaProrrogaCont;
                        ViewBag.prospecto = prospecto;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Prospecto");
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
        public ActionResult AgregarProrroga_Prospecto()
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
        public ActionResult AgregarProrroga_Prospecto([Bind(Include = "Id_Prospecto, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_Prospecto_Prorroga mT_ProrrogaProspecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());
                        var feha_prorroga = db.MT_Prospecto_Prorroga.Where(x => x.Id_Prospecto == idProspecto && x.Estado == mT_ProrrogaProspecto.Estado.Substring(0, 1)).ToList();
                        MT_Prospecto mT_ProspectoPro = db.MT_Prospecto.Find(idProspecto);
                        if (ModelState.IsValid)
                        {

                            var fecha_fin_Prospecto = db.MT_Prospecto.Where(x => x.Id_Prospecto == idProspecto).SingleOrDefault();

                            if (fecha_fin_Prospecto.Fecha_Fin != null)
                            {

                                if (feha_prorroga.Count() == 0)
                                {
                                    mT_ProrrogaProspecto.Fecha_Prorroga = (Convert.ToDateTime(fecha_fin_Prospecto.Fecha_Fin)).AddDays(Convert.ToInt32(mT_ProrrogaProspecto.Dia_Prorroga));
                                    mT_ProrrogaProspecto.Id_Prospecto = idProspecto;
                                    mT_ProrrogaProspecto.Estado = mT_ProrrogaProspecto.Estado.Substring(0, 1);
                                    mT_ProspectoPro.Fecha_Fin = mT_ProrrogaProspecto.Fecha_Prorroga;
                                    db.MT_Prospecto_Prorroga.Add(mT_ProrrogaProspecto);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    mT_ProrrogaProspecto.Id_Prospecto = idProspecto;
                                    mT_ProrrogaProspecto.Estado = mT_ProrrogaProspecto.Estado.Substring(0, 1);
                                    DataBase_Externo databaseOrden = new DataBase_Externo();
                                    databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaProspecto.Id_Prospecto, mT_ProrrogaProspecto.Estado, mT_ProrrogaProspecto.Dia_Prorroga, mT_ProrrogaProspecto.Descripcion);
                                    //mT_ProrrogaProspecto.Fecha_Prorroga = (Convert.ToDateTime(feha_prorroga)).AddDays(Convert.ToInt32(mT_ProrrogaProspecto.Dia_Prorroga));

                                }

                                TempData["mensaje_correcto"] = "Prospecto Prorroga guardado";
                                return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });

                            }
                            else
                            {
                                TempData["mensaje_error"] = "No hay fecha Fin, no se puede dar fecha prorroga";
                                return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });

                            }

                            //var prospectoProrroga = db.MT_Prospecto_Prorroga.Where(x => x.Id_Prospecto == mT_ProrrogaProspecto.Id_Prospecto && x.Fecha_Prorroga == mT_ProrrogaProspecto.Fecha_Prorroga && x.Estado == mT_ProrrogaProspecto.Estado);
                            //if (prospectoProrroga.Count() >= 1)
                            //{
                            //    TempData["mensaje_error"] = "Prospecto Prorroga ya existe";
                            //    return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
                            //}
                            //else
                            //{


                            //}

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
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
        public ActionResult EditarProrroga_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto_Prorroga mT_ProspectoProrroga = db.MT_Prospecto_Prorroga.Find(id);
                    bool seleccion = false;
                    if (mT_ProspectoProrroga != null)
                    {
                        return View(mT_ProspectoProrroga);

                    }
                    else
                    {

                        var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                        if (id_Prospecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProspecto = int.Parse(id_Prospecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
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
        public ActionResult EditarProrroga_Prospecto([Bind(Include = "Id_Prospecto_Prorroga, Id_Prospecto, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_Prospecto_Prorroga mT_ProrrogaProspecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];
                    // int bandera = 0;
                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_ProrrogaProspecto.Estado = mT_ProrrogaProspecto.Estado.Substring(0, 1);


                            db.Entry(mT_ProrrogaProspecto).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Prospecto Prorroga actualizado";

                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
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

        public ActionResult EliminarProrrogaProspecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {


                    var id_MTProspecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_MTProspecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_MTProspecto.ToString());

                        MT_Prospecto_Prorroga mT_Prorroga_elim = db.MT_Prospecto_Prorroga.Find(id);

                        if (mT_Prorroga_elim != null)
                        {
                            mT_Prorroga_elim.Estado = "E";
                            db.Entry(mT_Prorroga_elim).State = EntityState.Modified;
                            db.SaveChanges();

                            var ultimafechainact = db.MT_Prospecto_Prorroga.Where(vq => vq.Id_Prospecto == idProspecto && vq.Estado == "I").OrderByDescending(vq => vq.Fecha_Prorroga).Take(1).ToList();
                            MT_Prospecto_Prorroga mT_Prorroga_habili = db.MT_Prospecto_Prorroga.Find(ultimafechainact.ElementAt(0).Id_Prospecto_Prorroga);
                            mT_Prorroga_habili.Estado = "A";                            
                            db.Entry(mT_Prorroga_habili).State = EntityState.Modified;
                            db.SaveChanges();


                            MT_Prospecto mt_prospecto_edit_ffin = db.MT_Prospecto.Find(idProspecto);
                            mt_prospecto_edit_ffin.Fecha_Fin = ultimafechainact.ElementAt(0).Fecha_Prorroga;
                            db.Entry(mt_prospecto_edit_ffin).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Prorroga eliminada";
                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Prorroga_Prospecto", new { id = idProspecto });
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

        //Fiscalizador PROSPECTO
        #region
        [HttpGet]
        public ActionResult Fiscalizador(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                    if (mT_Prospecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Prospecto"] = mT_Prospecto.Id_Prospecto;
                        var listaFiscalizadorC = db.sp_Quimipac_ConsultaMT_ContratoFiscalizador(id, "Prospecto", empresa_id.ToString()).ToList();
                        var prospecto = mT_Prospecto.Codigo_Cliente;
                        ViewBag.listaFiscalizadorC = listaFiscalizadorC;
                        ViewBag.prospecto = prospecto;
                        var dbe = new DataBase_Externo();
                        ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Prospecto");
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
        public ActionResult AgregarFiscalizador_Prospecto()
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
        public ActionResult AgregarFiscalizador_Prospecto([Bind(Include = "Id_Proyecto_Prospecto, Nombre, Estado")] MT_Fiscalizador mT_Fiscalizador)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var prospectoFiscalizador = db.MT_Fiscalizador.Where(x => x.Id_Proyecto_Contrato == mT_Fiscalizador.Id_Proyecto_Contrato && x.Nombre == mT_Fiscalizador.Nombre && x.Estado == mT_Fiscalizador.Estado);
                            if (prospectoFiscalizador.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Prospecto Fiscalizador ya existe";
                                return RedirectToAction("Fiscalizador", new { id = idProspecto });
                            }
                            else
                            {
                                mT_Fiscalizador.Tipo = "Prospecto";
                                mT_Fiscalizador.Id_Proyecto_Contrato = idProspecto;
                                mT_Fiscalizador.Estado = mT_Fiscalizador.Estado.Substring(0, 1);
                                db.MT_Fiscalizador.Add(mT_Fiscalizador);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Fiscalizador de Prospecto guardado";
                                return RedirectToAction("Fiscalizador", new { id = idProspecto });

                            }

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idProspecto });
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
        public ActionResult EditarFiscalizador_Prospecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Fiscalizador mT_ProspectoFiscalizador = db.MT_Fiscalizador.Find(id);
                    bool seleccion = false;
                    if (mT_ProspectoFiscalizador != null)
                    {
                        return View(mT_ProspectoFiscalizador);

                    }
                    else
                    {

                        var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                        if (id_Prospecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProspecto = int.Parse(id_Prospecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Fiscalizador_Prospecto", new { id = idProspecto });
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
        public ActionResult EditarFiscalizador_Prospecto([Bind(Include = "Id_Fiscalizador, Id_Proyecto_Prospecto, Nombre, Estado")] MT_Fiscalizador mT_FiscalizadorProspecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Prospecto = System.Web.HttpContext.Current.Session["id_Prospecto"];
                    // int bandera = 0;
                    if (id_Prospecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_Prospecto.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_FiscalizadorProspecto.Estado = mT_FiscalizadorProspecto.Estado.Substring(0, 1);
                            mT_FiscalizadorProspecto.Tipo = "Prospecto";

                            db.Entry(mT_FiscalizadorProspecto).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Fiscalizador de Prospecto actualizado";

                            return RedirectToAction("Fiscalizador", new { id = idProspecto });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idProspecto });
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

        public ActionResult EliminarFiscalizadorProspecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {


                    var id_MTProspecto = System.Web.HttpContext.Current.Session["id_Prospecto"];

                    if (id_MTProspecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProspecto = int.Parse(id_MTProspecto.ToString());

                        MT_Fiscalizador mT_Fiscalizador = db.MT_Fiscalizador.Find(id);

                        if (mT_Fiscalizador != null)
                        {
                            mT_Fiscalizador.Estado = "E";
                            db.Entry(mT_Fiscalizador).State = EntityState.Modified;
                            //db.MT_Formulario.Remove(mT_Formulario);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Fiscalizador eliminado";
                            return RedirectToAction("Fiscalizador", new { id = idProspecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Fiscalizador", new { id = idProspecto });
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
        public ActionResult Insertar_ProspectoSYSBASE(int ProspectoID)
        {
            try
            {
                int id = ProspectoID;
                MT_Prospecto mT_Prospecto = db.MT_Prospecto.Find(id);
                DataBase_Externo databaseOrden = new DataBase_Externo();
                
                databaseOrden.InsertarProspecto(mT_Prospecto.Id_Prospecto,mT_Prospecto.Id_Empresa, mT_Prospecto.Id_Cliente, mT_Prospecto.Fecha_Inicio, mT_Prospecto.Fecha_Fin, mT_Prospecto.Codigo_Interno,
                mT_Prospecto.Codigo_Cliente, mT_Prospecto.Id_Usuario, mT_Prospecto.Id_Linea, mT_Prospecto.Categoria, mT_Prospecto.Subcategoria, mT_Prospecto.Nombre, mT_Prospecto.Estado, mT_Prospecto.Dia_Plazo, mT_Prospecto.tipo, mT_Prospecto.Valor_Referencial,
                mT_Prospecto.monto, mT_Prospecto.costo, mT_Prospecto.Responsable, mT_Prospecto.Secuencial, mT_Prospecto.Codigo_Interno_Ant, mT_Prospecto.Observaciones, mT_Prospecto.Codigo_interno_padre, mT_Prospecto.Fecha_registro, mT_Prospecto.Fecha_modificacion, mT_Prospecto.Localidad, mT_Prospecto.Fecha_Aprobacion_Cot, mT_Prospecto.Recepcion_Servicio, mT_Prospecto.Fecha_Firma_Conformidad, mT_Prospecto.Fecha_Cumplimiento_Inst);


                TempData["mensaje_correcto"] = "Prospecto guardado";
                return RedirectToAction("Prospecto");

            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }



        public JsonResult GetProspectoTipo_Estado(int id_tipo_Tipo)
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

        public JsonResult GetProspectoTipo_Estado_Edit(int id_prospecto_editar, int id_tipo_Tipo)
        {
            try
            {
                List<SelectListItem> itemsEstado_edit = new List<SelectListItem>();
                bool seleccion = false;
                MT_Prospecto mt_prospecto = db.MT_Prospecto.Find(id_prospecto_editar);
                var nombre_estado_selected = db.MT_TablaDetalle.Find(mt_prospecto.Estado).Descripcion;

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaEstadoTipo = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa, id_tipo_Tipo).ToList();

                //itemsEstado_edit.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                
                foreach (var estado in listaEstadoTipo)
                {
                    if (estado.Id_TablaDetalle == mt_prospecto.Estado)
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
                if(mt_prospecto.tipo == id_tipo_Tipo)
                { 
                if (cont ==0)
                {
                    seleccion = true;
                    var obj = new MT_TablaDetalle
                    {
                        Id_TablaDetalle = Convert.ToInt32(mt_prospecto.Estado),
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
        public ActionResult Prospecto_Verifar_funcion([Bind(Include = "Fecha_Inicio, Fecha_registro,Fecha_Fin, Origen,Tipo,Id_Cliente,Prospecto_Padre,Estado,Id_Linea,Categoria,Subcategoria, Localidad, Responsable")]sp_Quimipac_ConsultaMT_ProspectoGeneral_Result PROSPECTO)
        {
            try
            {
                var bde = new DataBase_Externo();
                var usuario = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var fecha_Inicio = PROSPECTO.Fecha_Inicio?.ToString("yyyy-MM-dd");
                var fecha_Registro = PROSPECTO.Fecha_registro?.ToString("yyyy-MM-dd");

                //var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion_grupo_trabajo, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_prospecto, ORDEN.Estado, ORDEN.cod_cli, ORDEN.Etapa, ORDEN.Id_OrdenTrabajo);
                var lkbusqueda = bde.BusquedaXParametro_Prospecto(fecha_Inicio, fecha_Registro, PROSPECTO.Fecha_Fin, PROSPECTO.Origen, PROSPECTO.tipo, PROSPECTO.Id_Cliente,  PROSPECTO.Estado, PROSPECTO.Id_Linea, PROSPECTO.Categoria, PROSPECTO.Subcategoria,  PROSPECTO.Localidad, PROSPECTO.Responsable);

                //var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "PROSPECTOS").ToList();
                //var listakProspecto = db.MT_Prospecto.Where(vq => ProspectosLI.Contains(vq.Id_Cliente)).ToList();
                ////listakProspecto = listakProspecto.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();
                ///

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "PROSPECTOS").ToList();
                lkbusqueda = lkbusqueda.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();


                TempData["listaFiltro"] = lkbusqueda;//"Tipo de Trabajo guardado";

                string XParametro = string.Empty;
                if (PROSPECTO.Origen.Equals("Ninguno")) { XParametro = ""; }
                else if (PROSPECTO.Origen.Equals("Tipo")) { XParametro = PROSPECTO.tipo.ToString(); }
                else if (PROSPECTO.Origen.Equals("Cliente")) { XParametro = PROSPECTO.Id_Cliente; }
                else if (PROSPECTO.Origen.Equals("Estado")) { XParametro = PROSPECTO.Estado.ToString(); }
                else if (PROSPECTO.Origen.Equals("Id_Linea")) { XParametro = PROSPECTO.Id_Linea; }
                else if (PROSPECTO.Origen.Equals("Categoria")) { XParametro = PROSPECTO.Categoria; }
                else if (PROSPECTO.Origen.Equals("Subcategoria")) { XParametro = PROSPECTO.Subcategoria; }
                else if (PROSPECTO.Origen.Equals("Localidad")) { XParametro = PROSPECTO.Localidad; }
                else if (PROSPECTO.Origen.Equals("Responsable")) { XParametro = PROSPECTO.Responsable.ToString(); }

               
                TempData["ParametroBusqueda"] = PROSPECTO.Fecha_Inicio + " " + PROSPECTO.Fecha_registro + " " + PROSPECTO.Origen + " " + XParametro;


                
                return RedirectToAction("Prospecto");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpPost]
        public ActionResult Prospecto_Filter(DateTime? Fecha_Inicio, DateTime? Fecha_registro, List<string> check_parametro, string SelectedAndOr, int? MenuSelectedFecha)
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

                var lkbusqueda = bde.execute_filter_prospecto(Fecha_Inicio, Fecha_registro, check_parametro, SelectedAndOr, MenuSelectedFecha);

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var ProspectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "PROSPECTOS").ToList();
                lkbusqueda = lkbusqueda.Where(vq => ProspectosLI.Contains(vq.Id_Prospecto.ToString())).ToList();

                //DATA GET PROSPECTO
                //ViewBag.listaProspecto = lkbusqueda;
                TempData["listaFiltro"] = lkbusqueda;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();

                ViewBag.listalkTipo = new SelectList(dbe.ParametroLkTipo("Tipo", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                ViewBag.listalkClienteC = new SelectList(dbe.ParametroLkClienteC("Cliente", empresa_id.ToString()), "cod_cli", "nom_cli");
                ViewBag.listalkEstadoC = new SelectList(dbe.ParametroLkEstadoC("Estado", empresa_id.ToString()), "Id_TablaDetalle", "Descripcion");
                ViewBag.listalkLinea = new SelectList(dbe.ParametroLkLinea("Unidad_Negocio", empresa_id.ToString()), "codigo", "descripcion");
                ViewBag.listalkCategoria = new SelectList(dbe.ParametroLkCategoria("Categoria", empresa_id.ToString()), "cod_servicio", "nombre");
                ViewBag.listakSubcategoria = new SelectList(dbe.ParametroLkSubcategoria("Subcategoria", empresa_id.ToString()), "codcen", "nombre");
                ViewBag.listalkLocalidad = new SelectList(dbe.ParametroLkLocalidad("Localidad", empresa_id.ToString()), "codigo_loc", "descripcion");
                ViewBag.listakResponsable = new SelectList(dbe.ParametroLkResponsable("Responsable", empresa_id.ToString()), "id_persona", "Nombres_Completos");

                //return View();


                return RedirectToAction("Prospecto");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

    }
}