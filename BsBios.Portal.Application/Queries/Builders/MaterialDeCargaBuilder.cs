using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class MaterialDeCargaBuilder : Builder<MaterialDeCarga, MaterialDeCargaVm>
    {
        public override MaterialDeCargaVm BuildSingle(MaterialDeCarga model)
        {
            return new MaterialDeCargaVm
                {
                    Codigo =  model.Codigo,
                    Descricao = model.Descricao
                };
        }
    }
}
