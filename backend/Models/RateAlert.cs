namespace RateAlerts.Api.Models
{
    public class RateAlert
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string BaseCurrency { get; set; } = string.Empty;

        public string TargetCurrency { get; set; } = string.Empty;

        public decimal Threshold { get; set; }

        public AlertDirection Direction { get; set; }

        public bool Triggered { get; set; }

        public DateTime? TriggeredAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
