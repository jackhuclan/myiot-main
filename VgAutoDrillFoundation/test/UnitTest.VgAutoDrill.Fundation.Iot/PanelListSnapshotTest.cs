using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class PanelListSnapshotTest
{
    [Fact]
    public void TestPanelListSnapshotEntryEqual()
    {
        var panelListSnapshotEntry1 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };
        var panelListSnapshotEntry2 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        Assert.Equal(panelListSnapshotEntry1, panelListSnapshotEntry2);
    }

    [Fact]
    public void TestPanelListSnapshotEntryEqualIgnoreCase()
    {
        var panelListSnapshotEntry1 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };
        var panelListSnapshotEntry2 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "ItEm1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        Assert.Equal(panelListSnapshotEntry1, panelListSnapshotEntry2);
    }

    [Fact]
    public void TestPanelListSnapshotEntryNotEqual_WhenItemCodeNotEqual()
    {
        var panelListSnapshotEntry1 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        var panelListSnapshotEntry2 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item2",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        Assert.NotEqual(panelListSnapshotEntry1, panelListSnapshotEntry2);
    }

    [Fact]
    public void TestPanelListSnapshotEntryNotEqual_WhenItemStatusNotEqual()
    {
        var panelListSnapshotEntry1 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        var panelListSnapshotEntry2 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.Finished_UNPIN,
            ItemCount = 1,
        };

        Assert.NotEqual(panelListSnapshotEntry1, panelListSnapshotEntry2);
    }

    [Fact]
    public void TestPanelListSnapshotEntryNotEqual_WhenItemCountNotEqual()
    {
        var panelListSnapshotEntry1 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 1,
        };

        var panelListSnapshotEntry2 = new PanelListSnapshotEntry()
        {
            SiloCode = "400010",
            ItemCode = "item1",
            ItemStatus = ProductStatus.EmptySiloBox,
            ItemCount = 2,
        };

        Assert.NotEqual(panelListSnapshotEntry1, panelListSnapshotEntry2);
    }

    [Fact]
    public void TestPanelListSnapshotEqual()
    {
        var panelList = Panel.HasSilo.NoPanelForSingleSpindle("400010", 1, 0, 50);

        panelList.UpdatePanelInfo(1, new int[] { 1, }, ProductStatus.Finished_PIN, "item1");
        panelList.UpdatePanelInfo(1, new int[] { 2, 3 }, ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1, "item1");
        panelList.UpdatePanelInfo(1, new int[] { 4, 5, 6 }, ProductStatus.Finished_PRE_BUFFER, "item1");
        panelList.UpdatePanelInfo(1, new int[] { 7, 8, 9, 10 }, ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1, "item1");
        panelList.UpdatePanelInfo(1, new int[] { 11, 12, 13, 14, 15 }, ProductStatus.WaitingForDrill, "item1");

        panelList.UpdatePanelInfo(1, new int[] { 16 }, ProductStatus.Finished_DRILL, "item2");
        panelList.UpdatePanelInfo(1, new int[] { 17, 18 }, ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1, "item2");
        panelList.UpdatePanelInfo(1, new int[] { 19, 20, 21 }, ProductStatus.Finished_POST_BUFFER, "item2");
        panelList.UpdatePanelInfo(1, new int[] { 22, 23, 24, 25 }, ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1, "item2");
        panelList.UpdatePanelInfo(1, new int[] { 26, 27, 28, 29, 30, 31 }, ProductStatus.Finished_DRILL, "item2");

        panelList.UpdatePanelInfo(1, new int[] { 32 }, ProductStatus.EmptySiloBox);
        panelList.UpdatePanelInfo(1, new int[] { 33, 34 }, ProductStatus.Finished_UNPIN);

        panelList.UpdatePanelInfo(1, new int[] { 35 }, ProductStatus.EmptyPayload);

        var panelListSnapshot1 = new PanelListSnapshot()
        {
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "item1",
                ItemStatus = ProductStatus.Finished_PIN,
                ItemCount = 15,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "item2",
                ItemStatus = ProductStatus.Finished_DRILL,
                ItemCount = 16,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "",
                ItemStatus = ProductStatus.EmptySiloBox,
                ItemCount = 18,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "",
                ItemStatus = ProductStatus.EmptyPayload,
                ItemCount = 1,
            }
        };

        var panelListSnapshotWithUpperItemCode = new PanelListSnapshot()
        {
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "ITEM1",
                ItemStatus = ProductStatus.Finished_PIN,
                ItemCount = 15,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "ITEM2",
                ItemStatus = ProductStatus.Finished_DRILL,
                ItemCount = 16,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "",
                ItemStatus = ProductStatus.EmptySiloBox,
                ItemCount = 18,
            },
            new PanelListSnapshotEntry()
            {
                SiloCode = "400010",
                ItemCode = "",
                ItemStatus = ProductStatus.EmptyPayload,
                ItemCount = 1,
            }
        };

        var panelListSnapshot2 = panelList.PanelSnapshot;
        Assert.Equal(panelListSnapshot1, panelListSnapshot2);
        Assert.Equal(panelListSnapshotWithUpperItemCode, panelListSnapshot2);
    }
}
