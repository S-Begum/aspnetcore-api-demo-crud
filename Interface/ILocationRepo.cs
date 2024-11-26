using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Models;

namespace Demo2CoreAPICrud.Interface
{
    public interface ILocationRepo
    {
        Task<IEnumerable<Location>> GetAllLocations();
        Task<Location?> GetLocationById(int locationId);
        Task<IEnumerable<Location>?> Search(LocationSearchDto locationSearchDto);
        bool LocationExistsByName(LocationDto locationDto);
        bool LocationExistsById(int locationId);        
        Task<bool> CreateLocation(Location location);
        Task<bool> UpdateLocation(Location location);
        Task<bool> DeleteLocation(Location location);
        Task<bool> Save();
    }
}
