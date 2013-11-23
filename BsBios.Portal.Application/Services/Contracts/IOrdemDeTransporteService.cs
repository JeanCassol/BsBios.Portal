using System.CodeDom.Compiler;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IOrdemDeTransporteService
    {
        void AtualizarOrdemDeTransporte(OrdemDeTransporteAtualizarDTO ordemDeTransporteAtualizarDTO);
        decimal SalvarColeta(ColetaSalvarVm coletaSalvarVm);
        decimal RemoverColeta(int idDaOrdemDeTransporte, int idDaColeta);
        void RealizarColeta(int idDaOrdemDeTransporte, int idDaColeta);
    }
}