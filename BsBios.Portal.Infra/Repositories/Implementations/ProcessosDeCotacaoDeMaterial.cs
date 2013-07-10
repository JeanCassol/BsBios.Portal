using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using System.Linq;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacaoDeMaterial : ProcessosDeCotacao, IProcessosDeCotacaoDeMaterial
    {
        public ProcessosDeCotacaoDeMaterial(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
            FiltraPorTipo(Enumeradores.TipoDeCotacao.Material);
        }

        //public IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(string numeroDaRequisicao, string numeroDoItem)
        //{
        //    Query = Query.Where(pcm => pcm.Itens.Any(item => ((ProcessoDeCotacaoDeMaterialItem) item).RequisicaoDeCompra.Numero == numeroDaRequisicao
        //        && ((ProcessoDeCotacaoDeMaterialItem)item).RequisicaoDeCompra.NumeroItem == numeroDoItem));
            
        //    return this;
        //}

        public IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(int idRequisicaoDeCompra)
        {
            //Query = Query.Where(pcm => pcm.Itens.Any(item => ((ProcessoDeCotacaoDeMaterialItem)item).RequisicaoDeCompra.Id == idRequisicaoDeCompra));
            Query = (from pcm in GetQuery()
                     from item in pcm.Itens
                     let itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item
                     where itemMaterial.RequisicaoDeCompra.Id == idRequisicaoDeCompra
                     select pcm);

            return this;
        }
    }
}