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

public class PanelListTest
{
    private static readonly DevicePanelChangedResponse success = new DevicePanelChangedResponse { Code = "Success" };

    [Fact]
    public void TestGetLayerPanels()
    {
        var list = new PanelList();
        var panels = list.GetLayerPanels(1);
        Assert.Empty(panels);
    }

    [Fact]
    public void TestGetLayerPanels2()
    {
        var list = new PanelList();
        list.Add(new Panel { PanelCode = "0001" });
        list.Add(new Panel { PanelCode = "0002" });
        var panels = list.GetLayerPanels(0);
        Assert.Equal(2, panels.Count());
    }

    [Fact]
    public void TestGetLayerPanels3()
    {
        var list = Panel.NoSilo.PanelForLayerFirst(6, 18, ProductStatus.EmptyPayload);
        var panels = list.GetLayerPanels(0);
        Assert.Equal(6, panels.Count());
    }

    [Fact]
    public void TestGetPositionPanels()
    {
        var list = new PanelList();
        var panels = list.GetPositionPanels(1);
        Assert.Empty(panels);
    }

    [Fact]
    public void TestGetPositionPanels2()
    {
        var list = new PanelList();
        list.Add(new Panel { PanelCode = "0001" });
        list.Add(new Panel { PanelCode = "0002" });
        var panels = list.GetPositionPanels(1);
        Assert.Equal(2, panels.Count());
    }

    [Fact]
    public void TestGetPositionPanels3()
    {
        var list = Panel.NoSilo.PanelForLayerFirst(6, 18, ProductStatus.EmptyPayload);
        var panels = list.GetPositionPanels(1);
        Assert.Equal(18, panels.Count());
    }

    [Fact]
    public void TestGetPanels()
    {
        var list = new PanelList();
        var panels = list.GetPanels(0, 1);
        Assert.Empty(panels);
    }

    [Fact]
    public void TestGetPanels2()
    {
        var list = new PanelList();
        list.Add(new Panel { PanelCode = "0001" });
        list.Add(new Panel { PanelCode = "0002" });
        var panels = list.GetPanels(0, 1);
        Assert.Equal(2, panels.Count());
    }

    [Fact]
    public void TestGetPanels3()
    {
        var list = Panel.NoSilo.PanelForLayerFirst(6, 18, ProductStatus.EmptyPayload);
        var panels = list.GetPanels(0, 1);
        Assert.Equal(1, panels.Count());
    }

    [Fact]
    public void PanelListCanBeSerialized()
    {
        var list = new PanelList();
        list.Add(new Panel { PanelCode = "0001" });
        list.Add(new Panel { PanelCode = "0002" });
        list.Add(new Panel { PanelCode = "0003" });
        list.Add(new Panel { PanelCode = "0004" });
        list.Add(new Panel { PanelCode = "0005" });

        var json = JsonSerializer.Serialize(list);
        var list2 = JsonSerializer.Deserialize<PanelList>(json);
        var list3 = JsonSerializer.Deserialize<List<Panel>>(json);
        var list4 = PanelList.FromList(list.GetRange(0, 5));
        Assert.True(json.Length > 0);
        Assert.Equal(list2.Count, list.Count);
        Assert.Equal(list3.Count, list.Count);
        Assert.Equal(list.Count, list4.Count);
    }

    [Fact]
    public void PanelList_SummaryPanelInfo_CanReturnValue()
    {
        var list = new PanelList();
        list.Add(new Panel { PanelCode = "0001", SiloCode = "silo01", ItemCode = "", ProductStatus = ProductStatus.EmptySiloBox });
        list.Add(new Panel { PanelCode = "0002", SiloCode = "silo01", ItemCode = "", ProductStatus = ProductStatus.EmptySiloBox });
        list.Add(new Panel { PanelCode = "0003", SiloCode = "silo01", ItemCode = "item01", ProductStatus = ProductStatus.Finished_PIN });
        list.Add(new Panel { PanelCode = "0004", SiloCode = "silo01", ItemCode = "item02", ProductStatus = ProductStatus.Finished_DRILL });
        list.Add(new Panel { PanelCode = "0005", SiloCode = "silo01", ItemCode = "item02", ProductStatus = ProductStatus.Finished_DRILL });

        var summaryInfo = list.SummaryPanelInfo();
        Assert.Equal(true, !string.IsNullOrEmpty(summaryInfo));
    }

