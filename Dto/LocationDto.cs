using System.Globalization;
using H = Demo2CoreAPICrud.Helpers.CustomiseString;

namespace Demo2CoreAPICrud.Dto
{
    public class LocationDto
    {
        private string _business = null!;
        private string _facility = null!;
        private string _town = null!;
        private string? _nation;

        public int AreaId { get; set; }
        public string Business 
        {
            get => _business;
            set => _business = H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture);
        }

        public string Facility 
        {
            get => _facility;
            set => _facility = H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture);
        }

        public string Town 
        {
            get => _town;
            set => _town = H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture);
        }

        public string? Nation 
        {
            get => _nation;
            set => _nation = (value != null) ? H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture) : value;            
        }
    }
}
