using BsBios.Portal.ApplicationServices.Contracts;

namespace BsBios.Portal.ApplicationServices.Implementation
{
    public class HelloWorld: IHelloWorld
    {
        public string SayHello(string nome)
        {
            return "Hello " + nome + "!";
        }

        public string SayGoodBye(string nome)
        {
            return "Good bye " + nome + "!";
        }
    }
}
