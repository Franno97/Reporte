using Microsoft.Reporting.NETCore;
using Mre.Visas.Reporte.Application.Reporte.Responses;
using Mre.Visas.Reporte.Application.VisaElectronica.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Api.Shared.Handlers
{
  public class ReportHandler
  {
    public string GenerarSolicitudVisa(Tramite tramite, ObtenerPagoResponse obtenerPagoResponse)
    {
      string base64 = string.Empty;
      try
      {
        var pago = obtenerPagoResponse.result.listaPagoDetalle.FirstOrDefault(x => x.orden == 1);
        string SOLTERO = string.Empty;
        string CASADO = string.Empty;
        string VIUDO = string.Empty;
        string DIVORCIADO = string.Empty;
        string UNION_HECHO = string.Empty;
        if (tramite.Beneficiario.EstadoCivil.Equals("SOLTERO"))
          SOLTERO = "x";
        else if (tramite.Beneficiario.EstadoCivil.Equals("CASADO"))
          CASADO = "x";
        else if (tramite.Beneficiario.EstadoCivil.Equals("VIUDO"))
          VIUDO = "x";
        else if (tramite.Beneficiario.EstadoCivil.Equals("DIVORCIADO"))
          DIVORCIADO = "x";
        else if (tramite.Beneficiario.EstadoCivil.Equals("UNION DE HECHO"))
          UNION_HECHO = "x";

        string FEMENINO = string.Empty;
        string MASCULINO = string.Empty;
        if (tramite.Beneficiario.Genero.Equals("F"))
          FEMENINO = "x";
        else if (tramite.Beneficiario.Genero.Equals("M"))
          MASCULINO = "x";

        byte[] imageIzquierda = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Shared", "Images", "SolicitudVisaIzquierda.png"));
        byte[] imageDerecha = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Shared", "Images", "SolicitudVisaDerecha.png"));
        string base64ImageIzquierda = Convert.ToBase64String(imageIzquierda);
        string base64ImageDerecha = Convert.ToBase64String(imageDerecha);

        string documentos = string.Empty;
        foreach (var i in tramite.Documentos)
        {
          documentos += i.TipoDocumentoDescripcion + "\n";
        }

        LocalReport report = new LocalReport();
        var parameters = new[]
        {
          new ReportParameter("varSolicitud", "x"),
          new ReportParameter("varCertificadoVisa",string.Empty),
          new ReportParameter("varRenovacion",string.Empty),
          new ReportParameter("varTransferencia",string.Empty),
          new ReportParameter("varCancelacion",string.Empty),
          new ReportParameter("varResidenciaTemporal","x"),
          new ReportParameter("varResidenciaPermanente",string.Empty),
          new ReportParameter("varVisitanteTemporal",string.Empty),
          new ReportParameter("varDiplomatico",string.Empty),
          new ReportParameter("varNumeroPasaporte",tramite.Beneficiario.Pasaporte.Numero),
          new ReportParameter("varTipoIdentificacion",tramite.Beneficiario.Pasaporte.TipoDocumentoIdentidadId),
          new ReportParameter("varPaisEmitido",tramite.Beneficiario.Pasaporte.PaisEmision),
          new ReportParameter("varFechaEmision",tramite.Beneficiario.Pasaporte.FechaEmision.ToString("dd/MM/yyyy")),
          new ReportParameter("varFechaExpiracion",tramite.Beneficiario.Pasaporte.FechaExpiracion.ToString("dd/MM/yyyy")),
          new ReportParameter("varApellido",tramite.Beneficiario.PrimerApellido+" "+tramite.Beneficiario.SegundoApellido),
          new ReportParameter("varNombres",tramite.Beneficiario.Nombres),
          new ReportParameter("varLugarNacimiento",tramite.Beneficiario.CiudadNacimiento),
          new ReportParameter("varFechaNacimiento",tramite.Beneficiario.FechaNacimiento.ToString("dd/MM/yyyy")),
          new ReportParameter("varNacionalidad",tramite.Beneficiario.Nacionalidad),
          new ReportParameter("varOcupacion",tramite.Beneficiario.Ocupacion),
          new ReportParameter("varSoltero",SOLTERO),
          new ReportParameter("varCasado",CASADO),
          new ReportParameter("varViudo",VIUDO),
          new ReportParameter("varDivorciado",DIVORCIADO),
          new ReportParameter("varUnionHecho",UNION_HECHO),
          new ReportParameter("varFemenino",FEMENINO),
          new ReportParameter("varMasculino",MASCULINO),
          new ReportParameter("varNumeroTramite",tramite.Numero),
          new ReportParameter("varDireccion",tramite.Beneficiario.Domicilio.Direccion),
          new ReportParameter("varCiudad",tramite.Beneficiario.Domicilio.Ciudad),
          new ReportParameter("varProvincia",tramite.Beneficiario.Domicilio.Provincia),
          new ReportParameter("varCorreoElectronico",tramite.Beneficiario.Correo),
          new ReportParameter("varTelefonoMovil",tramite.Beneficiario.Domicilio.TelefonoCelular),
          new ReportParameter("varTelefonoDomicilio",tramite.Beneficiario.Domicilio.TelefonoDomicilio),
          //datos de imagenes : foto
          new ReportParameter("varFoto",tramite.Beneficiario.Foto),
          new ReportParameter("varFotoMimeType","image/png"),
          //datos de imagenes : cabeceras
          new ReportParameter("varIzquierda",base64ImageIzquierda),
          new ReportParameter("varIzquierdaMimeType","image/png"),
          new ReportParameter("varDerecha",base64ImageDerecha),
          new ReportParameter("varDerechaMimeType","image/png"),
          new ReportParameter("varDocumentos",documentos),
          new ReportParameter("varFecha",tramite.Fecha),
          new ReportParameter("varArancel",pago.arancel),
          new ReportParameter("varNumeroActuacion","NO APLICA"),
          new ReportParameter("varValor",pago.valorTotal.ToString("F2")),
          new ReportParameter("varFechaPago",pago.created.ToString("dd/MM/yyyy"))
        };


        using var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "Shared", "Reports", "SolicitudVisa.rdlc"), FileMode.Open);
        //using var fs = new FileStream(@"Shared\Reports\SolicitudVisa.rdlc", FileMode.Open);

        report.EnableExternalImages = true;
        report.LoadReportDefinition(fs);
        report.SetParameters(parameters);

        byte[] bytes = report.Render("PDF");
        base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
        return base64;
      }
      catch
      {
        throw;
      }
    }
    public string GenerarVisa(VisaElectronica visaElectronica, string EncabezadoEsp, string EncabezadoIng, string InformacionEsp, string InformacionIng, string base64VisaQr, string base64VisaCodigoBarras, string base64PasaporteCodigoBarras)
    {

      EncabezadoEsp = EncabezadoEsp.Replace("UNIDAD_ADMINISTRATIVA", visaElectronica.unidadAdministrativaNombre).Replace("NUMERO", visaElectronica.numeroVisa);
      EncabezadoIng = EncabezadoIng.Replace("UNIDAD_ADMINISTRATIVA", visaElectronica.unidadAdministrativaNombre).Replace("NUMERO", visaElectronica.numeroVisa);

      string base64 = string.Empty;
      try
      {
        LocalReport report = new LocalReport();
        var fechaNacimiento = visaElectronica.fechaNacimiento.Substring(0, visaElectronica.fechaNacimiento.IndexOf(' '));

        var parameters = new[]
        {
          new ReportParameter("varLugarExpedicion", visaElectronica.unidadAdministrativaCiudad),//Ciudad de la direccion del unidad administrativa
          new ReportParameter("varValidoDesde",visaElectronica.fechaEmision.ToString("dd/MM/yyyy")),
          new ReportParameter("varValidoHasta",visaElectronica.fechaExpiracion.ToString("dd/MM/yyyy")),
          new ReportParameter("varActividad",visaElectronica.actividadDesarrollar),
          new ReportParameter("varTipo",visaElectronica.categoria),
          new ReportParameter("varApellidosNombres",visaElectronica.apellidosBeneficiario+" "+visaElectronica.nombresBeneficiario),
          new ReportParameter("varNumeroDocumento",visaElectronica.numeroVisa),
          new ReportParameter("varNumeroEntradas",visaElectronica.numeroAdmisiones),
          new ReportParameter("varNumeroPasaporte",visaElectronica.numeroPasaporte),
          new ReportParameter("varFechaNacimiento",fechaNacimiento),
          new ReportParameter("varGenero",visaElectronica.genero),
          new ReportParameter("varNacionalidad",visaElectronica.nacionalidad),
          new ReportParameter("varObservaciones",visaElectronica.observaciones),
          new ReportParameter("varEncabezadoEsp",HtmlToPlainText(EncabezadoEsp)),//aqui los replace con nombre
          new ReportParameter("varEncabezadoIng",HtmlToPlainText(EncabezadoIng)),
          new ReportParameter("varInformacionEsp",HtmlToPlainText(InformacionEsp)),
          new ReportParameter("varInformacionIng",HtmlToPlainText(InformacionIng)),
          //datos de imagenes : foto
          new ReportParameter("varFoto",visaElectronica.fotoBeneficiario),
          new ReportParameter("varFotoMimeType","image/png"),
          
          //codigos
          new ReportParameter("varVisaQr",base64VisaQr),
          new ReportParameter("varVisaQrMimeType","image/png"),

          new ReportParameter("varVisaCodigoBarras",base64VisaCodigoBarras),
          new ReportParameter("varVisaCodigoBarrasMimeType","image/png"),

          new ReportParameter("varPasaporteCodigoBarras",base64PasaporteCodigoBarras),
          new ReportParameter("varPasaporteCodigoBarrasMimeType","image/png")
        };

        using var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "Shared", "Reports", "VisaGenerada.rdlc"), FileMode.Open);
        //report.EnableExternalImages = true;
        report.LoadReportDefinition(fs);
        report.SetParameters(parameters);

        byte[] bytes = report.Render("PDF");
        base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
        return base64;
      }
      catch
      {
        throw;
      }
    }
    public string[] GenerarPagos(ObtenerPagoResponse obtenerPago)
    {
      byte[] imageIzquierda = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Shared", "Images", "SolicitudVisaIzquierda.png"));
      byte[] imageDerecha = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Shared", "Images", "SolicitudVisaDerecha.png"));
      string base64ImageIzquierda = Convert.ToBase64String(imageIzquierda);
      string base64ImageDerecha = Convert.ToBase64String(imageDerecha);

      string[] dato = { "", "" };
      try
      {
        #region Solicitud de Visa
        var resulPagoCabecera = obtenerPago.result;
        var resulSolicitudVisa = obtenerPago.result.listaPagoDetalle.FirstOrDefault(x => x.orden.Equals(1));
        LocalReport reportSolicitudVisa = new LocalReport();
        var paramCedulacion = new[]
        {
          new ReportParameter("varNumero", resulSolicitudVisa.ordenPago),
          new ReportParameter("varFechaEmision",resulSolicitudVisa.created.ToString("dd/MM/yyyy")),
          new ReportParameter("varDetalle",resulSolicitudVisa.descripcion),
          new ReportParameter("varValorUnitario",resulSolicitudVisa.valorArancel.ToString("F2")),
          new ReportParameter("varValorTotal",resulSolicitudVisa.valorTotal.ToString("F2")),
          new ReportParameter("varNumeroLetras",NumeroALetras(Convert.ToDouble(resulSolicitudVisa.valorTotal))),
          new ReportParameter("varNombreDepositante",obtenerPago.result.solicitante),
          new ReportParameter("varIdentificacion",obtenerPago.result.documentoIdentificacion),
          new ReportParameter("varIzquierda",base64ImageIzquierda),
          new ReportParameter("varIzquierdaMimeType","image/png"),
          new ReportParameter("varDerecha",base64ImageDerecha),
          new ReportParameter("varDerechaMimeType","image/png"),
          new ReportParameter("varBanco", resulPagoCabecera.banco),
          new ReportParameter("varNumeroCuenta", resulPagoCabecera.numeroCuenta),
          new ReportParameter("varTipoCuenta", resulPagoCabecera.tipoCuenta),
          new ReportParameter("varTitularCuenta", resulPagoCabecera.titularCuenta)
        };

        using var fsSolcititud = new FileStream(Path.Combine(Environment.CurrentDirectory, "Shared", "Reports", "ComprobanteRecaudacion.rdlc"), FileMode.Open);
        reportSolicitudVisa.EnableExternalImages = true;
        reportSolicitudVisa.LoadReportDefinition(fsSolcititud);
        reportSolicitudVisa.SetParameters(paramCedulacion);

        byte[] bytesSolicitudVisa = reportSolicitudVisa.Render("PDF");
        dato[0] = Convert.ToBase64String(bytesSolicitudVisa, 0, bytesSolicitudVisa.Length);
        fsSolcititud.Dispose();
        #endregion

        #region Orden de Cedulacion
        var resultOrdenCedulacion = obtenerPago.result.listaPagoDetalle.FirstOrDefault(x => x.orden.Equals(2));
        LocalReport reportOrdenCedulacion = new LocalReport();
        var parametersCedulacion = new[]
        {
          new ReportParameter("varNumero", resultOrdenCedulacion.ordenPago),
          new ReportParameter("varFechaEmision",resultOrdenCedulacion.created.ToString("dd/MM/yyyy")),
          new ReportParameter("varDetalle",resultOrdenCedulacion.descripcion),
          new ReportParameter("varValorUnitario",resultOrdenCedulacion.valorArancel.ToString("F2")),
          new ReportParameter("varValorTotal",resultOrdenCedulacion.valorTotal.ToString("F2")),
          new ReportParameter("varNumeroLetras",NumeroALetras(Convert.ToDouble(resultOrdenCedulacion.valorTotal))),
          new ReportParameter("varNombreDepositante",obtenerPago.result.solicitante),
          new ReportParameter("varIdentificacion",obtenerPago.result.documentoIdentificacion),
          new ReportParameter("varIzquierda",base64ImageIzquierda),
          new ReportParameter("varIzquierdaMimeType","image/png"),
          new ReportParameter("varDerecha",base64ImageDerecha),
          new ReportParameter("varDerechaMimeType","image/png"),
          new ReportParameter("varBanco", resulPagoCabecera.banco),
          new ReportParameter("varNumeroCuenta", resulPagoCabecera.numeroCuenta),
          new ReportParameter("varTipoCuenta", resulPagoCabecera.tipoCuenta),
          new ReportParameter("varTitularCuenta", resulPagoCabecera.titularCuenta)
        };

        using var fsCedulacion = new FileStream(Path.Combine(Environment.CurrentDirectory, "Shared", "Reports", "ComprobanteRecaudacion.rdlc"), FileMode.Open);
        reportOrdenCedulacion.EnableExternalImages = true;
        reportOrdenCedulacion.LoadReportDefinition(fsCedulacion);
        reportOrdenCedulacion.SetParameters(parametersCedulacion);

        byte[] bytesOrdenCedulacion = reportOrdenCedulacion.Render("PDF");
        dato[1] = Convert.ToBase64String(bytesOrdenCedulacion, 0, bytesOrdenCedulacion.Length);
        fsCedulacion.Dispose();
        #endregion

      }
      catch
      {
        throw;
      }

      return dato;
    }
    private static string HtmlToPlainText(string html)
    {
      const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
      const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
      const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
      var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
      var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
      var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

      var text = html;
      //Decode html specific characters
      text = System.Net.WebUtility.HtmlDecode(text);
      //Remove tag whitespace/line breaks
      text = tagWhiteSpaceRegex.Replace(text, "><");
      //Replace <br /> with line breaks
      text = lineBreakRegex.Replace(text, Environment.NewLine);
      //Strip formatting
      text = stripFormattingRegex.Replace(text, string.Empty);

      return text;
    }
    private static string NumeroALetras(double value)
    {
      string num2Text; value = Math.Truncate(value);
      if (value == 0) num2Text = "CERO";
      else if (value == 1) num2Text = "UNO";
      else if (value == 2) num2Text = "DOS";
      else if (value == 3) num2Text = "TRES";
      else if (value == 4) num2Text = "CUATRO";
      else if (value == 5) num2Text = "CINCO";
      else if (value == 6) num2Text = "SEIS";
      else if (value == 7) num2Text = "SIETE";
      else if (value == 8) num2Text = "OCHO";
      else if (value == 9) num2Text = "NUEVE";
      else if (value == 10) num2Text = "DIEZ";
      else if (value == 11) num2Text = "ONCE";
      else if (value == 12) num2Text = "DOCE";
      else if (value == 13) num2Text = "TRECE";
      else if (value == 14) num2Text = "CATORCE";
      else if (value == 15) num2Text = "QUINCE";
      else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
      else if (value == 20) num2Text = "VEINTE";
      else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
      else if (value == 30) num2Text = "TREINTA";
      else if (value == 40) num2Text = "CUARENTA";
      else if (value == 50) num2Text = "CINCUENTA";
      else if (value == 60) num2Text = "SESENTA";
      else if (value == 70) num2Text = "SETENTA";
      else if (value == 80) num2Text = "OCHENTA";
      else if (value == 90) num2Text = "NOVENTA";
      else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
      else if (value == 100) num2Text = "CIEN";
      else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
      else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
      else if (value == 500) num2Text = "QUINIENTOS";
      else if (value == 700) num2Text = "SETECIENTOS";
      else if (value == 900) num2Text = "NOVECIENTOS";
      else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
      else if (value == 1000) num2Text = "MIL";
      else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
      else if (value < 1000000)
      {
        num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
        if ((value % 1000) > 0)
        {
          num2Text = num2Text + " " + NumeroALetras(value % 1000);
        }
      }
      else if (value == 1000000)
      {
        num2Text = "UN MILLON";
      }
      else if (value < 2000000)
      {
        num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
      }
      else if (value < 1000000000000)
      {
        num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
        if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
        {
          num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
        }
      }
      else if (value == 1000000000000) num2Text = "UN BILLON";
      else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
      else
      {
        num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
        if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
        {
          num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
      }
      return num2Text;
    }
  }
}
