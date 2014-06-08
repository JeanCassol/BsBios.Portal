namespace BsBios.Portal.Domain.Entities
{
    public class MaterialDeCarga: IAggregateRoot
    {
        protected MaterialDeCarga() { }
        public virtual int Codigo { get; protected set; }
        public virtual string Descricao { get; protected set; }

        public MaterialDeCarga(string descricao)
        {
            Descricao = descricao;
        }
    }
}
