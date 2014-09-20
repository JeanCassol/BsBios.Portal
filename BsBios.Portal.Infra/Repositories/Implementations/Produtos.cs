using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
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

        public IProdutos FiltraPorListaDeCodigos(string[] codigos)
        {
            Query = Query.Where(x => codigos.Contains(x.Codigo));
            return this;
        }

        public IProdutos DescricaoContendo(string filtroDescricao)
        {
            if (string.IsNullOrEmpty(filtroDescricao)) return this;
            Query = Query.Where(x => x.Descricao.ToLower().Contains(filtroDescricao.ToLower()));
            return this;
        }

        public IProdutos CodigoContendo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return this;

            Query = Query.Where(x => x.Codigo.ToLower().Contains(codigo.ToLower()));
            return this;
        }

        public IProdutos TipoContendo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return this;
            Query = Query.Where(x => x.Tipo.ToLower().Contains(tipo.ToLower()));
            return this;
        }

    }
}
