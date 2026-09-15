using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class CutterTraysTest
{
    private readonly static DeviceCutterTrayChangedResponse success = new DeviceCutterTrayChangedResponse { Code = "Success" };

    [Fact]
    public void CutterTraysCanBeSerialized()
    {
        var list = new CutterTrays();
        list.Add(new CutterTray { TrayCode = "0001" });
        list.Add(new CutterTray { TrayCode = "0002" });
        list.Add(new CutterTray { TrayCode = "0003" });
        list.Add(new CutterTray { TrayCode = "0004" });
        list.Add(new CutterTray { TrayCode = "0005" });

        var json = JsonSerializer.Serialize(list);
        var list2 = JsonSerializer.Deserialize<CutterTrays>(json);
        var list3 = JsonSerializer.Deserialize<List<CutterTray>>(json);
        Assert.True(json.Length > 0);
        Assert.Equal(list2.Count, list.Count);
        Assert.Equal(list3.Count, list.Count);
    }

    [Fact]
    public void CutterTraysCollectionChangedShouldWork()
    {
        int items = 0;
        var list = new CutterTrays();
        list.CollectionChanged += (string locationCode) => Task.Run<DeviceCutterTrayChangedResponse>(() => { items++; return success; });
        list.Add(new CutterTray { TrayCode = "0001" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new CutterTray { TrayCode = "0002" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new CutterTray { TrayCode = "0003" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new CutterTray { TrayCode = "0004" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new CutterTray { TrayCode = "0005" });
        list.RaiseCollectionChangedEvent(string.Empty);

        Thread.Sleep(100);
        Assert.Equal(items, list.Count);
    }

    [Fact]
    public async Task ChangeCollectionSafely()
    {
        int items = 0;
        var list = new CutterTrays();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        await list.ChangeListSafely("", Task.Run(() =>
          {
              list.AddRange(new[] {
                new CutterTray { TrayCode = "0001" },
                new CutterTray { TrayCode = "0002" },
                new CutterTray { TrayCode = "0003" },
                new CutterTray { TrayCode = "0004" },
                new CutterTray { TrayCode = "0005" } });
          }));

        Thread.Sleep(100);
        Assert.Equal(1, items);
    }

    [Fact]
    public void CollectionChangedShouldTriggerOnce_WhenCutterTraysAddRange()
    {
        int items = 0;
        var list = new CutterTrays();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        list.AddRange(new[] {
            new CutterTray { TrayCode = "0001" },
            new CutterTray { TrayCode = "0002" },
            new CutterTray { TrayCode = "0003" },
            new CutterTray { TrayCode = "0004" },
            new CutterTray { TrayCode = "0005" } });
        list.RaiseCollectionChangedEvent("");

        Thread.Sleep(100);
        Assert.Equal(1, items);
    }

    [Fact]
    public void ClearListShouldBeWatched()
    {
        int items = 0;
        var list = new CutterTrays();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        list.AddRange(new[] {
            new CutterTray { TrayCode = "0001" },
            new CutterTray { TrayCode = "0002" },
            new CutterTray { TrayCode = "0003" },
            new CutterTray { TrayCode = "0004" },
            new CutterTray { TrayCode = "0005" } });

        list.RaiseCollectionChangedEvent(string.Empty);
        Thread.Sleep(100);
        Assert.Equal(1, items);

        list.Clear();
        list.RaiseCollectionChangedEvent(string.Empty);
        Thread.Sleep(100);
        Assert.Equal(2, items);

        Assert.Equal(0, list.Count);
    }
}
