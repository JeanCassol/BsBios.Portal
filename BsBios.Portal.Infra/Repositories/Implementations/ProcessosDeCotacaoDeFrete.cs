using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacaoDeFrete : ProcessosDeCotacao, IProcessosDeCotacaoDeFrete
    {
        public ProcessosDeCotacaoDeFrete(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IProcessosDeCotacaoDeFrete DoFornecedorDaMercadoria(string codigoDoFornecedor)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).FornecedorDaMercadoria.Codigo == codigoDoFornecedor);
            return this;
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

        public IProcessosDeCotacaoDeFrete DataDeValidadeAPartirDe(DateTime data)
        {
            Query = Query.Where(p => data <= ((ProcessoDeCotacaoDeFrete) p).DataDeValidadeFinal);
            return this;
        }

        public IProcessosDeCotacaoDeFrete DataDeValidadeAte(DateTime data)
        {
            Query = Query.Where(p => data >= ((ProcessoDeCotacaoDeFrete)p).DataDeValidadeInicial);
            return this;
        }

        public IProcessosDeCotacaoDeFrete SomenteClassificados()
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).Classificacao);
            return this;
        }

        public IProcessosDeCotacaoDeFrete SomenteNaoClassificados()
        {
            Query = Query.Where(x => !((ProcessoDeCotacaoDeFrete)x).Classificacao);
            return this;
        }

        public IProcessosDeCotacaoDeFrete DoItinerario(string codigoDoItinerario)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).Itinerario.Codigo == codigoDoItinerario);

            return this;
        }

        public IProcessosDeCotacaoDeFrete DoTerminal(string codigoDoTerminal)
        {
            Query = Query.Where(x => ((ProcessoDeCotacaoDeFrete) x).Terminal.Codigo == codigoDoTerminal);
            return this;
        }

        //public IProcessosDeCotacao DataDeValidadeNoPeriodo(string dataDeValidadeInicial, string dataDeValidadeFinal)
        //{
        //    Expression<Func<ProcessoDeCotacaoDeFrete, bool>> predicadoDataInicial = null, predicadoDataFinal = null, predicado = null;

        //    DateTime dataInicial, dataFinal;
            
        //    if (DateTime.TryParse(dataDeValidadeInicial, out dataInicial))
        //    {
        //        predicadoDataInicial = processo => processo.DataDeValidadeInicial >= dataInicial && processo.DataDeValidadeFinal <= dataInicial ;
        //    }

        //    if (DateTime.TryParse(dataDeValidadeFinal, out dataFinal))
        //    {
        //        predicadoDataFinal = processo => processo.DataDeValidadeFinal >= dataFinal && processo.DataDeValidadeFinal <= dataFinal;
        //    }

        //    if (predicadoDataInicial != null &&  predicadoDataFinal != null)
        //    {
        //        var body = Expression.OrElse(predicadoDataInicial.Body, predicadoDataFinal.Body);
        //        predicado = Expression.Lambda<Func<ProcessoDeCotacaoDeFrete, bool>>(body, predicadoDataInicial.Parameters[0], predicadoDataFinal.Parameters[0]);
        //    }
        //    else if (predicadoDataInicial != null)
        //    {
        //        predicado = predicadoDataInicial;
        //    }
        //    else if (predicadoDataFinal != null)
        //    {
        //        predicado = predicadoDataFinal;
        //    }

        //    if (predicado != null)
        //    {
        //        Query = Query.Where(predicado);
        //    }

        //}

    }
}