namespace Demo2CoreAPICrud.Dto
{
    public class LogsDto
    {
        public int LogNumber { get; set; }

        public DateTime? DateLogged { get; set; }

        public Guid UserId { get; set; }

        public bool UserPresent { get; set; }

        public int LocationNumber { get; set; }


    }
}
