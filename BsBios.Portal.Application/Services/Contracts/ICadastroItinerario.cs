using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroItinerario
    {
        void AtualizarItinerarios(IList<ItinerarioCadastroVm> itinerarios);
    }
}