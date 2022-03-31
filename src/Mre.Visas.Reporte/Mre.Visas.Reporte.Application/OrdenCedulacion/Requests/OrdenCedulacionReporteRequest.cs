using System;
using System.ComponentModel.DataAnnotations;

namespace Mre.Visas.Reporte.Application.OrdenCedulacion
{
    public class OrdenCedulacionReporteRequest
    {
        private const string ValorDefecto = "Presentar documento habilitante en Registro Civil";


        /// <summary>
        /// Nombre del tipo de visa emitida que genera esta orden de cedulacion
        /// </summary>
        [Required]
        public string TipoVisa { get; set; }

        /// <summary>
        /// Numero de orden de cedulacion
        /// </summary>
        [Required]
        public string Numero { get; set; }

        /// <summary>
        /// Codigo unico de orden de cedulacion
        /// </summary>
        [Required]
        public string CodigoVerificacion { get; set; }

        /// <summary>
        /// Fotografia del beneficiario de la orden de cedulacion. Formato base64
        /// </summary>
        [Required]
        public string Fotografia { get; set; }

        /// <summary>
        /// Nombres del beneficiario de la orden de cedulacíon
        /// </summary>
        [Required]
        public string Nombres { get; set; }

        /// <summary>
        /// Primer apellido del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string PrimerApellido { get; set; }

        /// <summary>
        /// Segundo apellido del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string SegundoApellido { get; set; }

        /// <summary>
        /// Nacionalidad del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string Nacionalidad { get; set; }

        /// <summary>
        /// Pais de nacimiento del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string PaisNacimiento { get; set; }

        /// <summary>
        /// Ciudad de nacimiento del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string CiudadNacimiento { get; set; }

        /// <summary>
        /// Fecha de nacimiento del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Sexo del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string Sexo { get; set; }

        /// <summary>
        /// Estado civil del beneficiario de la orden de cedulacion
        /// </summary>
        [Required]
        public string EstadoCivil { get; set; }

        /// <summary>
        /// Apellidos del conyuge, si aplica
        /// </summary>
        public string ApellidosConyuge { get; set; } = ValorDefecto;

        /// <summary>
        /// Nombres del conyuge, si aplica
        /// </summary>
        public string NombresConyuge { get; set; } = ValorDefecto;

        /// <summary>
        /// Nacionalidad del conyuge, si aplica
        /// </summary>
        public string NacionalidadConyuge { get; set; } = ValorDefecto;



        /// <summary>
        /// Numero de visa
        /// </summary>
        [Required]
        public string NumeroVisa { get; set; }

        /// <summary>
        /// Fecha de otorgamiento de visa
        /// </summary>
        [Required]
        public DateTime FechaOtorgamientoVisa { get; set; }

        /// <summary>
        /// Tiempo de vigencia de la visa
        /// </summary>
        [Required]
        public string TiempoVigenciaVisa { get; set; }

        /// <summary>
        /// Unidad de otorgamiento de la visa
        /// </summary>
        [Required]
        public string UnidadOtorgamientoVisa { get; set; }

        /// <summary>
        /// Fecha de emision de la orden de cedulacion
        /// </summary>
        [Required]
        public DateTime FechaEmision { get; set; }

        /// <summary>
        /// Periodo de validez de la orden de cedulacion en dias
        /// </summary>
        public int Validez { get; set; }


        [Required]
        public string Signatario { get; set; }

        [Required]
        public string CategoriaVisa { get; set; }


    }
}
