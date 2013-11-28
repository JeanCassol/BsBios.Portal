using System;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class OrdensDeTransporte : CompleteRepositoryNh<OrdemDeTransporte>, IOrdensDeTransporte
    {
        public OrdensDeTransporte(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }
        public IOrdensDeTransporte BuscaPorId(int id)
        {
            Query = Query.Where(ordem => ordem.Id == id);
            return this;
        }

        public IOrdensDeTransporte AutorizadasParaATransportadora(string codigoDaTransportadora)
        {
            Query = Query.Where(x => x.Fornecedor.Codigo == codigoDaTransportadora);
            return this;
        }

        public IOrdensDeTransporte CodigoDaTransportadoraContendo(string codigoDaTransportadora)
        {
            if (!string.IsNullOrEmpty(codigoDaTransportadora))
            {
                Query = Query.Where(x => x.Fornecedor.Codigo.ToLower().Contains(codigoDaTransportadora.ToLower()));
            }
            return this;
        }

        public IOrdensDeTransporte NomeDaTransportadoraContendo(string nomeDaTransportadora)
        {
            if (!string.IsNullOrEmpty(nomeDaTransportadora))
            {
                Query = Query.Where(x => x.Fornecedor.Nome.ToLower().Contains(nomeDaTransportadora.ToLower()));

            }
            return this;
        }

        public IOrdensDeTransporte NomeDoFornecedorDaMercadoriaContendo(string nomeDoFornecedorDaMercadoria)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.FornecedorDaMercadoria.Nome.ToLower().Contains(nomeDoFornecedorDaMercadoria.ToLower()));
            return this;
        }

        public IOrdensDeTransporte ComNumeroDeContrato(string numeroDoContrato)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.NumeroDoContrato == numeroDoContrato);
            return this;
        }

        public IOrdensDeTransporte ComOrigemNoMunicipio(string codigoDoMunicipio)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.MunicipioDeOrigem.Codigo == codigoDoMunicipio);
            return this;
        }
    }
}