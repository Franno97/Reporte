using AutoMapper;
using Mre.Visas.Reporte.Application.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Shared.Handlers
{
  public abstract class BaseHandler
  {
    protected BaseHandler()
    {
    }

    protected BaseHandler(IUnitOfWork unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }
    

    protected BaseHandler(IMapper mapper)
    {
      Mapper = mapper;
    }

    protected BaseHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
      Mapper = mapper;
      UnitOfWork = unitOfWork;
    }

    protected IUnitOfWork UnitOfWork;

    protected IMapper Mapper;

  }
}