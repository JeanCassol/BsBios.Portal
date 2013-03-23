using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class AgendamentoDeCarregamentoFactory: IAgendamentoDeCargaFactory
    {
        private readonly decimal _peso;

        public AgendamentoDeCarregamentoFactory(decimal peso)
        {
            _peso = peso;
        }

        public AgendamentoDeCarga Construir(DateTime data, string placa)
        {
            return new AgendamentoDeCarregamento(data, placa, _peso);
        }

        public void AdicionarNotaFiscal(NotaFiscalVm notaFiscalVm)
        {
            throw new AgendamentoDeCarregamentoComNotaFiscalException();
        }

        //public void AdicionarNota(string numero, string serie, DateTime dataDeEmissao, string nomeDoEmitente, string cnpjDoEmitente,
        //                          string nomeDoContratante, string cnpjDoContratante, string numeroDoContrato, decimal valor,
        //                          decimal peso)
        //{
        //    throw new Exception("Não é possível adicionar notas para agendamentos de carregamento");
        //}
    }
}
