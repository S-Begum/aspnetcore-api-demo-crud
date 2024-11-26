using Demo2CoreAPICrud.Data;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo2CoreAPICrud.Repository
{
    public class LogsRepo : ILogsRepo
    {
        private readonly DataContext _context;

        public LogsRepo(DataContext context)
        {
            _context = context;
        }        

        public async Task<IEnumerable<object>> GetAll()
        {
            var logs = await _context.Logs.OrderByDescending(o => o.LogDate).ToListAsync();
            var result = await SearchResult(logs);
            return result;
        }

        public async Task<object> GetById(int id)
        {
            var logs = await _context.Logs.Where(x => x.LogId == id).ToListAsync();
            var result = await SearchResult(logs);
            return result;
        }

        public async Task<IEnumerable<object>> GetByUserId(string id)
        {
            var logs = await _context.Logs
                    .Where(x => x.Id.ToString().Trim().ToLower() == id.Trim().ToLower())
                    .OrderByDescending(o => o.LogDate)
                    .ToListAsync();
            var result = await SearchResult(logs);
            return result;
        }

        public async Task<IEnumerable<object>> Search(LogsSearchDto logsSearchDto)
        {            
            var logsList = await _context.Logs
                    .Where(x =>
                        x.LogDate!.Value.Date == logsSearchDto.DateLogged.ToDateTime(TimeOnly.MinValue).Date &&
                        x.Present == logsSearchDto.UserPresent)
                    .ToListAsync();
            var result = await SearchResult(logsList);
            return result;
        }

        public async Task<IEnumerable<object>> SearchByUser(UserSearchDto userSearchDto)
        {        
            var guids = await _context.Users
                    .Where(x =>
                        x.FirstName.Contains(userSearchDto.Forename) &&
                        x.LastName.Contains(userSearchDto.Surname))
                    .Select(x => x.Id)
                    .ToListAsync();

            var logsList = await _context.Logs
                    .Where(x => guids.Contains((Guid)x.Id)).ToListAsync();
            var result = await SearchResult(logsList);
            return result;
        }

        public async Task<IEnumerable<object>> SearchByLocation(LocationSearchDto locationSearchDto)
        {
            var ids = await _context.Locations
                .Where(x =>
                    x.Company.Contains(locationSearchDto.CompanyName) &&
                    x.Building.Contains(locationSearchDto.BuildingName) &&
                    x.City.Contains(locationSearchDto.CityName) &&
                    x.Country!.Contains(locationSearchDto.CountryName))
                .Select(x => x.LocationId)
                .ToListAsync();

            var logsList = await _context.Logs
                    .Where(x => ids.Contains((int)x.LocationId)).ToListAsync();
            var result = await SearchResult(logsList);
            return result;
        }

        private async Task<IEnumerable<object>> SearchResult(List<Log> logs)
        {
            var users = await _context.Users.ToListAsync();
            var locations = await _context.Locations.ToListAsync();
            var result = logs
                    .Join(
                        users,
                        log => log.Id,
                        user => user.Id,
                        (log, user) => new { log, user })
                    .Join(
                        locations,
                        combined => combined.log.LocationId,
                        loc => loc.LocationId,
                        (combined, loc) => new
                        {
                            Date = combined.log.LogDate,
                            combined.user.Id,
                            Forename = combined.user.FirstName,
                            Surname = combined.user.LastName,                            
                            combined.log.Present,
                            loc.Company,
                            loc.Building,
                            loc.City,
                            loc.Country
                        });
            return result;
        }

        public async Task<bool> LocationUserExistById(int id, Guid guid)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(x => x.LocationId == id);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == guid);
            return location != null && user != null;
        }

        public async Task<bool> CreateLog(Log log)
        {
            log.LogId = 0;
            log.LogDate = DateTime.Now;
            await _context.Logs.AddAsync(log);
            return Save().Result;
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }        
    }
}