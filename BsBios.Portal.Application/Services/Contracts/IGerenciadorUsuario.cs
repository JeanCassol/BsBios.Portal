namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IGerenciadorUsuario
    {
        void CriarSenha(string login, string senha);
        void AlterarSenha(string login, string senhaAtual, string senhaNova);
        void Habilitar(string login);
        void Desabilitar(string login);
    }
}