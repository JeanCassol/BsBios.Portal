using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using NHibernate.Linq;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Produtos : CompleteRepositoryNh<Produto>, IProdutos
    {
        public Produtos(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Produto BuscaPeloCodigo(string codigoSap)
        {
            Produto produto = Query.SingleOrDefault(x => x.Codigo == codigoSap);
            Query = UnitOfWorkNh.Session.Query<Produto>();
            return produto;
        }

        public IProdutos FiltraPorDescricao(string filtroDescricao)
        {
            Query = Query.Where(x => x.Descricao.ToLower().Contains(filtroDescricao.ToLower()));
            return this;
        }

        public IProdutos FiltraPorListaDeCodigos(string[] codigos)
        {
            Query = Query.Where(x => codigos.Contains(x.Codigo));
            return this;
        }
    }
}
