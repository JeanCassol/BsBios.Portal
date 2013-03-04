using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
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
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builder;  


        public ConsultaProcessoDeCotacaoDeMaterial(IProcessosDeCotacao processosDeCotacao, IBuilder<Fornecedor, FornecedorCadastroVm> builder)
        {
            _processosDeCotacao = processosDeCotacao;
            _builder = builder;
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
                                             CodigoMaterial = processo.Produto.Codigo,
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
                    DataLimiteRetorno = processoDeCotacao.DataTerminoLeilao.HasValue ? processoDeCotacao.DataTerminoLeilao.Value.ToShortDateString(): null,
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    CodigoMaterial = processoDeCotacao.CodigoMaterial,
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

        public KendoGridVm FornecedoresParticipantes(int idProcessoCotacao)
        {
            IList<Fornecedor> fornecedoresParticipantes = (from p in
                                                               _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                           from fp in p.FornecedoresParticipantes
                                                           select fp.Fornecedor).ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = fornecedoresParticipantes.Count,
                    Registros = _builder.BuildList(fornecedoresParticipantes).Cast<ListagemVm>().ToList()
                };

            return kendoGridVm;

        }
    }
}