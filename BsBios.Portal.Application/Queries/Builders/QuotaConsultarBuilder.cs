using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class QuotaConsultarBuilder: Builder<Quota, QuotaConsultarVm>
    {
        public override QuotaConsultarVm BuildSingle(Quota model)
        {
            return new QuotaConsultarVm
                {
                    CodigoTransportadora = model.Transportadora.Codigo ,
                    NomeTransportadora = model.Transportadora.Nome,
                    FluxoDeCarga = (int) model.FluxoDeCarga,
                    DescricaoFluxoCarga = model.FluxoDeCarga.Descricao() ,
                    Peso = model.Peso
                };
        }
    }
}
