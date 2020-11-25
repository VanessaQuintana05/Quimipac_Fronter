using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quimipac_.Models
{
    public class clsUtil
    {

        public int ValidaMayor(int lkProceso, int lkAlerta, int lkCaida)
        {
            if (lkProceso > lkAlerta)
            {
                if (lkProceso > lkCaida)
                {
                    return lkProceso;
                }
                else
                { //lkCaidaMenor}
                    return lkCaida;
                }
            }
            else
            {
                if (lkAlerta > lkCaida)
                {//LkAlerta Mayor
                    return lkAlerta;
                }
                else
                {
                    return lkCaida;//lkCaidaMayor}
                }
            }
        }

        
        public DateTime FechaActual_SQL { get; set; }

        public string VariableString { get; set; }
        public int VariableInt { get; set; }
        public DateTime? VariableDateTime { get; set; }
    }
}