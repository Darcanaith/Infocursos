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
        EConector conector;
        string condicionante;

        public string Conector { get => ((EConector)conector).AsString(EnumFormat.Description); }

        public Filtro(string atributoFiltrado, string condicionante, ECondicionNum condicionNum, EConector conector)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionNum = condicionNum;
            this.condicionante = condicionante;
            this.conector = conector;
            condicionText = null;
        }
        public Filtro(string atributoFiltrado, string condicionante, ECondicionText condicionText, EConector conector)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionText = condicionText;
            this.condicionante = condicionante;
            this.conector = conector;
            condicionNum = null;
        }
        public Filtro(string atributoFiltrado, string condicionante, ECondicionNum condicionNum)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionNum = condicionNum;
            this.condicionante = condicionante;
            this.conector = EConector.AND;
            condicionText = null;
        }
        public Filtro(string atributoFiltrado, string condicionante, ECondicionText condicionText)
        {
            this.atributoFiltrado = atributoFiltrado;
            this.condicionText = condicionText;
            this.condicionante = condicionante;
            this.conector = EConector.AND;
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
                else if (condicionText == ECondicionText.Igual)
                    condicion = "LIKE  '" + condicionante + "'";
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
