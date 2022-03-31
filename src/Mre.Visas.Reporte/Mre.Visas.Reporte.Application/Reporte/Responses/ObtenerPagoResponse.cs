using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Reporte.Responses
{
  public class ObtenerPagoResponse
  {
    public int httpStatusCode { get; set; }
    public Result result { get; set; }
    
    public class Result
    {
      public string idTramite { get; set; }
      public int formaPago { get; set; }
      public string observacion { get; set; }
      public string estado { get; set; }
      public List<ListaPagoDetalle> listaPagoDetalle { get; set; }
      public string id { get; set; }
      public string solicitante { get; set; }
      public string documentoIdentificacion { get; set; }

      public string banco { get; set; }
      public string numeroCuenta { get; set; }
      public string tipoCuenta { get; set; }
      public string titularCuenta { get; set; }
    }
    public class ListaPagoDetalle
    {
      public string idTramite { get; set; }
      public string idPago { get; set; }
      public int orden { get; set; }
      public string descripcion { get; set; }
      public decimal valorArancel { get; set; }
      public decimal porcentajeDescuento { get; set; }
      public decimal valorDescuento { get; set; }
      public decimal valorTotal { get; set; }
      public string estado { get; set; }
      public string ordenPago { get; set; }
      public string numeroTransaccion { get; set; }
      public bool estaFacturado { get; set; }
      public string claveAcceso { get; set; }
      public string comprobantePago { get; set; }
      public string partidaArancelariaId { get; set; }
      public string servicioId { get; set; }
      public string servicio { get; set; }
      public string partidaArancelaria { get; set; }
      public string numeroPartida { get; set; }
      public string jerarquiaArancelariaId { get; set; }
      public string jerarquiaArancelaria { get; set; }
      public string arancelId { get; set; }
      public string arancel { get; set; }
      public string convenioId { get; set; }
      public string tipoServicio { get; set; }
      public string tipoExoneracionId { get; set; }
      public string tipoExoneracion { get; set; }
      public string facturarEn { get; set; }
      public DateTime lastModified { get; set; }
      public string lastModifierId { get; set; }
      public DateTime created { get; set; }
      public string creatorId { get; set; }
      public string id { get; set; }
      public bool isDeleted { get; set; }
    }
  }

}
