namespace BsBios.Portal.Domain.Entities
{
    public class CondicaoDePagamento: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected CondicaoDePagamento(){}
        public CondicaoDePagamento(string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        public virtual void AtualizarDescricao(string descricao)
        {
            Descricao = descricao;
        }

        #region overridiging members

        protected bool Equals(CondicaoDePagamento other)
        {
            return string.Equals(Codigo, other.Codigo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CondicaoDePagamento) obj);
        }

        public override int GetHashCode()
        {
            return (Codigo != null ? Codigo.GetHashCode() : 0);
        }

        #endregion
    }
}
