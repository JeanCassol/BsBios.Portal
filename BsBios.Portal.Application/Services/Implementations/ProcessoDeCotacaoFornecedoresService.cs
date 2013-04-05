using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoFornecedoresService : IProcessoDeCotacaoFornecedoresService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IFornecedores _fornecedores;
        private readonly IProcessoCotacaoIteracoesUsuario _processoCotacaoIteracoesUsuario;

        public ProcessoDeCotacaoFornecedoresService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, 
            IFornecedores fornecedores, IProcessoCotacaoIteracoesUsuario processoCotacaoIteracoesUsuario)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _fornecedores = fornecedores;
            _processoCotacaoIteracoesUsuario = processoCotacaoIteracoesUsuario;
        }

        public void AtualizarFornecedores(ProcessoDeCotacaoFornecedoresAtualizarVm atualizacaoDosFornecedoresVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = _processosDeCotacao.BuscaPorId(atualizacaoDosFornecedoresVm.IdProcessoCotacao).Single();
                string[] codigoDosFonecedoresAtuais = processoDeCotacao.FornecedoresParticipantes.Select(x => x.Fornecedor.Codigo).ToArray();
                IList<string> codigoDosFornecedoresQueDevemSerRemovidos =
                    codigoDosFonecedoresAtuais.Except(atualizacaoDosFornecedoresVm.CodigoFornecedoresSelecionados).ToList();

                string[] codigoDosFornecedoresQueDevemSerAdicionados =
                    atualizacaoDosFornecedoresVm.CodigoFornecedoresSelecionados.Except(codigoDosFonecedoresAtuais)
                                                .ToArray();
                IList<Fornecedor> fornecedoresParaAdicionar =
                    _fornecedores.BuscaListaPorCodigo(codigoDosFornecedoresQueDevemSerAdicionados).List();

                var fornecedoresParticipantesAdicionados = new List<FornecedorParticipante>();

                foreach (var fornecedor in fornecedoresParaAdicionar)
                {
                    fornecedoresParticipantesAdicionados.Add(processoDeCotacao.AdicionarFornecedor(fornecedor)); 
                }

                foreach (var codigoDoFornecedorQueDeveSerRemovido in codigoDosFornecedoresQueDevemSerRemovidos)
                {
                    processoDeCotacao.RemoverFornecedor(codigoDoFornecedorQueDeveSerRemovido);
                }

                _processosDeCotacao.Save(processoDeCotacao);

                foreach (var fornecedorParticipante in fornecedoresParticipantesAdicionados)
                {
                    var iteracaoUsuario = new ProcessoCotacaoIteracaoUsuario(fornecedorParticipante.Id);
                    _processoCotacaoIteracoesUsuario.Save(iteracaoUsuario);
                }

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