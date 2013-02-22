using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacaoDeMaterial:ICompleteRepository<ProcessoDeCotacao>
    {
        ProcessoDeCotacao BuscaPorId(int id);
    }
}
