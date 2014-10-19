using System.Collections.Generic;
using System.Threading.Tasks;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroDeConhecimentoDeTransporte
    {
        void Salvar(IList<ConhecimentoDeTransporteVm> conhecimentosDeTransporte);
    }
}
