using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Store;

internal class DefaultDeviceStore : IDeviceStore
{
    private readonly DeviceStoreOptions _deviceStoreOptions;
    private readonly ILogger<DefaultDeviceStore> _logger;
    private readonly ReaderWriterLockSlim _scheduleTaskLock = new();
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public DefaultDeviceStore(IOptions<DeviceStoreOptions> options,
        ILoggerFactory loggerFactory)
    {
        _deviceStoreOptions = options.Value;
        _logger = loggerFactory.CreateLogger<DefaultDeviceStore>();
    }

    public Task LoadPayloadPanels(Device device)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.PanelSaveEnabled)
                return;

            LoadList(device.DeviceId, Path.Combine(_deviceStoreOptions.SavePath, $"{device.DeviceId}_pnl.list"), device.PayloadPanels);
        });
    }

    public Task LoadPayloadPanels(string locationCode, PanelList panels)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.PanelSaveEnabled)
                return;

            var filePath = Path.Combine(_deviceStoreOptions.SavePath, $"{locationCode}_pnl.list");
            LoadList(locationCode, filePath, panels);
        });
    }

    public Task LoadPayloadCutterTrays(Device device)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.CutterSaveEnabled)
                return;

            LoadList(device.DeviceId, Path.Combine(_deviceStoreOptions.SavePath, $"{device.DeviceId}_cut.list"), device.PayloadCutterTrays);
        });
    }

    public Task LoadScheduleTasks(Device device)
    {
        return LoadScheduleTasks(Path.Combine(_deviceStoreOptions.SavePath, $"{device.DeviceId}_schetasks.list"), device.SchedulingTasks);
    }

    public Task SavePayloadPanels(Device device)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.PanelSaveEnabled)
                return;

            SaveList(device.DeviceId, Path.Combine(_deviceStoreOptions.SavePath, $"{device.DeviceId}_pnl.list"), device.PayloadPanels);
        });
    }

    public Task SavePayloadPanels(PanelList panels)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled
            || !_deviceStoreOptions.PanelSaveEnabled
            || string.IsNullOrEmpty(panels.LocationCode))
            {
                _logger.LogWarning($"SavePayloadPanels保存板料失败,_deviceStoreOptions.Enabled={_deviceStoreOptions.Enabled}," +
                    $"_deviceStoreOptions.PanelSaveEnabled={_deviceStoreOptions.PanelSaveEnabled}," +
                    $"panels.LocationCode={panels.LocationCode}");
                return;
            }

            SaveList(panels.LocationCode, Path.Combine(_deviceStoreOptions.SavePath, $"{panels.LocationCode}_pnl.list"), panels);
        });
    }

    public Task SavePayloadCutterTrays(Device device)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.CutterSaveEnabled)
                return;

            SaveList(device.DeviceId, Path.Combine(_deviceStoreOptions.SavePath, $"{device.DeviceId}_cut.list"), device.PayloadCutterTrays);
        });
    }

    private async Task LoadScheduleTasks(string filePath, PriorityQueue<DeviceServiceInvokeRequest, int> schedulingTasks)
    {
        await Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled
            || !_deviceStoreOptions.TrackScheduleTasks
            || !File.Exists(filePath))
                return;

            _scheduleTaskLock.EnterReadLock();

            try
            {
                string json = File.ReadAllText(filePath);
                var list = JsonSerializer.Deserialize<List<DeviceServiceInvokeRequest>>(json);
                if (list != null && list.Any())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        schedulingTasks.Enqueue(list[i], Device.DEFAULT_MAX_TASKS_LIMIT / 2 + i);
                    }
                }
            }
            finally
            {
                if (_scheduleTaskLock.IsReadLockHeld)
                    _scheduleTaskLock.ExitReadLock();
            }
        });
    }

    public Task SaveScheduleTasks(Device device)
    {
        return Task.Run(() =>
        {
            if (!_deviceStoreOptions.Enabled || !_deviceStoreOptions.TrackScheduleTasks)
                return;

            _scheduleTaskLock.EnterWriteLock();

            try
            {
                var list = new List<DeviceServiceInvokeRequest>();
                while (device.SchedulingTasks.Count > 0)
                {
                    list.Add(device.SchedulingTasks.Dequeue());
                }

                string json = JsonSerializer.Serialize(list, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _deviceStoreOptions.SavePath, $"{device.DeviceId}_schetasks.list"), json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if (_scheduleTaskLock.IsWriteLockHeld)
                    _scheduleTaskLock.ExitWriteLock();
            }
        });
    }

    private void LoadList<T, TResult>(string locationCode, string filePath, ObservableList<T, TResult> observableList)
    {
        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
        if (!File.Exists(filePath)) return;

        try
        {
            _autoResetEvent.WaitOne();

            string json = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<T>>(json);
            if (list != null && list.Any())
            {
                observableList.AddRange(list);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            _autoResetEvent.Set();
        }
    }

    private void SaveList<T, TResult>(string locationCode, string filePath, ObservableList<T, TResult> observableList)
    {
        try
        {
            _autoResetEvent.WaitOne();

            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            string json = JsonSerializer.Serialize(observableList, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            _autoResetEvent.Set();
        }
    }
}
