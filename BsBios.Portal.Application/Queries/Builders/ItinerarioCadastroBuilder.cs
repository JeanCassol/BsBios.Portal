using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class ItinerarioCadastroBuilder : Builder<Itinerario, ItinerarioCadastroVm>
    {
        public override ItinerarioCadastroVm BuildSingle(Itinerario model)
        {
            return new ItinerarioCadastroVm()
                {
                    Codigo = model.Codigo ,
                    Descricao = model.Descricao,
                };
        }

    }
}
