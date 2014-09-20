using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
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
                    CodigoDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem != null ? processoDeCotacao.MunicipioDeOrigem.Codigo: null ,
                    NomeDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem != null ? processoDeCotacao.MunicipioDeOrigem.Nome : null,
                    CodigoDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino != null ? processoDeCotacao.MunicipioDeDestino.Codigo : null ,
                    NomeDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino != null ? processoDeCotacao.MunicipioDeDestino.Nome: null,
                    CodigoDoDeposito = processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Codigo: null ,
                    Deposito = processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Nome : null,
                    CodigoDoTerminal = processoDeCotacao.Terminal.Codigo
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

                var cotacao =(CotacaoDeFrete) fornecedorParticipante.Cotacao.CastEntity();

                cotacaoSelecionarVm.IdCotacao = cotacao.Id;
                cotacaoSelecionarVm.QuantidadeAdquirida = cotacao.QuantidadeAdquirida;
                cotacaoSelecionarVm.Cadencia = cotacao.Cadencia;
                cotacaoSelecionarVm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;
                cotacaoSelecionarVm.ValorComImpostos = cotacao.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacao.Selecionada;
                cotacaoSelecionarVm.ObservacaoDoFornecedor = cotacao.Observacoes;
            }

            return retorno;
        }

        public decimal CalcularQuantidadeContratadaNoProcessoDeCotacao(int idDoProcessoDeCotacao)
        {
            //_processosDeCotacao.BuscaPorId(idDoProcessoDeCotacao);
            //var q = (from processo in _processosDeCotacao.GetQuery()
            //         from participante in processo.FornecedoresParticipantes
            //         select Gro
            //             )

            return _processosDeCotacao
                .BuscaPorId(idDoProcessoDeCotacao)
                .GetQuery()
                .Select(x => x.FornecedoresParticipantes.Where(fp => fp.Cotacao.Selecionada).Sum(fp => fp.Cotacao.QuantidadeAdquirida.Value)).SingleOrDefault();



        }
    }
}