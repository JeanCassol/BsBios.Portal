using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;
using StructureMap;

namespace BsBios.Portal.Infra.Queries.Implementations
{

    internal enum FuncaoDeAgrecacao
    {
        Soma,
        Media
    }

    internal enum TipoDeRelatorio
    {
        Analitico,
        Sintetico
    }

    public class ConsultaRelatorioDeProcessoDeCotacaoDeFrete : IConsultaRelatorioDeProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacaoDeFrete _processosDeCotacaoDeFrete;

        public ConsultaRelatorioDeProcessoDeCotacaoDeFrete(IProcessosDeCotacaoDeFrete processosDeCotacaoDeFrete)
        {
            _processosDeCotacaoDeFrete = processosDeCotacaoDeFrete;
        }

        //private IQueryable<ProcessoDeCotacao> AplicarFiltros(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        //{
        //    _processosDeCotacaoDeFrete.FiltraPorTipo(Enumeradores.TipoDeCotacao.Frete);

        //    if (filtro.Status.HasValue)
        //    {
        //        var statusDoProcessoDeCotacao  = (Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.Status.Value) );
        //        _processosDeCotacaoDeFrete.FiltraPorStatus(statusDoProcessoDeCotacao);
        //    }


        //    DateTime dataDeValidadeInicial, dataDeValidadeFinal;
        //    if (DateTime.TryParse(filtro.DataDeValidadeInicial, out dataDeValidadeInicial))
        //    {
        //        _processosDeCotacaoDeFrete.DataDeValidadeAPartirDe(dataDeValidadeInicial);

        //    }
        //    if (DateTime.TryParse(filtro.DataDeValidadeFinal, out dataDeValidadeFinal))
        //    {
        //        _processosDeCotacaoDeFrete.DataDeValidadeAte(dataDeValidadeFinal);
        //    }

        //    var escolhaSimples = (Enumeradores.EscolhaSimples) Enum.Parse(typeof(Enumeradores.EscolhaSimples), Convert.ToString(filtro.Classificacao));

        //    switch (escolhaSimples)
        //    {
        //        case Enumeradores.EscolhaSimples.Sim:
        //            _processosDeCotacaoDeFrete.SomenteClassificados();
        //            break;
        //        case Enumeradores.EscolhaSimples.Nao:
        //            _processosDeCotacaoDeFrete.SomenteNaoClassificados();
        //            break;
        //    }

        //    if (!string.IsNullOrEmpty(filtro.CodigoDoMaterial))
        //    {
        //        _processosDeCotacaoDeFrete.DoProduto(filtro.CodigoDoMaterial);
        //    }

        //    if (!string.IsNullOrEmpty(filtro.DescricaoDoMaterial))
        //    {
        //        _processosDeCotacaoDeFrete.DescricaoDoProdutoContendo(filtro.DescricaoDoMaterial);
        //    }

        //    if (!string.IsNullOrEmpty(filtro.CodigoDoItinerario))
        //    {
        //        _processosDeCotacaoDeFrete.DoItinerario(filtro.CodigoDoItinerario);
        //    }

        //    if (!string.IsNullOrEmpty(filtro.CodigoDoFornecedorDaMercadoria))
        //    {
        //        _processosDeCotacaoDeFrete.DoFornecedorDaMercadoria(filtro.CodigoDoFornecedorDaMercadoria);
        //    }

        //    if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
        //    {
        //        _processosDeCotacaoDeFrete.NomeDoFornecedorDaMercadoriaContendo(filtro.NomeDoFornecedorDaMercadoria);
        //    }

        //    if (filtro.CodigoDaTransportadora == null)
        //    {
        //        filtro.CodigoDaTransportadora = "";
        //    }
        //    if (filtro.NomeDaTransportadora == null)
        //    {
        //        filtro.NomeDaTransportadora = "";
        //    }

        //    return _processosDeCotacaoDeFrete.GetQuery();

        //}

        //public IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        //{
        //    IQueryable<ProcessoDeCotacao> queryable = AplicarFiltros(filtro);

        //    var fornecedoresSelecionados = (Enumeradores.SelecaoDeFornecedores)Enum.Parse(typeof(Enumeradores.SelecaoDeFornecedores), Convert.ToString(filtro.SelecaoDeFornecedores));

        //    bool cotacaoSelecionada = fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Selecionado;

