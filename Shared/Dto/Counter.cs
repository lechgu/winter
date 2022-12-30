namespace Winter.Shared.Dto;

public class Counter
{
    public string Name { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public long Value { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}