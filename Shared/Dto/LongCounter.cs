namespace Winter.Shared.Dto;

public class LongCounter
{

    public string Scope { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long Value { get; set; }
    public DateTime TimeStamp { get; set; }

}