using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroRequisicaoCompra : ICadastroRequisicaoCompra
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;
        private readonly IUsuarios _usuarios;
        private readonly IFornecedores _fornecedores;
        private readonly IProdutos _produtos ;

        public CadastroRequisicaoCompra(IUnitOfWork unitOfWork, IRequisicoesDeCompra requisicoesDeCompra,
            IUsuarios usuarios, IFornecedores fornecedores, IProdutos produtos, IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
            _unitOfWork = unitOfWork;
            _requisicoesDeCompra = requisicoesDeCompra;
            _usuarios = usuarios;
            _fornecedores = fornecedores;
            _produtos = produtos;
        }

        public void NovaRequisicao(RequisicaoDeCompraVm requisicaoDeCompraVm)
        {
            try
            {
                
                _unitOfWork.BeginTransaction();
                Usuario criador = _usuarios.BuscaPorLogin(requisicaoDeCompraVm.Criador);
                Fornecedor fornecedorPretendido = _fornecedores.BuscaPeloCodigo(requisicaoDeCompraVm.FornecedorPretendido);
                Produto material = _produtos.BuscaPeloCodigo(requisicaoDeCompraVm.Material);
                var requisicaoDeCompra = new RequisicaoDeCompra(criador, requisicaoDeCompraVm.Requisitante, fornecedorPretendido,
                                                                Convert.ToDateTime(requisicaoDeCompraVm.DataDeRemessa),
                                                                Convert.ToDateTime(requisicaoDeCompraVm.DataDeLiberacao),
                                                                Convert.ToDateTime(requisicaoDeCompraVm.DataDeSolicitacao),
                                                                requisicaoDeCompraVm.Centro,
                                                                requisicaoDeCompraVm.UnidadeMedida,
                                                                requisicaoDeCompraVm.Quantidade, material,
                                                                requisicaoDeCompraVm.Descricao,
                                                                requisicaoDeCompraVm.NumeroItem,
                                                                requisicaoDeCompraVm.NumeroRequisicao);

                var processoDeCotacaoDeMaterial = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();

                _requisicoesDeCompra.Save(requisicaoDeCompra);
                _processosDeCotacao.Save(processoDeCotacaoDeMaterial);

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}