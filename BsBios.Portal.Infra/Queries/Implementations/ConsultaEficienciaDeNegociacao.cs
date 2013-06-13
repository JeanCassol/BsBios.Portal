using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaEficienciaDeNegociacao : IConsultaEficienciaDeNegociacao
    {
        private readonly IProcessosDeCotacaoDeMaterial _processosDeCotacaoDeMaterial;

        public ConsultaEficienciaDeNegociacao(IProcessosDeCotacaoDeMaterial processosDeCotacaoDeMaterial)
        {
            _processosDeCotacaoDeMaterial = processosDeCotacaoDeMaterial;
        }

        public KendoGridVm ConsultarResumo(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro)
        {
            IQueryOver<ProcessoDeCotacao, ProcessoDeCotacao> queryOver = _processosDeCotacaoDeMaterial.GetQueryOver();

            queryOver = queryOver
                .Where(x => x.GetType() == typeof(ProcessoDeCotacaoDeMaterial))
                .And(x => x.Status == Enumeradores.StatusProcessoCotacao.Fechado);

            Usuario compradorAlias = null;
            queryOver = queryOver
                .JoinAlias(x => x.Comprador, () => compradorAlias);


            if (!string.IsNullOrEmpty(filtro.LoginComprador))
            {
                queryOver = queryOver.Where(() => compradorAlias.Login == filtro.LoginComprador);
            }
            if (filtro.DataDeFechamentoInicial.HasValue)
            {
                queryOver = queryOver.Where(x => x.DataDeFechamento >= filtro.DataDeFechamentoInicial.Value);
            }
            if (filtro.DataDeFechamentoFinal.HasValue)
            {
                queryOver = queryOver.Where(x => x.DataDeFechamento <= filtro.DataDeFechamentoFinal.Value);
            }

            ProcessoDeCotacaoDeMaterialItem processoDeCotacaoItem = null;

            queryOver = queryOver
                .JoinAlias(x => x.Itens, () => processoDeCotacaoItem);

            //para contabilizar os registros a query deve ser feita antes de fazer junção com as outras tabelas, para evitar repetições.
            //Precisa ter junção apenas com os itens do processo de cotação
            var contadorDeRegistros = queryOver.ToRowCountQuery().SingleOrDefault<int>();

            EficienciaDeNegociacaoResumoVm eficienciaDeNegociacaoResumoVm = null;

            FornecedorParticipante fornecedorParticipante = null;
            Cotacao cotacao = null;
            CotacaoMaterialItem cotacaoItem = null;
            Produto produto = null;
            RequisicaoDeCompra requisicaoDeCompra = null;

            queryOver = queryOver
                .JoinAlias(() => processoDeCotacaoItem.Produto, () => produto)
                .JoinAlias(() => processoDeCotacaoItem.RequisicaoDeCompra, () => requisicaoDeCompra)
                .JoinAlias(x => x.FornecedoresParticipantes, () => fornecedorParticipante)
                .JoinAlias(() => fornecedorParticipante.Cotacao, () => cotacao)
                .JoinAlias(() => cotacao.Itens, () => cotacaoItem)
                .Where(() => cotacaoItem.Selecionada);

            //Para calcular o percentual e o valor de eficiência tive que utilizar SQLProjection porque o método SelectSum() 
            // não entende uma expressão lambda, como por exemplo, "() => (PrecoInicial - Preco) * QuantidadeAdquirida)".
            //O método espera uma expressao lambda com o nome de uma propriedade que leve, para uma coluna do mapeamento.
            IProjection projecaoValorDeEficiencia =
                Projections.SqlProjection("SUM((PrecoInicial - Preco) * QuantidadeAdquirida) AS ValorDeEficiencia",
                                          new[] {"ValorDeEficiencia"}, new IType[] {NHibernateUtil.Decimal});

            IProjection projecaoPercentualDeEficiencia =
                Projections.SqlProjection(
                    "ROUND(SUM((PrecoInicial - Preco) * QuantidadeAdquirida) / SUM(PrecoInicial * QuantidadeAdquirida) * 100, 2) AS PercentualDeEficiencia",
                    new[] {"PercentualDeEficiencia"}, new IType[] {NHibernateUtil.Decimal});


            IList<EficienciaDeNegociacaoResumoVm> eficiencias = queryOver
                .SelectList(list => list
                                        .SelectGroup(x => x.Id).WithAlias(() => eficienciaDeNegociacaoResumoVm.IdProcessoCotacao)
                                        .SelectGroup(() => processoDeCotacaoItem.Id).WithAlias(() => eficienciaDeNegociacaoResumoVm.IdProcessoCotacaoItem)
                                        .SelectGroup(x => compradorAlias.Nome).WithAlias(() => eficienciaDeNegociacaoResumoVm.Comprador)
                                        .SelectGroup(() => produto.Descricao).WithAlias(() => eficienciaDeNegociacaoResumoVm.Produto)
                                        .SelectGroup(() => requisicaoDeCompra.Numero).WithAlias(() => eficienciaDeNegociacaoResumoVm.NumeroDaRequisicao)
                                        .SelectGroup(() => requisicaoDeCompra.NumeroItem).WithAlias(() => eficienciaDeNegociacaoResumoVm.NumeroDoItem)
                                        .Select(projecaoValorDeEficiencia)
                                        .Select(projecaoPercentualDeEficiencia)
                            )
                            .TransformUsing(Transformers.AliasToBean<EficienciaDeNegociacaoResumoVm>())
                            .Skip(paginacaoVm.Skip)
                            .Take(paginacaoVm.Take)
                            .List<EficienciaDeNegociacaoResumoVm>();

            var eficienciaTotal = queryOver
                .SelectList(list => list
                                        .Select(Projections.Constant("TOTAL")).WithAlias(() => eficienciaDeNegociacaoResumoVm.Produto)
                                        .Select(projecaoValorDeEficiencia)
                                        .Select(projecaoPercentualDeEficiencia)
                )
                .TransformUsing(Transformers.AliasToBean<EficienciaDeNegociacaoResumoVm>())
                .SingleOrDefault<EficienciaDeNegociacaoResumoVm>();

            eficiencias.Add(eficienciaTotal);

            return new KendoGridVm
                {
                    QuantidadeDeRegistros = contadorDeRegistros + 1, //soma 1 por causa do registro de total
                    Registros = eficiencias.Cast<ListagemVm>().ToList()
                };
        }

        //Comentei o método porque não consegui fazer a consulta com IQueryable. O Linq to NHibernate não permite fazer
        // group new {classes}  by {campos} quando classes é uma lista de classes. Só permite fazer se for uma única classe

        //public IList<EficienciaDeNegociacaoResumoVm> Consultar(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro)
        //{
        //    _processosDeCotacaoDeMaterial.Fechado();
        //    if (!string.IsNullOrEmpty(filtro.LoginComprador))
        //    {
        //        _processosDeCotacaoDeMaterial.EfetuadosPeloComprador(filtro.LoginComprador);
        //    }
        //    if (filtro.DataDeFechamentoInicial.HasValue)
        //    {
        //        _processosDeCotacaoDeMaterial.FechadosAPartirDe(filtro.DataDeFechamentoInicial.Value);

        //    }

        //    if (filtro.DataDeFechamentoFinal.HasValue)
        //    {
        //        _processosDeCotacaoDeMaterial.FechadosAte(filtro.DataDeFechamentoFinal.Value);
        //    }


        //    return (from pc in _processosDeCotacaoDeMaterial.GetQuery()
        //             from processoItem in pc.Itens
        //             from fp in pc.FornecedoresParticipantes
        //             from cotacaoItem in fp.Cotacao.Itens 
        //             let processoItemMaterial = (ProcessoDeCotacaoDeMaterialItem) processoItem
        //             where cotacaoItem.Selecionada 
        //             group new{pc, cotacaoItem, processoItemMaterial} by new{pc.Comprador.Nome,cotacaoItem.ProcessoDeCotacaoItem.Produto.Descricao,
        //             processoItemMaterial.RequisicaoDeCompra.Numero,processoItemMaterial.RequisicaoDeCompra.NumeroItem} into grupoPorProcesso
        //             select new EficienciaDeNegociacaoResumoVm
        //                 {
        //                     Comprador = grupoPorProcesso.Key.Nome,
        //                     Produto = grupoPorProcesso.Key.Descricao,
        //                     NumeroDaRequisicao = grupoPorProcesso.Key.Numero,
        //                     NumeroDoItem = grupoPorProcesso.Key.NumeroItem,
        //                     ValorDeEficiencia = grupoPorProcesso.Sum(x => (x.cotacaoItem.PrecoInicial - x.cotacaoItem.Preco)  * (x.cotacaoItem.QuantidadeAdquirida.HasValue ? x.cotacaoItem.QuantidadeAdquirida.Value : 0)),
        //                     //PercentualDeEficiencia = grupoPorProcesso.Sum(x => x.cotacaoItem.PrecoInicial - x.cotacaoItem.Preco) 
        //                     // grupoPorProcesso.Sum(x => x.cotacaoItem.PrecoInicial)  
        //                 })
        //                 .Skip(paginacaoVm.Skip)
        //                 .Take(paginacaoVm.Take)
        //                 .ToList();
        //}

    }
}