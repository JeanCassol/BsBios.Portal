using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class QuotaPorFornecedorBuilder:Builder<Quota, QuotaPorFornecedorVm>
    {
        public override QuotaPorFornecedorVm BuildSingle(Quota model)
        {
            return new QuotaPorFornecedorVm
                {
                    IdQuota = model.Id,
                    //CodigoTerminal =  model.CodigoTerminal,
                    //CodigoFornecedor = model.Fornecedor.Codigo,
                    Data = model.Data.ToShortDateString(),
                    //CodigoMaterial =  (int) model.Material,
                    DescricaoMaterial = model.Material.Descricao(),
                    CodigoFluxoDeCarga = (int) model.FluxoDeCarga,
                    FluxoDeCarga = model.FluxoDeCarga.Descricao(),
                    PesoTotal = model.PesoTotal  ,
                    PesoAgendado = model.PesoAgendado ,
                    PesoDisponivel = model.PesoDisponivel 
                };
        }
    }
}
