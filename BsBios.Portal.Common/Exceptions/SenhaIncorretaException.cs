using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class SenhaIncorretaException : Exception
    {
        private readonly string _message;
        public SenhaIncorretaException(string message) : base(message)
        {
            _message = message;
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}