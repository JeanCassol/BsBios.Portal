using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IMunicipios: IReadOnlyRepository<Municipio>
    {
        IMunicipios NomeComecandoCom(string nome);
        Municipio BuscaPeloCodigo(string codigo);
    }
}
