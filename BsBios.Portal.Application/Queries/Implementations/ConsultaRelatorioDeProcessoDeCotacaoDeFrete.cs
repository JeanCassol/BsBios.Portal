using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Linq;
using StructureMap;

namespace BsBios.Portal.Application.Queries.Implementations
{

    internal enum FuncaoDeAgrecacao
    {
        Soma,
        Media
    }

    public class ConsultaRelatorioDeProcessoDeCotacaoDeFrete : IConsultaRelatorioDeProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacaoDeFrete _processosDeCotacaoDeFrete;

        public ConsultaRelatorioDeProcessoDeCotacaoDeFrete(IProcessosDeCotacaoDeFrete processosDeCotacaoDeFrete)
        {
            _processosDeCotacaoDeFrete = processosDeCotacaoDeFrete;
        }

        private IQueryable<ProcessoDeCotacao> AplicarFiltros(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            _processosDeCotacaoDeFrete.FiltraPorTipo(Enumeradores.TipoDeCotacao.Frete);

            if (filtro.Status.HasValue)
            {
                var statusDoProcessoDeCotacao  = (Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.Status.Value) );
                _processosDeCotacaoDeFrete.FiltraPorStatus(statusDoProcessoDeCotacao);
            }


            DateTime dataDeValidadeInicial, dataDeValidadeFinal;
            if (DateTime.TryParse(filtro.DataDeValidadeInicial, out dataDeValidadeInicial))
            {
                _processosDeCotacaoDeFrete.DataDeValidadeAPartirDe(dataDeValidadeInicial);

            }
            if (DateTime.TryParse(filtro.DataDeValidadeFinal, out dataDeValidadeFinal))
            {
                _processosDeCotacaoDeFrete.DataDeValidadeAte(dataDeValidadeFinal);
            }

            var escolhaSimples = (Enumeradores.EscolhaSimples) Enum.Parse(typeof(Enumeradores.EscolhaSimples), Convert.ToString(filtro.Classificacao));

