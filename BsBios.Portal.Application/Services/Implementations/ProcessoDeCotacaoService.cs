using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoService : IProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IFornecedores _fornecedores;
        public ProcessoDeCotacaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IFornecedores fornecedores)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _fornecedores = fornecedores;
        }

        public void AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm)
        {
            try
            {
                _unitOfWork.BeginTransaction(); 
                var processoDeCotacaoDeMaterial = (ProcessoDeCotacaoDeMaterial)_processosDeCotacao.BuscaPorId(atualizacaoDosFornecedoresVm.IdProcessoCotacao).Single();
                string[] codigoDosFonecedoresAtuais = processoDeCotacaoDeMaterial.FornecedoresParticipantes.Select(x => x.Fornecedor.Codigo).ToArray();
                IList<string> codigoDosFornecedoresQueDevemSerRemovidos =
                    codigoDosFonecedoresAtuais.Except(atualizacaoDosFornecedoresVm.CodigoFornecedoresSelecionados).ToList();

                string[] codigoDosFornecedoresQueDevemSerAdicionados =
                    atualizacaoDosFornecedoresVm.CodigoFornecedoresSelecionados.Except(codigoDosFonecedoresAtuais)
                                                .ToArray();
                IList<Fornecedor> fornecedoresParaAdicionar =
                    _fornecedores.BuscaListaPorCodigo(codigoDosFornecedoresQueDevemSerAdicionados).List();

                foreach (var fornecedor in fornecedoresParaAdicionar)
                {
                    processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);
                }

                foreach (var codigoDoFornecedorQueDeveSerRemovido in codigoDosFornecedoresQueDevemSerRemovidos)
                {
                    processoDeCotacaoDeMaterial.RemoverFornecedor(codigoDoFornecedorQueDeveSerRemovido);
                }

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