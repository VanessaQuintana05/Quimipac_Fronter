using Quimipac_.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Quimipac_.Controllers.OrdenTrabajoController;
using static Quimipac_.Controllers.PresupuestoController;

namespace Quimipac_.Repositorio
{
    public class DataBase_Externo
    {
        //ClsNotificacion clsNotificaciones = new ClsNotificaciones();
        public List<ClsNotificacion> LkObtenerTitulos()
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                return (ctx.Database.SqlQuery<ClsNotificacion>("sp_Quimipac_MT_Tablas_XTitulos").ToList());

            }
        }


        public ClsDatosUsuario DatosUsuario(string nombUsuario)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                var RegistrosUsr = ctx.Database.SqlQuery<ClsDatosUsuario>("sp_LINK_ConsultaUsuarios @p0", nombUsuario).SingleOrDefault();
                return (RegistrosUsr);

            }
        }


        public int Verificar_GrupoTrabajoOrden(int id)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                

                var vr = ctx.Database.SqlQuery<ValidateGroup>("sp_Quimipac_ConsultaGrupoTTrabajoOrden @p0", id).ToList();
                

                if (vr.Count>0)
                {
                    return 1;

                }
                else
                {

                    return 0;
                }

                
                //var s = (r.ElementAt(0).GetHashCode());



            }
        }

        //public int Verificar_ActividadesTrabajoOrden(int id)
        //{ /// este de aqui es cuando se iba a verificar actividades peor ya cambie el sp 

        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
               

        //        var ac = ctx.Database.SqlQuery<ValidateGroup>("sp_Quimipac_ConsultaActividadesTTrabajoOrden @p0", id).ToList();


        //        if (ac.Count > 0)
        //        {
        //            return 1;

        //        }
        //        else
        //        {

        //            return 0;
        //        }
                
        //    }
        //}


        //public ParametrosBusquedaOT ObjBusqueda(string fi, string ff, string criterio)
        //{
        //    ParametrosBusquedaOT obj = new ParametrosBusquedaOT();

        //    obj.FeInicio = (new DateTime(long.Parse(fi))).ToShortDateString();
        //    obj.FeFin = (new DateTime(long.Parse(fi))).ToShortDateString();
        //    obj.Parametro = criterio;
        //    return obj;
        //}

        //public List<ParametrosBusquedaOT> BusquedaXParametro(string fi, string  ff, string criterio)
        //{
        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
        //        return (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_Tablas_XTitulos @p0 @p1 @p2", fi, ff, criterio).ToList());
        //    }
        //}

        /*public List<>*/

        public int Verificar_EquipoTrabajoOrden(int id_Orden, int? id_Grupo, DateTime? fecha_Inicio, DateTime? fecha_fin)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.SqlQuery<MT_OrdenTrabajo_Equipo>("sp_Quimipac_ValidarEquipoOdenTrabajo @p0, @p1, @p2, @p3", id_Orden, id_Grupo, fecha_Inicio, fecha_fin).ToList();


                if (ac.Count > 0)
                {
                    return 1;

                }
                else
                {

                    return 0;
                }

            }
        }

        public List<InsertarEquipoOrden> InsertarEquipoOrdenTrabajo(int id_Orden, int count2)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                 
                for (int i = 0; i < count2; i++)
                {
                    var ac = ctx.Database.SqlQuery<InsertarEquipoOrden>("sp_Quimipac_InsertarEquipoOrdenTrabajo @p0", id_Orden).ToList();
                  //var  ac = (ctx.Database.SqlQuery<MT_OrdenTrabajo_Equipo>("sp_Quimipac_InsertarEquipoOrdenTrabajo @p0, @p1, @p2, @p3, @p4", id_Orden, id_Grupo, fecha_Inicio, fecha_fin, estado, count2).ToList());
                
                }
                return null;


            }
        }

     public List<InsertarOrdenTrabajo> InsertarOrdenTrabajoEditNueva(int Id_OrdenTrabajo, int? Id_contrato, string Id_sucursal, int? Id_campamento, int? Id_tipo_trabajo_recibido,
       int? Id_tipo_trabajo_ejecutado, int? Estado, int? Id_sector, int? Id_orden_padre, int? Id_estacion, string Id_usuario, int? Id_entrega_orden_trabajo, int? Nivel_prioridad,
   DateTime? Fecha_creacion_cliente, DateTime? Fecha_maxima_ejecucion_cliente, DateTime? Fecha_asignacion_grupo_trabajo, DateTime? Fecha_finalizacion_obra, DateTime? Fecha_ini_permiso_municipal,
   DateTime? Fecha_fin_permiso_municipal, DateTime? Fecha_entrega, DateTime? Fecha_max_legalizacion, TimeSpan? Hora_acordada, int? Id_barrio, string Direccion, string Referencia_direccion, string Identificacion_suscriptor,
   string Nombre_suscriptor, string Tipo_suscriptor, string Telefono_suscriptor, string Origen, string Comentario, string Porcentaje_avance, TimeSpan? Tiempo_transcurrido, int? Id_planilla, string Estado_orden_planilla,
   string Codigo_Cliente, string Interna)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<InsertarOrdenTrabajo>("sp_Quimipac_InsertarOrdenTrabajoNueva @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20,  @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36",
                    Id_OrdenTrabajo, Id_contrato, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido,
 Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_usuario, Id_entrega_orden_trabajo, Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo,
  Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor,
 Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_planilla, Estado_orden_planilla, Codigo_Cliente, Interna).ToList();
                //var  ac = (ctx.Database.SqlQuery<MT_OrdenTrabajo_Equipo>("sp_Quimipac_InsertarEquipoOrdenTrabajo @p0, @p1, @p2, @p3, @p4", id_Orden, id_Grupo, fecha_Inicio, fecha_fin, estado, count2).ToList());


                return null;


            }
        }

        public int GetTipoTrabajoOrden(int Id_OrdenTrabajo, int? Id_tipo_trabajo_ejecutado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<MT_OrdenTrabajo>("sp_Quimipac_ValidarTipoTrabEjecutado_OrdeTrabajo @p0, @p1", Id_OrdenTrabajo, Id_tipo_trabajo_ejecutado).ToList();

                if (ac.Count > 0)  { return 1;  }
                else  {   return 0;  }
            }
        }


        /*Notificaciones*/

        // public int InsertarNotificacion(int Tipo_Notificacion, string Id_usuario, DateTime Fecha, int Prioridad, string Asunto, string Mensaje, int Criterio, int Tipo, string Correo, int Estado, int Id_Codigo_origen)
        // {
            // using (var ctx = new BD_QUIMIPACEntities())
            // {
                // var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Notificaciones_Ins @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7,@p8,@p9,@p10", Tipo_Notificacion, Id_usuario, Fecha, Prioridad, Asunto, Mensaje, Criterio, Tipo, Correo, Estado, Id_Codigo_origen);
                // return 0;
            // }
        // }
