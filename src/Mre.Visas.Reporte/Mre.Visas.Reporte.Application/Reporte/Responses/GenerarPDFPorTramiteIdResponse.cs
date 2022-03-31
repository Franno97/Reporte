using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Reporte.Responses
{
  public class GenerarPDFPorTramiteIdResponse
  {
    public string Estado { get; set; }
    public string Base64 { get; set; }
    public string Mensaje { get; set; }
    public GenerarPDFPorTramiteIdResponse()
    {
      Estado = "OK";
      Mensaje = "Generado Correctamente";
      Base64 = "";
    }
  }
}
