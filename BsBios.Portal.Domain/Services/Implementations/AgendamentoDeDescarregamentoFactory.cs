using System;
using System.Collections.Generic;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class AgendamentoDeDescarregamentoFactory: IAgendamentoDeCargaFactory
    {
        private readonly IList<NotaFiscalVm> _notasFiscais = new List<NotaFiscalVm>(); 
        public AgendamentoDeCarga Construir(DateTime data, string placa)
        {
            if (_notasFiscais.Count == 0)
            {
                throw new AgendamentoDeDescarregamentoSemNotaFiscalException();
            }
            var agendamento = new AgendamentoDeDescarregamento(data, placa);

            foreach (var notaFiscalVm in _notasFiscais)
            {
                agendamento.AdicionarNotaFiscal(notaFiscalVm);
            }

            return agendamento;
        }

        public void AdicionarNotaFiscal(NotaFiscalVm notaFiscalVm)
        {
            _notasFiscais.Add(notaFiscalVm);    
        }
    }
}
