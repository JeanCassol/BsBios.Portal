using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoStatusService : IProcessoDeCotacaoStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IEmailService _emailService;
        public  IComunicacaoSap ComunicacaoSap { get; set; }

        public ProcessoDeCotacaoStatusService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _emailService = emailService;
        }

        private void EnviarEmailDeAbertura(ProcessoDeCotacao processoDeCotacao)
        {
            string mensagem = "Fornecedor, você está convidado a participar do Processo de Cotação de Fretes." +
                              Environment.NewLine +
                              "Material: " + processoDeCotacao.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + processoDeCotacao.Quantidade + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacao.UnidadeDeMedida.Descricao  + Environment.NewLine +
                              "Data Limite de Retorno:  " + processoDeCotacao.DataLimiteDeRetorno.ToString() + Environment.NewLine +
                              "Para informar a sua cotação acesse o Portal da BS BIOS até a data limite de retorno.";

            var mensagemDeEmail = new MensagemDeEmail("Cotação de Frete", mensagem);

            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                _emailService.Enviar(fornecedorParticipante.Fornecedor.Email, mensagemDeEmail);
            }


        }

        public void AbrirProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Abrir();
                EnviarEmailDeAbertura(processoDeCotacao);
                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Fechar();
                ComunicacaoSap.EfetuarComunicacao(processoDeCotacao);
                _processosDeCotacao.Save(processoDeCotacao);
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