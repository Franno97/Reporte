using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Reporte.Requests
{
  public class ObtenerPagoRequest
  {
    public Guid idTramite { get; set; }
    public bool valoresMayoraCero { get; set; }
    public string facturarEn  { get; set; }
  }
}
