using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.OrdenCedulacion
{
    public class OrdenCedulacionProfile : Profile
    {
        public OrdenCedulacionProfile()
        {

            CreateMap<OrdenCedulacionReporteRequest, OrdenCedulacionReporteCommand>();
        }
    }
}
