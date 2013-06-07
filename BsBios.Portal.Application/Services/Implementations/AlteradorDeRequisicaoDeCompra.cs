using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AlteradorDeRequisicaoDeCompra : IAlteradorDeRequisicaoDeCompra
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;

        public AlteradorDeRequisicaoDeCompra(IUnitOfWork unitOfWork, IRequisicoesDeCompra requisicoesDeCompra)
        {
            _unitOfWork = unitOfWork;
            _requisicoesDeCompra = requisicoesDeCompra;
        }

        public void Alterar(int idRequisicaoDeCompra)
        {
            
        }
    }
}