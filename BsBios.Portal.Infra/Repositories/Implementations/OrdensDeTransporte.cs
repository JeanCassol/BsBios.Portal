using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using NHibernate.Linq;

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

        public IOrdensDeTransporte DoTerminal(string codigoDoTerminal)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.Terminal.Codigo == codigoDoTerminal);
            return this;
        }

        public IOrdensDeTransporte ComPeriodoDeValidadeContendoAData(DateTime data)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.DataDeValidadeInicial <= data
                                     && x.ProcessoDeCotacaoDeFrete.DataDeValidadeFinal >= data);

            return this;
        }

        public IOrdensDeTransporte ComColetaAberta()
        {
            Query = Query.Where(x => x.StatusParaColeta == Enumeradores.StatusParaColeta.Aberto);
            return this;
        }

        public IOrdensDeTransporte DoFornecedorDaMercadoria(string cnpjDoFornecedor)
        {
            Query = Query.Where(x => x.ProcessoDeCotacaoDeFrete.FornecedorDaMercadoria.Cnpj == cnpjDoFornecedor);
            return this;
        }

        public IOrdensDeTransporte DaTransportadora(string cnpjDaTransportadora)
        {
            Query = Query.Where(x => x.Fornecedor.Cnpj == cnpjDaTransportadora);
            return this;
        }

        public IOrdensDeTransporte IncluirProcessoDeCotacao()
        {
            Query = Query.Fetch(x => x.ProcessoDeCotacaoDeFrete);
            return this;
        }

        public IOrdensDeTransporte ContendoNotaFiscalDeColeta(string numero, string serie)
        {
            Query = Query
                .Where(x => x.Coletas.Any(c => c.NotasFiscais.Any(nf => nf.Numero == numero && nf.Serie == serie)));

            return this;
        }
    }
}