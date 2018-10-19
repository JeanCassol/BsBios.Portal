using System;

namespace BsBios.Portal.Domain.Entities
{
    public class CotacaoHistorico: IAggregateRoot
    {
        protected CotacaoHistorico()
        {
        }

        public virtual int Id { get; protected set; }
        public virtual int IdFornecedorParticipante { get; }
        public virtual string Usuario { get; }
        public virtual DateTime DataHora { get; }
        public virtual string Descricao { get; }

        public CotacaoHistorico(int idFornecedorParticipante, string usuario, string descricao)
        {
            IdFornecedorParticipante = idFornecedorParticipante;
            Usuario = usuario;
            DataHora = DateTime.Now;
            Descricao = descricao;
        }
    }
}