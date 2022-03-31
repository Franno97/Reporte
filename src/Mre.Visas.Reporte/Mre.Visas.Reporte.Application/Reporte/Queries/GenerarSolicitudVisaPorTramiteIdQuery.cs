//using FluentValidation;
//using MediatR;
//using Mre.Visas.Reporte.Application.Reporte.Requests;
//using Mre.Visas.Reporte.Application.Shared.Handlers;
//using Mre.Visas.Reporte.Application.Shared.Interfaces;
//using Mre.Visas.Reporte.Application.Shared.Wrappers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Mre.Visas.Reporte.Application.Reporte.Queries
//{
//  public class GenerarSolicitudVisaPorTramiteIdQuery : GenerarSolicitudVisaPorTramiteIdRequest, IRequest<ApiResponseWrapper>
//  {
//    public GenerarSolicitudVisaPorTramiteIdQuery(GenerarSolicitudVisaPorTramiteIdRequest request)
//    {
//      Id = request.Id;
//    }

//    public class GenerarPDFPorTramiteIdQueryHandler : BaseHandler, IRequestHandler<GenerarSolicitudVisaPorTramiteIdQuery, ApiResponseWrapper>
//    {
//      public GenerarPDFPorTramiteIdQueryHandler(IUnitOfWork unitOfWork)
//          : base(unitOfWork)
//      {
//      }

//      public async Task<ApiResponseWrapper> Handle(GenerarSolicitudVisaPorTramiteIdQuery query, CancellationToken cancellationToken)
//      {
//        try
//        {
//          //var tramite = await UnitOfWork.TramiteRepository.GetByIdTramiteCompleto(query.Id);
//          return new ApiResponseWrapper(HttpStatusCode.OK, null);
//        }
//        catch (System.Exception ex)
//        {
//          return new ApiResponseWrapper(HttpStatusCode.BadRequest, ex.Message);
//        }


//      }
//    }
//  }

//  public class GenerarPDFPorTramiteIdQueryValidator : AbstractValidator<GenerarSolicitudVisaPorTramiteIdQuery>
//  {
//    public GenerarPDFPorTramiteIdQueryValidator()
//    {
//      RuleFor(e => e.Id)
//          .NotEmpty().WithMessage("{PropertyName} es requerdio.")
//          .NotNull().WithMessage("{PropertyName} no puede ser nulo.");
//    }
//  }
//}