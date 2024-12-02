using API.Models;

namespace API.DAO.Interface
{
    public interface IUserDAO
    {
        User? Authenticate(string username, string password);

        bool CreateUser (User user);
    }
}
