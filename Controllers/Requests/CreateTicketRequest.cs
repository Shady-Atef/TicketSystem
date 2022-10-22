namespace TicketSystem.Controllers.Requests
{
    public class CreateTicketRequest
    {
        public string? PhoneNo { get; private set; }
        public int GovId { get; private set; }
        public int CityID { get; private set; }
        public int DistrictId { get; private set; }
    }
}