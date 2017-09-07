using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AlteradorDeRequisicaoDeCompra : IAlteradorDeRequisicaoDeCompra
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;
        private readonly IProcessosDeCotacaoDeMaterial _processosDeCotacaoDeMaterial;

        public AlteradorDeRequisicaoDeCompra(IUnitOfWork unitOfWork, IRequisicoesDeCompra requisicoesDeCompra, 
            IProcessosDeCotacaoDeMaterial processosDeCotacaoDeMaterial)
        {
            _unitOfWork = unitOfWork;
            _requisicoesDeCompra = requisicoesDeCompra;
            _processosDeCotacaoDeMaterial = processosDeCotacaoDeMaterial;
        }

        public void Bloquear(int idRequisicaoDeCompra)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                int quantidadeDeProcessosVinculados = _processosDeCotacaoDeMaterial.GeradosPelaRequisicaoDeCompra(idRequisicaoDeCompra).Count();
                if (quantidadeDeProcessosVinculados > 0)
                {
                    throw new RequisicaoDeCompraComProcessoDeCotacaoBloqueioException();
                }

                RequisicaoDeCompra requisicaoDeCompra =  _requisicoesDeCompra.BuscaPeloId(idRequisicaoDeCompra);
                requisicaoDeCompra.Bloquear();

                _requisicoesDeCompra.Save(requisicaoDeCompra);

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