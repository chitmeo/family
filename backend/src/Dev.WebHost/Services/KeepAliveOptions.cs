namespace Dev.WebHost.Services;

public class KeepAliveOptions
{
    public string Url { get; set; } = string.Empty;
    public int IntervalMinutes { get; set; } = 5;
}
