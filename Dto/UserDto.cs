using System.Globalization;
using H = Demo2CoreAPICrud.Helpers.CustomiseString;

namespace Demo2CoreAPICrud.Dto
{
    public class UserDto
    {
        private string _forename = null!;
        private string _surname = null!;


        public Guid UserId { get; set; }

        public string Forename 
        { 
            get => _forename;  
            set => _forename = H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture);            
        }

        public string Surname 
        {
            get => _surname;
            set => _surname = H.CapitalizeFirstLetter(value, CultureInfo.CurrentCulture);
        }        
    }
}
