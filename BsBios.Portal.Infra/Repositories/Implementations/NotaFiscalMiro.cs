using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class NotasFiscaisMiro: CompleteRepositoryNh<NotaFiscalMiro>, INotasFiscaisMiro
    {
        public NotasFiscaisMiro(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }
    }
}
