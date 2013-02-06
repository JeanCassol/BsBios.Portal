using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Fornecedores:CompleteRepositoryNh<Fornecedor>,  IFornecedores
    {
        public Fornecedores(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Fornecedor BuscaPeloCodigoSap(string codigoSap)
        {
            return Query.SingleOrDefault(x => x.CodigoSap == codigoSap);
        }
    }
}
