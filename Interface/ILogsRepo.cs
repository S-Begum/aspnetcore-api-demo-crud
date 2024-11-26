using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Models;

namespace Demo2CoreAPICrud.Interface
{
    public interface ILogsRepo
    {
        Task<IEnumerable<object>> GetAll();
        Task<object> GetById(int id);
        Task<IEnumerable<object>> GetByUserId(string id);
        Task<IEnumerable<object>> Search(LogsSearchDto logsSearchDto);
        Task<IEnumerable<object>> SearchByUser(UserSearchDto userSearchDto);
        Task<IEnumerable<object>> SearchByLocation(LocationSearchDto locationSearchDto);
        Task<bool> LocationUserExistById(int id, Guid guid); 
        Task<bool> CreateLog(Log log);
        Task<bool> Save();
    }
}
