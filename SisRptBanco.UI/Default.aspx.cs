using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisRptBanco.Data;
using DevExpress.Web;
using System.Drawing;

namespace SisRptBanco.UI
{
    public partial class Default : System.Web.UI.Page
    {
        //lapepe1
        //LaPePeEntities ctx = new LaPePeEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                deFechaInicio.Date = Herramientas.FechaDelServidor();
                deFechaFin.Date = Herramientas.FechaDelServidor();
                edsMovPorCuenta.WhereParameters.Add(new Parameter("fechaInicio", System.Data.DbType.DateTime));
                edsMovPorCuenta.WhereParameters.Add(new Parameter("fechaFin", System.Data.DbType.DateTime));

                gvMovPorCuenta.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                gvMovPorCuenta.Styles.AlternatingRow.BackColor = Color.LightGray;
                //gvMovPorCuenta.Styles.Row.BackColor = Color.Red;
                cbTemas.DataSource = DevExpress.Web.ASPxThemes.ThemesProviderEx.GetThemes();
                cbTemas.DataBind();
                var cTheme = Request.Cookies["theme"];
                var theme = (cTheme == null) ? "Office2003Olive" : cTheme.Value;
                cbTemas.Value = theme;
            }
        }

        protected void edsMovPorCuenta_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        {
            if (idCuentaBancariaActual() == 0)
                e.Cancel = true;
            else
            {
                var idCtaxBco = idCuentaBancariaActual();
                e.DataSource.WhereParameters["fechaInicio"].DefaultValue = deFechaInicio.Date.Date.ToString();
                e.DataSource.WhereParameters["fechaFin"].DefaultValue = deFechaFin.Date.Date.AddDays(1).AddMilliseconds(-1).ToString();
                e.DataSource.Where = "it.IdCtaxBco = " + idCtaxBco
                    + " AND it.dFecOpe >= @fechaInicio AND it.dFecOpe <= @fechaFin AND it.cEstado = 'A'";
                e.DataSource.OrderBy = "it.dFecOpe Desc, it.IdMovCtaxBco Desc";
            }
        }

        protected void edsCuentasBancarias_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        {
            e.DataSource.Where = "it.cEstado = 'A'";
            edsCuentasBancarias.Include = "TTipIns";
        }

        private int idCuentaBancariaActual()
        {
            int idCuenta = 0;
            if (lblCuentaActual.ToolTip != "")
                idCuenta = Convert.ToInt32(lblCuentaActual.ToolTip);

            return idCuenta;
        }

        protected void gvMovPorCuenta_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int idCuenta = idCuentaBancariaActual();
            e.Enabled = (e.ButtonType == ColumnCommandButtonType.New && idCuenta == 0) ? false : true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblCuentaActual.ToolTip = Convert.ToString(gluCuentasBancarias.Value);
            lblCuentaActual.Text = gluCuentasBancarias.Text; //" -- Entre Fechas: " + deFechaFin.Date.ToShortDateString() + " - " + deFechaFin.Date.ToShortDateString();
            gvMovPorCuenta.DataBind();
        }

        protected void edsMovPorCuenta_Inserting(object sender, EntityDataSourceChangingEventArgs e)
        {
            e.Cancel = true;
            int idCuenta = idCuentaBancariaActual();
            var entidad = (DMovCtaxBco)e.Entity;
            entidad.IdCtaxBco = idCuenta;
            entidad.dFecIng = Herramientas.FechaDelServidor();
            entidad.cEstado = "A";
            e.Context.SaveChanges();

            var ultiMovi = SrvLista.UltimoMovimientoPorFecha(idCuenta, entidad.IdMovCtaxBco, entidad.dFecOpe);
            decimal? saldoInicial = (ultiMovi == null) ? 0 : ultiMovi.nSalFin; //si no hay ultimovi entonces empezara de 0
            SrvLista.ActualizarUltimosSaldos(entidad.IdCtaxBco, entidad.IdMovCtaxBco, entidad.dFecOpe, saldoInicial);
        }

        protected void edsMovPorCuenta_Updating(object sender, EntityDataSourceChangingEventArgs e)
        {
            e.Cancel = true;
            var entidad = (DMovCtaxBco)e.Entity;
            var entidadBD = SrvLista.DMovCtaxBco(entidad.IdMovCtaxBco);
            e.Context.SaveChanges();

            if (entidad.nImporte != entidadBD.nImporte || entidad.dFecOpe != entidadBD.dFecOpe)
            {//-------------------si cambia la fecha, utilizo la fecha menor (mas cerca al saldo inicial)
                //al cambiar la fecha, debo actualizar todos los saldos afectados a partir de la fecha menor
                var compararFechas = DateTime.Compare(entidadBD.dFecOpe.Value, entidad.dFecOpe.Value);
                var fechaOpeMenor = (compararFechas < 0) ? entidadBD.dFecOpe : entidad.dFecOpe; //0 > date1 es menor, 0 = iguales, 0 < date2 es menor
                //PD la fechaIng siempre sera desde la BD porque el entitydatasource me muestra null
                
                
                var ultiMovi = SrvLista.UltimoMovimientoPorFecha(entidadBD.IdCtaxBco, entidadBD.IdMovCtaxBco, fechaOpeMenor);
                decimal? saldoInicial = (ultiMovi == null) ? 0 : ultiMovi.nSalFin; //si no hay ultimovi entonces empezara de 0
                SrvLista.ActualizarUltimosSaldos(entidadBD.IdCtaxBco, entidadBD.IdMovCtaxBco, fechaOpeMenor, saldoInicial);
            }
        }

        protected void edsMovPorCuenta_Deleting(object sender, EntityDataSourceChangingEventArgs e)
        {
            e.Cancel = true;
            var entidad = (DMovCtaxBco)e.Entity;
            var entidadBD = SrvLista.DMovCtaxBco(entidad.IdMovCtaxBco);
            entidad.cEstado = "E";
            e.Context.SaveChanges();

            var ultiMovi = SrvLista.UltimoMovimientoPorFecha(entidadBD.IdCtaxBco, entidadBD.IdMovCtaxBco, entidad.dFecOpe);
            decimal? saldoInicial = (ultiMovi == null) ? 0 : ultiMovi.nSalFin; //si no hay ultimovi entonces empezara de 0
            SrvLista.ActualizarUltimosSaldos(entidadBD.IdCtaxBco, entidadBD.IdMovCtaxBco, entidad.dFecOpe, saldoInicial);
        }

        protected void gvMovPorCuenta_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["dFecOpe"] = Herramientas.FechaDelServidor();
        }
    }
}