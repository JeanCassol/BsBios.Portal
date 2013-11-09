using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeFrete : IConsultaProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;


        public ConsultaProcessoDeCotacaoDeFrete(IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
        }


        public ProcessoCotacaoFreteCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial)
        {
             
            var processoDeCotacao = (ProcessoDeCotacaoDeFrete)  _processosDeCotacao.BuscaPorId(idProcessoCotacaoMaterial).Single();

            return new ProcessoCotacaoFreteCadastroVm()
                {
                    Id = processoDeCotacao.Id,
                    DataLimiteRetorno = processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString(),
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    CodigoMaterial = processoDeCotacao.Produto.Codigo,
                    DescricaoMaterial = processoDeCotacao.Produto.Descricao,
                    QuantidadeMaterial = processoDeCotacao.Quantidade ,
                    CodigoUnidadeMedida = processoDeCotacao.UnidadeDeMedida.CodigoInterno ,
                    CodigoItinerario = processoDeCotacao.Itinerario.Codigo,
                    DescricaoItinerario = processoDeCotacao.Itinerario.Descricao ,
                    Requisitos = processoDeCotacao.Requisitos ,
                    NumeroDoContrato = processoDeCotacao.NumeroDoContrato ,
                    DataValidadeCotacaoInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString() ,
                    DataValidadeCotacaoFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString(),
                    Cadencia = processoDeCotacao.Cadencia,
                    Classificacao = processoDeCotacao.Classificacao,
                    CodigoDoFornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria != null ? processoDeCotacao.FornecedorDaMercadoria.Codigo: null,
                    FornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria != null ? processoDeCotacao.FornecedorDaMercadoria.Nome: null ,
                    CodigoDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem.Codigo ,
                    NomeDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem.Nome,
                    CodigoDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino.Codigo ,
                    NomeDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino.Nome,
                    CodigoDoDeposito = processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Nome: null ,
                    Deposito = processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Nome : null
                };
        }

        public IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao)
        {
            var retorno = new List<CotacaoSelecionarVm>();
            ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoSelecionarVm { Fornecedor = fornecedorParticipante.Fornecedor.Nome };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = fornecedorParticipante.Cotacao;

                cotacaoSelecionarVm.IdCotacao = cotacao.Id;
                cotacaoSelecionarVm.QuantidadeAdquirida = cotacao.QuantidadeAdquirida;
                cotacaoSelecionarVm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;
                cotacaoSelecionarVm.ValorComImpostos = cotacao.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacao.Selecionada;
                cotacaoSelecionarVm.ObservacaoDoFornecedor = cotacao.Observacoes;
            }

            return retorno;
        }
    }
}