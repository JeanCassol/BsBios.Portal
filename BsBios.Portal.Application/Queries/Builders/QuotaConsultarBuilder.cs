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
                    CodigoFornecedor = model.Fornecedor.Codigo ,
                    NomeFornecedor = model.Fornecedor.Nome,
                    CodigoMaterial = (int) model.Material ,
                    DescricaoMaterial = model.Material.Descricao(),
                    CodigoFluxoCarga = (int) model.FluxoDeCarga,
                    DescricaoFluxoCarga = model.FluxoDeCarga.Descricao() ,
                    Peso = model.PesoTotal
                };
        }
    }
}
