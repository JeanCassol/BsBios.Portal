using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeFrete : IConsultaProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builder;


        public ConsultaProcessoDeCotacaoDeFrete(IProcessosDeCotacao processosDeCotacao, IBuilder<Fornecedor, FornecedorCadastroVm> builder)
        {
            _processosDeCotacao = processosDeCotacao;
            _builder = builder;
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
                    DataValidadeCotacaoFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString() ,
                };
        }

    }
}