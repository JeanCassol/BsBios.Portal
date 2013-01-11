using BsBios.Portal.ApplicationServices.Contracts;

namespace BsBios.Portal.ApplicationServices.Implementation
{
    public class SendMessage:ISendMessage
    {

        private readonly IHelloWorld _helloWorld;

        public SendMessage(IHelloWorld helloWorld)
        {
            _helloWorld = helloWorld;
        }

        public string SendWelcomeMessage(string destinatario)
        {
            return _helloWorld.SayHello(destinatario);
        }
    }
}
