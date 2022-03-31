using MediatR;
using Microsoft.Reporting.NETCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.OrdenCedulacion
{
    public class OrdenCedulacionReporteCommand : OrdenCedulacionReporteRequest, IRequest<byte[]>
    {
        public OrdenCedulacionReporteCommand()
        {

        }
    }

    public class OrdenCedulacionReporteHandler : IRequestHandler<OrdenCedulacionReporteCommand, byte[]>
    {
        readonly string Tipo = "PDF";

        private readonly LocalReport reporte;

        public OrdenCedulacionReporteHandler()
        {
            reporte = new LocalReport();
        }

        public Task<byte[]> Handle(OrdenCedulacionReporteCommand request, CancellationToken cancellationToken)
        {
            ReportParameter[] parametros = GenerarParametros(request);

            using (var fsPlantilla = new FileStream(Path.Combine(Environment.CurrentDirectory, "Shared", "Reports", "OrdenCedulacion.rdlc"), FileMode.Open))
            {
                reporte.LoadReportDefinition(fsPlantilla);
                reporte.SetParameters(parametros);

                byte[] bytes = reporte.Render(Tipo);

                fsPlantilla.Close();

                return Task.FromResult(bytes);

            }
        }


        private ReportParameter[] GenerarParametros(OrdenCedulacionReporteRequest ordenCedulacionReporte)
        {
            return new[]
            {
                new ReportParameter("varUnidadOtorgamientoVisa", ordenCedulacionReporte.UnidadOtorgamientoVisa),
                new ReportParameter("varTipoVisa", ordenCedulacionReporte.TipoVisa),
                new ReportParameter("varNumero", ordenCedulacionReporte.Numero),
                new ReportParameter("varCodigoVerificacion", ordenCedulacionReporte.CodigoVerificacion),
                new ReportParameter("varApellidos",  $"{ordenCedulacionReporte.PrimerApellido} {ordenCedulacionReporte.SegundoApellido}" ),
                new ReportParameter("varNombres",  ordenCedulacionReporte.Nombres),
                new ReportParameter("varNacionalidad", ordenCedulacionReporte.Nacionalidad),
                new ReportParameter("varPaisNacimiento", ordenCedulacionReporte.PaisNacimiento),
                new ReportParameter("varCiudadNacimiento", ordenCedulacionReporte.CiudadNacimiento),
                new ReportParameter("varFechaNacimiento", ordenCedulacionReporte.FechaNacimiento.ToString()),
                new ReportParameter("varSexo",  ordenCedulacionReporte.Sexo),
                new ReportParameter("varEstadoCivil", ordenCedulacionReporte.EstadoCivil),
                new ReportParameter("varApellidosConyuge",  ordenCedulacionReporte.ApellidosConyuge),
                new ReportParameter("varNombresConyuge",  ordenCedulacionReporte.NombresConyuge),
                new ReportParameter("varNacionalidadConyuge",  ordenCedulacionReporte.NacionalidadConyuge),
                new ReportParameter("varCategoriaVisa",  ordenCedulacionReporte.CategoriaVisa),
                new ReportParameter("varNumeroVisa", ordenCedulacionReporte.NumeroVisa),
                new ReportParameter("varFechaOtorgamientoVisa", ordenCedulacionReporte.FechaOtorgamientoVisa.ToString()),
                new ReportParameter("varTiempoVigenciaVisa", ordenCedulacionReporte.TiempoVigenciaVisa.ToString()),
                new ReportParameter("varSignatario", ordenCedulacionReporte.Signatario),
                new ReportParameter("varFotografiaBase64", ordenCedulacionReporte.Fotografia)

            };
        }
    }
}
