namespace BsBios.Portal.Domain.Model
{
    public class CondicaoDePagamento: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string CodigoSap { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected CondicaoDePagamento(){}
        public CondicaoDePagamento(string codigoSap, string descricao)
        {
            CodigoSap = codigoSap;
            Descricao = descricao;
        }
    }
}
