using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AtualizadorDeCotacaoDeFrete : IAtualizadorDeCotacaoDeFrete
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly ICotacaoHistoricoRepository _cotacaoHistoricos;
        private readonly UsuarioConectado _usuarioConectado;


        public AtualizadorDeCotacaoDeFrete(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, UsuarioConectado usuarioConectado, ICotacaoHistoricoRepository cotacaoHistoricos)
        {
            _processosDeCotacao = processosDeCotacao;
            _usuarioConectado = usuarioConectado;
            this._cotacaoHistoricos = cotacaoHistoricos;
            _unitOfWork = unitOfWork;
        }

        public void Atualizar(CotacaoInformarVm cotacaoInformarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(cotacaoInformarVm.IdProcessoCotacao).Single();
                ProcessoDeCotacaoDeFreteItem item = processoDeCotacao.ObterItem();

                var valorComImpostos = cotacaoInformarVm.ValorComImpostos ?? (item.ValorFechado ?? 0);
                var quantidadeDisponivel = cotacaoInformarVm.QuantidadeDisponivel ?? 0;
                var fornecedorParticipante = processoDeCotacao.InformarCotacao(cotacaoInformarVm.CodigoFornecedor,valorComImpostos,
                    quantidadeDisponivel, cotacaoInformarVm.ObservacoesDoFornecedor);

                _processosDeCotacao.Save(processoDeCotacao);

                //inserir histórico de remoção
                var cotacaoHistorico = new CotacaoHistorico(fornecedorParticipante.Id,  _usuarioConectado.ToString(),
                     $"Cotação informada: Quantidade = {quantidadeDisponivel}; Preço = {valorComImpostos}");

                this._cotacaoHistoricos.Save(cotacaoHistorico);


                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
            
        }

        public void SairDoProcesso(int idDoProcessoDeCotacao, string codigoDoFornecedor)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idDoProcessoDeCotacao).Single();

                processoDeCotacao.DesativarParticipante(codigoDoFornecedor);

                _processosDeCotacao.Save(processoDeCotacao);

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void Remover(int idDoProcessoDeCotacao, string codigoDoFornecedor)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                //remover processo
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idDoProcessoDeCotacao).Single();
                var fornecedorParticipante = processoDeCotacao.RemoverCotacao(codigoDoFornecedor);
                _processosDeCotacao.Save(processoDeCotacao);

                //inserir histórico de remoção
                var cotacaoHistorico = new CotacaoHistorico(fornecedorParticipante.Id, _usuarioConectado.ToString(),
                    "Liberação para informar nova cotação");

                this._cotacaoHistoricos.Save(cotacaoHistorico);

                _unitOfWork.Commit();

            }
            catch (Exception )
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}