public int InsertarNotificacion(int Tipo_Notificacion, string Id_usuario, DateTime Fecha, int Prioridad, string Asunto, string Mensaje, int Criterio, int Tipo, string Correo, int Estado, int Id_Codigo_origen, int Id_Notificacion)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Notificaciones_Ins @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7", Tipo_Notificacion, Id_usuario, Fecha, Prioridad, Asunto, Mensaje, Criterio, Id_Codigo_origen);

				string[] Separado = Correo.Split(';');
				int Id_NotificacionPer = 0;
				int? IdPersona = 0;
				foreach (var item in Separado)
				{
					var lista = ConsultaPersonas(item);
					
					if (lista != null) 
					{
						//var busqueda = ctx.MT_Notificaciones.Where(A => A.Fecha==Fecha && A.Asunto==Asunto && A.Id_Codigo_origen== Id_Codigo_origen && A.Mensaje==Mensaje && A.Prioridad==Prioridad && A.Id_usuario==Id_usuario && A.Criterio==Criterio && A.Tipo_Notificacion==Tipo_Notificacion);
						
						//if (busqueda!=null)
						//{
							//foreach (var items in busqueda)
							//{
								//Id_NotificacionPer = items.Id_Notificacion;
								//break;
							//}
							foreach (var itens in lista)
							{
								IdPersona = Convert.ToInt32(itens.id_persona);
								break;
							}

							MT_Notificacion_Persona obj = new MT_Notificacion_Persona()
							{
								Id_Notificacion = Id_NotificacionPer,
								Id_Persona = IdPersona,
								Tipo = Tipo,
								Correo = item,
							    Estado = Estado

							};

							var objectContext = ((IObjectContextAdapter)ctx).ObjectContext;
							objectContext.AddObject("MT_Notificacion_Persona", obj);
							ctx.SaveChanges();
						//}

					}
				}
				return 0;
			}
		}


