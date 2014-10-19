using System;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IOrdensDeTransporte: ICompleteRepository<OrdemDeTransporte>
    {
        IOrdensDeTransporte BuscaPorId(int id);
        IOrdensDeTransporte AutorizadasParaATransportadora(string codigoDaTransportadora);
        IOrdensDeTransporte CodigoDaTransportadoraContendo(string codigoDaTransportadora);
        IOrdensDeTransporte NomeDaTransportadoraContendo(string nomeDaTransportadora);
        IOrdensDeTransporte NomeDoFornecedorDaMercadoriaContendo(string nomeDoFornecedorDaMercadoria);
        IOrdensDeTransporte ComNumeroDeContrato(string numeroDoContrato);
        IOrdensDeTransporte ComOrigemNoMunicipio(string codigoDoMunicipio);
        IOrdensDeTransporte DoTerminal(string codigoDoTerminal);
        IOrdensDeTransporte ComPeriodoDeValidadeContendoAData(DateTime data);
        IOrdensDeTransporte ComColetaAberta();
        IOrdensDeTransporte DoFornecedorDaMercadoria(string cnpjDoFornecedor);
        IOrdensDeTransporte DaTransportadora(string cnpjDaTransportadora);
        IOrdensDeTransporte IncluirProcessoDeCotacao();
        IOrdensDeTransporte ContendoNotaFiscalDeColeta(string numero, string serie);
    }
}