using System;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacaoDeFrete: IProcessosDeCotacao
    {
        IProcessosDeCotacaoDeFrete DoFornecedorDaMercadoria(string codigoDoFornecedor);
        IProcessosDeCotacaoDeFrete NomeDoFornecedorDaMercadoriaContendo(string nomeDoFornecedorDaMercadoria);
        IProcessosDeCotacaoDeFrete PertencentesAoContratoDeNumero(string numeroDoContrato);
        IProcessosDeCotacaoDeFrete ComOrigemNoMunicipio(string codigoDoMunicipioDeOrigem);
        //IProcessosDeCotacao DataDeValidadeNoPeriodo(string dataDeValidadeInicial, string dataDeValidadeFinal);
        IProcessosDeCotacaoDeFrete DataDeValidadeAPartirDe(DateTime data);
        IProcessosDeCotacaoDeFrete DataDeValidadeAte(DateTime data);
        IProcessosDeCotacaoDeFrete SomenteClassificados();
        IProcessosDeCotacaoDeFrete SomenteNaoClassificados();
        IProcessosDeCotacaoDeFrete DoItinerario(string codigoDoItinerario);
        IProcessosDeCotacaoDeFrete DoTerminal(string codigoDoTerminal);
    }
}