using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;
using static RateAlerts.Api.Interfaces.IRateXEService;

namespace RateAlerts.Api.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _repository;
        private readonly IXeRateService _rateService;
        private readonly ILogger<AlertService> _logger;


        public AlertService(IAlertRepository repository,
        IXeRateService rateService, ILogger<AlertService> logger)
        {
            _repository = repository;
            _rateService = rateService;
            _logger = logger;

        }
        public async Task<RateAlert> CreateAsync(string userId, CreateRateAlertRequest request)
        {
            var alert = new RateAlert
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BaseCurrency = request.BaseCurrency.ToUpperInvariant(),
                TargetCurrency = request.TargetCurrency.ToUpperInvariant(),
                Threshold = request.Threshold,
                Direction = request.Direction,
                Triggered = false,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(alert);
        }

        public Task<bool> DeleteAsync(Guid id, string userId)
        {
            return _repository.DeleteAsync(id, userId);
        }

        public async Task EvaluateAlertsAsync(CancellationToken cancellationToken)
        {
            var alerts = await _repository.GetActiveAlertsAsync();

            var groupedAlerts = alerts
                .GroupBy(x => new
                {
                    x.BaseCurrency,
                    x.TargetCurrency
                });

            foreach (var group in groupedAlerts)
            {
                cancellationToken.ThrowIfCancellationRequested();

                XERateResponse currentRate = new XERateResponse();

                try
                {
                    currentRate = await _rateService.GetRateAsync(
                        group.Key.BaseCurrency,
                        group.Key.TargetCurrency,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to retrieve rate for {Base}/{Target}",
                        group.Key.BaseCurrency,
                        group.Key.TargetCurrency);

                    continue;
                }

                foreach (var alert in group)
                {
                    if (ShouldTrigger(alert, currentRate.Rates))
                    {
                        alert.Triggered = true;
                        alert.TriggeredAt = DateTime.UtcNow;

                        await _repository.UpdateAsync(alert);

                        _logger.LogInformation(
                            "Alert {AlertId} triggered. {Base}/{Target} = {Rate}",
                            alert.Id,
                            alert.BaseCurrency,
                            alert.TargetCurrency,
                            currentRate);
                    }
                }
            }
        }
        private static bool ShouldTrigger(
       RateAlert alert,
       decimal currentRate)
        {
            return alert.Direction switch
            {
                AlertDirection.Above =>
                    currentRate >= alert.Threshold,

                AlertDirection.Below =>
                    currentRate <= alert.Threshold,

                _ => false
            };
        }
        public Task<IReadOnlyCollection<RateAlert>> GetAllAsync(string userId)
        {
            return _repository.GetAllAsync(userId);
        }
    }
}