        //    return (from processo in queryable
        //            from fp in processo.FornecedoresParticipantes
        //            let p = processo as ProcessoDeCotacaoDeFrete
        //            let transportadora = fp.Fornecedor
        //            let cotacao = (CotacaoDeFrete) fp.Cotacao
        //            where (fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Todos || cotacao.Selecionada == cotacaoSelecionada)
        //            && (string.IsNullOrEmpty(filtro.CodigoDaTransportadora) || fp.Fornecedor.Codigo == filtro.CodigoDaTransportadora) 
        //            && (string.IsNullOrEmpty(filtro.NomeDaTransportadora) || fp.Fornecedor.Nome.ToLower().Contains(filtro.NomeDaTransportadora.ToLower()))
        //            orderby processo.Id ascending 
        //        select new RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm
        //        {
        //            Cadencia = cotacao.Cadencia == null ?  p.Cadencia : cotacao.Cadencia.Value,
        //            DataDeValidadeInicial = p.DataDeValidadeInicial.ToShortDateString(),
        //            DataDeValidadeFinal = p.DataDeValidadeFinal.ToShortDateString(),
        //            DataLimiteDeRetorno = p.DataLimiteDeRetorno.Value.ToShortDateString(),
        //            Classificacao = p.Classificacao ? "Sim": "Não",
        //            CnpjDoFornecedorDaMercadoria = p.FornecedorDaMercadoria != null ? p.FornecedorDaMercadoria.Cnpj : "Não informado",
        //            NomeDoFornecedorDaMercadoria = p.FornecedorDaMercadoria != null ? p.FornecedorDaMercadoria.Nome: "Não informado",
        //            IdDoProcessoDeCotacao = p.Id,
        //            Itinerario = p.Itinerario.Descricao,
        //            Material = p.Produto.Descricao,
        //            MunicipioDeOrigem = p.MunicipioDeOrigem != null  ? p.MunicipioDeOrigem.Nome + "/" + p.MunicipioDeOrigem.UF : "Não informado",
        //            MunicipioDeDestino = p.MunicipioDeDestino != null ? p.MunicipioDeDestino.Nome + "/" + p.MunicipioDeDestino.UF : "Não informado",
        //            NomeDoDeposito = p.Deposito != null ? p.Deposito.Nome : "Não informado",
        //            NumeroDoContrato = p.NumeroDoContrato,
        //            Quantidade = p.Quantidade,
        //            QuantidadeDisponivel = cotacao != null ?  cotacao.QuantidadeDisponivel:0,
        //            QuantidadeLiberada = cotacao != null && cotacao.QuantidadeAdquirida.HasValue ? cotacao.QuantidadeAdquirida.Value:0,
        //            Selecionado = cotacao != null && cotacao.Selecionada ? "Sim": "Não",
        //            ValorComImpostos = cotacao != null ? cotacao.ValorComImpostos : 0,
        //            Status = Convert.ToString(p.Status),
        //            Transportadora = transportadora.Nome,
        //            UnidadeDeMedida = p.UnidadeDeMedida.Descricao,
        //            Visualizado = "",

        //        }).ToList();
        //}


