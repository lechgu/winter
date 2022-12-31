namespace Winter.Shared.Dto;

public class LogRecord
{
    public string Scope { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
}