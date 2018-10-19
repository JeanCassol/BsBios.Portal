using System.Text;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAtualizadorDeCotacaoDeFrete
    {
        void Atualizar(CotacaoInformarVm cotacaoAtualizarVm);
        void SairDoProcesso(int idDoProcessoDeCotacao, string codigoDoFornecedor);
        void Remover(int idDoProcessoDeCotacao, string codigoDoFornecedor);

    }
}