        private IQueryOver<ProcessoDeCotacaoDeFrete, ProcessoDeCotacaoDeFrete> ConstruirFrom(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {

            ProcessoDeCotacaoDeFrete processoDeCotacao = null;
            FornecedorParticipante fornecedorParticipante = null;
            Fornecedor transportadora = null;
            CotacaoDeFrete cotacao = null;
            CotacaoFreteItem cotacaoItem = null;
            ProcessoDeCotacaoDeFreteItem processoCotacaoItem = null;
            Produto produto = null;
            UnidadeDeMedida unidadeDeMedida = null;
            Itinerario itinerario = null;
            Municipio municipioDeOrigem = null;
            Municipio municipioDeDestino = null;
            Fornecedor deposito = null;
            Fornecedor fornecedorDaMercadoria = null;
            Terminal terminal = null;

            var unitOfWork = ObjectFactory.GetInstance<IUnitOfWorkNh>();

            IQueryOver<ProcessoDeCotacaoDeFrete, ProcessoDeCotacaoDeFrete> queryOver = unitOfWork.Session.QueryOver(() => processoDeCotacao);

            queryOver
                .JoinAlias(x => x.Itinerario, () => itinerario)
                .JoinAlias(x => x.Terminal, () => terminal)
                .Left.JoinAlias(x => x.FornecedoresParticipantes, () => fornecedorParticipante)
                .Left.JoinAlias(x => fornecedorParticipante.Cotacao, () => cotacao)
                .Left.JoinAlias(x => cotacao.Itens, () => cotacaoItem)
                .JoinAlias(x => cotacaoItem.ProcessoDeCotacaoItem, () => processoCotacaoItem)
                .JoinAlias(x => processoCotacaoItem.UnidadeDeMedida, () => unidadeDeMedida)
                .Left.JoinAlias(x => fornecedorParticipante.Fornecedor, () => transportadora)
                .Left.JoinAlias(x => x.Deposito, () => deposito)
                .Left.JoinAlias(x => x.FornecedorDaMercadoria,() => fornecedorDaMercadoria)
                .Left.JoinAlias(x => x.MunicipioDeOrigem, () => municipioDeOrigem)
                .Left.JoinAlias(x => x.MunicipioDeDestino, () => municipioDeDestino);

            if (filtro.Status.HasValue)
            {
                var statusDoProcessoDeCotacao = (Enumeradores.StatusProcessoCotacao)Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.Status.Value));
                queryOver = queryOver.Where(x => x.Status == statusDoProcessoDeCotacao);
            }


            DateTime dataDeValidadeInicial, dataDeValidadeFinal, dataDeFechamento;
            if (DateTime.TryParse(filtro.DataDeValidadeInicial, out dataDeValidadeInicial))
            {
                queryOver = queryOver.Where(() => processoDeCotacao.DataDeValidadeFinal >= dataDeValidadeInicial);

            }
            if (DateTime.TryParse(filtro.DataDeValidadeFinal, out dataDeValidadeFinal))
            {
                queryOver = queryOver.Where(() => processoDeCotacao.DataDeValidadeInicial <= dataDeValidadeFinal);
            }

            if (DateTime.TryParse(filtro.DataDeFechamento, out dataDeFechamento))
            {
                //a propriedade data de fechamento é um datetime, mas o filtro é apenas uma data.
                queryOver = queryOver.Where(() => processoDeCotacao.DataDeFechamento >= dataDeFechamento
                    && processoDeCotacao.DataDeFechamento < dataDeFechamento.AddDays(1));
                
            }

            var escolhaSimples = (Enumeradores.EscolhaSimples)Enum.Parse(typeof(Enumeradores.EscolhaSimples), Convert.ToString(filtro.Classificacao));

            switch (escolhaSimples)
            {
                case Enumeradores.EscolhaSimples.Sim:
                    queryOver = queryOver.Where(() => processoDeCotacao.Classificacao);
                    break;
                case Enumeradores.EscolhaSimples.Nao:
                    queryOver = queryOver.Where(() => !processoDeCotacao.Classificacao);
                    break;
            }

            var selecaoDeFornecedores =
                (Enumeradores.SelecaoDeFornecedores)
                    Enum.Parse(typeof(Enumeradores.SelecaoDeFornecedores), Convert.ToString(filtro.SelecaoDeFornecedores));

