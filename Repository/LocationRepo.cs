using Demo2CoreAPICrud.Data;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo2CoreAPICrud.Repository
{
    public class LocationRepo : ILocationRepo
    {
        private readonly DataContext _context;

        public LocationRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            return await _context.Locations
                .OrderBy(x => x.Company)
                .ThenBy(x => x.Building).ThenBy(x => x.City).ThenBy(x => x.Country)
                .ToArrayAsync();
        }

        public async Task<Location?> GetLocationById(int locationId)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(x => x.LocationId == locationId);
        }

        public async Task<IEnumerable<Location>?> Search(LocationSearchDto locationSearchDto)
        {
            return await _context.Locations
                .Where(x =>
                    x.Company.Contains(locationSearchDto.CompanyName) &&
                    x.Building.Contains(locationSearchDto.BuildingName) &&
                    x.City.Contains(locationSearchDto.CityName) &&
                    x.Country!.Contains(locationSearchDto.CountryName))
                .OrderBy(x => x.Company)
                .ThenBy(x => x.Building).ThenBy(x => x.City).ThenBy(x => x.Country)
                .ToListAsync();
        }

        public bool LocationExistsByName(LocationDto locationDto)
        {
            var exists = _context.Locations
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.Company.ToLower() == locationDto.Business.Trim().ToLower() &&
                        x.Building.ToLower() == locationDto.Facility.Trim().ToLower() &&
                        x.City.ToLower() == locationDto.Town.Trim().ToLower() &&
                        x.Country!.ToLower() == locationDto.Nation!.Trim().ToLower());

            return exists != null;
        }
        
        public bool LocationExistsById(int locationId)
        {
            var exists = _context.Locations.Find(locationId);
            return exists != null;
        }

        public async Task<bool> CreateLocation(Location location)
        {
            location.LocationId = 0;
            await _context.Locations.AddAsync(location);
            return Save().Result;
        }

        public Task<bool> UpdateLocation(Location location)
        {
            var existingLocation = _context.Locations.Local
                            .SingleOrDefault(u => u.LocationId == location.LocationId);

            if (existingLocation != null)
            {
                _context.Entry(existingLocation).State = EntityState.Detached;
            }
            _context.Locations.Update(location);
            _context.Entry(location).State = EntityState.Modified;
            return Save();
        }

        public Task<bool> DeleteLocation(Location location)
        {
            _context.Locations.Remove(location);
            return Save();
        }        

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }        
    }
}
