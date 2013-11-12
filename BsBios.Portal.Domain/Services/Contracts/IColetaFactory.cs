using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IColetaFactory
    {
        Coleta Construir(ColetaSalvarVm coletaSalvarVm, decimal valorUnitario);
    }
}