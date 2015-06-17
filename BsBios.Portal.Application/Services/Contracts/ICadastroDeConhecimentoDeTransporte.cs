using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroDeConhecimentoDeTransporte
    {
        void Salvar(IList<ConhecimentoDeTransporteVm> conhecimentosDeTransporte);
        void Reprocessar();
        void Reprocessar(string chaveEletronica);
        
    }
}
