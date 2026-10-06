using Microsoft.Extensions.Logging;
using Moq;
using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;
using RateAlerts.Api.Services;
using static RateAlerts.Api.Interfaces.IRateXEService;

namespace RateAlertsTests
{
    public class RateAlertsTests
    {
        private readonly Mock<IAlertRepository> _repositoryMock;

        private readonly Mock<IXeRateService> _xeRateServiceMock;
        private readonly Mock<ILogger<AlertService>> _loggerMock;
        private readonly AlertService _service;
        public RateAlertsTests()
        {
            _repositoryMock = new Mock<IAlertRepository>();
            _xeRateServiceMock = new Mock<IXeRateService>();
            _loggerMock = new Mock<ILogger<AlertService>>();
            _service = new AlertService(_repositoryMock.Object, _xeRateServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task CreateAsync_ShouldCreateAlert()
        { // Arrange
            var request = new CreateRateAlertRequest { BaseCurrency = "GBP", TargetCurrency = "CAD", Threshold = 1.84m, Direction = AlertDirection.Above };
            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<RateAlert>())).ReturnsAsync((RateAlert alert) => alert); 
            // Act
            var result = await _service.CreateAsync( "user-1", request); 
            // Assert
            Assert.NotEqual(Guid.Empty, result.Id); Assert.Equal("user-1", result.UserId); Assert.Equal("GBP", result.BaseCurrency); Assert.Equal("CAD", result.TargetCurrency); Assert.Equal(1.84m, result.Threshold); Assert.Equal(AlertDirection.Above, result.Direction); Assert.False(result.Triggered); Assert.Null(result.TriggeredAt); _repositoryMock.Verify( x => x.AddAsync(It.IsAny<RateAlert>()), Times.Once); 
        }
        

    [Fact] 
        public async Task CreateAsync_ShouldRejectInvalidCurrencyCode() 
        { 
            var request = new CreateRateAlertRequest { BaseCurrency = "GB", TargetCurrency = "CAD", Threshold = 1.84m, Direction = AlertDirection.Above };
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync("user-1", request)); 
            Assert.Contains("three-letter ISO codes", exception.Message); 
        }

        [Fact]
        public async Task EvaluateAlertsAsync_ShouldTriggerAboveAlert_WhenRateIsAboveThreshold()
        { // Arrange
          var alert = CreateAlert( baseCurrency: "GBP", targetCurrency: "CAD", threshold: 1.84m, direction: AlertDirection.Above); 
            var XERateResponse = new XERateResponse { Pair = "GBP/CAD", Rates = 1.85m, TimeStamp = DateTime.Now.ToString() };
            _repositoryMock .Setup(x => x.GetActiveAlertsAsync()) .ReturnsAsync(new List<RateAlert> { alert }); 
            _xeRateServiceMock .Setup(x => x.GetRateAsync( "GBP", "CAD", It.IsAny<CancellationToken>())) .ReturnsAsync(XERateResponse); 
            // Act
            await _service.EvaluateAlertsAsync( CancellationToken.None); 
            // Assert
            Assert.True(alert.Triggered); 
            Assert.NotNull(alert.TriggeredAt); _repositoryMock.Verify( x => x.UpdateAsync(alert), Times.Once); 
        } 
        
        [Fact]
        public async Task EvaluateAlertsAsync_ShouldNotTriggerAboveAlert_WhenRateIsBelowThreshold()
        { // Arrange
          var alert = CreateAlert( baseCurrency: "GBP", targetCurrency: "CAD", threshold: 1.84m, direction: AlertDirection.Above); 
            _repositoryMock .Setup(x => x.GetActiveAlertsAsync()) .ReturnsAsync(new List<RateAlert> { alert });
            var XERateResponse = new XERateResponse { Pair = "GBP/CAD", Rates = 1.83m, TimeStamp = DateTime.Now.ToString() };

            _xeRateServiceMock.Setup(x => x.GetRateAsync( "GBP", "CAD", It.IsAny<CancellationToken>())) .ReturnsAsync(XERateResponse); 
            // Act
            await _service.EvaluateAlertsAsync( CancellationToken.None); 
            // Assert
            Assert.False(alert.Triggered); 
            Assert.Null(alert.TriggeredAt); 
            _repositoryMock.Verify( x => x.UpdateAsync(It.IsAny<RateAlert>()), Times.Never);
        }

        private static RateAlert CreateAlert(
    AlertDirection direction = AlertDirection.Above,
    decimal threshold = 1.84m,
    string baseCurrency = "GBP",
    string targetCurrency = "CAD",
    bool triggered = false)
        {
            return new RateAlert
            {
                Id = Guid.NewGuid(),
                UserId = "test-user",
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                Threshold = threshold,
                Direction = direction,
                Triggered = triggered,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
