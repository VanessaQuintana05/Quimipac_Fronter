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
    public class PostVentaController : Controller
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        
        //PostVenta General .
        #region
        [HttpGet]
        public ActionResult PostVenta()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresacod = empresa_id.ToString();


                    var ctx = new BD_QUIMIPACEntities();
                    var listaPostVenta = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_PostVentaGeneral_Result>("sp_Quimipac_ConsultaMT_PostVentaGeneral @p0", empresacod).ToList();
                    
                    //var listaPostVenta = db.sp_Quimipac_ConsultaMT_PostVentaGeneral(empresacod).ToList();
                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "CLIENTES").ToList();

                    listaPostVenta = listaPostVenta.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresacod, "POSTVENTA").ToList();
                    listaPostVenta = listaPostVenta.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();

                    ViewBag.listaPostVenta = listaPostVenta;
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

        public ActionResult Agregar_PostVenta(int? id_PostVenta_editar, int? ctr_pry, string cliente_edit, string linea_edit, string tipserv_edit, string centrocos_edit, string localidad_edit, string valor_ref_edit, string monto_edit, string costo_edit)
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
                    List<SelectListItem> itemsEquipoRef = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    

                    var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle == 1172).ToList();
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

                    var listaEquipoRef = db.MT_Equipo_Referencia.Where(x=> x.Estado == "A").ToList();
                    itemsEquipoRef.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                    foreach (var equiporef in listaEquipoRef)
                    {
                        itemsEquipoRef.Add(new SelectListItem { Value = Convert.ToString(equiporef.Id_Equipo), Text = equiporef.Descripcion });
                    }


                    SelectList selectlistaEquipoRef = new SelectList(itemsEquipoRef, "Value", "Text");


                    ViewBag.listaEstados = selectlistaEstado;
                    ViewBag.listacliente = selectlistacliente;
                    ViewBag.listaLinea = selectlistaLinea;
                    ViewBag.listaTipo = selectlistaTipo;
                    ViewBag.listaPersonas = selectlistaPersonas;
                    ViewBag.listaCategoria = selectlistaCategoria;
                    ViewBag.listaSubcategoria = selectlistaSubCategoria;
                    ViewBag.listaLocalidad = selectlistaLocalidad;
                    ViewBag.listaEquipoRef = selectlistaEquipoRef;
                    ViewBag.id_PostVenta_edit = id_PostVenta_editar;
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
        public ActionResult Agregar_PostVenta([Bind(Include = " Id_Cliente,Fecha_Inicio,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Fin,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst, Id_Equipo_Ref")] MT_PostVenta mT_PostVenta)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    if (mT_PostVenta.Monto_Aux != null) { 
                    string cadMonto = mT_PostVenta.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_PostVenta.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_PostVenta.Valor_Referencial_Aux != null) { 
                    string cadValorReferencial = mT_PostVenta.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_PostVenta.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_PostVenta.Costo_Aux != null) { 
                    string cadCosto = mT_PostVenta.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_PostVenta.costo = decimal.Parse(cadCosto);
                    }

                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        var PostVentas = db.MT_PostVenta.Where(x => x.Codigo_Cliente == mT_PostVenta.Codigo_Cliente && x.Nombre == mT_PostVenta.Nombre && x.Categoria == mT_PostVenta.Categoria && x.Subcategoria == mT_PostVenta.Subcategoria && x.Id_Linea == mT_PostVenta.Id_Linea && x.Id_Cliente == mT_PostVenta.Id_Cliente && x.Id_Empresa == empresa_id.ToString()).ToList();
                        if (PostVentas.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "PostVenta ya existe";
                            return RedirectToAction("PostVenta");
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
                                mT_PostVenta.Id_Usuario = user_id.ToString();
                                //var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                //if (mT_PostVenta.Dia_Plazo == null)
                                //{
                                //    mT_PostVenta.Fecha_Fin = (Convert.ToDateTime(mT_PostVenta.Fecha_Inicio)).AddDays(Convert.ToInt32(mT_PostVenta.Dia_Plazo));
                                //}
                                //else
                                //{
                                //}

                                var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                                mT_PostVenta.Id_Empresa = empresa_id.ToString();

                                /*ojooo verificar como se puede traer los clientes segun permiso*/
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == mT_PostVenta.Id_Cliente).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == mT_PostVenta.tipo).SingleOrDefault();
                                var codigo_guardado = db.MT_PostVenta.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == mT_PostVenta.Id_Cliente && x.Id_Linea == mT_PostVenta.Id_Linea && x.tipo == mT_PostVenta.tipo).ToList().Select(vq => new
                                {
                                    vq.Id_PostVenta,
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



                                mT_PostVenta.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + mT_PostVenta.Id_Linea + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_PostVenta.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + mT_PostVenta.Id_Linea + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                //mT_PostVenta.Codigo_Interno = nom_empresa.Substring(0, 3) + mT_PostVenta.Codigo_Cliente.Substring(0, 3) + listacliente.abreviatura_cliente + mT_PostVenta.Id_Linea + listaTipo.Codigo;
                                mT_PostVenta.Secuencial = seqregistro2;
                                //if (mT_PostVenta.PostVenta_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_PostVenta.Where(x => x.Id_PostVenta == mT_PostVenta.PostVenta_Padre).ToList();
                                //    mT_PostVenta.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}

                                //var lista_PostVenta = db.sp_LINK_ConsultaPostVentas(empresa_id.ToString()).ToList();
                                //foreach (var item in lista_PostVenta)
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
                                //    //databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaPostVenta.Id_PostVenta, mT_ProrrogaPostVenta.Estado, mT_ProrrogaPostVenta.Dia_Prorroga, mT_ProrrogaPostVenta.Descripcion);
                                //    databaseOrden.InsertarPostVenta(item.cia_codigo, cod_cliente, item.fecha_inicial, item.fecha_fin, item.codigo_secuencial_interno_migrado, item.codigo_PostVenta_asociado, user_id.ToString(), item.unidad_migrada, item.cod_servicio, item.codcen, item.detalle, Convert.ToInt32(id_estado.Id_TablaDetalle), Convert.ToDecimal(item.plazo_PostVenta), Convert.ToInt32(cod_tipo.Id_TablaDetalle), Convert.ToDecimal(item.monto), Convert.ToDecimal(item.costo), item.secuencial, item.codigo_secuencial_interno_padre, item.observaciones, item.codigo_secuencial_interno);
                                //}

                                mT_PostVenta.Fecha_registro = DateTime.Now;
                                db.MT_PostVenta.Add(mT_PostVenta);
                                db.SaveChanges();

                                //var ProrrogaInicial = new MT_PostVenta_Prorroga
                                //{
                                //    Id_PostVenta = mT_PostVenta.Id_PostVenta,
                                //    Descripcion = "Prorroga Inicial",
                                //    Dia_Prorroga = mT_PostVenta.Dia_Plazo,
                                //    Fecha_Prorroga = mT_PostVenta.Fecha_Fin,
                                //    Estado = "A",
                                //    Id_PostVenta_Prorroga = 0
                                //};
                                //db.MT_PostVenta_Prorroga.Add(ProrrogaInicial);
                                //db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_PostVenta.Id_PostVenta, "A", mT_PostVenta.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_PostVenta.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato =null,// db.MT_PostVenta.Where(x=>x.)
                                    Id_PostVenta = mT_PostVenta.Id_PostVenta,
                                    Id_Permiso =0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();
                                /*
                                var ProyectoPostVenta = new MT_Proyecto();

                                if (mT_PostVenta.tipo == 1152 || mT_PostVenta.tipo == 1165)
                                {

                                    ProyectoPostVenta = new MT_Proyecto
                                    {
                                        Id_Empresa = mT_PostVenta.Id_Empresa,
                                        Id_Cliente = mT_PostVenta.Id_Cliente,
                                        Fecha_Inicio = mT_PostVenta.Fecha_Inicio,
                                        Fecha_Fin = mT_PostVenta.Fecha_Fin,
                                        Codigo_Interno = mT_PostVenta.Codigo_Interno,
                                        Codigo_Cliente = mT_PostVenta.Codigo_Cliente,
                                        Linea = mT_PostVenta.Id_Linea,
                                        Categoria = mT_PostVenta.Categoria,
                                        Subcategoria = mT_PostVenta.Subcategoria,
                                        Valor_Inicial = mT_PostVenta.Valor_Referencial,
                                        Valor_Ajustado = mT_PostVenta.monto,
                                        Comentario = mT_PostVenta.Observaciones,
                                        Fecha_Registro = mT_PostVenta.Fecha_registro,
                                        Id_PostVenta = mT_PostVenta.Id_PostVenta,
                                        Estado = "A"
                                };
                                db.MT_Proyecto.Add(ProyectoPostVenta);
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
                                        Id_Cliente = mT_PostVenta.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoPostVenta.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_PostVenta.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();
                                }
                                */

                                

                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_PostVentaParametro(mT_PostVenta.Id_PostVenta);
                                TempData["mensaje_correcto"] = "PostVenta guardado";
                                return RedirectToAction("PostVenta");
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("PostVenta");
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
        public ActionResult Editar_PostVenta(int? id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);

                    mT_PostVenta.Valor_Referencial_Aux = mT_PostVenta.Valor_Referencial.ToString();
                    mT_PostVenta.Costo_Aux = mT_PostVenta.costo.ToString();
                    mT_PostVenta.Monto_Aux = mT_PostVenta.monto.ToString();


                    
                    System.Web.HttpContext.Current.Session["PostVenta_interno"] = mT_PostVenta.Codigo_Interno;
                    System.Web.HttpContext.Current.Session["codigo_interno_anterior"] = mT_PostVenta.Codigo_Interno_Ant;
                    System.Web.HttpContext.Current.Session["secuencial"] = mT_PostVenta.Secuencial;
                    System.Web.HttpContext.Current.Session["fecha_registro_cont"] = mT_PostVenta.Fecha_registro;
                    System.Web.HttpContext.Current.Session["id_cliente"] = mT_PostVenta.Id_Cliente;
                    System.Web.HttpContext.Current.Session["id_linea"] = mT_PostVenta.Id_Linea;
                    System.Web.HttpContext.Current.Session["id_tipo"] = mT_PostVenta.tipo;

                    System.Web.HttpContext.Current.Session["tipo_servicio_editagg"] = mT_PostVenta.Categoria;
                    System.Web.HttpContext.Current.Session["centro_costo_editagg"] = mT_PostVenta.Subcategoria;
                    System.Web.HttpContext.Current.Session["Localidad_editagg"] = mT_PostVenta.Localidad;
                    //System.Web.HttpContext.Current.Session["valor_referencial_edit"] = mT_PostVenta.Valor_Referencial;
                    //System.Web.HttpContext.Current.Session["valor_presupuestado_edit"] = mT_PostVenta.monto;
                    //System.Web.HttpContext.Current.Session["valor_ofertado_edit"] = mT_PostVenta.costo;


                    bool seleccion = false;
                    if (mT_PostVenta != null)
                    {


                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente = new List<SelectListItem>();
                        List<SelectListItem> itemsLinea = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        List<SelectListItem> itemsPostVentaPadres = new List<SelectListItem>();
                        List<SelectListItem> itemsPostVentaEsponsable = new List<SelectListItem>();
                        List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsLocalidad = new List<SelectListItem>();
                        List<SelectListItem> itemsEquipoRef = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                        //var listaPostVentaPadres = db.sp_Quimipac_ConsultaMT_PostVenta(empresa_id.ToString()).ToList();

                        int id_tipo = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipo"].ToString());
                        string cliente_id = System.Web.HttpContext.Current.Session["id_cliente"].ToString();

                        


                        var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(41).ToList();
                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_PostVenta.Estado)
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
                            if (cliente.cod_cli == mT_PostVenta.Id_Cliente)
                            {
                                seleccion = true;
                            }

                            itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli, Selected = seleccion });
                        }

                        SelectList selectlistaCliente = new SelectList(itemsCliente, "Value", "Text");




                        var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();

                        foreach (var linea in listaLinea)
                        {
                            if (linea.codigo == mT_PostVenta.Id_Linea)
                            {
                                seleccion = true;
                            }

                            itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaLinea = new SelectList(itemsLinea, "Value", "Text");


                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x=>x.Id_TablaDetalle == 1172).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            if (tipo.Id_TablaDetalle == mT_PostVenta.tipo)
                            {
                                seleccion = true;
                            }

                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaPersonas = db.sp_LINK_ConsultaPersonas().ToList();

                        foreach (var Persona in listaPersonas)
                        {
                            if (Persona.id_persona == mT_PostVenta.Responsable)
                            {
                                seleccion = true;
                            }

                            itemsPostVentaEsponsable.Add(new SelectListItem { Value = Convert.ToString(Persona.id_persona), Text = Persona.Nombres_Completos, Selected = seleccion });
                        }

                        SelectList selectlistaPersonas = new SelectList(itemsPostVentaEsponsable, "Value", "Text");

                        var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), mT_PostVenta.Id_Linea).ToList();
                        //itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        foreach (var categoria in listaCategoria)
                        {
                            if (categoria.cod_servicio == mT_PostVenta.Categoria)
                            {
                                seleccion = true;
                            }

                            itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                        var listaSubcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), mT_PostVenta.Id_Linea).ToList();
                        //itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var subcategoria in listaSubcategoria)
                        {
                            if (subcategoria.codcen == mT_PostVenta.Subcategoria)
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
                            if (localidad.codigo_loc == mT_PostVenta.Localidad)
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

                        
                        var listaRefeCtr = db.sp_Quimipac_ConsultaMT_PostVenta(empresa_id.ToString()).Where(vq => vq.tipo == 145 || vq.tipo == 146).ToList();

                        var ClienteLI_R = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        var listaEquipoRef = db.MT_Equipo_Referencia.Where(x=>x.Estado == "A").ToList();
                        itemsEquipoRef.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var equiporef in listaEquipoRef)
                        {
                            if (equiporef.Id_Equipo == mT_PostVenta.Id_Equipo_Ref)
                            {
                                seleccion = true;
                            }

                            itemsEquipoRef.Add(new SelectListItem { Value = Convert.ToString(equiporef.Id_Equipo), Text = equiporef.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEquipoRef = new SelectList(itemsEquipoRef, "Value", "Text");

                        ViewBag.listaEstados = selectlistaEstado;
                        ViewBag.listacliente = selectlistaCliente;
                        ViewBag.listaLinea = selectlistaLinea;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaPersonas = selectlistaPersonas;
                        ViewBag.listaCategoria = selectlistaCategoria;
                        ViewBag.listaSubcategoria = selectlistaSubcategoria;
                        ViewBag.id_PostVenta_Editar = id;
                        ViewBag.listaLocalidad = selectlistaLocalidad;
                        ViewBag.listaEquipoRef = selectlistaEquipoRef;
                        ViewBag.NombreUsuario = nombreUsuario;
                        ViewBag.NombreFiscalizador = nombre_Fiscalizador_Contr;
                        ViewBag.Cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"];
                        ViewBag.Linea_edit = System.Web.HttpContext.Current.Session["id_linea"];
                        ViewBag.tiposervicio_edit = System.Web.HttpContext.Current.Session["tipo_servicio_editagg"];
                        ViewBag.centrocosto_edit = System.Web.HttpContext.Current.Session["centro_costo_editagg"];
                        ViewBag.Localidad_edit = System.Web.HttpContext.Current.Session["Localidad_editagg"];
                        ViewBag.valor_ref_edit = mT_PostVenta.Valor_Referencial;
                        ViewBag.costo_edit = mT_PostVenta.costo;
                        ViewBag.monto_edit = mT_PostVenta.monto;
                        var PostVenta_codedit = mT_PostVenta.Codigo_Cliente;
                        if (PostVenta_codedit== null || PostVenta_codedit == "")
                        {
                            PostVenta_codedit = mT_PostVenta.Nombre;
                        }
                        ViewBag.PostVenta_codedit = PostVenta_codedit;
                        return View(mT_PostVenta);

                    }
                    else
                    {
                        TempData["mensaje_error"] = "PostVenta no existe";
                        return RedirectToAction("PostVenta");
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
        public ActionResult Editar_PostVenta([Bind(Include = "Id_PostVenta, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones,Valor_Referencial_Aux,Monto_Aux,Costo_Aux,Localidad,Fecha_Aprobacion_Cot,Recepcion_Servicio,Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst, Id_Equipo_Ref")] MT_PostVenta mT_PostVenta)
        {
            //var mT_PostVentaporroga = db.MT_PostVenta_Prorroga.Where(x => x.Id_PostVenta == mT_PostVenta.Id_PostVenta && x.Estado == "A");
            var codigointernoPostVenta = System.Web.HttpContext.Current.Session["PostVenta_interno"].ToString();
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
                    if (mT_PostVenta.Monto_Aux != null) { 
                    string cadMonto = mT_PostVenta.Monto_Aux.ToString();
                    cadMonto = cadMonto.Replace(",", "");
                    cadMonto = cadMonto.Replace(".", ",");
                    mT_PostVenta.monto = decimal.Parse(cadMonto);
                    }
                    if (mT_PostVenta.Valor_Referencial_Aux!=null) { 
                    string cadValorReferencial = mT_PostVenta.Valor_Referencial_Aux.ToString();
                    cadValorReferencial = cadValorReferencial.Replace(",", "");
                    cadValorReferencial = cadValorReferencial.Replace(".", ",");
                    mT_PostVenta.Valor_Referencial = decimal.Parse(cadValorReferencial);
                    }
                    if (mT_PostVenta.Costo_Aux!=null) { 
                    string cadCosto = mT_PostVenta.Costo_Aux.ToString();
                    cadCosto = cadCosto.Replace(",", "");
                    cadCosto = cadCosto.Replace(".", ",");
                    mT_PostVenta.costo = decimal.Parse(cadCosto);
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

                            //if (mT_PostVenta.Fecha_Fin == null)//{
                            //    mT_PostVenta.Fecha_Fin = (Convert.ToDateTime(mT_PostVenta.Fecha_Inicio)).AddDays(Convert.ToInt32(mT_PostVenta.Dia_Plazo));
                            //}//else//{//    if (mT_PostVentaporroga == null)
                            //    {//        mT_PostVenta.Fecha_Fin = (Convert.ToDateTime(mT_PostVenta.Fecha_Fin)).AddDays(Convert.ToInt32(mT_PostVenta.Dia_Plazo));
                            //    }//    else //    {//}//}                            

                            var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                            var id_cliente_nuevo = mT_PostVenta.Id_Cliente;
                            var id_linea_nuevo = mT_PostVenta.Id_Linea;
                            var id_tipo_nuevo = mT_PostVenta.tipo;
                            var categoria_nueva = mT_PostVenta.Categoria;
                            var subcategoria_nueva = mT_PostVenta.Subcategoria;

                            if (mT_PostVenta.Id_Cliente == id_cliente_edit && mT_PostVenta.Id_Linea == id_linea_edit && mT_PostVenta.tipo == id_tipo_edit)
                            {
                                mT_PostVenta.Id_Empresa = empresa_id.ToString();
                                mT_PostVenta.Id_Usuario = user_id.ToString();
                                mT_PostVenta.Secuencial = secuencial;
                                mT_PostVenta.Codigo_Interno_Ant = codigointerno_anterior;

                                //if (mT_PostVenta.PostVenta_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_PostVenta.Where(x => x.Id_PostVenta == mT_PostVenta.PostVenta_Padre).ToList();
                                //    mT_PostVenta.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}
                                mT_PostVenta.Fecha_registro = Convert.ToDateTime(fecha_registro);
                                mT_PostVenta.Fecha_modificacion = DateTime.Now;
                                DataBase_Externo databaseOrden = new DataBase_Externo();
                                databaseOrden.UpdatePostVenta(mT_PostVenta.Id_PostVenta, mT_PostVenta.Id_Cliente, mT_PostVenta.Fecha_Inicio, mT_PostVenta.Fecha_Fin, mT_PostVenta.Codigo_Interno, mT_PostVenta.Codigo_Cliente, mT_PostVenta.Id_Linea, mT_PostVenta.Categoria, mT_PostVenta.Subcategoria, mT_PostVenta.Nombre, mT_PostVenta.Estado, mT_PostVenta.Dia_Plazo, mT_PostVenta.tipo, mT_PostVenta.Valor_Referencial, mT_PostVenta.monto, mT_PostVenta.costo, mT_PostVenta.Responsable, mT_PostVenta.Secuencial, mT_PostVenta.Codigo_Interno_Ant, mT_PostVenta.Observaciones, mT_PostVenta.Codigo_interno_padre, mT_PostVenta.Fecha_registro, mT_PostVenta.Fecha_modificacion, mT_PostVenta.Localidad, mT_PostVenta.Fecha_Aprobacion_Cot, mT_PostVenta.Recepcion_Servicio, mT_PostVenta.Fecha_Firma_Conformidad, mT_PostVenta.Fecha_Cumplimiento_Inst);
                                //Id_PostVenta, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,PostVenta_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones
                                TempData["mensaje_actualizado"] = "PostVenta actualizado";
                            }
                            else
                            {

                                //mT_PostVenta.Id_Cliente = id_cliente_edit;
                                //mT_PostVenta.Id_Linea = id_linea_edit;
                                //mT_PostVenta.tipo = id_tipo_edit;
                                //mT_PostVenta.Categoria = categoria_edit;
                                //mT_PostVenta.Subcategoria = subcategoria_edit;
                                //mT_PostVenta.Id_Empresa = empresa_id.ToString();
                                //mT_PostVenta.Id_Usuario = user_id.ToString();
                                //mT_PostVenta.Secuencial = secuencial;
                                //mT_PostVenta.Codigo_Interno_Ant = codigointerno_anterior;                                

                                //if (mT_PostVenta.PostVenta_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_PostVenta.Where(x => x.Id_PostVenta == mT_PostVenta.PostVenta_Padre).ToList();
                                //    mT_PostVenta.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}
                                //mT_PostVenta.Fecha_registro = Convert.ToDateTime(fecha_registro);
                                //mT_PostVenta.Fecha_modificacion = DateTime.Now;
                                //mT_PostVenta.Estado = 1160;
                                MT_PostVenta mt_PostVentaEditar = db.MT_PostVenta.Find(mT_PostVenta.Id_PostVenta);
                                mt_PostVentaEditar.Estado = 1160;
                                db.Entry(mt_PostVentaEditar).State = EntityState.Modified;
                                db.SaveChanges();
                                int? secuencial_nuevo = 0;
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == id_cliente_nuevo).SingleOrDefault();

                                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == id_tipo_nuevo).SingleOrDefault();
                                var codigo_guardado = db.MT_PostVenta.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == id_cliente_nuevo && x.Id_Linea == id_linea_nuevo && x.tipo == id_tipo_nuevo).ToList().Select(vq => new
                                {
                                    vq.Id_PostVenta,
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



                                mT_PostVenta.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + id_linea_nuevo + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_PostVenta.Codigo_Interno_Ant = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + id_linea_nuevo + listaTipo.Codigo + auxSeq + DateTime.Now.Year;
                                mT_PostVenta.Secuencial = seqregistro2;
                                //if (mT_PostVenta.PostVenta_Padre != 0)
                                //{
                                //    var codigo_iterno_padre = db.MT_PostVenta.Where(x => x.Id_PostVenta == mT_PostVenta.PostVenta_Padre).ToList();
                                //    mT_PostVenta.Codigo_interno_padre = codigo_iterno_padre.ElementAt(0).Codigo_Interno;
                                //}

                                
                                mT_PostVenta.Fecha_registro = DateTime.Now;
                                mT_PostVenta.Id_Empresa = empresa_id.ToString();
                                mT_PostVenta.Id_Usuario = user_id.ToString();
                                mT_PostVenta.Id_Cliente = id_cliente_nuevo;
                                mT_PostVenta.Id_Linea = id_linea_nuevo;
                                mT_PostVenta.tipo = id_tipo_nuevo;
                                mT_PostVenta.Estado = 75;
                                mT_PostVenta.Categoria = categoria_nueva;
                                mT_PostVenta.Subcategoria= subcategoria_nueva;
                                db.MT_PostVenta.Add(mT_PostVenta);
                                db.SaveChanges();

                                //var ProrrogaInicial = new MT_PostVenta_Prorroga
                                //{
                                //    Id_PostVenta = mT_PostVenta.Id_PostVenta,
                                //    Descripcion = "Prorroga Inicial",
                                //    Dia_Prorroga = mT_PostVenta.Dia_Plazo,
                                //    Fecha_Prorroga = mT_PostVenta.Fecha_Fin,
                                //    Estado = "A",
                                //    Id_PostVenta_Prorroga = 0
                                //};
                                //db.MT_PostVenta_Prorroga.Add(ProrrogaInicial);
                                //db.SaveChanges();

                                //DataBase_Externo databaseOrden_Pro = new DataBase_Externo();
                                //databaseOrden_Pro.InsertarFecha_Prorroga(mT_PostVenta.Id_PostVenta, "A", mT_PostVenta.Dia_Plazo, "Prorroga Inicial");


                                var obPermiso = new MT_Permiso
                                {
                                    Aprobar = "S",
                                    Consultar = "S",
                                    Modificar = "S",
                                    Crear = "S",
                                    Eliminar = "N",
                                    Estado = "A",
                                    Fecha_Registro = DateTime.Now,
                                    Id_Cliente = mT_PostVenta.Id_Cliente,
                                    Id_Empresa = empresa_id.ToString(),
                                    Id_Linea = null,
                                    Id_Proyecto = null,
                                    Id_Tipo_Trabajo = null,
                                    Id_Usuario = user_id.ToString(),
                                    Usuario = user_id.ToString(),
                                    Id_Contrato = null,// db.MT_PostVenta.Where(x=>x.)
                                    Id_PostVenta = mT_PostVenta.Id_PostVenta,
                                    Id_Permiso = 0
                                };
                                db.MT_Permiso.Add(obPermiso);
                                db.SaveChanges();
                                //var dbe = new DataBase_Externo();
                                //dbe.Insertar_PostVentaParametro(mT_PostVenta.Id_PostVenta);
                                /*
                                var ProyectoPostVenta = new MT_Proyecto();
                                if (mT_PostVenta.tipo == 1152 || mT_PostVenta.tipo == 1165) { 
                                    ProyectoPostVenta = new MT_Proyecto
                                {
                                    Id_Empresa = mT_PostVenta.Id_Empresa,
                                    Id_Cliente = mT_PostVenta.Id_Cliente,
                                    Fecha_Inicio = mT_PostVenta.Fecha_Inicio,
                                    Fecha_Fin = mT_PostVenta.Fecha_Fin,
                                    Codigo_Interno = mT_PostVenta.Codigo_Interno,
                                    Codigo_Cliente = mT_PostVenta.Codigo_Cliente,
                                    Linea = mT_PostVenta.Id_Linea,
                                    Categoria = mT_PostVenta.Categoria,
                                    Subcategoria = mT_PostVenta.Subcategoria,
                                    Valor_Inicial = mT_PostVenta.Valor_Referencial,
                                    Valor_Ajustado = mT_PostVenta.monto,
                                    Comentario = mT_PostVenta.Observaciones,
                                    Fecha_Registro = mT_PostVenta.Fecha_registro,
                                    Id_PostVenta = mT_PostVenta.Id_PostVenta
                                };
                                db.MT_Proyecto.Add(ProyectoPostVenta);
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
                                        Id_Cliente = mT_PostVenta.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = ProyectoPostVenta.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_PostVenta.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermisoProyecto);
                                    db.SaveChanges();

                                }
                                */
                                

                                TempData["mensaje_correcto"] = "PostVenta anterior eliminado porque cambio el cliente, la linea o el tipo, PostVenta nuevo guardado";

                            }

                            return RedirectToAction("PostVenta");
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("PostVenta");
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
        public ActionResult EliminarPostVenta(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
                    using (var ctx = new BD_QUIMIPACEntities())
                    {
                        var nreg = ctx.Database.SqlQuery<MT_PostVenta>("select * from MT_PostVenta c where c.Id_PostVenta in(SELECT DISTINCT PostVenta_Padre " + 
                    "FROM MT_PostVenta) and Id_PostVenta =" + id).ToList();
                        if (nreg.Count > 0)
                        {
                            TempData["mensaje_error"] = "PostVenta no se puede eliminar porque este PostVenta tiene hijos";
                            return RedirectToAction("PostVenta");
                        }
                        else {
                            mT_PostVenta.Estado = 1160;//2156;

                            db.Entry(mT_PostVenta).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "PostVenta eliminado";
                            return RedirectToAction("PostVenta");
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

        public JsonResult GetPostVenta(string id)
        {
            try
            {
                List<SelectListItem> itemsPostVentas = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                
                System.Web.HttpContext.Current.Session["id_Cliente"] = id;

                //var listaTrabajoPadreAux = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(id, empresa_id.ToString()).ToList();
                var listaPostVentaPadre = db.sp_Quimipac_ConsultaMT_PostVenta(empresa_id.ToString()).Where(x => x.Id_Cliente == id).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaPostVentaPadre = listaPostVentaPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PostVentaS").ToList();
                listaPostVentaPadre = listaPostVentaPadre.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();

                itemsPostVentas.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                foreach (var PostVentas in listaPostVentaPadre)
                {
                    //itemsPostVentas.Add(new SelectListItem { Value = Convert.ToString(PostVentas.Id_PostVenta), Text = PostVentas.Codigo_Interno + " | " + PostVentas.Nombre });
                }
                // itemsTiposTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                SelectList selectlistaPostVenta = new SelectList(itemsPostVentas, "Value", "Text");

                // ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;

                return Json(selectlistaPostVenta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //get editar PostVenta y default
        public JsonResult GetPostVentaDefauledit(string id_Cliente, int id_PostVenta_Editar)
        {
            try
            {
                List<SelectListItem> itemsPostVentas = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                //2156 mentcol
                //var PostVentaPadre = db.MT_PostVenta.Where(x => x.Id_Empresa == empresa && x.Estado != 2156 && x.Id_Cliente == id_Cliente).Select(u=> new { u.Id_PostVenta, u.Id_Cliente, u.Nombre, u.PostVenta_Padre}).ToList();
                var PostVentaPadre = db.MT_PostVenta.Where(x => x.Id_Empresa == empresa && x.Estado != 1160 && x.Id_Cliente == id_Cliente).Select(u => new { u.Id_PostVenta, u.Id_Cliente, u.Nombre }).ToList();
                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                PostVentaPadre = PostVentaPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PostVentaS").ToList();
                PostVentaPadre = PostVentaPadre.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();

                return Json(PostVentaPadre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Response.StatusCode);
            }
        }

        //

        public JsonResult GetPostVentaTipo(string id_cliente_Tipo, int id_PostVenta_Editar, int id_tipo_Tipo)
        {
    try
    {
        List<SelectListItem> itemsPostVentasTipo = new List<SelectListItem>();
        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();
        string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
        var listaPostVentaPadreTipo = db.sp_Quimipac_ConsultaMT_PostVenta_TipoPadre(empresa, id_tipo_Tipo, id_cliente_Tipo).ToList();

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                listaPostVentaPadreTipo = listaPostVentaPadreTipo.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "POSTVENTA").ToList();
                listaPostVentaPadreTipo = listaPostVentaPadreTipo.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();

                return Json(listaPostVentaPadreTipo, JsonRequestBehavior.AllowGet);
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

        //public JsonResult GetCategoria_Edit(string id, int id_PostVenta_Editar, string id_Linea)
        

        public JsonResult GetCategoria_Edit(int id_PostVenta_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_PostVenta mt_PostVenta = db.MT_PostVenta.Find(id_PostVenta_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), id_Linea).ToList();
                var listaCateAux = new List<sp_LINK_ConsultaTipoServicio_Cat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                {
                    cia_codigo = empresa,
                    cod_linea = (mt_PostVenta.Categoria == "0" ?"OK":""),
                    cod_servicio = "0",
                    nombre = "Ninguno"
                });

                foreach (var categoria in listaCategoria)
                {
                    listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                    {
                        cia_codigo = empresa,
                        cod_linea = (categoria.cod_servicio == mt_PostVenta.Categoria)? "OK" : "",
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

        //public JsonResult GetSubCategoria_Edit(int id_PostVenta_Editar, string id_Linea)
        //{
        //    try
        //    {
        //        //id = id.Replace("ampersand", "&");
        //        id_Linea = id_Linea.Replace("ampersand", "&");


        //        bool seleccion = false;
        //        MT_PostVenta mt_PostVenta = db.MT_PostVenta.Find(id_PostVenta_Editar);

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
        //            if (subcategoria.codcen == mt_PostVenta.Subcategoria)
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

        public JsonResult GetSubCategoria_Edit(int id_PostVenta_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_PostVenta mt_PostVenta = db.MT_PostVenta.Find(id_PostVenta_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id_Linea).ToList();
                var listaSubCateAux = new List<sp_LINK_ConsultaCentroConsumo_SubCat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                {
                    cia_codigo = empresa,
                    quimi_linea = (mt_PostVenta.Subcategoria == "0" ? "OK" : ""),
                    codcen = "0",
                    nombre = "Ninguno"
                });

                foreach (var subcategoria in listaSubCategoria)
                {
                    listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                    {
                        cia_codigo = empresa,
                        quimi_linea = (subcategoria.codcen == mt_PostVenta.Subcategoria) ? "OK" : "",
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

        public JsonResult GetPostVentaPadreMonto(int id_PostVentaPadre_Change, int id_PostVenta_Editar)
        {
            try
            {
                //List<SelectListItem> itemsPostVentasTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                //string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                //var listaPostVentaPadreTipo = db.sp_Quimipac_ConsultaMT_PostVenta_TipoPadre(empresa, id_tipo_Tipo, id_cliente_Tipo).ToList();

                //var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "CLIENTES").ToList();

                //listaPostVentaPadreTipo = listaPostVentaPadreTipo.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                //var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa, "PostVentaS").ToList();
                //listaPostVentaPadreTipo = listaPostVentaPadreTipo.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();
                var mensaje = new JsonResult();
                // Valor_Referencial_PostVentaPadre = db.MT_PostVenta.Where(x=>x.Id_PostVenta == id_PostVentaPadre_Change).SingleOrDefault().Valor_Referencial;
                var lista_camp_PostVentaPadre = db.MT_PostVenta.Where(x=>x.Id_PostVenta == id_PostVentaPadre_Change).ToList().Select(vq => new
                {
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria,
                    vq.Valor_Referencial
                });
                var valr_ref_ = lista_camp_PostVentaPadre.ElementAt(0).Valor_Referencial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");


                var listaCampos_CtrPadre = new List<MT_PostVenta>();

                //cod_linea  lo utilizo com ocampo auxiliar
                var id_linea = lista_camp_PostVentaPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_PostVentaPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_PostVentaPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();
                listaCampos_CtrPadre.Add(new MT_PostVenta
                {
                    Id_Linea = lista_camp_PostVentaPadre.ElementAt(0).Id_Linea,
                    Categoria = lista_camp_PostVentaPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_PostVentaPadre.ElementAt(0).Subcategoria,
                    Valor_Referencial_Aux = auxValor,
                    lineaPostVenta = nombre_linea.ToString(),
                    nombre_Categoria = nombre_categoria.ToString(),
                    nombre_Subcategoria = nombre_Subcategoria.ToString()
                });
                
                //mensaje.Data = auxValor;// Valor_Referencial_PostVentaPadre;
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

        //FORMULARIOS DE PostVenta
        #region


        [HttpGet]
public ActionResult Formularios_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
            if (mT_PostVenta != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_PostVenta"] = mT_PostVenta.Id_PostVenta;
                var listaFormulariosC = db.sp_Quimipac_ConsultaMT_PostVentaDocumentado(id, empresa_id.ToString()).ToList();
                var PostVenta = mT_PostVenta.Codigo_Cliente;
                ViewBag.listaFormulariosC = listaFormulariosC;
                ViewBag.PostVenta = PostVenta;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("PostVenta");
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
public ActionResult AgregarFormulario_PostVenta()
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
public ActionResult AgregarFormulario_PostVenta([Bind(Include = "Id_PostVenta,Descripcion,Enlace,Fecha_Registro,Fecha_Validez,Estado,NombreArchivo,Tipo,Version")] MT_PostVenta_Documentado mT_FormularioC, HttpPostedFileBase NombreArchivo)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {
                    var FormularioC = db.MT_PostVenta_Documentado.Where(x => x.Estado == mT_FormularioC.Estado && x.NombreArchivo == mT_FormularioC.NombreArchivo && x.Tipo == mT_FormularioC.Tipo);
                    if (FormularioC.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "Formulario ya existe";
                        return RedirectToAction("PostVenta", new { id = idPostVenta });
                    }
                    else
                    {
                        if (NombreArchivo != null)
                        {
                            var ruta_archivo = @"~/Formularios_Actividad/";
                            var ruta_servidor = Path.GetFullPath(ruta_archivo);
                            var extension = Path.GetExtension(NombreArchivo.FileName);

                            int fileSize = NombreArchivo.ContentLength;

                            mT_FormularioC.Id_PostVenta = idPostVenta;
                            mT_FormularioC.NombreArchivo = "";
                            mT_FormularioC.Fecha_Registro = DateTime.Now;
                            mT_FormularioC.Estado = mT_FormularioC.Estado.Substring(0, 1);
                            mT_FormularioC.Enlace = ruta_servidor;
                            db.MT_PostVenta_Documentado.Add(mT_FormularioC);
                            db.SaveChanges();

                            int scope_identity_id = mT_FormularioC.Id_PostVenta_Documentado;

                            string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                            mT_FormularioC.NombreArchivo = nombreArchivo;

                            NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                            db.Entry(mT_FormularioC).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Formulario de PostVenta guardado";
                            return RedirectToAction("Formularios_PostVenta", new { id = idPostVenta });
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
                    return RedirectToAction("Formularios_PostVenta", new { id = idPostVenta });
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
public ActionResult EliminarFormularioPostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {


            var id_MTPostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_MTPostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_MTPostVenta.ToString());

                MT_PostVenta_Documentado mT_Formulario = db.MT_PostVenta_Documentado.Find(id);

                if (mT_Formulario != null)
                {
                    mT_Formulario.Estado = "E";
                    db.Entry(mT_Formulario).State = EntityState.Modified;
                    //db.MT_Formulario.Remove(mT_Formulario);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Archivo eliminado";
                    return RedirectToAction("Formularios_PostVenta", new { id = idPostVenta });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Formularios_PostVenta", new { id = idPostVenta });
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

//ALERTA DE PostVenta
#region

[HttpGet]
public ActionResult Alerta_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
            if (mT_PostVenta != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_PostVenta"] = mT_PostVenta.Id_PostVenta;
                var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(1169, empresa_id.ToString(), id).ToList();
                var PostVenta = mT_PostVenta.Codigo_Cliente;
                ViewBag.listaNotificaciones = listaNotificaciones;
                ViewBag.PostVenta = PostVenta;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("PostVenta");
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
                if (tipo.Descripcion.Equals("PostVenta"))
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
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];
            int idPostVenta = int.Parse(id_PostVenta.ToString());
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
                    return RedirectToAction("Alerta_PostVenta", new { id = idPostVenta });
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

                        var Id_noti_Nuevo = dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado, idPostVenta, Convert.ToInt32(Id_Notificacion), mT_Notificacion.Frecuencia, mT_Notificacion.EmpresaID, mT_Notificacion.Reenvio);
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
                        return RedirectToAction("Alerta_PostVenta", new { id = idPostVenta });
                    }

                }

            }
            else
            {
                TempData["mensaje_error"] = "Valores incorrectos";
                return RedirectToAction("Alerta_PostVenta", new { id = idPostVenta });
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


            var id_MTPostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_MTPostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idMTActividad = int.Parse(id_MTPostVenta.ToString());

                MT_Notificaciones mT_Alerta = db.MT_Notificaciones.Find(id);
                        
                if (mT_Alerta != null)
                {
                    mT_Alerta.Estado = false;
                    db.Entry(mT_Alerta).State = EntityState.Modified;
                    //db.MT_PostVentaAlerta.Remove(mT_Alerta);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Alerta eliminado";
                    return RedirectToAction("Alerta_PostVenta", new { id = id_MTPostVenta });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Alerta_PostVenta", new { id = id_MTPostVenta });
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

//PARAMETRO PostVenta
#region
/*
[HttpGet]
public ActionResult Parametro_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
            if (mT_PostVenta != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_PostVenta"] = mT_PostVenta.Id_PostVenta;
                var listaParametroC = db.sp_Quimipac_ConsultaMT_PostVentaParametro(id, empresa_id.ToString()).ToList();
                var PostVenta = mT_PostVenta.Codigo_Cliente;
                ViewBag.listaParametroC = listaParametroC;
                ViewBag.PostVenta = PostVenta;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("PostVenta");
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
public ActionResult AgregarParametro_PostVenta()
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
public ActionResult AgregarParametro_PostVenta([Bind(Include = "Id_PostVenta, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_PostVenta_Parametro mT_ParametroPostVenta)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {
                    var PostVentaParametro = db.MT_PostVenta_Parametro.Where(x => x.Id_PostVenta == mT_ParametroPostVenta.Id_PostVenta && x.Parametro == mT_ParametroPostVenta.Parametro && x.Valor_Inicial == mT_ParametroPostVenta.Valor_Inicial && x.Valor_Final == mT_ParametroPostVenta.Valor_Final);
                    if (PostVentaParametro.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "PostVenta Parametro ya existe";
                        return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
                    }
                    else
                    {

                        mT_ParametroPostVenta.Id_PostVenta = idPostVenta;
                        mT_ParametroPostVenta.Estado = mT_ParametroPostVenta.Estado.Substring(0, 1);
                        db.MT_PostVenta_Parametro.Add(mT_ParametroPostVenta);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Parametro de PostVenta guardado";
                        return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });

                    }

                }

                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
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
public ActionResult EditarParametro_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta_Parametro mT_PostVentaParametro = db.MT_PostVenta_Parametro.Find(id);
            bool seleccion = false;
            if (mT_PostVentaParametro != null)
            {
                List<SelectListItem> itemsTipo = new List<SelectListItem>();

                var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(64).ToList();
                foreach (var tipo in listaTipo)
                {
                    if (tipo.Id_TablaDetalle == mT_PostVentaParametro.Tipo_Dato)
                    {
                        seleccion = true;
                    }

                    itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                }

                SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                ViewBag.listaTipo = selectlistaTipo;

                return View(mT_PostVentaParametro);

            }
            else
            {

                var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

                if (id_PostVenta == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPostVenta = int.Parse(id_PostVenta.ToString());
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
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
public ActionResult EditarParametro_PostVenta([Bind(Include = "Id_PostVenta_Parametro, Id_PostVenta, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final")] MT_PostVenta_Parametro mT_ParametroPostVenta)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];
            // int bandera = 0;
            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {

                    //var PostVentaParametro = db.MT_PostVenta_Parametro.Where(x => x.Id_PostVenta == mT_ParametroPostVenta.Id_PostVenta && x.Parametro == mT_ParametroPostVenta.Parametro);
                    //if (PostVentaParametro.Count() >= 1)
                    //{


                    mT_ParametroPostVenta.Estado = mT_ParametroPostVenta.Estado.Substring(0, 1);


                    db.Entry(mT_ParametroPostVenta).State = EntityState.Modified;
                    db.SaveChanges();

                    //}

                    //else
                    //{
                    //    DataBase_Externo databaseOrden = new DataBase_Externo();
                    //    databaseOrden.InsertarParametroPostVentaEditNuevo(mT_ParametroPostVenta.Id_PostVenta_Parametro, mT_ParametroPostVenta.Id_PostVenta, mT_ParametroPostVenta.Parametro, mT_ParametroPostVenta.Descripcion, mT_ParametroPostVenta.Tipo_Dato, mT_ParametroPostVenta.Valor_Inicial, mT_ParametroPostVenta.Estado, mT_ParametroPostVenta.Valor_Final);
                    //}
                    TempData["mensaje_actualizado"] = "Parametro de PostVenta actualizado";

                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
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

public ActionResult EliminarParametroPostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {


            var id_MTPostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_MTPostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_MTPostVenta.ToString());

                MT_PostVenta_Parametro mT_parametro_ctr = db.MT_PostVenta_Parametro.Find(id);

                if (mT_parametro_ctr != null)
                {
                    mT_parametro_ctr.Estado = "E";
                    db.Entry(mT_parametro_ctr).State = EntityState.Modified;
                    //db.MT_Formulario.Remove(mT_Formulario);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Parametro eliminado";
                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Parametro_PostVenta", new { id = idPostVenta });
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

//Prorroga PostVenta
#region
/*
    * [HttpGet]
public ActionResult Prorroga_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
            if (mT_PostVenta != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_PostVenta"] = mT_PostVenta.Id_PostVenta;
                var listaProrrogaCont = db.sp_Quimipac_ConsultaMT_PostVentaProrroga(id, empresa_id.ToString()).ToList();
                var PostVenta = mT_PostVenta.Codigo_Cliente;
                ViewBag.listaProrrogaCont = listaProrrogaCont;
                ViewBag.PostVenta = PostVenta;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("PostVenta");
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
public ActionResult AgregarProrroga_PostVenta()
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
public ActionResult AgregarProrroga_PostVenta([Bind(Include = "Id_PostVenta, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_PostVenta_Prorroga mT_ProrrogaPostVenta)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());
                var feha_prorroga = db.MT_PostVenta_Prorroga.Where(x => x.Id_PostVenta == idPostVenta && x.Estado == mT_ProrrogaPostVenta.Estado.Substring(0, 1)).ToList();
                MT_PostVenta mT_PostVentaPro = db.MT_PostVenta.Find(idPostVenta);
                if (ModelState.IsValid)
                {

                    var fecha_fin_PostVenta = db.MT_PostVenta.Where(x => x.Id_PostVenta == idPostVenta).SingleOrDefault();

                    if (fecha_fin_PostVenta.Fecha_Fin != null)
                    {

                        if (feha_prorroga.Count() == 0)
                        {
                            mT_ProrrogaPostVenta.Fecha_Prorroga = (Convert.ToDateTime(fecha_fin_PostVenta.Fecha_Fin)).AddDays(Convert.ToInt32(mT_ProrrogaPostVenta.Dia_Prorroga));
                            mT_ProrrogaPostVenta.Id_PostVenta = idPostVenta;
                            mT_ProrrogaPostVenta.Estado = mT_ProrrogaPostVenta.Estado.Substring(0, 1);
                            mT_PostVentaPro.Fecha_Fin = mT_ProrrogaPostVenta.Fecha_Prorroga;
                            db.MT_PostVenta_Prorroga.Add(mT_ProrrogaPostVenta);
                            db.SaveChanges();
                        }
                        else
                        {
                            mT_ProrrogaPostVenta.Id_PostVenta = idPostVenta;
                            mT_ProrrogaPostVenta.Estado = mT_ProrrogaPostVenta.Estado.Substring(0, 1);
                            DataBase_Externo databaseOrden = new DataBase_Externo();
                            databaseOrden.InsertarFecha_Prorroga(mT_ProrrogaPostVenta.Id_PostVenta, mT_ProrrogaPostVenta.Estado, mT_ProrrogaPostVenta.Dia_Prorroga, mT_ProrrogaPostVenta.Descripcion);
                            //mT_ProrrogaPostVenta.Fecha_Prorroga = (Convert.ToDateTime(feha_prorroga)).AddDays(Convert.ToInt32(mT_ProrrogaPostVenta.Dia_Prorroga));

                        }

                        TempData["mensaje_correcto"] = "PostVenta Prorroga guardado";
                        return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });

                    }
                    else
                    {
                        TempData["mensaje_error"] = "No hay fecha Fin, no se puede dar fecha prorroga";
                        return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });

                    }

                    //var PostVentaProrroga = db.MT_PostVenta_Prorroga.Where(x => x.Id_PostVenta == mT_ProrrogaPostVenta.Id_PostVenta && x.Fecha_Prorroga == mT_ProrrogaPostVenta.Fecha_Prorroga && x.Estado == mT_ProrrogaPostVenta.Estado);
                    //if (PostVentaProrroga.Count() >= 1)
                    //{
                    //    TempData["mensaje_error"] = "PostVenta Prorroga ya existe";
                    //    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
                    //}
                    //else
                    //{


                    //}

                }

                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
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
public ActionResult EditarProrroga_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta_Prorroga mT_PostVentaProrroga = db.MT_PostVenta_Prorroga.Find(id);
            bool seleccion = false;
            if (mT_PostVentaProrroga != null)
            {
                return View(mT_PostVentaProrroga);

            }
            else
            {

                var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

                if (id_PostVenta == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPostVenta = int.Parse(id_PostVenta.ToString());
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
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
public ActionResult EditarProrroga_PostVenta([Bind(Include = "Id_PostVenta_Prorroga, Id_PostVenta, Fecha_Prorroga, Estado, Descripcion, Dia_Prorroga")] MT_PostVenta_Prorroga mT_ProrrogaPostVenta)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];
            // int bandera = 0;
            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {


                    mT_ProrrogaPostVenta.Estado = mT_ProrrogaPostVenta.Estado.Substring(0, 1);


                    db.Entry(mT_ProrrogaPostVenta).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "PostVenta Prorroga actualizado";

                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
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

public ActionResult EliminarProrrogaPostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {


            var id_MTPostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_MTPostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_MTPostVenta.ToString());

                MT_PostVenta_Prorroga mT_Prorroga_elim = db.MT_PostVenta_Prorroga.Find(id);

                if (mT_Prorroga_elim != null)
                {
                    mT_Prorroga_elim.Estado = "E";
                    db.Entry(mT_Prorroga_elim).State = EntityState.Modified;
                    db.SaveChanges();

                    var ultimafechainact = db.MT_PostVenta_Prorroga.Where(vq => vq.Id_PostVenta == idPostVenta && vq.Estado == "I").OrderByDescending(vq => vq.Fecha_Prorroga).Take(1).ToList();
                    MT_PostVenta_Prorroga mT_Prorroga_habili = db.MT_PostVenta_Prorroga.Find(ultimafechainact.ElementAt(0).Id_PostVenta_Prorroga);
                    mT_Prorroga_habili.Estado = "A";                            
                    db.Entry(mT_Prorroga_habili).State = EntityState.Modified;
                    db.SaveChanges();


                    MT_PostVenta mt_PostVenta_edit_ffin = db.MT_PostVenta.Find(idPostVenta);
                    mt_PostVenta_edit_ffin.Fecha_Fin = ultimafechainact.ElementAt(0).Fecha_Prorroga;
                    db.Entry(mt_PostVenta_edit_ffin).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Prorroga eliminada";
                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Prorroga_PostVenta", new { id = idPostVenta });
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

//Fiscalizador PostVenta
#region
[HttpGet]
public ActionResult Fiscalizador(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
            if (mT_PostVenta != null)
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                System.Web.HttpContext.Current.Session["id_PostVenta"] = mT_PostVenta.Id_PostVenta;
                var listaFiscalizadorC = db.sp_Quimipac_ConsultaMT_ContratoFiscalizador(id, "PostVenta", empresa_id.ToString()).ToList();
                var PostVenta = mT_PostVenta.Codigo_Cliente;
                ViewBag.listaFiscalizadorC = listaFiscalizadorC;
                ViewBag.PostVenta = PostVenta;
                var dbe = new DataBase_Externo();
                ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
                return View();
            }
            else
            {
                TempData["mensaje_error"] = "Código no existe";
                return RedirectToAction("PostVenta");
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine("postVenta/Fiscalizador"+e.Message);
            return RedirectToAction("Error", "Errores");
        }
    }
    else { return RedirectToAction("IniciarSesion", "Home"); }
}

[HttpGet]
public ActionResult AgregarFiscalizador_PostVenta()
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
public ActionResult AgregarFiscalizador_PostVenta([Bind(Include = "Id_Proyecto_Contrato, Nombre, Estado")] MT_Fiscalizador mT_Fiscalizador)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }
            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {
                    var PostVentaFiscalizador = db.MT_Fiscalizador.Where(x => x.Id_Proyecto_Contrato == mT_Fiscalizador.Id_Proyecto_Contrato && x.Nombre == mT_Fiscalizador.Nombre && x.Estado == mT_Fiscalizador.Estado);
                    if (PostVentaFiscalizador.Count() >= 1)
                    {
                        TempData["mensaje_error"] = "PostVenta Fiscalizador ya existe";
                        return RedirectToAction("Fiscalizador", new { id = idPostVenta });
                    }
                    else
                    {
                        mT_Fiscalizador.Tipo = "PostVenta";
                        mT_Fiscalizador.Id_Proyecto_Contrato = idPostVenta;
                        mT_Fiscalizador.Estado = mT_Fiscalizador.Estado.Substring(0, 1);
                        db.MT_Fiscalizador.Add(mT_Fiscalizador);
                        db.SaveChanges();
                        TempData["mensaje_correcto"] = "Fiscalizador de PostVenta guardado";
                        return RedirectToAction("Fiscalizador", new { id = idPostVenta });

                    }

                }

                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Fiscalizador", new { id = idPostVenta });
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
public ActionResult EditarFiscalizador_PostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            MT_Fiscalizador mT_PostVentaFiscalizador = db.MT_Fiscalizador.Find(id);
            bool seleccion = false;
            if (mT_PostVentaFiscalizador != null)
            {
                return View(mT_PostVentaFiscalizador);

            }
            else
            {

                var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

                if (id_PostVenta == null)
                {
                    return RedirectToAction("IniciarSesion", "Home");
                }

                else
                {
                    int idPostVenta = int.Parse(id_PostVenta.ToString());
                    TempData["mensaje_error"] = "Código no existe";
                    return RedirectToAction("Fiscalizador_PostVenta", new { id = idPostVenta });
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
public ActionResult EditarFiscalizador_PostVenta([Bind(Include = "Id_Fiscalizador, Id_Proyecto_Contrato, Nombre, Estado")] MT_Fiscalizador mT_FiscalizadorPostVenta)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {
            var id_PostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];
            // int bandera = 0;
            if (id_PostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_PostVenta.ToString());

                if (ModelState.IsValid)
                {


                    mT_FiscalizadorPostVenta.Estado = mT_FiscalizadorPostVenta.Estado.Substring(0, 1);
                    mT_FiscalizadorPostVenta.Tipo = "PostVenta";

                    db.Entry(mT_FiscalizadorPostVenta).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje_actualizado"] = "Fiscalizador de PostVenta actualizado";

                    return RedirectToAction("Fiscalizador", new { id = idPostVenta });

                }
                else
                {
                    TempData["mensaje_error"] = "Valores incorrectos";
                    return RedirectToAction("Fiscalizador", new { id = idPostVenta });
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

public ActionResult EliminarFiscalizadorPostVenta(int id)
{
    if (System.Web.HttpContext.Current.Session["usuario"] != null)
    {
        try
        {


            var id_MTPostVenta = System.Web.HttpContext.Current.Session["id_PostVenta"];

            if (id_MTPostVenta == null)
            {
                return RedirectToAction("IniciarSesion", "Home");
            }

            else
            {
                int idPostVenta = int.Parse(id_MTPostVenta.ToString());

                MT_Fiscalizador mT_Fiscalizador = db.MT_Fiscalizador.Find(id);

                if (mT_Fiscalizador != null)
                {
                    mT_Fiscalizador.Estado = "E";
                    db.Entry(mT_Fiscalizador).State = EntityState.Modified;
                    //db.MT_Formulario.Remove(mT_Formulario);
                    db.SaveChanges();
                    TempData["mensaje_correcto"] = "Fiscalizador eliminado";
                    return RedirectToAction("Fiscalizador", new { id = idPostVenta });
                }
                else
                {
                    TempData["mensaje_correcto"] = "Código no existe";
                    return RedirectToAction("Fiscalizador", new { id = idPostVenta });
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
public ActionResult Insertar_PostVentaSYSBASE(int PostVentaID)
{
    try
    {
        int id = PostVentaID;
        MT_PostVenta mT_PostVenta = db.MT_PostVenta.Find(id);
        DataBase_Externo databaseOrden = new DataBase_Externo();
                
        databaseOrden.InsertarPostVenta(mT_PostVenta.Id_PostVenta,mT_PostVenta.Id_Empresa, mT_PostVenta.Id_Cliente, mT_PostVenta.Fecha_Inicio, mT_PostVenta.Fecha_Fin, mT_PostVenta.Codigo_Interno,
        mT_PostVenta.Codigo_Cliente, mT_PostVenta.Id_Usuario, mT_PostVenta.Id_Linea, mT_PostVenta.Categoria, mT_PostVenta.Subcategoria, mT_PostVenta.Nombre, mT_PostVenta.Estado, mT_PostVenta.Dia_Plazo, mT_PostVenta.tipo, mT_PostVenta.Valor_Referencial,
        mT_PostVenta.monto, mT_PostVenta.costo, mT_PostVenta.Responsable, mT_PostVenta.Secuencial, mT_PostVenta.Codigo_Interno_Ant, mT_PostVenta.Observaciones, mT_PostVenta.Codigo_interno_padre, mT_PostVenta.Fecha_registro, mT_PostVenta.Fecha_modificacion, mT_PostVenta.Localidad, mT_PostVenta.Fecha_Aprobacion_Cot, mT_PostVenta.Recepcion_Servicio, mT_PostVenta.Fecha_Firma_Conformidad, mT_PostVenta.Fecha_Cumplimiento_Inst);


        TempData["mensaje_correcto"] = "PostVenta guardado";
        return RedirectToAction("postventa");
    }
    catch (Exception)
    {
        return RedirectToAction("Error", "Errores");
    }
}

public JsonResult GetPostVentaTipo_Estado(int id_tipo_Tipo)
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
        System.Diagnostics.Debug.WriteLine("/GetPostVentaTipo_Estado", e.Message);
        return Json(Response.StatusCode);
    }
}

public JsonResult GetPostVentaTipo_Estado_Edit(int id_PostVenta_editar, int id_tipo_Tipo)
{
    try
    {
        List<SelectListItem> itemsEstado_edit = new List<SelectListItem>();
        bool seleccion = false;
        MT_PostVenta mt_PostVenta = db.MT_PostVenta.Find(id_PostVenta_editar);
        var nombre_estado_selected = db.MT_TablaDetalle.Find(mt_PostVenta.Estado).Descripcion;

        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        string empresa = empresa_id.ToString();

        var listaEstadoTipo = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa, id_tipo_Tipo).ToList();

        //itemsEstado_edit.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                
        foreach (var estado in listaEstadoTipo)
        {
            if (estado.Id_TablaDetalle == mt_PostVenta.Estado)
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
        if(mt_PostVenta.tipo == id_tipo_Tipo)
        { 
        if (cont ==0)
        {
            seleccion = true;
            var obj = new MT_TablaDetalle
            {
                Id_TablaDetalle = Convert.ToInt32(mt_PostVenta.Estado),
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
        public ActionResult PostVenta_Verifar_funcion([Bind(Include = "Fecha_Inicio, Fecha_registro,Fecha_Fin, Origen,Tipo,Id_Cliente,PostVenta_Padre,Estado,Id_Linea,Categoria,Subcategoria, Localidad, Responsable")]sp_Quimipac_ConsultaMT_PostVentaGeneral_Result PostVenta)
        {
            try
            {
                var bde = new DataBase_Externo();
                var usuario = System.Web.HttpContext.Current.Session["usuario"];
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                var fecha_Inicio = PostVenta.Fecha_Inicio?.ToString("yyyy-MM-dd");
                var fecha_Registro = PostVenta.Fecha_registro?.ToString("yyyy-MM-dd");

                //var lkbusqueda = bde.BusquedaXParametro(ORDEN.Fecha_asignacion_grupo_trabajo, ORDEN.Fecha_registro, ORDEN.Origen, ORDEN.Id_tipo_trabajo_ejecutado, ORDEN.Id_campamento, ORDEN.Id_estacion, ORDEN.Id_sector, ORDEN.Id_sucursal, ORDEN.Id_PostVenta, ORDEN.Estado, ORDEN.cod_cli, ORDEN.Etapa, ORDEN.Id_OrdenTrabajo);
                var lkbusqueda = bde.BusquedaXParametro_PostVenta(fecha_Inicio, fecha_Registro, PostVenta.Fecha_Fin, PostVenta.Origen, PostVenta.tipo, PostVenta.Id_Cliente,  PostVenta.Estado, PostVenta.Id_Linea, PostVenta.Categoria, PostVenta.Subcategoria,  PostVenta.Localidad, PostVenta.Responsable);

                //var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "PostVentaS").ToList();
                //var listakPostVenta = db.MT_PostVenta.Where(vq => PostVentasLI.Contains(vq.Id_Cliente)).ToList();
                ////listakPostVenta = listakPostVenta.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();
                ///

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "POSTVENTA").ToList();
                lkbusqueda = lkbusqueda.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();


                TempData["listaFiltro"] = lkbusqueda;//"Tipo de Trabajo guardado";

                string XParametro = string.Empty;
                if (PostVenta.Origen.Equals("Ninguno")) { XParametro = ""; }
                else if (PostVenta.Origen.Equals("Tipo")) { XParametro = PostVenta.tipo.ToString(); }
                else if (PostVenta.Origen.Equals("Cliente")) { XParametro = PostVenta.Id_Cliente; }
                else if (PostVenta.Origen.Equals("Estado")) { XParametro = PostVenta.Estado.ToString(); }
                else if (PostVenta.Origen.Equals("Id_Linea")) { XParametro = PostVenta.Id_Linea; }
                else if (PostVenta.Origen.Equals("Categoria")) { XParametro = PostVenta.Categoria; }
                else if (PostVenta.Origen.Equals("Subcategoria")) { XParametro = PostVenta.Subcategoria; }
                else if (PostVenta.Origen.Equals("Localidad")) { XParametro = PostVenta.Localidad; }
                else if (PostVenta.Origen.Equals("Responsable")) { XParametro = PostVenta.Responsable.ToString(); }

               
                TempData["ParametroBusqueda"] = PostVenta.Fecha_Inicio + " " + PostVenta.Fecha_registro + " " + PostVenta.Origen + " " + XParametro;


                
                return RedirectToAction("PostVenta");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        [HttpPost]
        public ActionResult PostVenta_Filter(DateTime? Fecha_Inicio, DateTime? Fecha_registro, List<string> check_parametro, string SelectedAndOr, int? MenuSelectedFecha)
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

                var lkbusqueda = bde.execute_filter_postventa(Fecha_Inicio, Fecha_registro, check_parametro, SelectedAndOr, MenuSelectedFecha);

                var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "CLIENTES").ToList();

                lkbusqueda = lkbusqueda.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                var PostVentasLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(usuario.ToString(), empresa_id.ToString(), "POSTVENTA").ToList();
                lkbusqueda = lkbusqueda.Where(vq => PostVentasLI.Contains(vq.Id_PostVenta.ToString())).ToList();

                //DATA GET PostVenta
                //ViewBag.listaPostVenta = lkbusqueda;
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


                return RedirectToAction("PostVenta");//, "OrdenTrabajo");            sp_Quimipac_MT_OrdenTrabajo_QryXParametro  
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Errores");
            }
        }

        #endregion

    }
}