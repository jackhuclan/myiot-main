using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class PanelTest
{
    [Fact]
    public void TestPanelSerialization()
    {
        Panel panel = new Panel()
        {
            PanelCode = "P001",
            SiloCode = "S002"
        };

        var result = JsonSerializer.Serialize(panel);
        Assert.NotNull(result);
    }

    [Fact]
    public void TestPanelIsNull()
    {
        Panel panel = new Panel();
        Assert.True(panel.IsNull());
    }

    [Fact]
    public void TestNoSiloIsNotSame()
    {
        var panels = Panel.NoSilo.PanelForSingleSpindle(1, 0, 18);
        Assert.NotEqual(panels[0], panels[1]);
        Assert.Equal(1, panels[17].Position);
        Assert.Equal(17, panels[17].Layer);
        Assert.Equal(ProductStatus.EmptyPayload, panels[0].ProductStatus);
        Assert.Equal(18, panels.Count);
    }

    [Fact]
    public void TestNoSiloWholeInitialization()
    {
        var panels = Panel.NoSilo.PanelForSpindleFirst(19, 18);
        Assert.Equal(19, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(1, panels[17].Position);
        Assert.Equal(17, panels[17].Layer);
        Assert.Equal(ProductStatus.EmptyPayload, panels[17].ProductStatus);
        Assert.Equal(2, panels[18].Position);
    }

    [Fact]
    public void TestSiloNoPanelIsNotSame()
    {
        var panels = Panel.HasSilo.NoPanelForSpindleFirst("abc", 18, 18);
        Assert.NotEqual(panels[0], panels[1]);
        Assert.Equal(1, panels[17].Position);
        Assert.Equal(17, panels[17].Layer);
        Assert.Equal(ProductStatus.EmptySiloBox, panels[0].ProductStatus);
        Assert.Equal("abc", panels[0].SiloCode);
        Assert.Equal("abc", panels[17].SiloCode);
        Assert.Equal(18, panels.Count);
    }


    [Fact]
    public void TestSiloNoPaneWholeInitialization()
    {
        var panels = Panel.HasSilo.NoPanelForSpindleFirst("abc", 19, 18);
        Assert.Equal(19, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(1, panels[17].Position);
        Assert.Equal(17, panels[17].Layer);
        Assert.Equal(ProductStatus.EmptySiloBox, panels[17].ProductStatus);
        Assert.Equal("abc", panels[0].SiloCode);
        Assert.Equal("abc", panels[18].SiloCode);
        Assert.Equal(2, panels[18].Position);
    }

    [Fact]
    public void TestSiloNoPaneWholeInitialization_forDrill()
    {
        var panels = Panel.HasSilo.NoPanelForLayerFirst("abc", 5, 3);
        Assert.Equal(15, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(2, panels[6].Position);
        Assert.Equal(1, panels[6].Layer);
        Assert.Equal(5, panels[14].Position);
        Assert.Equal(2, panels[14].Layer);
        Assert.Equal(ProductStatus.EmptySiloBox, panels[14].ProductStatus);
        Assert.Equal("abc", panels[0].SiloCode);
        Assert.Equal("abc", panels[5].SiloCode);
        Assert.Equal("abc", panels[10].SiloCode);
        Assert.Equal("abc", panels[14].SiloCode);
    }

    [Fact]
    public void TestSiloNoPaneWholeInitialization_forAgv()
    {
        var panels = Panel.HasSilo.NoPanelForSpindleFirst("abc", 15, 15);
        Assert.Equal(15, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(1, panels[6].Position);
        Assert.Equal(6, panels[6].Layer);
        Assert.Equal(1, panels[14].Position);
        Assert.Equal(14, panels[14].Layer);
        Assert.Equal(ProductStatus.EmptySiloBox, panels[14].ProductStatus);
        Assert.Equal("abc", panels[0].SiloCode);
    }

    [Fact]
    public void TestSiloWithPanelSpindleFirstInitialization()
    {
        var panels = Panel.HasSilo.HasPanelForSpindleFirst("abc", 19, 18, ProductStatus.Finished_DRILL);
        Assert.Equal(19, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(1, panels[17].Position);
        Assert.Equal(17, panels[17].Layer);
        Assert.Equal(ProductStatus.Finished_DRILL, panels[17].ProductStatus);
        Assert.Equal(2, panels[18].Position);
        Assert.Equal("abc", panels[0].SiloCode);
    }

    [Fact]
    public void TestSiloWithPanelLayerFirstInitialization()
    {
        var panels = Panel.HasSilo.HasPanelForLayerFirst("abc", 5, 3, ProductStatus.Finished_DRILL);
        Assert.Equal(15, panels.Count);
        Assert.Equal(1, panels[0].Position);
        Assert.Equal(2, panels[6].Position);
        Assert.Equal(1, panels[6].Layer);
        Assert.Equal(5, panels[14].Position);
        Assert.Equal(2, panels[14].Layer);
        Assert.Equal(ProductStatus.Finished_DRILL, panels[14].ProductStatus);
        Assert.Equal("abc", panels[0].SiloCode);
    }

}