public List<sp_LINK_ConsultaUsuarios_Result> ConsultaPersonas(string CorreoPersona)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				CorreoPersona = CorreoPersona.Trim();
				if (CorreoPersona != null && CorreoPersona!="")
				{
					var Personas = (ctx.Database.SqlQuery<sp_LINK_ConsultaUsuarios_Result>("sp_Quimipac_ConsultaPerCorreo @p0", CorreoPersona).ToList());

					return Personas;
				}
				return null;
			}
		}

		//funcion criterio
		public int GetCriterioNoti(String Descripcion)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				var ac = ctx.Database.SqlQuery<CriterioNoti>("sp_Quimipac_ConsultaCriterioNoti @p0,@p1", "Notificacion", Descripcion).ToList();

				if (ac.Count > 0)
				{
					foreach (var item in ac)
					{
						/*var aci0 =item.Id_TablaDetalle;*/
						return item.Id_TablaDetalle;
					}
				}
				return 0;
			}
		}
		//obtener id para deducir que la notificacion es de entrada
		public List<MT_Notificaciones> LkObtenerNotificacion(int IdNotificacion)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				if (IdNotificacion > 0)
				{
					var listas = (ctx.Database.SqlQuery<MT_Notificaciones>("sp_Quimipac_Consulta_Notificaciones_Entrada @p0,@p1", "Entrada_Noti", IdNotificacion).ToList());
					return listas;
				}
				else
				{
					var Listas = (ctx.Database.SqlQuery<MT_Notificaciones>("sp_Quimipac_Consulta_Notificaciones_Entrada @p0,@p1", "Entrada_Noti", 0).ToList());
					return Listas;
				}
			}
		}
		//traer el correo de la tabla notificacion persona por id
		public List<MT_Notificacion_Persona> LkObtenerCorreo(int Id_Notificacion)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				return (ctx.Database.SqlQuery<MT_Notificacion_Persona>("sp_Quimipac_ConsultaMT_Notificacion_PersonaCorreo @p0", Id_Notificacion).ToList());

			}
		}

		// funcion que actualiza el estado en MT_Notificacion_Persona 
		public void LkActualizaEstado(int Id_Estado)
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				var lkLista = ctx.MT_Notificacion_Persona.Where(a => a.Id_Notificacion == Id_Estado);
				if (lkLista.Count() > 0)
				{
					foreach (var estado in lkLista)
					{
						estado.Estado = 93;
						estado.Fecha_Hora = DateTime.Now;
					}
					ctx.SaveChanges();
				}
			}
		}
		// consultar parametros smtp
		public List<ParametrosSmtp> LkParametrosSMTP()
		{
			using (var ctx = new BD_QUIMIPACEntities())
			{
				var SMTPParam = (ctx.Database.SqlQuery<ParametrosSmtp>("sp_Quimipac_Consulta_ParametrosSMTP").ToList());
				return SMTPParam;
			}

		}
		
      
        //Notificaciones

        /*MT_PROYECTO*/
        public int InsertarMTProyecto(MT_ProyectoPro p)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {
                  var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Proyecto_INS @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20",
                          p.Id_Empresa
                        , p.Id_Sucursal
                        , p.Id_Cliente
                        , p.Id_TipoTrabajo
                        , p.Id_Presupuesto
                        , p.Fecha_Registro
                        , p.Fecha_Inicio
                        , p.Fecha_Fin
                        , p.Fecha_Prorroga
                        , p.Estado
                        , p.Codigo_Interno
                        , p.Codigo_Cliente
                        , p.Direccion
                        , p.Ubicacion
                        , p.Comentario
                        , p.Porcentaje_avance
                        , p.Valor_Inicial
                        , p.Valor_Ajustado
                        , p.Linea
                        , p.Categoria
                        , p.Subcategoria

                    );
                return 1;
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }




        
        /*BusquedaXParametro ot   Fecha_asignacion_grupo_trabajo,Fecha_registro,Origen,Id_tipo_trabajo_ejecutado,Id_campamento,Id_estacion,Id_sector,Id_sucursal*/
        public List<ParametrosBusquedaOT> BusquedaXParametro(DateTime? fi, DateTime? ff, string criterio,int? Id_tipo_trabajo_ejecutado,int? Id_campamento,int? Id_estacion,int? Id_sector, string Id_sucursal) { 
            using (var ctx = new BD_QUIMIPACEntities())
            {
                string XParametro = string.Empty;
                int? XParametroInt = 0;

                if (criterio.Equals( "Ninguno")) { XParametroInt = 0; }
                else if (criterio.Equals("Tipos_Trabajo")) { XParametroInt = Id_tipo_trabajo_ejecutado; }
                else if (criterio.Equals("Campamento")) { XParametroInt = Id_campamento; }
                else if (criterio.Equals("Estacion")) { XParametroInt = Id_estacion; }
                else if (criterio.Equals("Sector")) { XParametroInt = Id_sector; }
                else if (criterio.Equals("Sucursal")) { XParametro = Id_sucursal; }

                var lksql = (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_OrdenTrabajo_QryXParametro @p0, @p1, @p2,@p3,@p4", fi, ff, criterio, XParametroInt, XParametro).ToList());
                return lksql;
            }
        }

        /*MT_ORDEN TRABAJO PARAMETRO*/
        public List<MT_TipoTrabajo> ParametroLkTipoTrabajo(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_TipoTrabajo>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());
                return lksql;
            }
        }

        public List<MT_Campamento> ParametroLkCampamento(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_Campamento>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());
                return lksql;
            }
        }

        public List<sp_LINK_ConsultaSucursal_Result> ParametroLkSucursal(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_LINK_ConsultaSucursal_Result>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());
                return lksql;
            }
        }

        public List<MT_Estacion> ParametroLkEstacion(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_Estacion>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());
                return lksql;
            }
        }

        public List<MT_Sector> ParametroLkSector(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_Sector>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());
                return lksql;
            }
        }

        //public List<MT_TipoTrabajo> ObtenerCriterioAccionOT(int IdTTEjecutado, string criterio) {
        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
        //        var lksql = ctx.MT_TipoTrabajo.Where(v => v.Id_TipoTrabajo == IdTTEjecutado);
        //        //(ctx.Database.SqlQuery<MT_Sector>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0", criterio).ToList());

        //        //from cliente in ctx.MT_TipoTrabajo
        //        //select cliente;
        //    }
        //}


        public MT_TipoTrabajo ObtenerCriterioAccionOT(int? IdTTEjecutado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var mtTIpoTrabajo = ctx.MT_TipoTrabajo.Where(v => v.Id_TipoTrabajo == IdTTEjecutado).SingleOrDefault(); ;  //ctx.Database.SqlQuery<MT_TipoTrabajo>("sp_LINK_ConsultaUsuarios @p0", nombUsuario).SingleOrDefault();
                return (mtTIpoTrabajo);

            }
        }

        /*Precio Referencial*/
        public int InsertarPrecioRef(int Id_TipoTablaDetalle, int Id_Proyecto_Contrato, string Id_Item, int Id_TipoTrabajo, string Id_Usuario,DateTime Fecha_Inicio, DateTime Fecha_Fin, string Moneda, decimal Precio, decimal Costo, string Estado)
        {

            
            using (var ctx = new BD_QUIMIPACEntities())
            {
                
               
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_PrecioReferencial_INS_UPD_X @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7,@p8,@p9,@p10,@p11", "INS",Id_TipoTablaDetalle, Id_Proyecto_Contrato, Id_Item, Id_TipoTrabajo, Id_Usuario, Fecha_Inicio, Fecha_Fin, Moneda, Precio, Costo,Estado);
                if (ac <= 0)
                {
                    return 0;
                } else { return ac;}
            }
        }

        /*EntregaOrden*/

        public int InsertarEntregaOrden(List<TempEntregaOrden> lkOrden, string Id_Empresa, string Id_Cliente, DateTime Fecha_Envio, DateTime Fecha_Recepcion, string Usuario, string Comentario, string Recibido_Por)
        {
            int cod = 0;

            //using (var db = new MyDbContext())
            //{
            //    db.Database.Connection.Open();
            //    var con = (SqlConnection)db.Database.Connection;
            //    var cmd = new SqlCommand("exec MyProc", con);
            //    DataTable dt = new DataTable();
            //    using (var rdr = cmd.ExecuteReader())
            //    {
            //        dt.Load(rdr);
            //    }


            using (var ctx = new BD_QUIMIPACEntities())
            {
                    ctx.Database.Connection.Open();
                    var con = (SqlConnection)ctx.Database.Connection;
                SqlCommand cmd = new SqlCommand("sp_Quimipac_OrdenEntregaOrden", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Criterio", "INS");
                cmd.Parameters.AddWithValue("@orden", 0);
                cmd.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
                cmd.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);
                //cmd.Parameters.AddWithValue("@Fecha_Elaboracion", Fecha_Elaboracion);
                cmd.Parameters.AddWithValue("@Fecha_Envio", Fecha_Envio);
                cmd.Parameters.AddWithValue("@Fecha_Recepcion", Fecha_Recepcion);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@Comentario", Comentario);
                cmd.Parameters.AddWithValue("@Recibido_Por", Recibido_Por); 
                cmd.Parameters.AddWithValue("@id", 0);

                try
                {
                    //con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        cod = Convert.ToInt32(dr["IdActual"].ToString());
                    }
                    dr.Close();
                    if (lkOrden != null)
                    {


                        foreach (var item in lkOrden)
                        {
                            SqlCommand cmd2 = new SqlCommand("sp_Quimipac_OrdenEntregaOrden", con);
                            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@Criterio", "UPD");
                            cmd2.Parameters.AddWithValue("@orden", item.Id_OrdenTrabajo);
                            cmd2.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
                            cmd2.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);
                            //cmd2.Parameters.AddWithValue("@Fecha_Elaboracion", Fecha_Elaboracion);
                            cmd2.Parameters.AddWithValue("@Fecha_Envio", Fecha_Envio);
                            cmd2.Parameters.AddWithValue("@Fecha_Recepcion", Fecha_Recepcion);
                            cmd2.Parameters.AddWithValue("@Usuario", Usuario);
                            cmd2.Parameters.AddWithValue("@Comentario", Comentario);
                            cmd2.Parameters.AddWithValue("@Recibido_Por", Recibido_Por);
                            cmd2.Parameters.AddWithValue("@id", cod);

                            //con.Open();
                            cmd2.ExecuteNonQuery();
                        }
                        return 1;
                    }
                    else { return cod; }
                    
                }
                catch (SqlException ex)
                {
                    //return 0;
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw new System.Exception(ex.Message);
                }

                //var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_OrdenEntregaOrden @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7, @p8", "INS", orden, Id_Empresa, Id_Cliente, Fecha_Elaboracion, Fecha_Envio, Fecha_Recepcion, Usuario, Comentario, Recibido_Por, id);
                //if (ac <= 0)
                //{
                //    return 0;
                //}
                //else { return ac; }
            }
        }


        //db.Roles.Where(v => v.Estado == "A").ToList();

        public List<sp_LINK_ConsultaStock_Result> ObtenerStock()
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var listaStock = (ctx.Database.SqlQuery<sp_LINK_ConsultaStock_Result>("sp_LINK_ConsultaStock").ToList());
                return listaStock;
            }
        }

        public List<sp_Quimipac_ConsultaMt_ProgramaTrabajo_Result> ObtenerPrograma()
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var listaPrograma = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMt_ProgramaTrabajo_Result>("sp_Quimipac_ConsultaMt_ProgramaTrabajo").ToList());
                return listaPrograma;
            }
        }

        public int GetTipoTrabajoProyecto(int Id_Proyecto, int? Id_tipo_trabajo)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<MT_Proyecto>("sp_Quimipac_ValidarTipoTraboProyecto @p0, @p1", Id_Proyecto, Id_tipo_trabajo).ToList();

                if (ac.Count > 0) { return 1; }
                else { return 0; }
            }
        }

        public List<InsertarSolicitud> InsertarSolicitudWeb(int dia , string codpro, int estado , DateTime? hora, string observacion, string usuario , DateTime? fecha_pro_comp , string observ_compras , string codcen , int? dias_prov , DateTime? fecha_disp , string observ_disp , string observ_pago , string usuario_compra ,
        string cod_suc ,  string categoria , string elemento , string num_ped_cotiz , string usuario_aprobador , string cia_codigo )
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<InsertarSolicitud>("sp_Quimipac_Link_InsertarSolicitud @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19",
                   dia, codpro, estado, hora, observacion, usuario, fecha_pro_comp, observ_compras, codcen, dias_prov, fecha_disp, observ_disp, observ_pago, usuario_compra, cod_suc, categoria, elemento, num_ped_cotiz, usuario_aprobador, cia_codigo).ToList();
                //var  ac = (ctx.Database.SqlQuery<MT_OrdenTrabajo_Equipo>("sp_Quimipac_InsertarEquipoOrdenTrabajo @p0, @p1, @p2, @p3, @p4", id_Orden, id_Grupo, fecha_Inicio, fecha_fin, estado, count2).ToList());
                return null;
            }
        }


        /*Permisos*/

        public List<sp_Quimipac_ConsultaUsuarios_Permisos_Result> ObtenerUsuariosPermisos()
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaUsuarios_Permisos_Result>("sp_Quimipac_ConsultaUsuarios_Permisos @p0", "").ToList());


                var lkDataRol = new List<sp_Quimipac_ConsultaUsuarios_Permisos_Result>();

                // for(int vq =0; vq < lksql.Count(); vq++ )
                foreach (var item in lksql)
                {

                    if (!lkDataRol.Exists(x => x.user_id.Equals(item.user_id))) //.Contains(lkDataRol.ElementAt(vq).User_id))
                    {

                        var Obj = new sp_Quimipac_ConsultaUsuarios_Permisos_Result
                        {
                            user_id = item.user_id,
                            //....user_clave = item.user_clave,
                            user_descrip = item.user_descrip,
                            user_status = item.user_status,
                            //User_dep = item.User_dep,
                            //User_cargo = item.User_cargo,
                            //User_cheq_nivel = item.User_cheq_nivel,
                            //User_cheq_maquina = item.User_cheq_maquina,
                            //User_uni_inc = item.User_uni_inc,
                            //Elimina = item.Elimina,
                            email = item.email,
                            /**/
                            //....  id_persona = item.id_persona,
                            primer_nombre = item.primer_nombre,
                            segundo_nombre = item.segundo_nombre,
                            primer_apellido = item.primer_apellido,
                            segundo_apellido = item.segundo_apellido,
                            //Fecha_nacimiento = item.Fecha_nacimiento,
                            //Genero = item.Genero,
                            //Estado_civil = item.Estado_civil,
                            //Tipo_identifica1 = item.Tipo_identifica1,
                            //Nro_identifica1 = item.Nro_identifica1,
                            //Tipo_identifica2 = item.Tipo_identifica2,
                            //Nro_identifica2 = item.Nro_identifica2,
                            //....  correo = item.correo,
                            //Nacionalidad = item.Nacionalidad,
                            //Correo_laboral = item.Correo_laboral,
                            //Tipo_identifica3 = item.Tipo_identifica3,
                            //Nro_identifica3 = item.Nro_identifica3,
                            /**/

                            Id_Permiso = item.Id_Permiso,
                            Id_Usuario = item.Id_Usuario,
                            Id_Cliente = item.Id_Cliente,
                            Id_Contrato = item.Id_Contrato,
                            Id_Proyecto = item.Id_Proyecto,
                            Id_Tipo_Trabajo = item.Id_Tipo_Trabajo,
                            Id_Linea = item.Id_Linea,
                            Consultar = item.Consultar,
                            Modificar = item.Modificar,
                            Crear = item.Crear,
                            Eliminar = item.Eliminar,
                            Usuario = item.Usuario,
                            Fecha_Registro = item.Fecha_Registro,
                            Estado = item.Estado

                        };

                        lkDataRol.Add(Obj);
                    }
                }

                return lkDataRol;
            }
        }
        //db.Roles.Where(v => v.Estado == "A").ToList();

        public List<sp_LINK_ConsultaClientes_Result> ObtenerClientes(string criterio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_LINK_ConsultaClientes_Result>("sp_LINK_ConsultaClientes").ToList());

                //List<SelectListItem> itemsClientes = new List<SelectListItem>();
                return lksql;
            }
        }

        [System.Web.Services.WebMethod]
        public List<sp_Quimipac_ConsultaMT_Permisos_XContratos_Result> ObtenerContratos(string IdEmpresa, int IdCliente)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_Permisos_XContratos_Result>("sp_Quimipac_ConsultaMT_Permisos_XContratos @p0,@p1", IdEmpresa, IdCliente).ToList());

                //List<SelectListItem> itemsClientes = new List<SelectListItem>();
                return lksql;
            }
        }

        public List<sp_Quimipac_ConsultaMT_Permisos_XProyectos_Result> ObtenerProyectos(string IdEmpresa, int IdCliente)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_Permisos_XProyectos_Result>("sp_Quimipac_ConsultaMT_Permisos_XProyectos @p0,@p1", IdEmpresa, IdCliente).ToList());

                //List<SelectListItem> itemsClientes = new List<SelectListItem>();
                return lksql;
            }
        }
        /*
        public List<MT_Permiso_Guardar> GuardarMTPermisos(string Id_Usuario, string Id_Cliente, int? Id_Contrato, int? Id_Proyecto, string Consultar, string Modificar, string Crear, string Eliminar, string Usuario, DateTime Fecha, string Estado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_Permiso_Guardar>("sp_Quimipac_GuardarMT_Permisos @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", Id_Usuario, Id_Cliente,Id_Contrato, Id_Proyecto, Consultar, Modificar, Crear, Eliminar, Usuario, Fecha, Estado).ToList());

                //List<SelectListItem> itemsClientes = new List<SelectListItem>();
                return lksql;
            }
        }
        */
        public int GuardarMTPermisos(string Id_Usuario, string Id_Cliente, int? Id_Contrato, int? Id_Proyecto, string Consultar, string Modificar, string Crear, string Eliminar, string Usuario, DateTime Fecha, string Estado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_GuardarMT_Permisos @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", Id_Usuario, Id_Cliente, Id_Contrato, Id_Proyecto, Consultar, Modificar, Crear, Eliminar, Usuario, Fecha, Estado);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }



        //Editar Permisoa Usuarios
        public List<sp_Quimipac_ConsultaMT_Permisos_Usuario_Result> ObtenerPermisosXUsuario(string IdUsuario)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_Permisos_Usuario_Result>("sp_Quimipac_ConsultaMT_Permisos_Usuario @p0", IdUsuario).ToList());
                return lksql;
            }
        }

        //actualizar estados y opciones Permisos
        public int ActualizarMTPermisos(int Id_Permiso, string Consultar, string Modificar, string Crear, string Eliminar, string Usuario, string Estado)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Editar_MTPermisos @p0,@p1,@p2,@p3,@p4,@p5,@p6", Id_Permiso, Consultar, Modificar, Crear, Eliminar, Usuario, Estado);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }

        //public int InsertarProyecto(List<TempProyecto> lkProyecto, string Id_Empresa, string Id_Cliente, string Id_Sucursal, string Id_Usuario, string Comentario, decimal Porcentaje, decimal Monto_Certificacion, decimal Iva, decimal Validez, string Moneda)
        //{
        //    int cod = 0;

        //    //using (var db = new MyDbContext())
        //    //{
        //    //    db.Database.Connection.Open();
        //    //    var con = (SqlConnection)db.Database.Connection;
        //    //    var cmd = new SqlCommand("exec MyProc", con);
        //    //    DataTable dt = new DataTable();
        //    //    using (var rdr = cmd.ExecuteReader())
        //    //    {
        //    //        dt.Load(rdr);
        //    //    }


        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
        //        ctx.Database.Connection.Open();
        //        var con = (SqlConnection)ctx.Database.Connection;
        //        SqlCommand cmd = new SqlCommand("sp_Quimipac_InsProyectoPresupuesto", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Criterio", "INS");
        //        cmd.Parameters.AddWithValue("@proyecto", 0);
        //        cmd.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
        //        cmd.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);
        //        cmd.Parameters.AddWithValue("@Id_Sucursal", Id_Sucursal);
        //        cmd.Parameters.AddWithValue("@Usuario", Id_Usuario);
        //        cmd.Parameters.AddWithValue("@Comentario", Comentario);
        //        cmd.Parameters.AddWithValue("@porcentaje", Porcentaje);
        //        cmd.Parameters.AddWithValue("@monto", Monto_Certificacion);
        //        cmd.Parameters.AddWithValue("@iva", Iva);
        //        cmd.Parameters.AddWithValue("@validez", Validez);
        //        cmd.Parameters.AddWithValue("@Moneda", Moneda);
        //        cmd.Parameters.AddWithValue("@id", 0);

        //        try
        //        {
        //            //con.Open();
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.Read())
        //            {
        //                cod = Convert.ToInt32(dr["IdActual"].ToString());
        //            }
        //            dr.Close();
        //            if (lkProyecto != null)
        //            {


        //                foreach (var item in lkProyecto)
        //                {
        //                    SqlCommand cmd2 = new SqlCommand("sp_Quimipac_InsProyectoPresupuesto", con);
        //                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
        //                    cmd2.Parameters.AddWithValue("@Criterio", "UPD");
        //                    cmd2.Parameters.AddWithValue("@proyecto", item.Id_Proyecto);
        //                    cmd2.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
        //                    cmd2.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);
        //                    cmd2.Parameters.AddWithValue("@Id_Sucursal", Id_Sucursal);
        //                    cmd2.Parameters.AddWithValue("@Usuario", Id_Usuario);
        //                    cmd2.Parameters.AddWithValue("@Comentario", Comentario);
        //                    cmd2.Parameters.AddWithValue("@porcentaje", Porcentaje);
        //                    cmd2.Parameters.AddWithValue("@monto", Monto_Certificacion);
        //                    cmd2.Parameters.AddWithValue("@iva", Iva);
        //                    cmd2.Parameters.AddWithValue("@validez", Validez);
        //                    cmd2.Parameters.AddWithValue("@Moneda", Moneda);
        //                    cmd2.Parameters.AddWithValue("@id", cod);

        //                    //con.Open();
        //                    cmd2.ExecuteNonQuery();
        //                }
        //                return 1;
        //            }
        //            else { return cod; }

        //        }
        //        catch (SqlException ex)
        //        {
        //            //return 0;
        //            System.Diagnostics.Debug.WriteLine(ex.Message);
        //            throw new System.Exception(ex.Message);
        //        }

        //        //var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_OrdenEntregaOrden @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7, @p8", "INS", orden, Id_Empresa, Id_Cliente, Fecha_Elaboracion, Fecha_Envio, Fecha_Recepcion, Usuario, Comentario, Recibido_Por, id);
        //        //if (ac <= 0)
        //        //{
        //        //    return 0;
        //        //}
        //        //else { return ac; }
        //    }
        //}

        public int InsertarPresupuesto(string Id_Empresa, string Id_Sucursal, string Id_Cliente, string Id_Usuario, string Comentario, decimal? Porcentaje, decimal? Monto_Certificacion, decimal? Iva, string Validez, string Moneda)
        {


            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_InsProyectoPresupuesto @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7,@p8,@p9,@p10", "INS", Id_Empresa, Id_Sucursal, Id_Cliente, Id_Usuario, Comentario, Porcentaje, Monto_Certificacion, Iva, Validez, Moneda);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }

        
        public int InsertarEstadoOrden(int Id_OrdenTrabajo, int? Estado)
        {


            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_InsEstadoOrdenTrabajo @p0, @p1, @p2", "INS", Id_OrdenTrabajo, Estado);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }

        
        public int ActualizarOrdenTrabajo(int? Id_OrdenTrabajo, int? Estado, DateTime? FechaI, DateTime? FechaF)
        {


            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_ActEstadoOrdenTrabajo @p0, @p1, @p2, @p3", Id_OrdenTrabajo, Estado, FechaI, FechaF);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }


    }


}