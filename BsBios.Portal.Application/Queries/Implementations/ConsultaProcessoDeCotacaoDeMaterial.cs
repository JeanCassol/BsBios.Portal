using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
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

        public ProcessoCotacaoMaterialCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial)
        {
             _processosDeCotacao.BuscaPorId(idProcessoCotacaoMaterial);
            var processoDeCotacao = (from p in _processosDeCotacao.GetQuery()
                                     let processo = (ProcessoDeCotacaoDeMaterial) p
                                     select new
                                         {
                                             processo.Id,
                                             DataTerminoLeilao = p.DataLimiteDeRetorno,
                                             processo.Status,
                                             processo.RequisicaoDeCompra.Numero,
                                             processo.RequisicaoDeCompra.NumeroItem,
                                             processo.RequisicaoDeCompra.Centro,
                                             Material = processo.Produto.Descricao,
                                             processo.RequisicaoDeCompra.Descricao,
                                             processo.Quantidade,
                                             processo.RequisicaoDeCompra.DataDeLiberacao,
                                             processo.RequisicaoDeCompra.DataDeRemessa,
                                             processo.RequisicaoDeCompra.DataDeSolicitacao,
                                             FornecedorPretendido = processo.RequisicaoDeCompra.FornecedorPretendido.Nome,
                                             Criador = processo.RequisicaoDeCompra.Criador.Nome, 
                                             processo.RequisicaoDeCompra.Requisitante,
                                             processo.RequisicaoDeCompra.UnidadeMedida
                                         }).Single();

            return new ProcessoCotacaoMaterialCadastroVm()
                {
                    Id = processoDeCotacao.Id,
                    DataTerminoLeilao = processoDeCotacao.DataTerminoLeilao.ToString(),
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    RequisicaoDeCompraVm = new RequisicaoDeCompraVm()
                        {
                            Centro = processoDeCotacao.Centro,
                            Criador = processoDeCotacao.Criador,
                            DataDeLiberacao = processoDeCotacao.DataDeLiberacao.ToShortDateString(),
                            DataDeRemessa = processoDeCotacao.DataDeRemessa.ToShortDateString(),
                            DataDeSolicitacao = processoDeCotacao.DataDeSolicitacao.ToShortDateString(),
                            Descricao = processoDeCotacao.Descricao,
                            FornecedorPretendido = processoDeCotacao.FornecedorPretendido,
                            Material = processoDeCotacao.Material,
                            NumeroItem = processoDeCotacao.NumeroItem,
                            NumeroRequisicao = processoDeCotacao.Numero,
                            Quantidade = processoDeCotacao.Quantidade,
                            Requisitante = processoDeCotacao.Requisitante,
                            UnidadeMedida = processoDeCotacao.UnidadeMedida
                        }
                };
        }
    }
}