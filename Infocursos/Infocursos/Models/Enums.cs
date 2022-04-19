using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infocursos.Models
{
    public static class Enums
    {
        public enum ECondicionNum  
        {
            [Display(Name = "Mas pequeño que")]
            [Description("<")]
            Lt,
            [Display(Name = "Igual que")]
            [Description("=")]
            Ig,
            [Display(Name = "Mas grande que")]
            [Description(">")]
            Gt
        }
        public enum ECondicionText
        {
            [Display(Name = "Empieza por")]
            [Description("%")]
            Ini,
            [Display(Name = "Contiene")]
            [Description("%")]
            Cont,
            [Display(Name = "Acaba por")]
            [Description("%")]
            Fin,
            [Display(Name = "Igual a")]
            [Description("")]
            Igual
        }


        public static string GetEnumDisplayName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Name;
            else
                return value.ToString();
        }

    }
}
