using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroUnidadeDeMedida
    {
        void AtualizarUnidadesDeMedida(IList<UnidadeDeMedidaCadastroVm> unidadesDeMedida);
    }
}