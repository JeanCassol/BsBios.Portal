using System;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class OrdemDeTransporteService : IOrdemDeTransporteService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrdensDeTransporte _ordensDeTransporte;
        private readonly IConsultaProcessoDeCotacaoDeFrete _consultaProcessoDeCotacaoDeFrete;
        private readonly IConsultaOrdemDeTransporte _consultaOrdemDeTransporte;

        public OrdemDeTransporteService(IUnitOfWork unitOfWork, IOrdensDeTransporte ordensDeTransporte, 
            IConsultaProcessoDeCotacaoDeFrete consultaProcessoDeCotacaoDeFrete, IConsultaOrdemDeTransporte consultaOrdemDeTransporte)
        {
            _unitOfWork = unitOfWork;
            _ordensDeTransporte = ordensDeTransporte;
            _consultaProcessoDeCotacaoDeFrete = consultaProcessoDeCotacaoDeFrete;
            _consultaOrdemDeTransporte = consultaOrdemDeTransporte;
        }

        public void AtualizarOrdemDeTransporte(OrdemDeTransporteAtualizarDTO ordemDeTransporteAtualizarDTO)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(ordemDeTransporteAtualizarDTO.Id).Single();
                int idDoProcessoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete.Id;

                decimal quantidadeContratada = _consultaProcessoDeCotacaoDeFrete.CalcularQuantidadeContratadaNoProcessoDeCotacao(idDoProcessoDeCotacao);
                decimal quantidadeLiberada = _consultaOrdemDeTransporte.CalcularQuantidadeLiberadaPeloProcessoDeCotacao(idDoProcessoDeCotacao);
                decimal quantidadeLiberadaParaOrdemDeTransporte = ordemDeTransporteAtualizarDTO.QuantidadeLiberada;
                decimal novaQuantidadeLiberada = quantidadeLiberada - ordemDeTransporte.QuantidadeLiberada + quantidadeLiberadaParaOrdemDeTransporte;

                if (novaQuantidadeLiberada > quantidadeContratada)
                {
                    throw new QuantidadeLiberadaSuperouQuantidadeAdquiridaException(quantidadeLiberadaParaOrdemDeTransporte, novaQuantidadeLiberada, quantidadeContratada);
                }


                ordemDeTransporte.AlterarQuantidadeLiberada(quantidadeLiberadaParaOrdemDeTransporte);
                _ordensDeTransporte.Save(ordemDeTransporte);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public decimal SalvarColeta(ColetaSalvarVm coletaSalvarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(coletaSalvarVm.IdDaOrdemDeTransporte).Single();

                if (coletaSalvarVm.IdColeta.HasValue)
                {
                    ordemDeTransporte.AtualizarColeta(coletaSalvarVm);
                }
                else
                {
                    var fabricaDeColeta = new ColetaFactory();
                    Coleta coleta = fabricaDeColeta.Construir(coletaSalvarVm, ordemDeTransporte.PrecoUnitario);
                    ordemDeTransporte.AdicionarColeta(coleta);
                }


                _unitOfWork.Commit();

                return ordemDeTransporte.QuantidadeColetada;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public decimal RemoverColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte).Single();
                ordemDeTransporte.RemoverColeta(idDaColeta);
                _ordensDeTransporte.Save(ordemDeTransporte);
                _unitOfWork.Commit();
                return ordemDeTransporte.QuantidadeColetada;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void RealizarColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte).Single();
                ordemDeTransporte.RealizarColeta(idDaColeta);
                _ordensDeTransporte.Save(ordemDeTransporte);
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