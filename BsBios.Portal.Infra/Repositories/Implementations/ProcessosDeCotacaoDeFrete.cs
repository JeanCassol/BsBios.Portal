using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacaoDeFrete : ProcessosDeCotacao, IProcessosDeCotacaoDeFrete
    {
        public ProcessosDeCotacaoDeFrete(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IProcessosDeCotacaoDeFrete NomeDoFornecedorDaMercadoriaContendo(string nomeDoFornecedorDaMercadoria)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).FornecedorDaMercadoria.Nome.ToLower().Contains(nomeDoFornecedorDaMercadoria.ToLower()));
            return this;
        }

        public IProcessosDeCotacaoDeFrete PertencentesAoContratoDeNumero(string numeroDoContrato)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).NumeroDoContrato == numeroDoContrato);
            return this;
        }

        public IProcessosDeCotacaoDeFrete ComOrigemNoMunicipio(string codigoDoMunicipioDeOrigem)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete)x).MunicipioDeOrigem.Codigo == codigoDoMunicipioDeOrigem);
            return this;
        }
    }
}