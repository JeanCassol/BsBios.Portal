namespace BsBios.Portal.Domain.Model
{
    public class Iva:IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected Iva(){}
        public Iva(string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        public virtual void AtualizaDescricao(string novaDescricao)
        {
            Descricao = novaDescricao;
        }
    }
}
