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
        #region Cifrado Cesar desplazamiento

        string abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ABCDE";
        //static string abc = "abcdefghijklmñnopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890_-+,#$%&/()=¿?¡!|,.;:{}[]";
        //static void Main(string[] args)
        //{

        //    String mensaje = "Cada vez que escucho hablar de ese gato, empiezo a sacar mi pistola [en relación a la Paradoja de Schrödinger]";

        //    //se cifra el mensaje
        //    string tmp = cifrar(mensaje, 10);


        //    Console.WriteLine("Mensaje cifrado: \"{0}\" \n\n", tmp);//se muestra en pantalla

        //    //se descifra el mensaje
        //    Console.WriteLine("Mensaje descifrado: \"{0}\" \n\n", descifrar(tmp, 10));

        //    //si se descifra con un desplazamiento diferente al que se uso para cifrar
        //    //el mensaje sera el equivocado
        //    Console.WriteLine("Mensaje descifrado con desplazamiento equivocado:\n \"{0}\"", descifrar(tmp, 7));

        //    Console.ReadKey();
        //}


        public string cifrar(string mensaje, int desplazamiento)
        {
            String cifrado = "";
            if (desplazamiento > 0 && desplazamiento < abc.Length)
            {
                //recorre caracter a caracter el mensaje a cifrar
                for (int i = 0; i < mensaje.Length; i++)
                {
                    int posCaracter = getPosABC(mensaje[i]);
                    if (posCaracter != -1) //el caracter existe en la variable abc
                    {
                        int pos = posCaracter + desplazamiento;
                        while (pos >= abc.Length)
                        {
                            pos = pos - abc.Length;
                        }
                        //concatena al mensaje cifrado
                        cifrado += abc[pos];
                    }
                    else//si no existe el caracter no se cifra
                    {
                        cifrado += mensaje[i];
                    }
                }

            }
            return cifrado;
        }

        /* 
 * El descifrado cesar es el procedimiento inverso al cifrado
 */
        public string descifrar(string mensaje, int desplazamiento)
        {

            // descifrar(tmp, 10)
            
            String cifrado = "";
            if (desplazamiento > 0 && desplazamiento < abc.Length)
            {
                for (int i = 0; i < mensaje.Length; i++)
                {
                    int posCaracter = getPosABC(mensaje[i]);
                    if (posCaracter != -1) //el caracter existe en la variable abc
                    {
                        int pos = posCaracter - desplazamiento;
                        while (pos < 0)
                        {
                            pos = pos + abc.Length;
                        }
                        cifrado += abc[pos];
                    }
                    else
                    {
                        cifrado += mensaje[i];
                    }
                }

            }
            return cifrado;
        }

        /* obtiene la posicion del caracter pasado como parametro 
  * en la variable abc que es nuestro abecedario de cifrado/descifrado
  */
        public int getPosABC(char caracter)
        {
            for (int i = 0; i < abc.Length; i++)
            {
                if (caracter == abc[i])
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion



        BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        //27-05-2019 fitro de OT para canvas
        public string IsValidParametroCANVAS_OT(int Orden_trabajoID, int? TipoTrabajoId) //string codigo
        {
            DataBase_Externo dbe = new DataBase_Externo();
            

            var lk_OrdenTrabajoTT = db.MT_OrdenTrabajo.Where(vq => vq.Id_tipo_trabajo_ejecutado == TipoTrabajoId).ToList();
            var TipoTrabajoOT = db.MT_TipoTrabajo.Where(vq => vq.Id_TipoTrabajo == TipoTrabajoId).SingleOrDefault();

            //DateTime fAsignacion = Convert.ToDateTime(lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion_grupo_trabajo);
            DateTime fAsignacion = Convert.ToDateTime(lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion);

            //if (lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion_grupo_trabajo != null)
            if (lk_OrdenTrabajoTT.ElementAt(0).Fecha_asignacion != null)
            {
                DateTime fActual = (dbe.ReturnFechaActual_SQL().ElementAt(0).FechaActual_SQL);

                DateTime? DateProceso = null;
                DateTime? DateAlerta = null;
                DateTime? DateCaida = null;

                if (TipoTrabajoOT != null)
                {
                    DateProceso = fAsignacion.AddDays(Convert.ToInt32(TipoTrabajoOT.Proceso));
                    DateAlerta = fAsignacion.AddDays(Convert.ToInt32(TipoTrabajoOT.Alerta));
                    DateCaida = fAsignacion.AddDays(Convert.ToInt32(TipoTrabajoOT.Caida));

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


        public bool Actividad_Iniciada(int id_Orden)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var actividadiniciada = (ctx.Database.SqlQuery<clsUtil>("select Fecha_inicio_ejecucion as VariableDateTime from MT_OrdenTrabajo where EstadoEditOrden = 'A' and Id_OrdenTrabajo=" + id_Orden).ToList());
                if (actividadiniciada.Count>0)
                {//{01/01/0001 
                    var booleanotrue = (actividadiniciada.ElementAt(0).VariableDateTime == null)?false: true;
                    return booleanotrue;
                }
                else { return false; }
            }

            //return false;
        }






        //ClsNotificacion clsNotificaciones = new ClsNotificaciones();

        public List<clsUtil> ReturnFechaActual_SQL()
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                return (ctx.Database.SqlQuery<clsUtil>("select GETDATE() as  FechaActual_SQL").ToList());

            }
        }




        //public string IsValidParametro(int Orden_trabajoID, int? ContratoID)
        /* 
        public string IsValidParametro(int Orden_trabajoID, int? ContratoID, string codigo)
        {
            DataBase_Externo dbe = new DataBase_Externo();
            BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();

            var lkContratoParam = db.MT_Contrato_Parametro.Where(vq => vq.Id_Contrato == ContratoID);
            var lkOT = db.MT_OrdenTrabajo.Where(vq => vq.Id_OrdenTrabajo == Orden_trabajoID && vq.Codigo_Cliente == codigo).ToList();

            //DateTime fAsignacion = Convert.ToDateTime(lkOT.ElementAt(0).Fecha_asignacion_grupo_trabajo);
            DateTime fAsignacion = Convert.ToDateTime(lkOT.ElementAt(0).Fecha_asignacion);
            //if (lkOT.ElementAt(0).Fecha_asignacion_grupo_trabajo != null)
            if (lkOT.ElementAt(0).Fecha_asignacion != null)
            {
                DateTime fActual = (dbe.ReturnFechaActual_SQL().ElementAt(0).FechaActual_SQL);

                DateTime? DateProceso = null;
                DateTime? DateAlerta = null;
                DateTime? DateCaida = null;

                int CountParametros = 0;
                foreach (var item in lkContratoParam)
                {
                    string parametro = item.Parametro.ToUpper();
                    switch (parametro)
                    {
                        case "EN PROCESO": DateProceso = fAsignacion.AddDays(Convert.ToInt32(item.Valor_Final)); CountParametros++; break;
                        case "ALERTA": DateAlerta = fAsignacion.AddDays(Convert.ToInt32(item.Valor_Final)); CountParametros++; break;
                        case "CAIDA": DateCaida = fAsignacion.AddDays(Convert.ToInt32(item.Valor_Inicial)); CountParametros++; break;
                    }
                }

                //debe devolverme solo 2 que tienen los 3
                 
                if(CountParametros==3) //(DateProceso != null && DateAlerta != null && DateCaida != null)
                {

               

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
            else { return null;}
            
        }
        */

        public List<ClsNotificacion> LkObtenerTitulos()
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                return (ctx.Database.SqlQuery<ClsNotificacion>("sp_Quimipac_MT_Tablas_XTitulos").ToList());

            }
        }


        public ClsDatosUsuario DatosUsuario(string nombUsuario)
        {

            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            string empresa = empresa_id.ToString();
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var RegistrosUsr = ctx.Database.SqlQuery<ClsDatosUsuario>("sp_LINK_ConsultaUsuarios @p0,@p1", nombUsuario, empresa).SingleOrDefault();
                return (RegistrosUsr);

            }
        }


        public int Verificar_GrupoTrabajoOrden(int id)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {


                var vr = ctx.Database.SqlQuery<ValidateGroup>("sp_Quimipac_ConsultaGrupoTTrabajoOrden @p0", id).ToList();


                if (vr.Count > 0)
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
      DateTime? Fecha_creacion_cliente, DateTime? Fecha_maxima_ejecucion_cliente, DateTime? Fecha_asignacion_grupo_trabajo, DateTime? Fecha_asignacion, DateTime? Fecha_finalizacion_obra, DateTime? Fecha_ini_permiso_municipal,
      DateTime? Fecha_fin_permiso_municipal, DateTime? Fecha_entrega, DateTime? Fecha_max_legalizacion, TimeSpan? Hora_acordada, int? Id_barrio, string Direccion, string Referencia_direccion, string Identificacion_suscriptor,
      string Nombre_suscriptor, string Tipo_suscriptor, string Telefono_suscriptor, string Origen, string Comentario, string Porcentaje_avance, TimeSpan? Tiempo_transcurrido, int? Id_planilla, string Estado_orden_planilla,
      string Codigo_Cliente, string Interna, bool? Excel_orden, DateTime? Fecha_maxima_contratista, bool? desalojo, bool? automatizacion, DateTime? fecha_inicio_ejecucion, DateTime? fecha_fin_ejecucion, int? Id_Prospecto)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<InsertarOrdenTrabajo>("sp_Quimipac_InsertarOrdenTrabajoNueva @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20,  @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36, @p37,@p38,@p39,@p40,@p41,@p42,@p43,@p44",
                    Id_OrdenTrabajo, Id_contrato, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido,
 Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_usuario, Id_entrega_orden_trabajo, Nivel_prioridad, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_asignacion,
  Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor,
 Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance, Tiempo_transcurrido, Id_planilla, Estado_orden_planilla, Codigo_Cliente, Interna, Excel_orden, Fecha_maxima_contratista, desalojo, automatizacion, fecha_inicio_ejecucion, fecha_fin_ejecucion, Id_Prospecto).ToList();
                //var  ac = (ctx.Database.SqlQuery<MT_OrdenTrabajo_Equipo>("sp_Quimipac_InsertarEquipoOrdenTrabajo @p0, @p1, @p2, @p3, @p4", id_Orden, id_Grupo, fecha_Inicio, fecha_fin, estado, count2).ToList());


                return null;


            }
        }

        public int GetTipoTrabajoOrden(int Id_OrdenTrabajo, int? Id_tipo_trabajo_ejecutado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<MT_OrdenTrabajo>("sp_Quimipac_ValidarTipoTrabEjecutado_OrdeTrabajo @p0, @p1", Id_OrdenTrabajo, Id_tipo_trabajo_ejecutado).ToList();

                if (ac.Count > 0) { return 1; }
                else { return 0; }
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
        public int InsertarNotificacion(int Tipo_Notificacion, string Id_usuario, DateTime Fecha, int Prioridad, string Asunto, string Mensaje, int Criterio, int Tipo, string Correo, int Estado, int Id_Codigo_origen, int Id_Notificacion, int Frecuencia,string EmpresaID, bool Reenvio)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                //var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Notificaciones_Ins @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7", Tipo_Notificacion, Id_usuario, Fecha, Prioridad, Asunto, Mensaje, Criterio, Id_Codigo_origen);
                var objetoNotif = new MT_Notificaciones {
                    Tipo_Notificacion = Tipo_Notificacion,
                    Id_Codigo_origen = Id_Codigo_origen,
                    Id_usuario = Id_usuario,
                    Fecha = Fecha,
                    Prioridad = Prioridad,
                    Asunto = Asunto,
                    Mensaje = Mensaje,
                    Criterio = Criterio,
                    Frecuencia = Frecuencia,
                    Reenvio = Reenvio,
                    EmpresaID = EmpresaID
                };

                db.MT_Notificaciones.Add(objetoNotif);
                db.SaveChanges();
                int Id_Noti = objetoNotif.Id_Notificacion;

                string[] Separado = Correo.Split(';');
                int Id_NotificacionPer = 0;
                int? IdPersona = 0;
                foreach (var item in Separado)
                {
                    var lista = ConsultaPersonas(item);

                    if (lista != null)
                    {                        
                        foreach (var itens in lista)
                        {
                            IdPersona = Convert.ToInt32(itens.id_persona);
                            break;
                        }

                        var obj = new MT_Notificacion_Persona
                        {
                            Id_Notificacion = Id_Noti,
                            Id_Persona = IdPersona,
                            Tipo = Tipo,
                            Correo = item,
                            Estado = Estado

                        };
                        db.MT_Notificacion_Persona.Add(obj);
                        db.SaveChanges();

                        //var objectContext = ((IObjectContextAdapter)ctx).ObjectContext;
                        //objectContext.AddObject("MT_Notificacion_Persona", obj);
                        //ctx.SaveChanges();
                        //}

                    }
                }
                return objetoNotif.Id_Notificacion;
            }
        }


        public List<sp_LINK_ConsultaUsuarios_Result> ConsultaPersonas(string CorreoPersona)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                CorreoPersona = CorreoPersona.Trim();
                if (CorreoPersona != null && CorreoPersona != "")
                {
                    var Personas = (ctx.Database.SqlQuery<sp_LINK_ConsultaUsuarios_Result>("sp_Quimipac_ConsultaPerCorreo @p0, @p1", CorreoPersona, empresa).Where(vq=>vq.user_status=="A").ToList());

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
                var ac = ctx.Database.SqlQuery<CriterioNoti>("sp_Quimipac_ConsultaCriterioNoti @p0,@p1", "Criterio Notificacion", Descripcion).ToList();

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
        public List<MT_Notificaciones> LkObtenerNotificacion(int IdNotificacion, string user)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                if (IdNotificacion > 0)
                {
                    var listas = (ctx.Database.SqlQuery<MT_Notificaciones>("sp_Quimipac_Consulta_Notificaciones_Entrada @p0,@p1,@p2", "Entrada_Noti", IdNotificacion, user).ToList());
                    return listas;
                }
                else
                {
                    var Listas = (ctx.Database.SqlQuery<MT_Notificaciones>("sp_Quimipac_Consulta_Notificaciones_Entrada @p0,@p1,@p2", "Entrada_Noti", 0, user).ToList());
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
        public List<ParametrosBusquedaOT> BusquedaXParametro(DateTime? fi, DateTime? ff, string criterio, int? Id_tipo_trabajo_ejecutado, int? Id_campamento, int? Id_estacion, int? Id_sector, string Id_sucursal, int? Id_contrato, int? Estado, string cod_cli, string Etapa, int Id_OrdenTrabajo)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            string empresa = empresa_id.ToString();
            var dbe = new DataBase_Externo();
            using (var ctx = new BD_QUIMIPACEntities())
            {
                string XParametro = string.Empty;
                int? XParametroInt = 0;

                if (criterio.Equals("Ninguno")) { XParametroInt = 0; }
                else if (criterio.Equals("Tipos_Trabajo")) { XParametroInt = Id_tipo_trabajo_ejecutado; }
                else if (criterio.Equals("Campamento")) { XParametroInt = Id_campamento; }
                else if (criterio.Equals("Estacion")) { XParametroInt = Id_estacion; }
                else if (criterio.Equals("Sector")) { XParametroInt = Id_sector; }
                else if (criterio.Equals("Sucursal")) { XParametro = Id_sucursal; }
                else if (criterio.Equals("Contrato")) { XParametroInt = Id_contrato; }
                else if (criterio.Equals("Estado")) { XParametroInt = Estado; }
                else if (criterio.Equals("Cliente")) { XParametro = cod_cli; }
                else if (criterio.Equals("Etapa")) { XParametro = Etapa;


                    var lksqlAux = (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_OrdenTrabajo_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, ff, criterio, XParametroInt, XParametro, empresa).ToList());
                    //if (lksqlAux != null)
                    //{
                    //    List<ParametrosBusquedaOT> listaPAC = new List<ParametrosBusquedaOT>();
                    //    foreach (var item in lksqlAux)
                    //    {
                    //        var listaNueva = dbe.IsValidParametroCANVAS_OT(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);

                    //        if (listaNueva.Equals(XParametro))
                    //        {
                    //            listaPAC.Add(item);
                    //        }
                    //    }
                    //    return listaPAC;
                    //}
                }
                else if (criterio.Equals("OrdenPadre")) { XParametroInt = Id_OrdenTrabajo; }

                var lksql = (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_OrdenTrabajo_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, ff, criterio, XParametroInt, XParametro, empresa).ToList());
                return lksql;
            }
        }

        /*MT_ORDEN TRABAJO PARAMETRO*/
        public List<ComboxOT> ParametroLkTipoTrabajo(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkCampamento(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkSucursal(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkEstacion(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkSector(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkContrato(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkProyecto(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkEstado(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkCliente(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<MT_OrdenTrabajo> ParametroLkOrdenPadre(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_OrdenTrabajo>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        //public List<MT_TipoTrabajo> ObtenerCriterioAccionOT(int IdTTEjecutado, string criterio) {
        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
        //        var lksql = ctx.MT_TipoTrabajo.Where(v => v.Id_TipoTrabajo == IdTTEjecutado);
        //        //(ctx.Database.SqlQuery<MT_Sector>("sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX @p0,@p1", criterio).ToList());

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
        //public int InsertarPrecioRef(int Id_TipoTablaDetalle, int Id_Proyecto_Contrato, string Id_Item, int Id_TipoTrabajo, string Id_Usuario,DateTime Fecha_Inicio, DateTime Fecha_Fin, string Moneda, decimal Precio, decimal Costo, string Estado)
        //{


        //    using (var ctx = new BD_QUIMIPACEntities())
        //    {
        //        //Precio = Convert.ToDecimal(Precio.ToString().Replace(".", ","));

        //        var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_PrecioReferencial_INS_UPD_X @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7,@p8,@p9,@p10,@p11", "INS",Id_TipoTablaDetalle, Id_Proyecto_Contrato, Id_Item, Id_TipoTrabajo, Id_Usuario, Fecha_Inicio, Fecha_Fin, Moneda, Precio, Costo,Estado);
        //        if (ac <= 0)
        //        {
        //            return 0;
        //        } else { return ac;}
        //    }
        //}

        public int InsertarPrecioRef(int Id_TipoTablaDetalle, int Id_Proyecto_Contrato, string Id_Item, int Id_TipoTrabajo, string Id_Usuario, DateTime Fecha_Inicio, DateTime Fecha_Fin, string Moneda, decimal Precio, decimal Costo, string Estado, string Unidad)
        {


            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_PrecioReferencial_INS_UPD_X @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7,@p8,@p9,@p10,@p11,@p12", "INS", Id_TipoTablaDetalle, Id_Proyecto_Contrato, Id_Item, Id_TipoTrabajo, Id_Usuario, Fecha_Inicio, Fecha_Fin, Moneda, Precio, Costo, Estado, Unidad);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }

        /*EntregaOrden*/

        //EntregaOrden Trabajo

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

        //public List<sp_Quimipac_Consulta_OrdenTrabajoEntrega_XClienteOT_Result> Get_MT_OTE(int Id_EOT, string Id_Cliente, string Criterio)
        //{
        //    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        //    using (var ctx1 = new BD_QUIMIPACEntities())
        //    {
        //        var lksql1 = (ctx1.Database.SqlQuery<sp_Quimipac_Consulta_OrdenTrabajoEntrega_XClienteOT_Result>("sp_Quimipac_Consulta_OrdenTrabajoEntrega_XClienteOT @p0, @p1,@p2, @p3", Id_EOT, Id_Cliente, Criterio, empresa_id.ToString()).ToList());
        //        return lksql1;
        //    }
        //}

        //public List<sp_Quimipac_Consulta_OrdenTrabajoGrupo_XClienteOT_Result> Get_MT_OrdenesGrupo(string Id_Cliente, string Criterio, int Id_Orden)
        //{
        //    var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
        //    using (var ctx1 = new BD_QUIMIPACEntities())
        //    {
        //        var lksql1 = (ctx1.Database.SqlQuery<sp_Quimipac_Consulta_OrdenTrabajoGrupo_XClienteOT_Result>("[sp_Quimipac_Consulta_OrdenTrabajoGrupo_XClienteOT] @p0, @p1,@p2,@p3", Id_Cliente, Criterio, empresa_id.ToString(), Id_Orden).ToList());
        //        return lksql1;
        //    }
        //}

        public int Editar_Entrega_OrdenTrabajo(int? Id_EntregaOrdenTrabajo, int? Id_OT, int? id_Cliente, DateTime? fechaEnvio, DateTime? fechaRecep, string comentario, string recibidoX, string Actualiza_OT)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Entrega_OrdenTrabajo_UPD @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", Id_EntregaOrdenTrabajo, Id_OT, id_Cliente, fechaEnvio, fechaRecep, comentario, recibidoX, Actualiza_OT, empresa_id.ToString());
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }


        public int Editar_Grupo_OrdenTrabajo(int? Id_ClienteGrupoOrden, int? Id_OT, int? id_Cliente, DateTime? fechaEnvio, DateTime? fechaRecep, string Actualiza_OT, int? Id_Grupo)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_Grupo_OrdenTrabajo_UPD @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", Id_ClienteGrupoOrden, Id_OT, id_Cliente, fechaEnvio, fechaRecep, Actualiza_OT, empresa_id.ToString(), Id_Grupo);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }


        //db.Roles.Where(v => v.Estado == "A").ToList();

        public List<sp_LINK_ConsultaStock_Result> ObtenerStock(string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var listaStock = (ctx.Database.SqlQuery<sp_LINK_ConsultaStock_Result>("sp_LINK_ConsultaStock @p0", empresa).ToList());
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

        //Programas de Trabajo
        //Contratos
        public List<sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos_Result> ObtenerContratoProgrTrab(string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos_Result>("sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos @p0", empresa).ToList());
                return lksql;
            }
        }

        public int GuardarProgramaTrabajo(int? Id_Contrato, string Id_Usuario, int? Id_TIpo_Trabajo, DateTime? Fecha_Registro, DateTime? Fecha_Ini, DateTime? Fecha_Fin, string Descripcion, string Direccion, string Ubicacion, string Estado, int? Id_GrupoTrabajo)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var n = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_ProgramaTrabajo_INS @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", Id_Contrato, Id_Usuario, Id_TIpo_Trabajo, Fecha_Registro, Fecha_Ini, Fecha_Fin, Descripcion, Direccion, Ubicacion, Estado, Id_GrupoTrabajo);
                if (n <= 0) { return 0; }
                else { return n; }

            }
        }

        //Parametros para OT desde GetDataMT_ProgTrabajo
        public int Generar_OT_ProgramaTrabajo(int? Id_ProgramaTrab, int? Id_Contrato, int? Id_TIpo_Trabajo, string Direccion, string user_id, int? Id_GrupoTrabajo, DateTime? Fecha_Inicio, DateTime? Fecha_Fin)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var n = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_ProgramaTrabajo_OT_INS @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", Id_ProgramaTrab, Id_Contrato, Id_TIpo_Trabajo, Direccion, user_id, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin);
                if (n <= 0) { return 0; }
                else { return n; }

            }
        }



        //UPD Fechas Inicio y fin para OT desde GetDataMT_ProgTrabajo
        public int UPD_FechaEvento_ProgTrab(int? IdProgTrabajo, DateTime? Fini, DateTime? Ffin)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var n = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_ProgramaTrabajo_FechIniFin_UPD @p0,@p1,@p2", IdProgTrabajo, Fini, Ffin);
                if (n <= 0) { return 0; }
                else { return n; }

            }
        }




        public int GetTipoTrabajoProyecto(int Id_Proyecto, int? Id_tipo_trabajo)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                var ac = ctx.Database.SqlQuery<MT_Proyecto>("sp_Quimipac_ValidarTipoTraboProyecto @p0, @p1,@p2", Id_Proyecto, Id_tipo_trabajo,empresa).ToList();

                if (ac.Count > 0) { return 1; }
                else { return 0; }
            }
        }

        //Actualizar precio referencial
        public int ActualizarPreRef(int IdPrecioReferencial, string usuario, DateTime fi, DateTime ff, decimal precio, decimal costo, string estado, string valueMoneda)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_MT_PrecioReferencial_Items_UPD @p0, @p1, @p2, @p3,@p4, @p5, @p6, @p7", IdPrecioReferencial, usuario, fi, ff, precio, costo, estado, valueMoneda);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }


        public List<InsertarSolicitud> InsertarSolicitudWeb(int dia, string codpro, int estado, DateTime? hora, string observacion, string usuario, DateTime? fecha_pro_comp, string observ_compras, string codcen, int? dias_prov, DateTime? fecha_disp, string observ_disp, string observ_pago, string usuario_compra,
        string cod_suc, string categoria, string elemento, string num_ped_cotiz, string usuario_aprobador, string cia_codigo)
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
        public Permisos_AccionUsuario Item_OpcPermiso()
        {
            try
            {
                string EmpresaID = System.Web.HttpContext.Current.Session["empresa"].ToString();
                string PersonaID = System.Web.HttpContext.Current.Session["usuario"].ToString();
                var PermisiItemLI = db.MT_Permiso.Where(vq => vq.Id_Usuario == PersonaID && vq.Id_Empresa == EmpresaID && vq.Estado == "A").Take(1).ToList();

                foreach (var item in PermisiItemLI)
                {
                    var obj = new Permisos_AccionUsuario{ 
                        Aprobar = item.Aprobar =="S" ? true:false,
                        Consultar = item.Consultar == "S" ? true : false,
                        Crear = item.Crear == "S" ? true : false,
                        Eliminar = item.Eliminar == "S" ? true : false,
                        Modificar = item.Modificar == "S" ? true : false
                    };
                    return obj;
                }
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Item_OpcPermiso:"+e.Message);
                return null;// Json(Response.StatusCode);
            }
        }
        //Editar Permisoa Usuarios
        public List<sp_Quimipac_ConsultaMT_Permisos_Usuario_Result> ObtenerPermisosXUsuario(string IdUsuario, string EmpresaID)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_Permisos_Usuario_Result>("sp_Quimipac_ConsultaMT_Permisos_Usuario @p0,@p1", IdUsuario, EmpresaID).ToList());

                return lksql;
            }
        }




        public List<sp_Quimipac_ConsultaUsuarios_Permisos_Result> ObtenerUsuariosPermisos()
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                string empresa = empresa_id.ToString();
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaUsuarios_Permisos_Result>("sp_Quimipac_ConsultaUsuarios_Permisos @p0,@p1", "", empresa).ToList());


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
                            Aprobar = item.Aprobar,
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

        public List<sp_LINK_ConsultaClientes_Result> ObtenerClientes(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_LINK_ConsultaClientes_Result>("sp_LINK_ConsultaClientes @p0", empresa).ToList());

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
        public int GuardarMTPermisos(string Id_Usuario, string Id_Cliente, int? Id_Contrato, int? Id_Proyecto, string Consultar, string Modificar, string Crear, string Eliminar, string Aprobar,string Usuario, DateTime Fecha, string Estado)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_GuardarMT_Permisos @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11", Id_Usuario, Id_Cliente, Id_Contrato, Id_Proyecto, Consultar, Modificar, Crear, Eliminar, Aprobar, Usuario, Fecha, Estado);
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
        public int ActualizarMTPermisos(int Id_Permiso, string Consultar, string Modificar, string Crear, string Eliminar, string Aprobar,string Usuario, string Estado)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Editar_MTPermisos @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", Id_Permiso, Consultar, Modificar, Crear, Eliminar, Aprobar, Usuario, Estado);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }

        //Texto Modal Notificaciones
        public List<MT_TablaDetalle> ObtenerDescripcionNoti(int IdDetalle)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<MT_TablaDetalle>("sp_Quimipac_Consulta_MTTablaDetalle_DescrInfo @p0", IdDetalle).ToList());
                return lksql;
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


        public int ActualizarOrdenTrabajo(int? Id_OrdenTrabajo, int? Estado, DateTime? FechaI, DateTime? FechaF, string Comentario)
        {


            using (var ctx = new BD_QUIMIPACEntities())
            {


                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_ActEstadoOrdenTrabajo @p0, @p1, @p2, @p3,@p4", Id_OrdenTrabajo, Estado, FechaI, FechaF,Comentario);
                if (ac <= 0)
                {
                    return 0;
                }
                else { return ac; }
            }
        }


        /// debo agregar la tabla de contrto parametro que no esta aqui en esta funcion 


        public int Insertar_ContratoParametro(int contrato)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                string sql = "INSERT INTO MT_Contrato_Parametro(Id_contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final)"
                          + " VALUES(" + contrato + ",'En Proceso', 'En Proceso', 'varchar', 1, 'A', 3)";
                //sql = sql.Replace("''", "null");
                var ac = (ctx.Database.ExecuteSqlCommand(sql));
                return ac;
            }
        }

        public List<sp_Quimipac_ConsultaMT_Items_Result> Obtener_Id_Ingresados(int idTipoTrabajo, int IdProyContrato)//, string Unidad)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {

                //sp_Quimipac_ConsultaMT_Items
                var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
                var listaItemsTT = ctx.sp_Quimipac_ConsultaMT_Items(idTipoTrabajo, empresa_id.ToString()).ToList();
                //  var lksql = (ctx.Database.SqlQuery<MT_Permiso>("sp_", criterio).ToList());
                //List<sp_Quimipac_ConsultaMT_Items_Result>
                var lksqlPre = ctx.MT_PrecioReferencial.Where(vq => vq.Id_TipoTrabajo == idTipoTrabajo && vq.Id_Proyecto_Contrato == IdProyContrato && vq.Estado == "A").ToList();// && vq.Unidad == Unidad).ToList();


                List<sp_Quimipac_ConsultaMT_Items_Result> lkNoRepetidos = new List<sp_Quimipac_ConsultaMT_Items_Result>();

                //var lksql =
                //Select * From MT_PrecioReferencial Where Id_Proyecto_Contrato = Id_Proyecto_Contrato
                foreach (var item in listaItemsTT)
                {



                    var bandPrecio = lksqlPre.Find(vq => vq.Id_Item == item.Id_Item && vq.Unidad == item.Unidad) == null ? false : true;

                    if (bandPrecio == false)
                    {
                        var obj = new sp_Quimipac_ConsultaMT_Items_Result
                        {
                            Id_ItemTipoTrabajo = item.Id_ItemTipoTrabajo,//.Id_ItemTipoTrabajo,
                            Id_TipoTrabajo = item.Id_TipoTrabajo,
                            Id_Item = item.Id_Item,
                            Unidad = item.Unidad,
                            Estado = item.Estado,
                            DescripcionItem = item.DescripcionItem,
                            NombreUnidad = item.NombreUnidad,
                            DescripcionTTrabajo = item.DescripcionTTrabajo
                        };
                        lkNoRepetidos.Add(obj);
                    }


                    // foreach (var itemPre in lksqlPre)
                    // {
                    //sp_Quimipac_ConsultaMT_Items_Result objlk;
                    //objlk= lkNoRepetidos.Find(vq => (vq.Id_ItemTipoTrabajo == item.Id_ItemTipoTrabajo));

                    //var objlk = lkNoRepetidos.Find(vq => )== null ? true : false;

                    //...var objlk = lkNoRepetidos.Find(vq => vq.Id_Item == itemPre.Id_Item && vq.Unidad == itemPre.Unidad) == null ? false: true;

                    //var objlk = lkNoRepetidos.Any(vq => vq.Id_Item == item.Id_Item && vq.Unidad == item.Unidad);



                    //if (!(itemPre.Id_Item.Equals(item.Id_Item)) && itemPre.Unidad != item.Unidad)
                    /*    if (itemPre.Id_Item.Equals(item.Id_Item) && itemPre.Unidad == item.Unidad)
                        {

                        }
                        else {  var obj = new sp_Quimipac_ConsultaMT_Items_Result { 
                                Id_ItemTipoTrabajo = item.Id_ItemTipoTrabajo,//.Id_ItemTipoTrabajo,
                                Id_TipoTrabajo = item.Id_TipoTrabajo,
                                Id_Item = item.Id_Item,
                                Unidad = item.Unidad,
                                Estado = item.Estado,
                                DescripcionItem = item.DescripcionItem,
                                NombreUnidad = item.NombreUnidad,
                                DescripcionTTrabajo = item.DescripcionTTrabajo
                         };
                            lkNoRepetidos.Add(obj);
                        //    break;
                        }
                        if (objlk ==false)
                        {
                            var obj = new sp_Quimipac_ConsultaMT_Items_Result
                            {
                                Id_ItemTipoTrabajo = item.Id_ItemTipoTrabajo,//.Id_ItemTipoTrabajo,
                                Id_TipoTrabajo = item.Id_TipoTrabajo,
                                Id_Item = item.Id_Item,
                                Unidad = item.Unidad,
                                Estado = item.Estado,
                                DescripcionItem = item.DescripcionItem,
                                NombreUnidad = item.NombreUnidad,
                                DescripcionTTrabajo = item.DescripcionTTrabajo
                            };
                            lkNoRepetidos.Add(obj);
                        }*/

                    //}
                    //}

                }
                return lkNoRepetidos;

            }
        }


        public List<Insertar_ContratoParametro> InsertarParametroContratoEditNuevo(int Id_Contrato_Parametro, int? Id_contrato, string Parametro, string Descripcion, string Tipo_Dato,
      int? Valor_Inicial, string Estado, int? Valor_Final)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<Insertar_ContratoParametro>("sp_Quimipac_InsertarParametroContratoNuevo @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                    Id_Contrato_Parametro, Id_contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final).ToList();

                return null;


            }
        }

        //public List<OrdenTrabajo_Excel> IngresarOrdenes_Excel(string Identificador, string Numerador, string Estado, string Producto, string Unidad_de_Trabajo, string Estado_Unidad_de_Trabajo, DateTime? Fecha_de_Creacion, DateTime? Fecha_de_Asignacion, string Tipo_de_Asignacion, DateTime? Fecha_Estimada_de_Ejecucion, DateTime? Fecha_Maxima_para_Legalizacion, DateTime? Ultima_Reprogramacion, DateTime? Fecha_de_Legalizacion, DateTime? Inicio_de_Ejecucion, DateTime? Fin_de_Ejecucion, string Causal_de_Legalizacion, string Personal, string Valor_de_la_Orden, string Número_de_Veces_Impresa, string Intentos_de_Legalizacion, string Anula_Otra_Orden, string Tipo_de_Compromiso, string Cita_Confirmada, string Consecutivo_Ruta, string Ruta, string Nombre_Ruta, string Dirección, string Ubicación_Geográfica, string Cliente, string Nombre_Suscriptor, string Apellido_Suscriptor, string Teléfono_Cliente, string Puntaje_Cliente, string Esfuerzo_de_la_Orden, string Comentario, string Tipo_de_Comentario, string Unidad_Asociada, string Ofertada, string Proyecto, string Etapa, string Estado_de_la_Orden, string PARENT_ID, string tipo_trabajo, string sector, string tipo_trabajo_planeado, string nivel_prioridad, string barrio, string tipo_cliente, TimeSpan? Hora_Acordada, string usuario, int contratoexcel, string empresa)
        public int IngresarOrdenes_Excel(string Identificador, string Numerador, string Estado, string Producto, string Unidad_de_Trabajo, string Estado_Unidad_de_Trabajo, DateTime? Fecha_de_Creacion, DateTime? Fecha_de_Asignacion, string Tipo_de_Asignacion, DateTime? Fecha_Estimada_de_Ejecucion, DateTime? Fecha_Maxima_para_Legalizacion, DateTime? Ultima_Reprogramacion, DateTime? Fecha_de_Legalizacion, DateTime? Inicio_de_Ejecucion, DateTime? Fin_de_Ejecucion, string Causal_de_Legalizacion, string Personal, string Valor_de_la_Orden, string Número_de_Veces_Impresa, string Intentos_de_Legalizacion, string Anula_Otra_Orden, string Tipo_de_Compromiso, string Cita_Confirmada, string Consecutivo_Ruta, string Ruta, string Nombre_Ruta, string Dirección, string Ubicación_Geográfica, string Cliente, string Nombre_Suscriptor, string Apellido_Suscriptor, string Teléfono_Cliente, string Puntaje_Cliente, string Esfuerzo_de_la_Orden, string Comentario, string Tipo_de_Comentario, string Unidad_Asociada, string Ofertada, string Proyecto, string Etapa, string Estado_de_la_Orden, string PARENT_ID, string tipo_trabajo, string sector, string tipo_trabajo_planeado, string nivel_prioridad, string barrio, string tipo_cliente, TimeSpan? Hora_Acordada, string usuario, int contratoexcel, string empresa, DateTime? fecha_maxima_contratista, DateTime? fecha_finalizacion_obra, bool? automatizacion, string variablecodigo, string variabletpcodigo, string Origen)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Insertar_OrdenTrabajoExcel @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47, @p48, @p49, @p50, @p51, @p52, @p53, @p54, @p55, @p56, @p57",
                    Identificador, Numerador, Estado, Producto, Unidad_de_Trabajo, Estado_Unidad_de_Trabajo, Fecha_de_Creacion, Fecha_de_Asignacion, Tipo_de_Asignacion, Fecha_Estimada_de_Ejecucion, Fecha_Maxima_para_Legalizacion, Ultima_Reprogramacion, Fecha_de_Legalizacion, Inicio_de_Ejecucion, Fin_de_Ejecucion, Causal_de_Legalizacion, Personal, Valor_de_la_Orden, Número_de_Veces_Impresa, Intentos_de_Legalizacion, Anula_Otra_Orden, Tipo_de_Compromiso, Cita_Confirmada, Consecutivo_Ruta, Ruta, Nombre_Ruta, Dirección, Ubicación_Geográfica, Cliente, Nombre_Suscriptor, Apellido_Suscriptor, Teléfono_Cliente, Puntaje_Cliente, Esfuerzo_de_la_Orden, Comentario, Tipo_de_Comentario, Unidad_Asociada, Ofertada, Proyecto, Etapa, Estado_de_la_Orden, PARENT_ID, tipo_trabajo, sector, tipo_trabajo_planeado, nivel_prioridad, barrio, tipo_cliente, Hora_Acordada, usuario, contratoexcel, empresa, fecha_maxima_contratista, fecha_finalizacion_obra, automatizacion, variablecodigo, variabletpcodigo, Origen);

                if (ac != -1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }


                


            }
        }

        public List<Insertar_ContratoProrroga> InsertarFecha_Prorroga(int? Id_contrato, string Estado, int? dia, string descripcion)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var ac = ctx.Database.SqlQuery<Insertar_ContratoProrroga>("sp_Quimipac_InsertarFecha_Prorroga @p0, @p1, @p2, @p3", Id_contrato, Estado, dia, descripcion).ToList();               

                return null;


            }
        }

        public List<Insertar_Contratosysbase> InsertarContrato(int Id_Contrato, string cia_codigo, string cod_cliente, DateTime? fecha_inicial, DateTime? fecha_fin, string codigo_secuencial_interno, string codigo_contrato_asociado, string user_id, string unidad, string cod_servicio, string codcen, string detalle, int? id_estado,
            int? plazo_contrato, int? cod_tipo, int? Contrato_Padre, decimal? Valor_Referencial,  decimal? monto, decimal? costo, int? Responsable, int? secuencial, string Codigo_Interno_Ant, string observaciones, string codigo_secuencial_interno_padre,  DateTime? Fecha_registro, DateTime? Fecha_modificacion, string Localidad, DateTime? Fecha_Aprobacion_Cot, string Recepcion_Servicio, DateTime? Fecha_Firma_Conformidad, DateTime? Fecha_Cumplimiento_Inst)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {
                    //var ac = ctx.Database.SqlQuery<Insertar_Contratosysbase>("sp_Quimipac_InsertarContratoSysbase @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19,@p20,@p21,@p22,@p23,@p24,@p25", Id_Contrato, cia_codigo, cod_cliente, fecha_inicial, fecha_fin, codigo_secuencial_interno, codigo_contrato_asociado, user_id, unidad, cod_servicio, codcen, detalle, id_estado, plazo_contrato, cod_tipo, Contrato_Padre, Valor_Referencial, monto, costo, Responsable, secuencial, Codigo_Interno_Ant, observaciones, codigo_secuencial_interno_padre, Fecha_registro, Fecha_modificacion).ToList();
                    var ac = ctx.Database.SqlQuery<Insertar_Contratosysbase>("sp_Quimipac_InsertarContratoSysbase @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29,@p30", Id_Contrato, cia_codigo, cod_cliente, fecha_inicial, fecha_fin, codigo_secuencial_interno, codigo_contrato_asociado, user_id, unidad, cod_servicio, codcen, detalle, id_estado, plazo_contrato, cod_tipo, Contrato_Padre, Valor_Referencial, monto, costo, Responsable, secuencial, Codigo_Interno_Ant, observaciones, codigo_secuencial_interno_padre, Fecha_registro, Fecha_modificacion,Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst).ToList();
                    //25,'02','1', '28/06/2019 0:00:00', '01/07/2019 0:00:00', 'HYDIWSTYAPRO0022019', 'Req # 19001269-01', 'BSALTO',
                    //'TYA','02','080', 'Elementos para Estaciones de Bombeo: Fuente conmutable (12) y supresores de voltaje para cable coaxial (20)',75,3,146,null,0.0000,8160.0000,6456.0000,
                    //null,2,'HYDIWSTYAPRO0022019','REQUISICION SOLICITADA POR LA ING. JANETA','HYDIWSTYAPRO0022019','19/03/2020 0:00:00',null




                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //throw new System.Exception(ex.Message);
                }      
                return null;
            }
        }


        //Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones

        public List<MT_Contrato> UpdateContrato(int Id_Contrato, string Id_Cliente, DateTime? Fecha_Inicio, DateTime? Fecha_Fin, string Codigo_Interno, string Codigo_Cliente, /*string Id_Linea, string Categoria, string Subcategoria,*/ string Nombre, int? Estado, int? Dia_Plazo, int? tipo, int? Contrato_Padre, decimal? Valor_Referencial, decimal? monto, decimal? costo, int? Responsable, int? Secuencial, string Codigo_Interno_Ant, string Observaciones, string Codigo_interno_padre, DateTime? Fecha_registro, DateTime? Fecha_modificacion, string Localidad, DateTime? Fecha_Aprobacion_Cot, string Recepcion_Servicio, DateTime? Fecha_Firma_Conformidad, DateTime? Fecha_Cumplimiento_Inst, int? Referencia)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {
                    var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_UpdateContrato @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26", Id_Contrato, Id_Cliente, Fecha_Inicio, Fecha_Fin, Codigo_Interno, Codigo_Cliente, /*Id_Linea, Categoria, Subcategoria,*/ Nombre, Estado, Dia_Plazo, tipo, Contrato_Padre, Valor_Referencial, monto, costo, Responsable, Secuencial, Codigo_Interno_Ant, Observaciones, Codigo_interno_padre, Fecha_registro, Fecha_modificacion, Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst, Referencia); /*, @p27,@p28,@p29*/

                    return null;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw new System.Exception(ex.Message);
                }
            }
        }

        
        public List<sp_Quimipac_ConsultaMT_ContratoGeneral_Result> Obtener_ContratoGeneral(string EmpresaID)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ContratoGeneral_Result>("sp_Quimipac_ConsultaMT_ContratoGeneral_Prueba @p0", EmpresaID).ToList());

                return lksql;
            }
        }

        public int InsertarEntregaOrdenGrupo(List<TempEntregaOrden> lkOrden, string Id_Grupo, DateTime Fecha_Inicio, DateTime Fecha_Fin)
        {
            int cod = 0;

            


            using (var ctx = new BD_QUIMIPACEntities())
            {
                ctx.Database.Connection.Open();
                var con = (SqlConnection)ctx.Database.Connection;
                SqlCommand cmd = new SqlCommand("sp_Quimipac_OrdenEntregaOrden", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Criterio", "INS");
                cmd.Parameters.AddWithValue("@orden", 0);                
                cmd.Parameters.AddWithValue("@Id_Cliente", Id_Grupo);
                //cmd.Parameters.AddWithValue("@Fecha_Elaboracion", Fecha_Elaboracion);
                cmd.Parameters.AddWithValue("@Fecha_Envio", Fecha_Inicio);
                cmd.Parameters.AddWithValue("@Fecha_Recepcion", Fecha_Fin);               
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
                            cmd2.Parameters.AddWithValue("@Id_Cliente", Id_Grupo);
                            //cmd2.Parameters.AddWithValue("@Fecha_Elaboracion", Fecha_Elaboracion);
                            cmd2.Parameters.AddWithValue("@Fecha_Envio", Fecha_Inicio);
                            cmd2.Parameters.AddWithValue("@Fecha_Recepcion", Fecha_Fin);                            
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

        //aqui cambie la clase de MT_OrdenTrabajo_Integrante por InsertarEquipoOrden
        public List<InsertarEquipoOrden> InsertarIntegranteOrdenTrabajo(int id_Orden, int ValueGrupo, DateTime? ValueFechaInicio, DateTime? ValueFechaFin)
        {

            using (var ctx = new BD_QUIMIPACEntities())
            {

                //for (int i = 0; i < count2; i++)
                //{
                    //var ac = ctx.Database.SqlQuery<MT_OrdenTrabajo_Integrante>("sp_Quimipac_InsertarIntegranteOrdenTrabajo @p0,", id_Orden).ToList();
                    var  ac = (ctx.Database.SqlQuery<InsertarEquipoOrden>("sp_Quimipac_InsertarIntegranteOrdenTrabajo @p0, @p1, @p2, @p3", id_Orden, ValueGrupo, ValueFechaInicio, ValueFechaFin).ToList());

                //}
                return null;


            }
        }


        public int IngresarPlanillaCab_Excel(string Identificador, string Unidad_de_Trabajo,  DateTime? Fecha_de_Asignacion,  DateTime? Fecha_de_Legalizacion, DateTime? Inicio_de_Ejecucion, DateTime? Fin_de_Ejecucion, string Ubicacion_Geográfica, string tipo_trabajo_planeado,  string usuario, int contratoexcel, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {

                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Insertar_PlanillaCabExcel @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10",
                    Identificador,  Unidad_de_Trabajo, Fecha_de_Asignacion, Fecha_de_Legalizacion, Inicio_de_Ejecucion, Fin_de_Ejecucion, Ubicacion_Geográfica, tipo_trabajo_planeado, usuario, contratoexcel, empresa);

                if (ac != -1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }





            }
        }

        #region MANT_MenuDRol

        public List<Menu_Aux> getItem_Menu(int RolID)
        {
            //var MenuPadre0_LI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
            //var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, 0, EmpresaID).ToList();

            var MenuFull_Tot = new List<Menu_Aux>();
            var MenuPadre0_LI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
            var opcPermitidas = db.MenuRol.Where(vq => vq.Estado == "A" && vq.Id_Rol == RolID).ToList();

            var MenuUsuario = new List<Menu_Aux>();

            foreach (var item in MenuPadre0_LI)
            {
                if (item.Action == null)//"")
                {
                    var o0 = new Menu_Aux();
                    o0.Id_Menu = item.Id_Menu;
                    o0.Descripcion_Menu = item.Menu1;
                    o0.Icono = item.Icono;
                    o0.IsPadre = true;
                    o0.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true;
                    o0.Nivel_Profundidad = 0;
                    o0.Orden = item.Orden;
                    o0.Padre = item.Padre;
                    o0.Estado = item.Estado;

                    MenuFull_Tot.Add(o0);

                    var opciones_nivel2 = db.Menu.Where(vq => vq.Padre == item.Id_Menu && vq.Estado == "A").OrderBy(vq => vq.Orden).ToList();
                    if (opciones_nivel2.Count() != 0)
                    {
                        foreach (var item2 in opciones_nivel2)
                        {
                            var opciones_nivel3 = db.Menu.Where(vq => vq.Padre == item2.Id_Menu && vq.Estado == "A").ToList();
                            if (opciones_nivel3.Count() != 0)
                            {
                                foreach (var item3 in opciones_nivel3)
                                {
                                }
                            }
                            else
                            {
                                var o2 = new Menu_Aux();
                                o2.Id_Menu = item2.Id_Menu;
                                o2.Descripcion_Menu = item2.Menu1;
                                o2.Icono = item2.Icono;
                                o2.IsPadre = false;
                                o2.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item2.Id_Menu) != true ? false : true;
                                o2.Nivel_Profundidad = 2;
                                o2.Orden = item2.Orden;
                                o2.Padre = item2.Padre;
                                o2.Estado = item2.Estado;

                                MenuFull_Tot.Add(o2);
                            }
                        }
                    }
                }
                else
                {
                    var o0 = new Menu_Aux();
                    o0.Id_Menu = item.Id_Menu;
                    o0.Descripcion_Menu = item.Menu1;
                    o0.Icono = item.Icono;
                    o0.IsPadre = false;
                    o0.IsSelected = opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true;
                    o0.Nivel_Profundidad = 0;
                    o0.Orden = item.Orden;
                    o0.Padre = item.Padre;
                    o0.Estado = item.Estado;

                    MenuFull_Tot.Add(o0);
                }
            }
            //getItem_Menu(MenuFull_Tot,item);

            var menuiten = MenuFull_Tot.OrderBy(vq => vq.Orden);
            return MenuFull_Tot;
        }


        public List<Menu_Aux> getItem_Menu00(int RolID)
        {
            var MenuLI = db.Menu.Where(vq => vq.Estado == "A" && vq.Padre == 0).OrderBy(vq => vq.Orden).ToList();
            //var opciones_padres = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, 0, EmpresaID).ToList();
            var opcPermitidas = db.MenuRol.Where(vq => vq.Estado == "A" && vq.Id_Rol == RolID).ToList();

            var MenuUsuario = new List<Menu_Aux>();
            foreach (var item in MenuLI)
            {
                bool IsPAdre = item.Action == "" ? true : false;
                bool IsSelected = false;
                if (item.Action == "")
                {
                    var objMenu0 = new Menu_Aux
                    {
                        Id_Menu = item.Id_Menu,
                        Descripcion_Menu = item.Menu1,
                        Icono = item.Icono,
                        IsPadre = true,
                        IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true),
                        Nivel_Profundidad = 0,
                        Orden = item.Orden,
                        Padre = item.Padre,
                        Estado = item.Estado
                    };
                    MenuUsuario.Add(objMenu0);
                    //var opciones_nivel2 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item.Id_Menu, EmpresaID).ToList();
                    var opciones_nivel2 = db.Menu.Where(vq => vq.Padre == item.Id_Menu && vq.Estado == "A").ToList();
                    if (opciones_nivel2.Count() != 0)
                    {
                        foreach (var item2 in opciones_nivel2)
                        {
                            //var opciones_nivel3 = db.sp_Quimipac_ConsultaOpcionesSistema(usuarioID, item2.Id_Menu, EmpresaID).ToList();
                            var opciones_nivel3 = db.Menu.Where(vq => vq.Padre == item2.Id_Menu && vq.Estado == "A").ToList();
                            if (opciones_nivel3.Count() != 0)
                            {
                                foreach (var item3 in opciones_nivel3)
                                {
                                    //al mommento no ay 3 niveles
                                }
                            }
                            else
                            {
                                var objMenu2 = new Menu_Aux
                                {
                                    Id_Menu = item2.Id_Menu,
                                    Descripcion_Menu = item2.Menu1,
                                    Icono = item2.Icono,
                                    IsPadre = false,
                                    IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item2.Id_Menu) != true ? false : true),
                                    Nivel_Profundidad = 1,
                                    Orden = item2.Orden,
                                    Padre = item2.Padre,
                                    Estado = item2.Estado
                                };
                                MenuUsuario.Add(objMenu2);
                                //< a href = "@Url.Action(item2.Action, item2.controlador)" id = "@item2.Action" >< i class="@item2.Icono"></i> <span>@item2.Menu</span></a>
                            }
                        }
                    }
                }
                else
                {
                    var objMenu1 = new Menu_Aux
                    {
                        Id_Menu = item.Id_Menu,
                        Descripcion_Menu = item.Menu1,
                        Icono = item.Icono,
                        IsPadre = false,
                        IsSelected = (opcPermitidas.Exists(vq => vq.Id_Menu == item.Id_Menu) != true ? false : true),
                        Nivel_Profundidad = 0,
                        Orden = item.Orden,
                        Padre = item.Padre,
                        Estado = item.Estado
                    };
                    MenuUsuario.Add(objMenu1);
                    //< a href = "@Url.Action(item.Action, item.controlador)" id = "@item.Action" > < i class="@item.Icono"></i> <span id = "color" > @item.Menu </ span ></ a >
                }
            }

            var menuiten = MenuUsuario.OrderBy(vq => vq.Orden);
            return MenuUsuario;
        }
        public List<Roles> getItemActivosMenuRol(int RolID)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var pData = ctx.Database.SqlQuery<Roles>(" SELECT R.* FROM MENUROL M"
                                                        + " INNER JOIN ROLES R ON M.ID_ROL = R.ID_ROL"
                                                        + " WHERE R.ESTADO = 'A' AND M.ESTADO = 'A'");
                /*from rm in db.MenuRol.Where(vq => vq.Estado == "A")
                        join r in db.Roles.Where(vq => vq.Estado == "A")
                        on rm.Id_Rol equals r.Id_Rol
                        select new Roles { Estado = r.Estado,Id_Rol = r.Id_Rol, Descripcion =r.Descripcion };*/

                return pData.ToList();
            }
        }
        #endregion

        public int ObtenerCriterioUltimoCtr(int? IdContrato)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                int id_contrato_prorroga =0;
                var mtUltimo_Prorroga = ctx.MT_Contrato_Prorroga.Where(v => v.Id_Contrato == IdContrato && v.Estado == "A").OrderByDescending(vq=>vq.Fecha_Prorroga).Take(1).ToList();
                if (mtUltimo_Prorroga.Count > 0)
                {
                     id_contrato_prorroga = mtUltimo_Prorroga.ElementAt(0).Id_Contrato_Prorroga;
                }

                return id_contrato_prorroga;

            }
        }

        //Identificador, Unidad_de_Trabajo, fecha_asignacion_, fecha_leg_, inicio_ej_, fin_ej_,  Ubicacion_Geográfica,  variable,  usuario, contrato_proyecto, empresa_id.ToString(), Tipo
        public int IngresarPlanilla_Cab(string Identificador, string Unidad_de_Trabajo,  DateTime? Fecha_de_Asignacion, DateTime? Fecha_de_Legalizacion, DateTime? Inicio_de_Ejecucion, DateTime? Fin_de_Ejecucion, string Ubicación_Geográfica,  string tipo_trabajo, string usuario, int? contratoproyexcel, string empresa, int? tipo)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {

                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Insertar_PlanillaCabExcel @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11",
                    Identificador, Unidad_de_Trabajo, Fecha_de_Asignacion, Fecha_de_Legalizacion, Inicio_de_Ejecucion, Fin_de_Ejecucion,  Ubicación_Geográfica, tipo_trabajo, usuario, contratoproyexcel, empresa, tipo);

                if (ac != -1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }





            }
        }


        public int IngresarPlanilla_Detalle(string Código_Acta, string Base_Administrativa, string Descripción_de_Acta, string Código_Periodo, string Descripción_de_Periodo, string Código_Tipo_de_Acta, string Descripción_de_Tipo_de_Acta, DateTime? fecha_Inicial_Periodo_, DateTime? fecha_Final_Periodo_, DateTime? fecha_Cierre_Acta_, string Estado_Acta, string Descripción_de_Estado_Acta, string Orden_Raiz, string No_Orden_Padre, string No_Orden, string Código_Detalle_Acta, string Código_Items, string Descripción_de_Items, string Cantidad, string Código_Unidad, string Unidad, string Valor_Unitario, string Valor_Total, string Código_Listado_de_Costo, string Descripción_Listado_de_Costo, string Tipo_Generación, string Código_de_Plan_de_Condición, string Tipo_de_Trabajo, string Descripción_de_Tipo_de_Trabajo, string Actividad, DateTime? fecha_Asignación_, DateTime? fecha_Fin_Permiso_Municipal_Calc_, DateTime? fecha_Asignación_OT_, DateTime? fecha_Ejecución_, DateTime? fecha_Legalización_, DateTime? fecha_Ejecucion_OT_Padre_, string Tiempo_Promedio_Inc_Desc_OT, string Tiempo_Promedio_Desc_Descarga, string Tiempo_Transcurrido_HORAS, string Tiempo_Optimo_Incentivo, string Tiempo_Máximo_Incentivo, string Porcentaje_Incentivo, string Tiempo_Optimo_Desc_Atención, string Tiempo_Máximo_Desc_Atención, string Porcentaje_de_Desc_Atención, string Tiempo_Máximo_Desc_Descarga, string Tiempo_Excede_Desc_Descarga, string Porcentaje_Desc_Descarga, string Valor_Aplica_Desc_Atención_OT, string Código_Contrato, string Descripción_Contrato, string Tipo_de_Contrato, string Descripción_de_Tipo_de_Contrato, string Contratista, string Descripción_Contratista, string Porcentaje_Costo_Indirecto_Contrato, string Porcentaje_Fondo_Garantia_Contrato, string Porcentaje_Amortización_Contrato, string Descuento_General, string Código_Unidad_Operativa, string Descripción_Unidad_Operativa, string Terminal, string Valor_Base, string Cumplimiento, string Tiempo_Contractual_HORAS, string Tiempo_de_Permiso_Municipal_HORAS, string Tipo_Clasifica, string Inventario, string Tiene_Caso_Especial, string Tipoe_de_Relación, string Nro_Contrato, string Nro_Producto, string Código, string Tipo_de_Irregularidad, string Descripción_de_Proyecto, string Usuario_Finalizo, string Código_Tipo_Comentario, string Comentario, string Observación_Orden_Actividad, DateTime? fecha_Pago_Sistema_Externo_, string Número_Factura_Sistema_Externo, string Cod_estado_orden, string usuario, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {

                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Insertar_PlanillaDetExcel @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11,@p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23,@p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35,@p36, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47,@p48, @p49, @p50, @p51, @p52, @p53, @p54, @p55, @p56, @p57, @p58, @p59,@p60, @p61, @p62, @p63, @p64, @p65, @p66, @p67, @p68, @p69, @p70, @p71,@p72, @p73, @p74, @p75, @p76, @p77, @p78, @p79, @p80, @p81, @p82, @p83",
                    Código_Acta, Base_Administrativa, Descripción_de_Acta, Código_Periodo, Descripción_de_Periodo, Código_Tipo_de_Acta, Descripción_de_Tipo_de_Acta, fecha_Inicial_Periodo_,  fecha_Final_Periodo_, fecha_Cierre_Acta_, Estado_Acta, Descripción_de_Estado_Acta, Orden_Raiz, No_Orden_Padre, No_Orden, Código_Detalle_Acta, Código_Items, Descripción_de_Items, Cantidad, Código_Unidad, Unidad, Valor_Unitario,  Valor_Total, Código_Listado_de_Costo, Descripción_Listado_de_Costo, Tipo_Generación, Código_de_Plan_de_Condición, Tipo_de_Trabajo, Descripción_de_Tipo_de_Trabajo, Actividad, fecha_Asignación_,   fecha_Fin_Permiso_Municipal_Calc_, fecha_Asignación_OT_, fecha_Ejecución_, fecha_Legalización_, fecha_Ejecucion_OT_Padre_, Tiempo_Promedio_Inc_Desc_OT, Tiempo_Promedio_Desc_Descarga, Tiempo_Transcurrido_HORAS, Tiempo_Optimo_Incentivo, Tiempo_Máximo_Incentivo, Porcentaje_Incentivo, Tiempo_Optimo_Desc_Atención, Tiempo_Máximo_Desc_Atención, Porcentaje_de_Desc_Atención, Tiempo_Máximo_Desc_Descarga, Tiempo_Excede_Desc_Descarga, Porcentaje_Desc_Descarga, Valor_Aplica_Desc_Atención_OT, Código_Contrato, Descripción_Contrato, Tipo_de_Contrato, Descripción_de_Tipo_de_Contrato, Contratista, Descripción_Contratista, Porcentaje_Costo_Indirecto_Contrato, Porcentaje_Fondo_Garantia_Contrato, Porcentaje_Amortización_Contrato, Descuento_General, Código_Unidad_Operativa, Descripción_Unidad_Operativa, Terminal, Valor_Base, Cumplimiento, Tiempo_Contractual_HORAS, Tiempo_de_Permiso_Municipal_HORAS, Tipo_Clasifica, Inventario, Tiene_Caso_Especial, Tipoe_de_Relación, Nro_Contrato, Nro_Producto, Código, Tipo_de_Irregularidad, Descripción_de_Proyecto, Usuario_Finalizo, Código_Tipo_Comentario, Comentario, Observación_Orden_Actividad, fecha_Pago_Sistema_Externo_, Número_Factura_Sistema_Externo, Cod_estado_orden, usuario, empresa);

                if (ac != -1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }





            }
        }

        /*MT_CONTRATO PARAMETROS*/
        public List<ComboxOT> ParametroLkTipo(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkClienteC(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkContratoPadre(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkEstadoC(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_Contrato_TipoEstado @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkLinea(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkCategoria(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkSubcategoria(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<ComboxOT> ParametroLkReferencia(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<ComboxOT>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkLocalidad(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        public List<JsonUtil> ParametroLkResponsable(string criterio, string empresa)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                var lksql = (ctx.Database.SqlQuery<JsonUtil>("sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro @p0,@p1", criterio, empresa).ToList());
                return lksql;
            }
        }

        //CONTRATO.Fecha_Inicio, CONTRATO.Fecha_registro, CONTRATO.Fecha_Fin, CONTRATO.Origen, CONTRATO.tipo, CONTRATO.Id_Cliente, CONTRATO.Contrato_Padre, CONTRATO.Estado, CONTRATO.Id_Linea, CONTRATO.Categoria, CONTRATO.Subcategoria, CONTRATO.Referencia, CONTRATO.Localidad, CONTRATO.Responsable)
        public List<sp_Quimipac_ConsultaMT_ContratoGeneral_Result> BusquedaXParametro_Contrato(string fi, string fregistro, DateTime? ff,  string criterio, int? tipo, string Id_Cliente, int? Contrato_Padre, int? Estado, string Id_Linea, string Categoria, string Subcategoria, int? Referencia, string Localidad, int? Responsable)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            string empresa = empresa_id.ToString();
            if (true)
            {

            }
            var dbe = new DataBase_Externo();
            using (var ctx = new BD_QUIMIPACEntities())
            {
                string XParametro = string.Empty;
                int? XParametroInt = 0;

                if (criterio.Equals("Ninguno")) { XParametroInt = 0; }
                else if (criterio.Equals("Tipo")) { XParametroInt = tipo; }
                else if (criterio.Equals("Cliente")) { XParametro = Id_Cliente; }
                else if (criterio.Equals("Contrato_Padre")) { XParametroInt = Contrato_Padre; }
                else if (criterio.Equals("Estado")) { XParametroInt = Estado; }
                else if (criterio.Equals("Unidad_Negocio")) { XParametro = Id_Linea; }
                else if (criterio.Equals("Categoria")) { XParametro = Categoria; }
                else if (criterio.Equals("Subcategoria")) { XParametro = Subcategoria; }
                else if (criterio.Equals("Referencia")) { XParametroInt = Referencia; }
                //else if (criterio.Equals("Etapa"))
                //{
                //    XParametro = Etapa;


                //    var lksqlAux = (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_OrdenTrabajo_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, ff, criterio, XParametroInt, XParametro, empresa).ToList());
                //    if (lksqlAux != null)
                //    {
                //        List<ParametrosBusquedaOT> listaPAC = new List<ParametrosBusquedaOT>();
                //        foreach (var item in lksqlAux)
                //        {
                //            var listaNueva = dbe.IsValidParametroCANVAS_OT(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);

                //            if (listaNueva.Equals(XParametro))
                //            {
                //                listaPAC.Add(item);
                //            }
                //        }
                //        return listaPAC;
                //    }
                //}
                else if (criterio.Equals("Localidad")) { XParametro = Localidad; }
                else if (criterio.Equals("Responsable")) { XParametroInt = Responsable; }

                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ContratoGeneral_Result>("sp_Quimipac_MT_Contrato_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, fregistro, criterio, XParametroInt, XParametro, empresa).ToList());
                return lksql;
            }
        }
        public List<sp_Quimipac_ConsultaMT_ProspectoGeneral_Result> BusquedaXParametro_Prospecto(string fi, string fregistro, DateTime? ff,  string criterio, int? tipo, string Id_Cliente, int? Estado, string Id_Linea, string Categoria, string Subcategoria, string Localidad, int? Responsable)
        { 
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            string empresa = empresa_id.ToString();
            if (true)
            {

            }
            var dbe = new DataBase_Externo();
            using (var ctx = new BD_QUIMIPACEntities())
            {
                string XParametro = string.Empty;
                int? XParametroInt = 0;

                if (criterio.Equals("Ninguno")) { XParametroInt = 0; }
                else if (criterio.Equals("Tipo")) { XParametroInt = tipo; }
                else if (criterio.Equals("Cliente")) { XParametro = Id_Cliente; }
                else if (criterio.Equals("Estado")) { XParametroInt = Estado; }
                else if (criterio.Equals("Unidad_Negocio")) { XParametro = Id_Linea; }
                else if (criterio.Equals("Categoria")) { XParametro = Categoria; }
                else if (criterio.Equals("Subcategoria")) { XParametro = Subcategoria; }
                //else if (criterio.Equals("Etapa"))
                //{
                //    XParametro = Etapa;


                //    var lksqlAux = (ctx.Database.SqlQuery<ParametrosBusquedaOT>("sp_Quimipac_MT_OrdenTrabajo_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, ff, criterio, XParametroInt, XParametro, empresa).ToList());
                //    if (lksqlAux != null)
                //    {
                //        List<ParametrosBusquedaOT> listaPAC = new List<ParametrosBusquedaOT>();
                //        foreach (var item in lksqlAux)
                //        {
                //            var listaNueva = dbe.IsValidParametroCANVAS_OT(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);

                //            if (listaNueva.Equals(XParametro))
                //            {
                //                listaPAC.Add(item);
                //            }
                //        }
                //        return listaPAC;
                //    }
                //}
                else if (criterio.Equals("Localidad")) { XParametro = Localidad; }
                else if (criterio.Equals("Responsable")) { XParametroInt = Responsable; }

                var lksql = (ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ProspectoGeneral_Result>("sp_Quimipac_MT_Contrato_QryXParametro @p0, @p1, @p2,@p3,@p4,@p5", fi, fregistro, criterio, XParametroInt, XParametro, empresa).ToList());
                return lksql;
            }
        }


        #region Filtros
        //contrato Gral
        public List<sp_Quimipac_ConsultaMT_ContratoGeneral_Result> execute_filter_contrato(DateTime? fi, DateTime? ff,List<string> parametro, string criterio_and_or,int? chkFecha) 
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

            string sql_filter = " select C.*, " 
                                +" '' as lineaContrato," 
                                +"cl.nom_cli as nombreCliente," 
                                + " '' as nombre_Categoria, '' as nombre_Subcategoria,"
                                + " cpadre.Nombre as Nomb_ContratoPadre, "
                                //+ " --t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato"
                                + " (select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion, "
                                + " (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato, "
                                + " (select Top(1) Fecha_Prorroga from MT_Contrato_Prorroga where C.Id_Contrato = Id_Contrato order by Fecha_Prorroga asc) as Fecha_proIni,"
                                + " (select Top(1) MT_Contrato_Prorroga.Dia_Prorroga from MT_Contrato_Prorroga where C.Id_Contrato = Id_Contrato order by Fecha_Prorroga asc) as Dia_ProIn,"
                                + " '' as Origen, referencia.Nombre as nom_Referencia, Localidad.descripcion as nom_Localidad, persona.primer_nombre as Nombres_Completos, cl.abreviatura_cliente  "

                                + " from MT_Contrato C "
                                //+ " inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo ='"+ empresa_id.ToString()+"'"
                                + " inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = '" + empresa_id.ToString() + "'"
                                //+ " LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo ='" + empresa_id.ToString() + "'"
                                //+ " left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = '" + empresa_id.ToString() + "'"
                                + " left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa ='" + empresa_id.ToString() + "'"
                                + " left outer join MT_Contrato referencia on c.Referencia = referencia.Id_Contrato and referencia.Id_Empresa ='" + empresa_id.ToString() + "'"
                                + " left outer join [SADATABASE]..[DBA].[LOCALIDAD] localidad on c.Localidad = localidad.codigo_loc and localidad.cia_codigo ='" + empresa_id.ToString() + "'"
                                + " left outer join [SADATABASE]..[DBA].[rh_persona] persona on c.Responsable = persona.id_persona ";
                                //+ " --inner join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado "
                                //+ " --inner join MT_TablaDetalle tipo on tipo.Id_TablaDetalle = C.tipo ";
                                //+ " where c.Id_Empresa = @empresa and c.Estado !=1160 --
                                
            string sql_where_dinamic = " Where ( c.Id_Empresa = '"+ empresa_id.ToString()+ "' and c.Estado !=1160)";

            criterio_and_or =(criterio_and_or == null || criterio_and_or =="") ? " and " : criterio_and_or;
            string Fecha_registro = "";
            if (chkFecha !=null)
            {
                if (fi != null && ff!=null)
                {
                    DateTime finicio= Convert.ToDateTime(fi);
                    DateTime ffin = Convert.ToDateTime(ff);
                    ffin = ffin.AddDays(1);
                    Fecha_registro = criterio_and_or + " c.Fecha_registro >='" + finicio.ToString("yyyy-MM-dd") + "' "+ criterio_and_or+" c.Fecha_registro < '" + ffin.ToString("yyyy-MM-dd") +"' ";

                }
            }
            sql_where_dinamic = sql_where_dinamic + " " + Fecha_registro;
            if (parametro != null)
            {


                for (int i = 0; i < parametro.Count(); i++)//paramametro = "Tipo:1" 
                {
                    var aux_split = parametro[i].Split(':');
                    criterio_and_or = (i <= (parametro.Count() - 1)) ? criterio_and_or : "";
                    string lista_parametros = "";
                    switch (aux_split[0].ToUpper())
                    {
                        case "TIPO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.tipo=" + aux_split[1] + " "; break;
                        case "CONT_PADRE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Contrato_Padre=" + aux_split[1] + " "; break;
                        case "LINEA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Id_Linea='" + aux_split[1] + "' "; break;
                        case "CATEGORIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Categoria='" + aux_split[1] + "' "; break;
                        case "SUBCATEGORIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Subcategoria='" + aux_split[1] + "' "; break;
                        case "LOCALIDAD": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Localidad='" + aux_split[1] + "' "; break;
                        case "CLIENTE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Id_Cliente='" + aux_split[1] + "'  "; break;
                        case "ESTADO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Estado=" + aux_split[1] + " "; break;
                        case "REFERENCIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Referencia=" + aux_split[1] + " "; break;
                        case "RESPONSABLE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Responsable=" + aux_split[1] + " "; break;
                        default: break;
                    }

                    


                }
               /// sql_filter = sql_filter + " " + sql_where_dinamic;
            }
            
            sql_filter = sql_filter + " " + sql_where_dinamic;
            
            var ctx = new BD_QUIMIPACEntities();
            var ListaParametros = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ContratoGeneral_Result>(sql_filter).ToList();

            return ListaParametros;
        }
        public List<sp_Quimipac_ConsultaMT_ProspectoGeneral_Result> execute_filter_prospecto(DateTime? fi, DateTime? ff,List<string> parametro, string criterio_and_or,int? chkFecha) 
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];

            string sql_filter = " select C.*, l.descripcion as lineaProspecto, cl.nom_cli as nombreCliente,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,"
                                
                                //+ " --t.Descripcion as descripcion , tipo.Descripcion as Tipo_Prospecto"
                                + " (select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion, "
                                + " (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Prospecto, "
                                + " '' as Origen, Localidad.descripcion as nom_Localidad, persona.primer_nombre as Nombres_Completos, cl.abreviatura_cliente  "

                                + " from MT_Prospecto C "
                                + " inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo ='"+ empresa_id.ToString()+"'"
                                + " inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = '" + empresa_id.ToString() + "'"
                                + " LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo ='" + empresa_id.ToString() + "'"
                                + " left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = '" + empresa_id.ToString() + "'"
                                + " left outer join [SADATABASE]..[DBA].[LOCALIDAD] localidad on c.Localidad = localidad.codigo_loc and localidad.cia_codigo ='" + empresa_id.ToString() + "'"
                                + " inner join [SADATABASE]..[DBA].[rh_persona] persona on c.Responsable = persona.id_persona ";
                                //+ " --inner join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado "
                                //+ " --inner join MT_TablaDetalle tipo on tipo.Id_TablaDetalle = C.tipo ";
                                //+ " where c.Id_Empresa = @empresa and c.Estado !=1160 --
                                
            string sql_where_dinamic = " Where ( c.Id_Empresa = '"+ empresa_id.ToString()+ "' and c.Estado !=1160)";

            criterio_and_or =(criterio_and_or == null || criterio_and_or =="") ? " and " : criterio_and_or;
            string Fecha_registro = "";
            if (chkFecha !=null)
            {
                if (fi != null && ff!=null)
                {
                    DateTime finicio= Convert.ToDateTime(fi);
                    DateTime ffin = Convert.ToDateTime(ff);
                    ffin = ffin.AddDays(1);
                    Fecha_registro = criterio_and_or + " c.Fecha_registro >='" + finicio.ToString("yyyy-MM-dd") + "' "+ criterio_and_or+" c.Fecha_registro < '" + ffin.ToString("yyyy-MM-dd") +"' ";

                }
            }
            sql_where_dinamic = sql_where_dinamic + " " + Fecha_registro;
            if (parametro != null)
            {


                for (int i = 0; i < parametro.Count(); i++)//paramametro = "Tipo:1" 
                {
                    var aux_split = parametro[i].Split(':');
                    criterio_and_or = (i <= (parametro.Count() - 1)) ? criterio_and_or : "";
                    string lista_parametros = "";
                    switch (aux_split[0].ToUpper())
                    {
                        case "TIPO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.tipo=" + aux_split[1] + " "; break;
                        case "CONT_PADRE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Contrato_Padre=" + aux_split[1] + " "; break;
                        case "LINEA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Id_Linea='" + aux_split[1] + "' "; break;
                        case "CATEGORIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Categoria='" + aux_split[1] + "' "; break;
                        case "SUBCATEGORIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Subcategoria='" + aux_split[1] + "' "; break;
                        case "LOCALIDAD": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Localidad='" + aux_split[1] + "' "; break;
                        case "CLIENTE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Id_Cliente='" + aux_split[1] + "'  "; break;
                        case "ESTADO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Estado=" + aux_split[1] + " "; break;
                        case "REFERENCIA": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Referencia=" + aux_split[1] + " "; break;
                        case "RESPONSABLE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " C.Responsable=" + aux_split[1] + " "; break;
                        default: break;
                    }

                    


                }
               /// sql_filter = sql_filter + " " + sql_where_dinamic;
            }
            
            sql_filter = sql_filter + " " + sql_where_dinamic;
            
            var ctx = new BD_QUIMIPACEntities();
            var ListaParametros = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_ProspectoGeneral_Result>(sql_filter).ToList();

            return ListaParametros;
        }
        #endregion


        #region Filtro_OT
        //Orden Trabajo
        //#..public List<ConsultaMT_OrdenTrabajo_AuxVM> execute_filter_OrdenTrabajo(DateTime? fi, DateTime? ff, List<string> parametro, string criterio_and_or, int? chkFecha, int automatizacion)
        public List<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result> execute_filter_OrdenTrabajo(DateTime? fi, DateTime? ff, List<string> parametro, string criterio_and_or, int? chkFecha, int automatizacion)
        {
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            bool band_etapa = false;
            string valor_etapa = "";
            string sql_filter = " select  DISTINCT (a_hijo.Fecha_asignacion) Fecha_asignacion	 ,a_hijo.Fecha_registro	 ,a_hijo.Id_tipo_trabajo_recibido      ,a_hijo.Id_tipo_trabajo_ejecutado	  ,a_hijo.Id_estacion ,a_hijo.Id_Proyecto ,a_hijo.Id_sucursal ,a_hijo.Estado ,a_hijo.Id_OrdenTrabajo  ,a_hijo.Codigo_Cliente  , mtp_PLA.descripcion as Dtrabajo	   , " +
                "mttej.Descripcion as clasif_orden_ttrabajoej , mtdet.Descripcion,mtp.Descripcion as trabajoejecutado  , pry.Codigo_Cliente as Cod_ClientePry	   ,a_hijo.Porcentaje_avance	   ,a_hijo.Tiempo_transcurrido	   , mtgrupo.Nombre	   ,a_hijo.Fecha_inicio_ejecucion      ,a_hijo.Fecha_fin_ejecucion      ,a_hijo.Fecha_finalizacion_obra	  ,a_hijo.Id_orden_padre	  ," +
                " a_hijo.EstadoEditOrden,	   a_hijo.Automatizacion	   ,a_hijo.Id_usuario  ,a_hijo.Id_campamento       ,a_hijo.Id_sector     ,a_hijo.Id_Prospecto,pry.Codigo_Interno as CodigoInternoProyecto, mtp.Proceso, mtp.Alerta, mtp.Caida	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,	" +
                "CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,ISNULL(mtp.Proceso, 0),a_hijo.Fecha_asignacion)),111))) THEN  			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,ISNULL(mtp.Alerta, 0) ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'			ELSE				 'ALERTA'			END	ELSE		 'EN PROCESO'	END AS Estado_P_A_C," +
                " CASE WHEN (a_hijo.Id_Proyecto != 0) THEN  pry.Codigo_Cliente  ELSE	 prosp.Codigo_Cliente + ' ' + prosp.Nombre	END AS Prospecto_Proyecto,	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz	   , prosp.Codigo_Cliente as Cod_ClienteProsp	from MT_OrdenTrabajo a_hijo	" +
                "left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 	left outer join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto and  pry.Id_Empresa = '" + empresa_id.ToString() + "'	left outer join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_recibido = mtp_PLA.Id_TipoTrabajo	left outer join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo		" +
                "left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'	left outer join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona	inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle	left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle	" +
                "left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo	left outer join MT_Prospecto prosp on a_hijo.Id_Prospecto = prosp.Id_Prospecto and prosp.Id_Empresa = '" + empresa_id.ToString() + "'";

            string sql_where_dinamic = " Where (a_hijo.EstadoEditOrden = 'A' and a_hijo.Automatizacion= " + automatizacion + " )";

            criterio_and_or = (criterio_and_or == null || criterio_and_or == "") ? " and " : criterio_and_or;
            string Fecha_registro = "";
            if (chkFecha != null)
            {
                if (fi != null && ff != null)
                {
                    DateTime finicio = Convert.ToDateTime(fi);
                    DateTime ffin = Convert.ToDateTime(ff);
                    ffin = ffin.AddDays(1);
                    Fecha_registro = criterio_and_or + " a_hijo.Fecha_registro >='" + finicio.ToString("yyyy-MM-dd") + "' " + criterio_and_or + " a_hijo.Fecha_registro < '" + ffin.ToString("yyyy-MM-dd") + "' ";

                }
            }
            sql_where_dinamic = sql_where_dinamic + " " + Fecha_registro;
            if (parametro != null)
            {
                for (int i = 0; i < parametro.Count(); i++)//paramametro = "Tipo:1" 
                {
                    var aux_split = parametro[i].Split(':');
                    criterio_and_or = (i <= (parametro.Count() - 1)) ? criterio_and_or : "";
                    string lista_parametros = "";
                    switch (aux_split[0].ToUpper())
                    {
                        case "TIPOTRABAJO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_tipo_trabajo_ejecutado=" + aux_split[1] + " "; break;
                        case "CAMPAMENTO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_campamento=" + aux_split[1] + " "; break;
                        case "ESTACION": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_estacion='" + aux_split[1] + "' "; break;
                        case "SECTOR": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_sector='" + aux_split[1] + "' "; break;
                        case "SUCURSAL": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_sucursal='" + aux_split[1] + "' "; break;
                        case "CONTRATO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_contrato='" + aux_split[1] + "' "; break;
                        case "ESTADO": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Estado='" + aux_split[1] + "'  "; break;
                        case "CLIENTE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " (pry.Id_Cliente='" + aux_split[1] + "' or prosp.Id_Cliente='" + aux_split[1] + "') "; break;
                        case "ETAPA": sql_where_dinamic = sql_where_dinamic + " "; band_etapa = true; valor_etapa = aux_split[1]; break;
                        case "ORDENPADRE": sql_where_dinamic = sql_where_dinamic + " " + criterio_and_or + " a_hijo.Id_orden_padre=" + aux_split[1] + " "; break;
                        default: break;
                    }
                }
                /// sql_filter = sql_filter + " " + sql_where_dinamic;
            }

            sql_filter = sql_filter + " " + sql_where_dinamic;

            var ctx = new BD_QUIMIPACEntities();
            var dbe = new DataBase_Externo();
            // #..var ListaParametros = ctx.Database.SqlQuery<ConsultaMT_OrdenTrabajo_AuxVM>(sql_filter).ToList();
            var ListaParametros = ctx.Database.SqlQuery<sp_Quimipac_ConsultaMT_OrdenTrabajo_Result>(sql_filter).ToList();

            // ADD PROCESO ALERTA CAIDA 
            if (band_etapa == true)
            {
                /*#....
                List<ConsultaMT_OrdenTrabajo_AuxVM> listaPAC = new List<ConsultaMT_OrdenTrabajo_AuxVM>();
                foreach (var item in ListaParametros)
                {
                    var listaNueva = dbe.IsValidParametroCANVAS_OT(item.Id_OrdenTrabajo, item.Id_tipo_trabajo_ejecutado);

                    if (listaNueva.Equals(valor_etapa))
                    {
                        listaPAC.Add(item);
                    }
                }
                return listaPAC;*/

                return ListaParametros.Where(vq => vq.Estado_P_A_C == valor_etapa).ToList();
            }
            /**/
            return ListaParametros;
        }



        #endregion

        //Excel orden automatizacion
        //Identificador, Estado, fecha_creacion_, fecha_asignacion_, inicio_ej_, fin_ej_, variable, variables, usuario, contrato_excel, empresa_id.ToString(), automatizacion);

        public int IngresarOrdenes_Excel_Aut(string Identificador, string Estado, DateTime? Fecha_de_Creacion, DateTime? Fecha_de_Asignacion,  DateTime? Inicio_de_Ejecucion, DateTime? Finalizacion_obra, string tipo_trabajo, string estacion, string usuario, int contratoexcel, string empresa, bool? automatizacion, string Origen)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {

                
                var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_Insertar_OrdenTrabajoExcel_Automat @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12",
                    Identificador, Estado,  Fecha_de_Creacion, Fecha_de_Asignacion, Inicio_de_Ejecucion, Finalizacion_obra, tipo_trabajo, estacion,  usuario, contratoexcel, empresa, automatizacion, Origen);

                return (ac != -1) ? 1 : -1;

                }
                catch (Exception e)
                {

                    return -1;
                }

            }
        }

        public List<Insertar_Contratosysbase> InsertarProspecto(int Id_Prospecto, string cia_codigo, string cod_cliente, DateTime? fecha_inicial, DateTime? fecha_fin, string codigo_secuencial_interno, string codigo_prospecto_asociado, string user_id, string unidad, string cod_servicio, string codcen, string detalle, int? id_estado,
            int? plazo_prospecto, int? cod_tipo, decimal? Valor_Referencial, decimal? monto, decimal? costo, int? Responsable, int? secuencial, string Codigo_Interno_Ant, string observaciones, string codigo_secuencial_interno_padre, DateTime? Fecha_registro, DateTime? Fecha_modificacion, string Localidad, DateTime? Fecha_Aprobacion_Cot, string Recepcion_Servicio, DateTime? Fecha_Firma_Conformidad, DateTime? Fecha_Cumplimiento_Inst)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {
                    //var ac = ctx.Database.SqlQuery<Insertar_Contratosysbase>("sp_Quimipac_InsertarContratoSysbase @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19,@p20,@p21,@p22,@p23,@p24,@p25", Id_Contrato, cia_codigo, cod_cliente, fecha_inicial, fecha_fin, codigo_secuencial_interno, codigo_prospecto_asociado, user_id, unidad, cod_servicio, codcen, detalle, id_estado, plazo_prospecto, cod_tipo, Contrato_Padre, Valor_Referencial, monto, costo, Responsable, secuencial, Codigo_Interno_Ant, observaciones, codigo_secuencial_interno_padre, Fecha_registro, Fecha_modificacion).ToList();
                    var ac = ctx.Database.SqlQuery<Insertar_Contratosysbase>("sp_Quimipac_InsertarProspectoSysbase @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29,@p30", Id_Prospecto, cia_codigo, cod_cliente, fecha_inicial, fecha_fin, codigo_secuencial_interno, codigo_prospecto_asociado, user_id, unidad, cod_servicio, codcen, detalle, id_estado, plazo_prospecto, cod_tipo, Valor_Referencial, monto, costo, Responsable, secuencial, Codigo_Interno_Ant, observaciones, codigo_secuencial_interno_padre, Fecha_registro, Fecha_modificacion, Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst).ToList();
                    //25,'02','1', '28/06/2019 0:00:00', '01/07/2019 0:00:00', 'HYDIWSTYAPRO0022019', 'Req # 19001269-01', 'BSALTO',
                    //'TYA','02','080', 'Elementos para Estaciones de Bombeo: Fuente conmutable (12) y supresores de voltaje para cable coaxial (20)',75,3,146,null,0.0000,8160.0000,6456.0000,
                    //null,2,'HYDIWSTYAPRO0022019','REQUISICION SOLICITADA POR LA ING. JANETA','HYDIWSTYAPRO0022019','19/03/2020 0:00:00',null




                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //throw new System.Exception(ex.Message);
                }
                return null;
            }
        }


        //Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones

        public List<MT_Prospecto> UpdateProspecto(int Id_Prospecto, string Id_Cliente, DateTime? Fecha_Inicio, DateTime? Fecha_Fin, string Codigo_Interno, string Codigo_Cliente, string Id_Linea, string Categoria, string Subcategoria, string Nombre, int? Estado, int? Dia_Plazo, int? tipo,  decimal? Valor_Referencial, decimal? monto, decimal? costo, int? Responsable, int? Secuencial, string Codigo_Interno_Ant, string Observaciones, string Codigo_interno_padre, DateTime? Fecha_registro, DateTime? Fecha_modificacion, string Localidad, DateTime? Fecha_Aprobacion_Cot, string Recepcion_Servicio, DateTime? Fecha_Firma_Conformidad, DateTime? Fecha_Cumplimiento_Inst)
        {
            using (var ctx = new BD_QUIMIPACEntities())
            {
                try
                {
                    var ac = ctx.Database.ExecuteSqlCommand("sp_Quimipac_UpdateProspecto @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27", Id_Prospecto, Id_Cliente, Fecha_Inicio, Fecha_Fin, Codigo_Interno, Codigo_Cliente, Id_Linea, Categoria, Subcategoria, Nombre, Estado, Dia_Plazo, tipo, Valor_Referencial, monto, costo, Responsable, Secuencial, Codigo_Interno_Ant, Observaciones, Codigo_interno_padre, Fecha_registro, Fecha_modificacion, Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst);

                    return null;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw new System.Exception(ex.Message);
                }
            }
        }


    }
}