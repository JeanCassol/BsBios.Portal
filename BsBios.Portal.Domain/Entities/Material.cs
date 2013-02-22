using System.Collections.Generic;

namespace BsBios.Portal.Domain.Entities
{
    public class Material
    {
        public string Nome { get; set; }
        public IList<Fornecedor> Fornecedores { get; private set; }

        public Material(string nome)
        {
            Nome = nome;
            Fornecedores = new List<Fornecedor>();
        }
        public virtual void AdicionarFornecedor(Fornecedor fornecedor)
        {
            Fornecedores.Add(fornecedor);
        } 


    }

}
