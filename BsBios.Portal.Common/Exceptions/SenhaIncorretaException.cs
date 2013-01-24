using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class SenhaIncorretaException : Exception
    {
        public override string Message
        {
            get { return "A senha informada está incorreta."; }
        }
    }
}