            if (selecaoDeFornecedores == Enumeradores.SelecaoDeFornecedores.Selecionado)
            {
                queryOver = queryOver.Where(x => cotacao.Selecionada);
            }
            else if (selecaoDeFornecedores == Enumeradores.SelecaoDeFornecedores.NaoSelecionado)
            {
                queryOver = queryOver.Where(x => !cotacao.Selecionada);
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoMaterial))
            {
                //queryOver = queryOver.Where(x => x.Produto.Codigo == filtro.CodigoDoMaterial);
            }

            if (!string.IsNullOrEmpty(filtro.DescricaoDoMaterial))
            {
                queryOver = queryOver.Where(Restrictions.On(() => produto.Descricao)
                .IsInsensitiveLike(filtro.DescricaoDoMaterial, MatchMode.Anywhere));

            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoItinerario))
            {
                queryOver = queryOver.Where(x => x.Itinerario.Codigo == filtro.CodigoDoItinerario);
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoFornecedorDaMercadoria))
            {
                queryOver = queryOver.Where(x => x.FornecedorDaMercadoria.Codigo == filtro.CodigoDoFornecedorDaMercadoria);
            }

            if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
            {
                //queryOver = queryOver.Where(x => x.FornecedorDaMercadoria.Nome.ToLower().Contains(filtro.NomeDoFornecedorDaMercadoria.ToLower()));
                queryOver = queryOver.Where(Restrictions.On(() => fornecedorDaMercadoria.Nome)
                .IsInsensitiveLike(filtro.NomeDoFornecedorDaMercadoria, MatchMode.Anywhere));
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDaTransportadora))
            {
                queryOver = queryOver.Where(() => transportadora.Codigo == filtro.CodigoDaTransportadora);
            }
            if (!string.IsNullOrEmpty(filtro.NomeDaTransportadora))
            {
                queryOver = queryOver.Where(Restrictions.On(() => transportadora.Nome)
                    .IsInsensitiveLike(filtro.NomeDaTransportadora, MatchMode.Anywhere));
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoTerminal))
            {
                queryOver = queryOver.Where(x => x.Terminal.Codigo == filtro.CodigoDoTerminal);
            }

            return queryOver;
            
        }


        private IEnumerable<object[]> ConstruirQueryOverRelatorioSintetico(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro, FuncaoDeAgrecacao funcaoDeAgrecacao)
        {

            ProcessoDeCotacaoDeFrete processoDeCotacao = null;
            Fornecedor transportadora = null;
            CotacaoDeFrete cotacao = null;
            CotacaoFreteItem cotacaoItem = null;
            Produto produto = null;
            UnidadeDeMedida unidadeDeMedida = null;
            Itinerario itinerario = null;
            Terminal terminal = null;

            IQueryOver<ProcessoDeCotacaoDeFrete, ProcessoDeCotacaoDeFrete> queryOver = ConstruirFrom(filtro);

            var projectionBuilder = new QueryOverProjectionBuilder<ProcessoDeCotacaoDeFrete>();

                projectionBuilder =
                projectionBuilder.SelectGroup(x => x.Status)
                .SelectGroup(x => produto.Descricao)
                .SelectGroup(x => unidadeDeMedida.Descricao)
                .SelectGroup(x => itinerario.Descricao)
                .SelectGroup(x => x.Classificacao)
                .SelectGroup(x => transportadora.Nome)
                .SelectGroup(x => cotacao.Selecionada)
                .SelectGroup(x => terminal.Nome);


            switch (funcaoDeAgrecacao)
            {
                case FuncaoDeAgrecacao.Soma:
                    projectionBuilder
                        .SelectSum(x => cotacaoItem.QuantidadeDisponivel)
                        .SelectSum(x => cotacaoItem.QuantidadeAdquirida)
                        .SelectSum(x => cotacaoItem.ValorComImpostos);
                    break;
                case FuncaoDeAgrecacao.Media:
                    //se não ussar este casting no banco para AnsiString, gera erro de overfow em projeções com média quando o nhibernate 
                    //tenta converter para decimal. Por isso não utilizei o método SelectAvg
                    projectionBuilder
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacaoItem.QuantidadeDisponivel)))
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacaoItem.QuantidadeAdquirida)))
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacaoItem.ValorComImpostos)));
                    break;
            }
            
            return queryOver
                .OrderBy(Projections.Property(() => processoDeCotacao.Status)).Asc
                .ThenBy(Projections.Property(() => produto.Descricao)).Asc
                .ThenBy(Projections.Property(() => itinerario.Descricao)).Asc
                .ThenBy(Projections.Property(() => transportadora.Nome)).Asc
            .SelectList(lista => projectionBuilder)
            .List<object[]>();

        }

        private IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ConstruirResultadoDosRelatoriosSinteticos(IEnumerable<object[]> registros)
        {
            return registros.
            Select(properties => new RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm
            {
                Status = ((Enumeradores.StatusProcessoCotacao)properties[0]).Descricao(),
                Material = (string)properties[1],
                UnidadeDeMedida = (string)properties[2],
                Itinerario = (string)properties[3],
                Classificacao = ((bool)properties[4]) ? "Sim" : "Não",
                Transportadora = (string)properties[5],
                Selecionado = properties[6] != null && ((bool)properties[6]) ? "Sim" : "Não",
                Terminal = (string) properties[7],
                QuantidadeDisponivel = properties[8] != null ? Convert.ToDecimal(properties[8]) : 0,
                QuantidadeLiberada = properties[9] != null ? Convert.ToDecimal(properties[9]) : 0,
                ValorComImpostos = properties[10] != null ? Convert.ToDecimal(properties[10]) : 0,

            }).ToList();
            
        }


        public IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm relatorio = null;

            ProcessoDeCotacaoDeFrete processoDeCotacao = null;
            Fornecedor transportadora = null;
            Fornecedor fornecedorDaMercadoria = null;
            Fornecedor deposito = null;
            CotacaoDeFrete cotacao = null;
            CotacaoFreteItem cotacaoItem = null;
            ProcessoDeCotacaoDeFreteItem processoCotacaoItem = null;
            Produto produto = null;
            UnidadeDeMedida unidadeDeMedida = null;
            Itinerario itinerario = null;
            Municipio municipioDeOrigem = null;
            Municipio municipioDeDestino = null;
            Terminal terminal = null;

            IQueryOver<ProcessoDeCotacaoDeFrete, ProcessoDeCotacaoDeFrete> queryOver = ConstruirFrom(filtro);

            var processosDeCotacao = queryOver
                .OrderBy(Projections.Property(() => processoDeCotacao.Id)).Asc
                .SelectList( lista => lista
                    .Select(x => terminal.Nome).WithAlias(() => relatorio.Terminal)
                    .Select(Projections.Conditional(Restrictions.IsNull(Projections.Property(() => cotacao.Cadencia)), 
                    Projections.Property(() => processoDeCotacao.Cadencia), 
                    Projections.Property(() => cotacao.Cadencia)).WithAlias(() => relatorio.Cadencia)).WithAlias(() => relatorio.Cadencia)

                    .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Property(() => processoDeCotacao.DataDeValidadeInicial))).WithAlias(() => relatorio.DataDeValidadeInicial)
                    .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Property(() => processoDeCotacao.DataDeValidadeFinal))).WithAlias(() => relatorio.DataDeValidadeFinal)
                    .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Property(() => processoDeCotacao.DataLimiteDeRetorno))).WithAlias(() => relatorio.DataLimiteDeRetorno)
                    
                    .Select(Projections.Conditional(Restrictions.Eq(Projections.Property(() => processoDeCotacao.Classificacao),true),Projections.Constant("Sim"), Projections.Constant("Não"))).WithAlias(() => relatorio.Classificacao)

                    .Select(x => fornecedorDaMercadoria != null ? fornecedorDaMercadoria.Cnpj : "Não informado").WithAlias(() => relatorio.CnpjDoFornecedorDaMercadoria)
                    .Select(x => fornecedorDaMercadoria != null ? fornecedorDaMercadoria.Nome: "Não informado").WithAlias(() => relatorio.NomeDoFornecedorDaMercadoria)
                    .Select(x => processoDeCotacao.Id).WithAlias(() => relatorio.IdDoProcessoDeCotacao)
                    .Select(x => itinerario.Descricao).WithAlias(() => relatorio.Itinerario)
                    .Select(x => produto.Descricao).WithAlias(() => relatorio.Material)
                    .Select(x => municipioDeOrigem != null ? municipioDeOrigem.Nome + "/" + municipioDeOrigem.UF : "Não informado").WithAlias(() => relatorio.MunicipioDeOrigem)
                    .Select(x => municipioDeDestino != null ? municipioDeDestino.Nome + "/" + municipioDeDestino.UF : "Não informado").WithAlias(() => relatorio.MunicipioDeDestino)
                    .Select(x => deposito != null ? deposito.Nome : "Não informado").WithAlias(() => relatorio.NomeDoDeposito)
                    .Select(x => processoDeCotacao.NumeroDoContrato).WithAlias(() => relatorio.NumeroDoContrato)
                    .Select(x => processoCotacaoItem.Quantidade).WithAlias(() => relatorio.Quantidade)
                    //.Select(x => cotacao != null ? cotacao.QuantidadeDisponivel : 0).WithAlias(() => relatorio.QuantidadeDisponivel)
                    .Select(Projections.Conditional(Restrictions.IsNotNull(Projections.Property(() => cotacaoItem.QuantidadeDisponivel)),
                    Projections.Property(() => cotacaoItem.QuantidadeDisponivel),
                    Projections.Cast(NHibernateUtil.Decimal,Projections.Constant(0)))).WithAlias(() => relatorio.QuantidadeDisponivel)

                    //.Select(x => cotacao != null && cotacao.QuantidadeAdquirida.HasValue ? cotacao.QuantidadeAdquirida.Value:0).WithAlias(() => relatorio.QuantidadeLiberada)
                    .Select(Projections.Conditional(Restrictions.IsNotNull(Projections.Property(() => cotacaoItem.QuantidadeAdquirida)),
                    Projections.Property(() => cotacaoItem.QuantidadeAdquirida),
                    Projections.Cast(NHibernateUtil.Decimal, Projections.Constant(0)))).WithAlias(() => relatorio.QuantidadeLiberada)
                    
                    //.Select(x => cotacao != null && cotacao.Selecionada ? "Sim": "Não").WithAlias(() => relatorio.Selecionado)
                    .Select(Projections.Conditional(Restrictions.Eq(Projections.Property(() => cotacao.Selecionada), true), 
                    Projections.Constant("Sim"), Projections.Constant("Não"))).WithAlias(() => relatorio.Selecionado)

                    //.Select(x => cotacao != null ? cotacao.ValorComImpostos : 0).WithAlias(() => relatorio.ValorComImpostos)
                    .Select(Projections.Conditional(Restrictions.IsNotNull(Projections.Property(() => cotacaoItem.ValorComImpostos)),
                    Projections.Property(() => cotacaoItem.ValorComImpostos),
                    Projections.Cast(NHibernateUtil.Decimal, Projections.Constant(0)))).WithAlias(() => relatorio.ValorComImpostos)
                    
                    .Select(x => processoDeCotacao.Status).WithAlias(() => relatorio.Status)
                    //.Select(Projections.Cast(NHibernateUtil.Int32, Projections.Property(() =>  processoDeCotacao.Status))).WithAlias(() => relatorio.Status)

                    .Select(x => transportadora.Nome).WithAlias(() => relatorio.Transportadora)
                    .Select(x => unidadeDeMedida.Descricao).WithAlias(() => relatorio.UnidadeDeMedida)
                    //.Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Property(() => processoDeCotacao.DataDeFechamento))).WithAlias(() => relatorio.DataHoraDeFechamento)
                    .Select(x => processoDeCotacao.DataDeFechamento).WithAlias(() => relatorio.DataHoraDeFechamento)

                ).TransformUsing(Transformers.AliasToBean<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm>())
                .List<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm>();

            foreach (var processo in processosDeCotacao)
            {
                var status = (Enumeradores.StatusProcessoCotacao)Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(processo.Status));
                processo.DescricaoDoStatus = status.Descricao();
            }

            return processosDeCotacao;

        }

        public IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComSoma(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IEnumerable<object[]> registros = ConstruirQueryOverRelatorioSintetico(filtro, FuncaoDeAgrecacao.Soma);

            return ConstruirResultadoDosRelatoriosSinteticos(registros);

            //.TransformUsing(Transformers.AliasToBean<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm>())
            //.List<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm>();
        }

        public IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComMedia(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IEnumerable<object[]> registros = ConstruirQueryOverRelatorioSintetico(filtro, FuncaoDeAgrecacao.Media);

            return ConstruirResultadoDosRelatoriosSinteticos(registros);


        }


        //public IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComSoma(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        //{
        //    IQueryable<ProcessoDeCotacao> queryable = AplicarFiltros(filtro);

        //    var fornecedoresSelecionados = (Enumeradores.SelecaoDeFornecedores)Enum.Parse(typeof(Enumeradores.SelecaoDeFornecedores), Convert.ToString(filtro.SelecaoDeFornecedores));

        //    bool cotacaoSelecionada = fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Selecionado;

        //    var queryIntermediaria = (from processo in queryable
        //        from fp in processo.FornecedoresParticipantes
        //        where (fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Todos || fp.Cotacao.Selecionada == cotacaoSelecionada)
        //            && (string.IsNullOrEmpty(filtro.CodigoDaTransportadora) || fp.Fornecedor.Codigo == filtro.CodigoDaTransportadora) 
        //            && (string.IsNullOrEmpty(filtro.NomeDaTransportadora) || fp.Fornecedor.Nome.ToLower().Contains(filtro.NomeDaTransportadora.ToLower()))
        //            let p = processo as ProcessoDeCotacaoDeFrete
        //            let transportadora = fp.Fornecedor
        //            select new 
        //            {
        //                Status = p.Status ,
        //                Material = p.Produto.Descricao,
        //                UnidadeDeMedida = p.UnidadeDeMedida.Descricao,
        //                Itinerario = p.Itinerario.Descricao,
        //                Classificacao = p.Classificacao,
        //                Transportadora = fp.Fornecedor.Nome ,
        //                Selecionado = fp.Cotacao != null && fp.Cotacao.Selecionada,
        //                //QuantidadeDisponivel = fp.Cotacao != null ? fp.Cotacao.QuantidadeDisponivel : 0,
        //                //QuantidadeLiberada = fp.Cotacao != null && fp.Cotacao.QuantidadeAdquirida != null ? fp.Cotacao.QuantidadeAdquirida.Value :0 ,
        //                //ValorComImpostos = fp.Cotacao != null ? fp.Cotacao.ValorComImpostos: 0
        //            });

        //    //var teste = queryIntermediaria.ToList();

        //    //OCORRE ERRO AO EXECUTAR
        //    return queryIntermediaria
        //        .GroupBy(
        //            x =>
        //                new
        //                {
        //                    x.Status,
        //                    x.Material,
        //                    x.UnidadeDeMedida,
        //                    x.Itinerario,
        //                    x.Classificacao,
        //                    x.Transportadora,
        //                    x.Selecionado
        //                })
        //        .Select(r => new RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm
        //        {
        //            //Status = (int) r.Key.Status,
        //            Material = r.Key.Material,
        //            UnidadeDeMedida = r.Key.UnidadeDeMedida,
        //            Transportadora = r.Key.Transportadora,
        //            Itinerario = r.Key.Itinerario
        //            //Classificacao = r.Key.Classificacao ? "Sim" : "Não",
        //            //Selecionado = r.Key.Selecionado ? "Sim" : "Não",
        //            //QuantidadeDisponivel = r.Sum( c => c.QuantidadeDisponivel),
        //            //QuantidadeLiberada = r.Sum(c => c.QuantidadeLiberada),
        //            //ValorComImpostos = r.Sum(c => c.ValorComImpostos)
        //        }).ToList();


        //    //OCORRE ERRO AO EXECUTAR

        //    //return (from processo in queryable
        //    //        from fp in processo.FornecedoresParticipantes
        //    //        let p = processo as ProcessoDeCotacaoDeFrete
        //    //        let transportadora = fp.Fornecedor
        //    //        let cotacao = (CotacaoDeFrete) fp.Cotacao
        //    //        where (fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Todos || cotacao.Selecionada == cotacaoSelecionada)
        //    //        && (string.IsNullOrEmpty(filtro.CodigoDaTransportadora) || fp.Fornecedor.Codigo == filtro.CodigoDaTransportadora) 
        //    //        && (string.IsNullOrEmpty(filtro.NomeDaTransportadora) || fp.Fornecedor.Nome.ToLower().Contains(filtro.NomeDaTransportadora.ToLower()))
        //    //        group p by new {p.Status, Material = p.Produto.Descricao, UnidadeDeMedida = p.UnidadeDeMedida.Descricao, Itinerario = p.Itinerario.Descricao, 
        //    //            Transportadora = transportadora.Nome,p.Classificacao, Selecionado = cotacao.Selecionada} into agrupamento
        //    //        select new RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm
        //    //        {
        //    //            Status = Convert.ToString(agrupamento.Key.Status) ,
        //    //            Material = agrupamento.Key.Material,
        //    //            UnidadeDeMedida = agrupamento.Key.UnidadeDeMedida,
        //    //            Itinerario = agrupamento.Key.Itinerario,
        //    //            Classificacao = agrupamento.Key.Classificacao ? "Sim" : "Não",
        //    //            Transportadora = agrupamento.Key.Transportadora ,
        //    //            Selecionado = agrupamento.Key.Selecionado ? "Sim" : "Não",
        //    //            QuantidadeDisponivel = agrupamento.Sum( p => p.FornecedoresParticipantes.Sum( fp => fp.Cotacao != null ? fp.Cotacao.QuantidadeDisponivel : 0))
        //    //        }).ToList();
        //}

    }


}