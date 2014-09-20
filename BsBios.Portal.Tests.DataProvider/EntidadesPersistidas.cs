using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using StructureMap;

namespace BsBios.Portal.Tests.DataProvider
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

        public static MaterialDeCarga ObterSoja()
        {
            var materiaisDeCarga = ObjectFactory.GetInstance<IMateriaisDeCarga>();
            return materiaisDeCarga.BuscaPorCodigo(1).Single();
        }

        public static MaterialDeCarga ObterFarelo()
        {
            var materiaisDeCarga = ObjectFactory.GetInstance<IMateriaisDeCarga>();
            return materiaisDeCarga.BuscaPorCodigo(2).Single();
        }
    }
}
