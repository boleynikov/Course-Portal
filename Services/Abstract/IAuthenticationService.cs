namespace Services.Abstract
{
    public interface IAuthenticationService
    {
        bool Login(string email, string password);
        void Logout();
        void Register(string name, string email, string password);
    }
}
