using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class InteractionBehaviorTest
{
    [Fact]
    public void ConstuctInteractionBehaviorShouldWork()
    {
        ushort mode = (ushort)InteractionMode.Passive;
        ushort deviceKind = (ushort)DeviceKind.CNC84Drill;
        ushort sequence = (ushort)InteractionSequence.LoadThenUnload;
        ushort position = (ushort)InteractionPosition.Front;
        ushort material = (ushort)MaterialKind.Cutter;
        ushort behavior = (ushort)((mode << 15) | (deviceKind << 10) | (sequence << 7) | (position << 4) | material);

        var ib = InteractionBehavior.Make(DeviceKind.CNC84Drill, behavior, InteractionMode.Passive);
        Assert.Equal(behavior, ib.Behavior);
        Assert.Equal(InteractionMode.Passive, ib.InteractionMode);
        Assert.Equal(DeviceKind.CNC84Drill, ib.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, ib.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, ib.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, ib.MaterialKind);
    }

    [Fact]
    public void OverrideInteractionBehaviorShouldWork()
    {
        ushort mode = (ushort)InteractionMode.Passive;
        ushort deviceKind = (ushort)DeviceKind.CNC84Drill;
        ushort sequence = (ushort)InteractionSequence.LoadThenUnload;
        ushort position = (ushort)InteractionPosition.Front;
        ushort material = (ushort)MaterialKind.Cutter;
        ushort behavior = (ushort)((mode << 15) | (deviceKind << 10) | (sequence << 7) | (position << 4) | material);

        var ib = InteractionBehavior.Make(DeviceKind.PublicPanelSiloWIP, behavior, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, ib.InteractionMode);
        Assert.Equal(DeviceKind.PublicPanelSiloWIP, ib.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, ib.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, ib.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, ib.MaterialKind);
    }


    [Fact]
    public void FullConstuctInteractionBehaviorShouldWork()
    {
        var ib = InteractionBehavior.Make(InteractionMode.Active, DeviceKind.PublicPanelSiloWIP, InteractionSequence.LoadThenUnload, InteractionPosition.Front, MaterialKind.Cutter);
        Assert.Equal(InteractionMode.Active, ib.InteractionMode);
        Assert.Equal(DeviceKind.PublicPanelSiloWIP, ib.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, ib.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, ib.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, ib.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork1()
    {
        var RearLoadPanelOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_PANEL_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadPanelOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadPanelOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, RearLoadPanelOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadPanelOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, RearLoadPanelOnly.MaterialKind);

        var RearUnloadPanelOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_PANEL_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadPanelOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadPanelOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, RearUnloadPanelOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadPanelOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, RearUnloadPanelOnly.MaterialKind);

        var RearLoadPanelThenUnloadPanel = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_PANEL_THEN_UNLOAD_PANEL, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadPanelThenUnloadPanel.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadPanelThenUnloadPanel.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, RearLoadPanelThenUnloadPanel.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadPanelThenUnloadPanel.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, RearLoadPanelThenUnloadPanel.MaterialKind);

        var RearUnloadPanelThenLoadPanel = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_PANEL_THEN_LOAD_PANEL, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadPanelThenLoadPanel.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadPanelThenLoadPanel.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, RearUnloadPanelThenLoadPanel.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadPanelThenLoadPanel.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, RearUnloadPanelThenLoadPanel.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork2()
    {
        var FrontLoadPanelOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_PANEL_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadPanelOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadPanelOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, FrontLoadPanelOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadPanelOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, FrontLoadPanelOnly.MaterialKind);

        var FrontUnloadPanelOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_PANEL_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadPanelOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadPanelOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, FrontUnloadPanelOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadPanelOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, FrontUnloadPanelOnly.MaterialKind);

        var FrontLoadPanelThenUnloadPanel = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_PANEL_THEN_UNLOAD_PANEL, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadPanelThenUnloadPanel.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadPanelThenUnloadPanel.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, FrontLoadPanelThenUnloadPanel.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadPanelThenUnloadPanel.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, FrontLoadPanelThenUnloadPanel.MaterialKind);

        var FrontUnloadPanelThenLoadPanel = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_PANEL_THEN_LOAD_PANEL, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadPanelThenLoadPanel.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadPanelThenLoadPanel.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, FrontUnloadPanelThenLoadPanel.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadPanelThenLoadPanel.InteractionPosition);
        Assert.Equal(MaterialKind.Panel, FrontUnloadPanelThenLoadPanel.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork3()
    {
        var RearLoadSiloOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_SILO_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadSiloOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadSiloOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, RearLoadSiloOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadSiloOnly.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, RearLoadSiloOnly.MaterialKind);

        var RearUnloadSiloOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_SILO_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadSiloOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadSiloOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, RearUnloadSiloOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadSiloOnly.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, RearUnloadSiloOnly.MaterialKind);

        var RearLoadSiloThenUnloadSilo = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_SILO_THEN_UNLOAD_SILO, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadSiloThenUnloadSilo.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadSiloThenUnloadSilo.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, RearLoadSiloThenUnloadSilo.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadSiloThenUnloadSilo.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, RearLoadSiloThenUnloadSilo.MaterialKind);

        var RearUnloadSiloThenLoadSilo = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_SILO_THEN_LOAD_SILO, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadSiloThenLoadSilo.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadSiloThenLoadSilo.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, RearUnloadSiloThenLoadSilo.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadSiloThenLoadSilo.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, RearUnloadSiloThenLoadSilo.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork4()
    {
        var RearLoadCutterOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_CUTTER_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadCutterOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadCutterOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, RearLoadCutterOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadCutterOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, RearLoadCutterOnly.MaterialKind);

        var RearUnloadCutterOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_CUTTER_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadCutterOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadCutterOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, RearUnloadCutterOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadCutterOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, RearUnloadCutterOnly.MaterialKind);

        var RearLoadCutterThenUnloadCutter = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_LOAD_CUTTER_THEN_UNLOAD_CUTTER, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearLoadCutterThenUnloadCutter.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearLoadCutterThenUnloadCutter.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, RearLoadCutterThenUnloadCutter.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearLoadCutterThenUnloadCutter.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, RearLoadCutterThenUnloadCutter.MaterialKind);

        var RearUnloadCutterThenLoadCutter = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.REAR_UNLOAD_CUTTER_THEN_LOAD_CUTTER, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, RearUnloadCutterThenLoadCutter.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, RearUnloadCutterThenLoadCutter.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, RearUnloadCutterThenLoadCutter.InteractionSequence);
        Assert.Equal(InteractionPosition.Rear, RearUnloadCutterThenLoadCutter.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, RearUnloadCutterThenLoadCutter.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork5()
    {
        var FrontLoadSiloOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_SILO_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadSiloOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadSiloOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, FrontLoadSiloOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadSiloOnly.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, FrontLoadSiloOnly.MaterialKind);

        var FrontUnloadSiloOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadSiloOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadSiloOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, FrontUnloadSiloOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadSiloOnly.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, FrontUnloadSiloOnly.MaterialKind);

        var FrontLoadSiloThenUnloadSilo = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_SILO_THEN_UNLOAD_SILO, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadSiloThenUnloadSilo.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadSiloThenUnloadSilo.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, FrontLoadSiloThenUnloadSilo.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadSiloThenUnloadSilo.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, FrontLoadSiloThenUnloadSilo.MaterialKind);

        var FrontUnloadSiloThenLoadSilo = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_SILO_THEN_LOAD_SILO, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadSiloThenLoadSilo.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadSiloThenLoadSilo.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, FrontUnloadSiloThenLoadSilo.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadSiloThenLoadSilo.InteractionPosition);
        Assert.Equal(MaterialKind.PanelSilo, FrontUnloadSiloThenLoadSilo.MaterialKind);
    }

    [Fact]
    public void DefaultInteractionBehaviorShouldWork6()
    {
        var FrontLoadCutterOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_CUTTER_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadCutterOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadCutterOnly.DeviceKind);
        Assert.Equal(InteractionSequence.LoadOnly, FrontLoadCutterOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadCutterOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, FrontLoadCutterOnly.MaterialKind);

        var FrontUnloadCutterOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_CUTTER_ONLY, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadCutterOnly.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadCutterOnly.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadOnly, FrontUnloadCutterOnly.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadCutterOnly.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, FrontUnloadCutterOnly.MaterialKind);

        var FrontLoadCutterThenUnloadCutter = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_CUTTER_THEN_UNLOAD_CUTTER, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontLoadCutterThenUnloadCutter.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontLoadCutterThenUnloadCutter.DeviceKind);
        Assert.Equal(InteractionSequence.LoadThenUnload, FrontLoadCutterThenUnloadCutter.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontLoadCutterThenUnloadCutter.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, FrontLoadCutterThenUnloadCutter.MaterialKind);

        var FrontUnloadCutterThenLoadCutter = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_UNLOAD_CUTTER_THEN_LOAD_CUTTER, InteractionMode.Active);
        Assert.Equal(InteractionMode.Active, FrontUnloadCutterThenLoadCutter.InteractionMode);
        Assert.Equal(DeviceKind.Unknown, FrontUnloadCutterThenLoadCutter.DeviceKind);
        Assert.Equal(InteractionSequence.UnloadThenLoad, FrontUnloadCutterThenLoadCutter.InteractionSequence);
        Assert.Equal(InteractionPosition.Front, FrontUnloadCutterThenLoadCutter.InteractionPosition);
        Assert.Equal(MaterialKind.Cutter, FrontUnloadCutterThenLoadCutter.MaterialKind);
    }

    [Fact]
    public void TestInteractionBehaviorCastType()
    {
        var FrontLoadCutterOnly = InteractionBehavior.Make(DeviceKind.Unknown, InteractionBehavior.FRONT_LOAD_CUTTER_ONLY);
        ushort a = FrontLoadCutterOnly;
        var behavior = (InteractionBehavior)a;
        Assert.NotNull(behavior);
        Assert.Equal(FrontLoadCutterOnly, behavior);
    }
}
