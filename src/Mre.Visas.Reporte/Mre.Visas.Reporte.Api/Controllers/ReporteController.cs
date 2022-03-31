using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mre.Visas.Reporte.Application.Reporte.Requests;
using Mre.Visas.Reporte.Application.Reporte.Responses;
using Mre.Visas.Reporte.Application.Shared.Wrappers;
using Mre.Visas.Reporte.Application.VisaElectronica.Requests;
using Mre.Visas.Reporte.Application.VisaElectronica.Responses;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReporteController : BaseController
  {
    private IConfiguration configuration;
    public ReporteController(IConfiguration iconfiguration)
    {
      configuration = iconfiguration;
    }
    [HttpGet("Mensaje")]
    [ActionName(nameof(MensajeAsync))]
    public string MensajeAsync()
    {
      return "Prueba";
    }
    [HttpPost("GenerarSolicitudVisaPorTramiteId")]
    [ActionName(nameof(GenerarSolicitudVisaPorTramiteIdAsync))]
    public async Task<GenerarPDFPorTramiteIdResponse> GenerarSolicitudVisaPorTramiteIdAsync(GenerarSolicitudVisaPorTramiteIdRequest request)
    {
      var result = new GenerarPDFPorTramiteIdResponse();
      try
      {
        HttpClient Client;
        String Uri = string.Empty;
        string PlacesJson = string.Empty;
        HttpResponseMessage Response;
        var datosCatalogo = new ConsultarCatalogoPorCatalogoCabeceraCodigoResponse();
        #region Consumir Pagos
        var pagoResponse = new ObtenerPagoResponse();
        ObtenerPagoRequest obtenerPagoRequest = new ObtenerPagoRequest { idTramite = request.Id, valoresMayoraCero = false, facturarEn = "0" };
        string urlPago = configuration.GetSection("RemoteServices").GetSection("Pago").GetSection("BaseUrl").Value;

        Client = new HttpClient();

        Uri = urlPago + "api/Pago/ObtenerPago?idTramite=" + obtenerPagoRequest.idTramite + "&valoresMayoraCero=false&facturarEn=0";
        Response = await Client.PostAsync(Uri, null);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          pagoResponse = JsonConvert.DeserializeObject<ObtenerPagoResponse>(PlacesJson);
        }
        else
        {
          return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Datos de pagos" };
        }
        #endregion
        #region Consumir Catalogos
        string urlCatalogo = configuration.GetSection("RemoteServices").GetSection("Catalogo").GetSection("BaseUrl").Value;

        Client = new HttpClient();
        Uri = urlCatalogo + "api/Catalogo/ConsultarCatalogoPorCatalogoCabeceraCodigo?codigoCatalogo=REQUISITO";
        Response = await Client.GetAsync(Uri);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;

          datosCatalogo = JsonConvert.DeserializeObject<ConsultarCatalogoPorCatalogoCabeceraCodigoResponse>(PlacesJson);
          if (datosCatalogo.httpStatusCode == 200)
          {
            //
          }
        }

        #endregion
        #region Consumir Tramite
        string urlTramite = configuration.GetSection("RemoteServices").GetSection("Tramite").GetSection("BaseUrl").Value;

        ConsultarTramitePorIdRequest consultarTramitePorIdRequest = new ConsultarTramitePorIdRequest
        {
          Id = request.Id
        };
        Client = new HttpClient();
        var json = JsonConvert.SerializeObject(consultarTramitePorIdRequest);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        Uri = urlTramite + "api/Tramite/ConsultarTramitePorId?Id=" + request.Id;
        Response = await Client.PostAsync(Uri, data);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          var datos = new ConsultarTramitePorIdResponse();

          datos = JsonConvert.DeserializeObject<Application.Reporte.Responses.ConsultarTramitePorIdResponse>(PlacesJson);
          foreach (var item in datos.result.Documentos)
          {
            if (datosCatalogo.result.FirstOrDefault(x => x.codigo.Equals(item.TipoDocumento)) != null)
            {
              item.Nombre = datosCatalogo.result.FirstOrDefault(x => x.codigo.Equals(item.TipoDocumento)).descripcion;
            }
          }

          string base64 = new Shared.Handlers.ReportHandler().GenerarSolicitudVisa(datos.result, pagoResponse);
          result.Base64 = base64;
          return result;
        }
        else
        {
          result.Estado = "ERROR";
          result.Mensaje = "No se logro generar el generar el pdf";
          return result;
        }
        #endregion
      }
      catch (Exception ex)
      {
        result.Estado = "ERROR";
        result.Mensaje = ex.Message.ToString();
        return result;
      }
    }

    // POST: api/Tramite/GenerarVisaPorTramiteId
    [HttpPost("GenerarVisaPorTramiteId")]
    [ActionName(nameof(GenerarVisaPorTramiteIdAsync))]
    public async Task<GenerarPDFPorTramiteIdResponse> GenerarVisaPorTramiteIdAsync(GenerarVisaPorTramiteIdRequest request)
    {
      var result = new GenerarPDFPorTramiteIdResponse();
      try
      {
        #region Mensajes
        string EncabezadoEsp = string.Empty;
        string EncabezadoIng = string.Empty;
        string InformacionEsp = string.Empty;
        string InformacionIng = string.Empty;

        string UrlMensajes = configuration.GetSection("RemoteServices").GetSection("Mensaje").GetSection("BaseUrl").Value;
        HttpClient Client;
        String Uri = string.Empty;
        string PlacesJson = string.Empty;
        HttpResponseMessage Response;
        var datosMensaje = new CrearMovimientoSharePointResponse();
        Client = new HttpClient();
        //Encabcezado Esp
        Uri = UrlMensajes + "SharePointMensajes/api/mensaje?modulo=VisaElectronica&pagina=EncabezadoEsp";
        Response = await Client.GetAsync(Uri);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          datosMensaje = JsonConvert.DeserializeObject<CrearMovimientoSharePointResponse>(PlacesJson);
          if (datosMensaje.Mensaje.Contains("Error al obtener la lista de mensajes"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "No existe el mensaje EncabezadoEsp" };
          else if (datosMensaje.Mensaje.Contains("Error"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = datosMensaje.Mensaje };
          else
            EncabezadoEsp = datosMensaje.Mensaje;
        }
        else
          return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: mensaje EncabezadoEsp" };
        //Encabezado Ing
        Uri = UrlMensajes + "SharePointMensajes/api/mensaje?modulo=VisaElectronica&pagina=EncabezadoIng";
        Response = await Client.GetAsync(Uri);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          datosMensaje = JsonConvert.DeserializeObject<CrearMovimientoSharePointResponse>(PlacesJson);
          if (datosMensaje.Mensaje.Contains("Error al obtener la lista de mensajes"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "No existe el mensaje EncabezadoIng" };
          else if (datosMensaje.Mensaje.Contains("Error"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = datosMensaje.Mensaje };
          else
            EncabezadoIng = datosMensaje.Mensaje;
        }
        else
          return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: mensaje EncabezadoIng" };
        //Informacion Ing
        Uri = UrlMensajes + "SharePointMensajes/api/mensaje?modulo=VisaElectronica&pagina=InformacionImportanteIng";
        Response = await Client.GetAsync(Uri);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          datosMensaje = JsonConvert.DeserializeObject<CrearMovimientoSharePointResponse>(PlacesJson);
          if (datosMensaje.Mensaje.Contains("Error al obtener la lista de mensajes"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "No existe el mensaje InformacionIng" };
          else if (datosMensaje.Mensaje.Contains("Error"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = datosMensaje.Mensaje };
          else
            InformacionIng = datosMensaje.Mensaje;
        }
        else
          return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: mensaje InformacionIng" };
        //Informacion Esp
        Uri = UrlMensajes + "SharePointMensajes/api/mensaje?modulo=VisaElectronica&pagina=InformacionImportanteEsp";
        Response = await Client.GetAsync(Uri);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          datosMensaje = JsonConvert.DeserializeObject<CrearMovimientoSharePointResponse>(PlacesJson);
          if (datosMensaje.Mensaje.Contains("Error al obtener la lista de mensajes"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "No existe el mensaje InformacionEsp" };
          else if (datosMensaje.Mensaje.Contains("Error"))
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = datosMensaje.Mensaje };
          else
            InformacionEsp = datosMensaje.Mensaje;
        }
        else
          return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: mensaje InformacionEsp" };
        #endregion

        #region Consumir Tramite
        string urlTramite = configuration.GetSection("RemoteServices").GetSection("Visa").GetSection("BaseUrl").Value;

        ConsultarVisaElectronicaPorTramiteIdRequest consultarTramitePorIdRequest = new ConsultarVisaElectronicaPorTramiteIdRequest
        {
          TramiteId = request.TramiteId
        };
        Client = new HttpClient();
        var json = JsonConvert.SerializeObject(consultarTramitePorIdRequest);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        Uri = urlTramite + "api/VisaElectronica/ConsultarVisaElectronicaPorTramiteId";
        Response = await Client.PostAsync(Uri, data);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          var datos = new ConsultarVisaElectronicaPorTramiteIdResponse();
          datos = JsonConvert.DeserializeObject<ConsultarVisaElectronicaPorTramiteIdResponse>(PlacesJson);

          #region Visa Codigos Qr
          string base64VisaQr = string.Empty;
          string UrlVisa = configuration.GetSection("RemoteServices").GetSection("Visa").GetSection("BaseUrl").Value;
          Uri = UrlVisa + "api/VisaElectronica/GenerarCodigoQr?numero=" + datos.result.numeroVisa + "";
          Response = await Client.GetAsync(Uri);
          if (Response.StatusCode == HttpStatusCode.OK)
            base64VisaQr = Response.Content.ReadAsStringAsync().Result;
          else
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: codigo Qr" };

          #endregion

          #region Visa Codigo Barras
          string base64VisaCodigoBarras = string.Empty;
          UrlVisa = configuration.GetSection("RemoteServices").GetSection("Visa").GetSection("BaseUrl").Value;
          Uri = UrlVisa + "api/VisaElectronica/GenerarBase64CodigoBarras?cadena=" + datos.result.numeroVisa + "";
          Response = await Client.GetAsync(Uri);
          if (Response.StatusCode == HttpStatusCode.OK)
            base64VisaCodigoBarras = Response.Content.ReadAsStringAsync().Result;
          else
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: codigo Qr" };

          #endregion

          #region Pasaporte Codigo Barras
          string base64PasaporteCodigoBarras = string.Empty;
          UrlVisa = configuration.GetSection("RemoteServices").GetSection("Visa").GetSection("BaseUrl").Value;
          Uri = UrlVisa + "api/VisaElectronica/GenerarBase64CodigoBarras?cadena=" + datos.result.numeroPasaporte + "";
          Response = await Client.GetAsync(Uri);
          if (Response.StatusCode == HttpStatusCode.OK)
            base64PasaporteCodigoBarras = Response.Content.ReadAsStringAsync().Result;
          else
            return result = new GenerarPDFPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Servicio no encontrado: codigo Qr" };

          #endregion

          string base64 = new Shared.Handlers.ReportHandler().GenerarVisa(datos.result, EncabezadoEsp, EncabezadoIng, InformacionEsp, InformacionIng, base64VisaQr, base64VisaCodigoBarras, base64PasaporteCodigoBarras);
          result.Base64 = base64;
          return result;
        }
        else
        {
          result.Estado = "ERROR";
          result.Mensaje = "No se logro generar el generar el pdf";
          return result;
        }
        #endregion
      }
      catch (Exception ex)
      {
        result.Estado = "ERROR";
        result.Mensaje = ex.Message.ToString();
        return result;
      }
    }

    // POST: api/Tramite/GenerarPagoPorTramiteId
    [HttpPost("GenerarPagoPorTramiteId")]
    [ActionName(nameof(GenerarPagoPorTramiteIdAsync))]
    public async Task<GenerarPagoPorTramiteIdResponse> GenerarPagoPorTramiteIdAsync(GenerarVisaPorTramiteIdRequest request)
    {
      var result = new GenerarPagoPorTramiteIdResponse();
      try
      {
        HttpClient Client;
        String Uri = string.Empty;
        string PlacesJson = string.Empty;
        HttpResponseMessage Response;
        var datosCatalogo = new ConsultarCatalogoPorCatalogoCabeceraCodigoResponse();
        #region Consumir Pagos
        var pagoResponse = new ObtenerPagoResponse();
        ObtenerPagoRequest obtenerPagoRequest = new ObtenerPagoRequest { idTramite = request.TramiteId, valoresMayoraCero = false, facturarEn = "0" };
        string urlPago = configuration.GetSection("RemoteServices").GetSection("Pago").GetSection("BaseUrl").Value;

        Client = new HttpClient();

        Uri = urlPago + "api/Pago/ObtenerPago?idTramite=" + obtenerPagoRequest.idTramite + "&valoresMayoraCero=false&facturarEn=0";
        Response = await Client.PostAsync(Uri, null);
        if (Response.StatusCode == HttpStatusCode.OK)
        {
          PlacesJson = Response.Content.ReadAsStringAsync().Result;
          pagoResponse = JsonConvert.DeserializeObject<ObtenerPagoResponse>(PlacesJson);
          string[] base64 = new Shared.Handlers.ReportHandler().GenerarPagos(pagoResponse);
          result.Base64 = base64;
          return result;
        }
        else
        {
          return result = new GenerarPagoPorTramiteIdResponse { Estado = "ERROR", Mensaje = "Datos de pagos" };
        }
        #endregion
      }
      catch (Exception ex)
      {
        return result = new GenerarPagoPorTramiteIdResponse { Estado = "ERROR", Mensaje = ex.Message.ToString() };
      }

    }

  }
}
