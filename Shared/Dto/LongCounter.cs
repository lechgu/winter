namespace Winter.Shared.Dto;

public class LongCounter
{
    readonly string scope;
    readonly string name;
    readonly long value;
    readonly DateTime timestamp;

    public LongCounter(string source, string name, long value, DateTime timestamp)
    {
        this.scope = source;
        this.name = name;
        this.value = value;
        this.timestamp = timestamp;
    }

    public string Scope { get => scope; }

    public string Name { get => name; }
    public long Value { get => value; }
    public DateTime TimeStamp { get => timestamp; }
}