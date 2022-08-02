namespace Services.Abstract
{
    public interface IAuthenticationService
    {
        void Login(string email, string password);
        void Logout();
        void Register(string name, string email, string password);
    }
}
