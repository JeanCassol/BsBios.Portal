using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaCondicaoPagamento: IConsultaCondicaoPagamento
    {
        private readonly ICondicoesDePagamento _condicoesDePagamento;

        public ConsultaCondicaoPagamento(ICondicoesDePagamento condicoesDePagamento)
        {
            _condicoesDePagamento = condicoesDePagamento;
        }

        public IList<CondicaoDePagamentoCadastroVm> Listar(PaginacaoVm paginacaoVm, CondicaoDePagamentoCadastroVm filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Codigo))
            {
                _condicoesDePagamento.BuscaPeloCodigoSap(filtro.Codigo);

            }

            if (!string.IsNullOrEmpty(filtro.Descricao))
            {
                _condicoesDePagamento.FiltraPelaDescricao(filtro.Descricao);
            }
            int skip = (paginacaoVm.Page - 1) * paginacaoVm.PageSize;

            //paginacaoVm.TotalRecords = _condicoesDePagamento.Count();

            var builder = new CondicaoPagamentoCadastroBuilder();
            return builder.BuildList(_condicoesDePagamento.Skip(skip).Take(paginacaoVm.Take).List());
            
        }
    }
}
