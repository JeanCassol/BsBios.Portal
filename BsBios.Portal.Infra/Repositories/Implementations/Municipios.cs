using System.Linq;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Municipios : ReadOnlyRepositoryNh<Municipio>, IMunicipios
    {
        public Municipios(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }
        public IMunicipios NomeComecandoCom(string nome)
        {
            Query = Query.Where(municipio => municipio.Nome.ToLower().StartsWith(nome.ToLower()));

            return this;
        }

        public Municipio BuscaPeloCodigo(string codigo)
        {
            return Query.SingleOrDefault(x => x.Codigo == codigo);
        }
    }
}