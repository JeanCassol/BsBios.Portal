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
                    Nome = model.Nome,
                    Cnpj = model.Cnpj,
                    Municipio = model.Municipio,
                    Uf = model.Uf,
                    Transportadora = model.Transportadora? "Sim": "Não"
                };
        }

    }
}
