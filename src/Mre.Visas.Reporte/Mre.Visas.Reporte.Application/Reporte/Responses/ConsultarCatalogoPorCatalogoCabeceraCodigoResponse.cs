using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Reporte.Responses
{
  public class ConsultarCatalogoPorCatalogoCabeceraCodigoResponse
  {
    public int httpStatusCode { get; set; }
    public List<Result> result { get; set; }
  }
  public class Result
  {
    public string catalogoCabeceraId { get; set; }
    public string codigo { get; set; }
    public string descripcion { get; set; }
    public string descripcion2 { get; set; }
    public string codigoEsigex { get; set; }
    public string codigoMdg { get; set; }
    public string codigoOtro { get; set; }
    public DateTime lastModified { get; set; }
    public string lastModifierId { get; set; }
    public DateTime created { get; set; }
    public string creatorId { get; set; }
    public string id { get; set; }
    public bool isDeleted { get; set; }
  }
}
