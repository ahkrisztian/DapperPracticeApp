using DataAccessLibrary.DTOs;
using DataAccessLibrary.Models;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dbcontext.Data
{
    public interface IUserData
    {
        Task<IEnumerable<UserModel>> CreateUserWithAddress(CreateUserModelDTO model);

        Task DeleteUser(int id);

        Task<UserModel> UpdateUser(int id, UpdateUserModelDTO model);

        Task<List<ReadUserModelDTO>> GetAllUsers();

        Task<IEnumerable<UserModel>> GetUserWithAddresses(int id);
    }
}