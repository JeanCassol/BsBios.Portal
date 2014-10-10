using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class FornecedorParticipante
    {
        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual Cotacao Cotacao { get; protected set; }
        public virtual Enumeradores.RespostaDaCotacao Resposta { get; set; }
        protected FornecedorParticipante(){}


        public FornecedorParticipante(ProcessoDeCotacao processoDeCotacao, Fornecedor fornecedor)
        {
            ProcessoDeCotacao = processoDeCotacao;
            Fornecedor = fornecedor;
            Resposta = Enumeradores.RespostaDaCotacao.Pendente;
        }

        public virtual Cotacao InformarCotacao(Cotacao cotacao)
        {
            Resposta = Enumeradores.RespostaDaCotacao.Aceito;
            Cotacao = cotacao;
            return Cotacao;
        }

        public virtual void Recusar()
        {
            Resposta = Enumeradores.RespostaDaCotacao.Recusado;
        }
    }
}
