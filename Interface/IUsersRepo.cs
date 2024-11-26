using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Models;

namespace Demo2CoreAPICrud.Interface
{
    public interface IUsersRepo
    {
        Task<IEnumerable<User>> GetAllUsers();
        User? GetUserById(string userId);
        Task<IEnumerable<User>?> Search(UserSearchDto userSearchDto);
        bool UserExistsById(Guid userId);
        bool UserExistsByName(string forename, string surname);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<bool> Save();
    }
}
