using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Infocursos
{
    public class CNX
    {
        public SqlConnection Connection { get; set; } = null;

        public CNX()
        {
            Connection = new SqlConnection("Data Source=217.71.207.123,54321;Initial Catalog=INFOCURSOS_DB;" +
                "Min Pool Size=0;Max Pool Size=10024;Persist Security Info=True;User ID=sa;Password=123456789");
            Connection.Open();
        }
    }
}