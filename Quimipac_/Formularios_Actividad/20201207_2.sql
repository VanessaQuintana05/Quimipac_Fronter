USE [BD_QUIMIPAC]
GO
/****** Object:  User [fronter]    Script Date: 04/10/2020 22:26:19 ******/
CREATE USER [fronter] FOR LOGIN [fronter] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [pbi]    Script Date: 04/10/2020 22:26:19 ******/
CREATE USER [pbi] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [fronter]
GO
ALTER ROLE [db_owner] ADD MEMBER [pbi]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[Id_Menu] [int] IDENTITY(1,1) NOT NULL,
	[Menu] [varchar](50) NULL,
	[Padre] [int] NULL,
	[Orden] [int] NULL,
	[Nivel_Profundidad] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Action] [varchar](max) NULL,
	[Controlador] [varchar](50) NULL,
	[Icono] [varchar](50) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id_Menu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuRol]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuRol](
	[Id_MenuRol] [int] IDENTITY(1,1) NOT NULL,
	[Id_Rol] [int] NULL,
	[Id_Menu] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MenuRol] PRIMARY KEY CLUSTERED 
(
	[Id_MenuRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Actividad]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Actividad](
	[Id_Actividad] [int] IDENTITY(1,1) NOT NULL,
	[Id_TipoTrabajo] [int] NULL,
	[Id_ActividadPadre] [int] NULL,
	[Codigo] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Tiempo_Estimado] [time](7) NULL,
	[Peso_Actividad] [float] NULL,
	[Orden] [int] NULL,
	[Obligatorio] [varchar](1) NULL,
	[Estado] [varchar](1) NULL,
	[Tipo] [int] NULL,
	[Id_Empresa] [varchar](5) NULL,
 CONSTRAINT [PK_MT_Actividad] PRIMARY KEY CLUSTERED 
(
	[Id_Actividad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Campamento]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Campamento](
	[Id_Campamento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Nombre] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Campamento] PRIMARY KEY CLUSTERED 
(
	[Id_Campamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Contrato]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Contrato](
	[Id_Contrato] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Codigo_Interno] [varchar](75) NULL,
	[Codigo_Cliente] [varchar](75) NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_Linea] [varchar](10) NULL,
	[Categoria] [varchar](10) NULL,
	[Subcategoria] [varchar](13) NULL,
	[Nombre] [varchar](max) NULL,
	[Estado] [int] NULL,
	[Dia_Plazo] [int] NULL,
	[tipo] [int] NULL,
	[Contrato_Padre] [int] NULL,
	[Valor_Referencial] [numeric](18, 4) NULL,
	[monto] [numeric](18, 4) NULL,
	[costo] [numeric](18, 4) NULL,
	[Responsable] [int] NULL,
	[Secuencial] [int] NULL,
	[Codigo_Interno_Ant] [varchar](75) NULL,
	[Observaciones] [varchar](max) NULL,
	[Codigo_interno_padre] [varchar](75) NULL,
	[Fecha_registro] [datetime] NULL,
	[Fecha_modificacion] [datetime] NULL,
	[Localidad] [varchar](2) NULL,
	[Fecha_Aprobacion_Cot] [datetime] NULL,
	[Recepcion_Servicio] [varchar](max) NULL,
	[Fecha_Firma_Conformidad] [datetime] NULL,
	[Fecha_Cumplimiento_Inst] [datetime] NULL,
	[Referencia] [int] NULL,
 CONSTRAINT [PK_Contrat] PRIMARY KEY CLUSTERED 
(
	[Id_Contrato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Contrato_Documentado]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Contrato_Documentado](
	[Id_Contrato_Documentado] [int] IDENTITY(1,1) NOT NULL,
	[Id_Contrato] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[Enlace] [varchar](max) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Validez] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[NombreArchivo] [varchar](max) NULL,
	[Tipo] [int] NULL,
	[Version] [numeric](18, 2) NULL,
 CONSTRAINT [PK_MT_Contrato_Documentado] PRIMARY KEY CLUSTERED 
(
	[Id_Contrato_Documentado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Contrato_Prorroga]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Contrato_Prorroga](
	[Id_Contrato_Prorroga] [int] IDENTITY(1,1) NOT NULL,
	[Id_Contrato] [int] NULL,
	[Fecha_Prorroga] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Descripcion] [varchar](max) NULL,
	[Dia_Prorroga] [int] NULL,
 CONSTRAINT [PK_MT_Contrato_Prorroga] PRIMARY KEY CLUSTERED 
(
	[Id_Contrato_Prorroga] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_ContratoAlerta]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_ContratoAlerta](
	[Id_Contrato_Alerta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Contrato] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Alerta] [datetime] NULL,
	[Mensaje] [varchar](max) NULL,
	[Repetir] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Correo] [varchar](50) NULL,
 CONSTRAINT [PK_MT_ContratoAlerta] PRIMARY KEY CLUSTERED 
(
	[Id_Contrato_Alerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_EntregaOrden_Trabajo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_EntregaOrden_Trabajo](
	[Id_Entrega_Orden_Trabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Fecha_Elaboracion] [datetime] NULL,
	[Fecha_Envio] [datetime] NULL,
	[Fecha_Recepcion] [datetime] NULL,
	[Usuario] [varchar](10) NULL,
	[Comentario] [varchar](max) NULL,
	[Recibido_Por] [varchar](max) NULL,
 CONSTRAINT [PK_MT_EntregaOrden_Trabajo] PRIMARY KEY CLUSTERED 
(
	[Id_Entrega_Orden_Trabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Equipo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Equipo](
	[Id_Equipo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proveedor] [varchar](50) NULL,
	[Id_Persona_asignada] [int] NULL,
	[Tipo] [int] NULL,
	[Codigo] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Numero_Serie] [varchar](max) NULL,
	[Marca] [varchar](max) NULL,
	[Modelo] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Horas_Usadas] [int] NULL,
 CONSTRAINT [PK_MT_Equipo] PRIMARY KEY CLUSTERED 
(
	[Id_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Equipo_Evento]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Equipo_Evento](
	[Id_Equipo_Evento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Equipo] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Tipo_Evento] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Proveedor] [varchar](max) NULL,
	[Valor] [numeric](18, 0) NULL,
	[Comentario] [varchar](max) NULL,
	[Id_Persona_Origen] [int] NULL,
	[Id_Persona_Destino] [int] NULL,
 CONSTRAINT [PK_MT_Equipo_Evento] PRIMARY KEY CLUSTERED 
(
	[Id_Equipo_Evento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Estacion]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Estacion](
	[Id_Estacion] [int] IDENTITY(1,1) NOT NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Id_Provincia] [int] NULL,
	[Id_Ciudad] [int] NULL,
	[Id_Macrosector] [int] NULL,
	[Nombre] [varchar](max) NULL,
	[Direccion] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
	[Coordenada_X] [varchar](50) NULL,
	[Coordenada_Y] [varchar](50) NULL,
 CONSTRAINT [PK_Estacion] PRIMARY KEY CLUSTERED 
(
	[Id_Estacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Fiscalizador]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Fiscalizador](
	[Id_Fiscalizador] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Contrato] [int] NULL,
	[Nombre] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
	[Tipo] [varchar](max) NULL,
 CONSTRAINT [PK_MT_Fiscalizador] PRIMARY KEY CLUSTERED 
(
	[Id_Fiscalizador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Formulario]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Formulario](
	[Id_Formulario] [int] IDENTITY(1,1) NOT NULL,
	[Id_Actividad] [int] NULL,
	[Codigo] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Enlace_Formulario] [varchar](max) NULL,
	[NombreArchivo] [varchar](max) NULL,
	[Fecha_Creacion] [datetime] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Formulario] PRIMARY KEY CLUSTERED 
(
	[Id_Formulario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_GrupoTrabajo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_GrupoTrabajo](
	[Id_GrupoTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Campamento] [int] NULL,
	[Nombre] [varchar](max) NULL,
	[Tipo] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Bodega] [varchar](10) NULL,
	[Color] [varchar](7) NULL,
	[Id_Vehiculo] [varchar](5) NULL,
 CONSTRAINT [PK__MT_Grupo__401ACB225B795103] PRIMARY KEY CLUSTERED 
(
	[Id_GrupoTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_GrupoTrabajoEquipo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_GrupoTrabajoEquipo](
	[Id_GrupoTrabajoEquipo] [int] IDENTITY(1,1) NOT NULL,
	[Id_GrupoTrabajo] [int] NULL,
	[Id_Equipo] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_GrupoTrabajoEquipo] PRIMARY KEY CLUSTERED 
(
	[Id_GrupoTrabajoEquipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_GrupoTrabajoIntegrante]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_GrupoTrabajoIntegrante](
	[Id_GrupoTrabajoIntegrante] [int] IDENTITY(1,1) NOT NULL,
	[Id_GrupoTrabajo] [int] NULL,
	[Id_Persona] [int] NULL,
	[Rol_Usuario] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_GrupoTrabajoIntegrante] PRIMARY KEY CLUSTERED 
(
	[Id_GrupoTrabajoIntegrante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Items]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Items](
	[Id_ItemTipoTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_TipoTrabajo] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Unidad] [varchar](50) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Items] PRIMARY KEY CLUSTERED 
(
	[Id_ItemTipoTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_ItemsBodega]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_ItemsBodega](
	[Id_ItemsBodega] [varchar](50) NOT NULL,
	[Bodega] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Cantidad] [int] NULL,
 CONSTRAINT [PK_MT_ItemsBodega] PRIMARY KEY CLUSTERED 
(
	[Id_ItemsBodega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Materiales_Solicitud]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Materiales_Solicitud](
	[Id_Material_Solicitud] [int] IDENTITY(1,1) NOT NULL,
	[Id_Solicitud] [int] NULL,
	[fecha] [datetime] NULL,
	[Id_Item] [varchar](20) NULL,
	[Cantidad] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Materiales_Solicitud] PRIMARY KEY CLUSTERED 
(
	[Id_Material_Solicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Notificacion_Persona]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Notificacion_Persona](
	[Id_Notificacion_Persona] [int] IDENTITY(1,1) NOT NULL,
	[Id_Notificacion] [int] NULL,
	[Id_Persona] [int] NULL,
	[Tipo] [int] NULL,
	[Correo] [varchar](max) NULL,
	[Fecha_Hora] [datetime] NULL,
	[Estado] [int] NULL,
 CONSTRAINT [PK_MT_Notificacion_Persona] PRIMARY KEY CLUSTERED 
(
	[Id_Notificacion_Persona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Notificaciones]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Notificaciones](
	[Id_Notificacion] [int] IDENTITY(1,1) NOT NULL,
	[Tipo_Notificacion] [int] NULL,
	[Id_Codigo_origen] [int] NULL,
	[Id_usuario] [varchar](10) NULL,
	[Fecha] [datetime] NULL,
	[Prioridad] [int] NULL,
	[Asunto] [varchar](max) NULL,
	[Mensaje] [varchar](max) NULL,
	[Criterio] [int] NULL,
	[Frecuencia] [int] NULL,
	[Enviado] [bit] NULL,
	[Fecha_Envio] [datetime] NULL,
	[Reenvio] [bit] NULL,
	[EmpresaID] [varchar](2) NULL,
	[Estado] [bit] NULL,
 CONSTRAINT [PK_MT_Notificaciones] PRIMARY KEY CLUSTERED 
(
	[Id_Notificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo](
	[Id_OrdenTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Id_sucursal] [varchar](10) NULL,
	[Id_campamento] [int] NULL,
	[Id_tipo_trabajo_recibido] [int] NULL,
	[Id_tipo_trabajo_ejecutado] [int] NULL,
	[Estado] [int] NULL,
	[Id_sector] [int] NULL,
	[Id_orden_padre] [int] NULL,
	[Id_estacion] [int] NULL,
	[Id_usuario] [varchar](10) NULL,
	[Id_entrega_orden_trabajo] [int] NULL,
	[Nivel_prioridad] [int] NULL,
	[Fecha_registro] [datetime] NULL,
	[Fecha_creacion_cliente] [datetime] NULL,
	[Fecha_maxima_ejecucion_cliente] [datetime] NULL,
	[Fecha_asignacion_grupo_trabajo] [datetime] NULL,
	[Fecha_inicio_ejecucion] [datetime] NULL,
	[Fecha_fin_ejecucion] [datetime] NULL,
	[Fecha_finalizacion_obra] [datetime] NULL,
	[Fecha_ini_permiso_municipal] [datetime] NULL,
	[Fecha_fin_permiso_municipal] [datetime] NULL,
	[Fecha_entrega] [datetime] NULL,
	[Fecha_max_legalizacion] [datetime] NULL,
	[Hora_acordada] [time](7) NULL,
	[Id_barrio] [int] NULL,
	[Direccion] [varchar](max) NULL,
	[Referencia_direccion] [varchar](max) NULL,
	[Identificacion_suscriptor] [varchar](max) NULL,
	[Nombre_suscriptor] [varchar](max) NULL,
	[Tipo_suscriptor] [varchar](max) NULL,
	[Telefono_suscriptor] [varchar](max) NULL,
	[Origen] [varchar](max) NULL,
	[Comentario] [varchar](max) NULL,
	[Porcentaje_avance] [varchar](max) NULL,
	[Tiempo_transcurrido] [time](7) NULL,
	[Id_planilla] [int] NULL,
	[Estado_orden_planilla] [varchar](max) NULL,
	[Codigo_Cliente] [varchar](max) NULL,
	[Interna] [varchar](1) NULL,
	[EstadoEditOrden] [varchar](1) NULL,
	[Excel_orden] [bit] NULL,
	[Fecha_asignacion] [datetime] NULL,
	[Fecha_maxima_contratista] [datetime] NULL,
	[Desalojo] [bit] NULL,
	[Automatizacion] [bit] NULL,
	[Id_Prospecto] [int] NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Actividad]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Actividad](
	[Id_OrdenTrabajo_Actividad] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Actividad] [int] NULL,
	[Estado] [int] NULL,
	[Orden] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Comentario] [varchar](max) NULL,
	[EstadoAct] [varchar](1) NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Fecha] [datetime] NULL,
	[Id_Usuario_Modifica] [varchar](10) NULL,
	[Fecha_Modifica] [datetime] NULL,
	[Responsable] [varchar](max) NULL,
	[Nombre_Actividad] [varchar](max) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Actividad] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Actividad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Anexo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Anexo](
	[Id_OrdenTrabajo_Anexo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Tipo_Anexo] [int] NULL,
	[Enlace_Anexo] [varchar](max) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Observacion] [varchar](max) NULL,
	[NombreArchivo] [varchar](80) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Anexo] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Anexo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_CausaRaiz]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_CausaRaiz](
	[Id_OrdenTrabajo_CausaRaiz] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Causa_Raiz] [int] NULL,
	[Motivo_Causa] [varchar](max) NULL,
	[Encontro] [int] NULL,
	[Motivo_E] [varchar](max) NULL,
	[Hizo] [int] NULL,
	[Motivo_H] [varchar](max) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_CausaRaiz] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_CausaRaiz] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Egreso]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Egreso](
	[Id_OrdenTrabajo_Egreso] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Fecha] [datetime] NULL,
	[Moneda] [varchar](50) NULL,
	[Unidad] [varchar](50) NULL,
	[Cantidad] [int] NULL,
	[Precio] [decimal](18, 4) NULL,
	[Costo] [decimal](18, 4) NULL,
	[Area] [float] NULL,
	[Tipo] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[EstadoAct] [varchar](1) NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_Usuario_Modifica] [varchar](10) NULL,
	[Fecha_Modifica] [datetime] NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Egreso] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Egreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Equipo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Equipo](
	[Id_OrdenTrabajo_Equipo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Equipo] [int] NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Id_GrupoTrabajo] [int] NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Equipo] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Estado]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Estado](
	[Id_OrdenTrabajo_Estado] [int] IDENTITY(1,1) NOT NULL,
	[Id_orden_Trabajo] [int] NULL,
	[Estado_Orden_Trabajo] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[EstadoO] [varchar](1) NULL,
	[Comentario] [varchar](max) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Estado] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Formulario]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Formulario](
	[Id_OrdenTrabajo_Formulario] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo_Detalle] [int] NULL,
	[Enlace_Formulario] [varchar](max) NULL,
	[NombreArchivo] [varchar](max) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Formulario] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Formulario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Integrante]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Integrante](
	[Id_OrdenTrabajo_Integrante] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Persona] [int] NULL,
	[Rol] [int] NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Id_GrupoTrabajo] [int] NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Integrante] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Integrante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Medida]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Medida](
	[Id_Orden_Trabajo_Medida] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Tipo_Trabajo_Medida] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Valor_Ini] [varchar](10) NULL,
	[Valor_Fin] [varchar](10) NULL,
	[EstadoAct] [varchar](1) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Medida] PRIMARY KEY CLUSTERED 
(
	[Id_Orden_Trabajo_Medida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Novedad]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Novedad](
	[Id_OrdenTrabajo_Novedad] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Usuario] [varchar](20) NULL,
	[Tipo_Novedad] [varchar](50) NULL,
	[Novedad] [varchar](max) NULL,
	[Fecha] [date] NULL,
	[Id_Dirigido_a1] [varchar](max) NULL,
	[Id_Dirigido_a2] [varchar](max) NULL,
	[Id_Dirigido_a3] [varchar](max) NULL,
	[Id_Dirigido_a4] [varchar](max) NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Novedad] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Novedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_OrdenTrabajo_Valor]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_OrdenTrabajo_Valor](
	[Id_OrdenTrabajo_Valor] [int] IDENTITY(1,1) NOT NULL,
	[Id_Orden_Trabajo] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Tipo_Valor] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Moneda] [varchar](30) NULL,
	[Fecha] [datetime] NULL,
	[Unidad] [varchar](50) NULL,
	[Cantidad] [int] NULL,
	[Precio] [numeric](18, 4) NULL,
	[Costo] [numeric](18, 4) NULL,
	[Iva] [numeric](18, 4) NULL,
	[Codigo_Valor_Costo] [varchar](20) NULL,
	[Id_Usuario_Modifica] [varchar](10) NULL,
	[Fecha_Modifica] [datetime] NULL,
	[Secuencia] [int] NULL,
 CONSTRAINT [PK_MT_OrdenTrabajo_Valor] PRIMARY KEY CLUSTERED 
(
	[Id_OrdenTrabajo_Valor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Permiso]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Permiso](
	[Id_Permiso] [int] IDENTITY(1,1) NOT NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Id_Contrato] [int] NULL,
	[Id_Proyecto] [int] NULL,
	[Id_Tipo_Trabajo] [int] NULL,
	[Id_Linea] [varchar](3) NULL,
	[Consultar] [varchar](1) NULL,
	[Modificar] [varchar](1) NULL,
	[Crear] [varchar](1) NULL,
	[Eliminar] [varchar](1) NULL,
	[Aprobar] [varchar](1) NULL,
	[Usuario] [varchar](20) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Id_Empresa] [varchar](2) NULL,
	[Id_Prospecto] [int] NULL,
 CONSTRAINT [PK_MT_Permiso] PRIMARY KEY CLUSTERED 
(
	[Id_Permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Planilla]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Planilla](
	[Id_Planilla] [int] IDENTITY(1,1) NOT NULL,
	[Id_PoyectoContr] [int] NULL,
	[Fecha_Asignacion] [datetime] NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Usuario] [varchar](30) NULL,
	[Estado] [int] NULL,
	[Identificador] [varchar](max) NULL,
	[Tipo_Trabajo] [int] NULL,
	[Unidad_Trabajo] [varchar](max) NULL,
	[Fecha_Legalizacion] [datetime] NULL,
	[Ubicacion_Geografica] [varchar](max) NULL,
	[Tipo_C_P] [int] NULL,
 CONSTRAINT [PK_MT_Planilla] PRIMARY KEY CLUSTERED 
(
	[Id_Planilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Planilla_Detalle]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Planilla_Detalle](
	[Id_Planilla_Detalle] [int] IDENTITY(1,1) NOT NULL,
	[Id_Planilla] [int] NULL,
	[Codigo_acta] [varchar](50) NULL,
	[Base_Admnistrativa] [varchar](250) NULL,
	[Descripcion_de_Acta] [varchar](150) NULL,
	[Codigo_Periodo] [decimal](14, 0) NULL,
	[Descripcion_de_Periodo] [varchar](50) NULL,
	[Codigo_Tipo_de_Acta] [varchar](10) NULL,
	[Descripcion_de_Tipo_de_Acta] [varchar](100) NULL,
	[Fecha_Inicial_Periodo] [datetime] NULL,
	[Fecha_Final_Periodo] [datetime] NULL,
	[Fecha_Cierre_Acta] [datetime] NULL,
	[Estado_Acta] [varchar](50) NULL,
	[Descripcion_de_Estado_Acta] [varchar](50) NULL,
	[Orden_Raiz] [varchar](100) NULL,
	[No_Orden_Padre] [varchar](100) NULL,
	[No_Orden] [varchar](100) NULL,
	[Codigo_Detalle_Acta] [varchar](50) NULL,
	[Código_Items] [varchar](50) NULL,
	[Descripcion_de_Items] [varchar](500) NULL,
	[Cantidad] [decimal](14, 4) NULL,
	[Codigo_Unidad] [varchar](50) NULL,
	[Unidad] [varchar](20) NULL,
	[Valor_Unitario] [decimal](14, 4) NULL,
	[Valor_Total] [decimal](14, 4) NULL,
	[Codigo_Listado_de_Costo] [varchar](50) NULL,
	[Descripcion_Listado_de_Costo] [varchar](250) NULL,
	[Tipo_Generacion] [varchar](50) NULL,
	[Codigo_de_Plan_de_Condicion] [varchar](50) NULL,
	[Tipo_de_Trabajo] [varchar](250) NULL,
	[Descripcion_de_Tipo_de_Trabajo] [varchar](250) NULL,
	[Actividad] [varchar](250) NULL,
	[Fecha_Asignacion] [datetime] NULL,
	[Fecha_Fin_Permiso_Municipal_Calc] [datetime] NULL,
	[Fecha_Asignacion_OT] [datetime] NULL,
	[Fecha_Ejecucion] [datetime] NULL,
	[Fecha_Legalizacion] [datetime] NULL,
	[Fecha_Ejecucion_OT_Padre] [datetime] NULL,
	[Tiempo_Promedio_Inc_Desc_OT] [decimal](14, 4) NULL,
	[Tiempo_Promedio_Desc_Descarga] [decimal](14, 4) NULL,
	[Tiempo_Transcurrido_HORAS] [decimal](14, 4) NULL,
	[Tiempo_Optimo_Incentivo] [decimal](14, 4) NULL,
	[Tiempo_Máximo_Incentivo] [decimal](14, 4) NULL,
	[Porcentaje_Incentivo] [decimal](14, 4) NULL,
	[Tiempo_Optimo_Desc_Atencion] [decimal](14, 4) NULL,
	[Tiempo_Máximo_Desc_Atencion] [decimal](14, 4) NULL,
	[Porcentaje_de_Desc_Atencion] [decimal](14, 4) NULL,
	[Tiempo_Máximo_Desc_Descarga] [decimal](14, 4) NULL,
	[Tiempo_Excede_Desc_Descarga] [decimal](14, 4) NULL,
	[Porcentaje_Desc_Descarga] [decimal](14, 4) NULL,
	[Valor_Aplica_Desc_Atencion_OT] [decimal](14, 4) NULL,
	[Codigo_Contrato] [varchar](50) NULL,
	[Descripcion_Contrato] [varchar](250) NULL,
	[Tipo_de_Contrato] [varchar](50) NULL,
	[Descripcion_de_Tipo_de_Contrato] [varchar](250) NULL,
	[Contratista] [varchar](250) NULL,
	[Descripcion_Contratista] [varchar](50) NULL,
	[Porcentaje_Costo_Indirecto_Contrato] [decimal](14, 4) NULL,
	[Porcentaje_Fondo_Garantia_Contrato] [decimal](14, 4) NULL,
	[Porcentaje_Amortizacion_Contrato] [decimal](14, 4) NULL,
	[Descuento_General] [decimal](14, 4) NULL,
	[Codigo_Unidad_Operativa] [varchar](50) NULL,
	[Descripcion_Unidad_Operativa] [varchar](250) NULL,
	[Terminal] [varchar](250) NULL,
	[Valor_Base] [varchar](50) NULL,
	[Cumplimiento] [varchar](50) NULL,
	[Tiempo_Contractual_HORAS] [decimal](14, 4) NULL,
	[Tiempo_de_Permiso_Municipal_HORAS] [decimal](14, 4) NULL,
	[Tipo_Clasifica] [varchar](150) NULL,
	[Inventario] [varchar](50) NULL,
	[Tiene_Caso_Especial] [varchar](1) NULL,
	[Tipoe_de_Relacion] [varchar](50) NULL,
	[Nro_Contrato] [varchar](50) NULL,
	[Nro_Producto] [varchar](50) NULL,
	[Codigo] [varchar](50) NULL,
	[Tipo_de_Irregularidad] [varchar](50) NULL,
	[Descripcion_de_Proyecto] [varchar](50) NULL,
	[Usuario_Finalizo] [varchar](150) NULL,
	[Codigo_Tipo_Comentario] [varchar](150) NULL,
	[Comentario] [varchar](500) NULL,
	[Observacion_Orden_Actividad] [varchar](500) NULL,
	[Fecha_Pago_Sistema_Externo] [varchar](50) NULL,
	[Numero_Factura_Sistema_Externo] [varchar](50) NULL,
	[Cod_estado_orden] [varchar](50) NULL,
 CONSTRAINT [PK_MT_Planilla_Detalle] PRIMARY KEY CLUSTERED 
(
	[Id_Planilla_Detalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_PrecioReferencial]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_PrecioReferencial](
	[Id_PrecioReferencial] [int] IDENTITY(1,1) NOT NULL,
	[Id_TipoTablaDetalle] [int] NULL,
	[Id_Proyecto_Contrato] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Id_TipoTrabajo] [int] NULL,
	[Id_Usuario] [varchar](30) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Moneda] [varchar](30) NULL,
	[Precio] [numeric](18, 4) NULL,
	[Costo] [numeric](18, 4) NULL,
	[Estado] [varchar](10) NULL,
	[Unidad] [varchar](10) NULL,
 CONSTRAINT [PK_MT_PrecioReferencial] PRIMARY KEY CLUSTERED 
(
	[Id_PrecioReferencial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Presupuesto]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Presupuesto](
	[Id_Presupuesto] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Id_Sucursal] [varchar](5) NULL,
	[Id_Cliente] [varchar](20) NULL,
	[Fecha] [datetime] NULL,
	[Id_Usuario] [varchar](30) NULL,
	[Comentario] [varchar](max) NULL,
	[Porcentaje_indirectos] [numeric](18, 0) NULL,
	[Monto_Certificacion] [numeric](18, 0) NULL,
	[Iva_Certificacion] [numeric](18, 0) NULL,
	[Validez] [varchar](50) NULL,
	[Moneda] [varchar](30) NULL,
 CONSTRAINT [PK_MT_Presupuesto] PRIMARY KEY CLUSTERED 
(
	[Id_Presupuesto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Presupuesto_Detalle]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Presupuesto_Detalle](
	[Id_Presupuesto_detalle] [int] IDENTITY(1,1) NOT NULL,
	[Id_Presupuesto] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Cantidad] [int] NULL,
	[unidad] [varchar](50) NULL,
	[Precio] [numeric](18, 0) NULL,
	[Valor] [numeric](18, 0) NULL,
	[IVA] [numeric](18, 0) NULL,
 CONSTRAINT [PK_MT_Presupuesto_Detalle] PRIMARY KEY CLUSTERED 
(
	[Id_Presupuesto_detalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_ProgramaTrabajo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_ProgramaTrabajo](
	[Id_ProgramaTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Contrato] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_TIpo_Trabajo] [int] NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Descripcion] [varchar](max) NULL,
	[Direccion] [varchar](max) NULL,
	[Ubicacion] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
	[Id_GrupoTrabajo] [int] NULL,
 CONSTRAINT [PK_MT_ProgramaTrabajo] PRIMARY KEY CLUSTERED 
(
	[Id_ProgramaTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Prospecto]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Prospecto](
	[Id_Prospecto] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_Linea] [varchar](10) NULL,
	[Categoria] [varchar](10) NULL,
	[Subcategoria] [varchar](13) NULL,
	[tipo] [int] NULL,
	[Responsable] [int] NULL,
	[Localidad] [varchar](2) NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Codigo_Interno] [varchar](75) NULL,
	[Codigo_Cliente] [varchar](75) NULL,
	[Nombre] [varchar](max) NULL,
	[Estado] [int] NULL,
	[Dia_Plazo] [int] NULL,
	[Valor_Referencial] [numeric](18, 4) NULL,
	[monto] [numeric](18, 4) NULL,
	[costo] [numeric](18, 4) NULL,
	[Secuencial] [int] NULL,
	[Codigo_Interno_Ant] [varchar](75) NULL,
	[Observaciones] [varchar](max) NULL,
	[Codigo_interno_padre] [varchar](75) NULL,
	[Fecha_registro] [datetime] NULL,
	[Fecha_modificacion] [datetime] NULL,
	[Fecha_Aprobacion_Cot] [datetime] NULL,
	[Recepcion_Servicio] [varchar](max) NULL,
	[Fecha_Firma_Conformidad] [datetime] NULL,
	[Fecha_Cumplimiento_Inst] [datetime] NULL,
 CONSTRAINT [PK_Prospecto] PRIMARY KEY CLUSTERED 
(
	[Id_Prospecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mt_Prospecto_Alerta]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mt_Prospecto_Alerta](
	[Id_Prospecto_Alerta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Prospecto] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Alerta] [datetime] NULL,
	[Mensaje] [varchar](max) NULL,
	[Repetir] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Correo] [varchar](50) NULL,
 CONSTRAINT [PK_Mt_Prospecto_Alerta] PRIMARY KEY CLUSTERED 
(
	[Id_Prospecto_Alerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Prospecto_Documentado]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Prospecto_Documentado](
	[Id_Prospecto_Documentado] [int] IDENTITY(1,1) NOT NULL,
	[Id_Prospecto] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[Enlace] [varchar](max) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Validez] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[NombreArchivo] [varchar](max) NULL,
	[Tipo] [int] NULL,
 CONSTRAINT [PK_MT_Prospecto_Documentado] PRIMARY KEY CLUSTERED 
(
	[Id_Prospecto_Documentado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto](
	[Id_Proyecto] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Id_Sucursal] [varchar](3) NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Id_TipoTrabajo] [int] NULL,
	[Id_Presupuesto] [int] NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Fecha_Prorroga] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Codigo_Interno] [varchar](75) NULL,
	[Codigo_Cliente] [varchar](75) NULL,
	[Direccion] [varchar](max) NULL,
	[Ubicacion] [varchar](20) NULL,
	[Comentario] [varchar](max) NULL,
	[Porcentaje_avance] [varchar](20) NULL,
	[Valor_Inicial] [numeric](18, 4) NULL,
	[Valor_Ajustado] [numeric](18, 4) NULL,
	[Linea] [varchar](10) NULL,
	[Categoria] [varchar](10) NULL,
	[Subcategoria] [varchar](13) NULL,
	[Fecha_Anticipo] [datetime] NULL,
	[Id_contrato] [int] NULL,
	[Id_Prospecto] [int] NULL,
 CONSTRAINT [PK_MT_Proyecto] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad](
	[Id_Proyecto_Actividad] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Id_Actividad] [int] NULL,
	[Estado] [int] NULL,
	[Orden] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Comentario] [varchar](max) NULL,
	[Id_Planilla] [int] NULL,
	[Estado_Actividad_Plantilla] [int] NULL,
	[EstadoAct] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad_Anexo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad_Anexo](
	[Id_Proyecto_Actividad_Anexo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Tipo] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[Enlace] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad_Anexo] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad_Anexo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad_Equipo]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad_Equipo](
	[Id_Proyecto_Actividad_Equipo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Id_Equipo] [int] NULL,
	[Id_Persona] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad_Equipo] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad_Integrante]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad_Integrante](
	[Id_Proyecto_Actividad_Integrante] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Id_Persona] [int] NULL,
	[Rol] [int] NULL,
	[Equipo] [int] NULL,
	[Fecha_Ini] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad_Integrante] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad_Integrante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad_Novedad]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad_Novedad](
	[Id_Proyecto_Actividad_Novedad] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Id_usuario] [varchar](10) NULL,
	[Tipo_Novedad] [varchar](50) NULL,
	[Novedad] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
	[Id_Dirigido_a] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad_Novedad] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad_Novedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Actividad_Valor]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Actividad_Valor](
	[Id_Proyecto_Actividad_Valor] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Tipo_Valor] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Moneda] [varchar](30) NULL,
	[Fecha] [datetime] NULL,
	[Unidad] [varchar](50) NULL,
	[Cantidad] [int] NULL,
	[Precio] [numeric](18, 0) NULL,
	[Costo] [numeric](18, 0) NULL,
	[IVA] [numeric](18, 0) NULL,
 CONSTRAINT [PK_MT_Proyecto_Actividad_Valor] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Actividad_Valor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_ActividadEgreso]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_ActividadEgreso](
	[ID_Proyecto_Actividad_Egreso] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto_Actividad] [int] NULL,
	[Id_Item] [varchar](20) NULL,
	[Fecha] [datetime] NULL,
	[Tipo] [int] NULL,
	[Unidad] [varchar](50) NULL,
	[Cantidad] [int] NULL,
 CONSTRAINT [PK_MT_Proyecto_ActividadEgreso] PRIMARY KEY CLUSTERED 
(
	[ID_Proyecto_Actividad_Egreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Alerta]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Alerta](
	[Id_Proyecto_Alerta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Alerta] [datetime] NULL,
	[Mensaje] [varchar](max) NULL,
	[Repetir] [varchar](50) NULL,
	[Estado] [varchar](1) NULL,
	[Correo] [varchar](max) NULL,
 CONSTRAINT [PK_MT_Proyecto_Alerta] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Alerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Documento]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Documento](
	[Id_Proyecto_Documento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Tipo] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[Enlace] [varchar](max) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Fecha_Validez] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[NombreArchivo] [varchar](max) NULL,
 CONSTRAINT [PK_MT_Proyecto_Documento] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Documento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Parametro]    Script Date: 04/10/2020 22:26:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Parametro](
	[Id_Proyecto_Parametros] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Parametro] [varchar](60) NULL,
	[Tipo_Dato] [varchar](20) NULL,
	[Valor] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Moneda] [varchar](30) NULL,
 CONSTRAINT [PK_MT_Proyecto_Parametro] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Parametros] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Proyecto_Prorroga]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Proyecto_Prorroga](
	[Id_Proyecto_Prorroga] [int] IDENTITY(1,1) NOT NULL,
	[Id_Proyecto] [int] NULL,
	[Fecha_Prorroga] [datetime] NULL,
	[Estado] [varchar](1) NULL,
	[Descripcion] [varchar](max) NULL,
	[Dia_Prorroga] [int] NULL,
 CONSTRAINT [PK_MT_Proyecto_Prorroga] PRIMARY KEY CLUSTERED 
(
	[Id_Proyecto_Prorroga] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Sector]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Sector](
	[Id_Sector] [int] IDENTITY(1,1) NOT NULL,
	[Id_Padre_Sector] [int] NULL,
	[Nombre] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
	[Id_Empresa] [varchar](5) NULL,
 CONSTRAINT [PK_MT_Sector] PRIMARY KEY CLUSTERED 
(
	[Id_Sector] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Servicio]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Servicio](
	[Id_Servicio] [int] IDENTITY(1,1) NOT NULL,
	[Id_Empresa] [varchar](5) NULL,
	[Codigo] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[Estado] [varchar](10) NULL,
 CONSTRAINT [PK_MT_Servicio] PRIMARY KEY CLUSTERED 
(
	[Id_Servicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Solicitud]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Solicitud](
	[Id_Solicitud] [int] IDENTITY(1,1) NOT NULL,
	[Id_OrdenTrabajo] [int] NULL,
	[fecha] [datetime] NULL,
	[BodegaRetirar] [varchar](max) NULL,
	[retira] [varchar](max) NULL,
	[solicitante] [varchar](max) NULL,
	[tipo] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Solicitud] PRIMARY KEY CLUSTERED 
(
	[Id_Solicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_TablaDetalle]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_TablaDetalle](
	[Id_TablaDetalle] [int] IDENTITY(1,1) NOT NULL,
	[Id_Tabla] [int] NULL,
	[Codigo] [varchar](100) NULL,
	[Descripcion] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
	[Id_Padre] [int] NULL,
	[Orden] [int] NULL,
 CONSTRAINT [PK_TablaDetalle] PRIMARY KEY CLUSTERED 
(
	[Id_TablaDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Tablas]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Tablas](
	[Id_Tabla] [int] IDENTITY(1,1) NOT NULL,
	[Tabla] [varchar](max) NULL,
	[Descripcion] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Tablas] PRIMARY KEY CLUSTERED 
(
	[Id_Tabla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_Tipo_Trabajo_Medida]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_Tipo_Trabajo_Medida](
	[Id_Tipo_trabajo_Medida] [int] IDENTITY(1,1) NOT NULL,
	[Id_Tipo_Trabajo] [int] NULL,
	[Codigo] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Tipo_Dato] [varchar](max) NULL,
	[Olbligatorio] [varchar](1) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_MT_Tipo_Trabajo_Medida] PRIMARY KEY CLUSTERED 
(
	[Id_Tipo_trabajo_Medida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_TipoTrabajo]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_TipoTrabajo](
	[Id_TipoTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Cliente] [varchar](10) NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Codigo] [varchar](50) NULL,
	[Linea] [varchar](10) NULL,
	[Descripcion] [varchar](max) NULL,
	[Tipo] [int] NULL,
	[Estado] [varchar](1) NULL,
	[Fecha_Registro] [datetime] NULL,
	[Control_Item] [varchar](1) NULL,
	[Control_Equipo] [varchar](1) NULL,
	[Control_Integrante] [varchar](1) NULL,
	[Control_Anexo] [varchar](1) NULL,
	[Control_Medida] [varchar](1) NULL,
	[Control_Costo] [varchar](1) NULL,
	[Id_Servicio] [int] NULL,
	[Id_Padre] [int] NULL,
	[Control_Raiz] [varchar](1) NULL,
	[Proceso] [int] NULL,
	[Alerta] [int] NULL,
	[Caida] [int] NULL,
	[Clasificacion] [int] NULL,
 CONSTRAINT [PK_MT_TipoTrabajo] PRIMARY KEY CLUSTERED 
(
	[Id_TipoTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MT_TipoTrabajo_Item]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MT_TipoTrabajo_Item](
	[Id_TipoTrabajo_Item] [int] IDENTITY(1,1) NOT NULL,
	[Id_Tipo_Trabajo] [int] NOT NULL,
	[Id_Item] [int] NOT NULL,
	[Unidad] [float] NULL,
 CONSTRAINT [PK_MT_TipoTrabajo_Item] PRIMARY KEY CLUSTERED 
(
	[Id_TipoTrabajo_Item] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id_Rol] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](max) NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles_Usuario]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles_Usuario](
	[Id_Rol_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[Id_Usuario] [varchar](10) NULL,
	[Id_Rol] [int] NULL,
	[Estado] [varchar](1) NULL,
 CONSTRAINT [PK_Roles_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id_Rol_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_quimi_mant_contratos_proyectos]    Script Date: 04/10/2020 22:26:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_quimi_mant_contratos_proyectos](
	[empresa] [varchar](5) NOT NULL,
	[cliente] [varchar](5) NOT NULL,
	[unidad] [varchar](5) NOT NULL,
	[proyecto] [varchar](5) NOT NULL,
	[secuencial] [varchar](4) NOT NULL,
	[anio] [varchar](4) NOT NULL,
	[codigo_secuencial_interno] [varchar](20) NULL,
	[codigo_secuencial_interno_padre] [varchar](20) NULL,
	[codigo_contrato_asociado] [varchar](50) NULL,
	[fecha_inicial] [date] NULL,
	[plazo_contrato] [decimal](10, 0) NULL,
	[fecha_fin] [date] NULL,
	[monto] [decimal](14, 2) NULL,
	[responsable] [varchar](100) NULL,
	[costo] [decimal](14, 2) NULL,
	[detalle] [varchar](250) NULL,
	[codigo_secuencial_interno_migrado] [varchar](20) NULL,
	[unidad_migrada] [varchar](5) NULL,
	[cod_servicio] [varchar](5) NULL,
	[codcen] [varchar](13) NULL,
	[estado] [varchar](2) NULL,
	[prorroga_inicial] [date] NULL,
	[plazo_prorroga] [decimal](10, 0) NULL,
	[prorroga_final] [date] NULL,
	[observaciones] [varchar](2000) NULL,
	[cia_codigo] [varchar](2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[empresa] ASC,
	[cliente] ASC,
	[unidad] ASC,
	[proyecto] ASC,
	[secuencial] ASC,
	[anio] ASC,
	[cia_codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Menu] ON 

INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (1, N'Dashboard', 0, 10, 1, N'A', N'Default', N'Main', N'fa fa-bar-chart-o')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (2, N'Datos Personales', 26, 10, 2, N'A', N'DatosPersonales', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (3, N'Configuración', 0, 20, 1, N'A', NULL, NULL, N'fa fa-gears')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (4, N'Tipos de Trabajo', 3, 40, 2, N'A', N'MantTipoTrabajo', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (5, N'Precios Referenciales', 3, 50, 2, N'A', N'MantPrecioReferencial', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (6, N'Lugares Medición', 3, 60, 2, N'A', N'MantLugarMedicion', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (7, N'Grupo Trabajo', 3, 80, 2, N'A', N'MantGrupoTrabajo', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (8, N'Sector', 3, 20, 2, N'A', N'MantSector', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (9, N'Servicios', 3, 30, 2, N'A', N'MantServicios', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (10, N'Tablas Referenciales', 3, 10, 2, N'A', N'MantTabla', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (11, N'Permisos', 26, 20, 2, N'A', N'MantPermisos', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (12, N'Campamentos', 3, 70, 2, N'A', N'MantCampamento', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (13, N'Origen', 0, 30, 1, N'A', N'Contrato', N'Contrato', N'fa fa-book')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (14, N'Programa Trabajo', 0, 40, 1, N'A', N'ProgramaTrabajo', N'Mantenimiento', N'fa fa-calendar')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (15, N'Orden Trabajo', 0, 50, 1, N'A', N'OrdenTrabajo', N'OrdenTrabajo', N'fa fa-cubes')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (16, N'Orden Automatizacion', 0, 60, 1, N'A', N'OrdenTrabajo_Automatizacion', N'OrdenTrabajo_Automatizacion', N'fa fa-industry')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (17, N'Gestión de proyectos', 0, 70, 1, N'A', N'Proyectos', N'Proyectos', N'fa fa-briefcase ')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (18, N'Equipo Trabajo', 0, 80, 1, N'A', N'MantEquipoTrabajo', N'Mantenimiento', N'fa fa-gavel')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (19, N'Cotizacion', 0, 90, 1, N'A', N'Presupuesto', N'Presupuesto', N'fa fa-calculator')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (20, N'Planilla', 0, 100, 1, N'A', N'Planilla', N'Planilla', N'fa fa-file-powerpoint-o')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (21, N'Solicitud de Materiales', 0, 110, 1, N'A', N'Solicitud', N'Mantenimiento', N'fa fa-file-picture-o')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (22, N'Ingreso Materiales', 0, 120, 1, N'A', N'IngresoMateriales', N'Mantenimiento', N'glyphicon glyphicon-copy')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (23, N'Sincronizacion', 0, 130, 1, N'A', N'Sincronizacion', N'Sincronizacion', N'fa fa-refresh')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (24, N'Notificaciones', 0, 140, 1, N'A', N'Notificaciones', N'Notificaciones', N'fa fa-bell')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (25, N'Entrega de órdenes trabajo', 0, 150, 1, N'A', N'Entrega_OrdenTrabajo', N'OrdenTrabajo', N'glyphicon glyphicon-paste')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (26, N'Seguridad', 0, 10, 1, N'A', NULL, NULL, N'fa fa-gears')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (27, N'Asignar grupo a las OT', 0, 160, 1, N'A', N'AsignarGrupo_OrdenTrabajo', N'OrdenTrabajo', N'glyphicon glyphicon-paste')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (29, N'Menu Rol', 26, 30, 2, N'A', N'MantMenuDRol', N'Mantenimiento', N'fa fa-hand-o-right')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (30, N'Reportes', 0, 170, 1, N'A', N'reportecontrato', N'reporte', N'fa fa-files-o')
INSERT [dbo].[Menu] ([Id_Menu], [Menu], [Padre], [Orden], [Nivel_Profundidad], [Estado], [Action], [Controlador], [Icono]) VALUES (31, N'Prospectos', 0, 180, 1, N'A', N'Prospecto', N'Prospecto', N'fa fa-book')
SET IDENTITY_INSERT [dbo].[Menu] OFF
SET IDENTITY_INSERT [dbo].[MenuRol] ON 

INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1035, 13, 13, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1038, 14, 1, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1039, 14, 26, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1040, 14, 2, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1041, 14, 11, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1042, 14, 29, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1101, 1, 1, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1102, 1, 26, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1103, 1, 2, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1104, 1, 11, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1105, 1, 29, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1106, 1, 3, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1107, 1, 10, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1108, 1, 8, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1109, 1, 9, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1110, 1, 4, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1111, 1, 5, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1112, 1, 6, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1113, 1, 12, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1114, 1, 7, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1115, 1, 13, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1116, 1, 14, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1117, 1, 15, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1118, 1, 16, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1119, 1, 17, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1120, 1, 18, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1121, 1, 19, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1122, 1, 20, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1123, 1, 21, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1124, 1, 22, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1125, 1, 23, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1126, 1, 24, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1127, 1, 25, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1128, 1, 27, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1129, 1, 30, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1130, 1, 31, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1131, 12, 1, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1132, 12, 26, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1133, 12, 2, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1134, 12, 11, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1135, 12, 29, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1136, 12, 3, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1137, 12, 10, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1138, 12, 8, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1139, 12, 9, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1140, 12, 4, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1141, 12, 5, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1142, 12, 6, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1143, 12, 12, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1144, 12, 7, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1145, 12, 13, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1146, 12, 14, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1147, 12, 15, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1148, 12, 16, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1149, 12, 17, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1150, 12, 18, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1151, 12, 19, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1152, 12, 20, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1153, 12, 21, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1154, 12, 22, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1155, 12, 23, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1156, 12, 24, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1157, 12, 25, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1158, 12, 27, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1159, 12, 30, N'A')
INSERT [dbo].[MenuRol] ([Id_MenuRol], [Id_Rol], [Id_Menu], [Estado]) VALUES (1160, 12, 31, N'A')
SET IDENTITY_INSERT [dbo].[MenuRol] OFF
SET IDENTITY_INSERT [dbo].[MT_Actividad] ON 

INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (1, 5, 0, N'01', N'Actividad 1', CAST(N'01:00:00' AS Time), 0, 1, N'N', N'A', 133, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (2, 6, 0, N'1', N'Revisar herramientas y EPP necesarias para realizar las tareas', CAST(N'00:15:00' AS Time), 1, 1, N'S', N'A', 133, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (3, 6, 0, N'2', N'Realizar revisión/mantenimiento del equipo incluido terminales, conectores y conexión', CAST(N'00:15:00' AS Time), NULL, 2, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (4, 6, 0, N'3', N'Tomar lectura del Voltaje de entrada', CAST(N'00:15:00' AS Time), NULL, 3, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (5, 6, 0, N'4', N'Tomar lectura de la Temperatura del Sensor de
Nivel', CAST(N'00:15:00' AS Time), 0, 4, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (6, 6, 0, N'5', N'Realizar medición física del nivel. (Adjuntar
Registro Fotográfico) y registrar de medición
física del nivel', CAST(N'00:15:00' AS Time), 0, 5, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (7, 6, 0, N'6', N'Corroborar que coincida el dato fisico, con el
dato del equipo y con el dato del Scada', CAST(N'00:30:00' AS Time), 0, 6, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (8, 6, 0, N'8', N'Verificar el correcto funcionamiento del equipo
con SCO y registrar observaciones respectivas:', CAST(N'00:15:00' AS Time), 0, 8, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (9, 7, 0, N'1', N'PLANIFICACION', CAST(N'07:00:00' AS Time), NULL, 1, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (10, 7, 0, N'2', N'PREPARACION DE MATERIALES', CAST(N'03:00:00' AS Time), 0, 2, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (11, 7, 0, N'3', N'INSPECCION', CAST(N'05:45:00' AS Time), 0, 3, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (12, 7, 0, N'4', N'INSTALACION', CAST(N'05:30:00' AS Time), 0, 4, N'N', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (13, 7, 0, N'5', N'PREPARACION INFORME', CAST(N'02:45:00' AS Time), NULL, 5, N'S', N'A', 133, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (14, 1, 0, N'1', N'INSPECCION', CAST(N'00:30:00' AS Time), 5, 1, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (15, 15, 0, N'OM-GY-MCR-M52-INV-000', N'INVERSOR M52 DOMINGO COMIN ', CAST(N'00:45:00' AS Time), 10, 1, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (16, 15, 0, N'OM-GY-MCR-M52-PTR-000 ', N'PUESTA A TIERRA M52 DOMINGO COMIN', CAST(N'00:30:00' AS Time), 15, 2, N'S', N'A', 133, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (17, 15, 0, N'OM-GY-MCR-M52-SII-000 ', N'SISTEMA DE ILUMINACION INTERNA M52 DOMINGO COMIN ', CAST(N'01:00:00' AS Time), 20, 4, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (18, 8, 0, N'INV-000', N'INVERSOR M52 DOMINGO COMIN', CAST(N'00:30:00' AS Time), 25, 1, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (19, 8, 0, N'PTR-000', N'PUESTA A TIERRA M52 DOMINGO COMIN', CAST(N'00:45:00' AS Time), 15, 2, N'S', N'A', 134, NULL)
INSERT [dbo].[MT_Actividad] ([Id_Actividad], [Id_TipoTrabajo], [Id_ActividadPadre], [Codigo], [Descripcion], [Tiempo_Estimado], [Peso_Actividad], [Orden], [Obligatorio], [Estado], [Tipo], [Id_Empresa]) VALUES (20, 8, 0, N'SII-000', N'SISTEMA DE ILUMINACION INTERNA M52 DOMINGO COMIN', CAST(N'01:00:00' AS Time), 45, 3, N'S', N'A', 134, NULL)
SET IDENTITY_INSERT [dbo].[MT_Actividad] OFF
SET IDENTITY_INSERT [dbo].[MT_Campamento] ON 

INSERT [dbo].[MT_Campamento] ([Id_Campamento], [Id_Empresa], [Nombre], [Estado]) VALUES (1, N'02', N'Campamento Sur', N'A')
SET IDENTITY_INSERT [dbo].[MT_Campamento] OFF
SET IDENTITY_INSERT [dbo].[MT_Contrato] ON 

INSERT [dbo].[MT_Contrato] ([Id_Contrato], [Id_Empresa], [Id_Cliente], [Fecha_Inicio], [Fecha_Fin], [Codigo_Interno], [Codigo_Cliente], [Id_Usuario], [Id_Linea], [Categoria], [Subcategoria], [Nombre], [Estado], [Dia_Plazo], [tipo], [Contrato_Padre], [Valor_Referencial], [monto], [costo], [Responsable], [Secuencial], [Codigo_Interno_Ant], [Observaciones], [Codigo_interno_padre], [Fecha_registro], [Fecha_modificacion], [Localidad], [Fecha_Aprobacion_Cot], [Recepcion_Servicio], [Fecha_Firma_Conformidad], [Fecha_Cumplimiento_Inst], [Referencia]) VALUES (1, N'02', N'1', CAST(N'2020-09-30T00:00:00.000' AS DateTime), CAST(N'2020-10-01T00:00:00.000' AS DateTime), N'HYDIWSADMCTR0012020', N'ctr1', N'BSALTO', N'ADM', N'01', N'004', N'Prueba', 75, 1, 144, 0, CAST(12.0000 AS Numeric(18, 4)), CAST(1000.0000 AS Numeric(18, 4)), CAST(2000.0000 AS Numeric(18, 4)), 258, 1, N'HYDIWSADMCTR0012020', N'prueba ctrorigenmmmmmmmm', NULL, CAST(N'2020-09-30T22:49:00.847' AS DateTime), CAST(N'2020-09-30T23:12:13.613' AS DateTime), N'01', NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[MT_Contrato] OFF
SET IDENTITY_INSERT [dbo].[MT_Contrato_Documentado] ON 

INSERT [dbo].[MT_Contrato_Documentado] ([Id_Contrato_Documentado], [Id_Contrato], [Descripcion], [Enlace], [Fecha_Registro], [Fecha_Validez], [Estado], [NombreArchivo], [Tipo], [Version]) VALUES (1, 1, N'prueba 1', N'C:\Program Files (x86)\IIS Express\~\Formularios_Actividad\', CAST(N'2020-09-30T23:02:01.347' AS DateTime), CAST(N'2020-10-01T00:00:00.000' AS DateTime), N'A', N'20200930_1.pdf', 88, CAST(1.00 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[MT_Contrato_Documentado] OFF
SET IDENTITY_INSERT [dbo].[MT_Contrato_Prorroga] ON 

INSERT [dbo].[MT_Contrato_Prorroga] ([Id_Contrato_Prorroga], [Id_Contrato], [Fecha_Prorroga], [Estado], [Descripcion], [Dia_Prorroga]) VALUES (1, 1, CAST(N'2020-10-01T00:00:00.000' AS DateTime), N'A', N'Prorroga Inicial', 1)
INSERT [dbo].[MT_Contrato_Prorroga] ([Id_Contrato_Prorroga], [Id_Contrato], [Fecha_Prorroga], [Estado], [Descripcion], [Dia_Prorroga]) VALUES (2, 1, CAST(N'2020-10-01T00:00:00.000' AS DateTime), N'E', N'Prorroga', 1)
INSERT [dbo].[MT_Contrato_Prorroga] ([Id_Contrato_Prorroga], [Id_Contrato], [Fecha_Prorroga], [Estado], [Descripcion], [Dia_Prorroga]) VALUES (3, 1, CAST(N'2020-10-01T00:00:00.000' AS DateTime), N'E', N'Prorroga', 1)
SET IDENTITY_INSERT [dbo].[MT_Contrato_Prorroga] OFF
SET IDENTITY_INSERT [dbo].[MT_Estacion] ON 

INSERT [dbo].[MT_Estacion] ([Id_Estacion], [Id_Cliente], [Id_Provincia], [Id_Ciudad], [Id_Macrosector], [Nombre], [Direccion], [Estado], [Coordenada_X], [Coordenada_Y]) VALUES (1, N'1', 3, 3, 1, N'Prueba para estacion', N'Gye', N'A', N'1.25', N'1.25')
SET IDENTITY_INSERT [dbo].[MT_Estacion] OFF
SET IDENTITY_INSERT [dbo].[MT_Fiscalizador] ON 

INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (1, 392, N'Ing. Jorge Velez', N'A', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (2, 393, N'Fiscalizador 1', N'A', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (3, 402, N'Fiscalizador Prueba', N'E', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (4, 402, N'Fiscalizador Prueba', N'E', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (5, 402, N'Fiscalizador Prueba 3', N'A', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (6, 412, N'ANGEL ZAMBRANO', N'A', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (7, 1, N'PruebaF', N'A', N'Contrato')
INSERT [dbo].[MT_Fiscalizador] ([Id_Fiscalizador], [Id_Proyecto_Contrato], [Nombre], [Estado], [Tipo]) VALUES (8, 1, N'pruebaF2Pry', N'A', N'Proyecto')
SET IDENTITY_INSERT [dbo].[MT_Fiscalizador] OFF
SET IDENTITY_INSERT [dbo].[MT_Notificacion_Persona] ON 

INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (7, 2, 781, 90, N'vanessitaq93@hotmail.com', NULL, 92)
INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (8, 2, 781, 90, N'sandro.ontime@gmail.com', NULL, 92)
INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (9, 3, 781, 90, N'vanessitaq93@hotmail.com', NULL, 92)
INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (10, 3, 781, 90, N'sandro.ontime@gmail.com', NULL, 92)
INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (11, 4, 781, 90, N'vanessitaq93@hotmail.com', NULL, 92)
INSERT [dbo].[MT_Notificacion_Persona] ([Id_Notificacion_Persona], [Id_Notificacion], [Id_Persona], [Tipo], [Correo], [Fecha_Hora], [Estado]) VALUES (12, 4, 781, 90, N'sandro.ontime@gmail.com', NULL, 92)
SET IDENTITY_INSERT [dbo].[MT_Notificacion_Persona] OFF
SET IDENTITY_INSERT [dbo].[MT_Notificaciones] ON 

INSERT [dbo].[MT_Notificaciones] ([Id_Notificacion], [Tipo_Notificacion], [Id_Codigo_origen], [Id_usuario], [Fecha], [Prioridad], [Asunto], [Mensaje], [Criterio], [Frecuencia], [Enviado], [Fecha_Envio], [Reenvio], [EmpresaID], [Estado]) VALUES (2, 82, 0, N'BSALTO', CAST(N'2020-10-02T22:59:19.153' AS DateTime), 83, N'prueba notificaciones', N'amooooorrr notificaciones', 95, 0, 1, CAST(N'2020-10-02T22:59:21.510' AS DateTime), 1, N'02', NULL)
INSERT [dbo].[MT_Notificaciones] ([Id_Notificacion], [Tipo_Notificacion], [Id_Codigo_origen], [Id_usuario], [Fecha], [Prioridad], [Asunto], [Mensaje], [Criterio], [Frecuencia], [Enviado], [Fecha_Envio], [Reenvio], [EmpresaID], [Estado]) VALUES (3, 119, 1, N'BSALTO', CAST(N'2020-10-02T23:08:31.140' AS DateTime), 83, N'fghjlkjh', N'ttttttttttttttttttttttuuuuuuuuuuuuuuuuuuuuuu', 95, 1161, 1, CAST(N'2020-10-02T23:11:27.340' AS DateTime), 1, N'02', NULL)
INSERT [dbo].[MT_Notificaciones] ([Id_Notificacion], [Tipo_Notificacion], [Id_Codigo_origen], [Id_usuario], [Fecha], [Prioridad], [Asunto], [Mensaje], [Criterio], [Frecuencia], [Enviado], [Fecha_Envio], [Reenvio], [EmpresaID], [Estado]) VALUES (4, 1166, 1, N'BSALTO', CAST(N'2020-10-02T23:31:11.700' AS DateTime), 83, N'prueba prospecto', N'mi amorfeoooooooo tu ', 95, 1161, 1, CAST(N'2020-10-02T23:31:15.467' AS DateTime), 1, N'02', NULL)
SET IDENTITY_INSERT [dbo].[MT_Notificaciones] OFF
SET IDENTITY_INSERT [dbo].[MT_Permiso] ON 

INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (10, N'BSALTO', N'1163', NULL, NULL, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.390' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (11, N'BSALTO', N'1192', NULL, NULL, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.450' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (12, N'BSALTO', N'102', NULL, NULL, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.477' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (13, N'BSALTO', N'1', NULL, NULL, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.517' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (14, N'BSALTO', N'1', 1, NULL, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.553' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (15, N'BSALTO', N'1', NULL, 1, NULL, NULL, N'S', N'S', N'S', N'S', N'N', N'BSALTO', CAST(N'2020-09-30T22:57:48.593' AS DateTime), N'A', N'02', NULL)
INSERT [dbo].[MT_Permiso] ([Id_Permiso], [Id_Usuario], [Id_Cliente], [Id_Contrato], [Id_Proyecto], [Id_Tipo_Trabajo], [Id_Linea], [Consultar], [Modificar], [Crear], [Eliminar], [Aprobar], [Usuario], [Fecha_Registro], [Estado], [Id_Empresa], [Id_Prospecto]) VALUES (16, N'BSALTO', N'102', NULL, NULL, NULL, NULL, N'S', N'S', N'S', N'N', N'S', N'BSALTO', CAST(N'2020-10-01T23:09:32.640' AS DateTime), N'A', N'02', 1)
SET IDENTITY_INSERT [dbo].[MT_Permiso] OFF
SET IDENTITY_INSERT [dbo].[MT_Prospecto] ON 

INSERT [dbo].[MT_Prospecto] ([Id_Prospecto], [Id_Empresa], [Id_Cliente], [Id_Usuario], [Id_Linea], [Categoria], [Subcategoria], [tipo], [Responsable], [Localidad], [Fecha_Inicio], [Fecha_Fin], [Codigo_Interno], [Codigo_Cliente], [Nombre], [Estado], [Dia_Plazo], [Valor_Referencial], [monto], [costo], [Secuencial], [Codigo_Interno_Ant], [Observaciones], [Codigo_interno_padre], [Fecha_registro], [Fecha_modificacion], [Fecha_Aprobacion_Cot], [Recepcion_Servicio], [Fecha_Firma_Conformidad], [Fecha_Cumplimiento_Inst]) VALUES (1, N'02', N'102', N'BSALTO', N'ADM', N'01', N'004', 146, 258, N'01', CAST(N'2020-10-01T00:00:00.000' AS DateTime), CAST(N'2020-10-07T00:00:00.000' AS DateTime), N'HYDETIADMPRO0012020', NULL, N'Prueba Prospecto', 1158, NULL, CAST(12121212.0000 AS Numeric(18, 4)), CAST(1212.0000 AS Numeric(18, 4)), CAST(121212.0000 AS Numeric(18, 4)), 1, N'HYDETIADMPRO0012020', N'prueba de prosp', NULL, CAST(N'2020-10-01T23:09:32.337' AS DateTime), CAST(N'2020-10-02T16:24:18.723' AS DateTime), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[MT_Prospecto] OFF
SET IDENTITY_INSERT [dbo].[MT_Prospecto_Documentado] ON 

INSERT [dbo].[MT_Prospecto_Documentado] ([Id_Prospecto_Documentado], [Id_Prospecto], [Descripcion], [Enlace], [Fecha_Registro], [Fecha_Validez], [Estado], [NombreArchivo], [Tipo]) VALUES (1, 1, N'Prueba Form Prospecto', N'C:\Program Files (x86)\IIS Express\~\Formularios_Actividad\', CAST(N'2020-10-02T16:28:41.600' AS DateTime), CAST(N'2020-10-07T00:00:00.000' AS DateTime), N'A', N'20201002_1.pdf', 1154)
SET IDENTITY_INSERT [dbo].[MT_Prospecto_Documentado] OFF
SET IDENTITY_INSERT [dbo].[MT_Proyecto] ON 

INSERT [dbo].[MT_Proyecto] ([Id_Proyecto], [Id_Empresa], [Id_Sucursal], [Id_Cliente], [Id_TipoTrabajo], [Id_Presupuesto], [Fecha_Registro], [Fecha_Inicio], [Fecha_Fin], [Fecha_Prorroga], [Estado], [Codigo_Interno], [Codigo_Cliente], [Direccion], [Ubicacion], [Comentario], [Porcentaje_avance], [Valor_Inicial], [Valor_Ajustado], [Linea], [Categoria], [Subcategoria], [Fecha_Anticipo], [Id_contrato], [Id_Prospecto]) VALUES (1, N'02', N'GAD', N'1', 0, NULL, CAST(N'2020-09-30T22:53:03.017' AS DateTime), NULL, NULL, NULL, N'A', N'pry1', N'pry1', N'Gye', N'1.2', N'prueba pry1', N'0', CAST(1.0000 AS Numeric(18, 4)), CAST(2000.0000 AS Numeric(18, 4)), N'ADM', N'01', N'004', NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[MT_Proyecto] OFF
SET IDENTITY_INSERT [dbo].[MT_Proyecto_Parametro] ON 

INSERT [dbo].[MT_Proyecto_Parametro] ([Id_Proyecto_Parametros], [Id_Proyecto], [Parametro], [Tipo_Dato], [Valor], [Estado], [Moneda]) VALUES (1, 1, N'Parametro 1', N'varchar', 1, N'A', N'DO')
SET IDENTITY_INSERT [dbo].[MT_Proyecto_Parametro] OFF
SET IDENTITY_INSERT [dbo].[MT_Sector] ON 

INSERT [dbo].[MT_Sector] ([Id_Sector], [Id_Padre_Sector], [Nombre], [Estado], [Id_Empresa]) VALUES (1, 0, N'Prueba', N'A', N'02')
SET IDENTITY_INSERT [dbo].[MT_Sector] OFF
SET IDENTITY_INSERT [dbo].[MT_Servicio] ON 

INSERT [dbo].[MT_Servicio] ([Id_Servicio], [Id_Empresa], [Codigo], [Descripcion], [Estado]) VALUES (1, N'02', 116, N'Servicio de Mantenimiento de Redes AAPP Y AASS', N'A')
INSERT [dbo].[MT_Servicio] ([Id_Servicio], [Id_Empresa], [Codigo], [Descripcion], [Estado]) VALUES (2, N'02', 3, N'Servicio Automatizacion Eléctrico, Electrónico, SCADA, Mecánico', N'A')
INSERT [dbo].[MT_Servicio] ([Id_Servicio], [Id_Empresa], [Codigo], [Descripcion], [Estado]) VALUES (3, N'02', 3, N'SERVICIOS DE MANTENIMIENTO DE AUTOMATIZACION Y TELEMETRIA ', N'E')
INSERT [dbo].[MT_Servicio] ([Id_Servicio], [Id_Empresa], [Codigo], [Descripcion], [Estado]) VALUES (4, N'02', 4, N'Servicio de Mantenimiento Eléctrico, Electrónico, SCADA, Mecánico', N'A')
INSERT [dbo].[MT_Servicio] ([Id_Servicio], [Id_Empresa], [Codigo], [Descripcion], [Estado]) VALUES (5, N'02', 5, N'SERVICIOS DE AUTOMATIZACION Y TELEMETRIA ', N'A')
SET IDENTITY_INSERT [dbo].[MT_Servicio] OFF
SET IDENTITY_INSERT [dbo].[MT_TablaDetalle] ON 

INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1, 1, N'PROY', N'PROYECTO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (2, 1, N'CONT', N'CONTRATO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (3, 2, N'GUAYAS', N'GUAYAS', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (4, 2, N'PICHINCHA', N'PICHINCHA', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (5, 2, N'AZUAY', N'AZUAY', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (10, 3, N'GUAYAQUIL', N'GUAYAQUIL', N'A', 3, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (11, 3, N'QUITO', N'QUITO', N'A', 4, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (12, 3, N'CUENCA', N'CUENCA', N'A', 5, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (15, 6, N'Macrosector1', N'Macrosector1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (16, 6, N'Macrosector2', N'Macrosector2', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (17, 7, N'Reposc', N'Reposición', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (18, 7, N'Autm', N'Automatizacion', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (19, 7, N'Limpz', N'Limpieza', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (20, 7, N'Medic', N'Medicion', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (21, 7, N'Dosif', N'Dosificación', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (22, 7, N'Sol téc', N'Solucion técnica', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (24, 8, N'Proc', N'Enproceso', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (25, 8, N'Env Client', N'Enviada al cliente', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (26, 8, N'Apr Cliente', N'Aprobada por cliente', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (27, 8, N'Apr', N'Pagada', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (30, 9, N'Reg', N'Registrado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (31, 9, N'Proc', N'Proceso', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (32, 9, N'Desest', N'Desestimado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (33, 9, N'Bloq', N'Bloqueado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (34, 9, N'Fin', N'Finalizado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (35, 10, N'Jefe', N'Jefe', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (36, 10, N'Int.', N'Integrante', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (37, 11, N'Muy Alta', N'Muy Alta', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (38, 11, N'Alta', N'Alta', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (39, 11, N'Media', N'Media', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (40, 12, N'MANTENIMIENTO TIPO TRABAJO ', N'En esta pagina se admistrara  Noti_MantTipoTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (41, 12, N'MANTENIMIENTO  PRECIO  REFERENCIAL', N'En esta pagina se administrara Noti_MantPreReferen', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (42, 12, N'MANTENIMIENTO LUGAR MEDICION', N'En esta pagina se administrara : Noti_MantLuMedi', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (43, 12, N'MANTENIMIENTO GRUPO TRABAJO ', N'En esta pagina se administrara: Noti_MantGrupTraba', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (44, 12, N'ITEMS MANTENIMIENTO TIPO TRABAJO', N'En esta pagina se administrara: Noti_ItemMantTiTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (45, 12, N'INTEGRANTES MANTENIMIENTO GRUPO TRABAJO', N'En esta pagina se administrara:Noti_IntegraMantGrupTrab ', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (46, 12, N'FORMULARIOS ACTIVIDAD ', N'En esta pagina se administrara: Noti_FormActivi', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (47, 12, N'EQUIPOS MANTENIMIENTO GRUPO DE TRABAJO ', N'En esta pagina se administrara: Noti_EquiMantgrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (48, 12, N'EDITAR MANTENIMIENTO GRUPO DE TRABAJO', N'En esta pagina se administrara: Noti_EdiMantTipTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (49, 12, N'EDITAR MANTENIMIENTO PRECIO REFERENCIAL', N'En esta pagina se administrara:Noti_EdiMantPreReferen', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (50, 12, N'EDITAR MANTENIMIENTO LUGAR MEDICION ', N'En esta pagina se administrara: Noti_EdiMantLuMedi', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (51, 12, N'EDITAR MANTENIMIENTO GRUPO DE TRABAJO', N'En esta pagina se administrara: Noti_EdiMantGrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (52, 12, N'EDITAR ITEMS MANTENIMIENTO TIPO DE TRABAJO', N'En esta pagina se administrara: Noti_Edi_ItemMantTipoTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (53, 12, N'EDITAR INTEGRANTE MANTENIMIENTO GRUPO DE TRABAJO ', N'En esta pagina se administrara: Noti_Edi_IntegrMantGrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (54, 12, N'EDITAR EQUIPO MANTENIMIENTO DE TRABAJO', N'En esta pagina se administrara: Noti_Edi_EquiMantGrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (55, 12, N'EDITAR ACTIVIDADES MANTENIMIENTO TIPO DE TRABAJO ', N'En esta pagina se administrara: Noti_Edi_ActMantTipTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (56, 12, N'AGREGAR  FORMULARIO DE ACTIVIDAD', N'En esta pagina se administrara: Noti_Agg_FormActiv', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (57, 12, N'AGREGAR MANTENIMINETO TIPO TRABAJO ', N'En esta pagina se administrara: Noti_Agg_MantTipoTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (58, 12, N'AGREGAR MANTENIMIENTO  PRECIO REFERENCIAL', N'En esta pagina se administrara: Noti_Agg_MantPreRefer', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (59, 12, N'AGREGAR MANTENIMINETO LUGAR MEDICION ', N'En esta pagina se administrara: Noti_Agg_MantLugMedi', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (60, 12, N'AGREGAR MANTENIMINETO GRUPO DE TRABAJO', N'En esta pagina se administrara: Noti_Agg_MantGrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (61, 12, N'AGREGAR ITEM MANTENIMIENTO TIPO DE TRABAJO ', N'En esta pagina se administrara: Noti_Agg_ItemMantTipTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (62, 12, N'AGREGAR INTEGRANTES MANTENIMIENTO GRUPO DE TRABAJO', N'En esta pagina se administrara: Noti_Agg_IntegrMantGrupTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (63, 12, N'AGREGAR EQUIPO MANTENIMIENTO GRUPO DE TRABAJO', N'En esta pagina se administrara: Noti_Agg_EquiMantGrupTraba', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (64, 12, N'AGREGAR ACTIVIDADES MANTENIMIENTO TIPO DE TRABAJO', N'En esta pagina se administrara: Noti_Agg_ActiMantTipTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (65, 12, N'ACTIVIDADES MANTENIMIENTO TIPO DE TRABAJO ', N'En esta pagina se administrara: Noti_Acti_MantTipTrab', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (66, 12, N'TITULO', N'BIENVENIDO AL SISTEMA FRONTER ', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (67, 39, N'No Iniciado', N'No Iniciado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (68, 39, N'En Proceso', N'En Proceso', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (69, 39, N'Finalizado', N'Finalizado', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (70, 39, N'No Fue Necesario', N'No Fue Necesario', N'A', 69, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (71, 39, N'No Realizado', N'No Realizado', N'A', 69, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (72, 40, N'Equipo', N'Equipo', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (73, 40, N'Herramienta', N'Herramienta', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (74, 12, N'titulo', N'hola, bienvenido', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (75, 41, N'01', N'En Ejecución', N'A', 0, 9)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (76, 41, N'02', N'Adjudicado', N'A', 0, 5)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (77, 41, N'03', N'Desertado', N'A', 0, 6)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (78, 42, N'Propio', N'Propio', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (79, 42, N'Cliente', N'Cliente', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (80, 43, N'Orden De trabajo', N'Orden De Trabajo', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (81, 43, N'Proyecto', N'Proyecto', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (82, 43, N'General', N'General', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (83, 44, N'Alta', N'Alta', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (84, 44, N'Media', N'Media', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (85, 44, N'Baja', N'Baja', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (88, 45, N'Prorroga', N'Prorroga', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (89, 45, N'Orden de Compra', N'Orden de Compra', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (90, 47, N'Dirigido a', N'Dirigido a', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (91, 47, N'Copiado a ', N'Copiado a ', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (92, 48, N'No Leido', N'No Leido', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (93, 48, N'Leido', N'Leido', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (94, 49, N'Entrada', N'Entrada', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (95, 49, N'Salida', N'Salida', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (96, 50, N'Reposicion Stock', N'Reposicion Stock', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (97, 41, N'04', N'No adjudicado', N'A', 0, 7)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (99, 51, N'Documento', N'Documento', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (100, 51, N'Foto', N'Foto', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (101, 52, N'SMTP_IDCompania', N'01;02', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (102, 52, N'SMTP_NombreCompania', N'QUIMIPAC 2011;Hydriapac', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (103, 52, N'SMTP_Servidor', N'smtp.gmail.com', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (104, 52, N'SMTP_Mail', N'QuimipacApp@gmail.com', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (105, 52, N'SMTP_Puerto', N'587', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (106, 52, N'SMTP_EnabledSSL', N'1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (108, 52, N'SMTP_Usuario', N'QuimipacApp@gmail.com', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (109, 52, N'SMTP_Clave', N'Quimipac2018', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (111, 52, N'SMTP_Cuerpo', N'<br/>
<center>
<div>
<div style="font-family: Roboto-Regular, Helvetica, Arial, sans-serif; padding: 40px 20px 38px; color: rgba(0, 0, 0, 0.87); ">

<table style="text-align: center; min-width: 348px;" width="50%" cellspacing="0" cellpadding="0" border="0">
  <tbody>
    <tr>
	   <td>
			<div style="font-family: Roboto-Regular, Helvetica, Arial, sans-serif; padding: 40px 20px 38px; border-bottom: thin solid #f0f0f0; color: rgba(0, 0, 0, 0.87); 			text-align: center; word-break: break-word;">
			<img src="~/Content/bower_components/ImagenVQ/FTIsotipo.png" class="CToWUd" width="92" height="32" alt="" style="font-size: 10.6667px; font-family: Arial; width: 92px; height: 32px;" />
			</div>
		</td>
	</tr>
	<tr>
	  <td>
		 <span style="font-family: Arial; font-size: 12px; text-align: center;">Estimad@: {Usuario}</span>
		 <div style="text-align: left;"><span style="text-align: center; font-size: 12px;">Se a recibido notificacion de:&nbsp;{Correo_Emisor}</span></div>
				<span style="font-size: 12px;">{Asunto}</span>	
				
		<img src="ggg" class="CToWUd" alt="" style="margin: 0px 7px 0px 0px; border-style: none;" />{Cuerpo}<br>
		<div style="text-align: center;"><span style="font-size: 8pt; color: #c0c0c0;">Te hemos enviado este correo electrónico para informarte de notificaciones importantes en tu cuenta.</span>
		</div><div style="text-align: center; direction: ltr;">
			<span style="font-size: 8pt; color: #c0c0c0;">© {Anio} {Empresa}</span></div>
	 </td>
	</tr>
  </tbody>
</table>
</div>
</div>
<center>', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (112, 52, N'SMTP_UsuarioCreacion', N'Admin', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (113, 52, N'SMTP_FechaCreacion', N'2018-10-30 14:10:10', N'A', 0, NULL)
GO
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (114, 52, N'SMTP_UsuarioModificacion', N'', N'I', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (115, 52, N'SMTP_FechaModificacion', N'', N'I', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (116, 52, N'SMTP_EsHtml', N'1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (117, 52, N'SMTP_Estado', N'1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (118, 52, N'SMTP_Criterio', N'Envio ', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (119, 43, N'Contrato', N'Contrato', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (120, 39, N'Concluido', N'Realizado- Concluido', N'A', 69, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (121, 54, N'Mantenimiento', N'Mantenimiento', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (122, 54, N'Uso', N'Uso', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (123, 54, N'Daño', N'Daño', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (124, 55, N'Bodega', N'Bodega', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (125, 55, N'Proveedor', N'Proveedor', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (126, 55, N'Compra en campo', N'Compra en campo', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (127, 56, N'CHEQUERA', N'CHEQUERA', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (128, 56, N'FACTURA', N'FACTURA', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (129, 56, N'APU', N'APU', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (130, 57, N'RESIDENCIALES', N'RESIDENCIALES', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (131, 57, N'COMERCIALES', N'COMERCIALES', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (132, 57, N'CORPORATIVOS', N'CORPORATIVOS', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (133, 58, N'Oficina', N'Oficina', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (134, 58, N'Campo', N'Campo', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (135, 60, N'TIPO 1', N'TIPO 1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (136, 60, N'TIPO 2', N'TIPO 2', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (137, 60, N'NINGUNO', N'NINGUNO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (138, 61, N'OPCIONH 1', N'OPCIONH 1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (139, 61, N'OPCIONH 2', N'OPCIONH 2', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (140, 61, N'NINGUNO', N'NINGUNO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (141, 62, N'OPCIONE 1', N'OPCIONE 1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (142, 62, N'OPCIONE 2', N'OPCIONE 2', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (143, 62, N'NINGUNO', N'NINGUNO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (144, 63, N'CTR', N'CONTRATO', N'A', 0, 3)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (145, 63, N'LIC', N'LICITACION', N'A', 0, 2)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (146, 63, N'PRO', N'PROSPECTO', N'A', 0, 1)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (147, 64, N'ENTERO', N'ENTERO', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (148, 64, N'DECIMAL', N'DECIMAL', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (149, 64, N'VARCHAR', N'VARCHAR', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (150, 64, N'DATETIME', N'DATETIME', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (151, 41, N'05', N'Finalizado', N'A', 0, 10)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1144, 9, N'Reit', N'Reiterada', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1147, 1064, N'ST', N'Solución Técnica', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1148, 1064, N'RP', N'Reposición', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1149, 1065, N'Operador 1', N'Operador 1', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1150, 1065, N'Operador 2', N'Operador 2', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1151, 41, N'06', N'Por confirmar', N'A', 0, 8)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1152, 63, N'PRY', N'PROYECTO CON ORDEN DE SERVICIO', N'A', 0, 4)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1153, 45, N'Contrato', N'Contrato', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1154, 45, N'Varios', N'Varios', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1156, 41, N'07', N'Propuesta Elaborada', N'A', 0, 3)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1157, 41, N'08', N'Propuesta Entregada', N'A', 0, 4)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1158, 41, N'09', N'Pendiente', N'A', 0, 1)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1159, 41, N'10', N'Preparacion de la Propuesta', N'A', 0, 2)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1160, 41, N'11', N'Eliminado', N'A', 0, 11)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1161, 1066, N'Ninguna', N'0', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1162, 1066, N'Diaria', N'1 dia', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1163, 1066, N'Semanal', N'7 dias', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1164, 1066, N'Mensual', N'30 dias', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1165, 63, N'MEJ', N'PROYECTO DE MEJORA', N'A', 0, 5)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1166, 43, N'Prospecto', N'Prospecto', N'A', 0, NULL)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1167, 63, N'OP NEG', N'OPORTUNIDAD DE NEGOCIO', N'A', 0, 6)
INSERT [dbo].[MT_TablaDetalle] ([Id_TablaDetalle], [Id_Tabla], [Codigo], [Descripcion], [Estado], [Id_Padre], [Orden]) VALUES (1168, 63, N'POST-VENTA', N'POST-VENTA', N'A', 0, 7)
SET IDENTITY_INSERT [dbo].[MT_TablaDetalle] OFF
SET IDENTITY_INSERT [dbo].[MT_Tablas] ON 

INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (1, N'TIPO_TRABAJO', N'Tipos de trabajo', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (2, N'PROVINCIA', N'Provincia', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (3, N'CIUDAD', N'Ciudad', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (6, N'MACRO_SECTOR', N'Macrosectores', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (7, N'TIPO_GRUPO_TRABAJO', N'Tipos de Grupo de Trabajo', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (8, N'PLANILLAS', N'Planillas', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (9, N'ESTADO_ORDEN', N'Estado Orden', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (10, N'ROL', N'Rol', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (11, N'NIVEL_PRIORIDAD', N'Nivel Prioridad', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (12, N'TITULO', N'Titulos de página', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (39, N'ESTADO_ORDEN_TRABAJO_ACTIVIDAD', N'Estado Orden Trabajo Actividad', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (40, N'TIPOS_EQUIPOS', N'Tipos Equipos', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (41, N'ESTADO', N'Estado del Contrato', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (42, N'TIPO_OT_EGRESO', N'Tipo OT Egreso', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (43, N'TIPO_NOTIFICACION', N'Tipo_Notificacion', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (44, N'PRIORIDAD', N'Prioridad', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (45, N'TIPOS', N'Tipos', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (47, N'NOTI_PERSONA_TIPO', N'Noti_PersonaTipo', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (48, N'ESTADO_PERSONA', N'No Leido/Leido', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (49, N'CRITERIO_NOTIFICACION', N'Criterio Notificacion', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (50, N'TIPO_SOLICITUD', N'Tipo_Solicitud', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (51, N'TIPO_PROYECTO', N'Tipo_Proyecto', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (52, N'SMTP_PARAMETROS', N'Parámetros SMTP', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (54, N'TIPOS_EVENTOS_EQUIPO', N'Tipos Eventos Equipo', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (55, N'TIPOS_PRODUCTO_EGRESO_PROY', N'Tipos Producto Egreso Proyecto', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (56, N'TIPO_PROYECTO_ACTIVIDAD_VALOR', N'Tipo Proyecto Actividad Valor', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (57, N'TIPO_CLIENTE', N'Tipo Cliente', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (58, N'TIPO_ACTIVIDAD', N'Tipo Actividad', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (60, N'TIPOO_CAUSARAIZ', N'Tipo_CausaRaiz', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (61, N'QUE SE HIZO', N'QUE SE HIZO', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (62, N'QUE SE ENCONTRO', N'QUE SE ENCONTRO', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (63, N'TIPO_CONTRATO', N'TIPO CONTRATO', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (64, N'TIPO_DATO', N'TIPO DATO', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (1064, N'CLASIFICACIÓN DE TIPO DE TRABAJO', N'Clasificación de Tipo de Trabajo', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (1065, N'OPERADORES', N'Operadores', N'A')
INSERT [dbo].[MT_Tablas] ([Id_Tabla], [Tabla], [Descripcion], [Estado]) VALUES (1066, N'TIPO FRECUENCIA CORREO', N'Tipo Frecuencia Correo', N'A')
SET IDENTITY_INSERT [dbo].[MT_Tablas] OFF
SET IDENTITY_INSERT [dbo].[MT_TipoTrabajo] ON 

INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (1, N'1', N'BSALTO', N'10453', N'M&R', N'ESCAPE EN LA VIA PUBLICA', 2, N'A', CAST(N'2020-06-24T12:30:29.080' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'S', 24, 36, 96, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (2, N'1', N'BSALTO', N'10517', N'M&R', N'SONDEOS', 2, N'A', CAST(N'2020-06-24T12:40:04.333' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'N', 1, 2, 3, NULL)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (3, N'1', N'BSALTO', N'11322', N'M&R', N'REPARACION DE VIAS - AA.PP', 2, N'A', CAST(N'2020-06-24T12:44:57.793' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'N', 1, 2, 3, NULL)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (4, N'1', N'BSALTO', N'11323', N'M&R', N'REPARACION DE VEREDA - AA.PP.', 2, N'A', CAST(N'2020-06-24T12:48:57.907' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'N', 1, 3, 6, NULL)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (5, N'1', N'BSALTO', N'TIPO EJEMPLO 1', N'TYA', N'TIPO EJEMPLO 1', 1, N'A', CAST(N'2020-06-30T08:57:53.130' AS DateTime), N'N', N'N', N'N', N'N', N'N', N'N', 1, 0, N'N', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (6, N'1', N'BSALTO', N'MANTPREVINS', N'TYA', N'MANTENIMIENTO PREVENTIVO INSTRUMENTACION', 1, N'A', CAST(N'2020-07-10T10:02:48.737' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'N', 1, 1, 1, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (7, N'1', N'BSALTO', N'INSTMED', N'ADM', N'INSTALACION DE MEDIDOR', 1, N'A', CAST(N'2020-07-10T10:15:25.967' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 1, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (8, N'1', N'BSALTO', N'1', N'TYA', N'ELECTRICO', 2, N'A', CAST(N'2020-07-31T10:00:29.837' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 2, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (9, N'1', N'BSALTO', N'2', N'TYA', N'INSTRUMENTACION', 2, N'A', CAST(N'2020-07-31T10:01:05.617' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 2, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (10, N'1', N'BSALTO', N'4', N'TYA', N'SCADA', 2, N'A', CAST(N'2020-07-31T10:01:52.550' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 2, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (11, N'1', N'BSALTO', N'5', N'TYA', N'ELECTRICO', 2, N'E', CAST(N'2020-07-31T10:02:41.763' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 4, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (12, N'1', N'BSALTO', N'6', N'TYA', N'INSTRUMENTACION', 2, N'E', CAST(N'2020-07-31T10:03:42.207' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 2, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (13, N'1', N'BSALTO', N'7', N'TYA', N'SCADA', 2, N'E', CAST(N'2020-07-31T10:04:09.867' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 4, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (14, N'1', N'BSALTO', N'8', N'TYA', N'INSTRUMENTACION', 2, N'E', CAST(N'2020-07-31T10:05:54.567' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 4, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (15, N'1', N'BSALTO', N'10', N'TYA', N'ELECTRICO', 1, N'A', CAST(N'2020-07-31T10:06:58.540' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 5, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (16, N'1', N'BSALTO', N'11', N'TYA', N'INSTRUMENTACION', 1, N'A', CAST(N'2020-07-31T10:07:32.820' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 5, 0, N'S', 1, 2, 3, 1147)
INSERT [dbo].[MT_TipoTrabajo] ([Id_TipoTrabajo], [Id_Cliente], [Id_Usuario], [Codigo], [Linea], [Descripcion], [Tipo], [Estado], [Fecha_Registro], [Control_Item], [Control_Equipo], [Control_Integrante], [Control_Anexo], [Control_Medida], [Control_Costo], [Id_Servicio], [Id_Padre], [Control_Raiz], [Proceso], [Alerta], [Caida], [Clasificacion]) VALUES (17, N'1', N'BSALTO', N'12', N'TYA', N'SCADA', 1, N'A', CAST(N'2020-07-31T10:07:57.713' AS DateTime), N'S', N'S', N'S', N'S', N'S', N'S', 5, 0, N'S', 1, 2, 3, 1147)
SET IDENTITY_INSERT [dbo].[MT_TipoTrabajo] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (1, N'Administrador', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (2, N'Coordinador', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (3, N'Jefe de Cuadrilla', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (4, N'Fiscalizador', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (5, N'Informante', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (6, N'Coordinador 1203', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (7, N'Rol1', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (8, N'RolDemo', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (9, N'RolDemo', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (10, N'RolDemo 938', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (11, N'RolDemo', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (12, N'Supervisor', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (13, N'Jefe Centro de Operaciones Hydriapac', N'A')
INSERT [dbo].[Roles] ([Id_Rol], [Descripcion], [Estado]) VALUES (14, N'Prueba General', N'A')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Roles_Usuario] ON 

INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (1, N'AAGUIRRE', 1, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (2, N'AACOSTA', 2, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (3, N'AACOSTA', 3, N'I')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (4, N'ETORRES', 2, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (5, N'SSANCHEZ', 2, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (6, N'MRIZZO', 2, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (7, N'JMOREIRA', 2, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (8, N'BSALTO', 12, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (9, N'EASENCIO', 13, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (11, N'VQUINTANA', 12, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (12, N'AZAMBRANO', 1, N'A')
INSERT [dbo].[Roles_Usuario] ([Id_Rol_Usuario], [Id_Usuario], [Id_Rol], [Estado]) VALUES (13, N'HCRUZ', 1, N'A')
SET IDENTITY_INSERT [dbo].[Roles_Usuario] OFF
ALTER TABLE [dbo].[MT_Contrato]  WITH CHECK ADD  CONSTRAINT [FK_MT_Contrato_MT_TablaDetalle] FOREIGN KEY([tipo])
REFERENCES [dbo].[MT_TablaDetalle] ([Id_TablaDetalle])
GO
ALTER TABLE [dbo].[MT_Contrato] CHECK CONSTRAINT [FK_MT_Contrato_MT_TablaDetalle]
GO
ALTER TABLE [dbo].[MT_Contrato_Documentado]  WITH CHECK ADD  CONSTRAINT [FK_MT_Contrato_Documentado_MT_Contrato] FOREIGN KEY([Id_Contrato])
REFERENCES [dbo].[MT_Contrato] ([Id_Contrato])
GO
ALTER TABLE [dbo].[MT_Contrato_Documentado] CHECK CONSTRAINT [FK_MT_Contrato_Documentado_MT_Contrato]
GO
ALTER TABLE [dbo].[MT_Contrato_Prorroga]  WITH CHECK ADD  CONSTRAINT [FK_MT_Contrato_Prorroga_MT_Contrato] FOREIGN KEY([Id_Contrato])
REFERENCES [dbo].[MT_Contrato] ([Id_Contrato])
GO
ALTER TABLE [dbo].[MT_Contrato_Prorroga] CHECK CONSTRAINT [FK_MT_Contrato_Prorroga_MT_Contrato]
GO
ALTER TABLE [dbo].[MT_ContratoAlerta]  WITH CHECK ADD  CONSTRAINT [FK_MT_ContratoAlerta_MT_Contrato] FOREIGN KEY([Id_Contrato])
REFERENCES [dbo].[MT_Contrato] ([Id_Contrato])
GO
ALTER TABLE [dbo].[MT_ContratoAlerta] CHECK CONSTRAINT [FK_MT_ContratoAlerta_MT_Contrato]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_MT_Prospecto] FOREIGN KEY([Id_Prospecto])
REFERENCES [dbo].[MT_Prospecto] ([Id_Prospecto])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_MT_Prospecto]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Actividad]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Actividad_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Actividad] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Actividad_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Anexo]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Anexo_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Anexo] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Anexo_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_CausaRaiz]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_CausaRaiz_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_CausaRaiz] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_CausaRaiz_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Egreso]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Egreso_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Egreso] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Egreso_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Equipo]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Equipo_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Equipo] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Equipo_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Estado]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Estado_MT_OrdenTrabajo] FOREIGN KEY([Id_orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Estado] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Estado_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Formulario]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Formulario_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo_Detalle])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Formulario] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Formulario_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Integrante]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Integrante_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Integrante] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Integrante_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Medida]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Medida_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Medida] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Medida_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Novedad]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Novedad_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Novedad] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Novedad_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Valor]  WITH CHECK ADD  CONSTRAINT [FK_MT_OrdenTrabajo_Valor_MT_OrdenTrabajo] FOREIGN KEY([Id_Orden_Trabajo])
REFERENCES [dbo].[MT_OrdenTrabajo] ([Id_OrdenTrabajo])
GO
ALTER TABLE [dbo].[MT_OrdenTrabajo_Valor] CHECK CONSTRAINT [FK_MT_OrdenTrabajo_Valor_MT_OrdenTrabajo]
GO
ALTER TABLE [dbo].[MT_Prospecto]  WITH CHECK ADD  CONSTRAINT [FK_MT_Prospecto_MT_TablaDetalle] FOREIGN KEY([tipo])
REFERENCES [dbo].[MT_TablaDetalle] ([Id_TablaDetalle])
GO
ALTER TABLE [dbo].[MT_Prospecto] CHECK CONSTRAINT [FK_MT_Prospecto_MT_TablaDetalle]
GO
ALTER TABLE [dbo].[Mt_Prospecto_Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Mt_Prospecto_Alerta_MT_Prospecto] FOREIGN KEY([Id_Prospecto])
REFERENCES [dbo].[MT_Prospecto] ([Id_Prospecto])
GO
ALTER TABLE [dbo].[Mt_Prospecto_Alerta] CHECK CONSTRAINT [FK_Mt_Prospecto_Alerta_MT_Prospecto]
GO
ALTER TABLE [dbo].[MT_Prospecto_Documentado]  WITH CHECK ADD  CONSTRAINT [FK_MT_Prospecto_Documentado_MT_Prospecto] FOREIGN KEY([Id_Prospecto])
REFERENCES [dbo].[MT_Prospecto] ([Id_Prospecto])
GO
ALTER TABLE [dbo].[MT_Prospecto_Documentado] CHECK CONSTRAINT [FK_MT_Prospecto_Documentado_MT_Prospecto]
GO
ALTER TABLE [dbo].[MT_Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_MT_Contrato] FOREIGN KEY([Id_contrato])
REFERENCES [dbo].[MT_Contrato] ([Id_Contrato])
GO
ALTER TABLE [dbo].[MT_Proyecto] CHECK CONSTRAINT [FK_MT_Proyecto_MT_Contrato]
GO
ALTER TABLE [dbo].[MT_Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_MT_Prospecto] FOREIGN KEY([Id_Prospecto])
REFERENCES [dbo].[MT_Prospecto] ([Id_Prospecto])
GO
ALTER TABLE [dbo].[MT_Proyecto] CHECK CONSTRAINT [FK_MT_Proyecto_MT_Prospecto]
GO
ALTER TABLE [dbo].[MT_Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto] CHECK CONSTRAINT [FK_MT_Proyecto_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Anexo]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_Anexo_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Anexo] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_Anexo_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Equipo]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_Equipo_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Equipo] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_Equipo_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Integrante]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_Integrante_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Integrante] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_Integrante_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Novedad]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_Novedad_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Novedad] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_Novedad_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Valor]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Actividad_Valor_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_Actividad_Valor] CHECK CONSTRAINT [FK_MT_Proyecto_Actividad_Valor_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_ActividadEgreso]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_ActividadEgreso_MT_Proyecto_Actividad] FOREIGN KEY([Id_Proyecto_Actividad])
REFERENCES [dbo].[MT_Proyecto_Actividad] ([Id_Proyecto_Actividad])
GO
ALTER TABLE [dbo].[MT_Proyecto_ActividadEgreso] CHECK CONSTRAINT [FK_MT_Proyecto_ActividadEgreso_MT_Proyecto_Actividad]
GO
ALTER TABLE [dbo].[MT_Proyecto_Alerta]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Alerta_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto_Alerta] CHECK CONSTRAINT [FK_MT_Proyecto_Alerta_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_Proyecto_Documento]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Documento_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto_Documento] CHECK CONSTRAINT [FK_MT_Proyecto_Documento_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_Proyecto_Parametro]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Parametro_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto_Parametro] CHECK CONSTRAINT [FK_MT_Proyecto_Parametro_MT_Proyecto]
GO
ALTER TABLE [dbo].[MT_Proyecto_Prorroga]  WITH CHECK ADD  CONSTRAINT [FK_MT_Proyecto_Prorroga_MT_Proyecto] FOREIGN KEY([Id_Proyecto])
REFERENCES [dbo].[MT_Proyecto] ([Id_Proyecto])
GO
ALTER TABLE [dbo].[MT_Proyecto_Prorroga] CHECK CONSTRAINT [FK_MT_Proyecto_Prorroga_MT_Proyecto]
GO
/****** Object:  StoredProcedure [dbo].[Sp_Dashboard_EstadoOT]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sp_Dashboard_EstadoOT]
@id_empresa varchar(2)
as

begin

select t.Descripcion as Estado, count(*) as total
from MT_OrdenTrabajo_Estado as mtpv
inner join MT_OrdenTrabajo mtp on mtpv.Id_Orden_Trabajo = mtp.Id_OrdenTrabajo
inner join MT_TablaDetalle t on mtpv.Estado_Orden_Trabajo = t.Id_TablaDetalle
--inner join MT_Contrato c on mtp.Id_contrato = c.Id_Contrato
inner join MT_Proyecto pry on mtp.Id_Proyecto = pry.Id_Proyecto
where 
--mtpv.Id_Orden_Trabajo = @idOrden 
mtpv.EstadoO IN ('A') and pry.Id_Empresa = @id_empresa
GROUP BY  t.Descripcion
having count(*)>0
order by t.Descripcion
end


GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_Consulta_Localidad]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_LINK_Consulta_Localidad] 
@empresa varchar(5)
AS
BEGIN

	SELECT *
	FROM [SADATABASE]..[DBA].[LOCALIDAD]  
	where cia_codigo = @empresa 

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaBodegas]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaBodegas] 
@empresa varchar(5)
AS
BEGIN

	SELECT bodegas.cod_bod, bodegas.nombre, bodegas.cia_codigo FROM [SADATABASE]..[DBA].[bodegas] as bodegas 
	where cia_codigo = @empresa

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaCampamento]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaCampamento] 
--@empresa varchar(5)
AS
BEGIN
--SELECT [COD_CAMPAMENTO]
--      ,[DESCRIPCION]
--      ,[ESTADO]
--      ,[cod_bod]
--  FROM [SADATABASE]..[DBA].[MT_CAMPAMENTO]

	SELECT campamento.COD_CAMPAMENTO,campamento.DESCRIPCION,campamento.ESTADO FROM [SADATABASE]..[DBA].[MT_CAMPAMENTO] as campamento
	--where cia_codigo = @empresa

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaCentroConsumo_SubCat]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaCentroConsumo_SubCat] 
@empresa varchar(5),
@linea varchar(10)
AS
BEGIN

	SELECT subcategorias.codcen, subcategorias.nombre, subcategorias.quimi_linea, subcategorias.cia_codigo, subcategorias.estado 
	FROM [SADATABASE]..[DBA].[centro_consumo] as subcategorias 
	where cia_codigo = @empresa and quimi_linea = @linea and estado = 'A'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaClientes]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaClientes] 
@empresa varchar(5)
AS
BEGIN

	SELECT clientes.cod_cli, clientes.nom_cli, clientes.direccion, clientes.telefono, clientes.email, clientes.estado, clientes.abreviatura_cliente, clientes.categoria, clientes.cia_codigo 
	FROM [SADATABASE]..[DBA].[clientes] as clientes
	where cia_codigo = @empresa and 
	clientes.abreviatura_cliente is not null

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaContratos]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaContratos] 
@empresa varchar(5)
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[tb_quimi_mant_contratos_proyectos] as contratos 
	where cia_codigo = @empresa --and proyecto in ('CTR', 'PRO', 'LIC', 'PRY')

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaCredencialUsuario]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_LINK_ConsultaCredencialUsuario] 
	@usuario_id varchar(10),
	@empresa varchar(5)
AS
BEGIN
Select usuario.user_id,usuario.user_clave, usuario.user_status, usuario.user_descrip, usuario.cia_codigo
from openquery([SADATABASE], 'SELECT user_id,user_clave, user_status, user_descrip, cia_codigo from tb_data_seg_def_user') as usuario 
--SELECT usuario.user_id,usuario.user_clave, usuario.user_status, usuario.user_descrip FROM [SADATABASE]..[DBA].[tb_data_seg_def_user] as usuario
where usuario.user_id = @usuario_id and usuario.cia_codigo = @empresa and usuario.user_status = 'A'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaEmpresas]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaEmpresas] 
	
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[TBCON_MCIAS] as empresas
	where CIA_ESTATUS = 'A'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaEmpresas_Nombre]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_LINK_ConsultaEmpresas_Nombre] 
	@cia_codigo varchar(5)
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[TBCON_MCIAS] as empresas
	where CIA_ESTATUS = 'A' and empresas.cia_codigo = @cia_codigo 

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaItems]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaItems] 
@empresa varchar(5)	
AS
BEGIN
	IF(@empresa='00')
	BEGIN
	----SELECT * FROM [SADATABASE]..[DBA].[items] as Items
	SELECT cod_item, ite_cod_item, descripcion, descrip_res, serie, tipo, costo_contable, costo_propio, costo_cont_dol, item_activo, fec_creacion, costo_ini, costo_proc, costo_ultimo, costo_doc_ini, fecha_ult_compra, costo_ini_anterio, costo_dolar_ante, cia_codigo FROM [SADATABASE]..[DBA].[items] as Items
	END
	ELSE
	BEGIN
	--SELECT * FROM [SADATABASE]..[DBA].[items] as Items 
	SELECT cod_item, ite_cod_item, descripcion, descrip_res, serie, tipo, costo_contable, costo_propio, costo_cont_dol, item_activo, fec_creacion, costo_ini, costo_proc, costo_ultimo, costo_doc_ini, fecha_ult_compra, costo_ini_anterio, costo_dolar_ante, cia_codigo FROM [SADATABASE]..[DBA].[items] as Items
	where item_activo = 'S' and cia_codigo = @empresa
	END

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaLineas]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaLineas] 
	@empresa varchar(5)	
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[lineas] as lineas
	where cia_codigo = @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Link_ConsultaMenu_XUsuario]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================
--	Create:  <28-04-2019> 
-- ====================================
CREATE PROCEDURE [dbo].[sp_Link_ConsultaMenu_XUsuario]
 @Id_Usuario Varchar(10)
,@Id_Empresa Varchar(2)
,@Grupo_ID Varchar(6)

As
Begin
	Select gr_us.grup_id as grup_id_GR_US,gr_us.user_id as user_id_GR_US,gr_us.cia_codigo as cia_codigo_GR_US
	, grupUsu.grup_id,grupUSU.grup_descrip
	,grus.*
	from openquery([SADATABASE], 'select * from tb_data_seg_rel_gr_us') as gr_us 
	Inner join [SADATABASE]..[DBA].[tb_data_seg_grup_user] as grupUsu on gr_us.grup_id = grupUsu.grup_id
	Inner join [SADATABASE]..[DBA].[tb_data_seg_rel_grus] as grus on gr_us.grup_id = grus.grus_id
	Where grus.cia_codigo=@Id_Empresa and gr_us.user_id=@Id_Usuario and grupUsu.grup_id=@Grupo_ID 
	--gr_us.cia_codigo='01' and 	gr_us.user_id='AAGUIRRE' and grupUsu.grup_id='WEB_FT'--WEB_UG  
End


GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaMonedas]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaMonedas] 
	@empresa varchar(5)	
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[monedas] as monedas
	where cia_codigo = @empresa
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaPersonas]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaPersonas] 
	
AS
BEGIN

	SELECT *, --concat(primer_nombre, ' ', segundo_nombre, ' ', primer_apellido, ' ', segundo_apellido) as 
	primer_nombre as Nombres_Completos 
	FROM [SADATABASE]..[DBA].[rh_persona] as Personas where estado = '001'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaStock]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_LINK_ConsultaStock] 
	@empresa varchar(5)	
AS
BEGIN
SET rowcount 10
	SELECT * FROM [SADATABASE]..[DBA].[stock] as stock	
	where cia_codigo = @empresa and saldo > 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaSucursal]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaSucursal] 
	@empresa varchar(5)	
AS
BEGIN

	SELECT sucursal.cod_suc, sucursal.nombre, sucursal.direccion, sucursal.cia_codigo FROM [SADATABASE]..[DBA].[sucursal] as sucursal
	where cia_codigo = @empresa
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaTipoServicio_Cat]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaTipoServicio_Cat] 
@empresa varchar(5),
@linea varchar(10)
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] as categorias 
	where cia_codigo = @empresa and cod_linea = @linea

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaUnidades]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaUnidades] 
	
AS
BEGIN

	SELECT * FROM [SADATABASE]..[DBA].[medidas] as Medidas

END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaUsuarios]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaUsuarios] 
	@Usuario varchar(10),
	@empresa varchar(5)
AS	
BEGIN

	--Cambiar left Join luego del tener enlace con email en Tabla_User.
	If @Usuario = 'ALL' Begin
		IF @empresa = '00' BEGIN
		Select * from openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr 
		--Select * From [SADATABASE]..[DBA].[tb_data_seg_def_user] as usr 
		left outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
		
		Order By user_id Asc
		END
		ELSE
		BEGIN
		Select * from openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr 
		--Select * From [SADATABASE]..[DBA].[tb_data_seg_def_user] as usr 
		left outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
		where  usr.cia_codigo =@empresa --and usr.user_status = 'A' and
		Order By user_id Asc
		END
	End
	Else Begin 
		Select * from openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr 
		--Select * From [SADATABASE]..[DBA].[tb_data_seg_def_user] as usr 
		left outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
		where user_id = @Usuario And usr.user_status = 'A' and usr.cia_codigo = @empresa
		Order By user_id Asc
	End
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LINK_ConsultaVehiculos]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LINK_ConsultaVehiculos] 
--@empresa varchar(5)
AS
BEGIN

	SELECT vehiculos.COD_CAMPAMENTO,vehiculos.COD_VEHICULO,vehiculos.DESCRIPCION,vehiculos.PLACA FROM [SADATABASE]..[DBA].[MT_VEHICULO] as vehiculos
	--where cia_codigo = @empresa

END
GO
/****** Object:  StoredProcedure [dbo].[Sp_MtOrdenTActividad]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_MtOrdenTActividad]    --Sp_MtOrdenTActividad 9, 66, 'BSALTO'
	-- Add the parameters for the stored procedure here
	 @id_titpotrabajo int,
	 @id_orden int,
	 @usuario varchar(10)
AS
BEGIN
	    -- Insert statements for procedure here

	 declare @primera_actividad_orden int, @fecha_asignacion_grupo datetime
 set @fecha_asignacion_grupo = (select Fecha_asignacion_grupo_trabajo from MT_OrdenTrabajo where Id_OrdenTrabajo = @id_orden)

	IF NOT EXISTS(SELECT * from MT_OrdenTrabajo_Actividad mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	left outer join MT_Actividad ma on mot.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Estado where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden)
		BEGIN
	
			INSERT INTO MT_OrdenTrabajo_Actividad(Id_Orden_Trabajo, Id_Actividad, EstadoAct, Orden, Fecha, Id_Usuario)(select mot.Id_OrdenTrabajo, ma.Id_Actividad, mot.EstadoEditOrden, ma.Orden, GETDATE(), @usuario from MT_OrdenTrabajo mot
		inner join MT_TipoTrabajo mtp on mot.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
		inner join MT_Actividad ma on mtp.Id_TipoTrabajo = ma.Id_TipoTrabajo
		where mot.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mot.Id_OrdenTrabajo = @id_orden and ma.Estado = 'A')

			UPDATE MT_OrdenTrabajo_Actividad  SET Estado = 67
									WHERE Id_Orden_Trabajo = @id_orden

 
				set @primera_actividad_orden = (SELECT top(1) Id_Actividad FROM MT_OrdenTrabajo_Actividad where Id_Orden_Trabajo = @id_orden ORDER BY Id_Actividad ASC)
 
				 IF @fecha_asignacion_grupo IS NOT NULL
					BEGIN
					update MT_OrdenTrabajo_Actividad set Fecha_Ini = @fecha_asignacion_grupo, Estado= 68 where Id_Orden_Trabajo = @id_orden and Id_Actividad = @primera_actividad_orden
					--UPDATE MT_OrdenTrabajo SET Fecha_inicio_ejecucion = @fecha_asignacion_grupo where Id_OrdenTrabajo = @id_orden
					END

			select mot.*, mo.Codigo_Cliente, ma.Descripcion, t.Descripcion as DescripcionEstado from MT_OrdenTrabajo_Actividad mot
			inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			inner join MT_Actividad ma on mot.Id_Actividad = ma.Id_Actividad
			left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Estado
			where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden
		
		END
	ELSE 
		BEGIN
	
			set @primera_actividad_orden = (SELECT top(1) Id_Actividad FROM MT_OrdenTrabajo_Actividad where Id_Orden_Trabajo = @id_orden ORDER BY Id_Actividad ASC)

					if @primera_actividad_orden != 0  
							begin
									IF @fecha_asignacion_grupo IS NOT NULL
										BEGIN
			 
											update MT_OrdenTrabajo_Actividad set Fecha_Ini = @fecha_asignacion_grupo, Estado= 68 where Id_Orden_Trabajo = @id_orden and Id_Actividad = @primera_actividad_orden
											--UPDATE MT_OrdenTrabajo SET Fecha_inicio_ejecucion = @fecha_asignacion_grupo where Id_OrdenTrabajo = @id_orden
										END

				
							select mot.*, mo.Codigo_Cliente, ma.Descripcion, t.Descripcion as DescripcionEstado from MT_OrdenTrabajo_Actividad mot
							inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
							inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
							inner join MT_Actividad ma on mot.Id_Actividad = ma.Id_Actividad
							left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Estado
							where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden
							
								--					END
							end
				    else
							begin
									select mot.*, mo.Codigo_Cliente, mot.Nombre_Actividad as Descripcion, t.Descripcion as DescripcionEstado from MT_OrdenTrabajo_Actividad mot
									inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
									inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
									--inner join MT_Actividad ma on mot.Id_Actividad = ma.Id_Actividad
									left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Estado
									where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden
							end
	END

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ActActividadOrden]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ActActividadOrden]
	-- Add the parameters for the stored procedure here
	@ID_ACTIVIDAD_ORDEN INT,
	@FECHA_INICIO DATETIME,
	@FECHA_FIN DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [MT_OrdenTrabajo_Actividad]
	SET Fecha_Ini = @FECHA_INICIO, Fecha_Fin = @FECHA_FIN
	WHERE Id_OrdenTrabajo_Actividad = @ID_ACTIVIDAD_ORDEN
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ActEstadoOrdenTrabajo]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ActEstadoOrdenTrabajo] 
	-- Add the parameters for the stored procedure here
	
	@id_OrdenTrabajo int,
	@estadoOrden int,
	@fechaI datetime,
	@fechaF datetime,
	@Comentario varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @id_Ordenestado int

    -- Insert statements for procedure here
	update MT_OrdenTrabajo set Estado = @estadoOrden where Id_OrdenTrabajo = @id_OrdenTrabajo

	set @id_Ordenestado = (select Id_OrdenTrabajo_Estado from MT_OrdenTrabajo_Estado where Id_orden_Trabajo = @id_OrdenTrabajo and EstadoO = 'A')

	update MT_OrdenTrabajo_Estado set EstadoO = 'I' , Fecha_Fin = @fechaI where Id_OrdenTrabajo_Estado = @id_Ordenestado

	INSERT INTO MT_OrdenTrabajo_Estado(Id_orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO, Comentario) values(@id_OrdenTrabajo, @estadoOrden, @fechaI, @fechaF, 'A', @Comentario)


END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_MTTablaDetalle_DescrInfo]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Sandro Yagual>
-- ALTER date: <15-12-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Consulta_MTTablaDetalle_DescrInfo]
	@Id_Detalle Int
AS
BEGIN
	Select *From MT_TablaDetalle Where  Id_Tabla= 12 And Id_TablaDetalle=@Id_detalle and Estado ='A'
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_Notificaciones_Entrada]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_Consulta_Notificaciones_Entrada]
@tipo varchar(50),
@IdNotificacion int,
@user varchar(10)
as
begin
--id de lectura por defecto 
if(@tipo = 'Entrada_Noti')begin

		declare @Id_Tabla_Criterio varchar(50)
		set @Id_Tabla_Criterio='Criterio_Notificacion'


		declare @Id_Tabla int,@Id_TablaDetalle int
		set @Id_Tabla=(select Id_Tabla from MT_Tablas where Descripcion = 'Criterio Notificacion')
		set @Id_TablaDetalle=(select top 1 Id_TablaDetalle from MT_Notificaciones n 
							  inner join MT_TablaDetalle mt on n.Criterio=mt.Id_TablaDetalle where mt.Descripcion='Entrada')
end
if(@IdNotificacion >0) begin
		select * from MT_Notificaciones where --Criterio=@Id_TablaDetalle and 
		Id_Notificacion=@IdNotificacion
		--select Id_TablaDetalle, Descripcion from MT_TablaDetalle where Codigo=@Id_Tabla_Criterio and Id_Tabla=@Id_Tabla
		--select Descripcion, Codigo from MT_TablaDetalle where Id_Tabla=49 select * from MT_Notificacion_Persona
end
else begin   -- Easencio
		select n.*, np.* 
		from MT_Notificaciones n
		inner join MT_Notificacion_Persona np on  n.Id_Notificacion=np.Id_Notificacion
		left outer JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.email=np.Correo
		where --Criterio=@Id_TablaDetalle and		 
		np.Estado=92 and usr.user_id = @user

end
end

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_Notificaciones_General]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_Consulta_Notificaciones_General] 
@tipo int,
@empresa varchar(5),
@id_codigo_Origen int
as
begin

if(@tipo = 80) --ORDEN TRABAJO

		BEGIN

		select n.* 
				,np.Id_Notificacion_Persona 
					  ,np.Id_Notificacion as IdNotificacion_Per
					  ,np.Id_Persona
					  ,np.Tipo
					  ,np.Correo
					  ,np.Fecha_Hora
					  ,np.Estado as Estado_Np, mtc.Codigo as Criterios,
					  mt.Codigo as Notificacion,
					  mtt.Codigo as Tipos,
					  mtp.Descripcion as prioridades,
					  mte.Descripcion as Estados,
					  usr.user_id as Usuario
				from MT_Notificaciones n
				inner join MT_Notificacion_Persona np on np.Id_Notificacion=n.Id_Notificacion
				inner join MT_TablaDetalle mt on n.Tipo_Notificacion = mt.Id_TablaDetalle 
				inner join MT_TablaDetalle mtp on n.Prioridad=mtp.Id_TablaDetalle
				inner join MT_TablaDetalle mtt on np.Tipo=mtt.Id_TablaDetalle
				inner join MT_TablaDetalle mte on np.Estado=mte.Id_TablaDetalle
				inner join MT_TablaDetalle mtc on n.Criterio=mtc.Id_TablaDetalle
				INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.user_id=n.Id_usuario
				inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
				--Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhpi on rhpi.id_persona=np.Id_Persona
	

				where n.Tipo_Notificacion = @tipo and usr.cia_codigo = @empresa and n.Id_Codigo_Origen = @id_codigo_Origen

			END
	ELSE
	if(@tipo = 81) --PROYECTO
				BEGIN
				select n.* 
					,np.Id_Notificacion_Persona 
						  ,np.Id_Notificacion as IdNotificacion_Per
						  ,np.Id_Persona
						  ,np.Tipo
						  ,np.Correo
						  ,np.Fecha_Hora
						  ,np.Estado as Estado_Np, mtc.Codigo as Criterios,
						  mt.Codigo as Notificacion,
						  mtt.Codigo as Tipos,
						  mtp.Descripcion as prioridades,
						  mte.Descripcion as Estados,
						  usr.user_id as Usuario, mpro.Codigo_Cliente as CodigoClieProyecto
					from MT_Notificaciones n
					inner join MT_Notificacion_Persona np on np.Id_Notificacion=n.Id_Notificacion
					inner join MT_TablaDetalle mt on n.Tipo_Notificacion = mt.Id_TablaDetalle 
					inner join MT_TablaDetalle mtp on n.Prioridad=mtp.Id_TablaDetalle
					inner join MT_TablaDetalle mtt on np.Tipo=mtt.Id_TablaDetalle
					inner join MT_TablaDetalle mte on np.Estado=mte.Id_TablaDetalle
					inner join MT_TablaDetalle mtc on n.Criterio=mtc.Id_TablaDetalle
					INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.user_id=n.Id_usuario
					inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
					--Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhpi on rhpi.id_persona=np.Id_Persona
					inner join MT_Proyecto mpro on n.Id_Codigo_origen = mpro.Id_Proyecto

					where n.Tipo_Notificacion = @tipo and usr.cia_codigo = @empresa and n.Id_Codigo_Origen = @id_codigo_Origen
				END
	ELSE
	if(@tipo = 82) --GENERAL
				BEGIN
				select n.* 
					,np.Id_Notificacion_Persona 
							,np.Id_Notificacion as IdNotificacion_Per
							,np.Id_Persona
							,np.Tipo
							,np.Correo
							,np.Fecha_Hora
							,np.Estado as Estado_Np, mtc.Codigo as Criterios,
							mt.Codigo as Notificacion,
							mtt.Codigo as Tipos,
							mtp.Descripcion as prioridades,
							mte.Descripcion as Estados,
							usr.user_id as Usuario
					from MT_Notificaciones n
					inner join MT_Notificacion_Persona np on np.Id_Notificacion=n.Id_Notificacion
					inner join MT_TablaDetalle mt on n.Tipo_Notificacion = mt.Id_TablaDetalle 
					inner join MT_TablaDetalle mtp on n.Prioridad=mtp.Id_TablaDetalle
					inner join MT_TablaDetalle mtt on np.Tipo=mtt.Id_TablaDetalle
					inner join MT_TablaDetalle mte on np.Estado=mte.Id_TablaDetalle
					inner join MT_TablaDetalle mtc on n.Criterio=mtc.Id_TablaDetalle
					INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.user_id=n.Id_usuario
					inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
					--Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhpi on rhpi.id_persona=np.Id_Persona
					

					where n.Tipo_Notificacion = @tipo and usr.cia_codigo = @empresa
				END
	ELSE
	if(@tipo = 119) --CONTRATO 
				BEGIN
				select n.* 
					,np.Id_Notificacion_Persona 
						  ,np.Id_Notificacion as IdNotificacion_Per
						  ,np.Id_Persona
						  ,np.Tipo
						  ,np.Correo
						  ,np.Fecha_Hora
						  ,np.Estado as Estado_Np, mtc.Codigo as Criterios,
						  mt.Codigo as Notificacion,
						  mtt.Codigo as Tipos,
						  mtp.Descripcion as prioridades,
						  mte.Descripcion as Estados,
						  usr.user_id as Usuario, mpro.Nombre as Nombre_Contrato
					from MT_Notificaciones n
					inner join MT_Notificacion_Persona np on np.Id_Notificacion=n.Id_Notificacion
					inner join MT_TablaDetalle mt on n.Tipo_Notificacion = mt.Id_TablaDetalle 
					inner join MT_TablaDetalle mtp on n.Prioridad=mtp.Id_TablaDetalle
					inner join MT_TablaDetalle mtt on np.Tipo=mtt.Id_TablaDetalle
					inner join MT_TablaDetalle mte on np.Estado=mte.Id_TablaDetalle
					inner join MT_TablaDetalle mtc on n.Criterio=mtc.Id_TablaDetalle
					INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.user_id=n.Id_usuario
					inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
					--Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhpi on rhpi.id_persona=np.Id_Persona
					inner join Mt_Contrato mpro on n.Id_Codigo_origen = mpro.Id_Contrato

					where n.Tipo_Notificacion = @tipo and usr.cia_codigo = @empresa and n.Id_Codigo_Origen = @id_codigo_Origen
				END
	
	ELSE
	if(@tipo = 1166) --PROSPECTO 
				BEGIN
				select n.* 
					,np.Id_Notificacion_Persona 
						  ,np.Id_Notificacion as IdNotificacion_Per
						  ,np.Id_Persona
						  ,np.Tipo
						  ,np.Correo
						  ,np.Fecha_Hora
						  ,np.Estado as Estado_Np, mtc.Codigo as Criterios,
						  mt.Codigo as Notificacion,
						  mtt.Codigo as Tipos,
						  mtp.Descripcion as prioridades,
						  mte.Descripcion as Estados,
						  usr.user_id as Usuario, mpro.Nombre as Nombre_Contrato
					from MT_Notificaciones n
					inner join MT_Notificacion_Persona np on np.Id_Notificacion=n.Id_Notificacion
					inner join MT_TablaDetalle mt on n.Tipo_Notificacion = mt.Id_TablaDetalle 
					inner join MT_TablaDetalle mtp on n.Prioridad=mtp.Id_TablaDetalle
					inner join MT_TablaDetalle mtt on np.Tipo=mtt.Id_TablaDetalle
					inner join MT_TablaDetalle mte on np.Estado=mte.Id_TablaDetalle
					inner join MT_TablaDetalle mtc on n.Criterio=mtc.Id_TablaDetalle
					INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS usr on usr.user_id=n.Id_usuario
					inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
					--Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhpi on rhpi.id_persona=np.Id_Persona
					inner join Mt_Contrato mpro on n.Id_Codigo_origen = mpro.Id_Contrato

					where n.Tipo_Notificacion = @tipo and usr.cia_codigo = @empresa and n.Id_Codigo_Origen = @id_codigo_Origen
				END

end
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_OrdenTrabajoEntrega_XClienteOT]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_Consulta_OrdenTrabajoEntrega_XClienteOT]
 @Id_EOT     Int
,@Id_Cliente varchar(10)
,@Criterio   varchar(15)
,@empresa varchar(2)
AS
BEGIN

	If @Criterio = 'All' Begin
		Select a_hijo.*--,clientes.cod_cli, clientes.nom_cli, con.Nombre As NombreContrato
	    From MT_OrdenTrabajo a_hijo
		--Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
		Inner Join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
		Inner Join [SADATABASE]..[DBA].[clientes] as clientes on pry.Id_Cliente = clientes.cod_cli
		Where clientes.cod_cli =@Id_Cliente 
		 And a_hijo.EstadoEditOrden ='A' And a_hijo.Id_entrega_orden_trabajo is Not Null and pry.Id_Empresa = @empresa and clientes.cia_codigo = @empresa
	End
	Else Begin
		Select a_hijo.*--,clientes.nom_cli, con.Nombre As NombreContrato
	    From MT_OrdenTrabajo a_hijo
		--Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
		Inner Join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
				Inner Join [SADATABASE]..[DBA].[clientes] as clientes on pry.Id_Cliente = clientes.cod_cli
		Where Id_Cliente = Id_Cliente And a_hijo.EstadoEditOrden ='A'
			  And Id_entrega_orden_trabajo= @Id_EOT
			   And a_hijo.Id_entrega_orden_trabajo is Not Null and pry.Id_Empresa = @empresa and clientes.cia_codigo = @empresa
	End
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_OrdenTrabajoGrupo_XClienteOT]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_Consulta_OrdenTrabajoGrupo_XClienteOT]
 @Id_Cliente varchar(10)
,@Criterio   varchar(15)
,@empresa varchar(2),
@id_orden int 
AS
BEGIN

	If @Criterio = 'All' Begin
		Select Distinct a_hijo.Id_OrdenTrabajo, moi.Id_GrupoTrabajo, a_hijo.Codigo_Cliente, tpej.Descripcion  --,clientes.cod_cli, clientes.nom_cli, con.Nombre As NombreContrato
	    From MT_OrdenTrabajo a_hijo
		Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
		Inner Join [SADATABASE]..[DBA].[clientes] as clientes on con.Id_Cliente = clientes.cod_cli
		inner join MT_OrdenTrabajo_Integrante moi on a_hijo.Id_OrdenTrabajo = moi.Id_Orden_Trabajo
		inner join MT_TipoTrabajo tpej on a_hijo.Id_tipo_trabajo_ejecutado = tpej.Id_TipoTrabajo
		Where clientes.cod_cli = @Id_Cliente
		 And a_hijo.EstadoEditOrden ='A'  and con.Id_Empresa = @empresa and clientes.cia_codigo = @empresa and moi.Estado = 'A'
		 		 
	End
	Else Begin
	--	Select a_hijo.*,clientes.nom_cli, con.Nombre As NombreContrato
	--    From MT_OrdenTrabajo a_hijo
	--	Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
	--	Inner Join [SADATABASE]..[DBA].[clientes] as clientes on con.Id_Cliente = clientes.cod_cli
	--	Where Id_Cliente = Id_Cliente And a_hijo.EstadoEditOrden ='A'
	--		  And Id_entrega_orden_trabajo= @Id_EOT
	--		   And a_hijo.Id_entrega_orden_trabajo is Not Null and con.Id_Empresa = @empresa and clientes.cia_codigo = @empresa
	
	Select Distinct a_hijo.Id_OrdenTrabajo, moi.Id_GrupoTrabajo, a_hijo.Codigo_Cliente, tpej.Descripcion  --,clientes.cod_cli, clientes.nom_cli, con.Nombre As NombreContrato
	    From MT_OrdenTrabajo a_hijo
		Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
		Inner Join [SADATABASE]..[DBA].[clientes] as clientes on con.Id_Cliente = clientes.cod_cli
		inner join MT_OrdenTrabajo_Integrante moi on a_hijo.Id_OrdenTrabajo = moi.Id_Orden_Trabajo
		inner join MT_TipoTrabajo tpej on a_hijo.Id_tipo_trabajo_ejecutado = tpej.Id_TipoTrabajo
		Where clientes.cod_cli = @Id_Cliente
		 And a_hijo.EstadoEditOrden ='A'  and con.Id_Empresa = @empresa and clientes.cia_codigo = @empresa and a_hijo.Id_OrdenTrabajo = @id_orden and moi.Estado = 'A'

		 --union 
	
		--Select Distinct a_hijo.Id_OrdenTrabajo, moi.Id_GrupoTrabajo, a_hijo.Codigo_Cliente, tpej.Descripcion  --,clientes.cod_cli, clientes.nom_cli, con.Nombre As NombreContrato
	 --   From MT_OrdenTrabajo a_hijo
		--Inner Join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
		--Inner Join [SADATABASE]..[DBA].[clientes] as clientes on con.Id_Cliente = clientes.cod_cli
		--inner join MT_OrdenTrabajo_Integrante moi on a_hijo.Id_OrdenTrabajo = moi.Id_Orden_Trabajo
		--inner join MT_TipoTrabajo tpej on a_hijo.Id_tipo_trabajo_ejecutado = tpej.Id_TipoTrabajo
		--Where clientes.cod_cli = 1
		-- And a_hijo.EstadoEditOrden ='A'  and con.Id_Empresa = @empresa and clientes.cia_codigo = @empresa 
	End


	
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_ParametrosSMTP]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_Consulta_ParametrosSMTP] as
begin

select Descripcion from MT_TablaDetalle where Id_Tabla=52 and Codigo like'SMTP_%' and Estado='A'

end

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Consulta_TipoTrabajoMedida]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Consulta_TipoTrabajoMedida]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here

	select mtm.* from MT_Tipo_Trabajo_Medida mtm
	inner join MT_TipoTrabajo mt on mtm.Id_Tipo_Trabajo = mt.Id_TipoTrabajo
	where mt.Tipo = 2

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaActividadesTTrabajoOrden]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <11/10/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaActividadesTTrabajoOrden] 
	-- Add the parameters for the stored procedure here

	--ojoooo este de aqui no lo estoy utilizando ya que era para ocultar si es q una actividad esta iniciada peor ya la cambie 
	
	@orden int,
	@fecha varchar(30)
AS
BEGIN

		if(@fecha = 'fechaInicio' )

		BEGIN
		select top(1) moa.* from MT_OrdenTrabajo mo 
		inner join MT_OrdenTrabajo_Actividad moa on mo.Id_OrdenTrabajo = moa.Id_Orden_Trabajo 
		where moa.Id_Orden_Trabajo = @orden and moa.Fecha_Ini is not null order by moa.Fecha_Ini asc
		END
		ELSE
		BEGIN

		select top(1) moa.* from MT_OrdenTrabajo mo 
		inner join MT_OrdenTrabajo_Actividad moa on mo.Id_OrdenTrabajo = moa.Id_Orden_Trabajo 
		where moa.Id_Orden_Trabajo = @orden and moa.Fecha_Fin is not null order by moa.Fecha_Fin DESC
		END
		
END




		
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaActividadesTTrabajoProyecto]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <11/10/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaActividadesTTrabajoProyecto] 
	-- Add the parameters for the stored procedure here
	
	@proyecto int,
	@fecha varchar(30),
	@empresa varchar(2)
AS
BEGIN

		if(@fecha = 'fechaInicio' )

		BEGIN
		select top(1) moa.* from MT_Proyecto mo 
		inner join MT_Proyecto_Actividad moa on mo.Id_Proyecto = moa.Id_Proyecto 
		where moa.Id_Proyecto = @proyecto and moa.Fecha_Ini is not null and mo.Id_Empresa = @empresa order by moa.Fecha_Ini asc
		END
		ELSE
		BEGIN

		select top(1) moa.* from MT_Proyecto mo 
		inner join MT_Proyecto_Actividad moa on mo.Id_Proyecto = moa.Id_Proyecto 
		where moa.Id_Proyecto = @proyecto and moa.Fecha_Fin is not null and mo.Id_Empresa = @empresa order by moa.Fecha_Fin DESC
		END
		
END




		

GO
/****** Object:  StoredProcedure [dbo].[SP_Quimipac_ConsultaClientesOrdenCont]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Quimipac_ConsultaClientesOrdenCont] 
	-- Add the parameters for the stored procedure here
@id_orden int,
@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	Select * from MT_EntregaOrden_Trabajo eo where exists 
	(select * from MT_OrdenTrabajo o 
	 --inner join MT_Contrato c on o.Id_contrato = c.Id_Contrato cambie por proyecto
	 inner join MT_Proyecto pry on o.Id_Proyecto = pry.Id_Proyecto 
	 where pry.Id_cliente = eo.Id_Cliente and o.Id_OrdenTrabajo = @id_orden and Id_Empresa = @empresa) 
	and eo.Id_Empresa = @empresa
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaCriterioNoti]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_ConsultaCriterioNoti] 
@pv_entrada varchar(50), 
@pv_salida varchar(50) as
begin
declare @Id_Tabla int,@Id_TablaDetalle int
set @Id_Tabla=(select Id_Tabla from MT_Tablas where Descripcion=@pv_entrada)
select Id_TablaDetalle, Descripcion from MT_TablaDetalle where Codigo=@pv_salida and Id_Tabla=@Id_Tabla
end 

--sp_Quimipac_ConsultaCriterioNoti 'Notificacion','Entrada'
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaEstacionMacrosector]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Yaritza Ponce>
-- ALTER date: <04-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaEstacionMacrosector] 
AS
BEGIN
	
select td.Id_TablaDetalle, td.Id_Tabla, td.Descripcion from MT_TablaDetalle td 
	inner join MT_Tablas t on td.Id_Tabla = t.Id_Tabla
	where td.Id_Tabla = 6 and td.Estado = 'A'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaEstacionProvincia]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <03-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaEstacionProvincia] 
	 --@padre int
AS
BEGIN
	 BEGIN

	select td.Id_TablaDetalle, td.Id_Tabla, td.Descripcion from MT_TablaDetalle td 
	inner join MT_Tablas t on td.Id_Tabla = t.Id_Tabla
	where td.Id_Tabla = 2

		
	 END
	 --ELSE
	 --BEGIN

	 --select * from MT_TablaDetalle where Id_Padre = @padre
		
	 --END

	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaEstacionProvinciaCiudad]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <03-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaEstacionProvinciaCiudad] 
	 @padre int
AS
BEGIN
	
	 BEGIN

	select td_padre.Id_TablaDetalle, td_padre.Id_Tabla, td_padre.Codigo as codigopro, td_padre.Descripcion as descripcionpro, td_padre.Estado, td_hijo.Id_Padre, td_hijo.Codigo as codigociu, td_hijo.Descripcion as descripcionciu  
	from MT_TablaDetalle td_padre 
	inner join MT_TablaDetalle td_hijo on td_padre.Id_TablaDetalle = td_hijo.Id_Padre
	where td_padre.Id_TablaDetalle = @padre

		
	 END
	 --ELSE
	 --BEGIN
	 
	 --select * from MT_TablaDetalle where Id_Padre = @padre
	 
		
	 --END

	
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaGrupoTTrabajoOrden]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <06/09/2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaGrupoTTrabajoOrden] 
	-- Add the parameters for the stored procedure here
	
	@orden int
AS
BEGIN
	--DECLARE @Exists INT
 
 --     IF EXISTS(select * from MT_OrdenTrabajo mo inner join MT_OrdenTrabajo_Integrante moi on mo.Id_OrdenTrabajo = moi.Id_Orden_Trabajo
	--			inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_recibido = mtp.Id_TipoTrabajo
	--		where moi.Id_Orden_Trabajo = @orden
	--	)
 --     BEGIN

 --           SET @Exists = 1
 --     END
 --     ELSE
 --     BEGIN
 --           SET @Exists = 0
 --     END
 
 --     select @Exists as Valor


		 select * from MT_OrdenTrabajo mo 
		 inner join MT_OrdenTrabajo_Integrante moi on mo.Id_OrdenTrabajo = moi.Id_Orden_Trabajo
				--inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_recibido = mtp.Id_TipoTrabajo
		where moi.Id_Orden_Trabajo = @orden
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMT_IntegranteOrdenTrabajo]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMT_IntegranteOrdenTrabajo]
	-- Add the parameters for the stored procedure here
	@pId_OrdenTrabajo int
	--@id_grupotrabajo int

AS
BEGIN
	    --Obtiene el IdEquipo
		DECLARE @TMP_TB_IdPersonaRol TABLE( Id int identity(1,1) not null primary key
										   ,IdPersona  int
										   ,Rol		   int  )
	    --Obtiene PK IdEquipo
		DECLARE @TMP_TB_PkIdOTInte   TABLE( Id int identity(1,1) not null primary key
										   ,PkIdOTInte int )
        
		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;

		--Variables @TMP_TB_IdEquipo
		DECLARE @IdPersona Int, @Rol int,@PkOTInte int;

		Set @contWhile  = 0;
		Set @NRegistros = (Select Count(DISTINCT mgi.Id_Persona)--, mgi.Rol_Usuario)
						   From MT_OrdenTrabajo_Integrante moi 
						   Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						   Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						   Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
						 );
		--INS TB TEMP
		Insert into @TMP_TB_IdPersonaRol (IdPersona,Rol)
					  	  Select Distinct mgi.Id_Persona, mgi.Rol_Usuario
						  From MT_OrdenTrabajo_Integrante moi 
						  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 

		--INS TB TEMP OTInte PK
		Insert into @TMP_TB_PkIdOTInte (PkIdOTInte)
					  	  Select Distinct moi.Id_OrdenTrabajo_Integrante
						  From MT_OrdenTrabajo_Integrante moi 
						  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
	    
		WHILE(@NRegistros > 0 AND @contWhile <= @NRegistros) BEGIN
			
			SET @IdPersona =(SELECT IdPersona  FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @Rol       =(SELECT Rol        FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @PkOTInte  =(SELECT PkIdOTInte FROM @TMP_TB_PkIdOTInte   WHERE Id=@contWhile)

			UPDATE MT_OrdenTrabajo_Integrante  SET Id_Persona = @IdPersona, Rol = @Rol 
			WHERE Id_Orden_Trabajo = @pId_OrdenTrabajo And Id_OrdenTrabajo_Integrante = @PkOTInte 

			SET @contWhile=@contWhile+1;
		END

		

--	UPDATE MT_OrdenTrabajo_Integrante 
--	SET Id_Persona = i.Id_Persona, 
--    Rol = i.Rol_Usuario 
--	FROM (
--    select DISTINCT mgi.Id_Persona, mgi.Rol_Usuario
--		from MT_OrdenTrabajo_Integrante moi 
--		inner join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
--		inner join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
--		where moi.Id_Orden_Trabajo = @id_ordentrabajo  ) i  --) i and moi.Id_GrupoTrabajo = @id_grupotrabajo
--where MT_OrdenTrabajo_Integrante.Id_Orden_Trabajo = @id_ordentrabajo  -- and MT_OrdenTrabajo_Integrante.Id_GrupoTrabajo = @id_grupotrabajo



	select moi.*, mgt.Nombre, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, mtd.Descripcion as Descripcion_Rol, mot.Codigo_Cliente 
	from MT_OrdenTrabajo_Integrante moi
	inner join MT_OrdenTrabajo mot on moi.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
	inner join MT_GrupoTrabajo mgt on moi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	inner join [SADATABASE]..[DBA].[rh_persona] p on moi.Id_Persona = p.id_persona
	inner join MT_TablaDetalle mtd on moi.Rol= mtd.Id_TablaDetalle
	where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo  --and moi.Id_GrupoTrabajo = @id_grupotrabajo



END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMt_OrdenTrabajoEgreso]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMt_OrdenTrabajoEgreso]
	-- Add the parameters for the stored procedure here
	 @id_titpotrabajo int,
	 @id_orden int,
	 @id_usuario varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here

	IF NOT EXISTS(SELECT mot.*, mo.Id_tipo_trabajo_ejecutado from MT_OrdenTrabajo_Egreso mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Tipo
	where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden)
	BEGIN
	INSERT INTO MT_OrdenTrabajo_Egreso(Id_Orden_Trabajo, Id_Item, EstadoAct, Fecha, Id_Usuario)(select mot.Id_OrdenTrabajo, ma.Id_Item, mot.EstadoEditOrden, GETDATE(),@id_usuario
	from MT_OrdenTrabajo mot
	inner join MT_TipoTrabajo mtp on mot.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	inner join MT_Items ma on mtp.Id_TipoTrabajo = ma.Id_TipoTrabajo
	where mot.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mot.Id_OrdenTrabajo = @id_orden)



	select mot.*, mo.Codigo_Cliente, it.Descripcion as ItemsItems, t.Descripcion as DescripcionTipo, mon.nombre 
	from MT_OrdenTrabajo_Egreso mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Tipo
	inner join [SADATABASE]..[DBA].[items] it on it.cod_item = mot.Id_Item
	left outer join [SADATABASE]..[DBA].[monedas] mon on mot.Moneda = mon.codmon
	where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden and mot.EstadoAct in ('A','I')
		
		END
	ELSE 
	BEGIN

	select mot.*, mo.Codigo_Cliente, it.Descripcion as ItemsItems, t.Descripcion as DescripcionTipo, mon.nombre 
	from MT_OrdenTrabajo_Egreso mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Tipo
	inner join [SADATABASE]..[DBA].[items] it on it.cod_item = mot.Id_Item
	left outer join [SADATABASE]..[DBA].[monedas] mon on mot.Moneda = mon.codmon
	where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden and mot.EstadoAct in ('A','I')
	

	END
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMT_OrdenTrabajoEquipoNuev]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <06-11-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMT_OrdenTrabajoEquipoNuev]
	@pid_ordentrabajo int,
	@nuevaOrden int
AS
BEGIN--sp_Quimipac_ConsultaMT_EquipoGrupoTrabajo
		--Obtiene el IdEquipo
		DECLARE @TMP_TB_IdEquipo TABLE( Id int identity(1,1) not null primary key
								       ,IdEquipo int, FechaI datetime, FechaF datetime, EstadoOE varchar(1), IdGrupoT int)
		--Obtiene La PK de IdOTEquipo
        --DECLARE @TMP_TB_IdOrdTrabEquipo TABLE( Id int identity(1,1) not null primary key
								--              ,IdOrdTrabEquipo int)

		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;
		
		--Variables tmp de Id
		DECLARE @IdEquipo Int, @FechaI datetime, @FechaF datetime, @EstadoOE varchar(1), @IdGrupoT int, @Pk_OTE Int;

		Set @NRegistros =(SELECT count(Distinct mge.Id_Equipo) AS IdEquipo
					      FROM MT_OrdenTrabajo_Equipo moe 
						  inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
						  where moe.Id_Orden_Trabajo = @pid_ordentrabajo
						 );
		Set @contWhile = 1;

		--IdEquipo
		Insert into @TMP_TB_IdEquipo (IdEquipo, FechaI, FechaF, EstadoOE, IdGrupoT)
			SELECT DISTINCT mge.Id_Equipo AS IdEquipo, moe.Fecha_Inicio, moe.Fecha_Fin, moe.Estado, moe.Id_GrupoTrabajo 
			FROM MT_OrdenTrabajo_Equipo moe 
			inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
			where moe.Id_Orden_Trabajo = @pid_ordentrabajo


		--Pk OTEq
		--Insert into @TMP_TB_IdOrdTrabEquipo (IdOrdTrabEquipo)
		--	SELECT DISTINCT moe.Id_OrdenTrabajo_Equipo AS Pk_OTEquipo
		--	FROM MT_OrdenTrabajo_Equipo moe 
		--	inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
		--	inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
		--	where moe.Id_Orden_Trabajo = @pid_ordentrabajo



		WHILE(@NRegistros>0 AND @contWhile<=@NRegistros) BEGIN

			
			SET @IdEquipo=(SELECT IdEquipo FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			SET @FechaI=(SELECT FechaI FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			SET @FechaF=(SELECT FechaF FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			SET @IdGrupoT=(SELECT IdGrupoT FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			--SET @Pk_OTE=(SELECT IdOrdTrabEquipo FROM @TMP_TB_IdOrdTrabEquipo WHERE Id=@contWhile)

			  --Update MT_OrdenTrabajo_Equipo Set Id_Equipo = @IdEquipo
			  --Where  Id_Orden_Trabajo = @pid_ordentrabajo And Id_OrdenTrabajo_Equipo = @Pk_OTE;

			  Insert into MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_Equipo, Fecha_Inicio, Fecha_Fin, Estado, Id_GrupoTrabajo) 
			  values (@nuevaOrden, @IdEquipo, @FechaI, @FechaF, 'A', @IdGrupoT)
			  --select @IdEquipo
			SET @contWhile=@contWhile+1;

		END

		--SELECT * FROM @TMP_TB_IdEquipo
		
		--select moe.*, mgt.Nombre, e.Descripcion, mot.Codigo_Cliente 
		--from MT_OrdenTrabajo_Equipo moe
		--inner join MT_OrdenTrabajo mot on moe.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
		--inner join MT_GrupoTrabajo mgt on moe.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
		--inner join MT_Equipo e on moe.Id_Equipo = e.Id_Equipo
		--where moe.Id_Orden_Trabajo = @pid_ordentrabajo --and moi.Id_GrupoTrabajo = @id_grupotrabajo

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMT_OrdenTrabajoIntegranteNuev]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12/10/2018>,
-- Description:	<Description,,>
-- =============================================

--sp_Quimipac_ConsultaMT_IntegranteOrdenTrabajo 2
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMT_OrdenTrabajoIntegranteNuev]
	-- Add the parameters for the stored procedure here
	@pId_OrdenTrabajo int,
	@nuevaOrden int

AS
BEGIN
	    --Obtiene el IdEquipo
		DECLARE @TMP_TB_IdPersonaRol TABLE( Id int identity(1,1) not null primary key
										   ,IdPersona  int
										   ,Rol int, FechaI datetime, FechaF datetime, EstadoOE varchar(1), IdGrupoT int)
	    --Obtiene PK IdEquipo
		--DECLARE @TMP_TB_PkIdOTInte   TABLE( Id int identity(1,1) not null primary key
		--								   ,PkIdOTInte int )
        
		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;

		--Variables @TMP_TB_IdEquipo
		DECLARE @IdPersona Int, @Rol int, @FechaI datetime, @FechaF datetime, @EstadoOE varchar(1), @IdGrupoT int,@PkOTInte int;

		--Set @contWhile  = 0;
		Set @NRegistros = (Select Count(DISTINCT mgi.Id_Persona)--, mgi.Rol_Usuario)
						   From MT_OrdenTrabajo_Integrante moi 
						   Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						   Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						   Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
						 );
		Set @contWhile  = 1;
		--INS TB TEMP
		Insert into @TMP_TB_IdPersonaRol (IdPersona,Rol, FechaI, FechaF, EstadoOE, IdGrupoT)
					  	  Select Distinct mgi.Id_Persona, mgi.Rol_Usuario, moi.Fecha_Inicio, moi.Fecha_Fin, moi.Estado, moi.Id_GrupoTrabajo
						  From MT_OrdenTrabajo_Integrante moi 
						  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 

		--INS TB TEMP OTInte PK
		--Insert into @TMP_TB_PkIdOTInte (PkIdOTInte)
		--			  	  Select Distinct moi.Id_OrdenTrabajo_Integrante
		--				  From MT_OrdenTrabajo_Integrante moi 
		--				  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
		--				  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
		--				  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
	    
		WHILE(@NRegistros > 0 AND @contWhile <= @NRegistros) BEGIN
			
			SET @IdPersona =(SELECT IdPersona  FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @Rol       =(SELECT Rol        FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @FechaI=(SELECT FechaI FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @FechaF=(SELECT FechaF FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @IdGrupoT=(SELECT IdGrupoT FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			--SET @PkOTInte  =(SELECT PkIdOTInte FROM @TMP_TB_PkIdOTInte   WHERE Id=@contWhile)

			--UPDATE MT_OrdenTrabajo_Integrante  SET Id_Persona = @IdPersona, Rol = @Rol 
			--WHERE Id_Orden_Trabajo = @pId_OrdenTrabajo And Id_OrdenTrabajo_Integrante = @PkOTInte 

			 Insert into MT_OrdenTrabajo_Integrante(Id_Orden_Trabajo, Id_Persona, Rol, Fecha_Inicio, Fecha_Fin, Estado, Id_GrupoTrabajo) 
			  values (@nuevaOrden, @IdPersona, @Rol, @FechaI, @FechaF, 'A', @IdGrupoT)

			SET @contWhile=@contWhile+1;
		END

		

--	UPDATE MT_OrdenTrabajo_Integrante 
--	SET Id_Persona = i.Id_Persona, 
--    Rol = i.Rol_Usuario 
--	FROM (
--    select DISTINCT mgi.Id_Persona, mgi.Rol_Usuario
--		from MT_OrdenTrabajo_Integrante moi 
--		inner join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
--		inner join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
--		where moi.Id_Orden_Trabajo = @id_ordentrabajo  ) i  --) i and moi.Id_GrupoTrabajo = @id_grupotrabajo
--where MT_OrdenTrabajo_Integrante.Id_Orden_Trabajo = @id_ordentrabajo  -- and MT_OrdenTrabajo_Integrante.Id_GrupoTrabajo = @id_grupotrabajo



	--select moi.*, mgt.Nombre, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, mtd.Descripcion as Descripcion_Rol, mot.Codigo_Cliente 
	--from MT_OrdenTrabajo_Integrante moi
	--inner join MT_OrdenTrabajo mot on moi.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
	--inner join MT_GrupoTrabajo mgt on moi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	--inner join [SADATABASE]..[DBA].rh_persona p on moi.Id_Persona = p.id_persona
	--inner join MT_TablaDetalle mtd on moi.Rol= mtd.Id_TablaDetalle
	--where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo  --and moi.Id_GrupoTrabajo = @id_grupotrabajo



END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMt_OrdenTrabajoMedida]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMt_OrdenTrabajoMedida]
	-- Add the parameters for the stored procedure here
	 @id_titpotrabajo int,
	 @id_orden int,
	 @usuario varchar(10),
	  @empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here

	IF NOT EXISTS(SELECT mot.* from MT_OrdenTrabajo_Medida mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	--inner join MT_Items mi on mot.Id_Item = mi.Id_ItemTipoTrabajo
	inner join MT_Tipo_Trabajo_Medida mi on mot.Id_Tipo_Trabajo_Medida = mi.Id_Tipo_trabajo_Medida
    where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden)
	BEGIN
	INSERT INTO MT_OrdenTrabajo_Medida(Id_Orden_Trabajo, Id_Tipo_Trabajo_Medida, EstadoAct)(select mot.Id_OrdenTrabajo, ma.Id_Tipo_trabajo_Medida, mot.EstadoEditOrden 
	from MT_OrdenTrabajo mot
	inner join MT_TipoTrabajo mtp on mot.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	--inner join MT_Items ma on mtp.Id_TipoTrabajo = ma.Id_TipoTrabajo
	inner join MT_Tipo_Trabajo_Medida ma on mtp.Id_TipoTrabajo = ma.Id_Tipo_Trabajo
	where mot.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mot.Id_OrdenTrabajo = @id_orden)

	--declare @id_ordentrabajo_medida varchar(10);

	--Set @id_ordentrabajo_medida = (select Id_Orden_Trabajo_Medida from MT_OrdenTrabajo_Medida mtom 
	--inner join MT_OrdenTrabajo mot on mtom.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
	--where mot.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mot.Id_OrdenTrabajo = @id_orden)


	--update MT_OrdenTrabajo_Medida set Id_Usuario = @usuario where Id_Orden_Trabajo_Medida = @id_ordentrabajo_medida

	DECLARE @NRegistros Int, @contWhile int;

		--Variables @TMP_TB_IdEquipo
		

		Set @contWhile  = 0;
		Set @NRegistros = (Select Count(Id_Orden_Trabajo_Medida)--, mgi.Rol_Usuario)
						   from MT_OrdenTrabajo_Medida mtom 
						   inner join MT_OrdenTrabajo mot on mtom.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
						   where mot.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mot.Id_OrdenTrabajo = @id_orden);


		WHILE(@NRegistros > 0 AND @contWhile <= @NRegistros) BEGIN
			
			

			UPDATE MT_OrdenTrabajo_Medida  SET Id_Usuario = @usuario 
			WHERE Id_Orden_Trabajo = @id_orden 

			SET @contWhile=@contWhile+1;
		END


	select mot.*, mo.Codigo_Cliente, ma.Descripcion
	from MT_OrdenTrabajo_Medida mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	--inner join MT_Items ma on mot.Id_Item = ma.Id_ItemTipoTrabajo
	inner join MT_Tipo_Trabajo_Medida ma on mot.Id_Tipo_Trabajo_Medida = ma.Id_Tipo_trabajo_Medida
	--left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Tipo
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.items it on it.cod_item = ma.Id_Item
	INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS  u on mot.Id_Usuario = u.user_id
	--inner join openquery([SADATABASE], 'select * from tb_data_seg_def_user') u on mot.Id_Usuario = u.user_id
	where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden and u.cia_codigo = @empresa
		
		END
	ELSE 
	BEGIN

	select mot.*, mo.Codigo_Cliente, ma.Descripcion
	from MT_OrdenTrabajo_Medida mot
	inner join MT_OrdenTrabajo mo on mot.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
	inner join MT_TipoTrabajo mtp on mo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
	--inner join MT_Items ma on mot.Id_Item = ma.Id_ItemTipoTrabajo
	inner join MT_Tipo_Trabajo_Medida ma on mot.Id_Tipo_Trabajo_Medida = ma.Id_Tipo_trabajo_Medida
	--left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mot.Tipo
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.items it on it.cod_item = ma.Id_Item
	--inner join openquery([SADATABASE], 'select * from tb_data_seg_def_user') u on mot.Id_Usuario = u.user_id
	INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS  u on mot.Id_Usuario = u.user_id
	where mo.Id_tipo_trabajo_ejecutado = @id_titpotrabajo and mo.Id_OrdenTrabajo = @id_orden and mot.EstadoAct in ('A', 'I') and u.cia_codigo = @empresa
	

	END
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaInsMt_ProyectoActividadEgreso]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaInsMt_ProyectoActividadEgreso]
	-- Add the parameters for the stored procedure here
	 --@id_titpotrabajo int,
	 --@id_proyecto int
@idProyectoactividad int,
@empresa varchar(2)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here
	
	--IF NOT EXISTS(SELECT mpae.*, mp.Id_TipoTrabajo from MT_Proyecto_ActividadEgreso mpae
	--inner join MT_Proyecto_Actividad mpa on mpae.Id_Proyecto_Actividad = mpa.Id_Proyecto_Actividad
	--inner join MT_Proyecto mp on mpa.Id_Proyecto = mp.Id_Proyecto
	--inner join MT_TipoTrabajo mtp on mp.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	--left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mpae.Tipo
	--where mp.Id_TipoTrabajo = @id_titpotrabajo and mp.Id_Proyecto = @id_proyecto)

	--BEGIN

	--select * from MT_Proyecto mp
	--inner join MT_TipoTrabajo mtp on mp.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	--inner join MT_Items mi on mp.Id_TipoTrabajo = mi.Id_TipoTrabajo order by Id_Proyecto desc
	
	--INSERT INTO MT_Proyecto_ActividadEgreso(Id_Proyecto_Actividad, Id_Item)(select mtpa.Id_Proyecto_Actividad, mi.Id_Item
	--from MT_Proyecto_Actividad mtpa	
	--inner join MT_Proyecto mp on mtpa.Id_Proyecto = mp.Id_Proyecto 
	--inner join MT_TipoTrabajo mtp on mp.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	--inner join MT_Items mi on mp.Id_TipoTrabajo = mi.Id_TipoTrabajo
	--where mp.Id_TipoTrabajo = 4 and mp.Id_Proyecto = 3)
	
	--select mpae.*--, ma.Descripcion , it.Descripcion as ItemsItems, t.Descripcion as DescripcionTipo
	--from MT_Proyecto_ActividadEgreso mpae
	--inner join MT_Proyecto_Actividad mpa on mpae.Id_Proyecto_Actividad = mpa.Id_Proyecto_Actividad
	--inner join MT_Proyecto mp on mpa.Id_Proyecto = mp.Id_Proyecto
	--inner join Mt_Actividad ma on mpa.Id_Actividad = ma.Id_Actividad
	--left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mpae.Tipo
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.items it on it.cod_item = mpae.Id_Item
	--where mp.Id_TipoTrabajo = @id_titpotrabajo and mp.Id_Proyecto = @id_proyecto

	--END
	--ELSE 
	--BEGIN

	select mpae.*, ma.Descripcion as DescripcionActividad , it.Descripcion as ItemsItems, t.Descripcion as DescripcionTipo
	from MT_Proyecto_ActividadEgreso mpae
	inner join MT_Proyecto_Actividad mpa on mpae.Id_Proyecto_Actividad = mpa.Id_Proyecto_Actividad
	--inner join MT_Proyecto mp on mpa.Id_Proyecto = mp.Id_Proyecto
	inner join Mt_Actividad ma on mpa.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = mpae.Tipo
	inner join [SADATABASE]..[DBA].[items] it on it.cod_item = mpae.Id_Item
	--where mp.Id_TipoTrabajo = @id_titpotrabajo and mp.Id_Proyecto = @id_proyecto	
	where mPAE.Id_Proyecto_Actividad = @idProyectoactividad and it.cia_codigo = @empresa

	--END
	
END

GO
/****** Object:  StoredProcedure [dbo].[Sp_Quimipac_ConsultaInsMt_ProyectoActvidad]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ericka,Loor>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_Quimipac_ConsultaInsMt_ProyectoActvidad]
	-- Add the parameters for the stored procedure here
	 @id_titpotrabajo int,
	 @id_orden int, 
	 @empresa varchar(2)
AS
BEGIN
	    -- Insert statements for procedure here

	IF NOT EXISTS(SELECT * from MT_Proyecto_Actividad pa
	inner join MT_Proyecto p on pa.Id_Proyecto = p.Id_Proyecto
	inner join MT_TipoTrabajo mtp on p.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	inner join MT_Actividad ma on pa.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = pa.Estado where p.Id_TipoTrabajo = @id_titpotrabajo and p.Id_Proyecto = @id_orden and p.Id_Empresa = @empresa)
	
	BEGIN
	
	INSERT INTO MT_Proyecto_Actividad(Id_Proyecto, Id_Actividad, EstadoAct)(select pa.Id_Proyecto, ma.Id_Actividad, pa.Estado 
	from MT_Proyecto pa
	inner join MT_TipoTrabajo mtp on pa.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	inner join MT_Actividad ma on mtp.Id_TipoTrabajo = ma.Id_TipoTrabajo
	where pa.Id_TipoTrabajo = @id_titpotrabajo and pa.Id_Proyecto = @id_orden and pa.Id_Empresa = @empresa)

	UPDATE MT_Proyecto_Actividad  SET Estado = 67
								WHERE Id_Proyecto = @id_orden


	select pa.*, p.Codigo_Cliente, ma.Descripcion, t.Descripcion as DescripcionEstado from MT_Proyecto_Actividad pa
	inner join MT_Proyecto p on pa.Id_Proyecto = p.Id_Proyecto
	inner join MT_TipoTrabajo mtp on p.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	inner join MT_Actividad ma on pa.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = pa.Estado
	where p.Id_TipoTrabajo = @id_titpotrabajo and p.Id_Proyecto = @id_orden and p.Id_Empresa  = @empresa
		
		END
	ELSE 

	BEGIN
	select pa.*, p.Codigo_Cliente, ma.Descripcion, t.Descripcion as DescripcionEstado from MT_Proyecto_Actividad pa
	inner join MT_Proyecto p on pa.Id_Proyecto = p.Id_Proyecto
	inner join MT_TipoTrabajo mtp on p.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	inner join MT_Actividad ma on pa.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = pa.Estado
	where p.Id_TipoTrabajo = @id_titpotrabajo and p.Id_Proyecto = @id_orden and p.Id_Empresa = @empresa
	END

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMenu]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================
--	 Author:		<Sandro Yagual>
--	 ALTER date:	<18-11-2018>
--	 UPD: 15-12-2018
-- ========================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMenu]
@padre int
AS
BEGIN
	If @padre = 0 Begin
		Select Id_Menu
			   ,Menu
			   ,Padre
		       ,Orden
			   ,Nivel_Profundidad
			   ,Estado
			   ,isnull(Action,'') Action
			   ,isnull(controlador,'') controlador
			   ,Icono
			   From Menu
			   Where Estado = 'A' And Nivel_Profundidad = 1
			   Order By Orden Asc
	End
	Else Begin
		 Select Id_Menu
			   ,Menu
			   ,Padre	
		       ,Orden
			   ,Nivel_Profundidad
			   ,Estado
			   ,isnull(Action,'') Action
			   ,isnull(controlador,'') controlador
			   ,Icono
			   From Menu
			   Where Estado = 'A' And Padre = @padre
			   Order By Orden Asc
	 END
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Actividad]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Actividad] 
	@id_tipotrabajo int,
	@id_empresa varchar(2)
AS
BEGIN

select a_hijo.*,ISNULL(a_padre.Codigo, '') as codigoPadre,ISNULL( a_padre.Descripcion ,'') as descripcionPadre, mttrabajo.Descripcion as Descripcion_TTrabajo, mtabladet.Descripcion as Descripcion_TActividad
	from MT_Actividad a_hijo
	left join MT_Actividad a_padre on a_padre.Id_Actividad = a_hijo.Id_ActividadPadre
	inner join MT_TipoTrabajo mttrabajo on a_hijo.Id_TipoTrabajo = mttrabajo.Id_TipoTrabajo
	inner join MT_TablaDetalle mtabladet on a_hijo.Tipo = mtabladet.Id_TablaDetalle
	inner join MT_Servicio ms on mttrabajo.Id_Servicio = ms.Id_Servicio
	where a_hijo.Id_TipoTrabajo = @id_tipotrabajo and ms.Id_Empresa = @id_empresa
	order by a_hijo.Orden asc

 --	IF NOT EXISTS(select a_hijo.*,ISNULL(a_padre.Codigo, '') as codigoPadre,ISNULL( a_padre.Descripcion ,'') as descripcionPadre, mttrabajo.Descripcion as Descripcion_TTrabajo, mtabladet.Descripcion as Descripcion_TActividad
	--from MT_Actividad a_hijo
	--left join MT_Actividad a_padre on a_padre.Id_Actividad = a_hijo.Id_ActividadPadre
	--inner join MT_TipoTrabajo mttrabajo on a_hijo.Id_TipoTrabajo = mttrabajo.Id_TipoTrabajo
	--inner join MT_TablaDetalle mtabladet on a_hijo.Tipo = mtabladet.Id_TablaDetalle
	--inner join MT_Servicio ms on mttrabajo.Id_Servicio = ms.Id_Servicio
	--where a_hijo.Id_TipoTrabajo = @id_tipotrabajo and ms.Id_Empresa = @id_empresa)
	--BEGIN
	--		select mtp.* from MT_TipoTrabajo mtp
	--		inner join Mt_Servicio s on mtp.Id_Servicio = s.Id_Servicio 			
	--		where mtp.Id_TipoTrabajo = @id_tipotrabajo and s.Id_Empresa = @id_empresa
	--	END
	--ELSE
	--BEGIN
	--select a_hijo.*,ISNULL(a_padre.Codigo, '') as codigoPadre,ISNULL( a_padre.Descripcion ,'') as descripcionPadre, mttrabajo.Descripcion as Descripcion_TTrabajo, mtabladet.Descripcion as Descripcion_TActividad
	--from MT_Actividad a_hijo
	--left join MT_Actividad a_padre on a_padre.Id_Actividad = a_hijo.Id_ActividadPadre
	--inner join MT_TipoTrabajo mttrabajo on a_hijo.Id_TipoTrabajo = mttrabajo.Id_TipoTrabajo
	--inner join MT_TablaDetalle mtabladet on a_hijo.Tipo = mtabladet.Id_TablaDetalle
	--inner join MT_Servicio ms on mttrabajo.Id_Servicio = ms.Id_Servicio
	--where a_hijo.Id_TipoTrabajo = @id_tipotrabajo and ms.Id_Empresa = @id_empresa
	--END

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Actividad_OrdenTipoTrabajo]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<VQ>
-- ALTER date: <11-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Actividad_OrdenTipoTrabajo] 

AS
BEGIN
 SELECT ma.*, ma.Descripcion as DescripcionActividad
  FROM [BD_QUIMIPAC].[dbo].[MT_Actividad] ma
  inner join MT_TipoTrabajo mtt on ma.Id_TipoTrabajo = mtt.Id_TipoTrabajo
  inner join MT_TablaDetalle mtd on mtt.Tipo = mtd.Id_TablaDetalle
  where mtt.Tipo = 2

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Actividad_Proyecto]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <11-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Actividad_Proyecto] 

AS
BEGIN
 SELECT ma.*  FROM [BD_QUIMIPAC].[dbo].[MT_Actividad] ma
  inner join MT_TipoTrabajo mtt on ma.Id_TipoTrabajo = mtt.Id_TipoTrabajo
  inner join MT_TablaDetalle mtd on mtt.Tipo = mtd.Id_TablaDetalle
  where mtt.Tipo = 1

END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Campamento]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <14-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Campamento] 
@empresa varchar(2)
AS
BEGIN
	
	select * from MT_Campamento where Estado in ('A', 'I')and Id_Empresa = @empresa


END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_CampamentoGeneral]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_CampamentoGeneral] 
@empresa varchar(2)
AS
BEGIN
	
	select * from MT_Campamento c
	left outer join [SADATABASE]..[DBA].[TBCON_MCIAS] as empresa on c.Id_Empresa = empresa.CIA_CODIGO
	where Estado in ('A', 'I') and Id_Empresa = @empresa

END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Contrato]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Contrato] 
@empresa varchar(2)
AS
BEGIN
	
	--SELECT * FROM MT_Contrato WHERE Id_Empresa = @empresa

	--Select Count(cont.Id_Contrato) as CountID,cont.Id_Contrato,cont.Nombre, cont.Codigo_Cliente 
	--from MT_Contrato cont
	--Inner Join MT_Contrato_Parametro cp on cont.Id_Contrato = cp.Id_Contrato
	--WHERE CONT.Id_Empresa = @empresa and cont.Estado = 75
	--Group by cont.Id_Contrato,cont.Nombre, cont.Codigo_Cliente 
	--Having Count(cont.Id_Contrato)=3 

	Select cont.Id_Contrato, cont.Nombre, cont.Codigo_Cliente, Id_Cliente, tipo, Codigo_Interno, Estado, Referencia
	from MT_Contrato cont
	--Inner Join MT_Contrato_Parametro cp on cont.Id_Contrato = cp.Id_Contrato
	WHERE CONT.Id_Empresa = @empresa and cont.Estado != 1160 
	--Group by cont.Id_Contrato,cont.Nombre, cont.Codigo_Cliente 

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Contrato_Combo_Filtro]
    @pComboParametro Varchar(50),
	@empresa varchar(2)
AS
BEGIN
	If @pComboParametro = 'Tipo' Begin
	select td.Id_TablaDetalle, td.Descripcion from MT_TablaDetalle td where td.Id_Tabla = 63 and td.Estado = 'A'
	order by orden asc
	End
	Else If @pComboParametro = 'Cliente' Begin
		SELECT clientes.cod_cli, clientes.nom_cli
	FROM [SADATABASE]..[DBA].[clientes] as clientes
	where cia_codigo = @empresa and clientes.abreviatura_cliente is not null

	End
	Else If @pComboParametro = 'Contrato_Padre' Begin
		select cp.Id_Contrato, cp.Nombre from MT_Contrato cp where cp.Id_Contrato in (select DISTINCT Contrato_Padre FROM MT_Contrato) and cp.Id_Empresa = '02'

	End
	Else If @pComboParametro = 'Estado' Begin
		select tde.Id_TablaDetalle, tde.Descripcion from MT_TablaDetalle tde
	where tde.Id_Tabla = 41 and tde.Estado = 'A'
	End
	Else If @pComboParametro = 'Unidad_Negocio' Begin
		SELECT lineas.codigo, lineas.descripcion FROM [SADATABASE]..[DBA].[lineas] as lineas
	where cia_codigo = @empresa

	End
	Else If @pComboParametro = 'Categoria' Begin
		SELECT categorias.cod_servicio, categorias.nombre FROM [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] as categorias 
	where cia_codigo = @empresa

	End
	Else If @pComboParametro = 'Subcategoria' Begin
		SELECT subcategorias.codcen, subcategorias.nombre 
	FROM [SADATABASE]..[DBA].[centro_consumo] as subcategorias 
	where cia_codigo = @empresa

	End
	Else If @pComboParametro = 'Referencia' Begin
		select cpr.Id_Contrato, cpr.Nombre from MT_Contrato cpr where cpr.Id_Contrato in (select DISTINCT Referencia FROM MT_Contrato) and cpr.Id_Empresa = @empresa
	
	End
	Else If @pComboParametro = 'Localidad' Begin
	SELECT codigo_loc, descripcion
	FROM [SADATABASE]..[DBA].[LOCALIDAD]  
	where cia_codigo = @empresa 
	 
	End
	Else If @pComboParametro = 'Responsable' Begin
	SELECT id_persona,	primer_nombre as Nombres_Completos 
	FROM [SADATABASE]..[DBA].[rh_persona] as Personas where estado = '001'
	 
	End
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Contrato_TipoEstado]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Contrato_TipoEstado] 
@empresa varchar(2),
@tipo int
AS
BEGIN
	--144	CTR	CONTRATO 	--145	LIC	LICITACION 	--146	PRO	PROSPECTO 	--1152	PRY	PROYECTO
	if(@tipo = 144 or @tipo = 1152 or @tipo = 1165)	
	begin
	select td.Id_TablaDetalle, td.Id_Tabla, td.Codigo, td.Descripcion, td.Estado, td.Id_Padre from MT_TablaDetalle td
	where td.Id_TablaDetalle in (75,151) and td.Estado = 'A' order by Orden asc
	end
	else
	begin
	if(@tipo = 145 or @tipo = 146 OR @tipo = 1167 OR @tipo = 1168)
	begin
	
	select td.Id_TablaDetalle, td.Id_Tabla, td.Codigo, td.Descripcion, td.Estado, td.Id_Padre from MT_TablaDetalle td
	where td.Id_TablaDetalle in (1158,1159,1156,1157,76,77) and td.Estado = 'A' --97,1151,
	order by Orden asc
	end
	end
	--select * from mt_tabladetalle where id_tabla = 41
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Contrato_TipoPadre]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Contrato_TipoPadre] 
@empresa varchar(2),
@tipo int,
@cliente varchar(10)
AS
BEGIN
--db.MT_Contrato.Where(x => x.Id_Empresa == empresa && x.Estado != 77 && x.Id_Cliente == id_Cliente)
	if(@tipo = 144 )	
	
	select c.Id_Contrato, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,c.Contrato_Padre, c.Id_Cliente, mtd.Descripcion from MT_Contrato c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.tipo in (144, 145,146,1152) and c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75

	else
	if(@tipo = 145 or @tipo = 146)
	select c.Id_Contrato, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,c.Contrato_Padre, c.Id_Cliente, mtd.Descripcion from MT_Contrato c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo not in (144,145,146,1152)
	else
	if(@tipo = 1152)
	select c.Id_Contrato, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,c.Contrato_Padre, c.Id_Cliente, mtd.Descripcion from MT_Contrato c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo in (144,145,1152)
	else
	if(@tipo = 1165)
	select c.Id_Contrato, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,c.Contrato_Padre, c.Id_Cliente, mtd.Descripcion from MT_Contrato c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo in (144, 145,146,1152)

	--proyecto 144, 145, 1152
	--cuando hay licitacion y prospecto no hay padre 
	--select * from MT_TablaDetalle where Id_Tabla = 63 


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoAlerta]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoAlerta] 
@idContrato int,
@empresa varchar(2)
AS
BEGIN
	
	
	select ca.*,  c.Codigo_Cliente as CodigoC  from MT_ContratoAlerta ca 

	inner join MT_Contrato c on c.Id_Contrato = ca.Id_Contrato where ca.Estado in ('A', 'I') and ca.Id_Contrato = @idContrato and c.Id_Empresa = @empresa
	
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoDocumentado]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoDocumentado] 
@idContrato int,
@empresa varchar(2)
AS
BEGIN


	select cd.*, c.Nombre as NombreContrato , c.Codigo_Cliente as CodigoC , t.Descripcion as DescripcionTipo 
	from MT_Contrato_Documentado cd 
     inner join MT_TablaDetalle t on t.Id_TablaDetalle = cd.Tipo
	inner join MT_Contrato c on c.Id_Contrato = cd.Id_Contrato where cd.Estado in ('A', 'I') and cd.Id_Contrato = @idContrato and c.Id_Empresa = @empresa
    
	

	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoFiscalizador]    Script Date: 04/10/2020 22:26:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoFiscalizador] 
@idProyectoContrato int,
@tipo varchar(20),
@empresa varchar(2)
AS
BEGIN
	
	IF @tipo = 'Contrato'
	BEGIN

	select f.* , c.Codigo_Cliente as Codigo
	from MT_Fiscalizador f 
	inner join MT_Contrato c on c.Id_Contrato = f.Id_Proyecto_Contrato
	 where f.Estado in ('A', 'I') and f.Id_Proyecto_Contrato = @idProyectoContrato and f.Tipo = @tipo and c.Id_Empresa = @empresa
	 END

	 ELSE
	 BEGIN
			 IF @tipo = 'Prospecto'
			BEGIN

			select f.* , c.Codigo_Cliente as Codigo
			from MT_Fiscalizador f 
			inner join MT_Prospecto c on c.Id_Prospecto = f.Id_Proyecto_Contrato
			 where f.Estado in ('A', 'I') and f.Id_Proyecto_Contrato = @idProyectoContrato and f.Tipo = @tipo and c.Id_Empresa = @empresa
			 END
			 ELSE
	
			BEGIN
					select f.*, p.Codigo_Cliente as Codigo
					from MT_Fiscalizador f 
					inner join MT_Proyecto p on p.Id_Proyecto = f.Id_Proyecto_Contrato
					 where f.Estado in ('A', 'I') and f.Id_Proyecto_Contrato = @idProyectoContrato and f.Tipo = @tipo and p.Id_Empresa = @empresa
			END

			END
	 
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoGeneral] 
@empresa varchar(2)
AS
BEGIN


	select C.Id_Contrato, C.Id_Empresa, c.Codigo_Cliente, c.Codigo_Interno, c.Nombre, c.Estado, c.Fecha_Inicio, c.Fecha_Fin, c.tipo, c.Subcategoria, c.Responsable, c.Localidad,
	c.Id_Linea, c.Id_Cliente, c.Fecha_registro, c.Contrato_Padre, c.Categoria, c.Referencia, c.Secuencial, c.Observaciones, c.monto, c.Dia_Plazo, c.costo, c.Codigo_Interno_Ant, c.Codigo_interno_padre, c.Id_Usuario,
	l.descripcion as lineaContrato, cl.nom_cli as nombreCliente,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
    (select Top(1) Fecha_Prorroga from MT_Contrato_Prorroga where C.Id_Contrato = Id_Contrato) as Fecha_proIni,
	(select Top(1) MT_Contrato_Prorroga.Dia_Prorroga from MT_Contrato_Prorroga where C.Id_Contrato = Id_Contrato) as Dia_ProIn,
	'' as Origen, referencia.Nombre as nom_Referencia, Localidad.descripcion as nom_Localidad, persona.primer_nombre as Nombres_Completos , cl.abreviatura_cliente 

	from MT_Contrato C
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	left outer join MT_Contrato referencia on c.Referencia = referencia.Id_Contrato and referencia.Id_Empresa = @empresa
	left outer join [SADATABASE]..[DBA].[LOCALIDAD] localidad on c.Localidad = localidad.codigo_loc and localidad.cia_codigo = @empresa
	inner join [SADATABASE]..[DBA].[rh_persona] persona on c.Responsable = persona.id_persona
	--inner join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado
	--inner join MT_TablaDetalle tipo on tipo.Id_TablaDetalle = C.tipo
	where c.Id_Empresa = @empresa and c.Estado !=1160 --and cl.cia_codigo = @empresa and l.cia_codigo = @empresa  --and subcateg.cia_codigo = '02' and categ.cia_codigo = '02'	
	order by Id_Contrato desc

	
	
	
	
	

	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoGeneral_Prueba]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoGeneral_Prueba] 
@empresa varchar(2)
AS
BEGIN
--ggg



	DECLARE @SQL NVARCHAR(max)
	
	


	SET @SQL = N'
		;WITH cte AS
(
   SELECT 
         ROW_NUMBER() OVER (PARTITION BY c.Id_Contrato ORDER BY c.Id_Contrato DESC) AS rn,
   c.Id_Contrato, c.Id_Empresa, c.Id_Cliente, c.Fecha_Inicio as Fecha_Inicio, c.Fecha_Fin, c.Codigo_Interno, c.Codigo_Cliente, c.Id_Usuario, c.Id_Linea, c.Categoria, c.Subcategoria, c.Nombre, c.Estado, c.Dia_Plazo, c.tipo, c.Contrato_Padre, c.Valor_Referencial, c.monto, c.costo, c.Responsable, c.Secuencial, c.Codigo_Interno_Ant, c.Observaciones, c.Codigo_interno_padre , (select top(1)categ.nombre from [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ where categ.cod_linea = c.Id_Linea and categ.cia_codigo = ('+@empresa+')) as nombre_Categoria,
	(select top(1)subcateg.nombre from [SADATABASE]..[DBA].[centro_consumo] subcateg where subcateg.quimi_linea = c.Id_Linea and subcateg.cia_codigo = ('+@empresa+')) as nombre_Subcategoria,
	(select top(1)cl.nom_cli from [SADATABASE]..[DBA].[clientes] cl where cl.cod_cli = c.Id_Cliente and cl.cia_codigo = ('+@empresa+')) as nombreCliente,
	(select top(1)l.descripcion from [SADATABASE]..[DBA].[lineas] l where l.codigo = c.Id_Linea and l.cia_codigo = ('+@empresa+')) as lineaContrato, t.Descripcion as descripcion 
	from MT_Contrato c, [SADATABASE]..[DBA].[TBCON_MCIAS] e, MT_TablaDetalle t
		where c.Id_Empresa = ('+@empresa+') and c.Estado in (75,76,77,97)
		
		--order by fa.ORDEN
		
)

SELECT *
FROM cte
WHERE rn = 1
order by Id_Contrato DESC'

EXEC sp_executesql @SQL

	
	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoOrdenPlanilla]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoOrdenPlanilla] 
@empresa varchar(2)
AS
BEGIN
	
	select DISTINCT c.Id_contrato, C.Codigo_Cliente 
	from MT_Contrato c
	inner join MT_OrdenTrabajo o on c.Id_Contrato = o.Id_contrato
	where c.Id_Empresa = @empresa
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoParametro]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoParametro] 
@idContrato int,
@empresa varchar(2)
AS
BEGIN


	select cd.*, c.Nombre as NombreContrato , c.Codigo_Cliente as CodigoC, mtd.Descripcion as Tipo_Datos
	from MT_Contrato_Parametro cd 
	inner join MT_Contrato c on c.Id_Contrato = cd.Id_Contrato
	inner join MT_TablaDetalle mtd on cd.Tipo_Dato = mtd.Id_TablaDetalle
	 where cd.Estado in ('A', 'I') and cd.Id_Contrato = @idContrato and c.Id_Empresa = @empresa
    
	

	 
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ContratoProrroga]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ContratoProrroga] 
@idContrato int,
@empresa varchar(2)
AS
BEGIN


	select cd.*, c.Nombre as NombreContrato , c.Codigo_Cliente as CodigoC
	from MT_Contrato_Prorroga cd 
	inner join MT_Contrato c on c.Id_Contrato = cd.Id_Contrato
	 where cd.Estado in ('A', 'I') and cd.Id_Contrato = @idContrato and c.Id_Empresa = @empresa
    
	

	 
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_DetallePlanilla]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_DetallePlanilla] 
	@id_planilla int,
	@empresa varchar(2)
AS
BEGIN


	select mpldetalle.* --mi.descripcion  
	from MT_Planilla_Detalle mpldetalle
	inner join MT_Planilla mpl on mpldetalle.Id_Planilla = mpl.Id_Planilla
	--inner join [SADATABASE]..[DBA].[items] mi on mpldetalle.Id_Item = mi.cod_item
	where mpldetalle.Id_Planilla = @id_planilla --and mi.cia_codigo = @empresa


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EditEntregaOrdenCkeck]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EditEntregaOrdenCkeck]
@id_entregaorden int,
@empresa varchar(2)
AS
BEGIN
		
	select eo.Id_Entrega_Orden_Trabajo, eo.Id_Cliente, o.Id_OrdenTrabajo, o.Id_entrega_orden_trabajo
	from MT_EntregaOrden_Trabajo eo
	left outer join MT_OrdenTrabajo o on eo.Id_Entrega_Orden_Trabajo = o.Id_entrega_orden_trabajo
	where o.Id_entrega_orden_trabajo = @id_entregaorden and eo.Id_Empresa = @empresa
	

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EntregaOrdenTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EntregaOrdenTrabajo] 
	-- Add the parameters for the stored procedure here
	@id_usuario varchar(20),
	@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	select meot.*, empresas.CIA_DESCRIPCION, clientes.nom_cli 
	from MT_EntregaOrden_Trabajo meot
	inner join [SADATABASE]..[DBA].[TBCON_MCIAS] as empresas on meot.Id_Empresa = empresas.CIA_CODIGO
	inner join [SADATABASE]..[DBA].[clientes] as clientes on meot.Id_Cliente = clientes.cod_cli
	--inner join [SADATABASE]..[DBA].[tb_data_seg_def_user] as usr on  meot.Usuario = usr.user_id
	inner join openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr on meot.Usuario = usr.user_id
	where meot.Usuario = @id_usuario and clientes.cia_codigo = @empresa and usr.cia_codigo = @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_Equipo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_Equipo] 
	
AS
BEGIN

	SELECT * FROM MT_Equipo as Equipos where Estado = 'A'

END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EquipoGrupoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<VanessaQuintana>
-- ALTER date: <13-09-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EquipoGrupoTrabajo] 
	@id_grupotrabajo int
AS
BEGIN
 	select mge.*, mgt.Nombre as Nom_GrupoT, me.Descripcion as Nom_Equipo from MT_GrupoTrabajoEquipo mge
	inner join MT_GrupoTrabajo mgt on mge.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	inner join MT_Equipo me on mge.Id_Equipo = me.Id_Equipo
	where mge.Id_GrupoTrabajo = @id_grupotrabajo

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12/10/2018>,
--				Upd 16-10-2018 12:53  --> SY
-- Description:	<Description,,>
-- =============================================

--sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo 2
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo]
	@pid_ordentrabajo int
	--@id_grupotrabajo int
	
AS
BEGIN
		--Obtiene el IdEquipo
		DECLARE @TMP_TB_IdEquipo TABLE( Id int identity(1,1) not null primary key
								       ,IdEquipo int	)
		--Obtiene La PK de IdOTEquipo
        DECLARE @TMP_TB_IdOrdTrabEquipo TABLE( Id int identity(1,1) not null primary key
								              ,IdOrdTrabEquipo int)

		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;
		
		--Variables tmp de Id
		DECLARE @IdEquipo Int, @Pk_OTE Int;

		Set @NRegistros =(SELECT count(Distinct mge.Id_Equipo) AS IdEquipo
					      FROM MT_OrdenTrabajo_Equipo moe 
						  inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
						  where moe.Id_Orden_Trabajo = @pid_ordentrabajo
						 );
		Set @contWhile = 1;

		--IdEquipo
		Insert into @TMP_TB_IdEquipo (IdEquipo)
			SELECT DISTINCT mge.Id_Equipo AS IdEquipo
			FROM MT_OrdenTrabajo_Equipo moe 
			inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
			where moe.Id_Orden_Trabajo = @pid_ordentrabajo


		--Pk OTEq
		Insert into @TMP_TB_IdOrdTrabEquipo (IdOrdTrabEquipo)
			SELECT DISTINCT moe.Id_OrdenTrabajo_Equipo AS Pk_OTEquipo
			FROM MT_OrdenTrabajo_Equipo moe 
			inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
			where moe.Id_Orden_Trabajo = @pid_ordentrabajo



		WHILE(@NRegistros>0 AND @contWhile<=@NRegistros) BEGIN

			
			SET @IdEquipo=(SELECT IdEquipo FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			SET @Pk_OTE=(SELECT IdOrdTrabEquipo FROM @TMP_TB_IdOrdTrabEquipo WHERE Id=@contWhile)

			  Update MT_OrdenTrabajo_Equipo Set Id_Equipo = @IdEquipo
			  Where  Id_Orden_Trabajo = @pid_ordentrabajo And Id_OrdenTrabajo_Equipo = @Pk_OTE;
			  --select @IdEquipo
			SET @contWhile=@contWhile+1;

		END

		--SELECT * FROM @TMP_TB_IdEquipo
		
		select moe.*, mgt.Nombre, e.Descripcion, mot.Codigo_Cliente 
		from MT_OrdenTrabajo_Equipo moe
		inner join MT_OrdenTrabajo mot on moe.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
		inner join MT_GrupoTrabajo mgt on moe.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
		inner join MT_Equipo e on moe.Id_Equipo = e.Id_Equipo
		where moe.Id_Orden_Trabajo = @pid_ordentrabajo --and moi.Id_GrupoTrabajo = @id_grupotrabajo



END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EquiposProyecto]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12/10/2018>,
--				Upd 16-10-2018 12:53  --> SY
-- Description:	<Description,,>
-- =============================================

--sp_Quimipac_ConsultaMT_EquiposOrdenTrabajo 2
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EquiposProyecto]
	@pid_ordentrabajo int
	--@id_grupotrabajo int
	
AS
BEGIN
		--Obtiene el IdEquipo
		DECLARE @TMP_TB_IdEquipo TABLE( Id int identity(1,1) not null primary key
								       ,IdEquipo int	)
		--Obtiene La PK de IdOTEquipo
        DECLARE @TMP_TB_IdOrdTrabEquipo TABLE( Id int identity(1,1) not null primary key
								              ,IdOrdTrabEquipo int)

		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;
		
		--Variables tmp de Id
		DECLARE @IdEquipo Int, @Pk_OTE Int;

		Set @NRegistros =(SELECT count(Distinct mge.Id_Equipo) AS IdEquipo
					      FROM MT_OrdenTrabajo_Equipo moe 
						  inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
						  where moe.Id_Orden_Trabajo = @pid_ordentrabajo
						 );
		Set @contWhile = 1;

		--IdEquipo
		Insert into @TMP_TB_IdEquipo (IdEquipo)
			SELECT DISTINCT mge.Id_Equipo AS IdEquipo
			FROM MT_OrdenTrabajo_Equipo moe 
			inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
			where moe.Id_Orden_Trabajo = @pid_ordentrabajo


		--Pk OTEq
		Insert into @TMP_TB_IdOrdTrabEquipo (IdOrdTrabEquipo)
			SELECT DISTINCT moe.Id_OrdenTrabajo_Equipo AS Pk_OTEquipo
			FROM MT_OrdenTrabajo_Equipo moe 
			inner join MT_OrdenTrabajo mo on moe.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
			inner join MT_GrupoTrabajoEquipo mge on moe.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
			where moe.Id_Orden_Trabajo = @pid_ordentrabajo



		WHILE(@NRegistros>0 AND @contWhile<=@NRegistros) BEGIN

			
			SET @IdEquipo=(SELECT IdEquipo FROM @TMP_TB_IdEquipo WHERE Id=@contWhile)
			SET @Pk_OTE=(SELECT IdOrdTrabEquipo FROM @TMP_TB_IdOrdTrabEquipo WHERE Id=@contWhile)

			  Update MT_OrdenTrabajo_Equipo Set Id_Equipo = @IdEquipo
			  Where  Id_Orden_Trabajo = @pid_ordentrabajo And Id_OrdenTrabajo_Equipo = @Pk_OTE;
			  --select @IdEquipo
			SET @contWhile=@contWhile+1;

		END

		--SELECT * FROM @TMP_TB_IdEquipo
		
		select moe.*, mgt.Nombre, e.Descripcion, mot.Codigo_Cliente 
		from MT_OrdenTrabajo_Equipo moe
		inner join MT_OrdenTrabajo mot on moe.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
		inner join MT_GrupoTrabajo mgt on moe.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
		inner join MT_Equipo e on moe.Id_Equipo = e.Id_Equipo
		where moe.Id_Orden_Trabajo = @pid_ordentrabajo --and moi.Id_GrupoTrabajo = @id_grupotrabajo



END





GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_EquipoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_EquipoTrabajo]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Equipos.*, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, t.Descripcion as DescripcionTipoEquipo 
	FROM MT_Equipo as Equipos 
	inner join [SADATABASE]..[DBA].[rh_persona] p on Equipos.Id_Persona_asignada = p.id_persona
	inner join MT_TablaDetalle t on  Equipos.Tipo =  t.Id_TablaDetalle
	where Equipos.Estado in ('A','I')





END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Estacion]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <04-09-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Estacion]
	-- Add the parameters for the stored procedure here
@empresa varchar(2)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    -- Insert statements for procedure here
	
 select td_provincia.Codigo as codigoProvincia, td_provincia.Descripcion as descripcionProvincia, td_ciudad.Codigo as codigoCiudad, td_ciudad.Descripcion as descripcionCiudad, se.Nombre, e.Nombre as nombreest, e.Direccion as direccionest, c.nom_cli as cliente, e.Coordenada_X, e.Coordenada_Y, e.Estado, e.Id_Estacion
 from MT_Estacion e
 inner join MT_TablaDetalle td_provincia on td_provincia.Id_TablaDetalle = e.Id_Provincia
 inner join MT_TablaDetalle td_ciudad on td_ciudad.Id_TablaDetalle = e.Id_Ciudad
 inner join MT_Sector se on e.Id_Macrosector = se.Id_Sector
 inner join [SADATABASE]..[DBA].clientes c on c.cod_cli = e.Id_Cliente where e.Estado in ('A', 'I') and c.cia_codigo = @empresa
 order by e.Estado
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_EventoEquipo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_EventoEquipo]
	-- Add the parameters for the stored procedure here
	@id_Equipo int,
	@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	select  mte.*, me.Descripcion 
	from MT_Equipo_Evento mte
	inner join MT_Equipo me on mte.Id_Equipo = me.Id_Equipo
	inner join openquery([SADATABASE], 'select * from tb_data_seg_def_user') u on mte.Id_Usuario = u.user_id
	where mte.Id_Equipo = @id_Equipo and u.cia_codigo = @empresa

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_GrupoIntegranteEquipo]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	
    -- Insert statements for procedure here
	--SELECT DISTINCT mgt.Id_GrupoTrabajo, mgt.Nombre as Nombre_Grupo from MT_GrupoTrabajoIntegrante mgi
	--inner join MT_GrupoTrabajo mgt on mgi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.rh_persona p on mgi.Id_Persona = p.id_persona where mgi.estado in('A')
	
	
	select DISTINCT mg.Id_GrupoTrabajo, mg.Nombre from MT_GrupoTrabajo mg
	inner join MT_GrupoTrabajoIntegrante mgi on mg.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
	inner join MT_GrupoTrabajoEquipo mge on mg.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
	where mg.Estado = 'A' and mgi.Estado = 'A' and mge.Estado = 'A'
	
		
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_GrupoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_GrupoTrabajo]
	-- Add the parameters for the stored procedure here
	@empresa varchar (2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

    -- Insert statements for procedure here
	
		select gt.*, b.nombre as Nombre_Bodega, c.Nombre as Nombre_campamento, t.Descripcion as DescripcionTipo_Grupo , veh.DESCRIPCION
		from Mt_GrupoTrabajo gt
		inner join [SADATABASE]..[DBA].[bodegas] b on b.cod_bod = gt.bodega
		inner join MT_Campamento c on gt.Id_Campamento = c.Id_Campamento
		inner join MT_TablaDetalle t on t.Id_TablaDetalle = gt.Tipo
		left outer join [SADATABASE]..[DBA].[MT_VEHICULO] veh on veh.COD_VEHICULO = gt.Id_Vehiculo
		where gt.Estado in ('A', 'I') and c.Id_Empresa = @empresa and b.cia_codigo = @empresa

	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_IntegranteEquipoOrdenGrupoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
--ALTER PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_IntegranteOrdenGrupoTrabajo]
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_IntegranteEquipoOrdenGrupoTrabajo]

	-- Add the parameters for the stored procedure here
	@empresa varchar(2)
AS
BEGIN
	
    -- Insert statements for procedure here
	--SELECT DISTINCT mgt.Id_GrupoTrabajo, mgt.Nombre as Nombre_Grupo from MT_GrupoTrabajoIntegrante mgi
	--inner join MT_GrupoTrabajo mgt on mgi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	--inner join [SADATABASE]..[DBA].rh_persona p on mgi.Id_Persona = p.id_persona where mgi.estado in('A')
	
	
	--select DISTINCT mg.Id_GrupoTrabajo, mg.Nombre from MT_GrupoTrabajo mg
	--inner join MT_GrupoTrabajoIntegrante mgi on mg.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
	--inner join MT_GrupoTrabajoEquipo mge on mg.Id_GrupoTrabajo = mge.Id_GrupoTrabajo
	

	select distinct mtoint.Codigo_Cliente , MT_GrupoTrabajo.Nombre, mtoint.Id_OrdenTrabajo, b.nombre as bodega, mtpeje.Descripcion
	from MT_GrupoTrabajo
	inner join MT_OrdenTrabajo_Equipo on MT_GrupoTrabajo.Id_GrupoTrabajo = MT_OrdenTrabajo_Equipo.Id_GrupoTrabajo
	inner join MT_OrdenTrabajo_Integrante on MT_GrupoTrabajo.Id_GrupoTrabajo = MT_OrdenTrabajo_Integrante.Id_GrupoTrabajo
	inner join MT_OrdenTrabajo mtoeq on MT_OrdenTrabajo_Equipo.Id_Orden_Trabajo = mtoeq.Id_OrdenTrabajo
	inner join MT_OrdenTrabajo mtoint on MT_OrdenTrabajo_Integrante.Id_Orden_Trabajo = mtoint.Id_OrdenTrabajo
	inner join [SADATABASE]..[DBA].[bodegas] b on b.cod_bod = MT_GrupoTrabajo.bodega
    inner join MT_Campamento c on MT_GrupoTrabajo.Id_Campamento = c.Id_Campamento
	inner join MT_TipoTrabajo mtpeje on mtoint.Id_tipo_trabajo_ejecutado = mtpeje.Id_TipoTrabajo
	where  c.Id_Empresa = @empresa and b.cia_codigo = @empresa and mtoint.EstadoEditOrden = 'A' AND MT_OrdenTrabajo_Integrante.Estado = 'A'
	AND MT_OrdenTrabajo_Equipo.Estado = 'A'
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_IntegranteGrupoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_IntegranteGrupoTrabajo]
	-- Add the parameters for the stored procedure here
	@id_grupotrabajo int
AS
BEGIN
	
    -- Insert statements for procedure here
	select mgi.*, mgt.Nombre, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, mtd.Descripcion as Descripcion_Rol from MT_GrupoTrabajoIntegrante mgi
	inner join MT_GrupoTrabajo mgt on mgi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	inner join [SADATABASE]..[DBA].[rh_persona] p on mgi.Id_Persona = p.id_persona
	inner join MT_TablaDetalle mtd on mgi.Rol_Usuario = mtd.Id_TablaDetalle
	where mgi.Id_GrupoTrabajo = @id_grupotrabajo and mgi.Estado in ('A')
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_IntegranteOrdenTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_IntegranteOrdenTrabajo]
	-- Add the parameters for the stored procedure here
	@pId_OrdenTrabajo int
	--@id_grupotrabajo int

AS
BEGIN
	    --Obtiene el IdEquipo
		DECLARE @TMP_TB_IdPersonaRol TABLE( Id int identity(1,1) not null primary key
										   ,IdPersona  int
										   ,Rol		   int  )
	    --Obtiene PK IdEquipo
		DECLARE @TMP_TB_PkIdOTInte   TABLE( Id int identity(1,1) not null primary key
										   ,PkIdOTInte int )
        
		--Variables para bucle
		DECLARE @NRegistros Int, @contWhile int;

		--Variables @TMP_TB_IdEquipo
		DECLARE @IdPersona Int, @Rol int,@PkOTInte int;

		Set @contWhile  = 0;
		Set @NRegistros = (Select Count(DISTINCT mgi.Id_Persona)--, mgi.Rol_Usuario)
						   From MT_OrdenTrabajo_Integrante moi 
						   Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						   Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						   Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
						 );
		--INS TB TEMP
		Insert into @TMP_TB_IdPersonaRol (IdPersona,Rol)
					  	  Select Distinct mgi.Id_Persona, mgi.Rol_Usuario
						  From MT_OrdenTrabajo_Integrante moi 
						  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 

		--INS TB TEMP OTInte PK
		Insert into @TMP_TB_PkIdOTInte (PkIdOTInte)
					  	  Select Distinct moi.Id_OrdenTrabajo_Integrante
						  From MT_OrdenTrabajo_Integrante moi 
						  Inner Join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
						  Inner Join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
						  Where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo 
	    
		WHILE(@NRegistros > 0 AND @contWhile <= @NRegistros) BEGIN
			
			SET @IdPersona =(SELECT IdPersona  FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @Rol       =(SELECT Rol        FROM @TMP_TB_IdPersonaRol WHERE Id=@contWhile)
			SET @PkOTInte  =(SELECT PkIdOTInte FROM @TMP_TB_PkIdOTInte   WHERE Id=@contWhile)

			UPDATE MT_OrdenTrabajo_Integrante  SET Id_Persona = @IdPersona, Rol = @Rol 
			WHERE Id_Orden_Trabajo = @pId_OrdenTrabajo And Id_OrdenTrabajo_Integrante = @PkOTInte 

			SET @contWhile=@contWhile+1;
		END

		

--	UPDATE MT_OrdenTrabajo_Integrante 
--	SET Id_Persona = i.Id_Persona, 
--    Rol = i.Rol_Usuario 
--	FROM (
--    select DISTINCT mgi.Id_Persona, mgi.Rol_Usuario
--		from MT_OrdenTrabajo_Integrante moi 
--		inner join MT_OrdenTrabajo mo on moi.Id_Orden_Trabajo = mo.Id_OrdenTrabajo
--		inner join MT_GrupoTrabajoIntegrante mgi on moi.Id_GrupoTrabajo = mgi.Id_GrupoTrabajo
--		where moi.Id_Orden_Trabajo = @id_ordentrabajo  ) i  --) i and moi.Id_GrupoTrabajo = @id_grupotrabajo
--where MT_OrdenTrabajo_Integrante.Id_Orden_Trabajo = @id_ordentrabajo  -- and MT_OrdenTrabajo_Integrante.Id_GrupoTrabajo = @id_grupotrabajo



	select moi.*, mgt.Nombre, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, mtd.Descripcion as Descripcion_Rol, mot.Codigo_Cliente 
	from MT_OrdenTrabajo_Integrante moi
	inner join MT_OrdenTrabajo mot on moi.Id_Orden_Trabajo = mot.Id_OrdenTrabajo
	inner join MT_GrupoTrabajo mgt on moi.Id_GrupoTrabajo = mgt.Id_GrupoTrabajo
	inner join [SADATABASE]..[DBA].[rh_persona] p on moi.Id_Persona = p.id_persona
	inner join MT_TablaDetalle mtd on moi.Rol= mtd.Id_TablaDetalle
	where moi.Id_Orden_Trabajo = @pId_OrdenTrabajo  --and moi.Id_GrupoTrabajo = @id_grupotrabajo



END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Items]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Items] 
	@idMTTipoTrabajo int,
	@empresa varchar(2)
AS
BEGIN
	

	select i.*, td.descripcion as DescripcionItem, med.nombre as NombreUnidad, mtp.Descripcion as DescripcionTTrabajo from MT_Items i
inner join [SADATABASE]..[DBA].[items] td on td.cod_item = i.Id_Item
inner join [SADATABASE]..[DBA].[medidas] med on med.uni_med = i.Unidad
inner join MT_TipoTrabajo mtp on i.Id_TipoTrabajo = mtp.Id_TipoTrabajo
where i.Id_TipoTrabajo = @idMTTipoTrabajo and td.cia_codigo = @empresa

--IF NOT EXISTS(select i.*, td.descripcion as DescripcionItem, med.nombre as NombreUnidad, mtp.Descripcion as DescripcionTTrabajo from MT_Items i
--inner join [SADATABASE]..[DBA].[items] td on td.cod_item = i.Id_Item
--inner join [SADATABASE]..[DBA].[medidas] med on med.uni_med = i.Unidad
--inner join MT_TipoTrabajo mtp on i.Id_TipoTrabajo = mtp.Id_TipoTrabajo
--where i.Id_TipoTrabajo = @idMTTipoTrabajo and td.cia_codigo = @empresa)
--		BEGIN
--			select mtp.* from MT_TipoTrabajo mtp
--			inner join Mt_Servicio s on mtp.Id_Servicio = s.Id_Servicio 			
--			where mtp.Id_TipoTrabajo = @idMTTipoTrabajo and s.Id_Empresa = @empresa
--		END
--	ELSE
--	BEGIN
--		select i.*, td.descripcion as DescripcionItem, med.nombre as NombreUnidad, mtp.Descripcion as DescripcionTTrabajo from MT_Items i
--		inner join [SADATABASE]..[DBA].[items] td on td.cod_item = i.Id_Item
--		inner join [SADATABASE]..[DBA].[medidas] med on med.uni_med = i.Unidad
--		inner join MT_TipoTrabajo mtp on i.Id_TipoTrabajo = mtp.Id_TipoTrabajo
--		where i.Id_TipoTrabajo = @idMTTipoTrabajo and td.cia_codigo = @empresa
--	END




END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ItemsBodega]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ItemsBodega] 
@empresa varchar(2)
AS
BEGIN
	
	select ib.*, b.nombre as Nombre_Bodega from MT_ItemsBodega ib
	inner join [SADATABASE]..[DBA].[bodegas] b on b.cod_bod = ib.Bodega
	where b.cia_codigo = @empresa
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_MedidasTTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12-10-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_MedidasTTrabajo] 
	@idMTTipoTrabajo int
AS
BEGIN
	
	select m.*, mtp.Descripcion as DescripcionTTrabajo from MT_Tipo_Trabajo_Medida m
	inner join MT_TipoTrabajo mtp on m.Id_Tipo_Trabajo = mtp.Id_TipoTrabajo
	where m.Id_Tipo_Trabajo = @idMTTipoTrabajo

	--IF NOT EXISTS(select m.*, mtp.Descripcion as DescripcionTTrabajo from MT_Tipo_Trabajo_Medida m
	--inner join MT_TipoTrabajo mtp on m.Id_Tipo_Trabajo = mtp.Id_TipoTrabajo
	--where m.Id_Tipo_Trabajo = @idMTTipoTrabajo)
	--BEGIN
	--		select mtp.* from MT_TipoTrabajo mtp
	--		inner join Mt_Servicio s on mtp.Id_Servicio = s.Id_Servicio 			
	--		where mtp.Id_TipoTrabajo = @idMTTipoTrabajo
	--	END
	--ELSE
	--BEGIN
	--select m.*, mtp.Descripcion as DescripcionTTrabajo from MT_Tipo_Trabajo_Medida m
	--inner join MT_TipoTrabajo mtp on m.Id_Tipo_Trabajo = mtp.Id_TipoTrabajo
	--where m.Id_Tipo_Trabajo = @idMTTipoTrabajo
	--END
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Notificacion_PersonaCorreo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_ConsultaMT_Notificacion_PersonaCorreo] @pn_IdNotificacion int 
as
begin
select*from MT_Notificacion_Persona where Id_Notificacion=@pn_IdNotificacion
end 



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Ordenes_no_Grupos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Ordenes_no_Grupos]

	-- Add the parameters for the stored procedure here
	@empresa varchar(2),
	@id_cliente varchar(10)
AS
BEGIN


	
  select a_hijo.*, tpeje.Descripcion 
	from MT_OrdenTrabajo a_hijo
	--inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato cambie por proyecto
	inner join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
	--inner join [SADATABASE]..[DBA].[clientes] as clientes on con.Id_Cliente = clientes.cod_cli
	inner join [SADATABASE]..[DBA].[clientes] as clientes on pry.Id_Cliente = clientes.cod_cli
	inner join MT_TipoTrabajo tpeje on a_hijo.Id_tipo_trabajo_ejecutado = tpeje.Id_TipoTrabajo
	where clientes.cod_cli = @id_cliente and a_hijo.EstadoEditOrden = 'A'  and 
	/*con.Id_Empresa */ pry.Id_Empresa= @empresa and clientes.cia_codigo = @empresa and Id_OrdenTrabajo not in (
	select Id_OrdenTrabajo from MT_OrdenTrabajo_Integrante moint 
	inner join MT_OrdenTrabajo motrab on moint.Id_Orden_Trabajo = motrab.Id_OrdenTrabajo
	inner join MT_OrdenTrabajo_Equipo moeq on moeq.Id_Orden_Trabajo = motrab.Id_OrdenTrabajo
	)

	

	--select Distinct Mt_GrupoTrabajo.Nombre, mtoeq.Codigo_Cliente   sp_Quimipac_ConsultaMT_Ordenes_no_Grupos '02',1
	--from MT_GrupoTrabajo
	--inner join MT_OrdenTrabajo_Equipo on MT_GrupoTrabajo.Id_GrupoTrabajo = MT_OrdenTrabajo_Equipo.Id_GrupoTrabajo
	--inner join MT_OrdenTrabajo_Integrante on MT_GrupoTrabajo.Id_GrupoTrabajo = MT_OrdenTrabajo_Integrante.Id_GrupoTrabajo
	--inner join MT_OrdenTrabajo mtoeq on MT_OrdenTrabajo_Equipo.Id_Orden_Trabajo = mtoeq.Id_OrdenTrabajo
	--inner join MT_OrdenTrabajo mtoint on MT_OrdenTrabajo_Integrante.Id_Orden_Trabajo = mtoint.Id_OrdenTrabajo
	--inner join [SADATABASE]..[DBA].[bodegas] b on b.cod_bod = MT_GrupoTrabajo.bodega
 --   inner join MT_Campamento c on MT_GrupoTrabajo.Id_Campamento = c.Id_Campamento
	--where  c.Id_Empresa = @empresa and b.cia_codigo = @empresa
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajo] 
@empresa varchar(2),
@automatizacion int
AS
BEGIN

if @automatizacion = 1 or @automatizacion =0
begin
 	select  DISTINCT 
	 (a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  --,a_hijo.Id_contrato cambie por proyecto
	  ,a_hijo.Id_Proyecto
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   --, con.Nombre as NombreContrato
	   , pry.Codigo_Cliente as Cod_ClientePry
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	/*,con.Codigo_Interno*/ ,pry.Codigo_Interno as CodigoInternoProyecto, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz
	
	from MT_OrdenTrabajo a_hijo
	left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
	--inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
	inner join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
	inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_recibido = mtp_PLA.Id_TipoTrabajo
	inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo	
	left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
	left outer join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
	inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
	left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
	
	where a_hijo.EstadoEditOrden = 'A' and pry.Id_Empresa = @empresa and a_hijo.Automatizacion = @automatizacion
	Order by a_hijo.Fecha_Registro desc 
	end

	else
	begin
	 	select  DISTINCT 
	 (a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  --,a_hijo.Id_contrato
	  ,a_hijo.Id_Proyecto
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   --, con.Nombre as NombreContrato
	   , pry.Codigo_Cliente as Cod_ClientePry
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	/*,con.Codigo_Interno*/ ,pry.Codigo_Interno as CodigoInternoProyecto, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz
	
	from MT_OrdenTrabajo a_hijo
	left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
	--inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
	inner join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
	inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_recibido = mtp_PLA.Id_TipoTrabajo
	inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo	
	left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
	left outer join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
	inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
	left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
	
	where a_hijo.EstadoEditOrden = 'A' and pry.Id_Empresa = @empresa
	Order by a_hijo.Fecha_Registro desc 
	end
	

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajo_Combo_QRYX]
    @pComboParametro Varchar(50),
	@empresa varchar(2)
AS
BEGIN
	If @pComboParametro = 'Tipo_Trabajo' Begin
		--Select * From MT_TipoTrabajo mt
		--inner join MT_Servicio ms on mt.Id_Servicio = ms.Id_Servicio Where mt.Estado in ('A','I') And Tipo =2 and ms.Id_Empresa = @empresa Order By mt.Descripcion Asc
	select tp.Id_TipoTrabajo, tp.Descripcion, tp.Codigo --, c.nom_cli as cliente, l.descripcion as lineaTipoTrabajo, t.Descripcion as tipoTrabajo, s.Descripcion as descripcionServicio, a_padre.Descripcion as DescripcionPadre 
	 from MT_TipoTrabajo tp
	 left join MT_TipoTrabajo a_padre on a_padre.Id_TipoTrabajo = tp.Id_Padre
 	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo 
	inner join Mt_Servicio s on tp.Id_Servicio = s.Id_Servicio
	where tp.Estado in ('A', 'I') and c.cia_codigo= @empresa and l.cia_codigo = @empresa and s.Id_Empresa = @empresa And tp.Tipo =2

	End
	Else If @pComboParametro = 'Campamento' Begin
		select camp.Id_Campamento, camp.Nombre from MT_Campamento camp where Estado in ('A', 'I') and Id_Empresa = @empresa Order By Nombre Asc
	End
	Else If @pComboParametro = 'Sucursal' Begin
		SELECT sucursal.cod_suc, sucursal.nombre FROM [SADATABASE]..[DBA].[sucursal] as sucursal where sucursal.cia_codigo = @empresa Order By Nombre Asc
	End
	Else If @pComboParametro = 'Estacion' Begin
		Select e.Id_Estacion, e.Nombre, e.Direccion From MT_Estacion e where e.Estado in ('A', 'I')
		 order By Direccion Asc
	End
	Else If @pComboParametro = 'Sector' Begin
		Select sect.Id_Sector, sect.Nombre From MT_Sector sect Where Estado In ('A','I') and Id_Empresa = @empresa Order By Nombre Asc
	End
	Else If @pComboParametro = 'Contrato' Begin
		Select cont.Id_Contrato, cont.Codigo_Cliente, cont.Nombre, cont.Id_Cliente From MT_Contrato cont Where Estado In (75,76,97,151) and Id_Empresa = @empresa Order By Nombre Asc
	End
	Else If @pComboParametro = 'Estado' Begin
		Select mtd.Id_TablaDetalle, mtd.Descripcion From MT_TablaDetalle mtd
		inner join MT_Tablas mt on mtd.Id_Tabla = mt.Id_Tabla Where mtd.Id_Tabla = 9 Order By mtd.Descripcion desc
	End
	Else If @pComboParametro = 'Cliente' Begin
		select clientes.cod_cli, clientes.nom_cli from [SADATABASE]..[DBA].[clientes] as clientes Where clientes.cia_codigo = @empresa 
	
	End
	Else If @pComboParametro = 'OrdenPadre' Begin

	select mo.* from MT_OrdenTrabajo mo
	--inner join MT_Contrato mc on mo.Id_contrato = mc.Id_Contrato
	inner join MT_Proyecto pry on mo.Id_Proyecto = pry.Id_Proyecto
	inner join MT_TipoTrabajo mt on mo.Id_tipo_trabajo_ejecutado = mt.Id_TipoTrabajo
	inner join MT_Servicio ms on mt.Id_Servicio = ms.Id_Servicio
	where mo.Id_OrdenTrabajo in (select DISTINCT Id_orden_padre FROM MT_OrdenTrabajo) and pry.Id_Empresa = @empresa and ms.Id_Empresa = @empresa and mo.Estado in (30,31) and mo.EstadoEditOrden = 'A'
 
	End
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoCausaRaiz]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







-- =============================================
-- Author:		<Vanessa>
-- ALTER date: <07-01-2019>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoCausaRaiz] 
	@idOrden int
AS
BEGIN
	
select mtcr.*, mtp.Codigo_Cliente, tc.Descripcion as DCausa, th.Descripcion as DHizo, te.Descripcion as DEncontro
from MT_OrdenTrabajo_CausaRaiz as mtcr
inner join MT_OrdenTrabajo mtp on mtcr.Id_Orden_Trabajo = mtp.Id_OrdenTrabajo
inner join MT_TablaDetalle tc on mtcr.Id_Causa_Raiz = tc.Id_TablaDetalle
inner join MT_TablaDetalle th on mtcr.Hizo = th.Id_TablaDetalle
inner join MT_TablaDetalle te on mtcr.Encontro = te.Id_TablaDetalle
where mtcr.Id_Orden_Trabajo = @idOrden 






END






GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoEntregaCliente]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoEntregaCliente] 
@id_cliente varchar(10),
@empresa varchar(2)
AS
BEGIN
 	select a_hijo.*, mteje.Descripcion 
	from MT_OrdenTrabajo a_hijo
	--inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
	inner join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
	inner join [SADATABASE]..[DBA].[clientes] as clientes on pry.Id_Cliente = clientes.cod_cli
	inner join MT_TipoTrabajo mteje on a_hijo.Id_tipo_trabajo_ejecutado = mteje.Id_TipoTrabajo
	where clientes.cod_cli = @id_cliente and a_hijo.EstadoEditOrden = 'A' and a_hijo.Id_entrega_orden_trabajo is null and 
	pry.Id_Empresa = @empresa and clientes.cia_codigo = @empresa
	
	
	--where a_hijo.Fecha_inicio_ejecucion is not null
	

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoEstado]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Vanessa>
-- ALTER date: <07-01-2019>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoEstado] 
	@idOrden int
AS
BEGIN
	
select mtpv.*, mtp.Codigo_Cliente, t.Descripcion as Estadod
from MT_OrdenTrabajo_Estado as mtpv
inner join MT_OrdenTrabajo mtp on mtpv.Id_Orden_Trabajo = mtp.Id_OrdenTrabajo
inner join MT_TablaDetalle t on mtpv.Estado_Orden_Trabajo = t.Id_TablaDetalle
where mtpv.Id_Orden_Trabajo = @idOrden and mtpv.EstadoO IN ('A','I')






END






GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoFormulario]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoFormulario] 
@idActividad int
AS
BEGIN


	select pd.*, o.Codigo_Cliente as CodOT from MT_OrdenTrabajo_Formulario pd
	inner join MT_OrdenTrabajo_Actividad p on p.Id_OrdenTrabajo_Actividad = pd.Id_Orden_Trabajo_Detalle 
	inner join MT_OrdenTrabajo o on p.Id_Orden_Trabajo = o.Id_OrdenTrabajo
	where pd.Id_Orden_Trabajo_Detalle = @idActividad


	

    
	

	 
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoValor]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OrdenTrabajoValor] 
	@idOrden int,
	@empresa varchar(2)
AS
BEGIN
	
select mtpv.*, mtp.Codigo_Cliente, i.descripcion as DescripcionItem, m.nombre, mt.Descripcion as DescrT 
from MT_OrdenTrabajo_Valor as mtpv
inner join MT_OrdenTrabajo mtp on mtpv.Id_Orden_Trabajo = mtp.Id_OrdenTrabajo
inner join [SADATABASE]..[DBA].[items] i on mtpv.Id_Item = i.cod_item
inner join MT_TablaDetalle mt on mtpv.Tipo_Valor = mt.Id_TablaDetalle
inner join [SADATABASE]..[DBA].[monedas] m on mtpv.Moneda = m.codmon
inner join [SADATABASE]..[DBA].[medidas] u on mtpv.Unidad = u.uni_med
where mtpv.Id_Orden_Trabajo = @idOrden and i.cia_codigo = @empresa and m.cia_codigo = @empresa






END





GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_OTAnexo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_OTAnexo] 
@idOT int
AS
BEGIN


	select ota.*, td.Descripcion as DescripcionTipo, ot.Codigo_Cliente as CodOT 
	from MT_OrdenTrabajo_Anexo ota
	inner join MT_TablaDetalle td on ota.Tipo_Anexo = td.Id_TablaDetalle
	inner join MT_OrdenTrabajo ot on ota.Id_Orden_Trabajo = ot.Id_OrdenTrabajo where ota.Id_Orden_Trabajo = @idOT


	

    
	

	 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_Usuario]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Permisos_Usuario]
	@IDUSUARIO  VARCHAR(10),
	@IDEMPRESA  VARCHAR(2)
AS	
BEGIN
	/*Select clientes.nom_cli,contr.Nombre,proy.Codigo_Interno, perm.* From [SADATABASE]..[DBA].[clientes] as clientes
			Inner Join MT_Permiso As perm On clientes.cod_cli = perm.Id_Cliente
			Left Outer Join MT_Contrato As contr On perm.Id_Contrato = contr.Id_Contrato
			Left Outer Join MT_Proyecto As proy On perm.Id_Proyecto = proy.Id_Proyecto
	Where perm.Id_Usuario = @IdUsuario And perm.Estado ='A'*/

	
	SELECT CLIENTES.nom_cli, PERM.*,CONTR.NOMBRE,PROY.CODIGO_INTERNO
	FROM MT_PERMISO PERM
	INNER JOIN [SADATABASE]..[DBA].[CLIENTES] AS CLIENTES ON PERM.ID_CLIENTE = CLIENTES.cod_cli
    LEFT OUTER JOIN MT_CONTRATO AS CONTR ON PERM.ID_CONTRATO = CONTR.ID_CONTRATO AND CONTR.ID_EMPRESA = @IDEMPRESA
    LEFT OUTER JOIN MT_PROYECTO AS PROY ON PERM.ID_PROYECTO = PROY.ID_PROYECTO AND PROY.ID_EMPRESA = @IDEMPRESA
	WHERE PERM.ID_USUARIO = @IDUSUARIO 
	AND PERM.ESTADO ='A' AND PERM.ID_EMPRESA = @IDEMPRESA AND CLIENTES.cia_codigo = @IDEMPRESA 
	ORDER BY CLIENTES.nom_cli ASC
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID]
@UsuarioID varchar(20),
@EmpresaID varchar(2),
@Criterio varchar(20)
AS
BEGIN
	DECLARE @Param varchar(20);

	SET @Param = (UPPER(@Criterio));
	IF @Param ='CLIENTES' 
	BEGIN
		SELECT DISTINCT(Id_Cliente) FROM MT_Permiso
		WHERE Id_Usuario = @UsuarioID AND Id_Empresa = @EmpresaID AND Estado='A';
	END
	ELSE IF @Param ='CONTRATOS' 
	BEGIN
		--SELECT CONCAT(Id_Contrato,'') FROM MT_Permiso
		SELECT CONCAT(ISNULL(Id_Contrato,0),'') FROM MT_Permiso
		WHERE Id_Usuario = @UsuarioID AND Id_Empresa = @EmpresaID AND Estado='A';
	END
	ELSE IF @Param ='PROYECTOS' 
	BEGIN
		--SELECT CONCAT(Id_Proyecto,'') FROM MT_Permiso
		SELECT CONCAT(ISNULL(Id_Proyecto,0),'') FROM MT_Permiso
		WHERE Id_Usuario = @UsuarioID AND Id_Empresa = @EmpresaID AND Estado='A';
	END
	ELSE IF @Param ='PROSPECTOS' 
	BEGIN
		--SELECT CONCAT(Id_Proyecto,'') FROM MT_Permiso
		SELECT CONCAT(ISNULL(Id_Prospecto,0),'') FROM MT_Permiso
		WHERE Id_Usuario = @UsuarioID AND Id_Empresa = @EmpresaID AND Estado='A';
	END
END


--SELECT * FROM MT_Permiso
--sp_Quimipac_ConsultaMT_Permisos_xClienteUsuarioID 'BSALTO', '02','clientes'
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_XContratos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Permisos_XContratos]

@IdEmpresa	Varchar(2),
@IdCliente	varchar(10)
AS
BEGIN

	Select C.*, l.descripcion as lineaContrato, cl.nom_cli as nombreCliente, t.Descripcion as descripcion
	From MT_Contrato C
	Inner Join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea
	Inner Join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente
	Inner Join [SADATABASE]..[DBA].[TBCON_MCIAS] e on  e.CIA_CODIGO = c.Id_Empresa
	Inner Join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado
	Where c.Id_Empresa = @IdEmpresa And c.Id_Cliente = @IdCliente And c.Estado in (75,76,97,151) and l.cia_codigo = @IdEmpresa and cl.cia_codigo = @IdEmpresa
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_XParametros]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ====================================
--	Author:		<SandrovYagual>
--	ALTER:  <5/11/2018 1:11:34> 
--	Upd 15-1-2018 - 30-11-2018
-- ====================================
CREATE procedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_XParametros]
--@pLkParametro varchar(30),
--@Criterio	  varchar(30)

@Id_Empresa	Varchar(2),
@Criterio   Varchar(30),
@IdInt		Int,
@pInt		Int,
@pVarchar   Varchar(30)


--@pVarchar   varchar(30),
--@pDecimal	decimal,
--@pDatetime	datetime,
As
Begin
	 --If @pLkParametro = 'Rol' Begin
		--SELECT * FROM Roles  where Estado = 'A'
	 --End


	 
--Select per.* from MT_Permiso as per 
--inner join  openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr on per.Id_Usuario=usr.user_id
--left outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo

--inner join [SADATABASE]..[DBA].[clientes] as cli on per.Id_Cliente = cli.cod_cli
--left outer join MT_Contrato as cont on per.Id_Contrato = cont.Id_Contrato
--left outer join MT_Proyecto as proy on per.Id_Proyecto = proy.Id_Proyecto
--where user_id = 'AAGUIRRE' And usr.user_status = 'A' and usr.cia_codigo = '01' And per.Estado='A'



	 If @Criterio = 'Contrato' Begin
		Select *From MT_Contrato
		Where Id_Cliente = @IdInt And Id_Empresa = @Id_Empresa--Estado in (75,76,77)
	 End
	 If @Criterio = 'Proyecto' Begin
		Select * From MT_Proyecto
		Where Id_Cliente= @IdInt And Id_Empresa = @Id_Empresa And Estado in ('A','I')
	 End
	 

End

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Permisos_XProyectos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Permisos_XProyectos]
@IdEmpresa	Varchar(2),
@IdCliente	varchar(10)
AS
BEGIN
	Select proy.*, suc.nombre, cl.nom_cli, tt.Descripcion AS DesTTrabajo, l.descripcion as DescLinea--, e.CIA_DESCRIPCION as DesEmpresa
	From MT_Proyecto proy 
	Inner Join [SADATABASE]..[DBA].[sucursal] suc On suc.cod_suc = proy.Id_Sucursal
	Inner Join [SADATABASE]..[DBA].[clientes] cl   On cl.cod_cli = proy.Id_Cliente
	Inner Join MT_TipoTrabajo tt On tt.Id_TipoTrabajo = proy.Id_TipoTrabajo
	inner join MT_Servicio ms on tt.Id_Servicio = ms.Id_Servicio
	Inner Join [SADATABASE]..[DBA].[lineas] l on l.codigo = proy.Linea
	--Inner Join [SADATABASE]..[DBA].[TBCON_MCIAS] e on  e.CIA_CODIGO = proy.Id_Empresa
	where proy.Id_Empresa = @IdEmpresa And proy.Id_Cliente = @IdCliente And proy.Estado = 'A' and suc.cia_codigo = @IdEmpresa
	and cl.cia_codigo = @IdEmpresa and l.cia_codigo = @IdEmpresa and ms.Id_Empresa = @IdEmpresa 
	Order by proy.Codigo_Cliente Asc
END

 
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Planilla]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Planilla] 
	@idTipoTablaDetalle int,
	@empresa varchar(2)
AS
BEGIN


 if @idTipoTablaDetalle = 1 --PROYECTO
 begin

 select mpl.*, mp.Codigo_Cliente, mt.Descripcion, mtt.Descripcion as DTipoTrabajo
 from MT_Planilla as mpl
 inner join MT_Proyecto mp on mpl.Id_PoyectoContr = mp.Id_Proyecto
 inner join MT_TablaDetalle mt on mpl.Tipo_C_P = mt.Id_TablaDetalle
 inner join MT_TipoTrabajo mtt on mpl.Tipo_Trabajo = mtt.Id_TipoTrabajo
 where mpl.Tipo_C_P = @idTipoTablaDetalle and 
 mp.Id_Empresa = @empresa 
 
 End
 else --CONTRATO
 begin
 select mpl.*, mp.Codigo_Cliente, mt.Descripcion, mtt.Descripcion as DTipoTrabajo 
 from MT_Planilla as mpl
 inner join MT_Contrato mp on mpl.Id_PoyectoContr = mp.Id_Contrato
 inner join MT_TablaDetalle mt on mpl.Tipo_C_P = mt.Id_TablaDetalle
 inner join MT_TipoTrabajo mtt on mpl.Tipo_Trabajo = mtt.Id_TipoTrabajo
 where mpl.Tipo_C_P = @idTipoTablaDetalle and 
 mp.Id_Empresa = @empresa 

 end
 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_PrecioReferencial]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_PrecioReferencial] 
	@idTipoTablaDetalle int,
	@empresa varchar(2)
AS
BEGIN


 if @idTipoTablaDetalle = 1 --PROYECTO
 begin
 select prf.Id_PrecioReferencial, 
	prf.Id_TipoTablaDetalle, 
	prf.Id_Item, 
	prf.Id_TipoTrabajo as Id_TipoTrabajoPrecio, 
	prf.Id_Usuario, 
	prf.Fecha_Registro, 
	prf.Fecha_Inicio, 
	prf.Fecha_Fin, 
	prf.Moneda, 
	prf.Precio, 
	prf.Costo, 
	prf.Estado, 
	i.descripcion as descripcion, 
	t.Descripcion as tipoTrabajo, 
	mo.nombre as nom_moneda, 
	p.Id_Proyecto as Id_Proyecto_Contrato, 
	p.Codigo_Interno, 
	p.Codigo_Cliente,
	tp.Id_TipoTrabajo,
	tp.Codigo as CodigoMTTipoTrabajo,
	tp.Descripcion as DescripcionMTTipoTrabajo
    from MT_PrecioReferencial prf
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = prf.Id_TipoTablaDetalle
	inner join MT_Proyecto p on  p.Id_Proyecto = prf.Id_Proyecto_Contrato 
	inner join MT_TipoTrabajo tp on tp.Id_TipoTrabajo = prf.Id_TipoTrabajo
	inner join [SADATABASE]..[DBA].[items] i on prf.Id_Item = i.cod_item
	inner join [SADATABASE]..[DBA].[monedas] mo on prf.Moneda = mo.codmon
	where prf.Id_TipoTablaDetalle = @idTipoTablaDetalle and prf.Estado in ('A', 'I') and p.Id_Empresa = @empresa and i.cia_codigo = @empresa and mo.cia_codigo = @empresa


 end
 else --CONTRATO
 begin
 select prf.Id_PrecioReferencial, 
	prf.Id_TipoTablaDetalle, 
	prf.Id_Item, 
	prf.Id_TipoTrabajo as Id_TipoTrabajoPrecio, 
	prf.Id_Usuario, 
	prf.Fecha_Registro, 
	prf.Fecha_Inicio, 
	prf.Fecha_Fin, 
	prf.Moneda, 
	prf.Precio, 
	prf.Costo, 
	prf.Estado, 
	i.descripcion as descripcion, 
	t.Descripcion as tipoTrabajo, 
	mo.nombre as nom_moneda, 
	c.Id_Contrato as Id_Proyecto_Contrato, 
	c.Codigo_Interno, 
	c.Codigo_Cliente,
	tp.Id_TipoTrabajo,
	tp.Codigo as CodigoMTTipoTrabajo,
	tp.Descripcion as DescripcionMTTipoTrabajo
    from MT_PrecioReferencial prf
	inner join [SADATABASE]..[DBA].[items] i on prf.Id_Item = i.cod_item
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = prf.Id_TipoTablaDetalle
	inner join [SADATABASE]..[DBA].[monedas] mo on prf.Moneda = mo.codmon
	inner join MT_Contrato c on  c.Id_Contrato = prf.Id_Proyecto_Contrato 
	inner join MT_TipoTrabajo tp on tp.Id_TipoTrabajo = prf.Id_TipoTrabajo
	where prf.Id_TipoTablaDetalle = @idTipoTablaDetalle and prf.Estado in ('A', 'I') and c.Id_Empresa = @empresa and i.cia_codigo = @empresa and mo.cia_codigo = @empresa
 end


	--select prf.* , i.descripcion as descripcion, t.Descripcion as tipoTrabajo, mo.nombre as nom_moneda 
	--from MT_PrecioReferencial prf
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.items i on prf.Id_Item = i.cod_item
	--inner join MT_TablaDetalle t on t.Id_TablaDetalle = prf.Id_TipoTablaDetalle
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.monedas mo on prf.Id_moneda = mo.codmon
	

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_PrecioReferencial_Items]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_PrecioReferencial_Items]
@IdPrecioReferencial int,
@empresa varchar(2)
AS
BEGIN
	--Select td.Descripcion,preRef.* From MT_PrecioReferencial preRef
	--Inner Join [SADATABASE]..[DBA].[items] td on preRef.Id_Item = td.cod_item
	--Where Id_Proyecto_Contrato =(SELECT Id_Proyecto_Contrato FROM MT_PrecioReferencial Where Id_PrecioReferencial =@IdPrecioReferencial) 
	--AND Estado ='A' and td.cia_codigo = @empresa--In ('A','I') 
	declare @trabajo int,
	@proyecto_contrato int
	set @trabajo = (select Id_TipoTrabajo from MT_PrecioReferencial where Id_PrecioReferencial = @IdPrecioReferencial)
	set @proyecto_contrato = (select Id_Proyecto_Contrato from MT_PrecioReferencial where Id_PrecioReferencial = @IdPrecioReferencial)
	select  td.Descripcion, preRef.* from MT_PrecioReferencial preRef
	inner join [SADATABASE]..[DBA].[items] td on preRef.Id_Item = td.cod_item
	where estado = 'A' and preRef.Id_TipoTrabajo = @trabajo and td.cia_codigo = @empresa and preRef.Id_Proyecto_Contrato = @proyecto_contrato

	
/*SELECT Id_Proyecto_Contrato, * FROM MT_PrecioReferencial
where Id_Proyecto_Contrato =(SELECT Id_Proyecto_Contrato FROM MT_PrecioReferencial
where Id_PrecioReferencial =2) AND Estado ='A'--In ('A','I')*/



END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Presupuesto]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Presupuesto] 
	-- Add the parameters for the stored procedure here
	@id_usuario varchar(20),
	@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select p.*, empresas.CIA_DESCRIPCION, clientes.nom_cli, usr.user_id, suc.nombre 
	from MT_Presupuesto p
	inner join [SADATABASE]..[DBA].[TBCON_MCIAS] as empresas on p.Id_Empresa = empresas.CIA_CODIGO
	inner join [SADATABASE]..[DBA].[clientes] as clientes on p.Id_Cliente = clientes.cod_cli
	inner join openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr on p.Id_Usuario = usr.user_id
	inner join [SADATABASE]..[DBA].[sucursal] as suc on p.Id_Sucursal = suc.cod_suc
	where p.Id_Usuario = @id_usuario and p.Id_Empresa = @empresa and clientes.cia_codigo = @empresa and usr.cia_codigo = @empresa

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_PresupuestoDetalle]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_PresupuestoDetalle] 
	@idMtPresupuesto int,
	@empresa varchar(2)
AS
BEGIN

select pred.*, td.descripcion as DescripcionItem, med.nombre as NombreUnidad from MT_Presupuesto_Detalle pred
inner join [SADATABASE]..[DBA].[items] td on td.cod_item = pred.Id_Item
inner join [SADATABASE]..[DBA].[medidas] med on med.uni_med = pred.Unidad
inner join MT_Presupuesto pre on pred.Id_Presupuesto = pre.Id_Presupuesto
where pred.Id_Presupuesto = @idMtPresupuesto and td.cia_codigo = @empresa

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
--	Author:		<VQ,SY>
--	ALTER date: <17-12-2018,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo]
	@id_usuario varchar(10),
	@empresa varchar(2)
AS
BEGIN

	--select c.Nombre as Curso, m.Materia, hm.Dia, hm.Fecha, hm.Hora_Inicio, hm.Hora_Fin, hm.IdMateriaCurso,mc.Nombre_Archivo , he.* from HorarioEstudiante he
	--	inner join HorarioMateria hm on hm.IdHorarioMateria  = he.IdHorarioMateria
	--	inner join Materia_Curso mc on mc.IdMateriaCurso = hm.IdMateriaCurso
	--	inner join Materias m on m.Id_Materias = mc.IdMateria
	--	inner join Curso c on c.IdCurso = mc.IdCurso
	--	where he.IdUsuario = @IdUsuario and he.Estado = 'A'

	--select a_hijo.*,ISNULL(a_padre.Codigo, '') as codigoPadre,ISNULL( a_padre.Descripcion ,'') as descripcionPadre, mttrabajo.Descripcion as Descripcion_TTrabajo, mttrabajo.Fecha_Registro
	--from MT_Actividad a_hijo
	--left join MT_Actividad a_padre on a_padre.Id_Actividad = a_hijo.Id_ActividadPadre
	--inner join MT_TipoTrabajo mttrabajo on a_hijo.Id_TipoTrabajo = mttrabajo.Id_TipoTrabajo
	--where mttrabajo.Id_Usuario = @id_usuario

	/*....Ant
	select pt.* from MT_ProgramaTrabajo pt
	inner join MT_Contrato c on pt.Id_Contrato = c.Id_Contrato
	inner join MT_TipoTrabajo t on pt.Id_TIpo_Trabajo = t.Id_TipoTrabajo 
	inner join MT_GrupoTrabajo g on pt.Id_GrupoTrabajo = g.Id_GrupoTrabajo
	where pt.Id_Usuario = @id_usuario*/


	Select pt.*,c.Nombre as NombreContrato, t.Descripcion as DescripcionTT, g.Nombre as NombreGrupo,g.Color From MT_ProgramaTrabajo pt
	Inner Join MT_Contrato c on pt.Id_Contrato = c.Id_Contrato
	Inner Join MT_TipoTrabajo t on pt.Id_TIpo_Trabajo = t.Id_TipoTrabajo 
	Inner Join MT_GrupoTrabajo g on pt.Id_GrupoTrabajo = g.Id_GrupoTrabajo
	Where pt.Id_Usuario = @id_usuario And pt.Estado ='A' and c.Id_Empresa = @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_Contratos]
	@empresa	Varchar(2)
  --@id_usuario Varchar(10),@Criterio Varchar(40)
AS
BEGIN
	
	Select C.*,
	l.descripcion as lineaContrato,	cl.nom_cli as nombreCliente,t.Descripcion as descripcionEstado,	e.CIA_Descripcion	
	From MT_Contrato C
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea
	inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente
	inner join [SADATABASE]..[DBA].[TBCON_MCIAS] e on  e.CIA_CODIGO = c.Id_Empresa
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado
	where c.Id_Empresa = @empresa  --and Usuario
	And c.Estado =75 and l.cia_codigo = @empresa and cl.cia_codigo = @empresa
	--in (75,76,77)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_ContratosNoOT]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
--	Author:		<Sandro Yagual>
--	ALTER date: <17-12-2018,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_ContratosNoOT]
	@empresa	Varchar(2)  --@id_usuario Varchar(10),@Criterio Varchar(40)
AS
BEGIN
	Select Distinct mo.Id_contrato As Id From MT_OrdenTrabajo mo 
	inner join Mt_Contrato mc on mo.Id_contrato = mc.Id_Contrato 
	Where mo.Estado <>34	--34 Finalizado
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_OT]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


 CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMt_ProgramaTrabajo_OT]
   -- @parametro Varchar(50)
   @empresa varchar(2)
AS
BEGIN
	
	Select DISTINCT
	   a_hijo.Id_OrdenTrabajo
      --,a_hijo.Id_contrato
      ,a_hijo.Id_Proyecto
      ,a_hijo.Id_sucursal
      ,a_hijo.Id_campamento
      ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
      ,a_hijo.Estado
      ,a_hijo.Id_sector
      ,a_hijo.Id_orden_padre
      ,a_hijo.Id_estacion
      ,a_hijo.Id_usuario
      ,a_hijo.Id_entrega_orden_trabajo
      ,a_hijo.Nivel_prioridad
      ,a_hijo.Fecha_registro
      ,a_hijo.Fecha_creacion_cliente
      ,a_hijo.Fecha_maxima_ejecucion_cliente--,format(a_hijo.Fecha_asignacion_grupo_trabajo,'yyyyMMdd HH:mm:ss') Fecha_asignacion_grupo_trabajo
      ,Convert(Datetime,convert(date,a_hijo.Fecha_asignacion_grupo_trabajo)) Fecha_asignacion_grupo_trabajo
      ,Convert(Datetime,convert(date,a_hijo.Fecha_asignacion)) Fecha_asignacion
	  --,a_hijo.Fecha_asignacion_grupo_trabajo
      ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
      ,a_hijo.Fecha_ini_permiso_municipal
      ,a_hijo.Fecha_fin_permiso_municipal
      ,a_hijo.Fecha_entrega
      ,a_hijo.Fecha_max_legalizacion
      ,a_hijo.Hora_acordada
      ,a_hijo.Id_barrio
      ,a_hijo.Direccion
      ,a_hijo.Referencia_direccion
      ,a_hijo.Identificacion_suscriptor
      ,a_hijo.Nombre_suscriptor
      ,a_hijo.Tipo_suscriptor
      ,a_hijo.Telefono_suscriptor
      ,a_hijo.Origen
      ,a_hijo.Comentario
      ,a_hijo.Porcentaje_avance
      ,a_hijo.Tiempo_transcurrido
      ,a_hijo.Id_planilla
      ,a_hijo.Estado_orden_planilla
      ,a_hijo.Codigo_Cliente
      ,a_hijo.Interna, 

 
	/*con.Codigo_Interno*/ pry.Codigo_Interno as CodigoInternoProyecto, /*con.Nombre*/ pry.Codigo_Cliente as CodigoClientePry--, cp.Parametro--, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto,   --,ISNULL( a_padre.Descripcion ,'') as descripcionPadre 
	--mttrabajo.Descripcion as Descripcion_TTrabajo 
	from MT_OrdenTrabajo a_hijo
	left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
	--inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
	inner join MT_Proyecto pry on a_hijo.Id_Proyecto = pry.Id_Proyecto
	--left outer join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
	left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
	left outer join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
	where a_hijo.EstadoEditOrden = 'A' And a_hijo.Estado <>34 and pry.Id_Empresa = @empresa
	Order by a_hijo.Fecha_Registro desc 
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Prospecto]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Prospecto] 
@empresa varchar(2)
AS
BEGIN
	
	--SELECT * FROM MT_Prospecto WHERE Id_Empresa = @empresa

	--Select Count(cont.Id_Prospecto) as CountID,cont.Id_Prospecto,cont.Nombre, cont.Codigo_Cliente 
	--from MT_Prospecto cont
	--Inner Join MT_Prospecto_Parametro cp on cont.Id_Prospecto = cp.Id_Prospecto
	--WHERE CONT.Id_Empresa = @empresa and cont.Estado = 75
	--Group by cont.Id_Prospecto,cont.Nombre, cont.Codigo_Cliente 
	--Having Count(cont.Id_Prospecto)=3 

	Select cont.Id_Prospecto, cont.Nombre, cont.Codigo_Cliente, Id_Cliente, tipo, Codigo_Interno, Estado
	--Referencia
	from MT_Prospecto cont
	--Inner Join MT_Prospecto_Parametro cp on cont.Id_Prospecto = cp.Id_Prospecto
	WHERE CONT.Id_Empresa = @empresa and cont.Estado != 1160 
	--Group by cont.Id_Prospecto,cont.Nombre, cont.Codigo_Cliente 

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Prospecto_TipoPadre]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Prospecto_TipoPadre] 
@empresa varchar(2),
@tipo int,
@cliente varchar(10)
AS
BEGIN
--db.MT_Prospecto.Where(x => x.Id_Empresa == empresa && x.Estado != 77 && x.Id_Cliente == id_Cliente)
	if(@tipo = 144 )	
	
	select c.Id_Prospecto, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,
	--c.Prospecto_Padre, 
	c.Id_Cliente, mtd.Descripcion from MT_Prospecto c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.tipo in (144, 145,146,1152) and c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75

	else
	if(@tipo = 145 or @tipo = 146 or @tipo = 1167 or @tipo = 1168)
	select c.Id_Prospecto, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,
	--c.Prospecto_Padre,
	c.Id_Cliente, mtd.Descripcion from MT_Prospecto c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo not in (144,145,146,1152)
	else
	if(@tipo = 1152)
	select c.Id_Prospecto, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,
	--c.Prospecto_Padre,
	c.Id_Cliente, mtd.Descripcion from MT_Prospecto c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo in (144,145,1152)
	else
	if(@tipo = 1165)
	select c.Id_Prospecto, c.Nombre, c.Codigo_Interno, c.Codigo_Cliente,
	--c.Prospecto_Padre, 
	c.Id_Cliente, mtd.Descripcion from MT_Prospecto c
	inner join MT_TablaDetalle mtd on c.tipo = mtd.Id_TablaDetalle
	where c.Id_Cliente = @cliente and c.Id_Empresa = @empresa and c.Estado = 75
	and c.tipo in (144, 145,146,1152)

	--proyecto 144, 145, 1152
	--cuando hay licitacion y prospecto no hay padre 
	--select * from MT_TablaDetalle where Id_Tabla = 63 


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProspectoDocumentado]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProspectoDocumentado] 
@idProspecto int,
@empresa varchar(2)
AS
BEGIN


	select cd.*, c.Nombre as NombreProspecto , c.Codigo_Cliente as CodigoC , t.Descripcion as DescripcionTipo 
	from MT_Prospecto_Documentado cd 
     inner join MT_TablaDetalle t on t.Id_TablaDetalle = cd.Tipo
	inner join MT_Prospecto c on c.Id_Prospecto = cd.Id_Prospecto where cd.Estado in ('A', 'I') and cd.Id_Prospecto = @idProspecto and c.Id_Empresa = @empresa
    
	

	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProspectoGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProspectoGeneral] 
@empresa varchar(2)
AS
BEGIN


	select C.Id_Prospecto, C.Id_Empresa, c.Codigo_Cliente, c.Codigo_Interno, c.Nombre, c.Estado, c.Fecha_Inicio, c.Fecha_Fin, c.tipo, c.Subcategoria, c.Responsable, c.Localidad,
	c.Id_Linea, c.Id_Cliente, c.Fecha_registro, 
	--c.Prospecto_Padre, 
	c.Categoria, 
	--c.Referencia, 
	c.Secuencial, c.Observaciones, c.monto, c.Dia_Plazo, c.costo, c.Codigo_Interno_Ant, c.Codigo_interno_padre, c.Id_Usuario,
	l.descripcion as lineaProspecto, cl.nom_cli as nombreCliente,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	--cpadre.Nombre as Nomb_ProspectoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Prospecto
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Prospecto,   
	
	Localidad.descripcion as nom_Localidad, persona.primer_nombre as Nombres_Completos , cl.abreviatura_cliente ,
	'' as Origen
	from MT_Prospecto C
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[LOCALIDAD] localidad on c.Localidad = localidad.codigo_loc and localidad.cia_codigo = @empresa
	inner join [SADATABASE]..[DBA].[rh_persona] persona on c.Responsable = persona.id_persona
	--inner join MT_TablaDetalle t on t.Id_TablaDetalle = c.estado
	--inner join MT_TablaDetalle tipo on tipo.Id_TablaDetalle = C.tipo
	where c.Id_Empresa = @empresa and c.Estado !=1160 --and cl.cia_codigo = @empresa and l.cia_codigo = @empresa  --and subcateg.cia_codigo = '02' and categ.cia_codigo = '02'	
	order by Id_Prospecto desc

	
	
	
	
	

	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Proyecto]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Proyecto] 
@empresa varchar(2)
AS
BEGIN
	
	select * from MT_Proyecto where Estado in('A', 'I') and Id_Empresa = @empresa

END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividades]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Ericka Loor>
-- ALTER date: <12-10-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividades] 
	@idProyecto int,
	@empresa varchar(2)
AS
BEGIN
	
	select pa.*, p.Codigo_Cliente, ma.Descripcion, t.Descripcion as DescripcionEstado from MT_Proyecto_Actividad pa
	inner join MT_Proyecto p on pa.Id_Proyecto = p.Id_Proyecto
	inner join MT_TipoTrabajo mtp on p.Id_TipoTrabajo = mtp.Id_TipoTrabajo
	inner join MT_Actividad ma on pa.Id_Actividad = ma.Id_Actividad
	left outer join MT_TablaDetalle t on t.Id_TablaDetalle = pa.Estado
	left outer join MT_Planilla mp on pa.Id_Planilla = mp.Id_Planilla
	where pa.Estado in ('A','I') and pa.Id_Proyecto = @idProyecto and p.Id_Empresa = @empresa
	 
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesEquipo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Vanessa>
-- ALTER date: <03-12-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesEquipo] 
	@idProyectoactividad int
AS
BEGIN
	
select mtpe.*, ma.Descripcion as DescripcionActividad, me.Descripcion as DescripcionEquipo from MT_Proyecto_Actividad_Equipo as mtpe
inner join MT_Proyecto_Actividad mtp on mtpe.Id_Proyecto_Actividad = mtp.Id_Proyecto_Actividad
inner join MT_Actividad ma on mtp.Id_Actividad = ma.Id_Actividad
inner join MT_Equipo me on mtpe.Id_Equipo = me.Id_Equipo
where mtpe.Id_Proyecto_Actividad = @idProyectoactividad






END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesIntegrantes]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Vanessa>
-- ALTER date: <03-12-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesIntegrantes] 
	@idProyectoactividad int
AS
BEGIN
	
select mtpe.*, ma.Descripcion as DescripcionActividad, Concat(p.primer_nombre, ' ',p.segundo_nombre, ' ',p.primer_apellido, ' ', p.segundo_apellido) as Nombre_Completo, mtd.Descripcion from MT_Proyecto_Actividad_Integrante as mtpe
inner join MT_Proyecto_Actividad mtp on mtpe.Id_Proyecto_Actividad = mtp.Id_Proyecto_Actividad
inner join MT_Actividad ma on mtp.Id_Actividad = ma.Id_Actividad
inner join [SADATABASE]..[DBA].rh_persona p on mtpe.Id_Persona = p.id_persona
inner join MT_TablaDetalle mtd on mtpe.Rol = mtd.Id_TablaDetalle
where mtpe.Id_Proyecto_Actividad = @idProyectoactividad

END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesValor]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


 CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividadesValor] 
	@idProyectoactividad int,
	@empresa varchar(2)
AS
BEGIN
	
select mtpv.*, ma.Descripcion as DescripcionActividad, i.descripcion as DescripcionItem, m.nombre, mt.Descripcion as DescrT from MT_Proyecto_Actividad_Valor as mtpv
inner join MT_Proyecto_Actividad mtp on mtpv.Id_Proyecto_Actividad = mtp.Id_Proyecto_Actividad
inner join MT_Actividad ma on mtp.Id_Actividad = ma.Id_Actividad
inner join [SADATABASE]..[DBA].[items] i on mtpv.Id_Item = i.cod_item
inner join MT_TablaDetalle mt on mtpv.Tipo_Valor = mt.Id_TablaDetalle
inner join [SADATABASE]..[DBA].[monedas] m on mtpv.Moneda = m.codmon
inner join [SADATABASE]..[DBA].[medidas] u on mtpv.Unidad = u.uni_med
where mtpv.Id_Proyecto_Actividad = @idProyectoactividad and i.cia_codigo = @empresa and m.cia_codigo = @empresa 






END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividdAnexo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoActividdAnexo] 
@idProyectoActividad int
AS
BEGIN


	select pd.*, td.Descripcion as DescripcionTipo from MT_Proyecto_Actividad_Anexo pd
	inner join MT_TablaDetalle td on pd.Tipo = td.Id_TablaDetalle
	inner join MT_Proyecto_Actividad p on p.Id_Proyecto_Actividad = pd.Id_Proyecto_Actividad where pd.Id_Proyecto_Actividad = @idProyectoActividad


	

    
	

	 
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoAlerta]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoAlerta] 
@idProyecto int,
@empresa varchar(2)
AS
BEGIN
	
	
	select pa .*, p.Codigo_Cliente as CodigoC    from MT_Proyecto_Alerta pa

	inner join MT_Proyecto p on p.Id_Proyecto = pa.Id_Proyecto  where pa.Estado in ('A', 'I') and pa.Id_Proyecto = @idProyecto and p.Id_Empresa = @empresa
	
END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoDocumentado]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoDocumentado] 
@idProyecto int,
@empresa varchar(2)
AS
BEGIN


	select pd.*, td.Descripcion as DescripcionTipo, p.Codigo_Cliente as CodProyecto from MT_Proyecto_Documento pd
	inner join MT_TablaDetalle td on pd.Tipo = td.Id_TablaDetalle
	inner join MT_Proyecto p on p.Id_Proyecto = pd.Id_Proyecto where pd.Estado in ('A', 'I') and pd.Id_Proyecto = @idProyecto
	and p.Id_Empresa = @empresa


	

    
	

	 
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


 CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoGeneral] 
@empresa varchar(2)
AS
BEGIN

	
	select P.*, l.descripcion as DescLinea, cl.nom_cli as nombreCliente,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 	
     cl.abreviatura_cliente , tt.Descripcion AS DesTTrabajo

	from MT_Proyecto P
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = p.Linea and l.cia_codigo = @empresa
	inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = p.Id_Cliente and cl.cia_codigo = @empresa
	
	left outer join MT_Contrato cpadre on p.Id_contrato = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa	
	left outer Join MT_TipoTrabajo tt On tt.Id_TipoTrabajo = p.Id_TipoTrabajo
	left outer join MT_Servicio ms on tt.Id_Servicio = ms.Id_Servicio and ms.Id_Empresa = @empresa

	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = P.Linea and p.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = P.Linea and p.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa

	where p.Id_Empresa = @empresa and p.Estado != 'E' --and cl.cia_codigo = @empresa and l.cia_codigo = @empresa  --and subcateg.cia_codigo = '02' and categ.cia_codigo = '02'	
	order by Id_Proyecto desc


					

END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoParametro]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoParametro] 
@idProyecto int,
@empresa varchar(2)
AS
BEGIN


	select pd.*, p.Codigo_Cliente as CodigoC, mon.nombre
	from MT_Proyecto_Parametro pd 
	inner join MT_Proyecto p on p.Id_Proyecto = pd.Id_Proyecto
	left outer join [SADATABASE]..[DBA].[monedas] mon on pd.Moneda = mon.codmon
	 where pd.Estado in ('A', 'I') and pd.Id_Proyecto = @idProyecto and p.Id_Empresa = @empresa and mon.cia_codigo = @empresa
    
	

	 
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ProyectoPresupuesto]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ProyectoPresupuesto] 
@id_cliente varchar(10),
@empresa varchar(2)
AS
BEGIN
 	select a_hijo.* 
	from MT_Proyecto a_hijo
	inner join [SADATABASE]..[DBA].[clientes] as clientes on a_hijo.Id_Cliente = clientes.cod_cli
	where clientes.cod_cli = @id_cliente and a_hijo.Estado = 'A' and a_hijo.Id_Presupuesto is null and a_hijo.Id_Empresa = @empresa


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Sector]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12-09-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Sector]
	-- Add the parameters for the stored procedure here
@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here

	select *
	from MT_Sector 
	where Estado in ('A','I') and Id_Empresa = @empresa


	
END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_SectorGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12-09-2018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_SectorGeneral]
	-- Add the parameters for the stored procedure here
@empresa varchar(2)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here

	select a_hijo.*, ISNULL( a_padre.Nombre ,'') as descripcionPadre
	from MT_Sector a_hijo
	left join MT_Sector a_padre on a_padre.Id_Sector = a_hijo.Id_Padre_Sector
	where a_hijo.Estado in ('A','I')and a_hijo.Id_Empresa = @empresa


	
END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Servicio]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Servicio] 
@empresa varchar(2)
AS
BEGIN
	
	select * from MT_Servicio where Estado in ('A', 'I') and Id_Empresa = @empresa

END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_ServicioGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_ServicioGeneral] 
@empresa varchar(2)
AS
BEGIN
	
	select * from MT_Servicio s
	left outer join [SADATABASE]..[DBA].[TBCON_MCIAS] as empresa on s.Id_Empresa = empresa.CIA_CODIGO
	where Estado in ('A', 'I') and Id_Empresa = @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Solicitud]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Solicitud] 
@Id_Usuario varchar(50),
@empresa varchar(2)
AS
BEGIN
	
	--select s.*, o.Codigo_Cliente, b.nombre as Nombre_Bodega from MT_Solicitud s
	--inner join MT_OrdenTrabajo o on s.Id_OrdenTrabajo = o.Id_OrdenTrabajo
	--inner join SADATABASE.[GQ-DB-SGCPM_].DBA.bodegas b on b.cod_bod = s.BodegaRetirar
	--where s.Estado in ('A', 'I')

	select * FROM [SADATABASE]..[DBA].[pla_compras] as Pla_compras where usuario =@Id_Usuario and cia_codigo = @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_SolicitudMateriales]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_SolicitudMateriales] 
@idSolicitud int,
@empresa varchar(2)
AS
BEGIN
	
	select ms.*, td.descripcion as descripcionItems from MT_Materiales_Solicitud ms
	inner join MT_Solicitud s on ms.Id_Solicitud = s.Id_Solicitud
	inner join [SADATABASE]..[DBA].[items] td on td.cod_item = ms.Id_Item
	where s.Id_Solicitud = @idSolicitud and ms.Estado in ('A','I') and td.cia_codigo =  @empresa

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_Tabla]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_Tabla] 

AS
BEGIN
	
	select * from MT_Tablas where Estado in ('A','I')

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TablaDetalle]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SY>
-- ALTER date: <30-08-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TablaDetalle] 
	@id_tabla int
AS
BEGIN
	
	select * from MT_TablaDetalle td where td.Id_Tabla = @id_tabla
	order by orden asc

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TablaDetalleGeneral]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TablaDetalleGeneral] 
	@id_tabla int
AS
BEGIN
	
	

	select a_hijo.*,ISNULL(a_padre.Codigo, '') as codigoPadre, ISNULL( a_padre.Descripcion ,'') as descripcionPadre, mtTabla.Descripcion as Descripcion_Tabla 
	from MT_TablaDetalle a_hijo
	left join MT_TablaDetalle a_padre on a_padre.Id_TablaDetalle = a_hijo.Id_Padre
	inner join MT_Tablas mtTabla on a_hijo.Id_Tabla = mtTabla.Id_Tabla
	where a_hijo.Id_Tabla = @id_tabla


END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo]
@empresa varchar(2)	
AS
BEGIN
	
	 select tp.*, c.nom_cli as cliente, l.descripcion as lineaTipoTrabajo, t.Descripcion as tipoTrabajo, s.Descripcion as descripcionServicio, a_padre.Descripcion as DescripcionPadre 
	 from MT_TipoTrabajo tp
	 left join MT_TipoTrabajo a_padre on a_padre.Id_TipoTrabajo = tp.Id_Padre
 	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo 
	inner join Mt_Servicio s on tp.Id_Servicio = s.Id_Servicio
	where tp.Estado in ('A', 'I') and c.cia_codigo= @empresa and l.cia_codigo = @empresa and s.Id_Empresa = @empresa
	order by tp.Id_Padre
	--order by tp.Estado 




	

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo] 
	@idTipo int,
	@empresa varchar(2)
AS
BEGIN
	
 select tp.*, c.nom_cli as cliente, l.descripcion as Descripcio_linea, t.Descripcion as tipoTrabajo from MT_TipoTrabajo tp
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where tp.Tipo = @idTipo and s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Estado in ('A', 'I')
	--order by tp.Estado  

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadre] 
	@idTipo int,
	@empresa varchar(2)
AS
BEGIN
	
  select tp.*, c.nom_cli as cliente, l.descripcion as Descripcio_linea, t.Descripcion as tipoTrabajo from MT_TipoTrabajo tp
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where tp.Tipo = @idTipo and s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Estado = 'A' and tp.Id_TipoTrabajo IN(SELECT DISTINCT Id_Padre FROM MT_TipoTrabajo)
	order by tp.Estado 


	

	

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_OTPadreHijos] 
	@empresa varchar(2),
	@padre int
AS
BEGIN
	
  select tp.*, c.nom_cli as cliente, l.descripcion as Descripcio_linea, t.Descripcion as tipoTrabajo from MT_TipoTrabajo tp
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Id_Padre = @padre and tp.Estado = 'A'

	order by tp.Estado 


	

	

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--se lo utilizo para tipotrabajo_nonietos y para precioreferencial si nietos
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajo_IdTipo_PrecioRef] 
	@idTipo int,
	@empresa varchar(2)
AS
BEGIN
	
  select tp.*, c.nom_cli as cliente, l.descripcion as Descripcio_linea, t.Descripcion as tipoTrabajo from MT_TipoTrabajo tp
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where tp.Tipo = @idTipo and s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Id_TipoTrabajo  NOT IN(SELECT DISTINCT Id_Padre FROM MT_TipoTrabajo)
	order by tp.Estado 


	

	

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <12-10-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaMT_TipoTrabajoActividadesItems] 
	@idTipo int,
	@empresa varchar(2)
AS
BEGIN
	
 select DISTINCT mtp.*, t.Descripcion as tipoTrabajo from MT_TipoTrabajo mtp
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = mtp.Tipo
	inner join MT_Actividad ma on mtp.Id_TipoTrabajo = ma.Id_TipoTrabajo
	inner join MT_Items mi on mtp.Id_TipoTrabajo = mi.Id_TipoTrabajo
	inner join MT_Servicio ms on mtp.Id_Servicio = ms.Id_Servicio
	where mtp.Tipo = @idTipo and ms.Id_Empresa = @empresa


END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaOpcionesSistema]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaOpcionesSistema] 
	@id_usuario varchar(10), @padre int, @empresa varchar(2)
AS
BEGIN
	
	 IF @padre = 0
	 BEGIN

		
		 select m.Id_Menu, m.Menu, m.Padre, isnull(m.controlador,'') controlador, isnull(m.Action,'') Action,m.Icono, m.Orden as Menu_Orden
			  from MenuRol mr
			  INNER JOIN Menu m on m.Id_Menu = mr.Id_Menu
			  INNER JOIN Roles r on r.Id_Rol = mr.Id_Rol
			  INNER JOIN Roles_Usuario ru on ru.Id_Rol = r.Id_Rol
			  -- INNER JOIN [SADATABASE]..[DBA].[tb_data_seg_def_user] u on u.user_id = ru.Id_Usuario
			  INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS  u on u.user_id = ru.Id_Usuario
		  where  u.user_id = @id_usuario  and u.user_status = 'A' and m.Estado = 'A' and padre = @padre and u.cia_codigo = @empresa and mr.Estado = 'A'
		  order by m.Orden
	 END
	 ELSE
	 BEGIN
		select m.Id_Menu, m.Menu, m.Padre, isnull(m.controlador,'') controlador, isnull(m.Action,'') Action,m.Icono,  m.Orden as Menu_Orden
		  from MenuRol mr
		  inner join Menu  m on mr.Id_Menu = m.Id_Menu
		  INNER JOIN Roles r on r.Id_Rol = mr.Id_Rol
			  INNER JOIN Roles_Usuario ru on ru.Id_Rol = r.Id_Rol
			  INNER JOIN OPENQUERY(SADATABASE, 'SELECT * FROM [DBA].[tb_data_seg_def_user]' ) AS  u on u.user_id = ru.Id_Usuario
		 where padre = @padre and m.Estado = 'A' and mr.Estado = 'A' and  u.user_id = @id_usuario  and u.user_status = 'A'
		 order by Menu_Orden
	 END

	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaPerCorreo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]


CREATE procedure [dbo].[sp_Quimipac_ConsultaPerCorreo] 
@Correo Varchar(80),
@empresa varchar(2)
 as
begin
Select * from openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr 
		inner join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
		Where usr.user_status In ('A','I') and usr.email=@Correo and usr.cia_codigo = @empresa
end 
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaUsuarios]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[bodegas_web]

--este sp np se esta utilizando ojo
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaUsuarios] 
	-- Add the parameters for the stored procedure here
	@userid varchar(30),
	@userclave varchar(30),
	@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @Exists INT
 
      IF EXISTS(SELECT * FROM [SADATABASE]..[DBA].[tb_data_seg_def_user] as usuario
	   WHERE usuario.user_id = @userid and usuario.user_clave = @userclave and usuario.cia_codigo = @empresa)
      BEGIN
            SET @Exists = 1
      END
      ELSE
      BEGIN
            SET @Exists = 0
      END
 
      RETURN @Exists
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaUsuarios_Permisos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--08-05-2019
--13-04-2019 sy
CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaUsuarios_Permisos]   --sp_Quimipac_ConsultaUsuarios_Permisos '','02'
	@Criterio varchar(10),
	@empresa varchar(2)
AS	
BEGIN
	If	@Criterio ='' Begin

	Select usr.user_id,usr.user_descrip,usr.user_status,usr.email
	,rhp.primer_nombre,rhp.segundo_nombre,rhp.primer_apellido,rhp.segundo_apellido,
	--rhp.nro_identifica1,rhp.correo_laboral	,
	mtPer.* From 
	openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr
	Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
	Left Outer join MT_Permiso as mtPer on usr.user_id = mtPer.Id_Usuario
	where usr.user_status = 'A' And --mtPer.Id_Usuario = @IdUsuario And mtPer.Fecha_Registro =@FechaMax and 
	usr.cia_codigo = @empresa
	Order By user_id Asc
	
	/*
					--		Tabla Temporal #TMPUsuarios
				Declare @TMP_Permisos Table(
						 user_id varchar(20),user_descrip varchar(40),user_status varchar(1),email varchar(80),
						 primer_nombre varchar(20),segundo_nombre varchar(20),primer_apellido varchar(20),segundo_apellido varchar(20),nro_identifica1 varchar(15),correo_laboral  varchar(80)
						,Id_Permiso int,Id_Usuario varchar(20),Id_Cliente int,Id_Contrato int,Id_Proyecto int,Id_Tipo_Trabajo int,Id_Linea varchar(10) ,Consultar  varchar(1)
						,Modificar varchar(1),Crear varchar(1),Eliminar varchar(1),Aprobar varchar(1),Usuario varchar(20),Fecha_Registro DateTime,Estado varchar(1)

				);
				Declare @IdUsuario varchar(20);
				Declare @FechaMax Datetime;

				DECLARE CUR_Usuario CURSOR FOR Select u.Id_Usuario,max(u.Fecha_Registro) as FechaMax 
											   From MT_Permiso u Group by u.Id_Usuario Order BY u.Id_Usuario Desc
				OPEN CUR_Usuario
				FETCH NEXT FROM CUR_Usuario INTO @IdUsuario,@FechaMax

					WHILE @@fetch_status = 0 BEGIN
						Insert Into @TMP_Permisos(user_id,user_descrip,user_status,email, primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,nro_identifica1,correo_laboral
												 ,Id_Permiso,Id_Usuario,Id_Cliente,Id_Contrato,Id_Proyecto,Id_Tipo_Trabajo,Id_Linea,Consultar
												 ,Modificar,Crear,Eliminar,Aprobar,Usuario,Fecha_Registro,Estado)


							Select usr.user_id,usr.user_descrip,usr.user_status,usr.email
							   ,rhp.primer_nombre,rhp.segundo_nombre,rhp.primer_apellido,rhp.segundo_apellido,rhp.nro_identifica1,rhp.correo_laboral
							   ,mtPer.* From 
								openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr
								Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
								Left Outer join MT_Permiso as mtPer on usr.user_id = mtPer.Id_Usuario
								where usr.user_status = 'A'And mtPer.Id_Usuario = @IdUsuario And mtPer.Fecha_Registro =@FechaMax and usr.cia_codigo = @empresa
								Order By user_id Asc
							
						FETCH NEXT FROM CUR_Usuario INTO @IdUsuario,@FechaMax
					END

				CLOSE CUR_Usuario
				DEALLOCATE CUR_Usuario

				Select *From @TMP_Permisos Order By user_id*/
	End	
	Else Begin
				/*NOOSelect * From 
					SADATABASE.[GQ-DB-SGCPM_].DBA.tb_data_seg_def_user as usr 
					Left Outer join SADATABASE.[GQ-DB-SGCPM_].DBA.rh_persona as rhp on usr.email = rhp.correo
					Left Outer join MT_Permiso as mtPer on usr.user_id = mtPer.Id_Usuario
					where usr.user_status = 'A' --And usr.user_id =@Criterio
					Order By user_id Asc*/


					Select usr.user_id,usr.user_descrip,usr.user_status,usr.email
							   ,rhp.primer_nombre,rhp.segundo_nombre,rhp.primer_apellido,rhp.segundo_apellido,
							   --rhp.nro_identifica1,rhp.correo_laboral							   ,
							   mtPer.* From 
								--[SADATABASE]..[DBA].[tb_data_seg_def_user] as usr 
								openquery([SADATABASE], 'select * from tb_data_seg_def_user') as usr
								Left Outer join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
								Left Outer join MT_Permiso as mtPer on usr.user_id = mtPer.Id_Usuario
								where usr.user_status = 'A' And usr.user_id =@Criterio and usr.cia_codigo = @empresa
								Order By user_id Asc
	End
	

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ConsultaUsuarios_Rol]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--como deberia salir SADATABASE.[GQ-DB-SGCPM_].DBA.tb_data_seg_def_user
-- como salio [SADATABASE]..[DBA].[bodegas_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_ConsultaUsuarios_Rol]
	@Usuario varchar(10),
	@empresa varchar(2)
AS	
BEGIN

	 Select
	 usr.user_id
	,usr.user_clave
	,usr.user_descrip
	,usr.user_status
	,usr.user_dep
	,usr.user_cargo
	,usr.user_cheq_nivel
	,usr.user_cheq_maquina
	,usr.user_uni_inc
	,usr.elimina
	,usr.email
	--
	,rhp.id_persona
	,rhp.primer_nombre
	,rhp.segundo_nombre
	,rhp.primer_apellido
	,rhp.segundo_apellido
	,rhp.fecha_nacimiento
	,rhp.genero
	,rhp.estado_civil
	--,rhp.tipo_identifica1
	--,rhp.nro_identifica1
	--,rhp.tipo_identifica2
	--,rhp.nro_identifica2
	,rhp.correo
	--,rhp.nacionalidad
	--,rhp.correo_laboral
	--,rhp.tipo_identifica3
	--,rhp.nro_identifica3
	--
	,rolU.Id_Rol_Usuario
	,rolU.Id_Usuario
	,rolU.Id_Rol
	,rolU.Estado As Estado_UsuarioRol
	--
	,rols.Id_Rol as Id_roles
	,rols.Descripcion
	,rols.Estado
	
	From [SADATABASE]..[DBA].[tb_data_seg_def_user] as usr 
		Left Outer Join [SADATABASE]..[DBA].[rh_persona] as rhp on usr.email = rhp.correo
		Left Outer Join Roles_Usuario as rolU on usr.user_id = rolU.Id_Usuario
		Left Outer Join Roles as rols on rolU.Id_Rol = rols.Id_Rol
		Where usr.user_status = 'A' and usr.cia_codigo = @empresa --And rolU.Estado <> 'I' 
		Order By user_id Asc

END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_CountMT_OrdenTrabajo_XParametro]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_CountMT_OrdenTrabajo_XParametro]
@empresa varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	Declare 
	 @nEnProceso int,
	 @nAlerta	 int,
	 @nCaida     int,
	 @TotalMax		 int,

	 --Select 
	 @selectCount int,
	 @selectDescripcion varchar(30),
	 @Contador int 
	 

	 SET @nEnProceso = 0;
	 SET @nAlerta	 = 0;
	 SET @nCaida     = 0;
	 SET @TotalMax   = 0;

	 --Select 
	 SET @selectCount = 0;

	 Set @Contador   =0;

	CREATE TABLE #TMPCountParametro (
	 CountP int,
	 Descripcion	varchar(20)
	 --CountCaida int	
    );


	CREATE TABLE #TMPResultParametro (
	 Codigo_Interno   varchar(20),
	 Nombre_Completo  varchar(20),
	 Parametro		  varchar(20),
	 Fecha_Asignacion Datetime,
	 codigoDiferente  varchar(20)
	 --CountCaida int	
    );


--

	
	SET @nEnProceso =(select count(*) as nEnProceso
						from MT_OrdenTrabajo a_hijo
						left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
						inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
						inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
						inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
						inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
						where Parametro='En proceso' and con.Id_Empresa = @empresa)
    
	INSERT INTO #TMPCountParametro(CountP, Descripcion)
					Values(@nEnProceso,'En proceso')

	SET @nAlerta = (select count(*) as nAlerta
					from MT_OrdenTrabajo a_hijo
					left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
					inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
					inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
					inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
					inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
					where Parametro='Alerta' and con.Id_Empresa = @empresa)

					INSERT INTO #TMPCountParametro(CountP, Descripcion)
					Values(@nAlerta,'Alerta')

	SET @nCaida =(select count(*) as nCaida
					from MT_OrdenTrabajo a_hijo
					left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
					inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
					inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
					inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
					inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
					where Parametro='Caida' and con.Id_Empresa = @empresa)

					INSERT INTO #TMPCountParametro(CountP, Descripcion)
					Values(@nCaida,'Caida')


					SELECT @selectCount= CountP, @selectDescripcion=Descripcion FROM #TMPCountParametro where CountP=(SELECT MAX(y.CountP) AS TOTAL FROM (select count(*) AS CountP from #TMPCountParametro) y )
					SELECT @TotalMax = MAX(CountP) FROM #TMPCountParametro
					--SELECT @Total 
				
					--select *FROM #TMPCountParametro 				

					  IF @TotalMax= @nEnProceso BEGIN--@selectDescripcion ='En proceso' BEGIN
					  	
						INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
					    select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
						a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
						from MT_OrdenTrabajo a_hijo
						left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
						inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
						inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
						inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
						inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
						where Parametro='En proceso' and con.Id_Empresa = @empresa	
						order by 3 desc;

						IF @nAlerta > 0 BEGIN
							INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
							a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
							from MT_OrdenTrabajo a_hijo
							left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
							inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
							inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
							inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
							inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
							where Parametro='Alerta' and con.Id_Empresa = @empresa	
							order by 3 desc;

							While @nAlerta < @nEnProceso BEGIN 
						
							 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							 VALUES ('','','','','');
							 SET @nAlerta += 1;
							END
						END 
						ELSE BEGIN 
						DECLARE @ContAlerta INT;
						SET @ContAlerta =0;

							While @ContAlerta < @nEnProceso BEGIN 
								 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
								 VALUES ('','','','','');
								 SET @ContAlerta += 1;
							END
						END

						IF @nCaida > 0 BEGIN
							INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
							a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
							from MT_OrdenTrabajo a_hijo
							left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
							inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
							inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
							inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
							inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
							where Parametro='Caida'	and con.Id_Empresa = @empresa
							order by 3 desc;

							While @nCaida < @nEnProceso BEGIN 
						
							 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							 VALUES ('','','','','');
							 SET @nCaida += 1;
							END
						END
						ELSE BEGIN 
						DECLARE @ContCaida INT;
						SET @ContCaida =0;

							While @ContCaida < @nEnProceso BEGIN 
								 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
								 VALUES ('','','','','');
								 SET @ContCaida += 1;
							END
						END
						-- ALERTA  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
					END
					ELSE IF @TotalMax = @nAlerta BEGIN-- @selectDescripcion ='Alerta' BEGIN
					  	
						IF @nEnProceso > 0 BEGIN
						
						INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
					    select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
						a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
						from MT_OrdenTrabajo a_hijo
						left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
						inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
						inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
						inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
						inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
						where Parametro='En proceso' and con.Id_Empresa = @empresa	
						order by 3 desc;	
						
							While @nEnProceso < @nAlerta BEGIN 
						
							 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							 VALUES ('','','','','');
							 SET @nAlerta += 1;
							END
						END
						ELSE BEGIN 
						DECLARE @ContaAlerta INT;
						SET @ContaAlerta =0;

							While @nEnProceso < @nAlerta BEGIN 
								 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
								 VALUES ('','','','','');
								 SET @ContaAlerta += 1;
							END
						END

						INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
					    select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
						a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
						from MT_OrdenTrabajo a_hijo
						left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
						inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
						inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
						inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
						inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
						where Parametro='Alerta'  and con.Id_Empresa = @empresa
						order by 3 desc;
						
						IF @nCaida > 0 BEGIN
							INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
							a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
							from MT_OrdenTrabajo a_hijo
							left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
							inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
							inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
							inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
							inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
							where Parametro='Caida'	and con.Id_Empresa = @empresa
							order by 3 desc;

							While @nCaida < @nAlerta BEGIN 
						
							 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							 VALUES ('','','','','');
							 SET @nCaida += 1;
							END
						END
						ELSE BEGIN 
						DECLARE @ContarCaida INT;
						SET @ContarCaida =0;

							While @ContarCaida < @nAlerta BEGIN 
								 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
								 VALUES ('','','','','');
								 SET @ContarCaida += 1;
							END
						END
					END
		
					 --------------------------------------------------------------------------------------------------------------------

					ELSE IF @TotalMax = @nCaida BEGIN--@selectDescripcion ='Caida' BEGIN
						IF @nEnProceso > 0 BEGIN
						
							INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
							a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
							from MT_OrdenTrabajo a_hijo
							left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
							inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
							inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
							inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
							inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
							where Parametro='En proceso' and con.Id_Empresa = @empresa	
							order by 3 desc;	
						
								While @nEnProceso < @nCaida BEGIN 
						
									INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
									VALUES ('','','','','');
									SET @nEnProceso += 1;
								END
						END
						ELSE BEGIN 
							DECLARE @ContadorProceso INT;
							SET @ContadorProceso =0;

								While @nEnProceso < @nCaida BEGIN 
										INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
										VALUES ('','','','','');
										SET @ContadorProceso += 1;
								END
						END

						IF @nAlerta > 0 BEGIN
							INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
							a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
							from MT_OrdenTrabajo a_hijo
							left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
							inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
							inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
							inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
							inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
							where Parametro='Alerta' and con.Id_Empresa = @empresa	
							order by 3 desc;

							While @nAlerta < @nCaida BEGIN 
						
							 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
							 VALUES ('','','','','');
							 SET @nAlerta += 1;
							END
						END 
						ELSE BEGIN 
							DECLARE @CntAlerta INT;
							SET @CntAlerta =0;

								While @CntAlerta < @nCaida BEGIN 
									 INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
									 VALUES ('','','','','');
									 SET @CntAlerta += 1;
								END
						END

					  	INSERT INTO #TMPResultParametro(Codigo_Interno, Nombre_Completo,Parametro,Fecha_Asignacion,codigoDiferente)
					    select con.Codigo_Interno, Concat(p.primer_nombre, ' ', p.primer_apellido) as Nombre_COmpleto, cp.Parametro,
						a_hijo.Fecha_asignacion_grupo_trabajo,a_hijo.Codigo_Cliente as CodigoDiferente
						from MT_OrdenTrabajo a_hijo
						left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
						inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
						inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
						inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
						inner join [SADATABASE]..[DBA].[rh_persona] as p on oi.ID_Persona = p.id_persona
						where Parametro='Caida'	and con.Id_Empresa = @empresa
						order by 3 desc;
	
					END
		
		SELECT * FROM #TMPResultParametro					  
     				--sp_Quimipac_CountMT_OrdenTrabajo_XParametro	
					--#TMPResultParametro
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Editar_MTPermisos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-------------------------------
  -- Sandro Yagual <13-04-2019> -
  -------------------------------*/

CREATE PROCEDURE [dbo].[sp_Quimipac_Editar_MTPermisos]
 @Id_Permiso Int
,@Consultar  varchar(1)
,@Modificar  varchar(1)
,@Crear      varchar(1)
,@Eliminar   varchar(1)
,@Aprobar   varchar(1)
,@Usuario    varchar(15)
,@Estado     varchar(1)
AS	
BEGIN
	Update MT_Permiso Set Consultar=@Consultar, Modificar=@Modificar, Crear=@Crear, Eliminar=@Eliminar,Aprobar=@Aprobar, Usuario=@Usuario, Estado=@Estado
	Where Id_Permiso = @Id_Permiso
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_GuardarMT_Permisos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--< Sandro Yagual > 13-04-2018--
-- Agregar Aprobar
CREATE PROCEDURE [dbo].[sp_Quimipac_GuardarMT_Permisos]
@Id_Usuario   Varchar(20),
@Id_Cliente	  int,
@Id_Contrato  int,
@Id_Proyecto  int,
@Consultar      Varchar(1),
@Modificar	    Varchar(1),
@Crear		    Varchar(1),
@Eliminar	    Varchar(1),
@Aprobar	    Varchar(1),
@Usuario	    Varchar(20),
@Fecha_Registro DateTime,
@Estado		    Varchar(1)

AS
BEGIN

	Insert Into MT_Permiso(Id_Usuario,Id_Cliente,Id_Contrato,Id_Proyecto,Consultar,Modificar,Crear,Eliminar,Aprobar,Usuario,Fecha_Registro,Estado)
	Values(@Id_Usuario,@Id_Cliente,@Id_Contrato,@Id_Proyecto,@Consultar,@Modificar,@Crear,@Eliminar,@Aprobar,@Usuario,@Fecha_Registro,@Estado);

	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InfoTitulos]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*creacion de proceso*/
CREATE procedure [dbo].[sp_Quimipac_InfoTitulos]
as
begin
/* declaracion de variables*/
declare 
@nombre_MantTipoTrab varchar(50),
@nombre_MantPreReferen varchar(50),
@nombre_MantLuMedi varchar(50),
@nombre_MantGrupTraba varchar(50),
@nombre_ItemMantTiTrab varchar(50),
@nombre_IntegraMantGrupTrab varchar(50),
@nombre_FormActivi varchar(50),
@nombre_EquiMantgrupTrab varchar(50),
@nombre_EdiMantTipTrab varchar(50),
@nombre_EdiMantPreReferen varchar(50),
@nombre_EdiMantLuMedi varchar(50),
@nombre_EdiMantGrupTrab varchar(50),
@nombre_Edi_ItemMantTipoTrab varchar(50),
@nombre_Edi_IntegrMantGrupTrab varchar(50),
@nombre_Edi_EquiMantGrupTrab varchar(50),
@nombre_Edi_ActMantTipTrab varchar(50),
@nombre_Agg_FormActiv varchar(50),
@nombre_Agg_MantTipoTrab varchar(50),
@nombre_Agg_MantPreRefer varchar(50),
@nombre_Agg_MantLugMedi varchar(50),
@nombre_Agg_MantGrupTrab varchar(50),
@nombre_Agg_ItemMantTipTrab varchar(50),
@nombre_Agg_IntegrMantGrupTrab varchar(50),
@nombre_Agg_EquiMantGrupTrab varchar(50),
@nombre_Agg_ActiMantTipTrab varchar(50),
@nombre_Acti_MantTipTrab varchar(50)
/* set */
set @nombre_MantTipoTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_MantPreReferen='Esta pantalla sirve ahhaahahahah'
set @nombre_MantLuMedi='Esta pantalla sirve ahhaahahahah'
set @nombre_MantGrupTraba='Esta pantalla sirve ahhaahahahah'
set @nombre_ItemMantTiTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_IntegraMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_FormActivi='Esta pantalla sirve ahhaahahahah'
set @nombre_EquiMantgrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_EdiMantTipTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_EdiMantPreReferen='Esta pantalla sirve ahhaahahahah'
set @nombre_EdiMantLuMedi='Esta pantalla sirve ahhaahahahah'
set @nombre_EdiMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Edi_ItemMantTipoTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Edi_IntegrMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Edi_EquiMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Edi_ActMantTipTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_FormActiv='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_MantTipoTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_MantPreRefer='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_MantLugMedi='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_MantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_ItemMantTipTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_IntegrMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_EquiMantGrupTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Agg_ActiMantTipTrab='Esta pantalla sirve ahhaahahahah'
set @nombre_Acti_MantTipTrab='Esta pantalla sirve ahhaahahahah'

end
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Insertar_OrdenTrabajoExcel]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <13/03/2020>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Insertar_OrdenTrabajoExcel]
	-- Add the parameters for the stored procedure here
@Identificador varchar(max),
@Numerador varchar(max),
@Estado varchar(max),
@Producto varchar(max),
@Unidad_de_Trabajo varchar(max),
@Estado_Unidad_de_Trabajo varchar(max),
@Fecha_de_Creación Datetime,
@Fecha_de_Asignación Datetime,
@Tipo_de_Asignación varchar(max),
@Fecha_Estimada_de_Ejecución Datetime,
@Fecha_Máxima_para_Legalización Datetime,
@Última_Reprogramación Datetime,
@Fecha_de_Legalización Datetime,
@Inicio_de_Ejecución Datetime,
@Fin_de_Ejecución Datetime,
@Causal_de_Legalización varchar(max),
@Personal varchar(max),
@Valor_de_la_Orden varchar(max),
@Número_de_Veces_Impresa varchar(max),
@Intentos_de_Legalización varchar(max),
@Anula_Otra_Orden varchar(max),
@Tipo_de_Compromiso varchar(max),
@Cita_Confirmada varchar(max),
@Consecutivo_Ruta varchar(max),
@Ruta varchar(max),
@Nombre_Ruta varchar(max),
@Dirección varchar(max),
@Ubicación_Geográfica varchar(max),
@Cliente varchar(max),
@Nombre_Suscriptor varchar(max),
@Apellido_Suscriptor varchar(max),
@Teléfono_Cliente varchar(max),
@Puntaje_Cliente varchar(max),
@Esfuerzo_de_la_Orden varchar(max),
@Comentario varchar(max),
@Tipo_de_Comentario varchar(max),
@Unidad_Asociada varchar(max),
@Ofertada varchar(max),
@Proyecto varchar(max),
@Etapa varchar(max),
@Estado_de_la_Orden varchar(max),
@PARENT_ID varchar(max),
@tipo_trabajo varchar(max),
@sectores varchar(max),
@tipo_trabajo_planeado varchar(max),
@Nivel_prioridades varchar(max),
@barrio varchar(max),
@tipo_clientes varchar(max),
@Hora_Acordada Time,
@usuario varchar(max),
@contrato_excel int,
@empresa varchar(5),
@fecha_maxima_contratista Datetime,
@fecha_finalizacion_obra Datetime,
@automatizacion bit,
@variablecodigo varchar(max),
@variabletpcodigo varchar(max)

AS
BEGIN
	
	declare @tipo_trabajorec int,
	@tipo_trabajoej int,
	@sector int,
	@nivel_prioridad int,
	@barrios int,
	@tipo_cliente int,
	@fechaRegistro datetime,
	@contrato int,
	@estado2 int,
	@nuevaOrden int,
	@cliente_contrato int,
	@campamento int

	set @fechaRegistro = convert(datetime,getdate(),111);
	
	
	set @estado2 = 30
	set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contrato_excel)
	select @cliente_contrato as cliente_contrato
	--set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Codigo_Cliente and EstadoEditOrden = 'A' and Id_contrato = @Id_contrato)
	set @tipo_trabajorec = (select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Codigo = @variablecodigo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato)
	select @tipo_trabajorec as tipo_trabajorec
	set @tipo_trabajoej = (select Id_TipoTrabajo from MT_TipoTrabajo mtej inner join MT_Servicio ms on mtej.Id_Servicio= ms.Id_Servicio where mtej.Descripcion = @tipo_trabajo_planeado and mtej.Codigo = @variabletpcodigo and mtej.Estado = 'A' and ms.Id_Empresa = @empresa and mtej.Id_Cliente = @cliente_contrato)
	select @tipo_trabajoej as tipo_trabajoej
	set @sector = (select Id_Sector from MT_Sector where Nombre = @sectores and Estado = 'A')
	select @sector as sector
	set @nivel_prioridad = (select Id_TablaDetalle from MT_TablaDetalle where Descripcion = @Nivel_prioridades and Estado = 'A' and Id_Tabla = 11)
	select @nivel_prioridad as nive_prioridad
	set @barrios = (select Id_Sector from MT_Sector where Nombre = @barrio and Estado = 'A')
	select @barrios as barrios
	set @tipo_cliente = (select Id_TablaDetalle from MT_TablaDetalle where Descripcion = @tipo_clientes and Estado = 'A' and Id_Tabla = 57)
	select @tipo_cliente as tipo_cliente
	set @campamento = (select top(1)Id_Campamento from MT_Campamento where Estado = 'A')
	select @campamento as campamento

	IF((@tipo_trabajorec!= '' and @tipo_trabajoej!= '') OR (@tipo_trabajorec!= NULL and @tipo_trabajoej!= NULL))
		
	BEGIN
	INSERT INTO Mt_OrdenTrabajo(Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_usuario, Nivel_prioridad, 
	Fecha_registro, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, /*Fecha_asignacion_grupo_trabajo*/ Fecha_asignacion, 
	Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, 
	Telefono_suscriptor, Comentario, Codigo_Cliente, EstadoEditOrden, Id_contrato, Id_campamento, Excel_orden, Id_sucursal, Id_orden_padre, 
	Id_estacion, Fecha_maxima_contratista, Fecha_inicio_ejecucion, Fecha_finalizacion_obra, Automatizacion)
	 VALUES (@tipo_trabajorec,@tipo_trabajoej,@estado2,@sector,@usuario, @nivel_prioridad,@fechaRegistro,
	 @Fecha_de_Creación,@Fecha_Estimada_de_Ejecución,@Fecha_de_Asignación,@Fecha_Máxima_para_Legalización,@Hora_acordada,@barrios,
	@Dirección,@Cliente,@Nombre_suscriptor,@tipo_cliente,@Teléfono_Cliente,@Comentario,
	@Identificador, 'A',@contrato_excel, @campamento, 1, 0,0,0, @fecha_maxima_contratista, @Inicio_de_Ejecución, @fecha_finalizacion_obra, @automatizacion)

	set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Identificador and EstadoEditOrden = 'A' and Id_contrato = @contrato_excel)
	select @nuevaOrden AS NUEVA_ORDEN

	INSERT INTO MT_OrdenTrabajo_Estado(Id_orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO) values(@nuevaOrden, @estado2, @fechaRegistro, null, 'A')
	
	
	select @nuevaOrden

	END

	else

	begin
	select -1;
	end
	
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Insertar_OrdenTrabajoExcel_Automat]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <13/03/2020>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Insertar_OrdenTrabajoExcel_Automat]
	-- Add the parameters for the stored procedure here
@Identificador varchar(max),
@Estado varchar(max),
@Fecha_de_Creación Datetime,
@Fecha_de_Asignación Datetime,
@Inicio_de_Ejecución Datetime,
@fecha_finalizacion_obra Datetime,
@tipo_trabajo varchar(max),
@estacion varchar(max),
@usuario varchar(max),
@contrato_excel int,
@empresa varchar(5),
@automatizacion bit

AS
BEGIN
	
	declare @tipo_trabajorec int,	
	
	@fechaRegistro datetime,
	@contrato int,
	@estado2 int,
	@nuevaOrden int,
	@cliente_contrato int,
	@campamento int,
	@estacion_2 int,
	@estacion_id int
	
	set @fechaRegistro = convert(datetime,getdate(),111);
	
	
	set @estado2 = 30
	set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contrato_excel)
	select @cliente_contrato as cliente_contrato
	set @tipo_trabajorec = (select top(1)Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato)
	select @tipo_trabajorec as tipo_trabajorec	

	set @estacion_2 = (select Id_Estacion from MT_Estacion mtest  where mtest.Nombre = @estacion and mtest.Estado = 'A')
	select @estacion_2 as estacion
	
	if(@estacion_2 != null OR @estacion_2 != '')	
	begin
	--insert into MT_Estacion([Id_Cliente],[Id_Provincia],[Id_Ciudad],[Id_Macrosector],[Nombre],[Direccion],[Estado],[Coordenada_X],[Coordenada_Y])
	--values()
	set @estacion_id = @estacion_2
	end
	
	set @campamento = (select top(1)Id_Campamento from MT_Campamento where Estado = 'A')
	select @campamento as campamento

	IF((@tipo_trabajorec!= '') OR (@tipo_trabajorec!= NULL))
		
	BEGIN
	INSERT INTO Mt_OrdenTrabajo(Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_usuario,Fecha_registro, Fecha_creacion_cliente,  Fecha_asignacion, 
	  Codigo_Cliente, EstadoEditOrden, Id_contrato, Id_campamento, Excel_orden,Id_estacion, Fecha_inicio_ejecucion, Fecha_finalizacion_obra, Automatizacion)
	 VALUES (@tipo_trabajorec,@tipo_trabajorec,@estado2,@usuario,@fechaRegistro,	 @Fecha_de_Creación,@Fecha_de_Asignación,
	@Identificador, 'A',@contrato_excel, @campamento, 1, @estacion_id,@Inicio_de_Ejecución, @fecha_finalizacion_obra, @automatizacion)

	set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Identificador and EstadoEditOrden = 'A' and Id_contrato = @contrato_excel)
	select @nuevaOrden AS NUEVA_ORDEN

	INSERT INTO MT_OrdenTrabajo_Estado(Id_orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO) values(@nuevaOrden, @estado2, @fechaRegistro, null, 'A')
	
	
	select @nuevaOrden

	END

	else

	begin
	select -1;
	end
	
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Insertar_PlanillaCabExcel]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <13/03/2020>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Insertar_PlanillaCabExcel]
	-- Add the parameters for the stored procedure here
--Identificador, Unidad_de_Trabajo, fecha_asignacion_, fecha_leg_, inicio_ej_, fin_ej_,  Ubicacion_Geográfica,  variable,  usuario, contrato_proyecto, empresa_id.ToString(), Tipo

@Identificador varchar(max),
@Unidad_de_Trabajo varchar(max),
@Fecha_de_Asignación Datetime,
@Fecha_de_Legalización Datetime,
@Inicio_de_Ejecución Datetime,
@Fin_de_Ejecución Datetime,
@Ubicación_Geográfica varchar(max),
@tipo_trabajo varchar(max),
@usuario varchar(max),
@contratoproy_excel int,
@empresa varchar(5),
@tipo int

AS
BEGIN
	
	declare @tipo_trabajorec int,	
	@fechaRegistro datetime,
	@contrato int,
	@estado2 int,	
	@nuevaPlanilla int,
	@cliente_contrato int

	set @fechaRegistro = convert(datetime,getdate(),111);
	
	
	set @estado2 = 30
	set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contratoproy_excel)
	select @cliente_contrato as cliente_contrato
	--set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Codigo_Cliente and EstadoEditOrden = 'A' and Id_contrato = @Id_contrato)
	set @tipo_trabajorec = (select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato)
	select @tipo_trabajorec as tipo_trabajorec
	
	IF((@tipo_trabajorec!= '') OR (@tipo_trabajorec!= NULL))
		
	BEGIN
	INSERT INTO MT_Planilla(Id_PoyectoContr,Fecha_Asignacion,Fecha_Inicio,Fecha_Fin,Usuario,Estado,Identificador,Tipo_Trabajo,Unidad_Trabajo,Fecha_Legalizacion,Ubicacion_Geografica,Tipo_C_P)
	 VALUES (@contratoproy_excel,@Fecha_de_Asignación, @Inicio_de_Ejecución, @Fin_de_Ejecución,@usuario,'1',@Identificador, @tipo_trabajorec, @Unidad_de_Trabajo, @Fecha_de_Legalización, @Ubicación_Geográfica,@tipo)

	set @nuevaPlanilla = (select Id_Planilla from MT_Planilla where Identificador = @Identificador and Estado = '1')
	select @nuevaPlanilla AS NUEVA_PLANILLA

	
	
	
	select @nuevaPlanilla

	END

	else

	begin
	select -1;
	end
	
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Insertar_PlanillaDetExcel]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <13/03/2020>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_Insertar_PlanillaDetExcel]
	-- Add the parameters for the stored procedure here
--Identificador, Unidad_de_Trabajo, fecha_asignacion_, fecha_leg_, inicio_ej_, fin_ej_,  Ubicacion_Geográfica,  variable,  usuario, contrato_proyecto, empresa_id.ToString(), Tipo

@Código_Acta varchar(max),
@Base_Administrativa varchar(max), 
@Descripción_de_Acta  varchar(max),
@Código_Periodo varchar(max),
@Descripción_de_Periodo varchar(max),
@Código_Tipo_de_Acta varchar(max),
@Descripción_de_Tipo_de_Acta varchar(max),
@fecha_Inicial_Periodo  Datetime,
@fecha_Final_Periodo Datetime, 
@fecha_Cierre_Acta  Datetime,
@Estado_Acta varchar(max),
@Descripción_de_Estado_Acta varchar(max),
@Orden_Raiz varchar(max),
@No_Orden_Padre varchar(max),
@No_Orden  varchar(max),
@Código_Detalle_Acta varchar(max), 
@Código_Items  varchar(max),
@Descripción_de_Items varchar(max),
@Cantidad varchar(max),
@Código_Unidad varchar(max),
@Unidad varchar(max),
@Valor_Unitario  varchar(max),
@Valor_Total varchar(max), 
@Código_Listado_de_Costo  varchar(max),
@Descripción_Listado_de_Costo varchar(max),
@Tipo_Generación varchar(max),
@Código_de_Plan_de_Condición varchar(max),
@Tipo_de_Trabajo varchar(max),
@Descripción_de_Tipo_de_Trabajo varchar(max), 
@Actividad  varchar(max),
@fecha_Asignación  Datetime,
@fecha_Fin_Permiso_Municipal_Calc Datetime,
@fecha_Asignación_OT Datetime,
@fecha_Ejecución Datetime,
@fecha_Legalización Datetime,
@fecha_Ejecucion_OT_Padre  Datetime,
@Tiempo_Promedio_Inc_Desc_OT  varchar(max),
@Tiempo_Promedio_Desc_Descarga  varchar(max),
@Tiempo_Transcurrido_HORAS varchar(max),
@Tiempo_Optimo_Incentivo varchar(max),
@Tiempo_Máximo_Incentivo varchar(max),
@Porcentaje_Incentivo varchar(max),
@Tiempo_Optimo_Desc_Atención varchar(max), 
@Tiempo_Máximo_Desc_Atención varchar(max),
@Porcentaje_de_Desc_Atención  varchar(max),
@Tiempo_Máximo_Desc_Descarga varchar(max),
@Tiempo_Excede_Desc_Descarga varchar(max),
@Porcentaje_Desc_Descarga varchar(max),
@Valor_Aplica_Desc_Atención_OT varchar(max),
@Código_Contrato varchar(max), 
@Descripción_Contrato  varchar(max),
@Tipo_de_Contrato  varchar(max),
@Descripción_de_Tipo_de_Contrato varchar(max),
@Contratista varchar(max),
@Descripción_Contratista varchar(max),
@Porcentaje_Costo_Indirecto_Contrato varchar(max),
@Porcentaje_Fondo_Garantia_Contrato varchar(max), 
@Porcentaje_Amortización_Contrato  varchar(max),
@Descuento_General  varchar(max),
@Código_Unidad_Operativa varchar(max),
@Descripción_Unidad_Operativa varchar(max),
@Terminal varchar(max),
@Valor_Base varchar(max),
@Cumplimiento  varchar(max),
@Tiempo_Contractual_HORAS varchar(max), 
@Tiempo_de_Permiso_Municipal_HORAS  varchar(max),
@Tipo_Clasifica varchar(max),
@Inventario varchar(max),
@Tiene_Caso_Especial varchar(max),
@Tipoe_de_Relación varchar(max),
@Nro_Contrato varchar(max), 
@Nro_Producto varchar(max), 
@Código  varchar(max),
@Tipo_de_Irregularidad varchar(max),
@Descripción_de_Proyecto varchar(max),
@Usuario_Finalizo varchar(max),
@Código_Tipo_Comentario varchar(max),
@Comentario varchar(max),
@Observación_Orden_Actividad varchar(max),
@fecha_Pago_Sistema_Externo Datetime,
@Número_Factura_Sistema_Externo varchar(max),
@Cod_estado_orden varchar(max),
@usuario varchar(max),
@contratoproy_excel int,
@empresa varchar(5),
@tipo int

AS
BEGIN
	
	declare @tipo_trabajorec int,	
	@fechaRegistro datetime,
	@contrato int,
	@estado2 int,	
	@nuevaPlanilla int,
	@cliente_contrato int

	set @fechaRegistro = convert(datetime,getdate(),111);
	
	
	set @estado2 = 30
	--set @cliente_contrato = (select Id_Cliente from MT_Contrato where Id_Contrato = @contratoproy_excel)
	--select @cliente_contrato as cliente_contrato
	--set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Codigo_Cliente and EstadoEditOrden = 'A' and Id_contrato = @Id_contrato)
	--set @tipo_trabajorec = (select Id_TipoTrabajo from MT_TipoTrabajo mt inner join MT_Servicio ms on mt.Id_Servicio= ms.Id_Servicio where mt.Descripcion = @tipo_trabajo and mt.Estado = 'A' and ms.Id_Empresa = @empresa and mt.Id_Cliente= @cliente_contrato)
	--select @tipo_trabajorec as tipo_trabajorec
	
	--IF((@tipo_trabajorec!= '') OR (@tipo_trabajorec!= NULL))
		
	--BEGIN
	INSERT INTO MT_Planilla_Detalle(Codigo_acta,	Base_Admnistrativa,	Descripcion_de_Acta,	Codigo_Periodo,	Descripcion_de_Periodo,	Codigo_Tipo_de_Acta,	Descripcion_de_Tipo_de_Acta,	Fecha_Inicial_Periodo,	Fecha_Final_Periodo,	Fecha_Cierre_Acta,	Estado_Acta,	Descripcion_de_Estado_Acta,	Orden_Raiz,	No_Orden_Padre,	No_Orden,	Codigo_Detalle_Acta,	Código_Items,	Descripcion_de_Items,	Cantidad,	Codigo_Unidad,	Unidad,	Valor_Unitario,	Valor_Total,	Codigo_Listado_de_Costo,	Descripcion_Listado_de_Costo,	Tipo_Generacion,	Codigo_de_Plan_de_Condicion,	Tipo_de_Trabajo,	Descripcion_de_Tipo_de_Trabajo,	Actividad,	Fecha_Asignacion,	Fecha_Fin_Permiso_Municipal_Calc,	Fecha_Asignacion_OT,	Fecha_Ejecucion,	Fecha_Legalizacion,	Fecha_Ejecucion_OT_Padre,	Tiempo_Promedio_Inc_Desc_OT,	Tiempo_Promedio_Desc_Descarga,	Tiempo_Transcurrido_HORAS,	Tiempo_Optimo_Incentivo,	Tiempo_Máximo_Incentivo,	Porcentaje_Incentivo,	Tiempo_Optimo_Desc_Atencion,	Tiempo_Máximo_Desc_Atencion,	Porcentaje_de_Desc_Atencion,	Tiempo_Máximo_Desc_Descarga,	Tiempo_Excede_Desc_Descarga,	Porcentaje_Desc_Descarga,	Valor_Aplica_Desc_Atencion_OT,	Codigo_Contrato,	Descripcion_Contrato,	Tipo_de_Contrato,	Descripcion_de_Tipo_de_Contrato,	Contratista,	Descripcion_Contratista,	Porcentaje_Costo_Indirecto_Contrato,	Porcentaje_Fondo_Garantia_Contrato,	Porcentaje_Amortizacion_Contrato,	Descuento_General,	Codigo_Unidad_Operativa,	Descripcion_Unidad_Operativa,	Terminal,	Valor_Base,	Cumplimiento,	Tiempo_Contractual_HORAS,	Tiempo_de_Permiso_Municipal_HORAS,	Tipo_Clasifica,	Inventario,	Tiene_Caso_Especial,	Tipoe_de_Relacion,	Nro_Contrato,	Nro_Producto,	Codigo,	Tipo_de_Irregularidad,	Descripcion_de_Proyecto,	Usuario_Finalizo,	Codigo_Tipo_Comentario,	Comentario,	Observacion_Orden_Actividad,	Fecha_Pago_Sistema_Externo,	Numero_Factura_Sistema_Externo,	Cod_estado_orden)
	 VALUES (@Código_Acta,	@Base_Administrativa, 	@Descripción_de_Acta ,	@Código_Periodo,	@Descripción_de_Periodo,	@Código_Tipo_de_Acta,	@Descripción_de_Tipo_de_Acta,	@fecha_Inicial_Periodo ,	@fecha_Final_Periodo, 	@fecha_Cierre_Acta ,	@Estado_Acta,	@Descripción_de_Estado_Acta,	@Orden_Raiz,	@No_Orden_Padre,	@No_Orden ,	@Código_Detalle_Acta, 	@Código_Items ,	@Descripción_de_Items,	@Cantidad,	@Código_Unidad,	@Unidad,	@Valor_Unitario ,	@Valor_Total, 	@Código_Listado_de_Costo ,	@Descripción_Listado_de_Costo,	@Tipo_Generación,	@Código_de_Plan_de_Condición,	@Tipo_de_Trabajo,	@Descripción_de_Tipo_de_Trabajo, 	@Actividad ,	@fecha_Asignación ,	@fecha_Fin_Permiso_Municipal_Calc,	@fecha_Asignación_OT,	@fecha_Ejecución,	@fecha_Legalización,	@fecha_Ejecucion_OT_Padre ,	@Tiempo_Promedio_Inc_Desc_OT ,	@Tiempo_Promedio_Desc_Descarga ,	@Tiempo_Transcurrido_HORAS,	@Tiempo_Optimo_Incentivo,	@Tiempo_Máximo_Incentivo,	@Porcentaje_Incentivo,	@Tiempo_Optimo_Desc_Atención, 	@Tiempo_Máximo_Desc_Atención,	@Porcentaje_de_Desc_Atención ,	@Tiempo_Máximo_Desc_Descarga,	@Tiempo_Excede_Desc_Descarga,	@Porcentaje_Desc_Descarga,	@Valor_Aplica_Desc_Atención_OT,	@Código_Contrato, 	@Descripción_Contrato ,	@Tipo_de_Contrato ,	@Descripción_de_Tipo_de_Contrato,	@Contratista,	@Descripción_Contratista,	@Porcentaje_Costo_Indirecto_Contrato,	@Porcentaje_Fondo_Garantia_Contrato, 	@Porcentaje_Amortización_Contrato ,	@Descuento_General ,	@Código_Unidad_Operativa,	@Descripción_Unidad_Operativa,	@Terminal,	@Valor_Base,	@Cumplimiento ,	@Tiempo_Contractual_HORAS, 	@Tiempo_de_Permiso_Municipal_HORAS ,	@Tipo_Clasifica,	@Inventario,	@Tiene_Caso_Especial,	@Tipoe_de_Relación,	@Nro_Contrato, 	@Nro_Producto, 	@Código ,	@Tipo_de_Irregularidad,	@Descripción_de_Proyecto,	@Usuario_Finalizo,	@Código_Tipo_Comentario,	@Comentario,	@Observación_Orden_Actividad,	@fecha_Pago_Sistema_Externo,	@Número_Factura_Sistema_Externo,	@Cod_estado_orden)

	--set @nuevaPlanilla = (select Id_Planilla from MT_Planilla where Identificador = @Identificador and Estado = '1')
	--select @nuevaPlanilla AS NUEVA_PLANILLA

	
	
	
	select @nuevaPlanilla

	--END

	--else

	--begin
	--select -1;
	--end
	
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarContratoSysbase]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarContratoSysbase]
	-- Add the parameters for the stored procedure here
	  @id_contrato int,
	  @cia_codigo  varchar(5),  
	  @cod_cliente varchar(10) ,  
	  @fecha_inicial DateTime ,  
	  @fecha_fin  DateTime,  
	  @codigo_secuencial_interno  varchar(75),  
	  @codigo_contrato_asociado  varchar(75),  
	  @user_id varchar(10),  
	  @unidad  varchar(10), 
	  @cod_servicio varchar(10),  
	  @codcen  varchar(13),  
	  @detalle  varchar(max),
	  @id_estado int, 
	  @plazo_contrato  int,  
	  @cod_tipo int,  
	  @contrato_Padre int,
	  @valor_referencial numeric(18,4),
	  @monto numeric(18,4),  
	  @costo numeric(18,4), 
	  @responsable int,
	  @secuencial int,  
	  @codigo_secuencial_interno_anterior varchar(75),	  
	  @observaciones varchar(max),
	  @codigo_secuencial_interno_padre varchar(75),
	  @fecha_registro Datetime,
	  @fecha_modificacion Datetime,
	  @Localidad varchar(2),
	  @Fecha_Aprobacion_Cot datetime,
	  @Recepcion_Servicio varchar(max),
	  @Fecha_Firma_Conformidad datetime,
	  @Fecha_Cumplimiento_Inst datetime
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	if exists (select * from [SADATABASE]..[DBA].[MT_Contrato] where Id_Contrato = @id_contrato)
   begin
   update [SADATABASE]..[DBA].[MT_Contrato] set Id_Empresa = @cia_codigo, Id_Cliente =@cod_cliente, Fecha_Inicio = @fecha_inicial, Fecha_Fin= @fecha_fin, Codigo_Interno = @codigo_secuencial_interno,
	Codigo_Cliente = @codigo_contrato_asociado, Id_Usuario = @user_id, Id_Linea = @unidad, Categoria = @cod_servicio, Subcategoria = @codcen, Nombre = @detalle, Estado = @id_estado, Dia_Plazo=@plazo_contrato, tipo=@cod_tipo, Contrato_Padre=@contrato_Padre, Valor_Referencial=@valor_referencial,
	monto=@monto,costo=@costo, Responsable=@responsable, Secuencial=@secuencial, Codigo_Interno_Ant=@codigo_secuencial_interno_anterior, Observaciones=@observaciones, Codigo_interno_padre=@codigo_secuencial_interno_padre, Fecha_registro=@fecha_registro, Fecha_modificacion=@fecha_modificacion, Localidad = @Localidad, 
	Fecha_Aprobacion_Cot = @Fecha_Aprobacion_Cot, Recepcion_Servicio = @Recepcion_Servicio, Fecha_Firma_Conformidad = @Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst = @Fecha_Cumplimiento_Inst
	where Id_Contrato = @id_contrato
   end
   else
   begin
	INSERT INTO [SADATABASE]..[DBA].[MT_Contrato](Id_Contrato,Id_Empresa, Id_Cliente, Fecha_Inicio, Fecha_Fin, Codigo_Interno,
		Codigo_Cliente, Id_Usuario, Id_Linea, Categoria, Subcategoria, Nombre, Estado, Dia_Plazo, tipo, Contrato_Padre, Valor_Referencial,
		monto,costo, Responsable, Secuencial, Codigo_Interno_Ant, Observaciones, Codigo_interno_padre,Fecha_registro,Fecha_modificacion, Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst)
		values( @id_contrato,@cia_codigo, @cod_cliente, @fecha_inicial, @fecha_fin, @codigo_secuencial_interno,
		@codigo_contrato_asociado, @user_id, @unidad, @cod_servicio, @codcen, @detalle, @id_estado, @plazo_contrato, @cod_tipo, @contrato_Padre, @valor_referencial,
		@monto, @costo, @responsable, @secuencial, @codigo_secuencial_interno_anterior, @observaciones, @codigo_secuencial_interno_padre, @fecha_registro, @fecha_modificacion, @Localidad, @Fecha_Aprobacion_Cot, @Recepcion_Servicio, @Fecha_Firma_Conformidad, @Fecha_Cumplimiento_Inst)
	
	end

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarContratoSysbase_Job]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarContratoSysbase_Job]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	IF  EXISTS(select * from MT_Contrato where Fecha_modificacion >= CAST(CAST(GETDATE() AS DATE) AS DATETIME) and Fecha_modificacion < CAST(CAST(GETDATE() + 1 AS DATE) AS DATETIME))

	begin
	UPDATE
    Table_A
    SET
    
    Table_A.Id_Empresa = Table_B.Id_Empresa, Table_A.Id_Cliente=Table_B.Id_Cliente, Table_A.Fecha_Inicio = Table_B.Fecha_Inicio,
	Table_A.Fecha_Fin = Table_B.Fecha_Fin, Table_A.Codigo_Interno = Table_B.Codigo_Interno,
	Table_A.Codigo_Cliente = Table_B.Codigo_Cliente	, Table_A.Id_Usuario = Table_B.Id_Usuario , 
	Table_A.Id_Linea = Table_B.Id_Linea , Table_A.Categoria = Table_B.Categoria , Table_A.Subcategoria = Table_B.Subcategoria , 
	Table_A.Nombre = Table_B.Nombre , Table_A.Estado = Table_B.Estado  , Table_A.Dia_Plazo = Table_B.Dia_Plazo , 
	Table_A.tipo = Table_B.tipo, Table_A.Contrato_Padre = Table_B.Contrato_Padre, Table_A.Valor_Referencial = Table_B.Valor_Referencial,
	Table_A.monto = Table_B.monto	, Table_A.costo = Table_B.costo , Table_A.Responsable = Table_B.Responsable , 
	Table_A.Secuencial = Table_B.Secuencial , Table_A.Codigo_Interno_Ant = Table_B.Codigo_Interno_Ant , 
	Table_A.Observaciones = Table_B.Observaciones , Table_A.Codigo_interno_padre = Table_B.Codigo_interno_padre , 
	Table_A.Fecha_registro = Table_B.Fecha_registro , Table_A.Fecha_modificacion = Table_B.Fecha_modificacion, Table_A.Localidad = Table_B.Localidad, 
	Table_A.Fecha_Aprobacion_Cot = Table_B.Fecha_Aprobacion_Cot, Table_A.Recepcion_Servicio = Table_B.Recepcion_Servicio, Table_A.Fecha_Firma_Conformidad = Table_B.Fecha_Firma_Conformidad, Table_A.Fecha_Cumplimiento_Inst = Table_B.Fecha_Cumplimiento_Inst

    FROM
    [SADATABASE]..[DBA].[MT_Contrato] AS Table_A
    INNER JOIN MT_Contrato AS Table_B ON Table_A.Id_Contrato = Table_B.Id_Contrato
	where Table_B.Fecha_modificacion >= CAST(CAST(GETDATE() AS DATE) AS DATETIME) and Table_B.Fecha_modificacion < CAST(CAST(GETDATE() + 1 AS DATE) AS DATETIME)

	end


		IF  EXISTS(select * from MT_Contrato where Fecha_registro >= CAST(CAST(GETDATE() AS DATE) AS DATETIME) and Fecha_registro < CAST(CAST(GETDATE() + 1 AS DATE) AS DATETIME))

		begin
		delete from [SADATABASE]..[DBA].[MT_Contrato]  where Fecha_registro >= convert(varchar,GETDATE(),111) and Fecha_registro < convert(varchar,GETDATE()+1,111)
		INSERT INTO [SADATABASE]..[DBA].[MT_Contrato](Id_Contrato,Id_Empresa, Id_Cliente, Fecha_Inicio, Fecha_Fin, Codigo_Interno,
		Codigo_Cliente, Id_Usuario, Id_Linea, Categoria, Subcategoria, Nombre, Estado, Dia_Plazo, tipo, Contrato_Padre, Valor_Referencial,
		monto,costo, Responsable, Secuencial, Codigo_Interno_Ant, Observaciones, Codigo_interno_padre,Fecha_registro,Fecha_modificacion, Localidad, Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst)
		SELECT Id_Contrato,Id_Empresa, Id_Cliente, convert(datetime,Fecha_Inicio,111), convert(datetime,Fecha_Fin,111), Codigo_Interno,
		Codigo_Cliente, Id_Usuario, Id_Linea, Categoria, Subcategoria, Nombre, Estado, Dia_Plazo, tipo, Contrato_Padre, Valor_Referencial,
		monto, costo, Responsable, Secuencial, Codigo_Interno_Ant, Observaciones, Codigo_interno_padre, convert(datetime,Fecha_registro,111), convert(datetime,Fecha_modificacion,111), Localidad , Fecha_Aprobacion_Cot, Recepcion_Servicio, Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst FROM MT_Contrato
		where Fecha_registro >= CAST(CAST(GETDATE() AS DATE) AS DATETIME) and Fecha_registro < CAST(CAST(GETDATE() + 1 AS DATE) AS DATETIME)
		end

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarEquipoOrdenTrabajo]    Script Date: 04/10/2020 22:26:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarEquipoOrdenTrabajo]
	-- Add the parameters for the stored procedure here
	@id_orden int
	--@id_grupo int,
	--@fecha_inicio datetime,
	--@fecha_fin datetime,
	--@estado varchar(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	--INSERT INTO MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)
	--values(@id_orden, @id_grupo, @fecha_inicio, @fecha_fin, @estado)

	INSERT INTO MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)(select DISTINCT moi.Id_Orden_Trabajo, moi.Id_GrupoTrabajo, moi.Fecha_Inicio, moi.Fecha_Fin, moi.Estado from MT_OrdenTrabajo_Integrante moi
																										    where moi.Id_Orden_Trabajo = @id_orden)
	

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarFecha_Prorroga]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<VQ>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarFecha_Prorroga]
	-- Add the parameters for the stored procedure here
	
	@Id_contrato int,
	@Estado varchar(1),
	@dia int,
	@descripcion varchar(max)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	

  
	declare @fechaprorroga datetime, @fechaantprorroga datetime
		
    set @fechaantprorroga = (select Fecha_Prorroga from MT_Contrato_Prorroga WHERE Id_Contrato = @Id_contrato and Estado = 'A')
	UPDATE MT_Contrato_Prorroga  SET Estado = 'I'
				WHERE Id_Contrato = @Id_contrato and Fecha_Prorroga = @fechaantprorroga
   

   set @fechaprorroga = (SELECT DATEADD(DAY,@dia,@fechaantprorroga));
	
	

	INSERT INTO MT_Contrato_Prorroga(Id_contrato, Estado, Dia_Prorroga, Fecha_Prorroga, Descripcion)
	 VALUES (@Id_contrato,'A',@dia, @fechaprorroga, @descripcion)


	 UPDATE MT_Contrato  SET Fecha_Fin = @fechaprorroga
				WHERE Id_Contrato = @Id_contrato 
	
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarIntegranteOrdenTrabajo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarIntegranteOrdenTrabajo]
	-- Add the parameters for the stored procedure here
	@id_orden int,
	@id_grupo int,
	@fecha_inicio datetime,
	@fecha_fin datetime
	--@estado varchar(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	--INSERT INTO MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)
	--values(@id_orden, @id_grupo, @fecha_inicio, @fecha_fin, @estado)

	INSERT INTO MT_OrdenTrabajo_Integrante(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)values(@id_orden,@id_grupo,@fecha_inicio,@fecha_fin,'A')
	update MT_OrdenTrabajo set Fecha_asignacion_grupo_trabajo = @fecha_inicio where Id_OrdenTrabajo = @id_orden

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarOrdenTrabajoNueva]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<VQ>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarOrdenTrabajoNueva] 
	-- Add the parameters for the stored procedure here
	@Id_OrdenTrabajo int,
	@Id_contrato int,
	@Id_sucursal varchar(10),
	@Id_campamento int,
	@Id_tipo_trabajo_recibido int,
	@Id_tipo_trabajo_ejecutado int,
	@Estado int,
	@Id_sector int,
	@Id_orden_padre int,
	@Id_estacion int,
	@Id_usuario varchar(10),
	@Id_entrega_orden_trabajo int,
	@Nivel_prioridad int,
	@Fecha_creacion_cliente datetime,
	@Fecha_maxima_ejecucion_cliente datetime,
	@Fecha_asignacion_grupo_trabajo datetime,
	@Fecha_asignacion datetime,
	@Fecha_finalizacion_obra datetime,
	@Fecha_ini_permiso_municipal datetime,
	@Fecha_fin_permiso_municipal datetime,
	@Fecha_entrega datetime,
	@Fecha_max_legalizacion datetime,
	@Hora_acordada time,
	@Id_barrio int,
	@Direccion varchar(max),
	@Referencia_direccion varchar(max),
	@Identificacion_suscriptor varchar(max),
	@Nombre_suscriptor varchar(max),
	@Tipo_suscriptor varchar(max),
	@Telefono_suscriptor varchar(max),
	@Origen varchar(max),
	@Comentario varchar(max),
	@Porcentaje_avance varchar(max),
	@Tiempo_transcurrido time,
	@Id_planilla int,
	@Estado_orden_planilla varchar(max),
	@Codigo_Cliente varchar(max),
	@Interna varchar(1),
	@excel_orden bit,
	@fecha_maxima_contratista datetime,
	@desalojo bit,
	@automatizacion bit,
	@Fecha_inicio_ejecucion datetime,
	@Fecha_fin_ejecucion datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	UPDATE Mt_OrdenTrabajo  SET EstadoEditOrden = 'I'
				WHERE Id_OrdenTrabajo = @Id_OrdenTrabajo

   UPDATE MT_OrdenTrabajo_Actividad  SET EstadoAct = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo

  UPDATE MT_OrdenTrabajo_Egreso  SET EstadoAct = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo

   UPDATE MT_OrdenTrabajo_Medida  SET EstadoAct = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo

   UPDATE Mt_OrdenTrabajo_Equipo  SET Estado = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo

   UPDATE MT_OrdenTrabajo_Integrante  SET Estado = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo

   UPDATE MT_OrdenTrabajo_Estado  SET EstadoO = 'I'
				WHERE Id_Orden_Trabajo = @Id_OrdenTrabajo
	

	declare @fechaRegistro datetime,
	@nuevaOrden int
	set @fechaRegistro = convert(datetime,getdate(),111);
    -- Insert statements for procedure here
	--INSERT INTO MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)
	--values(@id_orden, @id_grupo, @fecha_inicio, @fecha_fin, @estado)

	INSERT INTO Mt_OrdenTrabajo(Id_contrato, Id_sucursal, Id_campamento, Id_tipo_trabajo_recibido, Id_tipo_trabajo_ejecutado, Estado, Id_sector, Id_orden_padre, Id_estacion, Id_usuario, Id_entrega_orden_trabajo, Nivel_prioridad, 
	Fecha_registro, Fecha_creacion_cliente, Fecha_maxima_ejecucion_cliente, Fecha_asignacion_grupo_trabajo, Fecha_asignacion, Fecha_finalizacion_obra, Fecha_ini_permiso_municipal, Fecha_fin_permiso_municipal, Fecha_entrega, 
	Fecha_max_legalizacion, Hora_acordada, Id_barrio, Direccion, Referencia_direccion, Identificacion_suscriptor, Nombre_suscriptor, Tipo_suscriptor, Telefono_suscriptor, Origen, Comentario, Porcentaje_avance,
	 Tiempo_transcurrido, Id_Planilla, Estado_orden_planilla, Codigo_Cliente, Interna, EstadoEditOrden, Excel_orden, Fecha_maxima_contratista, desalojo, Automatizacion, Fecha_inicio_ejecucion, Fecha_fin_ejecucion)
	 VALUES (@Id_contrato,@Id_sucursal,@Id_campamento,@Id_tipo_trabajo_recibido,@Id_tipo_trabajo_ejecutado,@Estado,@Id_sector,@Id_orden_padre,@Id_estacion,@Id_usuario, @Id_entrega_orden_trabajo, @Nivel_prioridad,@fechaRegistro,
	 @Fecha_creacion_cliente,@Fecha_maxima_ejecucion_cliente,@Fecha_asignacion_grupo_trabajo, @Fecha_asignacion, @Fecha_finalizacion_obra,@Fecha_ini_permiso_municipal,@Fecha_fin_permiso_municipal,@Fecha_entrega,@Fecha_max_legalizacion,@Hora_acordada,@Id_barrio,
	@Direccion,@Referencia_direccion,@Identificacion_suscriptor,@Nombre_suscriptor,@Tipo_suscriptor,@Telefono_suscriptor,@Origen,@Comentario,@Porcentaje_avance,@Tiempo_transcurrido,@Id_planilla,@Estado_orden_planilla,@Codigo_Cliente,@Interna, 'A', @excel_orden, @fecha_maxima_contratista, @desalojo, @automatizacion, @Fecha_inicio_ejecucion, @Fecha_fin_ejecucion)
	
	
	set @nuevaOrden = (select Id_OrdenTrabajo from Mt_OrdenTrabajo where Codigo_Cliente = @Codigo_Cliente and EstadoEditOrden = 'A' and Id_contrato = @Id_contrato)

	--INSERT INTO MT_OrdenTrabajo_Actividad(Id_Orden_Trabajo, Id_Actividad, EstadoAct)( select mot.Id_Orden_Trabajo, mot.Id_Actividad, mot.EstadoAct 
	--from MT_OrdenTrabajo_Actividad mot where mot.Id_Orden_Trabajo = @nuevaOrden)
	
	--INSERT INTO MT_OrdenTrabajo_Egreso(Id_Orden_Trabajo, Id_Item, EstadoAct)( select mot.Id_Orden_Trabajo, mot.Id_Item, mot.EstadoAct 
	--from MT_OrdenTrabajo_Egreso mot where mot.Id_Orden_Trabajo = @nuevaOrden)


	--INSERT INTO Mt_OrdenTrabajo_Equipo(Id_GrupoTrabajo, Id_Equipo, Fecha_Inicio, Fecha_Fin)(select Id_GrupoTrabajo, Id_Equipo, Fecha_Inicio, Fecha_Fin from Mt_OrdenTrabajo_Equipo where Id_Orden_Trabajo = @Id_OrdenTrabajo)
	
	EXEC sp_Quimipac_ConsultaInsMT_OrdenTrabajoEquipoNuev @Id_OrdenTrabajo, @nuevaOrden
	EXEC sp_Quimipac_ConsultaInsMT_OrdenTrabajoIntegranteNuev @Id_OrdenTrabajo, @nuevaOrden
	INSERT INTO MT_OrdenTrabajo_Estado(Id_orden_Trabajo, Estado_Orden_Trabajo, Fecha_Ini, Fecha_Fin, EstadoO) values(@nuevaOrden, 30 , @fechaRegistro, null, 'A')


	

	
	
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsertarParametroContratoNuevo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<VQ>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsertarParametroContratoNuevo]
	-- Add the parameters for the stored procedure here
	@Id_Contrato_Parametro int,
	@Id_contrato int,
	@Parametro varchar(60),
	@Descripcion varchar(max),
	@Tipo_Dato varchar(max),
	@Valor_Inicial int,
	@Estado varchar(1),
	@Valor_Final int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	UPDATE MT_Contrato_Parametro  SET Estado = 'I'
				WHERE Id_Contrato_Parametro = @Id_Contrato_Parametro

   
	INSERT INTO MT_Contrato_Parametro(Id_contrato, Parametro, Descripcion, Tipo_Dato, Valor_Inicial, Estado, Valor_Final)
	 VALUES (@Id_contrato, @Parametro, @Descripcion, @Tipo_Dato, @Valor_Inicial, 'A', @Valor_Final)
	
	
		
END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsEstadoOrdenTrabajo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--============================================
--	 Author:		<VQ>
--	 ALTER date: <07-01-2019>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsEstadoOrdenTrabajo]

@Criterio varchar(15),
--@proyecto int,
@Id_Orden int,
@Estado int

--@id int
	
AS
BEGIN
	
	if(@Criterio = 'INS')
	begin
		INSERT INTO MT_OrdenTrabajo_Estado(Id_orden_Trabajo,Fecha_Ini, Estado_Orden_Trabajo, EstadoO)
		VALUES(@Id_Orden,getdate(), @Estado, 'A')
	
	end
	
END




GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_InsProyectoPresupuesto]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--============================================
--	 Author:		<VQ>
--	 ALTER date: <22-11-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_InsProyectoPresupuesto]

@Criterio varchar(15),
--@proyecto int,
@Id_Empresa varchar(2),
@Id_Sucursal varchar(15),
@Id_Cliente varchar(30), 
@Usuario varchar(15),
@Comentario varchar(max),
@porcentaje decimal,
@monto decimal,
@iva decimal,
@validez varchar(50),
@Moneda varchar(30)
--@id int
	
AS
BEGIN
	
	if(@Criterio = 'INS')
	begin
		INSERT INTO MT_Presupuesto(Id_Empresa,Id_Cliente,Id_Sucursal, Fecha, Id_Usuario, Comentario,Porcentaje_indirectos, Monto_Certificacion, Iva_Certificacion, Validez, Moneda)
		VALUES(@Id_Empresa,@Id_Cliente, @Id_Sucursal, getDate(), @Usuario,@Comentario,@porcentaje,@monto,@iva,@validez,@Moneda)
	select @@IDENTITY as IdActual
	end
	--else
	--begin

	--update MT_Proyecto set Id_Presupuesto = @id where Id_Proyecto = @proyecto 
	--end
  


END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Link_Consulta_PlaCompras_CabDetalle_xPK]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_Link_Consulta_PlaCompras_CabDetalle_xPK]

 @semana Varchar(10)
,@dia     int   
,@codpro   Varchar(10)
,@usuario   Varchar(10)
,@secuencia  int
,@cia_codigo  Varchar(2)
AS
BEGIN    

Select comp.*, compDeta.items,compDeta.cantidad,compDeta.cant_des,compDeta.estado,compDeta.comentario,compDeta.comentario_com,compDeta.unidad
From [SADATABASE]..[DBA].[pla_compras] as comp
Inner Join [SADATABASE]..[DBA].[pla_compras_det] as compDeta
														 on compDeta.semana =comp.semana And
														    compDeta.dia =comp.dia And
															compDeta.codpro =comp.codpro And
															compDeta.usuario =comp.usuario And
															compDeta.secuencia =comp.secuencia 
															--compDeta.dia =comp.dia And


Where compDeta.semana = @semana And  compDeta.dia =@dia And compDeta.codpro = @codpro And compDeta.usuario = @usuario And compDeta.secuencia = @secuencia and comp.cia_codigo = @cia_codigo
Order By items Asc

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Link_Consulta_PlaCompras_xPK]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_Quimipac_Link_Consulta_PlaCompras_xPK]
 @semana Varchar(10)
,@dia     int   
,@codpro   Varchar(10)
,@usuario   Varchar(10)
,@secuencia  int
,@cia_codigo  Varchar(2)
AS
BEGIN
	Select * FROM [SADATABASE]..[DBA].[pla_compras] as placompras
	Where semana = @semana And  dia =@dia And codpro = @codpro And usuario = @usuario And secuencia = @secuencia  And cia_codigo = @cia_codigo
	Order by hora desc
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Link_EditarSolicitud_PlaCompras]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_Link_EditarSolicitud_PlaCompras]
	@semana varchar(7),
	@dia int, 
	@codpro varchar(15), 
	@usuario varchar(15),
	@secuencia int,
	@cia_codigo varchar(2),
	@estado int, 
	@hora Datetime, 
	@observacion varchar(max), 
	@fecha_pro_comp Datetime, 
	@observ_compras varchar(max), 
	@codcen varchar(15), 
	@dias_prov int, 
	@fecha_disp Datetime, 
	@observ_disp varchar(max), 
	@observ_pago varchar(max),  
	@usuario_compra varchar(10), 
	@cod_suc varchar(15),  
	@categoria varchar(15), 
	@elemento varchar(15), 
	@num_ped_cotiz varchar(50), 
	@usuario_aprobador varchar(50)
AS
BEGIN

	Update [SADATABASE]..[DBA].[pla_compras] Set	estado = @estado, 
															hora = @hora, 
															observacion = @observacion, 
															fecha_pro_comp = @fecha_pro_comp, 
															observ_compras = @observ_compras, 
															codcen = @codcen, 
															dias_prov = @dias_prov, 
															fecha_disp = @fecha_disp, 
															observ_disp = @observ_disp, 
															observ_pago = @observ_pago,  
															usuario_compra = @usuario_compra, 
															cod_suc  = @cod_suc,  
															categoria = @categoria, 
															elemento = @elemento, 
															num_ped_cotiz = @num_ped_cotiz, 
															usuario_aprobador = @usuario_aprobador
	Where semana = @semana And dia = @dia And codpro = @codpro And usuario = @usuario And secuencia = @secuencia And cia_codigo = @cia_codigo
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Link_InsertarSolicitud]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_Link_InsertarSolicitud] 
	-- Add the parameters for the stored procedure here
	
	@dia int, 
	@codpro varchar(15), 
	@estado int, 
	@hora Datetime, 
	@observacion varchar(max), 
	@usuario varchar(10), 
	@fecha_pro_comp Datetime, 
	@observ_compras varchar(max), 
	@codcen varchar(15), 
	@dias_prov int, 
	@fecha_disp Datetime, 
	@observ_disp varchar(max), 
	@observ_pago varchar(max),  
	@usuario_compra varchar(10), 
	@cod_suc varchar(15),  
	@categoria varchar(15), 
	@elemento varchar(15), 
	@num_ped_cotiz varchar(50), 
	@usuario_aprobador varchar(50), 
	@cia_codigo varchar(2)
AS
BEGIN


	declare @semana char(9),
		@var1 int,
		@var2 int,
		@secuencia int, 
		@fecha Datetime

	set @fecha = convert(datetime,getdate(),111);
	set @var1 = (select 
    datepart(year, @fecha) as 'Year')
    set @var2 = (select
    datepart(week, @fecha) as 'Calendar Week')
	set @semana = Concat(@var1, '-', @var2)

	set @codpro = '000'
	
	--set @secuencia = 1
	-- Obtiene Secuencia 
    Set @secuencia =(Select max(secuencia)+1 From [SADATABASE]..[DBA].[pla_compras] Where Semana = @semana And  dia =@dia And codpro =@codpro And Usuario = @usuario And cia_Codigo =@cia_codigo)
	If @secuencia = null Begin
	 Set @secuencia =1;
	End

	/*

	Select *From [SADATABASE]..[DBA].[pla_compras] 
	 if (@usuario_compra ='Ninguno') Begin
	     Set @usuario_compra = null;
     End

	 if (@usuario_aprobador ='Ninguno')Begin
         Set @usuario_aprobador = null;
     End
	 */

	INSERT INTO [SADATABASE]..[DBA].[pla_compras](semana, dia, codpro, fecha, estado, hora, observacion, usuario, fecha_pro_comp, observ_compras, codcen, dias_prov, fecha_disp, observ_disp, observ_pago, secuencia, usuario_compra, cod_suc, categoria, elemento, num_ped_cotiz, usuario_aprobador , cia_codigo)
	VALUES(@semana, @dia, @codpro, @fecha, @estado, convert(datetime,@hora), @observacion, @usuario, convert(datetime,@fecha_pro_comp), @observ_compras, @codcen, @dias_prov, convert(datetime,@fecha_disp), @observ_disp, @observ_pago, @secuencia, @usuario_compra, @cod_suc, @categoria, @elemento, @num_ped_cotiz, @usuario_aprobador, @cia_codigo)

	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_QUIMIPAC_MENU_X_ROL]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_QUIMIPAC_MENU_X_ROL]
@CRITERIO VARCHAR(100)
AS
BEGIN
	IF @CRITERIO = 'TBGENERAL'
	BEGIN
			SELECT M.MENU,M.PADRE,M.ORDEN,M.NIVEL_PROFUNDIDAD,M.ICONO
			,MR.ID_MENUROL,MR.ID_ROL
			,R.DESCRIPCION
			FROM MENU M
			INNER JOIN MENUROL MR ON MR.ID_MENU = M.ID_MENU
			INNER JOIN ROLES R ON MR.ID_ROL = R.ID_ROL
			WHERE M.ESTADO ='A' AND MR.ESTADO = 'A' AND R.ESTADO ='A'
			ORDER BY DESCRIPCION, MENU ASC
	END


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Contrato_QryXParametro]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-------------------------------
-- Filtro de busqueda OT
-- VQ 100620
-------------------------------  

CREATE  PROCEDURE [dbo].[sp_Quimipac_MT_Contrato_QryXParametro]
@FInicio	   date,
@FFin		   date,
@Criterio      varchar(50),
@XParametroInt int,
@XParametro    varchar(3),
@empresa varchar(2)
AS

BEGIN

declare @fechaIdate date,
@fechaFdate date
set @fechaIdate = convert(date, @FInicio);
set @fechaFdate = convert(date, @FFin);
--select @FInicio
--select @fechaIdate

  If @Criterio = 'Ninguno' Begin

  select C.*,   categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate	
	order by Id_Contrato desc
		
  End
  Else If @Criterio = 'Tipo' Begin
      select C.*, categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)
	And C.tipo = @XParametroInt
	order by Id_Contrato desc
			
  End
  Else If @Criterio = 'Cliente' Begin
	select C.*,   categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Id_Cliente = @XParametro
	order by Id_Contrato desc


  End
  Else If @Criterio = 'Contrato_Padre' Begin
	select C.*,   categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Contrato_Padre = @XParametroInt
	order by Id_Contrato desc

  End
  Else If @Criterio = 'Estado' Begin
		select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate
	--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)
	And C.Estado = @XParametroInt
	order by Id_Contrato desc

  End 
  Else If @Criterio = 'Unidad_Negocio' Begin
			select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Id_Linea = @XParametro
	order by Id_Contrato desc

  End
  Else If @Criterio = 'Categoria' Begin
				select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Categoria = @XParametro
	order by Id_Contrato desc

  End
  Else If @Criterio = 'Subcategoria' Begin
				select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Subcategoria = @XParametro
	order by Id_Contrato desc
	
  End

  Else If @Criterio = 'Referencia' Begin
					select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Referencia = @XParametroInt
	order by Id_Contrato desc
	
  End

  Else If @Criterio = 'Localidad' Begin
	select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Localidad = @XParametroInt
	order by Id_Contrato desc

  End

  Else If @Criterio = 'Responsable' Begin
	select C.*,  categ.nombre as nombre_Categoria, subcateg.nombre as nombre_Subcategoria,
	cpadre.Nombre as Nomb_ContratoPadre, 
	--t.Descripcion as descripcion , tipo.Descripcion as Tipo_Contrato
	(select Descripcion from MT_TablaDetalle where C.Estado = Id_TablaDetalle ) as descripcion,
    (select Descripcion from MT_TablaDetalle where C.tipo = Id_TablaDetalle ) as Tipo_Contrato,
	(select descripcion from [SADATABASE]..[DBA].[lineas] l where C.Id_Linea = codigo and cia_codigo = @empresa) as lineaContrato,
	(select nom_cli from [SADATABASE]..[DBA].[clientes] cl where C.Id_Cliente = cod_cli and cia_codigo = @empresa) as nombreCliente,
	'' as Origen

	from MT_Contrato C
	--inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = c.Id_Linea and l.cia_codigo = @empresa
	--inner join [SADATABASE]..[DBA].[clientes] cl on  cl.cod_cli = c.Id_Cliente and cl.cia_codigo = @empresa
	LEFT OUTER join [SADATABASE]..[DBA].[tb_quimi_tipo_servicios] categ on categ.cod_linea = C.Id_Linea and c.Categoria = categ.cod_servicio and categ.cia_codigo = @empresa
	left outer join [SADATABASE]..[DBA].[centro_consumo] subcateg on subcateg.quimi_linea = C.Id_Linea and c.Subcategoria = subcateg.codcen and subcateg.cia_codigo = @empresa
	left outer join MT_Contrato cpadre on c.Contrato_Padre = cpadre.Id_Contrato and cpadre.Id_Empresa = @empresa
	
	where c.Id_Empresa = @empresa and c.Estado !=1160 and C.Fecha_registro >= @fechaIdate AND C.Fecha_registro <=@fechaFdate--and C.Fecha_registro >= CONVERT(DATE, @FInicio)  AND C.Fecha_registro <= CONVERT(DATE, @FFin)	
	And C.Responsable = @XParametroInt
	order by Id_Contrato desc
	
  End

  
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Entrega_OrdenTrabajo_UPD]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_MT_Entrega_OrdenTrabajo_UPD]    --EstadoEditOrden ???
 @Id_EntregaOT Int
,@Id_OT        Int
,@Id_Cliente   Int
,@FechaEnvio   DateTime
,@FechaRecep   DateTime
,@Comentario   Varchar(max)
,@RecibidoX	   Varchar(100)
,@Actualiza_OT Varchar(2),
@empresa varchar(2)
AS
BEGIN
	Update MT_EntregaOrden_Trabajo	SET Id_Cliente = @Id_Cliente
										,Fecha_Envio = @FechaEnvio
										,Fecha_Recepcion = @FechaRecep
										,Comentario = @Comentario
										,Recibido_Por = @RecibidoX
	Where Id_Entrega_Orden_Trabajo = @Id_EntregaOT and Id_Empresa = @empresa

	If @Actualiza_OT = 'SI' Begin	
		Update MT_OrdenTrabajo Set Id_entrega_orden_trabajo = Null
		Where Id_OrdenTrabajo=@Id_OT And Id_Entrega_Orden_Trabajo = @Id_EntregaOT And EstadoEditOrden ='A'
		delete from MT_EntregaOrden_Trabajo Where Id_Cliente= @Id_Cliente And Id_Entrega_Orden_Trabajo = @Id_EntregaOT 

	End

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Grupo_OrdenTrabajo_UPD]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE PROCEDURE [dbo].[sp_Quimipac_MT_Grupo_OrdenTrabajo_UPD]    --EstadoEditOrden ???
 @Id_EntregaOT Int
,@Id_OT        Int
,@Id_Cliente   Int
,@FechaEnvio   DateTime
,@FechaRecep   DateTime
,@Actualiza_OT Varchar(2),
@empresa varchar(2),
@Id_Grupo int
AS
BEGIN
	declare @id_grupo_anterior int, @count1 int, @count2 int, @cont int = 0
	set @id_grupo_anterior = (select top 1 Id_GrupoTrabajo from MT_OrdenTrabajo_Integrante where Id_Orden_Trabajo = @Id_OT and Estado = 'A')
	if(@id_grupo_anterior = @Id_Grupo)
	Begin
	
	Update MT_OrdenTrabajo_Integrante	SET Fecha_Inicio = @FechaEnvio
										,Fecha_Fin = @FechaRecep										
	Where Id_Orden_Trabajo = @Id_OT and Id_GrupoTrabajo = @Id_Grupo
		
	Update MT_OrdenTrabajo_Equipo SET Fecha_Inicio = @FechaEnvio
										,Fecha_Fin = @FechaRecep										
	Where Id_Orden_Trabajo = @Id_OT and Id_GrupoTrabajo = @Id_Grupo
	Update MT_OrdenTrabajo SET Fecha_asignacion_grupo_trabajo = @FechaEnvio

	If @Actualiza_OT = 'SI' Begin	
		Update MT_OrdenTrabajo_Integrante	SET Estado = 'I'										
	Where Id_Orden_Trabajo = @Id_OT and Id_GrupoTrabajo = @Id_Grupo
		
	Update MT_OrdenTrabajo_Equipo SET Estado = 'I'										
	Where Id_Orden_Trabajo = @Id_OT and Id_GrupoTrabajo = @Id_Grupo
	Update MT_OrdenTrabajo SET Fecha_asignacion_grupo_trabajo = null
							End

	end

	else

	Begin 
	Update MT_OrdenTrabajo_Integrante	SET Estado = 'I'										
	Where Id_Orden_Trabajo = @Id_OT 
		
	Update MT_OrdenTrabajo_Equipo SET Estado = 'I'										
	Where Id_Orden_Trabajo = @Id_OT 

	set @count1 = (select count(*)  from MT_GrupoTrabajoIntegrante where Id_GrupoTrabajo = @Id_Grupo and Estado = 'A')
	set @count2 = (select count(*)  from MT_GrupoTrabajoEquipo where Id_GrupoTrabajo = @Id_Grupo and Estado = 'A')

		WHILE(@count1 > 0 AND @cont < @count1) 
		BEGIN
		INSERT INTO MT_OrdenTrabajo_Integrante(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)values(@Id_OT,@id_grupo,@FechaEnvio,@FechaRecep,'A')
		set @cont = @cont +1

		End
		set @cont = 0;
				WHILE(@count2 > 0 AND @cont < @count2) 
				BEGIN
				INSERT INTO MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo, Id_GrupoTrabajo, Fecha_Inicio, Fecha_Fin, Estado)values(@Id_OT,@id_grupo,@FechaEnvio,@FechaRecep,'A')
				set @cont = @cont +1

				End
		Update MT_OrdenTrabajo SET Fecha_asignacion_grupo_trabajo = @FechaEnvio
		

	End

	
	

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Notificacion_Persona_EstadoFecha_UDP]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_MT_Notificacion_Persona_EstadoFecha_UDP] 
@pv_leido varchar(10),
@Id_Notificacion int 
as
begin
declare @Id_Tabla_EstadoPersona int

declare @Id_Tabla int,@Id_TablaDetalle int
set @Id_Tabla=(select Id_Tabla from MT_Tablas where Tabla='Estado_Persona')
set @Id_TablaDetalle=(select top 1 Id_TablaDetalle from MT_TablaDetalle where Id_Tabla=@Id_Tabla and Descripcion=@pv_leido) 
					
update MT_Notificacion_Persona
set Estado=@Id_TablaDetalle
where Id_Notificacion=@Id_Notificacion
end
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Notificaciones_Ins]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[SADATABASE]..[DBA].[tb_data_seg_def_user_web]

CREATE procedure [dbo].[sp_Quimipac_MT_Notificaciones_Ins] 
@Tipo_Notificacion int,
 @Id_usuario varchar(50), 
 @Fecha DateTime,
 @Prioridad  int , 
  @Asunto varchar(50), 
  @Mensaje varchar(MAX),
   @Criterio int,
   --@Tipo int, 
   --@Correo varchar(50), 
   --@Estado int, 
   @Id_Codigo_origen int 
   as

  begin
  --declare  @Id_Notificacion_Persona int 
  declare @Id_Notificacion int

  If @Id_Codigo_origen < 1
  Begin
	set @Id_Codigo_origen =0
  End

  Insert into MT_Notificaciones(Tipo_Notificacion,Id_Codigo_origen,Id_usuario,Fecha,Prioridad,Asunto,Mensaje,Criterio)
  values (@Tipo_Notificacion,@Id_Codigo_origen , @Id_usuario,@Fecha,@Prioridad, @Asunto, @Mensaje,@Criterio)
  
 select @@IDENTITY as Id_Notificacion
 --Insert into MT_Notificacion_Persona(Id_Notificacion,Tipo,Correo,Estado)
 --Values(@Id_Notificacion,@Tipo,@Correo,@Estado)

  --set @Id_Notificacion_Persona=( select Id_Notificacion from MT_Notificaciones where Tipo_Notificacion=@Tipo_Notificacion  and Id_Codigo_origen=@Id_Codigo_origen and Id_usuario= @Id_usuario 
		--											  and Fecha=@Fecha and Prioridad=@Prioridad and Asunto=@Asunto and Mensaje=@Mensaje 
		--											  and Criterio=@Criterio)
													   
  --Insert into MT_Notificacion_Persona(Id_Notificacion,Tipo,Correo,Estado)
  --Values(@Id_Notificacion_Persona,@Tipo,@Correo,@Estado)

  end 

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_OrdenTrabajo_QryXParametro]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-------------------------------
-- Filtro de busqueda OT
-- Sandro Yagual 2018-10
-------------------------------  

CREATE  PROCEDURE [dbo].[sp_Quimipac_MT_OrdenTrabajo_QryXParametro]
@FInicio	   date,
@FFin		   date,
@Criterio      varchar(50),
@XParametroInt int,
@XParametro    varchar(3),
@empresa varchar(2)
AS

BEGIN

  If @Criterio = 'Ninguno' Begin
	Select distinct
			(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz


			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
	
			--WHERE a_hijo.Fecha_asignacion_grupo_trabajo >= @FInicio AND a_hijo.Fecha_asignacion_grupo_trabajo <=@FFin And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			WHERE a_hijo.EstadoEditOrden = 'A' and a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin  and con.Id_Empresa = @empresa
			--ORDER BY a_hijo.Fecha_asignacion_grupo_trabajo ASC
			ORDER BY a_hijo.Fecha_registro desc

		--inner join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo
		--inner join SADATABASE.[GQ-DB-SGCPM_.DBA.rh_persona as p on oi.ID_Persona = p.id_persona
  End
  Else If @Criterio = 'Sucursal' Begin
    Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz


			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_sucursal = @XParametro And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End
  Else If @Criterio = 'Campamento' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_campamento = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End
  Else If @Criterio = 'Tipos_Trabajo' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz


			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo

			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_tipo_trabajo_ejecutado = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End
  Else If @Criterio = 'Sector' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			--WHERE a_hijo.Fecha_asignacion_grupo_trabajo >= @FInicio AND a_hijo.Fecha_asignacion_grupo_trabajo <=@FFin And a_hijo.Id_sector = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			--ORDER BY a_hijo.Fecha_asignacion_grupo_trabajo ASC
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_sector = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End 
  Else If @Criterio = 'Estacion' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_estacion = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End
  Else If @Criterio = 'Contrato' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz
	
			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo

			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Id_contrato = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End
  Else If @Criterio = 'Estado' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo

			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.Estado = @XParametroInt And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc 
  End

  Else If @Criterio = 'Cliente' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo

			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And con.Id_Cliente = @XParametro And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc 
  End

  Else If @Criterio = 'Etapa' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo

			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.EstadoEditOrden = 'A' and con.Id_Empresa = @empresa
			ORDER BY a_hijo.Fecha_registro desc
  End

  Else If @Criterio = 'OrdenPadre' Begin
	Select distinct 
	(a_hijo.Fecha_asignacion) Fecha_asignacion
	 ,a_hijo.Fecha_registro
	 ,a_hijo.Id_tipo_trabajo_recibido
      ,a_hijo.Id_tipo_trabajo_ejecutado
	  ,a_hijo.Id_estacion
	  ,a_hijo.Id_contrato
      ,a_hijo.Id_sucursal
	   ,a_hijo.Estado
	   ,a_hijo.Id_OrdenTrabajo
	   ,a_hijo.Codigo_Cliente
	   , mtp_PLA.descripcion as Dtrabajo
	   , mttej.Descripcion as clasif_orden_ttrabajoej
	   , mtdet.Descripcion
	   ,mtp.Descripcion as trabajoejecutado
	   , con.Nombre as NombreContrato
	   ,a_hijo.Porcentaje_avance
	   ,a_hijo.Tiempo_transcurrido
	   , mtgrupo.Nombre
	   ,a_hijo.Fecha_inicio_ejecucion
      ,a_hijo.Fecha_fin_ejecucion
      ,a_hijo.Fecha_finalizacion_obra
	  ,a_hijo.Id_orden_padre
	  , a_hijo.EstadoEditOrden,
	   a_hijo.Automatizacion
	   ,a_hijo.Id_usuario      
      ,a_hijo.Id_campamento
       ,a_hijo.Id_sector    
 
	,con.Codigo_Interno as CodigoInternoContrato, mtp.Proceso, mtp.Alerta, mtp.Caida
	,ISNULL( a_padre.Codigo_Cliente ,'') as descripcionPadre, a_hijo.Fecha_maxima_contratista, a_hijo.Desalojo,
	CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.proceso ,a_hijo.Fecha_asignacion)),111))) THEN  
			CASE WHEN (GETDATE() > (convert (datetime,(DATEADD(DAY,mtp.Alerta ,a_hijo.Fecha_asignacion)),111))) THEN 'CAIDA'
			ELSE
				 'ALERTA'
			END
	ELSE
		 'EN PROCESO'
	END AS Estado_P_A_C,
	mtp.Control_Anexo, mtp.Control_Costo, mtp.Control_Equipo, mtp.Control_Integrante, mtp.Control_Item, mtp.Control_Medida, mtp.Control_Raiz

			FROM MT_OrdenTrabajo a_hijo
			left join MT_OrdenTrabajo a_padre on a_hijo.Id_orden_padre = a_padre.Id_OrdenTrabajo 
			inner join MT_Contrato con on a_hijo.Id_contrato = con.Id_Contrato
			--inner join MT_Contrato_Parametro cp on con.Id_Contrato = cp.Id_Contrato 
			inner join MT_TipoTrabajo mtp_PLA on a_hijo.Id_tipo_trabajo_ejecutado = mtp_PLA.Id_TipoTrabajo
			inner join MT_TipoTrabajo mtp on a_hijo.Id_tipo_trabajo_ejecutado = mtp.Id_TipoTrabajo
			inner join MT_Servicio ms on mtp.Id_Servicio = ms.Id_Servicio
			left outer join MT_OrdenTrabajo_Integrante oi on a_hijo.Id_OrdenTrabajo = oi.Id_Orden_Trabajo and oi.Estado = 'A'
			inner join MT_TablaDetalle  mtdet on a_hijo.Estado = mtdet.Id_TablaDetalle
	        left outer join MT_GrupoTrabajo mtgrupo on oi.Id_GrupoTrabajo = mtgrupo.Id_GrupoTrabajo
			left outer join MT_TablaDetalle  mttej on mtp.Clasificacion = mttej.Id_TablaDetalle
			WHERE a_hijo.Fecha_asignacion >= @FInicio AND a_hijo.Fecha_asignacion <=@FFin And a_hijo.EstadoEditOrden = 'A' 
			and con.Id_Empresa = @empresa and ms.Id_Empresa = @empresa AND a_hijo.Id_orden_padre = @XParametroInt
			ORDER BY a_hijo.Fecha_registro desc 
  End

  
END
--sp_Quimipac_ConsultaMT_OrdenTrabajo_Result
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_PrecioReferencial_INS_UPD_X]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =====================================
--	 Author:		<Sandro Yagual>
--	 ALTER date: <13-11-2018>
-- =====================================
CREATE PROCEDURE [dbo].[sp_Quimipac_MT_PrecioReferencial_INS_UPD_X]
	@Criterio varchar(15),
	--@Id_PrecioReferencial int,
	@Id_TipoTablaDetalle int,
	@Id_Proyecto_Contrato int,
	@Id_Item varchar(20),
	@Id_TipoTrabajo int,
	@Id_Usuario varchar(30),
	--@Fecha_Registro datetime,
	@Fecha_Inicio datetime,
	@Fecha_Fin datetime,
	@Moneda varchar(30),
	@Precio numeric(18, 4),
	@Costo numeric(18, 4),
	@Estado varchar(10),
	@Unidad varchar(10)
AS
BEGIN
	If @Criterio = 'INS' Begin
		INSERT INTO MT_PrecioReferencial(Id_TipoTablaDetalle,Id_Proyecto_Contrato,Id_Item,Id_TipoTrabajo,Id_Usuario,Fecha_Registro ,Fecha_Inicio,Fecha_Fin,Moneda,Precio,Costo,Estado,Unidad)
		VALUES(@Id_TipoTablaDetalle,@Id_Proyecto_Contrato,@Id_Item,@Id_TipoTrabajo,@Id_Usuario,GETDATE(),@Fecha_Inicio,@Fecha_Fin,@Moneda,@Precio,@Costo,@Estado,@Unidad)
	End
	--Else Begin
	--	;
	--End
  
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_PrecioReferencial_Items_UPD]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sandro Yagual>
-- ALTER date: <11-12-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_MT_PrecioReferencial_Items_UPD]
 @IdPrecRef Int
,@Usuario	Varchar(40)
,@Fi		Date
,@Ff		Date
,@Precio	Decimal
,@Costo		Decimal
,@Estado	Varchar(1)
,@Moneda	Varchar(6)
AS
BEGIN
	Update MT_PrecioReferencial
			SET Id_Usuario = @Usuario
				,Fecha_Inicio = @Fi
				,Fecha_Fin = @Ff
				,Precio = @Precio
				,Costo = @Costo
				,Estado = @Estado
				,Moneda = @Moneda
	Where Id_PrecioReferencial = @IdPrecRef

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_ProgramaTrabajo_FechIniFin_UPD]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================
--   Author:	  <Sandro Yagual>
--	 ALTER date: <27-12-2018>
-- =================================

CREATE PROCEDURE [dbo].[sp_Quimipac_MT_ProgramaTrabajo_FechIniFin_UPD]
  @IdProgTrabajo Int
 ,@FechaInicio   DateTime
 ,@FechaFin		 DateTime
AS
BEGIN

	Declare @Ff DateTime;
	Set @Ff =  (SELECT DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0, @FechaFin))));
	
	Update MT_ProgramaTrabajo Set Fecha_Ini = @FechaInicio, Fecha_Fin = @Ff 
	Where Id_ProgramaTrabajo = @IdProgTrabajo

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_ProgramaTrabajo_INS]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ---------------------------
   - ALTER: <Sandro Yagual> -
   - Date:   19-12-2018  upd -
   ---------------------------*/

CREATE PROCEDURE [dbo].[sp_Quimipac_MT_ProgramaTrabajo_INS]
  @Id_Contrato     int
 ,@Id_Usuario      Varchar(20)
 ,@Id_TIpo_Trabajo int
 ,@Fecha_Registro  DateTime
 ,@Fecha_Ini       DateTime
 ,@Fecha_Fin       DateTime
 ,@Descripcion	   Varchar(60)
 ,@Direccion       Varchar(150)
 ,@Ubicacion	   Varchar(25)
 ,@Estado	       Varchar(1)
 ,@Id_GrupoTrabajo int
AS
BEGIN

	Set @Fecha_Fin =  (SELECT DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0, @Fecha_Fin))));
 
	Insert Into MT_ProgramaTrabajo(Id_Contrato,Id_Usuario,Id_TIpo_Trabajo,Fecha_Registro,Fecha_Ini,Fecha_Fin,Descripcion,Direccion,Ubicacion,Estado,Id_GrupoTrabajo)
	Values (@Id_Contrato,@Id_Usuario,@Id_TIpo_Trabajo,@Fecha_Registro,@Fecha_Ini,@Fecha_Fin,@Descripcion,@Direccion,@Ubicacion,@Estado,@Id_GrupoTrabajo)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_ProgramaTrabajo_OT_INS]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Sandro Yagual>
-- ALTER date: <26-12-2018>
-- =============================================
 CREATE PROCEDURE [dbo].[sp_Quimipac_MT_ProgramaTrabajo_OT_INS]
   @Id_ProgramaTrab Int
  ,@Id_Contrato     Int
  ,@Id_TIpo_Trabajo Int
  ,@Direccion	    Varchar(max)
  ,@user_id		    Varchar(30)
  ,@Id_GrupoTrabajo Int
  ,@Fecha_Inicio	DateTime
  ,@Fecha_Fin		DateTime
  AS
BEGIN
	-- Variable para INS en OTE y OTI
	Declare 
	 @Count_In Int
	,@Count_Eq Int
	,@ContGral Int
	
	,@Id_Actual_OT int
	--Fecha 23:59:59
	,@Ff DateTime;

	INSERT INTO MT_OrdenTrabajo (Id_contrato,Id_tipo_trabajo_recibido,Id_tipo_trabajo_ejecutado,Estado,Id_usuario,Fecha_registro,Direccion,Referencia_direccion,EstadoEditOrden)
    VALUES (@Id_Contrato,@Id_TIpo_Trabajo,@Id_TIpo_Trabajo,30,@user_id,GETDATE(),@Direccion,@Direccion,'A')
	Set @Id_Actual_OT = SCOPE_IDENTITY();--	IdActual Insertado

	Update MT_ProgramaTrabajo Set Estado = 'I' Where Id_ProgramaTrabajo = @Id_ProgramaTrab

	Set @Count_In = (Select Count(Id_GrupoTrabajo) From MT_GrupoTrabajoIntegrante  Where Id_GrupoTrabajo = @Id_GrupoTrabajo And Estado ='A');
	Set @Count_Eq = (Select Count(Id_GrupoTrabajo) From MT_GrupoTrabajoEquipo Where Id_GrupoTrabajo =@Id_GrupoTrabajo And Estado = 'A');
	

	
	--Fecha en formato con hora 23:59:59
	Set @Ff =  (SELECT DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0, @Fecha_Fin))));
	
	-- Insert de Orden de Trabajo  para OTI
	Set @ContGral = 0;
		WHILE(@Count_In > 0 AND @ContGral < @Count_In) BEGIN
			Insert Into MT_OrdenTrabajo_Integrante (Id_Orden_Trabajo,Fecha_Inicio,Fecha_Fin,Estado,Id_GrupoTrabajo) 
			Values (@Id_Actual_OT,@Fecha_Inicio,@Ff,'A',@Id_GrupoTrabajo);
			Set @ContGral=@ContGral+1;
		END

	-- Insert de Orden de Trabajo  para OTE 
	Set @ContGral = 0;
		WHILE(@Count_Eq > 0 AND @ContGral < @Count_Eq) BEGIN
			Insert Into MT_OrdenTrabajo_Equipo(Id_Orden_Trabajo,Fecha_Inicio,Fecha_Fin,Estado,Id_GrupoTrabajo) Values (@Id_Actual_OT,@Fecha_Inicio,@Ff,'A',@Id_GrupoTrabajo);
			Set @ContGral=@ContGral+1;
		END
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Proyecto_INS]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sandro Yagual>
-- ALTER date: <29-10-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_MT_Proyecto_INS]
      @Id_Empresa        varchar(2),
      @Id_Sucursal       varchar(3),
      @Id_Cliente        varchar(10),
      @Id_TipoTrabajo    int,
      @Id_Presupuesto    int,
      @Fecha_Registro    datetime,
      @Fecha_Inicio      datetime,
      @Fecha_Fin         datetime,
      @Fecha_Prorroga    datetime,
      @Estado		     varchar(1),
      @Codigo_Interno    varchar(20),
      @Codigo_Cliente    varchar(20),
      @Direccion         varchar(max),
      @Ubicacion         varchar(20),
      @Comentario        varchar(max),
      @Porcentaje_avance varchar(20),
      @Valor_Inicial     numeric(19,4),
      @Valor_Ajustado    numeric(19,4),
      @Linea			 varchar(10),
      @Categoria		 varchar(50),
      @Subcategoria		 varchar(50)
      --,@Fecha_Anticipo	 datetime
AS
BEGIN
	
  Insert Into MT_Proyecto(Id_Empresa,Id_Sucursal,Id_Cliente,Id_TipoTrabajo,Id_Presupuesto,Fecha_Registro,Fecha_Inicio,Fecha_Fin,Fecha_Prorroga,Estado,Codigo_Interno,Codigo_Cliente,Direccion,Ubicacion,Comentario,Porcentaje_avance,Valor_Inicial,Valor_Ajustado,Linea,Categoria,Subcategoria)
  Values(@Id_Empresa,@Id_Sucursal,@Id_Cliente,@Id_TipoTrabajo,@Id_Presupuesto,@Fecha_Registro,@Fecha_Inicio,@Fecha_Fin,@Fecha_Prorroga,@Estado,@Codigo_Interno,@Codigo_Cliente,@Direccion,@Ubicacion,@Comentario,@Porcentaje_avance,@Valor_Inicial,@Valor_Ajustado,@Linea,@Categoria,@Subcategoria)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MT_Tablas_XTitulos]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Quimipac_MT_Tablas_XTitulos]
AS
BEGIN

select Descripcion, Codigo from MT_TablaDetalle where Id_Tabla=12
END

--execute sp_Quimipac_MT_Tablas_XTitulos 'MANTENIMIENTO LUGAR MEDICION'
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_MTGrupoTrabajo_INS]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* Create: Sandro Yagual <Date:2019-01-25*/
CREATE PROCEDURE [dbo].[sp_Quimipac_MTGrupoTrabajo_INS] 
  @Id_Campamento Int
 ,@Nombre		 Varchar(40)
 ,@Tipo			 Int
 ,@Estado		 Varchar(1)
 ,@Bodega		 Varchar(10)
 ,@Color		 Varchar(10)
AS
 BEGIN 

	Insert into MT_GrupoTrabajo (Id_Campamento,Nombre,Tipo,Estado,Bodega,color)
	Values (@Id_Campamento,@Nombre,@Tipo,@Estado,@Bodega,@Color);
 END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ObtenerTitulosInfo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Quimipac_ObtenerTitulosInfo] 
@pvIdTitulo varchar(50)
AS
BEGIN
/* declaracion de variables*/
DECLARE 
@Noti_MantTipoTrab varchar(50),
@Noti_MantPreReferen varchar(50),
@Noti_MantLuMedi varchar(50),
@Noti_MantGrupTraba varchar(50),
@Noti_ItemMantTiTrab varchar(50),
@Noti_IntegraMantGrupTrab varchar(50),
@Noti_FormActivi varchar(50),
@Noti_EquiMantgrupTrab varchar(50),
@Noti_EdiMantTipTrab varchar(50),
@Noti_EdiMantPreReferen varchar(50),
@Noti_EdiMantLuMedi varchar(50),
@Noti_EdiMantGrupTrab varchar(50),
@Noti_Edi_ItemMantTipoTrab varchar(50),
@Noti_Edi_IntegrMantGrupTrab varchar(50),
@Noti_Edi_EquiMantGrupTrab varchar(50),
@Noti_Edi_ActMantTipTrab varchar(50),
@Noti_Agg_FormActiv varchar(50),
@Noti_Agg_MantTipoTrab varchar(50),
@Noti_Agg_MantPreRefer varchar(50),
@Noti_Agg_MantLugMedi varchar(50),
@Noti_Agg_MantGrupTrab varchar(50),
@Noti_Agg_ItemMantTipTrab varchar(50),
@Noti_Agg_IntegrMantGrupTrab varchar(50),
@Noti_Agg_EquiMantGrupTrab varchar(50),
@Noti_Agg_ActiMantTipTrab varchar(50),
@Noti_Acti_MantTipTrab varchar(50)


--Tabla Temporal llenar con los titulos
CREATE TABLE #TMPTITULOS (
	 IDTmpTitulo  int,
	 TituloPagina Varchar(80),
	 Descripcion  varchar(250)
    );


	
/* set */
set @Noti_MantTipoTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_MantPreReferen='Esta pantalla sirve ahhaahahahah';
set @Noti_MantLuMedi='Esta pantalla sirve ahhaahahahah';
set @Noti_MantGrupTraba='Esta pantalla sirve ahhaahahahah';
set @Noti_ItemMantTiTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_IntegraMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_FormActivi='Esta pantalla sirve ahhaahahahah';
set @Noti_EquiMantgrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_EdiMantTipTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_EdiMantPreReferen='Esta pantalla sirve ahhaahahahah';
set @Noti_EdiMantLuMedi='Esta pantalla sirve ahhaahahahah';
set @Noti_EdiMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Edi_ItemMantTipoTrab='Esta pantalla sirve ahhaahahahah'; 
set @Noti_Edi_IntegrMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Edi_EquiMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Edi_ActMantTipTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_FormActiv='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_MantTipoTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_MantPreRefer='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_MantLugMedi='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_MantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_ItemMantTipTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_IntegrMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_EquiMantGrupTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Agg_ActiMantTipTrab='Esta pantalla sirve ahhaahahahah';
set @Noti_Acti_MantTipTrab='Esta pantalla sirve ahhaahahahah';


/* insert*/
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion)
VALUES(@pvIdTitulo,'MantTipoTrab',@Noti_MantTipoTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'MantPreReferen',@Noti_MantPreReferen);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'MantLuMedi',@Noti_MantLuMedi);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'MantGrupTraba',@Noti_MantGrupTraba);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'ItemMantTiTrab',@Noti_ItemMantTiTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'IntegraMantGrupTrab',@Noti_IntegraMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'FormActivi',@Noti_FormActivi);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'EquiMantgrupTrab',@Noti_EquiMantgrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'EdiMantTipTrab',@Noti_EdiMantTipTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'EdiMantPreReferen',@Noti_EdiMantPreReferen);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'EdiMantLuMedi',@Noti_EdiMantLuMedi);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'EdiMantGrupTrab',@Noti_EdiMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Edi_ItemMantTipoTrab',@Noti_Edi_ItemMantTipoTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Edi_IntegrMantGrupTrab',@Noti_Edi_IntegrMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Edi_EquiMantGrupTrab',@Noti_Edi_EquiMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Edi_ActMantTipTrab',@Noti_Edi_ActMantTipTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_FormActiv',@Noti_Agg_FormActiv);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_MantTipoTrab',@Noti_Agg_MantTipoTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_MantPreRefer',@Noti_Agg_MantPreRefer);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_MantLugMedi',@Noti_Agg_MantLugMedi);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_MantGrupTrab',@Noti_Agg_MantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_ItemMantTipTrab',@Noti_Agg_ItemMantTipTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_IntegrMantGrupTrab',@Noti_Agg_IntegrMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_EquiMantGrupTrab',@Noti_Agg_EquiMantGrupTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Agg_ActiMantTipTrab',@Noti_Agg_ActiMantTipTrab);
INSERT INTO #TMPTITULOS(IDTmpTitulo, TituloPagina,Descripcion) VALUES(@pvIdTitulo,'Acti_MantTipTrab',@Noti_Acti_MantTipTrab);

SELECT *FROM #TMPTITULOS


--Exec Sp_Quimipac_ObtenerTitulosInfo 1


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_OrdenEntregaOrden]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--============================================
--	 Author:		<VQ>
--	 ALTER date: <22-11-2018>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_OrdenEntregaOrden]

@Criterio varchar(15),
@orden int,
@Id_Empresa varchar(2),
@Id_Cliente varchar(15), 
--@Fecha_Elaboracion datetime,
@Fecha_Envio datetime,
@Fecha_Recepcion datetime,
@Usuario varchar(15),
@Comentario varchar(max),
@Recibido_Por varchar(max),
@id int
	
AS
BEGIN
	
	if(@Criterio = 'INS')
	begin
		INSERT INTO MT_EntregaOrden_Trabajo(Id_Empresa,Id_Cliente,Fecha_Elaboracion,Fecha_Envio,Fecha_Recepcion,Usuario,Comentario,Recibido_Por)
		VALUES(@Id_Empresa,@Id_Cliente,getDate(),@Fecha_Envio,@Fecha_Recepcion,@Usuario,@Comentario,@Recibido_Por)
	select @@IDENTITY as IdActual
	end
	else
	begin

	update MT_OrdenTrabajo set Id_entrega_orden_trabajo = @id where Id_OrdenTrabajo = @orden 
	end
  


END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_Padre_Hijo_XTTRABAJO]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[sp_Quimipac_Padre_Hijo_XTTRABAJO]

@empresa varchar(2),
@tipo_trabajo int, 
@criterio varchar(10),
@id_padre int,
@id_hijo int
as

begin

if(@criterio = 'SinPadre')
begin

  select tp.Id_TipoTrabajo tipo_trabajohijo, tp.Id_Padre padrehijo, a_padre.Id_TipoTrabajo tipotrabajopadre, a_padre.Id_Padre padrepadre , tp.*, c.nom_cli as cliente, l.descripcion as lineaTipoTrabajo, t.Descripcion as tipoTrabajo, s.Descripcion as descripcionServicio, a_padre.Descripcion as DescripcionPadre 
	from MT_TipoTrabajo tp
	left join MT_TipoTrabajo a_padre on a_padre.Id_TipoTrabajo = tp.Id_Padre
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Estado in ('A','I') --and tp.Id_TipoTrabajo IN(SELECT DISTINCT Id_Padre FROM MT_TipoTrabajo)
	and tp.Id_Padre = 0


end
else
if(@criterio = 'Hijo')
begin

 select tp.Id_TipoTrabajo tipo_trabajohijo, tp.Id_Padre padrehijo, a_padre.Id_TipoTrabajo tipotrabajopadre, a_padre.Id_Padre padrepadre , tp.*, c.nom_cli as cliente, l.descripcion as lineaTipoTrabajo, t.Descripcion as tipoTrabajo, s.Descripcion as descripcionServicio, a_padre.Descripcion as DescripcionPadre  from MT_TipoTrabajo tp
	left join MT_TipoTrabajo a_padre on a_padre.Id_TipoTrabajo = tp.Id_Padre
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Id_Padre = @id_padre and tp.Estado in ('A','I')

end

else

if(@criterio='Nieto')
begin

 select tp.Id_TipoTrabajo tipo_trabajohijo, tp.Id_Padre padrehijo, a_padre.Id_TipoTrabajo tipotrabajopadre, a_padre.Id_Padre padrepadre , tp.*, c.nom_cli as cliente, l.descripcion as lineaTipoTrabajo, t.Descripcion as tipoTrabajo, s.Descripcion as descripcionServicio, a_padre.Descripcion as DescripcionPadre  from MT_TipoTrabajo tp
	left join MT_TipoTrabajo a_padre on a_padre.Id_TipoTrabajo = tp.Id_Padre
	inner join [SADATABASE]..[DBA].[clientes] c on c.cod_cli = tp.Id_Cliente
	inner join [SADATABASE]..[DBA].[lineas] l on l.codigo = tp.Linea
	inner join MT_TablaDetalle t on t.Id_TablaDetalle = tp.Tipo
	inner join MT_Servicio s on s.Id_Servicio = tp.Id_Servicio
	where s.Id_Empresa = @empresa and c.cia_codigo = @empresa and l.cia_codigo = @empresa and tp.Estado in ('A','I')  
	 and tp.Id_Padre in (select Id_TipoTrabajo from MT_TipoTrabajo padre where Id_Padre = @id_padre)

end
end






GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_UpdateContrato]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_UpdateContrato]
	-- Add the parameters for the stored procedure here
        --//Id_Contrato, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Contrato_Padre
		--,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones
	  @Id_Contrato int,	  
	  @cod_cliente varchar(10) ,  
	  @fecha_inicial DateTime ,  
	  @fecha_fin  DateTime,  
	  @codigo_secuencial_interno  varchar(75),  
	  @codigo_contrato_asociado  varchar(75),  	   
	  @unidad  varchar(10), 
	  @cod_servicio varchar(10),  
	  @codcen  varchar(13),  
	  @detalle  varchar(max),
	  @id_estado int, 
	  @plazo_contrato  int,  
	  @cod_tipo int,
	  @cont_padre int,
	  --@valorrefer decimal,
	  --@monto decimal,  
	  --@costo decimal,
	  @valorrefer numeric(18,4),
	  @monto numeric(18,4),  
	  @costo numeric(18,4),
	  @Responsable int,
	  @secuencial int,
	  @codigo_secuencial_interno_anterior varchar(75),
	  @observaciones varchar(max),
	  @codigo_interno_padre varchar(75),
	  @fecha_registro DateTime ,  
	  @fecha_modificacion  DateTime,
	  @Localidad varchar(2),
	  @Fecha_Aprobacion_Cot  DateTime,
	  @Recepcion_Servicio varchar(max),
	  @Fecha_Firma_Conformidad  DateTime,
	  @Fecha_Cumplimiento_Inst datetime,
	  @Referencia int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

   

	UPDATE MT_Contrato SET Id_Cliente = @cod_cliente, Fecha_Inicio = @fecha_inicial, Fecha_Fin = @fecha_fin, Codigo_Interno = @codigo_secuencial_interno, 
	Codigo_Cliente = @codigo_contrato_asociado, Id_Linea = @unidad, Categoria = @cod_servicio, Subcategoria = @codcen, Nombre = @detalle, Estado = @id_estado,
	Dia_Plazo = @plazo_contrato, tipo = @cod_tipo, Contrato_Padre = @cont_padre, Valor_Referencial = @valorrefer, monto = @monto, costo = @costo, Responsable = @Responsable,
	Secuencial = @secuencial, Codigo_Interno_Ant = @codigo_secuencial_interno_anterior, Observaciones = @observaciones, Codigo_interno_padre = @codigo_interno_padre, Fecha_registro = @fecha_registro, 
	Fecha_modificacion = @fecha_modificacion, Localidad = @Localidad, Fecha_Aprobacion_Cot = @Fecha_Aprobacion_Cot, Recepcion_Servicio = @Recepcion_Servicio, Fecha_Firma_Conformidad = @Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst = @Fecha_Cumplimiento_Inst,
	Referencia = @Referencia 
	WHERE Id_Contrato = @Id_Contrato
	
	declare @fechaprorroga datetime, @fechaantprorroga datetime
		
    set @fechaantprorroga = (select Fecha_Prorroga from MT_Contrato_Prorroga WHERE Id_Contrato = @Id_contrato and Estado = 'A')
	
	if (@fechaantprorroga is null)
	begin
	set @fechaantprorroga = @fecha_inicial
	end

	UPDATE MT_Contrato_Prorroga  SET Estado = 'I'
				WHERE Id_Contrato = @Id_contrato and Fecha_Prorroga = @fechaantprorroga
   
   if(@cod_tipo = 144 and @cod_tipo = 1152)
	   begin
	   if(@plazo_contrato is null)
		   begin
		   set @plazo_contrato = 0
		   end

	   set @fechaprorroga = (SELECT DATEADD(DAY,@plazo_contrato,@fechaantprorroga));	

		INSERT INTO MT_Contrato_Prorroga(Id_contrato, Estado, Dia_Prorroga, Fecha_Prorroga, Descripcion)
		 VALUES (@Id_contrato,'A',@plazo_contrato, @fechaprorroga, 'Prorroga')

		 UPDATE MT_Contrato SET Fecha_Fin = @fechaprorroga
					WHERE Id_Contrato = @Id_contrato 
		end
	else
		begin
		INSERT INTO MT_Contrato_Prorroga(Id_contrato, Estado, Dia_Prorroga, Fecha_Prorroga, Descripcion)
		 VALUES (@Id_contrato,'A',@plazo_contrato, @fecha_fin, 'Prorroga')
		end

		if(@cod_tipo = 1152 or @cod_tipo = 1165)
		begin
		update MT_Proyecto set Id_Cliente = @cod_cliente, Fecha_Inicio = @fecha_inicial, Fecha_Fin = @fecha_fin,   Codigo_Interno = @codigo_secuencial_interno,  Codigo_Cliente = @codigo_contrato_asociado, Linea = @unidad, Categoria = @cod_servicio,
       Subcategoria = @codcen, Valor_Inicial = @valorrefer, Valor_Ajustado = @monto, Comentario = @observaciones, Fecha_Registro = @fecha_registro, Id_contrato = @Id_Contrato
	   end
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_UpdateProspecto]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_UpdateProspecto]
	-- Add the parameters for the stored procedure here
        --//Id_Prospecto, Id_Cliente,Fecha_Inicio,Fecha_Fin,Codigo_Interno, Codigo_Cliente,Id_Linea,Categoria,Subcategoria,Nombre,Estado,Dia_Plazo,tipo,Prospecto_Padre
		--,Valor_Referencial,monto,costo,Responsable, Secuencial, Codigo_Interno_Ant, Observaciones
	  @Id_Prospecto int,	  
	  @cod_cliente varchar(10) ,  
	  @fecha_inicial DateTime ,  
	  @fecha_fin  DateTime,  
	  @codigo_secuencial_interno  varchar(75),  
	  @codigo_prospecto_asociado  varchar(75),  	   
	  @unidad  varchar(10), 
	  @cod_servicio varchar(10),  
	  @codcen  varchar(13),  
	  @detalle  varchar(max),
	  @id_estado int, 
	  @plazo_prospecto  int,  
	  @cod_tipo int,
	  
	  --@valorrefer decimal,
	  --@monto decimal,  
	  --@costo decimal,
	  @valorrefer numeric(18,4),
	  @monto numeric(18,4),  
	  @costo numeric(18,4),
	  @Responsable int,
	  @secuencial int,
	  @codigo_secuencial_interno_anterior varchar(75),
	  @observaciones varchar(max),
	  @codigo_interno_padre varchar(75),
	  @fecha_registro DateTime ,  
	  @fecha_modificacion  DateTime,
	  @Localidad varchar(2),
	  @Fecha_Aprobacion_Cot  DateTime,
	  @Recepcion_Servicio varchar(max),
	  @Fecha_Firma_Conformidad  DateTime,
	  @Fecha_Cumplimiento_Inst datetime
	  
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

   

	UPDATE MT_Prospecto SET Id_Cliente = @cod_cliente, Fecha_Inicio = @fecha_inicial, Fecha_Fin = @fecha_fin, Codigo_Interno = @codigo_secuencial_interno, 
	Codigo_Cliente = @codigo_prospecto_asociado, Id_Linea = @unidad, Categoria = @cod_servicio, Subcategoria = @codcen, Nombre = @detalle, Estado = @id_estado,
	Dia_Plazo = @plazo_prospecto, tipo = @cod_tipo, /*Prospecto_Padre = @cont_padre,*/ Valor_Referencial = @valorrefer, monto = @monto, costo = @costo, Responsable = @Responsable,
	Secuencial = @secuencial, Codigo_Interno_Ant = @codigo_secuencial_interno_anterior, Observaciones = @observaciones, Codigo_interno_padre = @codigo_interno_padre, Fecha_registro = @fecha_registro, 
	Fecha_modificacion = @fecha_modificacion, Localidad = @Localidad, Fecha_Aprobacion_Cot = @Fecha_Aprobacion_Cot, Recepcion_Servicio = @Recepcion_Servicio, Fecha_Firma_Conformidad = @Fecha_Firma_Conformidad, Fecha_Cumplimiento_Inst = @Fecha_Cumplimiento_Inst
	/*Referencia = @Referencia*/ 
	WHERE Id_Prospecto = @Id_Prospecto
	
	declare @fechaprorroga datetime, @fechaantprorroga datetime
		
    --set @fechaantprorroga = (select Fecha_Prorroga from MT_Prospecto_Prorroga WHERE Id_Prospecto = @Id_prospecto and Estado = 'A')
	
	if (@fechaantprorroga is null)
	begin
	set @fechaantprorroga = @fecha_inicial
	end

	--UPDATE MT_Prospecto_Prorroga  SET Estado = 'I'
	--			WHERE Id_Prospecto = @Id_prospecto and Fecha_Prorroga = @fechaantprorroga
   
   if(@cod_tipo = 144 and @cod_tipo = 1152)
	   begin
	   if(@plazo_prospecto is null)
		   begin
		   set @plazo_prospecto = 0
		   end

	   set @fechaprorroga = (SELECT DATEADD(DAY,@plazo_prospecto,@fechaantprorroga));	

		--INSERT INTO MT_Prospecto_Prorroga(Id_prospecto, Estado, Dia_Prorroga, Fecha_Prorroga, Descripcion)
		-- VALUES (@Id_prospecto,'A',@plazo_prospecto, @fechaprorroga, 'Prorroga')

		 UPDATE MT_Prospecto SET Fecha_Fin = @fechaprorroga
					WHERE Id_Prospecto = @Id_prospecto 
		end
	--else
		----begin
		----INSERT INTO MT_Prospecto_Prorroga(Id_prospecto, Estado, Dia_Prorroga, Fecha_Prorroga, Descripcion)
		---- VALUES (@Id_prospecto,'A',@plazo_prospecto, @fecha_fin, 'Prorroga')
		----end

		--if(@cod_tipo = 1152 or @cod_tipo = 1165)
		--begin
		--update MT_Proyecto set Id_Cliente = @cod_cliente, Fecha_Inicio = @fecha_inicial, Fecha_Fin = @fecha_fin,   Codigo_Interno = @codigo_secuencial_interno,  Codigo_Cliente = @codigo_prospecto_asociado, Linea = @unidad, Categoria = @cod_servicio,
  --     Subcategoria = @codcen, Valor_Inicial = @valorrefer, Valor_Ajustado = @monto, Comentario = @observaciones, Fecha_Registro = @fecha_registro, Id_prospecto = @Id_Prospecto
	 --  end
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ValidarActividadesTipoTrabEjec_OrdeTrabajo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Vanessa Quintana>
-- ALTER date: <31/02018>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ValidarActividadesTipoTrabEjec_OrdeTrabajo]
	-- Add the parameters for the stored procedure here

	@Id_OrdenTrabajo int, 
	@id_tipotrabajo int
	
AS
BEGIN

    -- Insert statements for procedure here
	--select * from MT_OrdenTrabajo 
	--where Id_OrdenTrabajo = @Id_OrdenTrabajo and Id_tipo_trabajo_ejecutado = @id_tipotrabajo
	
	select * from MT_OrdenTrabajo mo 
		inner join MT_OrdenTrabajo_Actividad moa on mo.Id_OrdenTrabajo = moa.Id_Orden_Trabajo 
		where moa.Id_Orden_Trabajo = @Id_OrdenTrabajo and mo.Id_tipo_trabajo_ejecutado = @id_tipotrabajo

	



END



GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ValidarEquipoOdenTrabajo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ValidarEquipoOdenTrabajo]
	-- Add the parameters for the stored procedure here

	@id_orden int,
	@id_grupo int,
	@fecha_inicio datetime,
	@fecha_fin datetime
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	

    -- Insert statements for procedure here
	select * from MT_OrdenTrabajo_Equipo 
	where Id_Orden_Trabajo = @id_orden and Id_GrupoTrabajo = @id_grupo and Fecha_Inicio = @fecha_inicio and Fecha_Fin = @fecha_fin
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ValidarTipoTrabEjecutado_OrdeTrabajo]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ValidarTipoTrabEjecutado_OrdeTrabajo]
	-- Add the parameters for the stored procedure here

	@Id_OrdenTrabajo int, 
	@id_tipotrabajo int
	
AS
BEGIN

    -- Insert statements for procedure here
	select * from MT_OrdenTrabajo 
	where Id_OrdenTrabajo = @Id_OrdenTrabajo and Id_tipo_trabajo_ejecutado = @id_tipotrabajo 

	



END


GO
/****** Object:  StoredProcedure [dbo].[sp_Quimipac_ValidarTipoTraboProyecto]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Quimipac_ValidarTipoTraboProyecto]
	-- Add the parameters for the stored procedure here

	@Id_Proyecto int, 
	@id_tipotrabajo int,
	@empresa varchar(2)
	
AS
BEGIN

    -- Insert statements for procedure here
	select * from MT_Proyecto 
	where Id_Proyecto = @Id_Proyecto and Id_TipoTrabajo = @id_tipotrabajo and Id_Empresa = @empresa

	



END



GO
/****** Object:  StoredProcedure [dbo].[sp_service_mail_notificacion]    Script Date: 04/10/2020 22:26:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_service_mail_notificacion]
--@EMPRESAID VARCHAR(2)
AS
BEGIN
	SELECT NP.CORREO,N.* FROM MT_NOTIFICACION_PERSONA NP
	INNER JOIN MT_NOTIFICACIONES N ON NP.ID_NOTIFICACION = N.ID_NOTIFICACION
	WHERE -- N.EMPRESAID = @EMPRESAID AND
	 N.FRECUENCIA != 0 AND N.REENVIO = 1 AND N.ENVIADO = 1 
	ORDER BY  N.PRIORIDAD, N.FECHA DESC
END



--update MT_Notificaciones set Fecha_Envio = '2020-05-06 18:55:37.523' where Id_Notificacion = 2038 
GO
