using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot;

public class TransferBehaviorTest
{
    [Fact]
    public void ConstuctTransferBehaviorShouldWork()
    {
        byte transferPathKind = (byte)TransferPathKind.ForkToWip;
        byte materialKind = (byte)MaterialKind.EmptyPanelSilo;
        byte panelKind = (byte)PanelKind.Unspecified;
        int behavior = ((byte)transferPathKind << 16) | (materialKind << 8) | panelKind;

        var transferBehavior = SiloTransferBehavior.Make(TransferPathKind.ForkToWip,
            MaterialKind.EmptyPanelSilo,
            PanelKind.Unspecified);

        Assert.Equal(behavior, transferBehavior.Behavior);
        Assert.Equal(TransferPathKind.ForkToWip, transferBehavior.TransferPathKind);
        Assert.Equal(MaterialKind.EmptyPanelSilo, transferBehavior.MaterialKind);
        Assert.Equal(PanelKind.Unspecified, transferBehavior.PanelKind);

        var behavior2 = (SiloTransferBehavior)behavior;
        Assert.True(transferBehavior.Equals(behavior2));
        Assert.Equal(behavior, behavior2);
    }
}
