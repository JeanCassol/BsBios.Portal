using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CadastroCondicaoPagamentoOperacao : ICadastroCondicaoPagamentoOperacao
    {
        public CondicaoDePagamento Criar(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        {
            var condicaoDePagamento = new CondicaoDePagamento(condicaoDePagamentoCadastroVm.Codigo,
                                                              condicaoDePagamentoCadastroVm.Descricao);
            return condicaoDePagamento;

        }

        public void Alterar(CondicaoDePagamento condicaoDePagamento, CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm)
        {
            condicaoDePagamento.AtualizarDescricao(condicaoDePagamentoCadastroVm.Descricao);
        }
    }
}