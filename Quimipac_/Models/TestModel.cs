using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quimipac_.Models
{
    public class TestModel
    {
        private BD_QUIMIPACEntities db = new BD_QUIMIPACEntities();
        public TestModel()
        {
            Triggers = GetTriggers();
        }

        public List<string> SelectedTriggers { get; set; }
        public IEnumerable<SelectListItem> Triggers { get; set; }


        public List<SelectListItem> GetTriggers()
        {
            //List<SelectListItem> aux = new List<SelectListItem>();

            //aux.Add(new SelectListItem { Text = "Uno", Value = "1" });
            //aux.Add(new SelectListItem { Text = "Dos", Value = "2" });
            //aux.Add(new SelectListItem { Text = "Tres", Value = "3" });
            //aux.Add(new SelectListItem { Text = "Cuatro", Value = "4" });
            //aux.Add(new SelectListItem { Text = "Cinco", Value = "5" });
            var empresa_id = System.Web.HttpContext.Current.Session["empresa"];
            List<SelectListItem> itemssector = new List<SelectListItem>();
            var listaOrden = db.sp_Quimipac_ConsultaMT_OrdenTrabajoEntregaCliente("",empresa_id.ToString()).ToList();
            foreach (var ordenCliente in listaOrden)
            {
                itemssector.Add(new SelectListItem { Value = Convert.ToString(ordenCliente.Id_OrdenTrabajo), Text = ordenCliente.Codigo_Cliente });
            }
            //SelectList selectlistaSector = new SelectList(itemssector, "Value", "Text");

            return itemssector;

        }
    }
}