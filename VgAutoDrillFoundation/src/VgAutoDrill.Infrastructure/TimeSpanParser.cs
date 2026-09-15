namespace VgAutoDrill.Infrastructure;

public class TimeSpanParser
{
    /// <summary>
    /// parse string as 5ms,5m,5s,5h,5d
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static bool TryParse(string interval, out TimeSpan timeSpan)
    {
        timeSpan = TimeSpan.Zero;

        try
        {
            if (interval.EndsWith("ms", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(interval.Substring(0, interval.Length - 2), out int time))
                {
                    timeSpan = TimeSpan.FromMilliseconds(time);
                }
            }

            if (interval.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(interval.Substring(0, interval.Length - 1), out int time))
                {
                    timeSpan = TimeSpan.FromSeconds(time);
                }
            }

            if (interval.EndsWith("m", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(interval.Substring(0, interval.Length - 1), out int time))
                {
                    timeSpan = TimeSpan.FromMinutes(time);
                }
            }

            if (interval.EndsWith("h", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(interval.Substring(0, interval.Length - 1), out int time))
                {
                    timeSpan = TimeSpan.FromHours(time);
                }
            }

            if (interval.EndsWith("d", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(interval.Substring(0, interval.Length - 1), out int time))
                {
                    timeSpan = TimeSpan.FromDays(time);
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
