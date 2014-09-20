using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Transform;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaRelatorioDeOrdemDeTransporte : IConsultaRelatorioDeOrdemDeTransporte
    {

        private readonly ISession _session;

        public ConsultaRelatorioDeOrdemDeTransporte(IUnitOfWorkNh unitOfWorkNh)
        {
            _session = unitOfWorkNh.Session;
        }

    //    public IList<RelatorioDeOrdemDeTransporteAnaliticoVm> ListagemAnalitica(RelatorioDeOrdemDeTransporteFiltroVm filtro)
    //    {

    //        OrdemDeTransporte ordemDeTransporte = null;
    //        ProcessoDeCotacaoDeFrete processoDeCotacao = null;
    //        Produto produto = null;
    //        Fornecedor fornecedorDaMercadoria = null;
    //        Fornecedor transportadora = null;
    //        Coleta coleta = null;
    //        RelatorioDeOrdemDeTransporteAnaliticoVm resultado = null;

    //        IQueryOver<OrdemDeTransporte, OrdemDeTransporte> queryOrdemDeTransporte = _session.QueryOver(() =>  ordemDeTransporte);

    //        queryOrdemDeTransporte
    //            .JoinAlias(x => x.ProcessoDeCotacaoDeFrete, () => processoDeCotacao)
    //            .JoinAlias(x => processoDeCotacao.Produto, () => produto)
    //            .JoinAlias(x => processoDeCotacao.FornecedorDaMercadoria, () => fornecedorDaMercadoria)
    //            .JoinAlias(x => x.Fornecedor, () => transportadora);

    //        if (filtro.Status.HasValue)
    //        {
    //            var statusDaOrdemDeTransporte = (Enumeradores.StatusDaOrdemDeTransporte)Enum.Parse(typeof(Enumeradores.StatusDaOrdemDeTransporte), Convert.ToString(filtro.Status.Value));

    //            switch (statusDaOrdemDeTransporte)
    //            {
    //                case Enumeradores.StatusDaOrdemDeTransporte.Pendente:
    //                    queryOrdemDeTransporte = queryOrdemDeTransporte.Where(x => x.QuantidadeRealizada < x.QuantidadeLiberada);
    //                    break;
    //                case Enumeradores.StatusDaOrdemDeTransporte.Concluido:
    //                    queryOrdemDeTransporte = queryOrdemDeTransporte.Where(x => x.QuantidadeRealizada >= x.QuantidadeLiberada);
    //                    break;
    //            }

    //        }

    //        DateTime dataDeValidadeInicial, dataDeValidadeFinal;
    //        if (DateTime.TryParse(filtro.DataDeValidadeInicial, out dataDeValidadeInicial))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => processoDeCotacao.DataDeValidadeFinal >= dataDeValidadeInicial);
    //        }
    //        if (DateTime.TryParse(filtro.DataDeValidadeFinal, out dataDeValidadeFinal))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => processoDeCotacao.DataDeValidadeInicial <= dataDeValidadeFinal);
    //        }

    //        if (!string.IsNullOrEmpty(filtro.CodigoDoMaterial))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => processoDeCotacao.Produto.Codigo == filtro.CodigoDoMaterial);
    //        }

    //        if (!string.IsNullOrEmpty(filtro.DescricaoDoMaterial))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => produto.Descricao.ToLower().Contains(filtro.DescricaoDoMaterial.ToLower()));
    //        }

    //        if (!string.IsNullOrEmpty(filtro.CodigoDoFornecedorDaMercadoria))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => processoDeCotacao.FornecedorDaMercadoria.Codigo == filtro.CodigoDoFornecedorDaMercadoria);
    //        }

    //        if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(() => fornecedorDaMercadoria.Nome.ToLower().Contains(filtro.NomeDoFornecedorDaMercadoria.ToLower()));
    //        }

    //        if (!string.IsNullOrEmpty(filtro.CodigoDaTransportadora))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(x => x.Fornecedor.Codigo == filtro.CodigoDaTransportadora);
    //        }
    //        if (!string.IsNullOrEmpty(filtro.NomeDaTransportadora))
    //        {
    //            queryOrdemDeTransporte = queryOrdemDeTransporte.Where(x => x.Fornecedor.Nome.ToLower().Contains(filtro.CodigoDaTransportadora));
    //        }

    //        //QueryOver<Coleta, Coleta> subQueryColeta = QueryOver.Of<Coleta>();
    //        //subQueryColeta.Where(c => c.Realizado)
    //        //    .And(c => c.OrdemDeTransporte == ordemDeTransporte)
    //        //    .SelectList(lista => lista
    //        //        .SelectCount(c => c.Id)
    //        //        .SelectSum(c => c.Peso));

    //        OrdemSumarizada ordemSumarizada = null;

    //        QueryOver<Coleta, Coleta> subQueryColeta = QueryOver.Of<Coleta>(() => coleta);
            
    //        subQueryColeta.Where(c => c.Realizado)
    //            .And(c => c.OrdemDeTransporte.Id == ordemDeTransporte.Id)

    //            .SelectList(lista => lista
    //                .SelectGroup(c => c.Id).WithAlias(() => ordemSumarizada.Id)
    //                .SelectCount(c => c.Id).WithAlias(()=> ordemSumarizada.Contador)
    //                .SelectSum(c => c.Peso).WithAlias(() => ordemSumarizada.Total))
    //                .TransformUsing(Transformers.AliasToBean<OrdemSumarizada>());

    //        //queryOrdemDeTransporte.Left.JoinQueryOver(() => subQueryColeta,);

    //        return queryOrdemDeTransporte.SelectList(lista => lista
    //            .Select(x => x.Id).WithAlias(() => resultado.IdDaOrdemDeTransporte)
    //            .SelectSubQuery(subQueryColeta.SelectList(listaColeta => listaColeta
    //                .SelectCount(c => coleta.Id).WithAlias(() => resultado.QuantidadeDeColetasRealizadas)
    //                .SelectSum(c => coleta.Peso).WithAlias(() => resultado.QuantidadeLiberada)
    //                )))
    //            .TransformUsing(Transformers.AliasToBean<RelatorioDeOrdemDeTransporteAnaliticoVm>())
    //            .TransformUsing(Transformers.DistinctRootEntity)
    //            .List<RelatorioDeOrdemDeTransporteAnaliticoVm>();

    //    }

        public IList<RelatorioDeOrdemDeTransporteAnaliticoVm> ListagemAnalitica(RelatorioDeOrdemDeTransporteFiltroVm filtro)
        {
            var itens = _session.GetNamedQuery("RELATORDEMTRANSPORTEANALITICO")
                .SetParameter("p_status", filtro.Status)
                .SetParameter("p_dataDeValidadeInicial", filtro.DataDeValidadeInicial)
                .SetParameter("p_dataDeValidadeFinal", filtro.DataDeValidadeFinal)
                .SetParameter("p_codigoMaterial", filtro.CodigoDoMaterial)
                .SetParameter("p_material", filtro.DescricaoDoMaterial)
                .SetParameter("p_codigoFornecedorDaMercadoria", filtro.CodigoDoFornecedorDaMercadoria)
                .SetParameter("p_fornecedorDaMercadoria", filtro.NomeDoFornecedorDaMercadoria)
                .SetParameter("p_codigoTransportadora", filtro.CodigoDaTransportadora)
                .SetParameter("p_transportadora", filtro.NomeDaTransportadora)
                .SetParameter("p_codigoTerminal", filtro.CodigoTerminal)

                .SetResultTransformer(Transformers.AliasToBean<RelatorioDeOrdemDeTransporteAnaliticoVm>())
                .List<RelatorioDeOrdemDeTransporteAnaliticoVm>();

            return itens;
        }

        public IList<RelatorioDeOrdemDeTransporteSinteticoVm> ListagemSintetica(RelatorioDeOrdemDeTransporteFiltroVm filtro)
        {
            throw new NotImplementedException();
        }
    }

    //public class OrdemSumarizada
    //{
    //    public int Id  { get; set; }
    //    public int Contador { get; set; }

    //    public int Total { get; set; }
    //}
}