using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Ivas:CompleteRepositoryNh<Iva>, IIvas
    {
        public Ivas(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Iva BuscaPeloCodigo(string codigoSap)
        {
            return Query.SingleOrDefault(x => x.Codigo == codigoSap);
        }

        public IIvas BuscaListaPorCodigo(string[] codigosIva)
        {
            Query = Query.Where(iva => codigosIva.Contains(iva.Codigo));
            return this;
        }
    }
}
