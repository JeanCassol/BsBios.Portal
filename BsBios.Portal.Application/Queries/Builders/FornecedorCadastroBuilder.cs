using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class FornecedorCadastroBuilder: Builder<Fornecedor,FornecedorCadastroVm>
    {
        public override FornecedorCadastroVm BuildSingle(Fornecedor model)
        {
            return new FornecedorCadastroVm()
                {
                    Codigo = model.Codigo ,
                    Email = model.Email,
                    Nome = model.Nome
                };
        }

        //public IList<FornecedorCadastroVm> BuildList(IList<Fornecedor> models)
        //{
        //    return models.Select(BuildSingle).ToList();
        //}
    }
}
