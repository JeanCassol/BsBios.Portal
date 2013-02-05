namespace BsBios.Portal.Domain.Model
{
    public class Produto: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string CodigoSap { get; protected set; }
        public virtual string Descricao { get; protected set; }

        protected Produto()
        {
        }

        public Produto(string codigoSap, string descricao)
        {
            CodigoSap = codigoSap;
            Descricao = descricao;
        }

    }
}
