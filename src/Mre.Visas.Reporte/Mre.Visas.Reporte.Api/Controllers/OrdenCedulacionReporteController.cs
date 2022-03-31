using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Mre.Visas.Reporte.Application.OrdenCedulacion;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenCedulacionReporteController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public OrdenCedulacionReporteController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GenerarAsync(OrdenCedulacionReporteRequest request)
        {
            var ordenCedulacionReporteCommand = mapper.Map<OrdenCedulacionReporteCommand>(request);

            var resultado = await mediator.Send(ordenCedulacionReporteCommand);

            var fileName = $"OrdenCedulacionReporte_{request.Numero}.pdf";
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

            return  File(resultado, contentType, fileName);
        }
    
    }
}
