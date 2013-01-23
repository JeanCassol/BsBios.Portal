using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class FornecedorMap: ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            Id(x => x.Id);
            Map(x => x.Nome).Column("Nome");
        }
    }
}
