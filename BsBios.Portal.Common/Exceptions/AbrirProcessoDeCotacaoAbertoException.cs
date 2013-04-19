using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class AbrirProcessoDeCotacaoAbertoException: Exception
    {
        public override string Message
        {
            get { return "  Não é possível abrir um Processo de Cotação que já está aberto."; }
        }
    }
}