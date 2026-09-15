using Microsoft.Extensions.Logging;

namespace VgDeviceGateway.Devices.Common
{
    public class LoopTask
    {
        private Thread thread;
        public bool _isExcute { get; private set; } = true;
        public bool _isStop { get; private set; } = false;
        protected ILogger Logger { get; private set; }

        public LoopTask(Action action, int period, ILogger logger)
        {
            Logger = logger;
            thread = new Thread(() =>
            {
                while (_isExcute)
                {
                    try
                    {
                        action.Invoke();
                        Thread.Sleep(period);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"LoopTask: {action.Method.Name} Error:{e}");
                    }
                }

                _isExcute = false;
            });

            thread.IsBackground = true;
        }

        public void Stop()
        {
            _isExcute = false;
        }

        public void Start()
        {
            _isExcute = true;
            thread.Start();
        }
    }
}
