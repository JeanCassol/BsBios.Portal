using System;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteFactory
    {
        void AdicionarItem(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida);

        ProcessoDeCotacaoDeFrete CriarProcesso(string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno,
                                        DateTime dataDeValidadeInicial,
                                        DateTime dataDeValidadeFinal, Itinerario itinerario);
    }
}