using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.VisaElectronica.Responses
{
  public class ConsultarVisaElectronicaPorTramiteIdResponse
  {
    public int httpStatusCode { get; set; }
    public VisaElectronica result { get; set; }
  }
  public class VisaElectronica
  {
    public Guid tramiteId { get; set; }
    public string observaciones { get; set; }
    public string signatarioId { get; set; }
    public string nombreSignatario { get; set; }
    public int diasVigencia { get; set; }
    public DateTime fechaEmision { get; set; }
    public DateTime fechaExpiracion { get; set; }
    public int secuenciaVisa { get; set; }
    public string numeroVisa { get; set; }
    public string calidadMigratoria { get; set; }
    public string categoria { get; set; }
    public string numeroAdmisiones { get; set; }
    public string numeroPasaporte { get; set; }
    public string codigoVerificacion { get; set; }
    public string informacionQR { get; set; }
    public string nombresBeneficiario { get; set; }
    public string apellidosBeneficiario { get; set; }
    public string direccionDomiciliaria { get; set; }
    public string actividadDesarrollar { get; set; }
    public string requisitosCumplidos { get; set; }
    public string unidadAdministrativaId { get; set; }
    public string unidadAdministrativaNombre { get; set; }
    public string usuarioId { get; set; }
    public DateTime lastModified { get; set; }
    public string lastModifierId { get; set; }
    public DateTime created { get; set; }
    public string creatorId { get; set; }
    public string id { get; set; }
    public bool isDeleted { get; set; }

    public string fechaNacimiento { get; set; }
    public string genero { get; set; }
    public string nacionalidad { get; set; }
    public string unidadAdministrativaCiudad { get; set; }
    public string fotoBeneficiario { get; set; }
  }
}