    [Fact]
    public void PanelList_SummaryPanelInfo_CanReturnEmpty()
    {
        var list = new PanelList();
        list.Add(new Panel { ProductStatus = ProductStatus.EmptyPayload });
        list.Add(new Panel { ProductStatus = ProductStatus.EmptyPayload });
        list.Add(new Panel { ProductStatus = ProductStatus.EmptyPayload });
        list.Add(new Panel { ProductStatus = ProductStatus.EmptyPayload });
        list.Add(new Panel { ProductStatus = ProductStatus.EmptyPayload });

        var summaryInfo = list.SummaryPanelInfo();
        Assert.Equal(true, string.IsNullOrEmpty(summaryInfo));
    }

    [Fact]
    public void PanelListCollectionChangedShouldWork()
    {
        int items = 0;
        var list = new PanelList();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });
        list.Add(new Panel { PanelCode = "0001" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new Panel { PanelCode = "0002" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new Panel { PanelCode = "0003" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new Panel { PanelCode = "0004" });
        list.RaiseCollectionChangedEvent(string.Empty);
        list.Add(new Panel { PanelCode = "0005" });
        list.RaiseCollectionChangedEvent(string.Empty);

        Thread.Sleep(100);
        Assert.Equal(items, list.Count);
    }

    [Fact]
    public async Task ChangeCollectionSafely()
    {
        int items = 0;
        var list = new PanelList();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        await list.ChangeListSafely("", Task.Run(() =>
        {
            list.AddRange(new[] {
                new Panel { PanelCode = "0001" },
                new Panel { PanelCode = "0002" },
                new Panel { PanelCode = "0003" },
                new Panel { PanelCode = "0004" },
                new Panel { PanelCode = "0005" } });
        }));

        Thread.Sleep(100);
        Assert.Equal(1, items);
    }

    [Fact]
    public async Task MultipleThreadChangeCollectionSafely()
    {
        var testDevice = new TestDevice();
        int items = 0;
        var list = testDevice.panels;
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });
        for (int i = 0; i < 100; i++)
        {
            list.Add(new Panel
            {
                Position = i,
                Barcode = "b" + i.ToString().PadLeft(3, '0'),
            });
        }

        var taskList = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            var task1 = ChangeList(i, list);
            taskList.Add(task1);
        }

        Task.WaitAll(taskList.ToArray());
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i + "_b" + i.ToString().PadLeft(3, '0'), list[i].Barcode);
        }
    }

    private static async Task ChangeList(int position, PanelList list)
    {
        await list.ChangeListSafely("", Task.Run(() =>
        {
            list[position].Barcode = position + "_" + list[position].Barcode;
            Task.Delay(200);
        }));

        //await list.ChangeListSafely("", Task.Factory.StartNew((obj) =>
        //{
        //    list[(int)obj].Barcode = (int)obj + "_" + list[(int)obj].Barcode;
        //}, position));
    }

    [Fact]
    public void CollectionChangedShouldTriggerOnce_WhenPanelListAddRange()
    {
        int items = 0;
        var list = new PanelList();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        list.AddRange(new[] {
            new Panel { PanelCode = "0001" },
            new Panel { PanelCode = "0002" },
            new Panel { PanelCode = "0003" },
            new Panel { PanelCode = "0004" },
            new Panel { PanelCode = "0005" } });
        list.RaiseCollectionChangedEvent(string.Empty);

        Thread.Sleep(100);
        Assert.Equal(1, items);
    }

    [Fact]
    public async void ClearListShouldBeWatched()
    {
        int items = 0;
        var list = new PanelList();
        list.CollectionChanged += (string locationCode) => Task.Run(() => { items++; return success; });

        list.AddRange(new[] {
            new Panel { PanelCode = "0001" },
            new Panel { PanelCode = "0002" },
            new Panel { PanelCode = "0003" },
            new Panel { PanelCode = "0004" },
            new Panel { PanelCode = "0005" } });

        list.RaiseCollectionChangedEvent(string.Empty);
        Thread.Sleep(100);
        Assert.Equal(1, items);

        list.Clear();
        list.RaiseCollectionChangedEvent(string.Empty);
        Thread.Sleep(100);
        Assert.Equal(2, items);

        Assert.Equal(0, list.Count);
    }

    public class TestDevice
    {
        public PanelList panels { get; set; } = new PanelList();

    }
}
