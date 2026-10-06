namespace RateAlerts.Api.Models
{
    public class CreateRateAlertRequest
    {
        public string BaseCurrency { get; set; } = string.Empty;

        public string TargetCurrency { get; set; } = string.Empty;

        public string Pair {  get; set; } = string.Empty;
        public decimal Threshold { get; set; }

        public AlertDirection Direction { get; set; }
    }
    public enum AlertDirection
    {
        Above,
        Below
    }

}
