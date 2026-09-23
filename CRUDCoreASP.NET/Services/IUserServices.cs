using CRUDCoreASP.NET.Models;

namespace CRUDCoreASP.NET.Services
{
    public interface IUserServices
    {
        Task<List<Users>> GetAllUsers();
        Task<List<Users>> CreateUser(Users user);
        Task<List<Users>> UpdateUser(Users user);
        Task<bool> DeleteUser(int id);
    }
}
