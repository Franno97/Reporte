using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Reporte.Responses
{
  public class ConsultarTramitePorIdResponse
  {
    public int httpStatusCode { get; set; }
    public Tramite result { get; set; }

  }
  public class Tramite : AuditableEntity
  {
    /// <summary>
    /// Numero del tramite se compone de la fecha en formato yyyyMMdd-contador
    /// </summary>
    public string Numero { get; set; }

    /// <summary>
    /// Fecha del tramite
    /// </summary>
    public string Fecha { get; set; }

    /// <summary>
    /// Nombre d la actividad
    /// </summary>
    public Guid ActividadId { get; set; }

    /// <summary>
    /// Datos del Beneficiario
    /// </summary>
    public Beneficiario Beneficiario { get; set; }

    /// <summary>
    /// Id del Beneficiario
    /// </summary>
    public Guid BeneficiarioId { get; set; }

    /// <summary>
    /// Calidad migratoria
    /// </summary>
    public Guid CalidadMigratoriaId { get; set; }

    /// <summary>
    /// Los documentos adjuntos
    /// </summary>
    public List<Documento> Documentos { get; set; }

    /// <summary>
    /// Datos del grupo que pertenece
    /// </summary>
    public Guid TipoConvenioId { get; set; }

    /// <summary>
    /// Datos del solciitante
    /// </summary>
    public Solicitante Solicitante { get; set; }

    /// <summary>
    /// Id del solicitante
    /// </summary>
    public Guid SolicitanteId { get; set; }

    /// <summary>
    /// Tipo de visas
    /// </summary>
    public Guid TipoVisaId { get; set; }

    /// <summary>
    /// Unidad Administrativa entro de Emision de Visas
    /// </summary>
    public Guid UnidadAdministrativaIdCEV { get; set; }

    /// <summary>
    /// Unidad Administrativa de la Zonal
    /// </summary>
    public Guid UnidadAdministrativaIdZonal { get; set; }

    /// <summary>
    /// Los documentos adjuntos
    /// </summary>
    public List<SoporteGestion> SoporteGestiones { get; set; }

    /// <summary>
    /// Los documentos adjuntos
    /// </summary>
    public List<Movimiento> Movimientos { get; set; }

    /// <summary>
    /// Servicio Id
    /// </summary>
    public Guid ServicioId { get; set; }

    /// <summary>
    /// Codigo de Pais
    /// </summary>
    public string CodigoPais { get; set; }

    /// <summary>
    /// Persona Id
    /// </summary>
    public Guid PersonaId { get; set; }

    /// <summary>
    /// Persona Id
    /// </summary>
    public Guid OrigenId { get; set; }

    /// <summary>
    /// Fecha del tramite
    /// </summary>
    public string TipoTramite { get; set; }

  }
  public class AuditableEntity : BaseEntity
  {
    public DateTime LastModified { get; set; }

    public Guid LastModifierId { get; set; }

    public DateTime Created { get; set; }

    public Guid CreatorId { get; set; }
  }
  public class BaseEntity
  {
    #region Constructors

    public BaseEntity()
    {
    }

    #endregion Constructors

    #region Properties

    public Guid Id { get; protected set; }

    public bool IsDeleted { get; set; }

    #endregion Properties

    #region Methods

    public void AssignId()
    {
      Id = Guid.NewGuid();
    }

    public void MarkAsDeleted()
    {
      IsDeleted = true;
    }

    #endregion Methods
  }
  public class Beneficiario : BaseEntity
  {
    public TipoCiudadano.TipoCiudadanoEnum TipoCiudadano { get; set; }

    public string CiudadNacimiento { get; set; }

    public string CodigoMDG { get; set; }

    public string Correo { get; set; }

    public Domicilio Domicilio { get; set; }

    public Guid DomicilioId { get; set; }

    public int Edad { get; set; }

    public string EstadoCivil { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Foto { get; set; }

    public string Genero { get; set; }

    public string Ocupacion { get; set; }

    public string NacionalidadId { get; set; }

    public string Nacionalidad { get; set; }

    public string Nombres { get; set; }

    public string PaisNacimiento { get; set; }

    public Pasaporte Pasaporte { get; set; }

    public Guid PasaporteId { get; set; }

    public bool PoseeDiscapacidad { get; set; }

    public int PorcentajeDiscapacidad { get; set; }

    public string CarnetConadis { get; set; }

    public DateTime FechaEmisionConadis { get; set; }

    public DateTime FechaCaducidadConadis { get; set; }

    public string PrimerApellido { get; set; }

    public string SegundoApellido { get; set; }

    public Visa Visa { get; set; }

    public Guid VisaId { get; set; }
  }
  public class Documento : AuditableEntity
  {
    /// <summary>
    /// Id
    /// </summary>
    public Guid TramiteId { get; set; }

    public Tramite Tramite { get; set; }

    /// <summary>
    /// Nombre del archivo
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Ruta del documentoa almancenado el sharepoint
    /// </summary>
    public string Ruta { get; set; }

    /// <summary>
    /// Observacion del documento
    /// </summary>
    public string Observacion { get; set; }

    /// <summary>
    /// Campo para el Tipo de Documento
    /// </summary>
    public string TipoDocumento { get; set; }

    /// <summary>
    /// IconoNombre
    /// </summary>
    public string IconoNombre { get; set; }

    /// <summary>
    /// ImagenNombre
    /// </summary>
    public string ImagenNombre { get; set; }

    /// <summary>
    /// DescripcionDocumento
    /// </summary>
    public string DescripcionDocumento { get; set; }

    /// <summary>
    /// Estado del proceso cuando pasa por datacap
    /// </summary>
    public string EstadoProceso { get; set; }

    /// <summary>
    /// Campo para el Tipo de Documento Descripcion
    /// </summary>
    public string TipoDocumentoDescripcion { get; set; }

  }
  public class Domicilio : BaseEntity
  {
    public string Ciudad { get; set; }

    public string Direccion { get; set; }

    public string Pais { get; set; }

    public string Provincia { get; set; }

    public string TelefonoCelular { get; set; }

    public string TelefonoDomicilio { get; set; }

    public string TelefonoTrabajo { get; set; }
  }
  public class Movimiento : AuditableEntity
  {
    public Guid TramiteId { get; set; }

    public Tramite Tramite { get; set; }

    /// <summary>
    /// Numero de Orden como ingresa el proceso
    /// </summary>
    public int Orden { get; set; }

    /// <summary>
    /// Usuario que va a gestionar el tramite
    /// Ejemplo puede ser Consultor, Funcionario, Perito, otros
    /// </summary>
    public Guid UsuarioId { get; set; }

    /// <summary>
    /// El estado del flujo de proceso
    /// </summary>
    public string Estado { get; set; }

    /// <summary>
    /// El nombre estado del flujo de proceso
    /// </summary>
    public string NombreEstado { get; set; }

    /// <summary>
    /// Vigente se considera al ultimo como True y los anteres al orden como False
    /// </summary>
    public bool Vigente { get; set; }

    /// <summary>
    /// Nombre del rol del usuario en ese momento
    /// </summary>
    public string NombreRol { get; set; }

    /// <summary>
    /// Unidad Administrativa de la Zonal
    /// </summary>
    public Guid UnidadAdministrativaId { get; set; }

    /// <summary>
    /// observacion del tramite
    /// </summary>
    public string ObservacionDatosPersonales { get; set; }

    /// <summary>
    /// Observaciones de beneficiarios
    /// </summary>
    public string ObservacionDomicilios { get; set; }

    /// <summary>
    /// Observaciones de soportes de gestion
    /// </summary>
    public string ObservacionSoportesGestion { get; set; }

    /// <summary>
    /// Las observaciones al tab de movimientos migratorios
    /// </summary>
    public string ObservacionMovimientoMigratorio { get; set; }

    /// <summary>
    /// La observacion del tab de multas
    /// </summary>
    public string ObservacionMultas { get; set; }

    /// <summary>
    /// Este campo solo aplica para los movimientos de ciudadano
    /// </summary>
    public int DiasTranscurridos { get; set; }

    /// <summary>
    /// Formato de fecha desde para la cita ejemplo 2022-10-10 10:00:00
    /// </summary>
    public DateTime FechaHoraCita { get; set; }

  }
  public class Pasaporte : BaseEntity
  {
    public string CiudadEmision { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Nombres { get; set; }

    public string Numero { get; set; }

    public string PaisEmision { get; set; }

    public string TipoDocumentoIdentidadId { get; set; }
  }
  public class Solicitante : BaseEntity
  {
    public string Identificacion { get; set; }
    public string TipoIdentificacion { get; set; }

    public string Ciudad { get; set; }

    public string ConsuladoNombre { get; set; }

    public string ConsuladoPais { get; set; }

    public string Direccion { get; set; }

    public int Edad { get; set; }

    public string Nacionalidad { get; set; }

    public string Nombres { get; set; }

    public string Pais { get; set; }

    public string Telefono { get; set; }

    public string Correo { get; set; }
  }
  public class SoporteGestion : AuditableEntity
  {
    /// <summary>
    /// Id de tramite
    /// </summary>
    public Guid TramiteId { get; set; }

    public Tramite Tramite { get; set; }

    /// <summary>
    /// Nombre del archivo con extension
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Descripción del documento que se está adjuntando
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// Ruta del archivo de sharepoint
    /// </summary>
    public string Ruta { get; set; }
  }
  public class Visa : BaseEntity
  {
    public DateTime FechaEmision { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public string Numero { get; set; }

    public bool PoseeVisa { get; set; }

    public string Tipo { get; set; }

    public bool ConfirmacionVisa { get; set; }
  }
  public class FiltroTipo
  {
    public enum Tipo { Ninguno, NombreBeneficiario, NombreSolicitante, NumeroTramite, FechaEmision }
  }

  public class TipoCiudadano
  {
    public enum TipoCiudadanoEnum { Titular, Conyuge, Hijo }

  }
}
