using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisRptBanco.Data
{
    public class Herramientas
    {
        public static DateTime FechaDelServidor()
        {
            using (var ctx = new LaPePeEntities())
            {
                var dQuery = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ");
                return dQuery.AsEnumerable().First();
            }
        }
    }
}
