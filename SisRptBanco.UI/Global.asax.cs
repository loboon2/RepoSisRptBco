using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SisRptBanco.UI
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var cTheme = Request.Cookies["theme"];
            DevExpress.Web.ASPxWebControl.GlobalTheme = (cTheme == null) ? "Glass" : cTheme.Value;
        }
    }
}