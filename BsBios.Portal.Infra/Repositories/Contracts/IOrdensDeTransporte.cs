using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
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
    }
}