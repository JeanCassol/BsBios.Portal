using System.Collections.Generic;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class ProcessoDeCotacaoDeMaterialFactory : IProcessoDeCotacaoDeMaterialFactory
    {
        private readonly IList<RequisicaoDeCompra> _requisicoesDeCompra;
        public ProcessoDeCotacaoDeMaterialFactory()
        {
            _requisicoesDeCompra = new List<RequisicaoDeCompra>();
            
        }
        public void AdicionarRequisicaoDeCompra(RequisicaoDeCompra requisicaoDeCompra)
        {
            _requisicoesDeCompra.Add(requisicaoDeCompra);
        }

        public ProcessoDeCotacaoDeMaterial CriarProcesso()
        {
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial();
            if (_requisicoesDeCompra.Count == 0)
            {
                throw new ProcessoDeCotacaoSemItemException();
            }
            foreach (var requisicaoDeCompra in _requisicoesDeCompra)
            {
                processoDeCotacao.AdicionarItem(requisicaoDeCompra);
            }
            return processoDeCotacao;
        }
    }
}