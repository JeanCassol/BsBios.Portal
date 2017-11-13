using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteFactory
    {
        void AdicionarItem(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida);

        ProcessoDeCotacaoDeFrete CriarProcesso(string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno,
                                        DateTime dataDeValidadeInicial,
                                        DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedorDaMercadoria, decimal cadencia, bool classificacao,
            Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal, decimal valorPrevisto);
    }
}