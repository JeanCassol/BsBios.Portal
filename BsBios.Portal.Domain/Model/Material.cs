using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
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
