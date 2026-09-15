namespace VgAutoDrill.Central.Core;

public interface IClickHouseLogger
{
    void LogDebug(string? message, params object?[] args);
    void LogError(string? message, params object?[] args);
    void LogInformation(string? message, params object?[] args);
    void LogWarning(string? message, params object?[] args);
}