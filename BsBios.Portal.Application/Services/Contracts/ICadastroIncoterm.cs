using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroIncoterm
    {
        void AtualizarIncoterms(IList<IncotermCadastroVm> incoterms);
    }
}