using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Quimipac_.Models
{
    public partial class Login
    {
        [Required]
        public string User_id { get; set; }
        [Required]
        public string User_clave { get; set; }
        [Required]
        public string Id_empresa { get; set; }
        public string Id_moneda { get; set; }
    }
}