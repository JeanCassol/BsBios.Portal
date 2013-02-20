using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Produtos : CompleteRepositoryNh<Produto>, IProdutos
    {
        public Produtos(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Produto BuscaPeloCodigo(string codigoSap)
        {
            return Query.SingleOrDefault(x => x.Codigo == codigoSap);
        }
    }
}
