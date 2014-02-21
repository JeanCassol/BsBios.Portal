using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class AgendamentoDeCarregamentoFactory: IAgendamentoDeCargaFactory
    {
        private readonly string _motorista;
        private readonly string _destino;
        private readonly decimal _peso;

        public AgendamentoDeCarregamentoFactory(string motorista, string destino, decimal peso)
        {
            _motorista = motorista;
            _destino = destino;
            _peso = peso;
        }

        public AgendamentoDeCarga Construir(Quota quota, string placa)
        {
            return new AgendamentoDeCarregamento(quota, placa,_motorista,_destino, _peso);
        }

        public void AdicionarNotaFiscal(NotaFiscalVm notaFiscalVm)
        {
            throw new AgendamentoDeCarregamentoComNotaFiscalException();
        }

    }
}
