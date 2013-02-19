using BsBios.Portal.Domain.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICadastroCondicaoPagamentoOperacao
    {
        CondicaoDePagamento Criar(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm);
        void Alterar(CondicaoDePagamento condicaoDePagamento, CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm);
    }
}