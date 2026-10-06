using RateAlerts.Api.Interfaces;
using static RateAlerts.Api.Interfaces.IRateXEService;

namespace RateAlerts.Api.Services
{
    public class XEService : IXeRateService
    {
        public Task<decimal> GetRateAsync(string baseCurrency, string targetCurrency, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
