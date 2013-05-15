using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class TipoDeFreteBuilder: Builder<Enumeradores.TipoDeFrete, TipoDeFreteVm>
    {
        public override TipoDeFreteVm BuildSingle(Enumeradores.TipoDeFrete model)
        {
            return new TipoDeFreteVm()
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
