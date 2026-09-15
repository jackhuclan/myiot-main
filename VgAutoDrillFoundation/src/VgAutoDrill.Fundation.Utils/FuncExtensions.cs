namespace VgAutoDrill.Fundation.Utils;

public static class FuncExtensions
{
    public static async Task Retry(this Func<bool> func, int times)
    {
        await func.Retry(times, TimeSpan.Zero);
    }

    public static async Task Retry(this Func<bool> func, int times, TimeSpan interval)
    {
        if (times < 0) throw new ArgumentOutOfRangeException("times should greater than zero");
        if (times == 0 || times == 1)
        {
            var taskResult = await Task.Run(func);

            if (taskResult)
                return;
        }
        else
        {
            while (times > 0)
            {
                await Task.Delay(interval);
                times--;
                var taskResult = await Task.Run(func);

                if (taskResult)
                    break;
            }
        }
    }
}
