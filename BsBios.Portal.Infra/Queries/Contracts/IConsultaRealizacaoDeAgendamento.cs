using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaMaterialDeCarga
    {
        IList<MaterialDeCargaVm> Listar();
    }
}