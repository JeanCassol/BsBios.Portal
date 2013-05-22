using System;

namespace BsBios.Portal.Domain.Entities
{
    public class RequisicaoDeCompra: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string Numero { get; protected set; }
        public virtual string NumeroItem { get; protected set; }
        public virtual string Descricao { get; protected set; }
        public virtual Produto Material { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual UnidadeDeMedida UnidadeMedida { get; protected set; }
        public virtual string Centro { get; protected set; }
        public virtual DateTime DataDeSolicitacao { get; protected set; }
        public virtual DateTime DataDeLiberacao { get; protected set; }
        public virtual DateTime DataDeRemessa { get; protected set; }
        public virtual Fornecedor FornecedorPretendido { get; protected set; }
        public virtual string Requisitante { get; protected set; }
        public virtual Usuario Criador { get; protected set; }
        public virtual string CodigoGrupoDeCompra { get; protected set; }
        public virtual bool Mrp { get; protected set; }
        public virtual bool GerouProcessoDeCotacao { get; protected set; }

        protected RequisicaoDeCompra(){}

        public RequisicaoDeCompra(Usuario criador, string requisitante, Fornecedor fornecedorPretendido, 
            DateTime dataDeRemessa, DateTime dataDeLiberacao, DateTime dataDeSolicitacao, string centro, 
            UnidadeDeMedida unidadeMedida, decimal quantidade, Produto material, string descricao, string numeroItem, 
            string numero, string codigoGrupoDeCompra, bool mrp)
        {
            Criador = criador;
            Requisitante = requisitante;
            FornecedorPretendido = fornecedorPretendido;
            DataDeRemessa = dataDeRemessa;
            DataDeLiberacao = dataDeLiberacao;
            DataDeSolicitacao = dataDeSolicitacao;
            Centro = centro;
            UnidadeMedida = unidadeMedida;
            Quantidade = quantidade;
            Material = material;
            Descricao = descricao;
            NumeroItem = numeroItem;
            Numero = numero;
            CodigoGrupoDeCompra = codigoGrupoDeCompra;
            Mrp = mrp;
            GerouProcessoDeCotacao = false;
        }

        public virtual ProcessoDeCotacaoDeMaterial GerarProcessoDeCotacaoDeMaterial()
        {
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(/*this*/);
            processoDeCotacao.AdicionarItem(this);
            return processoDeCotacao;
        }

        public virtual void VincularComProcessoDeCotacao()
        {
            GerouProcessoDeCotacao = true;
        }

        public virtual void DesvincularDeProcessoDeCotacao()
        {
            GerouProcessoDeCotacao = false;
        }

        #region Equals

        protected bool Equals(RequisicaoDeCompra other)
        {
            return string.Equals(NumeroItem, other.NumeroItem) && string.Equals(Numero, other.Numero);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NumeroItem.GetHashCode() * 397) ^ Numero.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RequisicaoDeCompra) obj);
        }
        #endregion
    }
}
