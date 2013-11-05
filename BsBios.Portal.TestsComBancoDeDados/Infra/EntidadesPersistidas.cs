using System.Collections.Generic;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra
{
    public class EntidadesPersistidas
    {
        public static List<Municipio> ObterDoisMunicipiosCadastrados()
        {
            var municipios = ObjectFactory.GetInstance<IMunicipios>();

            Municipio municipioOrigem = municipios.BuscaPeloCodigo("4312401"); //Montenegro
            Municipio municipioDestino = municipios.BuscaPeloCodigo("4312443"); //Morrinhos

            return new List<Municipio>
            {
                municipioOrigem,
                municipioDestino
            };

        }
    }
}
