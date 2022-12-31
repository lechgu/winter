using Winter.Shared.Dto;

namespace Winter.Frontend.Services;

public record CounterDescription(string Resource, string Scope, string Name);
public class AppState
{
    readonly List<LogRecord> logRecordsCache = new();
    readonly Dictionary<CounterDescription, Counter> countersCache = new();

    public IReadOnlyCollection<LogRecord> LogRecords
    {
        get => logRecordsCache.AsReadOnly();
    }

    public void AddLogRecord(LogRecord record)
    {
        logRecordsCache.Add(record);
    }

    public IReadOnlyCollection<Counter> Counters
    {
        get => countersCache.Values;
    }

    public void UpsertCounter(Counter counter)
    {
        var desc = new CounterDescription(counter.Resource, counter.Scope, counter.Name);
        countersCache[desc] = counter;
    }
}