using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                throw new Exception("É necessário informar pelo menos uma nota fiscal");
            }
            var agendamento = new AgendamentoDeDescarregamento(data, placa);

            foreach (var notaFiscalVm in _notasFiscais)
            {
                agendamento.AdicionarNotaFiscal(notaFiscalVm);
            }

            return agendamento;
        }

        public void InformarPeso(decimal peso)
        {
            throw new Exception("O peso deve ser informado em cada uma das notas do agendamento.");
        }

        public void AdicionarNota(NotaFiscalVm notaFiscalVm)
        {
            _notasFiscais.Add(notaFiscalVm);    
        }
    }
}
