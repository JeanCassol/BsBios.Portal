using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AtualizadorDeIteracaoDoUsuario : IAtualizadorDeIteracaoDoUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessoCotacaoIteracoesUsuario _processoCotacaoIteracoesUsuario;

        public AtualizadorDeIteracaoDoUsuario(IUnitOfWork unitOfWork, IProcessoCotacaoIteracoesUsuario processoCotacaoIteracoesUsuario)
        {
            _unitOfWork = unitOfWork;
            _processoCotacaoIteracoesUsuario = processoCotacaoIteracoesUsuario;
        }

        public void Atualizar(int idIteracaoUsuario)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoCotacaoIteracaoUsuario iteracaoUsuario = _processoCotacaoIteracoesUsuario.BuscaPorIdParticipante(idIteracaoUsuario);
                if (iteracaoUsuario.VisualizadoPeloFornecedor)
                {
                    _unitOfWork.RollBack();
                    return;
                }
                iteracaoUsuario.FornecedorVisualizou();
                _processoCotacaoIteracoesUsuario.Save(iteracaoUsuario);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void Adicionar(IList<FornecedorParticipante> fornecedorParticipantes)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var fornecedorParticipante in fornecedorParticipantes)
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