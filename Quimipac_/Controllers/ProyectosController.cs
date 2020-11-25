using System.Collections.Generic;
using System.Data.Entity;
using Quimipac_.Models;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Web;
using System;
using Quimipac_.Repositorio;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;

namespace Quimipac_.Controllers
{
    public class ProyectosController : Controller
    {


        //public string EstadoProrroga { get; set; }


        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

        //PROYECTOS
        #region
        [HttpGet]
        public ActionResult Proyectos()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                    string empresaCod = empresa_id.ToString();
                    string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                    
                    var listaProyecto = db.sp_Quimipac_ConsultaMT_ProyectoGeneral(empresaCod).OrderByDescending(x => x.Id_Proyecto).ToList();

                    var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresaCod, "CLIENTES").ToList();

                    listaProyecto = listaProyecto.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                    var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresaCod, "PROYECTOS").ToList();
                    listaProyecto = listaProyecto.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();

                    ViewBag.listaProyecto = listaProyecto;
                    var dbe = new DataBase_Externo();
                    ViewBag.MTPermiso_CONT = dbe.Item_OpcPermiso();
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
        public ActionResult Agregar_Proyectos()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                try
                {
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (user_id == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {

                        List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente = new List<SelectListItem>();
                        List<SelectListItem> itemsTipoTrabajo = new List<SelectListItem>();
                        List<SelectListItem> itemsLinea = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoHijo = new List<SelectListItem>();
                        List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsProspectos = new List<SelectListItem>();
                        List<SelectListItem> itemsProyectoPadre = new List<SelectListItem>();
                        //contrato
                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente_CTR = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoPadre = new List<SelectListItem>();
                        
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        //LkSucursal
                        var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                        itemsSucursal.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var sucursal in listaSucursal)
                        {
                            itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre });
                        }
                        SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");


                        //LkClientes
                        var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaClientes = listaClientes.Where(vq => ClienteLI.Contains(vq.cod_cli)).ToList();
                        

                        foreach (var cliente in listaClientes)
                        {
                            itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                        }
                        SelectList selectlistaClientes = new SelectList(itemsCliente, "Value", "Text");


                        //LktipoTrabajo
                        var listaTipoTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1, empresa_id.ToString()).Where(x=>x.Id_TipoTrabajo==5).ToList();
                        //itemsTipoTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var tipo in listaTipoTrabajo)
                        {
                            itemsTipoTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TipoTrabajo), Text = tipo.Descripcion });
                        }
                        SelectList selectlistaTipoTrabajo = new SelectList(itemsTipoTrabajo, "Value", "Text");


                        //LkLinea
                        var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
                        foreach (var linea in listaLinea)
                        {
                            itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                        }
                        SelectList selectLinea = new SelectList(itemsLinea, "Value", "Text");

                        //var listaContratoHijo = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).Where(x => x.tipo == 144 || x.tipo == 146).ToList();
                        var listaContratoHijo = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();
                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratoHijo = listaContratoHijo.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratoHijo = listaContratoHijo.Where(vq => ContratosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                        foreach (var contrato in listaContratoHijo)
                        {
                            itemsContratoHijo.Add(new SelectListItem { Value = Convert.ToString(contrato.Id_Contrato), Text = contrato.Nombre });
                        }

                        itemsContratoHijo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        SelectList selectlistaContratoHijo = new SelectList(itemsContratoHijo, "Value", "Text");

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
                                          

                        var listaProspecto = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).ToList();
                        //aquiiiii agregue
                        var ClienteLI4 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProspecto = listaProspecto.Where(vq => ClienteLI4.Contains(vq.Id_Cliente)).ToList();
                        var ProspectosLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                        listaProspecto = listaProspecto.Where(vq => ProspectosLI2.Contains(vq.Id_Prospecto.ToString())).ToList();
                        itemsProspectos.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var prospecto in listaProspecto)
                        {
                            itemsProspectos.Add(new SelectListItem { Value = Convert.ToString(prospecto.Id_Prospecto), Text = prospecto.Nombre });
                        }
                        SelectList selectlistaProspecto = new SelectList(itemsProspectos, "Value", "Text");

                        var listaProyectoPadre = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();
                        //aquiiiii agregue
                        var ClienteLI5 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProyectoPadre = listaProyectoPadre.Where(vq => ClienteLI5.Contains(vq.Id_Cliente)).ToList();
                        var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                        listaProyectoPadre = listaProyectoPadre.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                        itemsProyectoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var proyecto in listaProyectoPadre)
                        {
                            itemsProyectoPadre.Add(new SelectListItem { Value = Convert.ToString(proyecto.Id_Proyecto), Text = proyecto.Codigo_Cliente });
                        }
                        SelectList selectlistaProyectoPadre = new SelectList(itemsProyectoPadre, "Value", "Text");


                        //----------contrato
                        
                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == 144 || x.Id_TablaDetalle == 1152).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaEstados = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa_id.ToString(), listaTipo.ElementAt(0).Id_TablaDetalle).ToList();
                                            
                        foreach (var estado in listaEstados)
                        {
                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion });
                        }

                        SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");


                        var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                        var ClienteLI2_CTR = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listacliente = listacliente.Where(vq => ClienteLI2_CTR.Contains(vq.cod_cli)).ToList();

                        //itemsCliente.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var cliente in listacliente)
                        {
                            itemsCliente_CTR.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                        }

                        SelectList selectlistacliente = new SelectList(itemsCliente_CTR, "Value", "Text");

                        string empresa = empresa_id.ToString();
                        int id_tipo = listaTipo.ElementAt(0).Id_TablaDetalle;
                        string cliente_id = listacliente.ElementAt(0).cod_cli;

                        var listaContratoPadre = db.sp_Quimipac_ConsultaMT_Contrato_TipoPadre(empresa, id_tipo, cliente_id).ToList();

                        var ClienteLI_CTT = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratoPadre = listaContratoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI_CTR = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratoPadre = listaContratoPadre.Where(vq => ContratosLI_CTR.Contains(vq.Id_Contrato.ToString())).ToList();
                            //itemsContratoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var contratoPadre in listaContratoPadre)
                        {
                            itemsContratoPadre.Add(new SelectListItem { Value = Convert.ToString(contratoPadre.Id_Contrato), Text = contratoPadre.Codigo_Interno + " | " + contratoPadre.Nombre });
                        }
                        SelectList selectlistaContratoPadre = new SelectList(itemsContratoPadre, "Value", "Text");
                                              

                        //------

                        ViewBag.listaSucursal = selectlistaSucursal;
                        ViewBag.listaClientes = selectlistaClientes;
                        ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;
                        ViewBag.listaLinea = selectLinea;
                        ViewBag.listaContratoHijo = selectlistaContratoHijo;
                        ViewBag.listaCategoria = selectlistaCategoria;
                        ViewBag.listaSubcategoria = selectlistaSubCategoria;                        
                        ViewBag.listaProspecto = selectlistaProspecto;
                        ViewBag.listaProyectoPadre = selectlistaProyectoPadre;

                        //CONTRATO
                        ViewBag.listaEstados = selectlistaEstado;
                        ViewBag.listacliente = selectlistacliente;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaContratoPadre = selectlistaContratoPadre;
                        //---------------

                        return View();
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
        public ActionResult Agregar_Proyectos([Bind(Include = "Id_Sucursal,Id_Cliente,Id_TipoTrabajo,Fecha_Inicio,Fecha_Fin,Estado,Codigo_Interno,Codigo_Cliente,Direccion,Ubicacion,Comentario,Porcentaje_avance,Valor_Inicial,Valor_Ajustado,Linea,Categoria,Subcategoria, Id_contrato, Id_Prospecto, Proyecto_Padre, Valor_Ajustado_Aux, Valor_Inicial_Aux, Dia_Plazo")] MT_Proyecto mt_Proyecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    if (mt_Proyecto.Valor_Ajustado_Aux != null)
                    {
                        string cadValor_Ajustado = mt_Proyecto.Valor_Ajustado_Aux.ToString();
                        cadValor_Ajustado = cadValor_Ajustado.Replace(",", "");
                        cadValor_Ajustado = cadValor_Ajustado.Replace(".", ",");
                        mt_Proyecto.Valor_Ajustado = decimal.Parse(cadValor_Ajustado);
                    }
                    if (mt_Proyecto.Valor_Inicial_Aux != null)
                    {
                        string cadValor_Inicial = mt_Proyecto.Valor_Inicial_Aux.ToString();
                        cadValor_Inicial = cadValor_Inicial.Replace(",", "");
                        cadValor_Inicial = cadValor_Inicial.Replace(".", ",");
                        mt_Proyecto.Valor_Inicial = decimal.Parse(cadValor_Inicial);
                    }

                    if (ModelState.IsValid)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        //var proyecto = db.MT_Proyecto.Where(x => x.Codigo_Interno == mt_Proyecto.Codigo_Interno && x.Codigo_Cliente == mt_Proyecto.Codigo_Cliente);
                        var proyecto = db.MT_Proyecto.Where(x => x.Codigo_Cliente == mt_Proyecto.Codigo_Cliente && x.Categoria == mt_Proyecto.Categoria && x.Subcategoria == mt_Proyecto.Subcategoria && x.Linea == mt_Proyecto.Linea && x.Id_Cliente == mt_Proyecto.Id_Cliente && x.Id_Empresa == empresa_id.ToString()).ToList();

                        if (proyecto.Count() >= 1)
                        {
                            TempData["mensaje_error"] = "Proyecto ya registrado registrado";
                            return RedirectToAction("Proyectos");
                        }
                        else
                        {
                            var user_id = System.Web.HttpContext.Current.Session["usuario"];


                            if (user_id == null)
                            {
                                return Redirect(Url.Action("IniciarSesion", "Home"));
                            }
                            else {
                                var Codigo_tipo = "";

                                //if (mt_Proyecto.Id_contrato != null || mt_Proyecto.Id_contrato != 0)
                                //{
                                //    if (mt_Proyecto.Id_contrato != 0)
                                //    {
                                //        Codigo_tipo = "CTR";
                                //    }
                                //}
                                //else {
                                //    if (mt_Proyecto.Id_Prospecto != null || mt_Proyecto.Id_Prospecto != 0)
                                //    {
                                //        if (mt_Proyecto.Id_Prospecto != 0)
                                //        {
                                //            Codigo_tipo = "PRO";
                                //        }
                                //    }
                                    //else {
                                        Codigo_tipo = "PRY";
                                    //}
                                //}


                                int? secuencial = 0;

                                var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                                mt_Proyecto.Id_Empresa = empresa_id.ToString();

                                /*ojooo verificar como se puede traer los clientes segun permiso*/
                                var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == mt_Proyecto.Id_Cliente).SingleOrDefault();

                                /****var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == mt_Proyecto.tipo).SingleOrDefault();***/
                                var codigo_guardado = db.MT_Proyecto.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == mt_Proyecto.Id_Cliente && x.Linea == mt_Proyecto.Linea /*&& x.tipo == mt_Proyecto.tipo*/).ToList().Select(vq => new
                                {
                                    vq.Id_Proyecto,
                                    Fecha_Inicio = (vq.Fecha_Inicio != null ? DateTime.Parse(vq.Fecha_Inicio.ToString()) : DateTime.Today),//(vq.Fecha_Fin != null) ?(DateTime.Parse(vq.Fecha_Fin?.ToString())):null
                                    vq.Codigo_Interno,
                                    vq.Secuencial
                                });


                                int? SeqRegistro = 0;
                                int seqregistro2 = 0;
                                codigo_guardado = codigo_guardado.Where(vq => vq.Fecha_Inicio.Year == DateTime.Now.Year).OrderByDescending(vq => vq.Fecha_Inicio).Take(1).ToList();
                                if (codigo_guardado.Count() > 0)
                                {
                                    secuencial = (codigo_guardado.ElementAt(0).Secuencial);
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



                            mt_Proyecto.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + mt_Proyecto.Linea + Codigo_tipo + auxSeq + DateTime.Now.Year;
                            mt_Proyecto.Secuencial = seqregistro2;

                            mt_Proyecto.Id_Empresa = empresa_id.ToString();

                            mt_Proyecto.Estado = mt_Proyecto.Estado.Substring(0, 1);
                            mt_Proyecto.Fecha_Registro = DateTime.Now;
                            db.MT_Proyecto.Add(mt_Proyecto);
                            db.SaveChanges();

                            var obPermiso = new MT_Permiso
                            {
                                Aprobar = "S",
                                Consultar = "S",
                                Modificar = "S",
                                Crear = "S",
                                Eliminar = "N",
                                Estado = "A",
                                Fecha_Registro = DateTime.Now,
                                Id_Cliente = mt_Proyecto.Id_Cliente,
                                Id_Empresa = empresa_id.ToString(),
                                Id_Linea = null,
                                Id_Proyecto = mt_Proyecto.Id_Proyecto,
                                Id_Tipo_Trabajo = null,
                                Id_Usuario = user_id.ToString(),
                                Usuario = user_id.ToString(),
                                Id_Contrato = null,// db.MT_Contrato.Where(x=>x.)
                                Id_Permiso = 0
                            };
                            db.MT_Permiso.Add(obPermiso);
                            db.SaveChanges();

                            TempData["mensaje_correcto"] = "Proyecto guardado";
                            return RedirectToAction("Proyectos");
                        }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Proyectos");
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
        public ActionResult Editar_Proyectos(int id)
        {

            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                string UserID = System.Web.HttpContext.Current.Session["usuario"].ToString();

                try
                {
                    MT_Proyecto mt_Proyecto = db.MT_Proyecto.Find(id);

                    System.Web.HttpContext.Current.Session["contrato_interno"] = mt_Proyecto.Codigo_Interno;
                    System.Web.HttpContext.Current.Session["secuencial"] = mt_Proyecto.Secuencial;
                    System.Web.HttpContext.Current.Session["fecha_registro_cont"] = mt_Proyecto.Fecha_Registro;
                    System.Web.HttpContext.Current.Session["id_cliente"] = mt_Proyecto.Id_Cliente;
                    System.Web.HttpContext.Current.Session["id_linea"] = mt_Proyecto.Linea;
                    System.Web.HttpContext.Current.Session["id_tipo_trabajoP"] = mt_Proyecto.Id_TipoTrabajo;

                    mt_Proyecto.Valor_Ajustado_Aux = mt_Proyecto.Valor_Ajustado.ToString();
                    mt_Proyecto.Valor_Inicial_Aux = mt_Proyecto.Valor_Inicial.ToString();

                    System.Web.HttpContext.Current.Session["FechaRegistro"] = mt_Proyecto.Fecha_Registro;
                    System.Web.HttpContext.Current.Session["FechaProrroga"] = mt_Proyecto.Fecha_Prorroga;

                    var mt_ProyectoActividadProyectoFI = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaInicio").ToList();
                    var mt_ProyectoActividadProyectoFF = db.sp_Quimipac_ConsultaActividadesTTrabajoOrden(id, "fechaFin").ToList();
                    if (mt_ProyectoActividadProyectoFI.Count > 0)
                    {

                        foreach (var proyectoactividadproyectoFI in mt_ProyectoActividadProyectoFI)
                        {
                            if (proyectoactividadproyectoFI.Id_Orden_Trabajo == mt_Proyecto.Id_Proyecto)
                            {
                                System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] = proyectoactividadproyectoFI.Fecha_Ini;

                            }
                        }
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] = null;

                    }

                    if (mt_ProyectoActividadProyectoFF.Count > 0)
                    {

                        foreach (var proyectooactividadproyectoFF in mt_ProyectoActividadProyectoFF)
                        {
                            if (proyectooactividadproyectoFF.Id_Orden_Trabajo == mt_Proyecto.Id_Proyecto)
                            {
                                System.Web.HttpContext.Current.Session["FechaFinEjecucion"] = proyectooactividadproyectoFF.Fecha_Fin;

                            }
                        }
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["FechaFinEjecucion"] = null;

                    }


                    bool seleccion = false;
                    if (mt_Proyecto != null)
                    {
                        List<SelectListItem> itemsSucursal = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente = new List<SelectListItem>();
                        List<SelectListItem> itemsTipoTrabajo = new List<SelectListItem>();
                        List<SelectListItem> itemsLinea = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoHijo = new List<SelectListItem>();
                        List<SelectListItem> itemsCategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsSubcategoria = new List<SelectListItem>();
                        List<SelectListItem> itemsProspecto = new List<SelectListItem>();
                        List<SelectListItem> itemsProyectoPadre = new List<SelectListItem>();

                        //contrato
                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsCliente_CTR = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();
                        List<SelectListItem> itemsContratoPadre = new List<SelectListItem>();

                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaSucursal = db.sp_LINK_ConsultaSucursal(empresa_id.ToString()).ToList();
                        itemsSucursal.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var sucursal in listaSucursal)
                        {
                            if (sucursal.cod_suc == mt_Proyecto.Id_Sucursal)
                            {
                                seleccion = true;
                            }

                            itemsSucursal.Add(new SelectListItem { Value = Convert.ToString(sucursal.cod_suc), Text = sucursal.nombre });
                        }
                        SelectList selectlistaSucursal = new SelectList(itemsSucursal, "Value", "Text");


                        //LkClientes
                        var listaClientes = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                        var ClienteLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaClientes = listaClientes.Where(vq => ClienteLI.Contains(vq.cod_cli)).ToList();
                        

                        foreach (var cliente in listaClientes)
                        {
                            if (cliente.cod_cli == mt_Proyecto.Id_Cliente)
                            {
                                seleccion = true;
                            }

                            itemsCliente.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                        }
                        SelectList selectlistaClientes = new SelectList(itemsCliente, "Value", "Text");


                        //LktipoTrabajo
                        var listaTipoTrabajo = db.sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo(1, empresa_id.ToString()).Where(x=>x.Id_TipoTrabajo==5).ToList();
                        //itemsTipoTrabajo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var tipo in listaTipoTrabajo)
                        {
                            if (tipo.Id_TipoTrabajo == mt_Proyecto.Id_TipoTrabajo)
                            {
                                seleccion = true;
                            }

                            itemsTipoTrabajo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TipoTrabajo), Text = tipo.Descripcion });
                        }
                        SelectList selectlistaTipoTrabajo = new SelectList(itemsTipoTrabajo, "Value", "Text");


                        //LkLinea
                        var listaLinea = db.sp_LINK_ConsultaLineas(empresa_id.ToString()).ToList();
                        foreach (var linea in listaLinea)
                        {
                            if (linea.codigo == mt_Proyecto.Linea)
                            {
                                seleccion = true;
                            }

                            itemsLinea.Add(new SelectListItem { Value = Convert.ToString(linea.codigo), Text = linea.descripcion });
                        }
                        SelectList selectLinea = new SelectList(itemsLinea, "Value", "Text");

                        //mt_Proyecto.Fecha_Inicio = ;


                        // MT_ProyectoPro mt_ProyectoPro =

                        //var listaCntratoHijo = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).Where(x => x.tipo == 144 || x.tipo == 146).ToList();
                        var listaCntratoHijo = db.sp_Quimipac_ConsultaMT_Contrato(empresa_id.ToString()).ToList();

                        var ClienteLI2 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaCntratoHijo = listaCntratoHijo.Where(vq => ClienteLI2.Contains(vq.Id_Cliente)).ToList();
                        var ContratostosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaCntratoHijo = listaCntratoHijo.Where(vq => ContratostosLI.Contains(vq.Id_Contrato.ToString())).ToList();
                        itemsContratoHijo.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var contratohijo in listaCntratoHijo)
                        {
                            if (contratohijo.Id_Contrato == mt_Proyecto.Id_contrato)
                            {
                                seleccion = true;
                            }

                            itemsContratoHijo.Add(new SelectListItem { Value = Convert.ToString(contratohijo.Id_Contrato), Text = contratohijo.Nombre });
                        }
                        SelectList selectlistaContratoHijo = new SelectList(itemsContratoHijo, "Value", "Text");

                        var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), mt_Proyecto.Linea).ToList();
                        itemsCategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });

                        foreach (var categoria in listaCategoria)
                        {
                            if (categoria.cod_servicio == mt_Proyecto.Categoria)
                            {
                                seleccion = true;
                            }

                            itemsCategoria.Add(new SelectListItem { Value = Convert.ToString(categoria.cod_servicio), Text = categoria.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaCategoria = new SelectList(itemsCategoria, "Value", "Text");

                        var listaSubcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), mt_Proyecto.Linea).ToList();
                        itemsSubcategoria.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var subcategoria in listaSubcategoria)
                        {
                            if (subcategoria.codcen == mt_Proyecto.Subcategoria)
                            {
                                seleccion = true;
                            }

                            itemsSubcategoria.Add(new SelectListItem { Value = Convert.ToString(subcategoria.codcen), Text = subcategoria.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaSubcategoria = new SelectList(itemsSubcategoria, "Value", "Text");

                        var listaProspecto = db.sp_Quimipac_ConsultaMT_Prospecto(empresa_id.ToString()).ToList();

                        var ClienteLI3 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProspecto = listaProspecto.Where(vq => ClienteLI3.Contains(vq.Id_Cliente)).ToList();
                        var ProspectostosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROSPECTOS").ToList();
                        listaProspecto = listaProspecto.Where(vq => ProspectostosLI.Contains(vq.Id_Prospecto.ToString())).ToList();
                        itemsProspecto.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var prospectohijo in listaProspecto)
                        {
                            if (prospectohijo.Id_Prospecto == mt_Proyecto.Id_Prospecto)
                            {
                                seleccion = true;
                            }

                            itemsProspecto.Add(new SelectListItem { Value = Convert.ToString(prospectohijo.Id_Prospecto), Text = prospectohijo.Nombre });
                        }
                        SelectList selectlistaProspecto = new SelectList(itemsProspecto, "Value", "Text");

                        var listaProyectoPadre = db.sp_Quimipac_ConsultaMT_Proyecto(empresa_id.ToString()).ToList();

                        var ClienteLI4 = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaProyectoPadre = listaProyectoPadre.Where(vq => ClienteLI4.Contains(vq.Id_Cliente)).ToList();
                        var ProyectosLI = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "PROYECTOS").ToList();
                        listaProyectoPadre = listaProyectoPadre.Where(vq => ProyectosLI.Contains(vq.Id_Proyecto.ToString())).ToList();
                        itemsProyectoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var proyectopadre in listaProyectoPadre)
                        {
                            if (proyectopadre.Id_Proyecto == mt_Proyecto.Id_Proyecto)
                            {
                                seleccion = true;
                            }

                            itemsProyectoPadre.Add(new SelectListItem { Value = Convert.ToString(proyectopadre.Id_Proyecto), Text = proyectopadre.Codigo_Cliente });
                        }
                        SelectList selectlistaProyectoPadre = new SelectList(itemsProyectoPadre, "Value", "Text");

                        //----------contrato

                        var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == 144 || x.Id_TablaDetalle == 1152).ToList();
                        foreach (var tipo in listaTipo)
                        {
                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                        }

                        SelectList selectlistaTipo = new SelectList(itemsTipo, "Value", "Text");

                        var listaEstados = db.sp_Quimipac_ConsultaMT_Contrato_TipoEstado(empresa_id.ToString(), listaTipo.ElementAt(0).Id_TablaDetalle).ToList();

                        foreach (var estado in listaEstados)
                        {
                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion });
                        }

                        SelectList selectlistaEstado = new SelectList(itemsEstado, "Value", "Text");


                        var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).ToList();

                        var ClienteLI2_CTR = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listacliente = listacliente.Where(vq => ClienteLI2_CTR.Contains(vq.cod_cli)).ToList();

                        //itemsCliente.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var cliente in listacliente)
                        {
                            itemsCliente_CTR.Add(new SelectListItem { Value = Convert.ToString(cliente.cod_cli), Text = cliente.nom_cli });
                        }

                        SelectList selectlistacliente = new SelectList(itemsCliente_CTR, "Value", "Text");

                        string empresa = empresa_id.ToString();
                        int id_tipo = listaTipo.ElementAt(0).Id_TablaDetalle;
                        string cliente_id = listacliente.ElementAt(0).cod_cli;

                        var listaContratoPadre = db.sp_Quimipac_ConsultaMT_Contrato_TipoPadre(empresa, id_tipo, cliente_id).ToList();

                        var ClienteLI_CTT = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CLIENTES").ToList();

                        listaContratoPadre = listaContratoPadre.Where(vq => ClienteLI.Contains(vq.Id_Cliente)).ToList();
                        var ContratosLI_CTR = db.sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID(UserID, empresa_id.ToString(), "CONTRATOS").ToList();
                        listaContratoPadre = listaContratoPadre.Where(vq => ContratosLI_CTR.Contains(vq.Id_Contrato.ToString())).ToList();
                        //itemsContratoPadre.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguno" });
                        foreach (var contratoPadre in listaContratoPadre)
                        {
                            itemsContratoPadre.Add(new SelectListItem { Value = Convert.ToString(contratoPadre.Id_Contrato), Text = contratoPadre.Codigo_Interno + " | " + contratoPadre.Nombre });
                        }
                        SelectList selectlistaContratoPadre = new SelectList(itemsContratoPadre, "Value", "Text");


                        //------


                        ViewBag.listaSucursal = selectlistaSucursal;
                        ViewBag.listaClientes = selectlistaClientes;
                        ViewBag.listaTipoTrabajo = selectlistaTipoTrabajo;
                        ViewBag.listaLinea = selectLinea;
                        ViewBag.listaContratoHijo = selectlistaContratoHijo;
                        ViewBag.listaCategoria = selectlistaCategoria;
                        ViewBag.listaSubcategoria = selectlistaSubcategoria;
                        ViewBag.listaProspecto = selectlistaProspecto;
                        ViewBag.listaProyectoPadre = selectlistaProyectoPadre;

                        var proyecto_codedit = mt_Proyecto.Codigo_Cliente;
                        
                        ViewBag.proyecto_codedit = proyecto_codedit;
                        ViewBag.id_proyecto_Editar = id;

                        //CONTRATO
                        ViewBag.listaEstados = selectlistaEstado;
                        ViewBag.listacliente = selectlistacliente;
                        ViewBag.listaTipo = selectlistaTipo;
                        ViewBag.listaContratoPadre = selectlistaContratoPadre;
                        //---------------


                        return View(mt_Proyecto);

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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


        //Editar Datos Modal

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Proyectos([Bind(Include = "Id_Proyecto,Id_Sucursal,Id_Cliente,Id_TipoTrabajo,Fecha_Inicio,Fecha_Fin,Fecha_Prorroga,EstadoProrroga,Estado,Codigo_Interno,Codigo_Cliente,Direccion,Ubicacion,Comentario,Porcentaje_avance,Valor_Inicial,Valor_Ajustado,Linea,Categoria,Subcategoria,Id_contrato, Id_Prospecto, Proyecto_Padre, Valor_Ajustado_Aux, Valor_Inicial_Aux, Dia_Plazo")] MT_Proyecto mT_Proyecto)
        {
            var codigointernoproyecto = System.Web.HttpContext.Current.Session["contrato_interno"].ToString();
            int secuencial = Convert.ToInt32(System.Web.HttpContext.Current.Session["secuencial"].ToString());
            var fecha_registro = System.Web.HttpContext.Current.Session["fecha_registro_cont"];
            var id_cliente_edit = System.Web.HttpContext.Current.Session["id_cliente"].ToString();
            var id_linea_edit = System.Web.HttpContext.Current.Session["id_linea"].ToString();
            var id_tipo_trabajo = System.Web.HttpContext.Current.Session["id_tipo_trabajoP"].ToString();


            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    if (mT_Proyecto.Valor_Ajustado_Aux != null)
                    {
                        string cadMonto = mT_Proyecto.Valor_Ajustado_Aux.ToString();
                        cadMonto = cadMonto.Replace(",", "");
                        cadMonto = cadMonto.Replace(".", ",");
                        mT_Proyecto.Valor_Ajustado = decimal.Parse(cadMonto);
                    }
                    if (mT_Proyecto.Valor_Inicial_Aux != null)
                    {
                        string cadValorReferencial = mT_Proyecto.Valor_Inicial_Aux.ToString();
                        cadValorReferencial = cadValorReferencial.Replace(",", "");
                        cadValorReferencial = cadValorReferencial.Replace(".", ",");
                        mT_Proyecto.Valor_Inicial = decimal.Parse(cadValorReferencial);
                    }
                    

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

                            int retornaTO = databaseOrden.GetTipoTrabajoProyecto(mT_Proyecto.Id_Proyecto, mT_Proyecto.Id_TipoTrabajo);
                            if (retornaTO == 1)
                            {
                                mT_Proyecto.Id_TipoTrabajo = Convert.ToInt32(id_tipo_trabajo);
                            }
                            else
                            {
                                mT_Proyecto.Id_TipoTrabajo = mT_Proyecto.Id_TipoTrabajo;
                                var mt_proyectoactividadEditar = db.MT_Proyecto_Actividad.Where(x=>x.Id_Proyecto==mT_Proyecto.Id_Proyecto).ToList();
                                foreach (var item in mt_proyectoactividadEditar)
                                {
                                    var status = false;

                                    using (BD_QUIMIPACEntities dc = new BD_QUIMIPACEntities())
                                    {                                      
                                        
                                            var t = dc.MT_Proyecto_Actividad.Find(item.Id_Proyecto_Actividad);
                                            t.EstadoAct = "E";
                                            dc.SaveChanges();
                                            status = true;
                                        
                                    }
                                }
                                //mt_proyectoactividadEditar.ElementAt(0).EstadoAct = "E";
                                //db.Entry(mt_proyectoactividadEditar).State = EntityState.Modified;
                                //db.SaveChanges();
                            }

                            var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
                                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                                var id_cliente_nuevo = mT_Proyecto.Id_Cliente;
                                var id_linea_nuevo = mT_Proyecto.Linea;
                                var categoria_nueva = mT_Proyecto.Categoria;
                                var subcategoria_nueva = mT_Proyecto.Subcategoria;                            

                            if (mT_Proyecto.Id_Cliente == id_cliente_edit && mT_Proyecto.Linea == id_linea_edit)
                                {
                                

                                    mT_Proyecto.Id_Empresa = empresa_id.ToString();
                                    mT_Proyecto.Secuencial = secuencial;


                                    mT_Proyecto.Fecha_Registro = Convert.ToDateTime(fecha_registro);
                                    //mT_Proyecto.Fecha_modificacion = DateTime.Now;
                                    mT_Proyecto.Id_Empresa = empresa_id.ToString();
                                    mT_Proyecto.Fecha_Registro = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaRegistro"]);
                                    //Estado Proyecto
                                    mT_Proyecto.Estado = mT_Proyecto.Estado.Substring(0, 1);

                                    if (System.Web.HttpContext.Current.Session["FechaInicioEjecucion"] != null)
                                    {
                                        mT_Proyecto.Fecha_Inicio = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaInicioEjecucion"]);
                                    }

                                    if (System.Web.HttpContext.Current.Session["FechaFinEjecucion"] != null)
                                    {
                                        mT_Proyecto.Fecha_Fin = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucion"]);
                                    }

                                    //Estado Prorroga Proyecto  public string EstadoProrroga { get; set; }
                                    if (mT_Proyecto.EstadoProrroga.Equals("Inactivo"))
                                    {
                                        mT_Proyecto.Fecha_Prorroga = null;
                                    }

                                    /*Cambio Repositorio*/
                                    db.Entry(mT_Proyecto).State = EntityState.Modified;
                                    db.SaveChanges();

                                    if (mT_Proyecto.Fecha_Prorroga != null)//|| mT_Proyecto.Fecha_Prorroga != "")
                                    {
                                        var LkProyectoActual = db.MT_Proyecto.Where(v => v.Fecha_Registro == mT_Proyecto.Fecha_Registro && v.Id_Empresa == mT_Proyecto.Id_Empresa && v.Id_Sucursal == mT_Proyecto.Id_Sucursal && v.Id_Cliente == mT_Proyecto.Id_Cliente && v.Id_TipoTrabajo == mT_Proyecto.Id_TipoTrabajo && v.Linea == mT_Proyecto.Linea && v.Categoria == mT_Proyecto.Categoria && v.Subcategoria == mT_Proyecto.Subcategoria && v.Codigo_Interno == mT_Proyecto.Codigo_Interno && v.Codigo_Cliente == mT_Proyecto.Codigo_Cliente);
                                        int IdProyActual = 0;
                                        foreach (var item in LkProyectoActual)
                                        {
                                            IdProyActual = item.Id_Proyecto;
                                        }


                                        var lkProyProrroga = db.MT_Proyecto_Prorroga.Where(prorroga => prorroga.Id_Proyecto == IdProyActual);
                                        if (lkProyProrroga.Count() > 0)
                                        {
                                            foreach (var itemProrroga in lkProyProrroga)
                                            {
                                                itemProrroga.Estado = itemProrroga.Estado.Replace("A", "I");
                                            }
                                            db.SaveChanges();

                                        }

                                        //...
                                        MT_Proyecto_Prorroga TBProyProrroga = new MT_Proyecto_Prorroga()
                                        {
                                            Id_Proyecto = IdProyActual,
                                            Fecha_Prorroga = mT_Proyecto.Fecha_Prorroga,
                                            Estado = mT_Proyecto.EstadoProrroga.Substring(0, 1)
                                        };

                                        var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                                        objectContext.AddObject("MT_Proyecto_Prorroga", TBProyProrroga);
                                        db.SaveChanges();
                                        //..

                                    }
                                    else
                                    {
                                        var LkProyectoActual = db.MT_Proyecto.Where(v => v.Fecha_Registro == mT_Proyecto.Fecha_Registro && v.Id_Empresa == mT_Proyecto.Id_Empresa && v.Id_Sucursal == mT_Proyecto.Id_Sucursal && v.Id_Cliente == mT_Proyecto.Id_Cliente && v.Id_TipoTrabajo == mT_Proyecto.Id_TipoTrabajo && v.Linea == mT_Proyecto.Linea && v.Categoria == mT_Proyecto.Categoria && v.Subcategoria == mT_Proyecto.Subcategoria && v.Codigo_Interno == mT_Proyecto.Codigo_Interno && v.Codigo_Cliente == mT_Proyecto.Codigo_Cliente);
                                        int IdProyActual = 0;
                                        foreach (var item in LkProyectoActual)
                                        {
                                            IdProyActual = item.Id_Proyecto;
                                        }

                                        var lkProyProrroga = db.MT_Proyecto_Prorroga.Where(prorroga => prorroga.Id_Proyecto == IdProyActual && prorroga.Estado == "A");
                                        if (lkProyProrroga.Count() > 0)
                                        {
                                            foreach (var itemProrroga in lkProyProrroga)
                                            {
                                                if (itemProrroga.Estado.Equals("A"))
                                                {
                                                    itemProrroga.Estado = itemProrroga.Estado.Replace("A", "I");
                                                }

                                            }
                                            db.SaveChanges();
                                        }
                                    }

                                    TempData["mensaje_actualizado"] = "Proyecto actualizado";
                                }
                                else
                                {


                                    MT_Proyecto mt_proyectoEditar = db.MT_Proyecto.Find(mT_Proyecto.Id_Proyecto);
                                    mt_proyectoEditar.Estado = "E";
                                    db.Entry(mt_proyectoEditar).State = EntityState.Modified;
                                    db.SaveChanges();
                                    int? secuencial_nuevo = 0;
                                    var listacliente = db.sp_LINK_ConsultaClientes(empresa_id.ToString()).Where(x => x.cod_cli == id_cliente_nuevo).SingleOrDefault();

                                    //var listaTipo = db.sp_Quimipac_ConsultaMT_TablaDetalle(63).Where(x => x.Id_TablaDetalle == id_tipo_nuevo).SingleOrDefault();
                                    var codigo_guardado = db.MT_Proyecto.Where(x => x.Id_Empresa == empresa_id.ToString() && x.Id_Cliente == id_cliente_nuevo && x.Linea == id_linea_nuevo /*&& x.tipo == id_tipo_nuevo*/).ToList().Select(vq => new
                                    {
                                        vq.Id_Proyecto,
                                        Fecha_Inicio = (vq.Fecha_Inicio != null ? DateTime.Parse(vq.Fecha_Inicio.ToString()) : DateTime.Today),//(vq.Fecha_Fin != null) ?(DateTime.Parse(vq.Fecha_Fin?.ToString())):null
                                        vq.Codigo_Interno,
                                        vq.Secuencial
                                    });

                                    var Codigo_tipo = "";

                                    //if (mT_Proyecto.Id_contrato != null || mT_Proyecto.Id_contrato != 0)
                                    //{
                                    //    Codigo_tipo = "CTR";
                                    //}
                                    //else
                                    //{
                                    //    if (mT_Proyecto.Id_Prospecto != null || mT_Proyecto.Id_Prospecto != 0)
                                    //    {
                                    //        Codigo_tipo = "PRO";
                                    //    }
                                    //    else
                                    //    {
                                            Codigo_tipo = "PRY";
                                    //    }
                                    //}
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



                                    mT_Proyecto.Codigo_Interno = nom_empresa.Substring(0, 3) + listacliente.abreviatura_cliente + id_linea_nuevo + Codigo_tipo + auxSeq + DateTime.Now.Year;
                                    mT_Proyecto.Secuencial = seqregistro2;


                                    mT_Proyecto.Fecha_Registro = DateTime.Now;
                                    mT_Proyecto.Id_Empresa = empresa_id.ToString();
                                    //mT_Proyecto.Id_Usuario = user_id.ToString();
                                    mT_Proyecto.Id_Cliente = id_cliente_nuevo;
                                    mT_Proyecto.Linea = id_linea_nuevo;
                                    //mT_Proyecto.tipo = id_tipo_nuevo;
                                    mT_Proyecto.Estado = "A";
                                    mT_Proyecto.Categoria = categoria_nueva;
                                    mT_Proyecto.Subcategoria = subcategoria_nueva;
                                    db.MT_Proyecto.Add(mT_Proyecto);
                                    db.SaveChanges();

                                    if (mT_Proyecto.Fecha_Prorroga != null)//|| mT_Proyecto.Fecha_Prorroga != "")
                                    {
                                        var LkProyectoActual = db.MT_Proyecto.Where(v => v.Fecha_Registro == mT_Proyecto.Fecha_Registro && v.Id_Empresa == mT_Proyecto.Id_Empresa && v.Id_Sucursal == mT_Proyecto.Id_Sucursal && v.Id_Cliente == mT_Proyecto.Id_Cliente && v.Id_TipoTrabajo == mT_Proyecto.Id_TipoTrabajo && v.Linea == mT_Proyecto.Linea && v.Categoria == mT_Proyecto.Categoria && v.Subcategoria == mT_Proyecto.Subcategoria && v.Codigo_Interno == mT_Proyecto.Codigo_Interno && v.Codigo_Cliente == mT_Proyecto.Codigo_Cliente);
                                        int IdProyActual = 0;
                                        foreach (var item in LkProyectoActual)
                                        {
                                            IdProyActual = item.Id_Proyecto;
                                        }


                                        var lkProyProrroga = db.MT_Proyecto_Prorroga.Where(prorroga => prorroga.Id_Proyecto == IdProyActual);
                                        if (lkProyProrroga.Count() > 0)
                                        {
                                            foreach (var itemProrroga in lkProyProrroga)
                                            {
                                                itemProrroga.Estado = itemProrroga.Estado.Replace("A", "I");
                                            }
                                            db.SaveChanges();

                                        }

                                        //...
                                        MT_Proyecto_Prorroga TBProyProrroga = new MT_Proyecto_Prorroga()
                                        {
                                            Id_Proyecto = IdProyActual,
                                            Fecha_Prorroga = mT_Proyecto.Fecha_Prorroga,
                                            Estado = mT_Proyecto.EstadoProrroga.Substring(0, 1)
                                        };

                                        var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                                        objectContext.AddObject("MT_Proyecto_Prorroga", TBProyProrroga);
                                        db.SaveChanges();
                                        //..

                                    }
                                    else
                                    {
                                        var LkProyectoActual = db.MT_Proyecto.Where(v => v.Fecha_Registro == mT_Proyecto.Fecha_Registro && v.Id_Empresa == mT_Proyecto.Id_Empresa && v.Id_Sucursal == mT_Proyecto.Id_Sucursal && v.Id_Cliente == mT_Proyecto.Id_Cliente && v.Id_TipoTrabajo == mT_Proyecto.Id_TipoTrabajo && v.Linea == mT_Proyecto.Linea && v.Categoria == mT_Proyecto.Categoria && v.Subcategoria == mT_Proyecto.Subcategoria && v.Codigo_Interno == mT_Proyecto.Codigo_Interno && v.Codigo_Cliente == mT_Proyecto.Codigo_Cliente);
                                        int IdProyActual = 0;
                                        foreach (var item in LkProyectoActual)
                                        {
                                            IdProyActual = item.Id_Proyecto;
                                        }

                                        var lkProyProrroga = db.MT_Proyecto_Prorroga.Where(prorroga => prorroga.Id_Proyecto == IdProyActual && prorroga.Estado == "A");
                                        if (lkProyProrroga.Count() > 0)
                                        {
                                            foreach (var itemProrroga in lkProyProrroga)
                                            {
                                                if (itemProrroga.Estado.Equals("A"))
                                                {
                                                    itemProrroga.Estado = itemProrroga.Estado.Replace("A", "I");
                                                }

                                            }
                                            db.SaveChanges();
                                        }
                                    }



                                    var obPermiso = new MT_Permiso
                                    {
                                        Aprobar = "S",
                                        Consultar = "S",
                                        Modificar = "S",
                                        Crear = "S",
                                        Eliminar = "N",
                                        Estado = "A",
                                        Fecha_Registro = DateTime.Now,
                                        Id_Cliente = mT_Proyecto.Id_Cliente,
                                        Id_Empresa = empresa_id.ToString(),
                                        Id_Linea = null,
                                        Id_Proyecto = mT_Proyecto.Id_Proyecto,
                                        Id_Tipo_Trabajo = null,
                                        Id_Usuario = user_id.ToString(),
                                        Usuario = user_id.ToString(),
                                        Id_Contrato = null,// db.MT_Contrato.Where(x=>x.)
                                        Id_Permiso = 0
                                    };
                                    db.MT_Permiso.Add(obPermiso);
                                    db.SaveChanges();
                                    //var dbe = new DataBase_Externo();
                                    //dbe.Insertar_ContratoParametro(mT_Contrato.Id_Contrato);
                                                                       

                                    TempData["mensaje_correcto"] = "Proyecto anterior eliminado porque cambio el cliente, la linea o el tipo, Proyecto nuevo guardado";

                                }


                                
                                
                            
                            

                            return RedirectToAction("Proyectos");
                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Proyectos");
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

        //Eliminar
        [HttpGet]
        public ActionResult EliminarProyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    MT_Proyecto mt_Proyecto = db.MT_Proyecto.Find(id);

                    mt_Proyecto.Estado = "E";

                    db.Entry(mt_Proyecto).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["mensaje_actualizado"] = "Proyecto Eliminado";
                    return RedirectToAction("Proyectos");
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

        public JsonResult GetCategoria(string id)
        {
            try
            {
                id = id.Replace("ampersand", "&");
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


        public JsonResult GetSubCategoria(string id)
        {
            try
            {
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

        public JsonResult GetContratoPadreMonto(int id_ContratoPadre_Change, int id_contrato_Editar)
        {
            try
            {
                //List<SelectListItem> itemsContratosTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var mensaje = new JsonResult();
                var lista_camp_ContratoPadre = db.MT_Contrato.Where(x => x.Id_Contrato == id_ContratoPadre_Change).ToList().Select(vq => new
                {
                    
                    vq.Valor_Referencial,
                    vq.monto,
                    vq.Fecha_Inicio,
                    vq.Dia_Plazo,
                    vq.Fecha_Fin,
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria
                });
                var valr_ref_ = lista_camp_ContratoPadre.ElementAt(0).Valor_Referencial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");

                var valr_monto_ = lista_camp_ContratoPadre.ElementAt(0).monto;
                string auxValorMonto = valr_monto_.ToString();
                auxValorMonto = auxValorMonto.Replace(".", ",");

                var fechaI = DateTime.Parse(lista_camp_ContratoPadre.ElementAt(0).Fecha_Inicio.ToString());


                var listaCampos_CtrPadre = new List<MT_Contrato>();

                var id_linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_ContratoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ContratoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();


                //cod_linea  lo utilizo com ocampo auxiliar

                listaCampos_CtrPadre.Add(new MT_Contrato
                {
                    
                    Valor_Referencial_Aux = auxValor,
                    Monto_Aux = auxValorMonto,
                    Fecha_Inicio = fechaI,
                    Dia_Plazo = lista_camp_ContratoPadre.ElementAt(0).Dia_Plazo,
                    Fecha_Fin = lista_camp_ContratoPadre.ElementAt(0).Fecha_Fin,
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

                    vq.Valor_Referencial,
                    vq.monto,
                    vq.Fecha_Inicio,
                    vq.Dia_Plazo,
                    vq.Fecha_Fin,
                    vq.Id_Linea,
                    vq.Categoria,
                    vq.Subcategoria
                });
                var valr_ref_ = lista_camp_ContratoPadre.ElementAt(0).Valor_Referencial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");

                var valr_monto_ = lista_camp_ContratoPadre.ElementAt(0).monto;
                string auxValorMonto = valr_monto_.ToString();
                auxValorMonto = auxValorMonto.Replace(".", ",");

                var fechaI = DateTime.Parse(lista_camp_ContratoPadre.ElementAt(0).Fecha_Inicio.ToString());


                var listaCampos_CtrPadre = new List<MT_Prospecto>();


                var id_linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea;
                var id_Cat = lista_camp_ContratoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ContratoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();


                //cod_linea  lo utilizo com ocampo auxiliar

                listaCampos_CtrPadre.Add(new MT_Prospecto
                {

                    Valor_Referencial_Aux = auxValor,
                    Monto_Aux = auxValorMonto,
                    Fecha_Inicio = fechaI,
                    Dia_Plazo = lista_camp_ContratoPadre.ElementAt(0).Dia_Plazo,
                    Fecha_Fin = lista_camp_ContratoPadre.ElementAt(0).Fecha_Fin,
                    Id_Linea = lista_camp_ContratoPadre.ElementAt(0).Id_Linea,
                    Categoria = lista_camp_ContratoPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_ContratoPadre.ElementAt(0).Subcategoria,
                    //Valor_Referencial_Aux = auxValor,
                    lineaProspecto = nombre_linea.ToString(),
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

        public JsonResult GetProyectoPadreMonto(int id_ProyectoPadre_Change, int id_proyecto_Editar)
        {
            try
            {
                //List<SelectListItem> itemsContratosTipo = new List<SelectListItem>();
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var mensaje = new JsonResult();
                var lista_camp_ProyectoPadre = db.MT_Proyecto.Where(x => x.Id_Proyecto == id_ProyectoPadre_Change).ToList().Select(vq => new
                {

                    vq.Valor_Ajustado,
                    vq.Valor_Inicial,
                    vq.Fecha_Inicio,
                    vq.Dia_Plazo,
                    vq.Fecha_Fin,
                    vq.Linea,
                    vq.Categoria,
                    vq.Subcategoria
                });
                var valr_ref_ = lista_camp_ProyectoPadre.ElementAt(0).Valor_Inicial;
                string auxValor = valr_ref_.ToString();
                auxValor = auxValor.Replace(".", ",");

                var valr_monto_ = lista_camp_ProyectoPadre.ElementAt(0).Valor_Ajustado;
                string auxValorMonto = valr_monto_.ToString();
                auxValorMonto = auxValorMonto.Replace(".", ",");

                var fechaI = DateTime.Parse(lista_camp_ProyectoPadre.ElementAt(0).Fecha_Inicio.ToString());


                var listaCampos_CtrPadre = new List<MT_Contrato>();

                var id_linea = lista_camp_ProyectoPadre.ElementAt(0).Linea;
                var id_Cat = lista_camp_ProyectoPadre.ElementAt(0).Categoria;
                var id_Sub = lista_camp_ProyectoPadre.ElementAt(0).Subcategoria;
                var nombre_linea = db.sp_LINK_ConsultaLineas(empresa).Where(vq => vq.codigo == id_linea).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_categoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa, id_linea).Where(vq => vq.cod_servicio == id_Cat).Select(vq => vq.nombre).SingleOrDefault();
                var nombre_Subcategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa, id_linea).Where(vq => vq.codcen == id_Sub).Select(vq => vq.nombre).SingleOrDefault();


                //cod_linea  lo utilizo com ocampo auxiliar

                listaCampos_CtrPadre.Add(new MT_Contrato
                {

                    Valor_Referencial_Aux = auxValor,
                    Monto_Aux = auxValorMonto,
                    Fecha_Inicio = fechaI,
                    Dia_Plazo = lista_camp_ProyectoPadre.ElementAt(0).Dia_Plazo,
                    Fecha_Fin = lista_camp_ProyectoPadre.ElementAt(0).Fecha_Fin,
                    Id_Linea = lista_camp_ProyectoPadre.ElementAt(0).Linea,
                    Categoria = lista_camp_ProyectoPadre.ElementAt(0).Categoria,
                    Subcategoria = lista_camp_ProyectoPadre.ElementAt(0).Subcategoria,
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



        //FORMULARIOS DE PROYECTO
        #region
        [HttpGet]
        public ActionResult Formularios_Proyectos(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(id);
                    if (mT_Proyecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_Proyecto.Id_Proyecto;
                        var listaFormulariosP = db.sp_Quimipac_ConsultaMT_ProyectoDocumentado(id, empresa_id.ToString()).ToList();
                        var proyecto = mT_Proyecto.Codigo_Cliente;
                        ViewBag.listaFormulariosP = listaFormulariosP;
                        ViewBag.proyecto = proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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
        public ActionResult AgregarFormulario_Proyecto()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsTipoP = new List<SelectListItem>();

                    var listaTipoP = db.sp_Quimipac_ConsultaMT_TablaDetalle(51).ToList();
                    foreach (var tipo in listaTipoP)
                    {
                        itemsTipoP.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }

                    SelectList selectlistaTipoP = new SelectList(itemsTipoP, "Value", "Text");


                    ViewBag.listaTipoP = selectlistaTipoP;

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
        public ActionResult AgregarFormulario_Proyecto([Bind(Include = "Id_Proyecto,Tipo,Descripcion,Enlace,Fecha_Registro,Fecha_Validez,Estado,NombreArchivo")] MT_Proyecto_Documento mT_FormularioP, HttpPostedFileBase NombreArchivo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var FormularioP = db.MT_Contrato_Documentado.Where(x => x.NombreArchivo == mT_FormularioP.NombreArchivo);
                            if (FormularioP.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Formulario ya existe";
                                return RedirectToAction("Proyectos", new { id = idProyecto });
                            }
                            else
                            {
                                if (NombreArchivo != null)
                                {
                                    var ruta_archivo = @"~/Formularios_Actividad/";
                                    var ruta_servidor = Path.GetFullPath(ruta_archivo);
                                    var extension = Path.GetExtension(NombreArchivo.FileName);

                                    int fileSize = NombreArchivo.ContentLength;

                                    mT_FormularioP.Id_Proyecto = idProyecto;
                                    mT_FormularioP.NombreArchivo = "";
                                    mT_FormularioP.Fecha_Registro = DateTime.Now;
                                    mT_FormularioP.Estado = mT_FormularioP.Estado.Substring(0, 1);
                                    mT_FormularioP.Enlace = ruta_servidor;
                                    db.MT_Proyecto_Documento.Add(mT_FormularioP);
                                    db.SaveChanges();

                                    int scope_identity_id = mT_FormularioP.Id_Proyecto_Documento;

                                    string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                                    mT_FormularioP.NombreArchivo = nombreArchivo;

                                    NombreArchivo.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                                    db.Entry(mT_FormularioP).State = EntityState.Modified;
                                    db.SaveChanges();
                                    TempData["mensaje_correcto"] = "Formulario de Proyecto guardado";
                                    return RedirectToAction("Formularios_Proyectos", new { id = idProyecto });
                                }
                                else
                                {
                                    return View(mT_FormularioP);
                                }

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Formularios_Proyectos", new { id = idProyecto });
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
        public ActionResult EliminarFormularioProyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {


                    var id_MTProyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_MTProyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyecto = int.Parse(id_MTProyecto.ToString());

                        MT_Proyecto_Documento mT_Formulario = db.MT_Proyecto_Documento.Find(id);

                        if (mT_Formulario != null)
                        {
                            mT_Formulario.Estado = "E";
                            db.Entry(mT_Formulario).State = EntityState.Modified;
                            //db.MT_Formulario.Remove(mT_Formulario);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Archivo eliminado";
                            return RedirectToAction("Formularios_Proyectos", new { id = idProyecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Formularios_Proyectos", new { id = idProyecto });
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

        //ALERTA DE PROYECTO
        #region
        [HttpGet]
        public ActionResult Alerta_Proyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(id);
                    if (mT_Proyecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_Proyecto.Id_Proyecto;
                        var listaNotificaciones = db.sp_Quimipac_Consulta_Notificaciones_General(81, empresa_id.ToString(), id).ToList();
                        var proyecto = mT_Proyecto.Codigo_Cliente;
                        ViewBag.listaNotificaciones = listaNotificaciones;
                        ViewBag.proyecto = proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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
        public ActionResult AgregarAlertaP()
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
                        if (tipo.Descripcion.Equals("Proyecto"))
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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarAlertaP([Bind(Include = "Tipo_Notificacion,Id_Codigo_Origen,Id_usuario,Fecha,Prioridad,Asunto,Mensaje,Criterio,Id_Notificacion,Id_Persona,Tipo,Correo, Fecha_Hora,Estado,Frecuencia, Enviado, Fecha_Envio, Reenvio, EmpresaID")] InsertNotificacion mT_Notificacion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    int idProyecto = int.Parse(id_Proyecto.ToString());
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
                            return RedirectToAction("Alerta_Proyecto", new { id = idProyecto });
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

                                var Id_noti_Nuevo = dbe.InsertarNotificacion(mT_Notificacion.Tipo_Notificacion, mT_Notificacion.Id_usuario, mT_Notificacion.Fecha, mT_Notificacion.Prioridad, mT_Notificacion.Asunto, mT_Notificacion.Mensaje, dbe.GetCriterioNoti("Salida"), mT_Notificacion.Tipo, mT_Notificacion.Correo, mT_Notificacion.Estado, idProyecto, Convert.ToInt32(Id_Notificacion), mT_Notificacion.Frecuencia, mT_Notificacion.EmpresaID, mT_Notificacion.Reenvio);
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
                                return RedirectToAction("Alerta_Proyecto", new { id = idProyecto });
                            }

                        }

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Alerta_Proyecto", new { id = idProyecto });
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
        public ActionResult EliminarAlerta(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {


                    var id_MTProyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_MTProyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMTActividad = int.Parse(id_MTProyecto.ToString());

                        MT_Proyecto_Alerta mT_Alerta = db.MT_Proyecto_Alerta.Find(id);

                        if (mT_Alerta != null)
                        {
                            mT_Alerta.Estado = "E";
                            db.Entry(mT_Alerta).State = EntityState.Modified;
                            // db.MT_Proyecto_Alerta.Remove(mT_Alerta);
                            db.SaveChanges();
                            TempData["mensaje_correcto"] = "Archivo eliminado";
                            return RedirectToAction("Alerta_Proyecto", new { id = id_MTProyecto });
                        }
                        else
                        {
                            TempData["mensaje_correcto"] = "Código no existe";
                            return RedirectToAction("Alerta_Proyecto", new { id = id_MTProyecto });
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

        //ACTIVIDADES DE PROYECTOS
        #region
        [HttpGet]
        public ActionResult Actividades_Proyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(id);


                    if (mT_Proyecto != null)
                    {
                        System.Web.HttpContext.Current.Session["id_tipoTrabajo"] = mT_Proyecto.Id_TipoTrabajo;
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_Proyecto.Id_Proyecto;
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaActividades_Proyecto = db.Sp_Quimipac_ConsultaInsMt_ProyectoActvidad(mT_Proyecto.Id_TipoTrabajo, mT_Proyecto.Id_Proyecto, empresa_id.ToString()).ToList();
                        var proyecto = mT_Proyecto.Codigo_Cliente;
                        ViewBag.listaActividades_Proyecto = listaActividades_Proyecto;
                        ViewBag.proyecto = proyecto;
                        return View();

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Actividades_Proyecto");
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
        public ActionResult Agregar_Actividades_Proyectos()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsActividad = new List<SelectListItem>();
                    List<SelectListItem> itemsEstado = new List<SelectListItem>();
                    var id_tipo_trabajo = System.Web.HttpContext.Current.Session["id_tipoTrabajo"].ToString();
                    var id_trabajo = Convert.ToInt32(id_tipo_trabajo);
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
                    var listaActividades = db.sp_Quimipac_ConsultaMT_Actividad(id_trabajo, empresa_id).ToList();
                    foreach (var actividad in listaActividades)
                    {
                        itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Descripcion });
                    }

                    itemsActividad.Insert(0, new SelectListItem() { Value = "0", Text = "Ninguna" });

                    SelectList selectlistaActividades = new SelectList(itemsActividad, "Value", "Text");

                    var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();

                    foreach (var estado in listaEstados)
                    {

                        itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion });
                    }

                    SelectList selectlistaEstadoActividadesProyecto = new SelectList(itemsEstado, "Value", "Text");

                    ViewBag.listaActividades = selectlistaActividades;
                    ViewBag.listaEstadoActividad = selectlistaEstadoActividadesProyecto;

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
        public ActionResult Agregar_Actividades_Proyectos([Bind(Include = "Id_Proyecto, Id_Actividad,Estado,Orden, Fecha_Ini, Fecha_Fin,Comentario,Id_Planilla,Estado_Actividad_Plantilla,EstadoAct")] MT_Proyecto_Actividad mT_Actividad)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_MantTipoTrabajo = System.Web.HttpContext.Current.Session["id_tipoTrabajo"];
                    var id_proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    var idProyecto = int.Parse(id_proyecto.ToString());

                    if (id_proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantTipoTrabajo = int.Parse(id_MantTipoTrabajo.ToString());

                        if (ModelState.IsValid)
                        {
                            //var tipoTrabajo = db.sp_Quimipac_ConsultaMT_Actividad(idMantTipoTrabajo, empresa_id.ToString()).ToList().Where(x => x.Descripcion == mT_Actividad.Descripcion && x.Codigo == mT_Actividad.Codigo);
                            var actividadestipotrabajo = db.MT_Proyecto_Actividad.Where(x => x.Id_Proyecto == mT_Actividad.Id_Proyecto && x.Id_Actividad == mT_Actividad.Id_Actividad && x.EstadoAct == "A").ToList();
                            if (actividadestipotrabajo.Count() >= 1)
                            {                                
                                TempData["mensaje_error"] = "Código ya existe";

                                return RedirectToAction("Actividades_Proyecto", new { id = idProyecto });
                            }
                            else
                            {
                                mT_Actividad.Id_Proyecto = idProyecto;                      

                                mT_Actividad.EstadoAct = mT_Actividad.EstadoAct.Substring(0, 1);


                                db.MT_Proyecto_Actividad.Add(mT_Actividad);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Actividad guardada";
                                return RedirectToAction("Actividades_Proyecto", new { id = idProyecto });


                            }


                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Actividades_Proyecto", new { id = idProyecto });
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
        public ActionResult Editar_Actividades_Proyectos(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    System.Web.HttpContext.Current.Session["id_actividad"] = mT_ProyectoActividad.Id_Actividad;
                    var id_tipotrabajo = Convert.ToInt32(System.Web.HttpContext.Current.Session["id_tipoTrabajo"].ToString());
                    System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"] = mT_ProyectoActividad.Fecha_Ini;
                    System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"] = mT_ProyectoActividad.Fecha_Fin;
                    System.Web.HttpContext.Current.Session["EstadoAct"] = mT_ProyectoActividad.EstadoAct;
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();

                    bool seleccion = false;
                    if (mT_ProyectoActividad != null)
                    {

                        List<SelectListItem> itemsEstado = new List<SelectListItem>();
                        List<SelectListItem> itemsActividad = new List<SelectListItem>();

                        //var listaActividad = db.sp_Quimipac_ConsultaMT_Actividad_Proyecto().ToList();
                        var listaActividad = db.sp_Quimipac_ConsultaMT_Actividad(id_tipotrabajo, empresa_id).ToList();


                        foreach (var actividad in listaActividad)
                        {
                            if (actividad.Id_Actividad == mT_ProyectoActividad.Id_Actividad)
                            {
                                seleccion = true;
                            }

                            itemsActividad.Add(new SelectListItem { Value = Convert.ToString(actividad.Id_Actividad), Text = actividad.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaActividadesProyecto = new SelectList(itemsActividad, "Value", "Text");


                        var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();

                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_ProyectoActividad.Estado)
                            {
                                seleccion = true;
                            }

                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEstadoActividadesProyecto = new SelectList(itemsEstado, "Value", "Text");

                        ViewBag.listaActividad = selectlistaActividadesProyecto;
                        ViewBag.listaEstados = selectlistaEstadoActividadesProyecto;
                        ViewBag.fechaI = mT_ProyectoActividad.Fecha_Ini;
                        ViewBag.fechaF = mT_ProyectoActividad.Fecha_Fin;


                        return View(mT_ProyectoActividad);
                    }
                    else
                    {

                        var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                        if (id_Proyecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProyecto = int.Parse(id_Proyecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Actividades_Proyecto", new { id = idProyecto });
                        }

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_Actividades_Proyectos([Bind(Include = "Id_Proyecto_Actividad, Id_Proyecto, Id_Actividad,Estado,Orden, Fecha_Ini, Fecha_Fin,Comentario,Id_Planilla,Estado_Actividad_Plantilla,EstadoAct")] MT_Proyecto_Actividad mT_ProyectoActividad)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    var id_actividad = System.Web.HttpContext.Current.Session["id_actividad"];

                    int idProyecto = int.Parse(id_Proyecto.ToString());
                    int idMActividad = int.Parse(id_actividad.ToString());


                    if (ModelState.IsValid)
                    {

                        mT_ProyectoActividad.Id_Actividad = idMActividad;
                        mT_ProyectoActividad.Id_Proyecto = idProyecto;
                        if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"] != null)
                        {
                            mT_ProyectoActividad.Fecha_Ini = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaInicioEjecucionAct"]);
                        }

                        if (System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"] != null)
                        {
                            mT_ProyectoActividad.Fecha_Fin = Convert.ToDateTime(System.Web.HttpContext.Current.Session["FechaFinEjecucionAct"]);
                        }
                        mT_ProyectoActividad.EstadoAct = Convert.ToString(System.Web.HttpContext.Current.Session["EstadoAct"]);


                        db.Entry(mT_ProyectoActividad).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Actividad actualizada";

                        return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividad.Id_Proyecto });

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividad.Id_Proyecto });
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
        public ActionResult IniciarActividades_Proyectos(int id)
        {
            try
            {
                MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                var listaProyectoActividadesEgreso = db.MT_Proyecto_ActividadEgreso.Where(x => x.Id_Proyecto_Actividad == id).ToList();
                var listaProyectoActividadesEquipo = db.MT_Proyecto_Actividad_Equipo.Where(x => x.Id_Proyecto_Actividad == id).ToList();
                var listaProyectoActividadesIntegrante = db.MT_Proyecto_Actividad_Integrante.Where(x => x.Id_Proyecto_Actividad == id).ToList();

                if (listaProyectoActividadesEgreso.Count > 0 && listaProyectoActividadesEquipo.Count > 0 && listaProyectoActividadesIntegrante.Count > 0)
                {

                    if (mT_ProyectoActividad.Fecha_Ini == null)
                    {

                        mT_ProyectoActividad.Fecha_Ini = DateTime.Now;
                        mT_ProyectoActividad.Estado = 68;
                        //mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;

                        var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(idProyecto);

                        //var fecha = db.MT_OrdenTrabajo.Where(x => x.Fecha_inicio_ejecucion == mT_OrdenTrabajo.Fecha_inicio_ejecucion);

                        //if (fecha.Count() > 0)
                        //{
                        //    mT_OrdenTrabajo.Fecha_inicio_ejecucion = DateTime.Now;
                        //}

                        if (mT_Proyecto.Fecha_Inicio == null)
                        {

                            mT_Proyecto.Fecha_Inicio = DateTime.Now;
                        }


                        db.Entry(mT_ProyectoActividad).State = EntityState.Modified;
                        db.Entry(mT_Proyecto).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Proyecto Actividad Ya fue iniciada";
                        return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividad.Id_Proyecto });
                    }

                }
                else
                {
                    TempData["mensaje_error"] = "Revise que tenga equipo, integrantes, e items";
                    return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividad.Id_Proyecto });

                }

                TempData["mensaje_actualizado"] = "Proyecto Actividad Iniciada";
                return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividad.Id_Proyecto });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }

        }

        [HttpGet]
        public ActionResult TerminarActividades_Proyectos(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividadEstado = db.MT_Proyecto_Actividad.Find(id);
                    System.Web.HttpContext.Current.Session["id_actividad"] = mT_ProyectoActividadEstado.Id_Actividad;
                    System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] = mT_ProyectoActividadEstado.Fecha_Ini;
                    System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] = mT_ProyectoActividadEstado.Fecha_Fin;

                    bool seleccion = false;
                    if (mT_ProyectoActividadEstado != null)
                    {

                        List<SelectListItem> itemsEstado = new List<SelectListItem>();

                        //var listaEstados = db.sp_Quimipac_ConsultaMT_TablaDetalle(39).ToList();
                        var listaEstados = db.MT_TablaDetalle.Where(x => x.Id_Padre == 69).ToList();


                        foreach (var estado in listaEstados)
                        {
                            if (estado.Id_TablaDetalle == mT_ProyectoActividadEstado.Estado)
                            {
                                seleccion = true;
                            }

                            itemsEstado.Add(new SelectListItem { Value = Convert.ToString(estado.Id_TablaDetalle), Text = estado.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEstadoActividadesProyecto = new SelectList(itemsEstado, "Value", "Text");


                        ViewBag.listaEstados = selectlistaEstadoActividadesProyecto;
                        //ViewBag.fechaI = mT_OrdenActividad.Fecha_Ini;
                        //ViewBag.fechaF = mT_OrdenActividad.Fecha_Fin;


                        return View(mT_ProyectoActividadEstado);
                    }
                    else
                    {

                        var id_MantProyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                        if (id_MantProyecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idMantProyecto = int.Parse(id_MantProyecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Actividades_Proyecto", new { id = idMantProyecto });
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

        public ActionResult TerminarActividades_Proyectos([Bind(Include = "Id_Proyecto_Actividad, Id_Proyecto, Estado")] MT_Proyecto_Actividad mT_ProyectoActividadFinalizada)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    var id_actividad = System.Web.HttpContext.Current.Session["id_actividad"];

                    int idProyecto = int.Parse(id_Proyecto.ToString());
                    int idMActividad = int.Parse(id_actividad.ToString());


                    if (ModelState.IsValid)
                    {
                        if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] != null && System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] == null)
                        {
                            mT_ProyectoActividadFinalizada.Id_Actividad = idMActividad;
                            mT_ProyectoActividadFinalizada.Id_Proyecto = idProyecto;

                            var lkProyProrroga = db.MT_Proyecto_Actividad.Where(x => x.Id_Proyecto_Actividad == mT_ProyectoActividadFinalizada.Id_Proyecto_Actividad);
                            if (lkProyProrroga.Count() > 0)
                            {
                                foreach (var itemProrroga in lkProyProrroga)
                                {
                                    itemProrroga.Estado = mT_ProyectoActividadFinalizada.Estado;
                                    itemProrroga.Fecha_Fin = DateTime.Now;
                                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(idProyecto);

                                    mT_Proyecto.Fecha_Fin = DateTime.Now;
                                }
                                db.SaveChanges();


                            }
                        }
                        else if (System.Web.HttpContext.Current.Session["FechaInicioEjecucionActTermin"] != null && System.Web.HttpContext.Current.Session["FechaFinEjecucionActTermin"] != null)
                        {
                            mT_ProyectoActividadFinalizada.Id_Proyecto = idProyecto;
                            TempData["mensaje_actualizado"] = "Actividad ya fue iniciada y terminada";

                            return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividadFinalizada.Id_Proyecto });
                        }
                        else
                        {
                            mT_ProyectoActividadFinalizada.Id_Proyecto = idProyecto;
                            TempData["mensaje_actualizado"] = "Actividad No ha sido iniciada";

                            return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividadFinalizada.Id_Proyecto });

                        }

                        //db.Entry(mT_OrdenActividadFinalizada).State = EntityState.Modified;
                        //db.SaveChanges();
                        //DataBase_Externo database4 = new DataBase_Externo();

                        //    database4.ActualizarActividadOrden(mT_OrdenActividadFinalizada.Id_OrdenTrabajo_Actividad, mT_OrdenActividadFinalizada.Id_Actividad, mT_OrdenActividadFinalizada.Id_Orden_Trabajo, mT_OrdenActividadFinalizada.Estado);


                        TempData["mensaje_actualizado"] = "Actividad actualizada";

                        return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividadFinalizada.Id_Proyecto });

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("Actividades_Proyecto", new { id = mT_ProyectoActividadFinalizada.Id_Proyecto });
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
        public ActionResult Eliminar_Actividades_Proyectos(int id)
        {
            try
            {
                MT_Proyecto_Actividad mT_ActividadProyecto = db.MT_Proyecto_Actividad.Find(id);
                mT_ActividadProyecto.EstadoAct = "E";
                db.Entry(mT_ActividadProyecto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensaje_actualizado"] = "Actividad Proyectos Eliminada";
                return RedirectToAction("Actividades_Proyecto", new { id = mT_ActividadProyecto.Id_Proyecto });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Errores");
            }
        }


        //[HttpGet]
        //public ActionResult EliminarArchivoFormulario(int id)
        //{
        //    try
        //    {


        //        var id_MTActividad = System.Web.HttpContext.Current.Session["id_MTActividad"];

        //        if (id_MTActividad == null)
        //        {
        //            return RedirectToAction("IniciarSesion", "Home");
        //        }

        //        else
        //        {
        //            int idMTActividad = int.Parse(id_MTActividad.ToString());

        //            MT_Proyecto_Actividad_Anexo mT_Formulario = db.MT_Proyecto_Actividad_Anexo.Find(id);

        //            if (mT_Formulario != null)
        //            {
        //                mT_Formulario.Estado = "E";
        //                db.Entry(mT_Formulario).State = EntityState.Modified;
        //                //db.MT_Formulario.Remove(mT_Formulario);
        //                db.SaveChanges();
        //                TempData["mensaje_correcto"] = "Archivo eliminado";
        //                return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
        //            }
        //            else
        //            {
        //                TempData["mensaje_correcto"] = "Código no existe";
        //                return RedirectToAction("Formularios_Actividad", new { id = idMTActividad });
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }
        //}
        #endregion

        //Proyectos Actividades Equipo
        #region
        [HttpGet]
        public ActionResult ProyectoActividades_Equipo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    if (mT_ProyectoActividad != null)
                    {
                        System.Web.HttpContext.Current.Session["id_ProyectoActividad"] = mT_ProyectoActividad.Id_Proyecto_Actividad;
                        var listaEquiposProyectoActividades = db.sp_Quimipac_ConsultaMT_ProyectoActividadesEquipo(id).ToList();
                        ViewBag.listaEquiposProyectoActividades = listaEquiposProyectoActividades;
                        ViewBag.idProyecto = mT_ProyectoActividad.Id_Proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Equipo Proyecto ctividad no existe";
                        return RedirectToAction("Actividades_Proyecto");
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
        public ActionResult Agregar_ProyectoActividades_Equipo()
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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ProyectoActividades_Equipo([Bind(Include = "Id_Equipo, Fecha_Ini, Fecha_Fin")] MT_Proyecto_Actividad_Equipo mT_ProyectoActividadEquipo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {
                            mT_ProyectoActividadEquipo.Id_Proyecto_Actividad = idMantProyectoActividad;
                            var itemProyectoActividad = db.MT_Proyecto_Actividad_Equipo.Where(x => x.Id_Proyecto_Actividad == mT_ProyectoActividadEquipo.Id_Proyecto_Actividad && x.Id_Equipo == mT_ProyectoActividadEquipo.Id_Equipo && x.Fecha_Ini == mT_ProyectoActividadEquipo.Fecha_Ini && x.Fecha_Fin == mT_ProyectoActividadEquipo.Fecha_Fin);
                            if (itemProyectoActividad.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Equipo ya existe";
                                return RedirectToAction("ProyectoActividades_Equipo", new { id = idMantProyectoActividad });
                            }
                            else
                            {

                                mT_ProyectoActividadEquipo.Id_Proyecto_Actividad = idMantProyectoActividad;


                                db.MT_Proyecto_Actividad_Equipo.Add(mT_ProyectoActividadEquipo);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Equipo guardado";
                                return RedirectToAction("ProyectoActividades_Equipo", new { id = idMantProyectoActividad });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Equipo", new { id = idMantProyectoActividad });
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
        public ActionResult Editar_ProyectoActividades_Equipo(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad_Equipo mT_proyectoactividadEquipo = db.MT_Proyecto_Actividad_Equipo.Find(id);
                    bool seleccion = false;
                    if (mT_proyectoactividadEquipo != null)
                    {

                        List<SelectListItem> equipos = new List<SelectListItem>();



                        var listaEquipos = db.sp_Quimipac_ConsultaMt_Equipo().ToList();
                        foreach (var equipo in listaEquipos)
                        {
                            if (mT_proyectoactividadEquipo.Id_Equipo == equipo.Id_Equipo)
                            {
                                seleccion = true;
                            }

                            equipos.Add(new SelectListItem { Value = Convert.ToString(equipo.Id_Equipo), Text = equipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaEquipos = new SelectList(equipos, "Value", "Text");


                        ViewBag.listaEquipos = selectlistaEquipos;

                        return View(mT_proyectoactividadEquipo);

                    }
                    else
                    {
                        var id_proyectoactividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                        if (id_proyectoactividad == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idproyectoactividad = int.Parse(id_proyectoactividad.ToString());
                            TempData["mensaje_error"] = "Equipo no existe";
                            return RedirectToAction("ProyectoActividades_Equipo", new { id = idproyectoactividad });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_ProyectoActividades_Equipo([Bind(Include = "Id_Proyecto_Actividad_Equipo, Id_Proyecto_Actividad, Id_Equipo, Fecha_Ini, Fecha_Fin")] MT_Proyecto_Actividad_Equipo mT_ProyectoActividadEquipo)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];
                    // int bandera = 0;
                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_ProyectoActividadEquipo.Id_Proyecto_Actividad = idProyectoActividad;


                            db.Entry(mT_ProyectoActividadEquipo).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Equipo Proyecto Actividad guardado";

                            return RedirectToAction("ProyectoActividades_Equipo", new { id = idProyectoActividad });


                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Equipo", new { id = idProyectoActividad });
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

        //Proyectos Actividades Integrante
        #region
        [HttpGet]
        public ActionResult ProyectoActividades_Integrante(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    if (mT_ProyectoActividad != null)
                    {
                        System.Web.HttpContext.Current.Session["id_ProyectoActividad"] = mT_ProyectoActividad.Id_Proyecto_Actividad;
                        var listaProyectoActividadesIntegrantes = db.sp_Quimipac_ConsultaMT_ProyectoActividadesIntegrantes(id).ToList();
                        ViewBag.listaProyectoActividadesIntegrantes = listaProyectoActividadesIntegrantes;
                        ViewBag.idProyecto = mT_ProyectoActividad.Id_Proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Integrante Proyecto ctividad no existe";
                        return RedirectToAction("Actividades_Proyecto");
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
        public ActionResult Agregar_ProyectoActividades_Integrante()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    List<SelectListItem> personas = new List<SelectListItem>();
                    List<SelectListItem> roles = new List<SelectListItem>();

                    var listapersona = db.sp_LINK_ConsultaPersonas().ToList();
                    foreach (var persona in listapersona)
                    {
                        personas.Add(new SelectListItem
                        {
                            Value = Convert.ToString(persona.id_persona),
                            Text = persona.Nombres_Completos
                        });
                    }

                    SelectList selectlistaPersona = new SelectList(personas, "Value", "Text");

                    var listarol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                    foreach (var rol in listarol)
                    {
                        roles.Add(new SelectListItem
                        {
                            Value = Convert.ToString(rol.Id_TablaDetalle),
                            Text = rol.Descripcion
                        });
                    }

                    SelectList selectlistarol = new SelectList(roles, "Value", "Text");


                    ViewBag.listapersona = selectlistaPersona;
                    ViewBag.listarol = selectlistarol;

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
        public ActionResult Agregar_ProyectoActividades_Integrante([Bind(Include = "Id_Persona, Rol, Fecha_Ini, Fecha_Fin, Estado")] MT_Proyecto_Actividad_Integrante mT_ProyectoActividadIntegrante)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {
                            mT_ProyectoActividadIntegrante.Id_Proyecto_Actividad = idMantProyectoActividad;
                            var itemProyectoActividad = db.MT_Proyecto_Actividad_Integrante.Where(x => x.Id_Proyecto_Actividad == mT_ProyectoActividadIntegrante.Id_Proyecto_Actividad && x.Id_Persona == mT_ProyectoActividadIntegrante.Id_Persona && x.Rol == mT_ProyectoActividadIntegrante.Rol && x.Estado == mT_ProyectoActividadIntegrante.Estado);
                            if (itemProyectoActividad.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Integrante ya existe";
                                return RedirectToAction("ProyectoActividades_Integrante", new { id = idMantProyectoActividad });
                            }
                            else
                            {

                                mT_ProyectoActividadIntegrante.Id_Proyecto_Actividad = idMantProyectoActividad;
                                mT_ProyectoActividadIntegrante.Estado = mT_ProyectoActividadIntegrante.Estado.Substring(0, 1);


                                db.MT_Proyecto_Actividad_Integrante.Add(mT_ProyectoActividadIntegrante);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Integrante guardado";
                                return RedirectToAction("ProyectoActividades_Integrante", new { id = idMantProyectoActividad });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Integrante", new { id = idMantProyectoActividad });
                        }
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
        public ActionResult Editar_ProyectoActividades_Integrante(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad_Integrante mT_proyectoactividadIntegrante = db.MT_Proyecto_Actividad_Integrante.Find(id);
                    bool seleccion = false;
                    if (mT_proyectoactividadIntegrante != null)
                    {

                        List<SelectListItem> integrante = new List<SelectListItem>();
                        List<SelectListItem> rol = new List<SelectListItem>();



                        var listaIntegrante = db.sp_LINK_ConsultaPersonas().ToList();
                        foreach (var integrantes in listaIntegrante)
                        {
                            if (mT_proyectoactividadIntegrante.Id_Persona == integrantes.id_persona)
                            {
                                seleccion = true;
                            }

                            integrante.Add(new SelectListItem { Value = Convert.ToString(integrantes.id_persona), Text = integrantes.Nombres_Completos, Selected = seleccion });
                        }

                        SelectList selectlistaIntegrante = new SelectList(integrante, "Value", "Text");

                        var listarol = db.sp_Quimipac_ConsultaMT_TablaDetalle(10).ToList();
                        foreach (var roles in listarol)
                        {
                            if (mT_proyectoactividadIntegrante.Rol == roles.Id_TablaDetalle)
                            {
                                seleccion = true;
                            }

                            rol.Add(new SelectListItem { Value = Convert.ToString(roles.Id_TablaDetalle), Text = roles.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaRol = new SelectList(rol, "Value", "Text");


                        ViewBag.listaIntegrante = selectlistaIntegrante;
                        ViewBag.listarol = selectlistaRol;

                        return View(mT_proyectoactividadIntegrante);

                    }
                    else
                    {
                        var id_proyectoactividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                        if (id_proyectoactividad == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idproyectoactividad = int.Parse(id_proyectoactividad.ToString());
                            TempData["mensaje_error"] = "Integrante no existe";
                            return RedirectToAction("ProyectoActividades_Integrante", new { id = idproyectoactividad });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_ProyectoActividades_Integrante([Bind(Include = "Id_Proyecto_Actividad_Integrante, Id_Proyecto_Actividad, Id_Persona, Rol, Fecha_Ini, Fecha_Fin, Estado")] MT_Proyecto_Actividad_Integrante mT_ProyectoActividadIntegrante)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];
                    // int bandera = 0;
                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_ProyectoActividadIntegrante.Id_Proyecto_Actividad = idProyectoActividad;
                            mT_ProyectoActividadIntegrante.Estado = mT_ProyectoActividadIntegrante.Estado.Substring(0, 1);

                            db.Entry(mT_ProyectoActividadIntegrante).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Integrante grupo de proyecto activiad";

                            return RedirectToAction("ProyectoActividades_Integrante", new { id = idProyectoActividad });


                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Integrante", new { id = idProyectoActividad });
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


        #endregion

        // Proyectos Actividades ITEMS
        #region

        [HttpGet]
        public ActionResult ProyectoActividades_Items(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    if (mT_ProyectoActividad != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_ProyectoActividad"] = mT_ProyectoActividad.Id_Proyecto_Actividad;
                        var listaProyectoActividadesItems = db.sp_Quimipac_ConsultaInsMt_ProyectoActividadEgreso(id, empresa_id.ToString()).ToList();
                        ViewBag.listaProyectoActividadesItems = listaProyectoActividadesItems;
                        ViewBag.idProyecto = mT_ProyectoActividad.Id_Proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Items Proyecto Actividad no existe";
                        return RedirectToAction("Actividades_Proyecto");
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
        public ActionResult Agregar_ProyectoActividades_Items()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsItems = new List<SelectListItem>();
                    List<SelectListItem> itemsUnidades = new List<SelectListItem>();
                    List<SelectListItem> itemsTipo = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();

                    foreach (var items in listaItems)
                    {

                        itemsItems.Add(new SelectListItem { Value = Convert.ToString(items.cod_item), Text = items.descripcion });

                    }

                    SelectList selectlistaItems = new SelectList(itemsItems, "Value", "Text");

                    var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();

                    foreach (var unidades in listaUnidades)
                    {

                        itemsUnidades.Add(new SelectListItem { Value = Convert.ToString(unidades.uni_med), Text = unidades.nombre });

                    }

                    SelectList selectlistaUnidades = new SelectList(itemsUnidades, "Value", "Text");

                    var listaTipoEgreso = db.sp_Quimipac_ConsultaMT_TablaDetalle(55).ToList();

                    foreach (var tipo in listaTipoEgreso)
                    {

                        itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });

                    }

                    SelectList selectlistaTipoEgreso = new SelectList(itemsTipo, "Value", "Text");


                    ViewBag.listaItems = selectlistaItems;
                    ViewBag.listaUnidades = selectlistaUnidades;
                    ViewBag.listaTipoEgreso = selectlistaTipoEgreso;


                    return View();
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


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ProyectoActividades_Items([Bind(Include = "Id_Proyecto_Actividad,Id_Item,Fecha,Tipo,Unidad,Cantidad")] MT_Proyecto_ActividadEgreso mT_ProyectoactividadEgreso)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_MantProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];


                    if (id_MantProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantProyectoActividad = int.Parse(id_MantProyectoActividad.ToString());


                        if (ModelState.IsValid)
                        {
                            mT_ProyectoactividadEgreso.Id_Proyecto_Actividad = idMantProyectoActividad;

                            var itemProyectoActividad = db.MT_Proyecto_ActividadEgreso.Where(x => x.Id_Proyecto_Actividad == mT_ProyectoactividadEgreso.Id_Proyecto_Actividad && x.Id_Item == mT_ProyectoactividadEgreso.Id_Item && x.Tipo == mT_ProyectoactividadEgreso.Tipo && x.Unidad == mT_ProyectoactividadEgreso.Unidad);
                            if (itemProyectoActividad.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Egreso ya existe";
                                return RedirectToAction("ProyectoActividades_Items", new { id = idMantProyectoActividad });
                            }
                            else
                            {


                                mT_ProyectoactividadEgreso.Id_Proyecto_Actividad = idMantProyectoActividad;
                                mT_ProyectoactividadEgreso.Fecha = DateTime.Now;

                                db.MT_Proyecto_ActividadEgreso.Add(mT_ProyectoactividadEgreso);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Egreso guardado";
                                return RedirectToAction("ProyectoActividades_Items", new { id = idMantProyectoActividad });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Items", new { id = idMantProyectoActividad });
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
        public ActionResult Editar_ProyectoActividades_Items(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_ActividadEgreso mT_ProyectoActividadEgreso = db.MT_Proyecto_ActividadEgreso.Find(id);
                    System.Web.HttpContext.Current.Session["id_Item"] = mT_ProyectoActividadEgreso.Id_Item;
                    System.Web.HttpContext.Current.Session["Fecha"] = mT_ProyectoActividadEgreso.Fecha;
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    bool seleccion = false;
                    if (mT_ProyectoActividadEgreso != null)
                    {

                        List<SelectListItem> itemsItems = new List<SelectListItem>();
                        List<SelectListItem> itemsUnidades = new List<SelectListItem>();
                        List<SelectListItem> itemsTipo = new List<SelectListItem>();


                        var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();

                        foreach (var items in listaItems)
                        {

                            if (items.cod_item == mT_ProyectoActividadEgreso.Id_Item)
                            {
                                seleccion = true;
                            }


                            itemsItems.Add(new SelectListItem { Value = Convert.ToString(items.cod_item), Text = items.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaItemsProyectoActividad = new SelectList(itemsItems, "Value", "Text");


                        var listaUnidades = db.sp_LINK_ConsultaUnidades().ToList();

                        foreach (var unidades in listaUnidades)
                        {

                            if (unidades.uni_med == mT_ProyectoActividadEgreso.Unidad)
                            {
                                seleccion = true;
                            }


                            itemsUnidades.Add(new SelectListItem { Value = Convert.ToString(unidades.uni_med), Text = unidades.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaUnidadesProyectoActividad = new SelectList(itemsUnidades, "Value", "Text");

                        var listaTipoEgreso = db.sp_Quimipac_ConsultaMT_TablaDetalle(55).ToList();

                        foreach (var tipo in listaTipoEgreso)
                        {

                            if (tipo.Id_TablaDetalle == mT_ProyectoActividadEgreso.Tipo)
                            {
                                seleccion = true;
                            }


                            itemsTipo.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipoEgresoProyectoActividad = new SelectList(itemsTipo, "Value", "Text");

                        ViewBag.listaItems = selectlistaItemsProyectoActividad;
                        ViewBag.listaUnidades = selectlistaUnidadesProyectoActividad;
                        ViewBag.listaTipoEgreso = selectlistaTipoEgresoProyectoActividad;

                        return View(mT_ProyectoActividadEgreso);
                    }
                    else
                    {

                        var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                        if (id_ProyectoActividad == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProyectoActividad = int.Parse(id_ProyectoActividad.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("ProyectoActividades_Items", new { id = idProyectoActividad });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_ProyectoActividades_Items([Bind(Include = "ID_Proyecto_Actividad_Egreso, Id_Proyecto_Actividad,Id_Item,Fecha,Tipo,Unidad,Cantidad")] MT_Proyecto_ActividadEgreso mT_ProyectoactividadEgreso)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];
                    var id_Item = System.Web.HttpContext.Current.Session["id_Item"];


                    int idProyectoActividad = int.Parse(id_ProyectoActividad.ToString());
                    string idItem = id_Item.ToString();

                    if (ModelState.IsValid)
                    {

                        mT_ProyectoactividadEgreso.Id_Item = idItem;
                        mT_ProyectoactividadEgreso.Id_Proyecto_Actividad = idProyectoActividad;
                        mT_ProyectoactividadEgreso.Fecha = Convert.ToDateTime(System.Web.HttpContext.Current.Session["Fecha"]);



                        db.Entry(mT_ProyectoactividadEgreso).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje_actualizado"] = "Egreso actualizado";

                        return RedirectToAction("ProyectoActividades_Items", new { id = mT_ProyectoactividadEgreso.Id_Proyecto_Actividad });

                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valores incorrectos";
                        return RedirectToAction("ProyectoActividades_Items", new { id = mT_ProyectoactividadEgreso.Id_Proyecto_Actividad });
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

        //[HttpGet]
        //public ActionResult EliminarItemsOrdenTrabajo(int id)
        //{
        //    try
        //    {

        //        MT_Proyecto_ActividadEgreso mT_ProyectoActividadEgreso = db.MT_Proyecto_ActividadEgreso.Find(id);

        //        mT_ProyectoActividadEgreso.es = "E";

        //        db.Entry(mT_OrdenEgreso).State = EntityState.Modified;
        //        db.SaveChanges();

        //        TempData["mensaje_actualizado"] = "Egreso Orden Trabajo Eliminado";
        //        return RedirectToAction("Items_OrdenTrabajo", new { id = mT_OrdenEgreso.Id_Orden_Trabajo });
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("Error", "Errores");
        //    }

        //}

        #endregion

        //FORMULARIOS ACTIVIDADES DE PROYECTO
        #region
        [HttpGet]
        public ActionResult Formularios_Actividad(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    if (mT_ProyectoActividad != null)
                    {
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_ProyectoActividad.Id_Proyecto_Actividad;
                        var listaFormulariosP = db.sp_Quimipac_ConsultaMT_ProyectoActividdAnexo(id).ToList();
                        ViewBag.listaFormulariosPA = listaFormulariosP;
                        ViewBag.idProyecto = mT_ProyectoActividad.Id_Proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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
        public FileResult DescargarArchivoFormularioActividad(string nombre)
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
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsTipoP = new List<SelectListItem>();

                    var listaTipoP = db.sp_Quimipac_ConsultaMT_TablaDetalle(51).ToList();
                    foreach (var tipo in listaTipoP)
                    {
                        itemsTipoP.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion });
                    }

                    SelectList selectlistaTipoP = new SelectList(itemsTipoP, "Value", "Text");


                    ViewBag.listaTipoP = selectlistaTipoP;

                    return View();
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



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarFormulario_Actividad([Bind(Include = "Id_Proyecto_Actividad,Tipo,Descripcion,Enlace,Fecha")] MT_Proyecto_Actividad_Anexo mT_FormularioP, HttpPostedFileBase Descripcion)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var FormularioP = db.MT_Proyecto_Actividad_Anexo.Where(x => x.Descripcion == mT_FormularioP.Descripcion && x.Enlace == mT_FormularioP.Enlace && x.Tipo == mT_FormularioP.Tipo);
                            if (FormularioP.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Formulario ya existe";
                                return RedirectToAction("Proyectos", new { id = idProyecto });
                            }
                            else
                            {
                                if (Descripcion != null)
                                {
                                    var ruta_archivo = @"~/Formularios_Actividad/";
                                    var ruta_servidor = Path.GetFullPath(ruta_archivo);
                                    var extension = Path.GetExtension(Descripcion.FileName);

                                    int fileSize = Descripcion.ContentLength;

                                    mT_FormularioP.Id_Proyecto_Actividad = idProyecto;
                                    mT_FormularioP.Descripcion = "";
                                    mT_FormularioP.Fecha = DateTime.Now;
                                    mT_FormularioP.Enlace = ruta_servidor;
                                    db.MT_Proyecto_Actividad_Anexo.Add(mT_FormularioP);
                                    db.SaveChanges();

                                    int scope_identity_id = mT_FormularioP.Id_Proyecto_Actividad_Anexo;

                                    string nombreArchivo = DateTime.Now.ToString("yyyyMMdd") + "_" + scope_identity_id + extension;
                                    mT_FormularioP.Descripcion = nombreArchivo;

                                    Descripcion.SaveAs(Server.MapPath(ruta_archivo + nombreArchivo));


                                    db.Entry(mT_FormularioP).State = EntityState.Modified;
                                    db.SaveChanges();
                                    TempData["mensaje_correcto"] = "Formulario de Proyecto guardado";
                                    return RedirectToAction("Formularios_Actividad", new { id = idProyecto });
                                }
                                else
                                {
                                    return View(mT_FormularioP);
                                }

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Formularios_Actividad", new { id = idProyecto });
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

        //Proyectos Actividades Valor
        #region
        [HttpGet]
        public ActionResult ProyectoActividades_Valor(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad mT_ProyectoActividad = db.MT_Proyecto_Actividad.Find(id);
                    if (mT_ProyectoActividad != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_ProyectoActividad"] = mT_ProyectoActividad.Id_Proyecto_Actividad;
                        var listaValorProyectoActividades = db.sp_Quimipac_ConsultaMT_ProyectoActividadesValor(id, empresa_id.ToString()).ToList();
                        ViewBag.listaValorProyectoActividades = listaValorProyectoActividades;
                        ViewBag.idProyecto = mT_ProyectoActividad.Id_Proyecto;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Valor Proyecto Actividad no existe";
                        return RedirectToAction("Actividades_Proyecto");
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
        public ActionResult Agregar_ProyectoActividades_Valor()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Agregar_ProyectoActividades_Valor([Bind(Include = "Id_Proyecto_Actividad, Id_Usuario, Tipo_Valor, Id_Item,Moneda,Fecha,Unidad,Cantidad,Precio,Costo,IVA")] MT_Proyecto_Actividad_Valor mT_ProyectoActividadValor)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];
                    var user_id = System.Web.HttpContext.Current.Session["usuario"];

                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idMantProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {
                            mT_ProyectoActividadValor.Id_Usuario = user_id.ToString();
                            mT_ProyectoActividadValor.Fecha = DateTime.Now;
                            mT_ProyectoActividadValor.Id_Proyecto_Actividad = idMantProyectoActividad;
                            var itemProyectoActividad = db.MT_Proyecto_Actividad_Valor.Where(x => x.Id_Proyecto_Actividad == mT_ProyectoActividadValor.Id_Proyecto_Actividad && x.Id_Item == mT_ProyectoActividadValor.Id_Item && x.Moneda == mT_ProyectoActividadValor.Moneda && x.Cantidad == mT_ProyectoActividadValor.Cantidad);
                            if (itemProyectoActividad.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Proyecto Actividad Valor ya existe";
                                return RedirectToAction("ProyectoActividades_Valor", new { id = idMantProyectoActividad });
                            }
                            else
                            {

                                mT_ProyectoActividadValor.Id_Proyecto_Actividad = idMantProyectoActividad;


                                db.MT_Proyecto_Actividad_Valor.Add(mT_ProyectoActividadValor);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Proyecto Actividad Valor guardado";
                                return RedirectToAction("ProyectoActividades_Valor", new { id = idMantProyectoActividad });

                            }

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Valor", new { id = idMantProyectoActividad });
                        }
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
        public ActionResult Editar_ProyectoActividades_Valor(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Actividad_Valor mT_proyectoactividadValor = db.MT_Proyecto_Actividad_Valor.Find(id);
                    System.Web.HttpContext.Current.Session["Fecha"] = mT_proyectoactividadValor.Fecha;
                    bool seleccion = false;
                    if (mT_proyectoactividadValor != null)
                    {

                        List<SelectListItem> items = new List<SelectListItem>();
                        List<SelectListItem> TipoValor = new List<SelectListItem>();
                        List<SelectListItem> Monedas = new List<SelectListItem>();
                        List<SelectListItem> Unidades = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaItems = db.sp_LINK_ConsultaItems(empresa_id.ToString()).ToList();
                        foreach (var item in listaItems)
                        {
                            if (mT_proyectoactividadValor.Id_Item == item.cod_item)
                            {
                                seleccion = true;
                            }

                            items.Add(new SelectListItem { Value = Convert.ToString(item.cod_item), Text = item.descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaItems = new SelectList(items, "Value", "Text");

                        var listaTipo_Valor = db.sp_Quimipac_ConsultaMT_TablaDetalle(56).ToList();
                        foreach (var tipo in listaTipo_Valor)
                        {
                            if (mT_proyectoactividadValor.Tipo_Valor == tipo.Id_TablaDetalle)
                            {
                                seleccion = true;
                            }

                            TipoValor.Add(new SelectListItem { Value = Convert.ToString(tipo.Id_TablaDetalle), Text = tipo.Descripcion, Selected = seleccion });
                        }

                        SelectList selectlistaTipoValor = new SelectList(TipoValor, "Value", "Text");

                        var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();
                        foreach (var moneda in listaMoneda)
                        {
                            if (mT_proyectoactividadValor.Moneda == moneda.codmon)
                            {
                                seleccion = true;
                            }

                            Monedas.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaMoneda = new SelectList(Monedas, "Value", "Text");

                        var listaUnidad = db.sp_LINK_ConsultaUnidades().ToList();
                        foreach (var unidad in listaUnidad)
                        {
                            if (mT_proyectoactividadValor.Unidad == unidad.uni_med)
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

                        return View(mT_proyectoactividadValor);

                    }
                    else
                    {
                        var id_proyectoactividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];

                        if (id_proyectoactividad == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idproyectoactividad = int.Parse(id_proyectoactividad.ToString());
                            TempData["mensaje_error"] = "Proyecto Actividad Valor no existe";
                            return RedirectToAction("ProyectoActividades_Valor", new { id = idproyectoactividad });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar_ProyectoActividades_Valor([Bind(Include = "Id_Proyecto_Actividad_Valor,Id_Proyecto_Actividad, Id_Usuario, Tipo_Valor, Id_Item,Moneda,Fecha,Unidad,Cantidad,Precio,Costo,IVA")] MT_Proyecto_Actividad_Valor mT_ProyectoActividadValor)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {

                    var user_id = System.Web.HttpContext.Current.Session["usuario"];
                    var id_ProyectoActividad = System.Web.HttpContext.Current.Session["id_ProyectoActividad"];
                    var fecha = System.Web.HttpContext.Current.Session["Fecha"];

                    // int bandera = 0;
                    if (id_ProyectoActividad == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyectoActividad = int.Parse(id_ProyectoActividad.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_ProyectoActividadValor.Id_Usuario = user_id.ToString();
                            mT_ProyectoActividadValor.Fecha = Convert.ToDateTime(fecha);
                            mT_ProyectoActividadValor.Id_Proyecto_Actividad = idProyectoActividad;

                            db.Entry(mT_ProyectoActividadValor).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Proyecto Actividad Valor guardado";

                            return RedirectToAction("ProyectoActividades_Valor", new { id = idProyectoActividad });


                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("ProyectoActividades_Valor", new { id = idProyectoActividad });
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



        //Fiscalizador Proyecto
        #region
        [HttpGet]
        public ActionResult Fiscalizador(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(id);
                    if (mT_Proyecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_Proyecto.Id_Proyecto;
                        var listaFiscalizadorP = db.sp_Quimipac_ConsultaMT_ContratoFiscalizador(id, "Proyecto", empresa_id.ToString()).ToList();
                        var proyecto = mT_Proyecto.Codigo_Cliente;
                        ViewBag.proyecto = proyecto;
                        ViewBag.listaFiscalizadorP = listaFiscalizadorP;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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
        public ActionResult AgregarFiscalizador_Proyecto()
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
            else
            { return RedirectToAction("IniciarSesion", "Home"); }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgregarFiscalizador_Proyecto([Bind(Include = "Id_Proyecto_Contrato, Nombre, Estado, Tipo")] MT_Fiscalizador mT_Fiscalizador)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var contratoFiscalizador = db.MT_Fiscalizador.Where(x => x.Id_Proyecto_Contrato == mT_Fiscalizador.Id_Proyecto_Contrato && x.Nombre == mT_Fiscalizador.Nombre && x.Estado == mT_Fiscalizador.Estado && x.Tipo == mT_Fiscalizador.Tipo);
                            if (contratoFiscalizador.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Proyecto Fiscalizador ya existe";
                                return RedirectToAction("Fiscalizador", new { id = idProyecto });
                            }
                            else
                            {
                                mT_Fiscalizador.Tipo = "Proyecto";
                                mT_Fiscalizador.Id_Proyecto_Contrato = idProyecto;
                                mT_Fiscalizador.Estado = mT_Fiscalizador.Estado.Substring(0, 1);
                                db.MT_Fiscalizador.Add(mT_Fiscalizador);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Fiscalizador de Proyecto guardado";
                                return RedirectToAction("Fiscalizador", new { id = idProyecto });

                            }

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idProyecto });
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

        [HttpGet]
        public ActionResult EditarFiscalizador_Proyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Fiscalizador mT_ProyectoFiscalizador = db.MT_Fiscalizador.Find(id);
                    bool seleccion = false;
                    if (mT_ProyectoFiscalizador != null)
                    {
                        return View(mT_ProyectoFiscalizador);

                    }
                    else
                    {

                        var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                        if (id_Proyecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProyecto = int.Parse(id_Proyecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Fiscalizador", new { id = idProyecto });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditarFiscalizador_Proyecto([Bind(Include = "Id_Fiscalizador, Id_Proyecto_Contrato, Nombre, Estado, Tipo")] MT_Fiscalizador mT_FiscalizadorProyecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    // int bandera = 0;
                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_FiscalizadorProyecto.Estado = mT_FiscalizadorProyecto.Estado.Substring(0, 1);
                            mT_FiscalizadorProyecto.Tipo = "Proyecto";

                            db.Entry(mT_FiscalizadorProyecto).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Fiscalizador de Proyecto actualizado";

                            return RedirectToAction("Fiscalizador", new { id = idProyecto });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Fiscalizador", new { id = idProyecto });
                        }
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

        #endregion


        //PARAMETRO PROYECTO
        #region
        [HttpGet]
        public ActionResult Parametro_Proyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto mT_Proyecto = db.MT_Proyecto.Find(id);
                    if (mT_Proyecto != null)
                    {
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                        System.Web.HttpContext.Current.Session["id_Proyecto"] = mT_Proyecto.Id_Proyecto;
                        var listaParametroP = db.sp_Quimipac_ConsultaMT_ProyectoParametro(id, empresa_id.ToString()).ToList();
                        var proyecto = mT_Proyecto.Codigo_Cliente;
                        ViewBag.proyecto = proyecto;
                        ViewBag.listaParametroP = listaParametroP;
                        return View();
                    }
                    else
                    {
                        TempData["mensaje_error"] = "Código no existe";
                        return RedirectToAction("Proyectos");
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
        public ActionResult AgregarParametro_Proyecto()
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    List<SelectListItem> itemsMoneda = new List<SelectListItem>();
                    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                    var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

                    foreach (var moneda in listaMoneda)
                    {

                        itemsMoneda.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre });

                    }

                    SelectList selectlistaMoneda = new SelectList(itemsMoneda, "Value", "Text");

                    ViewBag.listaMoneda = selectlistaMoneda;

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
        public ActionResult AgregarParametro_Proyecto([Bind(Include = "Id_Proyecto, Parametro, Tipo_Dato, Valor, Estado, Moneda")] MT_Proyecto_Parametro mT_ParametroProyecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }
                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {
                            var proyectoParametro = db.MT_Proyecto_Parametro.Where(x => x.Id_Proyecto == mT_ParametroProyecto.Id_Proyecto && x.Parametro == mT_ParametroProyecto.Parametro && x.Valor == mT_ParametroProyecto.Valor && x.Moneda == mT_ParametroProyecto.Moneda);
                            if (proyectoParametro.Count() >= 1)
                            {
                                TempData["mensaje_error"] = "Proyecto Parametro ya existe";
                                return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });
                            }
                            else
                            {

                                mT_ParametroProyecto.Id_Proyecto = idProyecto;
                                mT_ParametroProyecto.Estado = mT_ParametroProyecto.Estado.Substring(0, 1);
                                db.MT_Proyecto_Parametro.Add(mT_ParametroProyecto);
                                db.SaveChanges();
                                TempData["mensaje_correcto"] = "Parametro de Proyecto guardado";
                                return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });

                            }

                        }

                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });
                        }
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
        public ActionResult EditarParametro_Proyecto(int id)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    MT_Proyecto_Parametro mT_ProyectoParametro = db.MT_Proyecto_Parametro.Find(id);
                    bool seleccion = false;
                    if (mT_ProyectoParametro != null)
                    {
                        List<SelectListItem> itemsMoneda = new List<SelectListItem>();
                        var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

                        var listaMoneda = db.sp_LINK_ConsultaMonedas(empresa_id.ToString()).ToList();

                        foreach (var moneda in listaMoneda)
                        {

                            if (moneda.codmon == mT_ProyectoParametro.Moneda)
                            {
                                seleccion = true;
                            }


                            itemsMoneda.Add(new SelectListItem { Value = Convert.ToString(moneda.codmon), Text = moneda.nombre, Selected = seleccion });
                        }

                        SelectList selectlistaMoneda = new SelectList(itemsMoneda, "Value", "Text");
                        ViewBag.listaMoneda = selectlistaMoneda;

                        return View(mT_ProyectoParametro);

                    }
                    else
                    {

                        var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];

                        if (id_Proyecto == null)
                        {
                            return RedirectToAction("IniciarSesion", "Home");
                        }

                        else
                        {
                            int idProyecto = int.Parse(id_Proyecto.ToString());
                            TempData["mensaje_error"] = "Código no existe";
                            return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditarParametro_Proyecto([Bind(Include = "Id_Proyecto_Parametros, Id_Proyecto, Parametro, Tipo_Dato, Valor, Estado, Moneda")] MT_Proyecto_Parametro mT_ParametroProyecto)
        {
            if (System.Web.HttpContext.Current.Session["usuario"] != null)
            {
                try
                {
                    var id_Proyecto = System.Web.HttpContext.Current.Session["id_Proyecto"];
                    // int bandera = 0;
                    if (id_Proyecto == null)
                    {
                        return RedirectToAction("IniciarSesion", "Home");
                    }

                    else
                    {
                        int idProyecto = int.Parse(id_Proyecto.ToString());

                        if (ModelState.IsValid)
                        {


                            mT_ParametroProyecto.Estado = mT_ParametroProyecto.Estado.Substring(0, 1);


                            db.Entry(mT_ParametroProyecto).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["mensaje_actualizado"] = "Parametro de Proyecto actualizado";

                            return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });

                        }
                        else
                        {
                            TempData["mensaje_error"] = "Valores incorrectos";
                            return RedirectToAction("Parametro_Proyecto", new { id = idProyecto });
                        }
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


        public JsonResult GetCategoria_Edit(int id_proyecto_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Proyecto mt_proyecto = db.MT_Proyecto.Find(id_proyecto_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaCategoria = db.sp_LINK_ConsultaTipoServicio_Cat(empresa_id.ToString(), id_Linea).ToList();
                var listaCateAux = new List<sp_LINK_ConsultaTipoServicio_Cat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                {
                    cia_codigo = empresa,
                    cod_linea = (mt_proyecto.Categoria == "0" ? "OK" : ""),
                    cod_servicio = "0",
                    nombre = "Ninguno"
                });

                foreach (var categoria in listaCategoria)
                {
                    listaCateAux.Add(new sp_LINK_ConsultaTipoServicio_Cat_Result
                    {
                        cia_codigo = empresa,
                        cod_linea = (categoria.cod_servicio == mt_proyecto.Categoria) ? "OK" : "",
                        cod_servicio = categoria.cod_servicio,
                        nombre = categoria.nombre
                    });

                }

                return Json(listaCateAux, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AjaxEditCategoria:" + e.Message);
                return Json(Response.StatusCode);
            }
        }

        public JsonResult GetSubCategoria_Edit(int id_proyecto_Editar, string id_Linea)
        {
            try
            {
                id_Linea = id_Linea.Replace("ampersand", "&");
                MT_Proyecto mt_proyecto = db.MT_Proyecto.Find(id_proyecto_Editar);

                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();

                var listaSubCategoria = db.sp_LINK_ConsultaCentroConsumo_SubCat(empresa_id.ToString(), id_Linea).ToList();
                var listaSubCateAux = new List<sp_LINK_ConsultaCentroConsumo_SubCat_Result>();

                //cod_linea  lo utilizo com ocampo auxiliar
                listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                {
                    cia_codigo = empresa,
                    quimi_linea = (mt_proyecto.Subcategoria == "0" ? "OK" : ""),
                    codcen = "0",
                    nombre = "Ninguno"
                });

                foreach (var subcategoria in listaSubCategoria)
                {
                    listaSubCateAux.Add(new sp_LINK_ConsultaCentroConsumo_SubCat_Result
                    {
                        cia_codigo = empresa,
                        quimi_linea = (subcategoria.codcen == mt_proyecto.Subcategoria) ? "OK" : "",
                        codcen = subcategoria.codcen,
                        nombre = subcategoria.nombre
                    });

                }

                return Json(listaSubCateAux, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("AjaxEditSubCategoria:" + e.Message);
                return Json(Response.StatusCode);
            }
        }


        #endregion


        public JsonResult CrearContrato(MT_Contrato mT_Contrato)
        {
            var msn = new JsonResult();
            var user_id = System.Web.HttpContext.Current.Session["usuario"].ToString();
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"].ToString();
            var nom_empresa = System.Web.HttpContext.Current.Session["empresa_Nombre"].ToString();
            var contratos = db.MT_Contrato.Where(x => x.Codigo_Cliente == mT_Contrato.Codigo_Cliente && x.Nombre == mT_Contrato.Nombre && x.Id_Cliente == mT_Contrato.Id_Cliente && x.Id_Empresa == empresa_id.ToString()).ToList(); /*&& x.Categoria == mT_Contrato.Categoria && x.Subcategoria == mT_Contrato.Subcategoria && x.Id_Linea == mT_Contrato.Id_Linea*/
            if (contratos.Count() >= 1)
            {
                TempData["mensaje_error"] = "Contrato ya existe";
                msn.Data = TempData["mensaje_error"];
                return msn;
            }
            else
            {
                mT_Contrato.Id_Empresa = empresa_id;
                mT_Contrato.Id_Usuario = user_id;

                int? secuencial = 0;

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
                    secuencial = (codigo_guardado.ElementAt(0).Secuencial);
                    SeqRegistro = secuencial + 1;

                    seqregistro2 = Convert.ToInt32(SeqRegistro.ToString());
                
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
                mT_Contrato.Secuencial = seqregistro2;
                mT_Contrato.Fecha_registro = DateTime.Now;

                db.MT_Contrato.Add(mT_Contrato);
                db.SaveChanges();

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
                    Id_Empresa = empresa_id,
                    Id_Linea = null,
                    Id_Proyecto = null,
                    Id_Tipo_Trabajo = null,
                    Id_Usuario = user_id,
                    Usuario = user_id,
                    Id_Contrato = mT_Contrato.Id_Contrato,// db.MT_Contrato.Where(x=>x.)
                    Id_Permiso = 0
                };
                db.MT_Permiso.Add(obPermiso);
                db.SaveChanges();

                msn.Data = mT_Contrato;
                return msn;
            }
        }



    }
}