using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IMunicipios: IReadOnlyRepository<Municipio>
    {
        IMunicipios NomeComecandoCom(string nome);
        Municipio BuscaPeloCodigo(string codigo);
    }
}
