using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.ValueObjects
{
    public class Municipio: IAggregateRoot
    {
        public Municipio(string codigo, string nome)
        {
            Codigo = codigo;
            Nome = nome;
        }

        protected Municipio()
        {
        }

        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string UF { get; set; }
        public override string ToString()
        {
            return Nome  + "/" + UF;
        }
    }
}
