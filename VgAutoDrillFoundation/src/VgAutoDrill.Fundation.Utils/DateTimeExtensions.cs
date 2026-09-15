namespace VgAutoDrill.Fundation.Utils;

public static class DateTimeExtensions
{
    public static long TimeStamp(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
    }
}
