using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate.Transform;
using StructureMap;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaMonitorDeOrdemDeTransporte : IConsultaMonitorDeOrdemDeTransporte
    {
        private IList<MonitorDeOrdemDeTransporteListagemVm> Listar(MonitorDeOrdemDeTransporteConfiguracaoVm filtro)
        {
            var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            var session = unitOfWorkNh.Session;
            var itens = session.GetNamedQuery("MONITORDEORDEMDETRANSPORTE")
                .SetParameter("p_agrupamentos", filtro.Agrupamentos)
                .SetParameter("p_codigoMaterial", filtro.CodigoDoMaterial)
                .SetParameter("p_material", filtro.DescricaoDoMaterial)
                .SetParameter("p_codigoFornecedorDaMercadoria", filtro.CodigoDoFornecedorDaMercadoria)
                .SetParameter("p_fornecedorDaMercadoria", filtro.NomeDoFornecedorDaMercadoria)
                .SetParameter("p_codigoTransportadora", filtro.CodigoDaTransportadora)
                .SetParameter("p_transportadora", filtro.NomeDaTransportadora)
                .SetParameter("p_codigoDoMunicipioDeOrigem", filtro.CodigoDoMunicipioDeOrigem)
                .SetParameter("p_codigoDoMunicipioDeDestino", filtro.CodigoDoMunicipioDeDestino)

                .SetResultTransformer(Transformers.AliasToBean<MonitorDeOrdemDeTransporteListagemVm>())
                .List<MonitorDeOrdemDeTransporteListagemVm>();

            return itens;
        }

        //public IList<MonitorDeOrdemDeTransporteVm> ListagemDoMonitor2(MonitorDeOrdemDeTransporteFiltroVm filtro)
        //{
        //    var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        //    ISession session = unitOfWorkNh.Session;

        //    //OrdemDeTransporte ordemDeTransporte = null;
        //    Fornecedor fornecedor = null;
        //    ProcessoDeCotacaoDeFrete processoDeCotacao = null;
        //    Produto produto = null;
        //    //Coleta coleta = null;

        //    //var subQueryChegadaNoDia = QueryOver.Of<Coleta>()
        //    //    .Where(c => c.DataDePrevisaoDeChegada == DateTime.Today)
        //    //    .SelectList(list => list.SelectSubQuery())

        //    Expression<Func<OrdemDeTransporte, bool>> whereDoPeriodo =
        //        ordem => processoDeCotacao.DataDeValidadeFinal >= filtro.DataInicial
        //                 && processoDeCotacao.DataDeValidadeInicial <= filtro.DataFinal;

        //    MonitorDeOrdemDeTransporteVm registro = null;

        //    //parâmetro columnAliases não funciona. Tenho que colocar o AS "" junto com a expressão SQL.
        //    var projecaoDeQuantidadeEmTransito = Projections.SqlProjection("SUM(QuantidadeColetada - QuantidadeRealizada) AS \"QuantidadeEmTransito\" ",
        //        new[] { "QuantidadeEmTransito" }, new IType[] { NHibernateUtil.Decimal });

        //    var projecaoDeQuantidadePendente = Projections.SqlProjection("SUM(QuantidadeLiberada - QuantidadeColetada) AS \"QuantidadePendente\" ",
        //        new[] { "QuantidadePendente" }, new IType[] { NHibernateUtil.Decimal });

        //    var queryQuantidadeLiberada = session.QueryOver<OrdemDeTransporte>(/*() => ordemDeTransporte*/)
        //        .Where(whereDoPeriodo)
        //        .JoinAlias(ordem => ordem.Fornecedor, () => fornecedor)
        //        .JoinAlias(ordem => ordem.ProcessoDeCotacaoDeFrete, () => processoDeCotacao)
        //        .JoinAlias(ordem => processoDeCotacao.Produto, () => produto)
        //        //.JoinAlias(ordem => ordem.Coletas, () => coleta)
        //        .SelectList(list => list
        //            .SelectGroup(g => fornecedor.Nome).WithAlias(() => registro.Fornecedor)
        //            .SelectGroup(g => produto.Descricao).WithAlias(() => registro.Material)
        //            .SelectSum(g => g.QuantidadeLiberada).WithAlias(() => registro.QuantidadeLiberada)
        //            .Select(projecaoDeQuantidadeEmTransito).WithAlias(() => registro.QuantidadeEmTransito)
        //            .Select(projecaoDeQuantidadePendente).WithAlias(() => registro.QuantidadePendente)
        //            .SelectSubQuery(
        //                QueryOver.Of<Coleta>()
        //                .Where(c => c.DataDePrevisaoDeChegada == DateTime.Today /*&& c.OrdemDeTransporte.Id == ordemDeTransporte.Id*/)
        //            //select subquery tem problema porque não consigo ligar o id da ordem de transporte da query principal com o id da ordem de transporte da coleta,
        //            //já que não uso relacionamento bidirecional.
        //            //.And(Restrictions.EqProperty("OrdemDeTransporte.Id","Coleta.IdOrdemTransporte"))
        //                .SelectList(lista => lista.SelectSum(m => m.Peso))
        //            ).WithAlias(() => registro.PrevisaoDeChegadaNoDia)
        //        )

        //            .TransformUsing(Transformers.AliasToBean<MonitorDeOrdemDeTransporteVm>())
        //        //.TransformUsing(Transformers.DistinctRootEntity)
        //            .List<MonitorDeOrdemDeTransporteVm>();



        //    return queryQuantidadeLiberada;

        //}

        public IList<MonitorDeOrdemDeTransportePorMaterialVm> ListarPorMaterial(MonitorDeOrdemDeTransporteConfiguracaoVm filtro)
        {
            var listagem = Listar(filtro);

            return listagem.GroupBy(l => l.Material).Select(m => new MonitorDeOrdemDeTransportePorMaterialVm
            {
                Material = m.Key,
                Registros = m.Select(o => new MonitorDeOrdemDeTransporteVm
                {
                    FornecedorDaMercadoria = o.FornecedorDaMercadoria,
                    NumeroDoContrato = o.NumeroDoContrato,
                    NumeroDaOrdemDeTransporte = o.NumeroDaOrdemDeTransporte,
                    Transportadora = o.Transportadora,
                    MunicipioDeOrigem = o.MunicipioDeOrigem,
                    MunicipioDeDestino = o.MunicipioDeDestino,
                    QuantidadeLiberada = o.QuantidadeLiberada,
                    QuantidadeRealizada = o.QuantidadeRealizada,
                    QuantidadeEmTransito = o.QuantidadeEmTransito,
                    PrevisaoDeChegadaNoDia = o.PrevisaoDeChegadaNoDia ,
                    QuantidadePendente = o.QuantidadePendente ,
                    PercentualPendente = o.PercentualPendente,
                    PercentualProjetado = o.PercentualProjetado
                }).ToList()
            }).ToList();
        }
    }
}