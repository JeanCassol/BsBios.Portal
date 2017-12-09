using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class QuotaConsultarBuilder: Builder<Quota, QuotaConsultarVm>
    {
        public override QuotaConsultarVm BuildSingle(Quota model)
        {
            return new QuotaConsultarVm
                {
                    IdQuota =  model.Id,
                    CodigoFornecedor = model.Fornecedor.Codigo ,
                    NomeFornecedor = model.Fornecedor.Nome,
                    CodigoMaterial = model.Material.Codigo ,
                    DescricaoMaterial = model.Material.Descricao,
                    CodigoFluxoCarga = (int) model.FluxoDeCarga,
                    DescricaoFluxoCarga = model.FluxoDeCarga.Descricao() ,
                    Peso = model.PesoTotal,
                    PesoAgendado = model.PesoAgendado
                };
        }
    }
}
