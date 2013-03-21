using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class MaterialDeCargaBuilder : Builder<Enumeradores.MaterialDeCarga, MaterialDeCargaVm>
    {
        public override MaterialDeCargaVm BuildSingle(Enumeradores.MaterialDeCarga model)
        {
            return new MaterialDeCargaVm
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
