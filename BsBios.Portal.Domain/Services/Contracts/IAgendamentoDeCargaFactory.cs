using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IAgendamentoDeCargaFactory
    {
        AgendamentoDeCarga Construir(DateTime data, string placa);
        //void InformarPeso(decimal peso);
        //void AdicionarNota(string numero, string serie, DateTime dataDeEmissao,string nomeDoEmitente, string cnpjDoEmitente, 
        //    string nomeDoContratante, string cnpjDoContratante, string numeroDoContrato, decimal valor, decimal peso);
        void AdicionarNotaFiscal(NotaFiscalVm notaFiscalVm);
    }
}