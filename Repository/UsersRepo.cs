using Demo2CoreAPICrud.Data;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo2CoreAPICrud.Repository
{
    public class UsersRepo : IUsersRepo
    {
        private readonly DataContext _context;

        public UsersRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                    .OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                    .ToArrayAsync();
        }

        public User? GetUserById(string userId)
        {
            return _context.Users
                    .FirstOrDefault(x => x.Id.ToString().Trim().ToLower() == userId.Trim().ToLower());
        }

        public async Task<IEnumerable<User>?> Search(UserSearchDto userSearchDto)
        {
            return await _context.Users
                    .Where(x =>
                        x.FirstName.Contains(userSearchDto.Forename) &&
                        x.LastName.Contains(userSearchDto.Surname))
                    .OrderBy(o => o.LastName).ThenBy(o => o.FirstName)
                    .ToListAsync();            
        }

        public bool UserExistsById(Guid userId)
        {
            var exists = _context.Users.Find(userId);
            return exists != null;
        }

        public bool UserExistsByName(string forename, string surname)
        {
            var exists = _context.Users
                    .FirstOrDefault(x =>
                        x.FirstName.ToLower() == forename.Trim().ToLower() &&
                        x.LastName.ToLower() == surname.Trim().ToLower());

            return exists != null;
        }

        public async Task<bool> CreateUser(User user)
        {           
            user.Id = Guid.NewGuid();
            await _context.Users.AddAsync(user);
            return Save().Result;
        }

        public Task<bool> UpdateUser(User user)
        {
            var existingUser = _context.Users.Local.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Detached;
            }
            _context.Users.Update(user);
            _context.Entry(user).State = EntityState.Modified;
            return Save();
        }        

        public Task<bool> DeleteUser(User user)
        {
            var userLogs = _context.Logs.Where(x=> x.Id == user.Id).ToList();

            if (userLogs != null && userLogs.Count > 0)
                _context.Logs.RemoveRange(userLogs);

            if(user != null) _context.Users.Remove(user);

            return Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }        
    }
}
