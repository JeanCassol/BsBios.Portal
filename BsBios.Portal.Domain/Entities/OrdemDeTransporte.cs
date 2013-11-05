using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public class OrdemDeTransporte : IAggregateRoot
    {


        internal OrdemDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao, Fornecedor fornecedor, 
            decimal quantidadeAdquirida, decimal precoUnitario)
        {
            ProcessoDeCotacaoDeFrete = processoDeCotacao;
            Fornecedor = fornecedor;
            QuantidadeAdquirida = quantidadeAdquirida;
            PrecoUnitario = precoUnitario;
            QuantidadeLiberada = quantidadeAdquirida;
        }

        protected OrdemDeTransporte(){}

        public virtual ProcessoDeCotacaoDeFrete ProcessoDeCotacaoDeFrete { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual decimal QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeLiberada { get; protected set; }
        public virtual decimal QuantidadeColetada { get; protected set; }

        public virtual int Id { get; protected set; }

        public virtual decimal PrecoUnitario { get; set; }

        public virtual void AlterarQuantidadeLiberada(decimal novaQuantidadeLiberada)
        {
            if (novaQuantidadeLiberada > QuantidadeAdquirida)
            {
                throw new QuantidadeLiberadaSuperouQuantidadeAdquiridaException(novaQuantidadeLiberada, QuantidadeAdquirida);
            }
            QuantidadeLiberada = novaQuantidadeLiberada;
        }
    }
}