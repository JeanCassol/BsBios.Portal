using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeMaterial : IConsultaProcessoDeCotacaoDeMaterial
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ConsultaProcessoDeCotacaoDeMaterial(IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoMaterialFiltroVm filtro)
        {
            var query = (from p in _processosDeCotacao.GetQuery()
                         select new 
                         {
                             CodigoMaterial = p.Produto.Codigo,
                             Material = p.Produto.Descricao,
                             DataTermino = p.DataLimiteDeRetorno.ToString(),
                             Id = p.Id,
                             Quantidade = p.Quantidade,
                             Status = p.Status,
                             UnidadeDeMedida = "UND"
                         }
                        );

            var quantidadeDeRegistros = query.Count();

            var registros = query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList()
                             .Select(x => new ProcessoCotacaoMaterialListagemVm()
                                 {
                                     Id = x.Id,
                                     CodigoMaterial = x.CodigoMaterial,
                                     Material = x.Material,
                                     DataTermino = x.DataTermino,
                                     Quantidade = x.Quantidade,
                                     Status = x.Status.Descricao(),
                                     UnidadeDeMedida = x.UnidadeDeMedida
                                 }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = quantidadeDeRegistros,
                    Registros = registros
                };

            return kendoGridVm;

        }
    }
}