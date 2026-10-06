namespace RateAlerts.Api.Interfaces
{
    public interface IRateXEService
    {
        public interface IXeRateService
        {
            Task<decimal> GetRateAsync(
                string baseCurrency,
                string targetCurrency,
                CancellationToken cancellationToken);
        }
    }
}
