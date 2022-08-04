using Domain;

namespace Services.Abstract
{
    public interface IAuthenticationService
    {
        User GetCurrentAccount();
        bool Login(string email, string password);
        void Logout();
        void Register(string name, string email, string password);
    }
}
