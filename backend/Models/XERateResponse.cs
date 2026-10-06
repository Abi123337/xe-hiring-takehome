namespace RateAlerts.Api.Models
{
    public class XERateResponse
    {
        public string Pair { get; set; } = string.Empty;
        public decimal Rates { get; set; }
        public string TimeStamp { get; set; } = string.Empty;

        public static implicit operator decimal(XERateResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
