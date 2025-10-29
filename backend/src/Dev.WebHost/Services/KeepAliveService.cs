using Microsoft.Extensions.Options;

namespace Dev.WebHost.Services;

public class KeepAliveService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<KeepAliveService> _logger;
    private readonly KeepAliveOptions _options;

    public KeepAliveService(
        IHttpClientFactory httpClientFactory,
        ILogger<KeepAliveService> logger,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = configuration.GetSection("KeepAlive").Get<KeepAliveOptions>() ?? new KeepAliveOptions();
        _logger.LogInformation("KeepAliveService config loaded: Url={url}, Interval={interval}",
            _options.Url, _options.IntervalMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Url))
        {
            _logger.LogError("KeepAlive URL is not configured properly");
            return;
        }

        if (!Uri.TryCreate(_options.Url, UriKind.Absolute, out var uri))
        {
            _logger.LogError("Invalid KeepAlive URL: {url}", _options.Url);
            return;
        }

        _logger.LogInformation("KeepAliveService started. Url={url}, Interval={interval} min",
            _options.Url, _options.IntervalMinutes);

        var interval = TimeSpan.FromMinutes(_options.IntervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(uri, stoppingToken);

                if (response.IsSuccessStatusCode)
                    _logger.LogInformation("KeepAlive OK at {time}", DateTime.UtcNow);
                else
                    _logger.LogWarning("KeepAlive failed ({status}) at {time}",
                        response.StatusCode, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in KeepAlive ping at {time}", DateTime.UtcNow);
            }

            await Task.Delay(interval, stoppingToken);
        }
    }

}
