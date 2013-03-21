using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class FluxoDeCargaBuilder: Builder<Enumeradores.FluxoDeCarga, FluxoDeCargaVm>
    {
        public override FluxoDeCargaVm BuildSingle(Enumeradores.FluxoDeCarga model)
        {
            return new FluxoDeCargaVm()
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
