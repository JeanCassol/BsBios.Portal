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
                    CodigoTerminal =  model.Terminal,
                    CodigoFornecedor = model.Fornecedor.Codigo,
                    Data = model.Data,
                    CodigoMaterial =  (int) model.Material,
                    DescricaoMaterial = model.Material.Descricao(),
                    Fluxo = model.FluxoDeCarga.Descricao(),
                    PesoTotal = model.PesoTotal  ,
                    PesoAgendado = model.PesoAgendado ,
                    PesoDisponivel = model.PesoDisponivel 
                };
        }
    }
}
