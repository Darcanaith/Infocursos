using EnumsNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infocursos.Models.Enums;

namespace Infocursos.Models
{
    
    public class Filtro
    {
        string atributoFiltrado;
        ECondicionText? condicionText;
        ECondicionNum? condicionNum;
        string condicionante;

        public Filtro(string atributoFiltrado, string condicionante, ECondicionNum condicionNum)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionNum = condicionNum;
            this.condicionante = condicionante;
            condicionText = null;
        }
        public Filtro(string atributoFiltrado, string condicionante, ECondicionText condicionText)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionText = condicionText;
            this.condicionante = condicionante;
            condicionNum = null;
        }


        override
            public string ToString()
        {
            string condicion = null;
            string simbolo;

            if (condicionText != null)
            {
                simbolo = ((ECondicionText)condicionText).AsString(EnumFormat.Description);
                if (condicionText == ECondicionText.Ini)
                    condicion = "LIKE '" + condicionante + simbolo + "'";
                else if (condicionText == ECondicionText.Cont)
                    condicion = "LIKE '"+simbolo + condicionante + simbolo + "'";
                else if (condicionText == ECondicionText.Fin)
                    condicion = "LIKE  '" + simbolo + condicionante + "'";
            }

            else if ((condicionNum != null)) 
            {
                simbolo = ((ECondicionNum)condicionNum).AsString(EnumFormat.Description);
                condicion = simbolo + " " + condicionante;
            }

            return atributoFiltrado+" "+condicion;
        }
    }
}
