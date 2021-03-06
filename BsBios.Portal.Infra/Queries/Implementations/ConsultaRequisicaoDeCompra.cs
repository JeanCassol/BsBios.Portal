using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaRequisicaoDeCompra : IConsultaRequisicaoDeCompra
    {
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> _builder;

        public ConsultaRequisicaoDeCompra(IRequisicoesDeCompra requisicoesDeCompra, IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> builder, 
            IProcessosDeCotacao processosDeCotacao)
        {
            _requisicoesDeCompra = requisicoesDeCompra;
            _builder = builder;
            _processosDeCotacao = processosDeCotacao;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, RequisicaoDeCompraFiltroVm filtro)
        {
            _requisicoesDeCompra.SomenteAtivas();
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

            _processosDeCotacao.BuscaPorId(filtro.IdProcessoCotacao);

            var queryItens = (from pc in _processosDeCotacao.GetQuery()
                              from item in pc.Itens
                              let itemMaterial = (ProcessoDeCotacaoDeMaterialItem)  item
                              select itemMaterial.RequisicaoDeCompra.Id
                             );

            var query = (from rc in _requisicoesDeCompra.GetQuery()
                         where !rc.GerouProcessoDeCotacao || queryItens.Contains(rc.Id)
                         select rc
                        );

            return new KendoGridVm
                {
                    QuantidadeDeRegistros = query.Count() ,
                    Registros = _builder.BuildList(query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList()).Cast<ListagemVm>().ToList()
                };
        }

        public IList<RequisicaoDeCompraVm> RequisicoesDoGrupoDeCompras(string codigoDoGrupoDeCompras)
        {
            _requisicoesDeCompra.PertencentesAoGrupoDeCompra(codigoDoGrupoDeCompras);
            return _builder.BuildList(_requisicoesDeCompra.List());
        }
    }
}