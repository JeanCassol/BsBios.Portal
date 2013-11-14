using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public class OrdemDeTransporte : IAggregateRoot
    {


        internal OrdemDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao, Fornecedor fornecedor, 
            decimal quantidadeAdquirida, decimal precoUnitario):this()
        {
            ProcessoDeCotacaoDeFrete = processoDeCotacao;
            Fornecedor = fornecedor;
            QuantidadeAdquirida = quantidadeAdquirida;
            PrecoUnitario = precoUnitario;
            QuantidadeLiberada = quantidadeAdquirida;
        }

        protected OrdemDeTransporte()
        {
            Coletas = new List<Coleta>();
        }

        public virtual int Id { get; protected set; }
        public virtual ProcessoDeCotacaoDeFrete ProcessoDeCotacaoDeFrete { get; protected set; }
        public virtual Fornecedor Fornecedor { get; protected set; }
        public virtual decimal QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeLiberada { get; protected set; }
        public virtual decimal QuantidadeColetada { get; protected set; }

        public virtual decimal PrecoUnitario { get; protected set; }

        public virtual IList<Coleta> Coletas { get; protected set; }

        public virtual void AlterarQuantidadeLiberada(decimal novaQuantidadeLiberada)
        {
            if (novaQuantidadeLiberada > QuantidadeAdquirida)
            {
                throw new QuantidadeLiberadaSuperouQuantidadeAdquiridaException(novaQuantidadeLiberada, QuantidadeAdquirida);
            }
            if (novaQuantidadeLiberada < QuantidadeColetada)
            {
                throw new QuantidadeLiberadaAbaixoDaQuantidadeColetadaException(novaQuantidadeLiberada, QuantidadeColetada);
            }
            QuantidadeLiberada = novaQuantidadeLiberada;
        }

        public virtual void AdicionarColeta(Coleta coleta)
        {
            Coletas.Add(coleta);
            QuantidadeColetada = Coletas.Sum(x => x.Peso);
            if (QuantidadeColetada > QuantidadeLiberada)
            {
                throw new QuantidadeColetadaUltrapassouQuantidadeLiberadaException(QuantidadeColetada, QuantidadeLiberada);
            }
        }

        public virtual void AtualizarColeta(ColetaSalvarVm coletaSalvarVm)
        {
            var coleta = Coletas.Single(x => x.Id == coletaSalvarVm.IdColeta);

            coleta.Atualizar(coletaSalvarVm,PrecoUnitario);

            QuantidadeColetada = Coletas.Sum(x => x.Peso);

        }


        public virtual void RealizarColeta(int idDaColeta)
        {
            Coleta coleta = Coletas.Single(c => c.Id == idDaColeta);
            coleta.Realizar();
        }
    }
}