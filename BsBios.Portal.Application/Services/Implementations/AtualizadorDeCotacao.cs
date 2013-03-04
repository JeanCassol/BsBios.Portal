using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacao : IAtualizadorDeCotacao
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IIvas _ivas;
        private readonly IIncoterms _incoterms;
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public AtualizadorDeCotacao(IProcessosDeCotacao processosDeCotacao, IIvas ivas, 
            IIncoterms incoterms, ICondicoesDePagamento condicoesDePagamento)
        {
            _processosDeCotacao = processosDeCotacao;
            _ivas = ivas;
            _incoterms = incoterms;
            _condicoesDePagamento = condicoesDePagamento;
        }

        public void Atualizar(CotacaoAtualizarVm cotacaoAtualizarVm)
        {
            throw new System.NotImplementedException();
        }
    }
}