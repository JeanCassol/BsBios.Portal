using System.Linq;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProdutosDeFrete : Produtos, IProdutosDeFrete
    {
        public string TiposDeProdutoDeFrete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="tiposDeProdutoDeFrete">Tipos de Produto que serão utilizados nas cotações de Frete</param>
        public ProdutosDeFrete(IUnitOfWorkNh unitOfWork, string tiposDeProdutoDeFrete) : base(unitOfWork)
        {
            TiposDeProdutoDeFrete = tiposDeProdutoDeFrete;
            string[] codigos = tiposDeProdutoDeFrete.Split(',');
            Query = Query.Where(x => codigos.Contains(x.Codigo));
        }
    }
}
