﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class OrdenGrupo_IntegranteEquipoCliente
    {
        public int Id_Orden_Trabajo { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Estado { get; set; }
        public int Id_GrupoTrabajo { get; set; }
        public string Id_Cliente { get; set; }
    }
}