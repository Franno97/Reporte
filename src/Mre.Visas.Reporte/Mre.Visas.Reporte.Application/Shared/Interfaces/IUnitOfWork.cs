using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mre.Visas.Reporte.Application.Shared.Interfaces
{
  public interface IUnitOfWork
  {
    //ITramiteRepository TramiteRepository { get; }

    Task<(bool, string)> SaveChangesAsync();
  }
}