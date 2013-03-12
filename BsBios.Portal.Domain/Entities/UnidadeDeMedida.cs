using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Entities
{
    public class UnidadeDeMedida: IAggregateRoot
    {
        public virtual string CodigoInterno { get; protected set; }
        public virtual string CodigoExterno { get; protected set; }
        public virtual string Descricao { get; protected set; }

        protected UnidadeDeMedida() { }

        public UnidadeDeMedida(string codigoInterno, string codigoExterno, string descricao)
        {
            CodigoInterno = codigoInterno;
            CodigoExterno = codigoExterno;
            Descricao = descricao;
        }

        public virtual void Atualizar(string codigoExterno, string descricao)
        {
            CodigoExterno = codigoExterno;
            Descricao = descricao;
        }
    }



}
