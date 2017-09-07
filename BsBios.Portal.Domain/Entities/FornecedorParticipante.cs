using System;
using System.Linq;
using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class FornecedorParticipante
    {
        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacao ProcessoDeCotacao { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual Cotacao Cotacao { get; protected set; }
        public virtual Enumeradores.RespostaDaCotacao Resposta { get; protected set; }
        public virtual string NumeroDaCotacao { get; protected set; }
        
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

        public virtual void AtualizarNumeroDaCotacao(string numeroDaCotacao)
        {
            NumeroDaCotacao = numeroDaCotacao;
        }

        public virtual void Recusar()
        {
            if (Cotacao != null && Cotacao.Itens.Any(x => x.Selecionada))
            {
                throw new Exception("Não é possível recusar a cotação, pois a mesma já foi selecionada.");
            }
            Resposta = Enumeradores.RespostaDaCotacao.Recusado;
        }

        public virtual void AceitarCotacao()
        {
            this.Resposta = Enumeradores.RespostaDaCotacao.Aceito;
        }
    }
}
