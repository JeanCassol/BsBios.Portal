using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class ProcessoDeCotacaoFactory : IProcessoDeCotacaoFactory
    {
        private readonly IList<ProcessoDeCotacaoItemVm> _itens;
        public void AdicionarItem(Produto material, UnidadeDeMedida unidadeDeMedida, decimal quantidade)
        {
            throw new System.NotImplementedException();
        }

        private void ValidaItens()
        {
            if (_itens.Count == 0)
            {
                throw new ProcessoDeCotacaoSemItemException();
            }
        }

        public ProcessoDeCotacaoDeMaterial CriarProcesso(RequisicaoDeCompra requisicaoDeCompra)
        {
            ValidaItens();
            throw new System.NotImplementedException();
        }

        public ProcessoDeCotacaoDeFrete CriarProcesso()
        {
            ValidaItens();
            throw new System.NotImplementedException();
        }
    }
}