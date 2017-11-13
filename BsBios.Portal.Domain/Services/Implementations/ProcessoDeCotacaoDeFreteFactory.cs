using System;
using System.Collections.Generic;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Services.Implementations
{
    internal class ProcessoDeCotacaoDeFreteItemVm
    {
        public Produto Produto { get; set; }
        public decimal Quantidade { get; set; }
        public UnidadeDeMedida UnidadeDeMedida { get; set; }
    }
    public class ProcessoDeCotacaoDeFreteFactory: IProcessoDeCotacaoDeFreteFactory
    {
        private readonly IList<ProcessoDeCotacaoDeFreteItemVm> _itens;
        public ProcessoDeCotacaoDeFreteFactory()
        {
            _itens = new List<ProcessoDeCotacaoDeFreteItemVm>();
            
        }
        public void AdicionarItem(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida)
        {
            _itens.Add(new ProcessoDeCotacaoDeFreteItemVm
                {
                    Produto = produto ,
                    Quantidade = quantidade,
                    UnidadeDeMedida = unidadeDeMedida
                });
        }

        public ProcessoDeCotacaoDeFrete CriarProcesso(string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
            DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedorDaMercadoria, decimal cadencia, bool classificacao,
            Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal, decimal valorPrevisto)
        {
            var processoDeCotacao = new ProcessoDeCotacaoDeFrete(requisitos, numeroDoContrato, dataLimiteDeRetorno, dataDeValidadeInicial, dataDeValidadeFinal, itinerario,
                fornecedorDaMercadoria, cadencia, classificacao, municipioDeOrigem, municipioDeDestino, deposito, terminal, valorPrevisto);
            if (_itens.Count == 0)
            {
                throw new ProcessoDeCotacaoSemItemException();
            }
            foreach (var item in _itens)
            {
                processoDeCotacao.AdicionarItem(item.Produto, item.Quantidade, item.UnidadeDeMedida);
            }
            return processoDeCotacao;
        }
    }
}