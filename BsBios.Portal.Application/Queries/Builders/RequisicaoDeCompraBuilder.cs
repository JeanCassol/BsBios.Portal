using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class RequisicaoDeCompraBuilder:Builder<RequisicaoDeCompra,RequisicaoDeCompraVm>
    {
        public override RequisicaoDeCompraVm BuildSingle(RequisicaoDeCompra model)
        {
            return new RequisicaoDeCompraVm()
                {
                    Id = model.Id,
                    Centro = model.Centro,
                    Criador = model.Criador.Nome,
                    DataDeLiberacao = model.DataDeLiberacao.ToShortDateString(),
                    DataDeRemessa = model.DataDeRemessa.ToShortDateString(),
                    DataDeSolicitacao = model.DataDeSolicitacao.ToShortDateString(),
                    Descricao = model.Descricao,
                    FornecedorPretendido = model.FornecedorPretendido != null ? model.FornecedorPretendido.Nome:"",
                    Material = model.Material.Descricao,
                    NumeroItem = model.NumeroItem,
                    NumeroRequisicao = model.Numero,
                    Quantidade = model.Quantidade,
                    Requisitante = model.Requisitante,
                    UnidadeMedida = model.UnidadeMedida.Descricao
                };
        }
    }
}
