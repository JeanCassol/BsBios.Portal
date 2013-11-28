using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacaoDeFrete: IProcessosDeCotacao
    {
        IProcessosDeCotacaoDeFrete NomeDoFornecedorDaMercadoriaContendo(string nomeDoFornecedorDaMercadoria);
        IProcessosDeCotacaoDeFrete PertencentesAoContratoDeNumero(string numeroDoContrato);
        IProcessosDeCotacaoDeFrete ComOrigemNoMunicipio(string codigoDoMunicipioDeOrigem);
    }
}