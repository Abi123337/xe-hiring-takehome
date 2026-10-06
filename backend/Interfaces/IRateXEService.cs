using RateAlerts.Api.Models;

namespace RateAlerts.Api.Interfaces
{
    public interface IRateXEService
    {
        public interface IXeRateService
        {
             Task<XERateResponse> GetRateAsync(
                string baseCurrency,
                string targetCurrency,
                CancellationToken cancellationToken);
        }
    }
}
