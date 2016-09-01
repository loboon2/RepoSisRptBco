using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisRptBanco.Data
{
    public class SrvLista
    {
        //public static IQueryable<MCtaxBco> Select_CuentasBancarias(LaPePeEntities ctx)
        //{
        //    var query = ctx.MCtaxBco.AsQueryable();
        //    return query;
        //}

        //public static IQueryable<DMovCtaxBco> Select_MovimientosPorCuenta(LaPePeEntities ctx, int? idCta)
        //{
        //    var query = ctx.DMovCtaxBco.Where(mov => mov.IdCtaxBco == idCta).AsQueryable();
        //    return query;
        //}

        public static DMovCtaxBco UltimoMovimientoPorFecha(int? idCtaxBco, int? idMovCtaxBco, DateTime? fechaOperacion)
        {//ultimo movimiento, diferente del que se esta editando para que no interfiera en las operaciones
            using (var ctx = new LaPePeEntities())
            {
                //busco los q tengan fechaOpe igual o menor
                var ultimoMovimiento = ctx.DMovCtaxBco.Where(mov => mov.cEstado == "A" && mov.IdCtaxBco == idCtaxBco && mov.IdMovCtaxBco != idMovCtaxBco) //hago un pre filtrado
                    .Where(mov => mov.dFecOpe == fechaOperacion && mov.IdMovCtaxBco < idMovCtaxBco || mov.dFecOpe < fechaOperacion) //con el or || seleccionara 2 veces segun las condiciones
                    .OrderByDescending(mov => mov.dFecOpe).ThenByDescending(mov => mov.IdMovCtaxBco).FirstOrDefault(); //agregar toList si deseo depurarlo??

                return ultimoMovimiento;
            }
        }

        public static void ActualizarUltimosSaldos(int? idCtaxBco, int? idMovCtaxBco, DateTime? fechaOperacion, decimal? saldoInicial)
        {//ultimo movimiento, diferente del que se esta editando para que no interfiera en las operaciones
            using (var ctx = new LaPePeEntities())
            {
                //busco los q tengan fechaOpe igual o menor
                var ultimosMovimientos = ctx.DMovCtaxBco.Where(mov => mov.cEstado == "A" && mov.IdCtaxBco == idCtaxBco) //hago un pre filtrado
                    .Where(mov => mov.dFecOpe == fechaOperacion && mov.IdMovCtaxBco >= idMovCtaxBco || mov.dFecOpe > fechaOperacion) //con el or || seleccionara 2 veces segun las condiciones
                    .OrderBy(mov => mov.dFecOpe).ThenBy(mov => mov.IdMovCtaxBco); //agregar toList si deseo depurarlo??

                foreach (DMovCtaxBco movimiento in ultimosMovimientos)
                {
                    saldoInicial = saldoInicial + movimiento.nImporte;
                    movimiento.nSalFin = saldoInicial;
                }
                ctx.SaveChanges();
            }
        }

        public static DMovCtaxBco DMovCtaxBco(int? IdMovCtaxBco)
        {//ultimo movimiento, diferente del que se esta editando para que no interfiera en las operaciones
            using (var ctx = new LaPePeEntities())
            {
                var query = ctx.DMovCtaxBco.Where(mov => mov.IdMovCtaxBco == IdMovCtaxBco).FirstOrDefault();
                return query;
            }
        }
    }
}
