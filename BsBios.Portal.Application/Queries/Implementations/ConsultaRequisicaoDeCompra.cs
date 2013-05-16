using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using System.Linq;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaRequisicaoDeCompra : IConsultaRequisicaoDeCompra
    {
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;
        private readonly IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> _builder;

        public ConsultaRequisicaoDeCompra(IRequisicoesDeCompra requisicoesDeCompra, IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> builder)
        {
            _requisicoesDeCompra = requisicoesDeCompra;
            _builder = builder;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, RequisicaoDeCompraFiltroVm filtro)
        {
            _requisicoesDeCompra.DisponiveisParaProcessoDeCotacao(filtro.IdProcessoCotacao);
            if (!string.IsNullOrEmpty(filtro.CodigoDoGrupoDeCompras))
            {
                _requisicoesDeCompra.PertencentesAoGrupoDeCompra(filtro.CodigoDoGrupoDeCompras);
            }
            if (filtro.DataDeSolicitacaoInicial.HasValue)
            {
                _requisicoesDeCompra.SolicitadasApartirDe(filtro.DataDeSolicitacaoInicial.Value);
            }
            if (filtro.DataDeSolicitacaoFinal.HasValue)
            {
                _requisicoesDeCompra.SolicitadasAte(filtro.DataDeSolicitacaoFinal.Value);
            }

            return new KendoGridVm
                {
                    QuantidadeDeRegistros = _requisicoesDeCompra.Count() ,
                    Registros = _builder.BuildList(_requisicoesDeCompra.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List()).Cast<ListagemVm>().ToList()
                };
        }
    }
}