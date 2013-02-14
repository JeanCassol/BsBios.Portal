namespace BsBios.Portal.Domain.Model
{
    public class Iva:IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string CodigoSap { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected Iva(){}
        public Iva(string codigoSap, string descricao)
        {
            CodigoSap = codigoSap;
            Descricao = descricao;
        }

        public virtual void AtualizaDescricao(string novaDescricao)
        {
            Descricao = novaDescricao;
        }
    }
}
