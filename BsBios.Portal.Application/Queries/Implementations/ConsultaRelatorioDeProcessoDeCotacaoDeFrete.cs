using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
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

            if (filtro.StatusDoProcessoDeCotacao.HasValue)
            {
                var statusDoProcessoDeCotacao  = (Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.StatusDoProcessoDeCotacao.Value) );
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


            return _processosDeCotacaoDeFrete.GetQuery();

        }

        public IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IQueryable<ProcessoDeCotacao> queryable = AplicarFiltros(filtro);

            var fornecedoresSelecionados = (Enumeradores.SelecaoDeFornecedores)Enum.Parse(typeof(Enumeradores.SelecaoDeFornecedores), Convert.ToString(filtro.SelecaoDeFornecedores));

            bool cotacaoSelecionada = fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Selecionado;

            if (filtro.CodigoDaTransportadora ==  null)
            {
                filtro.CodigoDaTransportadora = "";
            }
            if (filtro.NomeDaTransportadora == null)
            {
                filtro.NomeDaTransportadora = "";
            }

            return (from processo in queryable
                    from fp in processo.FornecedoresParticipantes
                    let p = processo as ProcessoDeCotacaoDeFrete
                    let transportadora = fp.Fornecedor
                    let cotacao = (CotacaoDeFrete) fp.Cotacao
                    where (fornecedoresSelecionados == Enumeradores.SelecaoDeFornecedores.Todos || cotacao.Selecionada == cotacaoSelecionada)
                    && (string.IsNullOrEmpty(filtro.CodigoDaTransportadora) || fp.Fornecedor.Codigo == filtro.CodigoDaTransportadora) 
                    && (string.IsNullOrEmpty(filtro.NomeDaTransportadora) || fp.Fornecedor.Nome.ToLower().Contains(filtro.NomeDaTransportadora.ToLower()))
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
    }
}