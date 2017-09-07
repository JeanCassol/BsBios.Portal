using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoDeMaterialItensService : IProcessoDeCotacaoDeMaterialItensService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IRequisicoesDeCompra _requisicoesDeCompra;

        public ProcessoDeCotacaoDeMaterialItensService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IRequisicoesDeCompra requisicoesDeCompra)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _requisicoesDeCompra = requisicoesDeCompra;
        }

        public void AtualizarItens(int idProcessoCotacao, IList<int> idsDasRequisicoesDeCompra)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial) _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                
                //busca todos os itens que devem ser removidos: são os itens que estão no processo de cotação mas não estão na lista de ids recebidas por parâmetro. 
                IList<ProcessoDeCotacaoItem> itensParaRemover = processoDeCotacao.Itens.Where(x => idsDasRequisicoesDeCompra.All(id => id != ((ProcessoDeCotacaoDeMaterialItem) x).RequisicaoDeCompra.Id)).ToList();
                foreach (var processoDeCotacaoItem in itensParaRemover)
                {
                    var itemMaterial = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacaoItem;
                    var requisicaoDeCompra = itemMaterial.RequisicaoDeCompra;
                    processoDeCotacao.RemoverItem(processoDeCotacaoItem);
                    //o método processoDeCotacao.RemoverItem(processoDeCotacaoItem) remove o vinculo entre a requisição de compra e o item.
                    //após fazer isso tem que salvar a requisição de compra
                    _requisicoesDeCompra.Save(requisicaoDeCompra); 
                }

                //busca todos os itens que devem ser adicionados: são os itens que estão na lista de ids recebidas por parâmetro e não estão no processo de cotação
                int[] itensParaAdicionar = idsDasRequisicoesDeCompra.Where(r => processoDeCotacao.Itens.All(item => ((ProcessoDeCotacaoDeMaterialItem)item).RequisicaoDeCompra.Id != r)).ToArray();

                //carrega lista de requisições que devem ser adicionadas
                IList<RequisicaoDeCompra> requisicoesDeComprasSelecionadas = _requisicoesDeCompra.FiltraPorIds(itensParaAdicionar);
                foreach (var requisicaoDeCompra in requisicoesDeComprasSelecionadas)
                {
                    processoDeCotacao.AdicionarItem(requisicaoDeCompra);
                }

                _processosDeCotacao.Save(processoDeCotacao);

                foreach (var requisicaoDeCompra in requisicoesDeComprasSelecionadas)
                {
                    //o método processoDeCotacao.AdicionarItem(requisicaoDeCompra) cria um vinculo com o item na Requisição de Compra
                    //após fazer isso tem que salvar a requisição. Isto tem que ser feito após salvar o processo de cotação, pois é o 
                    //processo que salvar o item (gerando sua chave) que fica relacionada com a requisição de compra.
                    _requisicoesDeCompra.Save(requisicaoDeCompra);
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