            switch (escolhaSimples)
            {
                case Enumeradores.EscolhaSimples.Sim:
                    _processosDeCotacaoDeFrete.SomenteClassificados();
                    break;
                case Enumeradores.EscolhaSimples.Nao:
                    _processosDeCotacaoDeFrete.SomenteNaoClassificados();
                    break;
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoMaterial))
            {
                _processosDeCotacaoDeFrete.DoProduto(filtro.CodigoDoMaterial);
            }

            if (!string.IsNullOrEmpty(filtro.DescricaoDoMaterial))
            {
                _processosDeCotacaoDeFrete.DescricaoDoProdutoContendo(filtro.DescricaoDoMaterial);
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoItinerario))
            {
                _processosDeCotacaoDeFrete.DoItinerario(filtro.CodigoDoItinerario);
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoFornecedorDaMercadoria))
            {
                _processosDeCotacaoDeFrete.DoFornecedorDaMercadoria(filtro.CodigoDoFornecedorDaMercadoria);
            }

            if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
            {
                _processosDeCotacaoDeFrete.NomeDoFornecedorDaMercadoriaContendo(filtro.NomeDoFornecedorDaMercadoria);
            }

            if (filtro.CodigoDaTransportadora == null)
            {
                filtro.CodigoDaTransportadora = "";
            }
            if (filtro.NomeDaTransportadora == null)
            {
                filtro.NomeDaTransportadora = "";
            }

            return _processosDeCotacaoDeFrete.GetQuery();

        }

        public IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IQueryable<ProcessoDeCotacao> queryable = AplicarFiltros(filtro);

            var fornecedoresSelecionados = (Enumeradores.SelecaoDeFornecedores)Enum.Parse(typeof(Enumeradores.SelecaoDeFornecedores), Convert.ToString(filtro.SelecaoDeFornecedores));

            bool cotacaoSelecionada = fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Selecionado;

            return (from processo in queryable
                    from fp in processo.FornecedoresParticipantes
                    let p = processo as ProcessoDeCotacaoDeFrete
                    let transportadora = fp.Fornecedor
                    let cotacao = (CotacaoDeFrete) fp.Cotacao
                    where (fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Todos || cotacao.Selecionada == cotacaoSelecionada)
                    && (string.IsNullOrEmpty(filtro.CodigoDaTransportadora) || fp.Fornecedor.Codigo == filtro.CodigoDaTransportadora) 
                    && (string.IsNullOrEmpty(filtro.NomeDaTransportadora) || fp.Fornecedor.Nome.ToLower().Contains(filtro.NomeDaTransportadora.ToLower()))
                    orderby processo.Id ascending 
                select new RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm
                {
                    Cadencia = cotacao.Cadencia == null ?  p.Cadencia : cotacao.Cadencia.Value,
                    DataDeValidadeInicial = p.DataDeValidadeInicial.ToShortDateString(),
                    DataDeValidadeFinal = p.DataDeValidadeFinal.ToShortDateString(),
                    DataLimiteDeRetorno = p.DataLimiteDeRetorno.Value.ToShortDateString(),
                    Classificacao = p.Classificacao ? "Sim": "Não",
                    CnpjDoFornecedorDaMercadoria = p.FornecedorDaMercadoria != null ? p.FornecedorDaMercadoria.Cnpj : "Não informado",
                    NomeDoFornecedorDaMercadoria = p.FornecedorDaMercadoria != null ? p.FornecedorDaMercadoria.Nome: "Não informado",
                    IdDoProcessoDeCotacao = p.Id,
                    Itinerario = p.Itinerario.Descricao,
                    Material = p.Produto.Descricao,
                    MunicipioDeOrigem = p.MunicipioDeOrigem != null  ? p.MunicipioDeOrigem.Nome + "/" + p.MunicipioDeOrigem.UF : "Não informado",
                    MunicipioDeDestino = p.MunicipioDeDestino != null ? p.MunicipioDeDestino.Nome + "/" + p.MunicipioDeDestino.UF : "Não informado",
                    NomeDoDeposito = p.Deposito != null ? p.Deposito.Nome : "Não informado",
                    NumeroDoContrato = p.NumeroDoContrato,
                    Quantidade = p.Quantidade,
                    QuantidadeDisponivel = cotacao != null ?  cotacao.QuantidadeDisponivel:0,
                    QuantidadeLiberada = cotacao != null && cotacao.QuantidadeAdquirida.HasValue ? cotacao.QuantidadeAdquirida.Value:0,
                    Selecionado = cotacao != null && cotacao.Selecionada ? "Sim": "Não",
                    ValorComImpostos = cotacao != null ? cotacao.ValorComImpostos : 0,
                    Status = Convert.ToString(p.Status),
                    Transportadora = transportadora.Nome,
                    UnidadeDeMedida = p.UnidadeDeMedida.Descricao,
                    Visualizado = "",

                }).ToList();
        }

        private IList<object[]> ConstruirQueryOver(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro, FuncaoDeAgrecacao funcaoDeAgrecacao)
        {
            var unitOfWork = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            IQueryOver<ProcessoDeCotacaoDeFrete, ProcessoDeCotacaoDeFrete> queryOver = unitOfWork.Session.QueryOver<ProcessoDeCotacaoDeFrete>();

            FornecedorParticipante fornecedorParticipante = null;
            Fornecedor transportadora = null;
            Cotacao cotacao = null;
            Produto produto = null;
            UnidadeDeMedida unidadeDeMedida = null;
            Itinerario itinerario = null;

            queryOver
                .JoinAlias(x => x.Produto, () => produto)
                .JoinAlias(x => x.UnidadeDeMedida, () => unidadeDeMedida)
                .JoinAlias(x => x.Itinerario, () => itinerario)
                .JoinAlias(x => x.FornecedoresParticipantes, () => fornecedorParticipante)
                .JoinAlias(x => fornecedorParticipante.Cotacao, () => cotacao)
                .JoinAlias(x => fornecedorParticipante.Fornecedor, () => transportadora);

            if (filtro.Status.HasValue)
            {
                var statusDoProcessoDeCotacao = (Enumeradores.StatusProcessoCotacao)Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.Status.Value));
                queryOver = queryOver.Where(x => x.Status == statusDoProcessoDeCotacao);
            }


            DateTime dataDeValidadeInicial, dataDeValidadeFinal;
            if (DateTime.TryParse(filtro.DataDeValidadeInicial, out dataDeValidadeInicial))
            {
                queryOver = queryOver.Where(x => x.DataDeValidadeFinal >= dataDeValidadeInicial);
            }
            if (DateTime.TryParse(filtro.DataDeValidadeFinal, out dataDeValidadeFinal))
            {
                queryOver = queryOver.Where(x => x.DataDeValidadeInicial <= dataDeValidadeFinal);
            }

            var escolhaSimples = (Enumeradores.EscolhaSimples)Enum.Parse(typeof(Enumeradores.EscolhaSimples), Convert.ToString(filtro.Classificacao));

            switch (escolhaSimples)
            {
                case Enumeradores.EscolhaSimples.Sim:
                    queryOver = queryOver.Where(x => x.Classificacao);
                    break;
                case Enumeradores.EscolhaSimples.Nao:
                    queryOver = queryOver.Where(x => !x.Classificacao);
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
                queryOver = queryOver.Where(x => x.Produto.Codigo == filtro.CodigoDoMaterial);
            }

            if (!string.IsNullOrEmpty(filtro.DescricaoDoMaterial))
            {
                queryOver = queryOver.Where(x => produto.Descricao.ToLower().Contains(filtro.DescricaoDoMaterial.ToLower()));
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
                queryOver = queryOver.Where(x => x.FornecedorDaMercadoria.Nome.ToLower().Contains(filtro.NomeDoFornecedorDaMercadoria.ToLower()));
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDaTransportadora))
            {
                queryOver = queryOver.Where(() => transportadora.Codigo == filtro.CodigoDaTransportadora);
            }
            if (!string.IsNullOrEmpty(filtro.NomeDaTransportadora))
            {
                queryOver = queryOver.Where(Restrictions.On(() => transportadora.Nome)
                    .IsInsensitiveLike(filtro.NomeDaTransportadora, MatchMode.Anywhere));
                //queryOver = queryOver.Where(
                //    Restrictions.Disjunction()
                //    .Add(Restrictions.On(() => transportadora.Nome)
                //    .IsInsensitiveLike(filtro.NomeDaTransportadora, MatchMode.Anywhere)));
            }

            //queryOver.OrderBy(x => x.Status).Asc
            //    .ThenByAlias(() => produto.Descricao).Asc
            //    .ThenByAlias(() => itinerario.Descricao).Asc
            //    .ThenByAlias(() => transportadora.Nome);


            var projectionBuilder = new QueryOverProjectionBuilder<ProcessoDeCotacaoDeFrete>();

                projectionBuilder =
                projectionBuilder.SelectGroup(x => x.Status)
                .SelectGroup(x => produto.Descricao)
                .SelectGroup(x => unidadeDeMedida.Descricao)
                .SelectGroup(x => itinerario.Descricao)
                .SelectGroup(x => x.Classificacao)
                .SelectGroup(x => transportadora.Nome)
                .SelectGroup(x => cotacao.Selecionada);

            switch (funcaoDeAgrecacao)
            {
                case FuncaoDeAgrecacao.Soma:
                    projectionBuilder
                        .SelectSum(x => cotacao.QuantidadeDisponivel)
                        .SelectSum(x => cotacao.QuantidadeAdquirida)
                        .SelectSum(x => cotacao.ValorComImpostos);
                    break;
                case FuncaoDeAgrecacao.Media:
                    //se não ussar este casting no banco para AnsiString, gera erro de overfow em projeções com média quando o nhibernate 
                    //tenta converter para decimal. Por isso não utilizei o método SelectAvg
                    projectionBuilder
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacao.QuantidadeDisponivel)))
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacao.QuantidadeAdquirida)))
                        .Select(Projections.Cast(NHibernateUtil.AnsiString, Projections.Avg(() => cotacao.ValorComImpostos)));
                    break;
            }
            
            return queryOver
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
                QuantidadeDisponivel = properties[7] != null ? Convert.ToDecimal(properties[7]) : 0,
                QuantidadeLiberada = properties[8] != null ? Convert.ToDecimal(properties[8]) : 0,
                ValorComImpostos = properties[9] != null ? Convert.ToDecimal(properties[9]) : 0,

            }).ToList();
            
        }


        public IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComSoma(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IList<object[]> registros = ConstruirQueryOver(filtro, FuncaoDeAgrecacao.Soma);

            return ConstruirResultadoDosRelatoriosSinteticos(registros);

            //.TransformUsing(Transformers.AliasToBean<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm>())
            //.List<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm>();
        }

        public IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComMedia(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IList<object[]> registros = ConstruirQueryOver(filtro, FuncaoDeAgrecacao.Media);

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