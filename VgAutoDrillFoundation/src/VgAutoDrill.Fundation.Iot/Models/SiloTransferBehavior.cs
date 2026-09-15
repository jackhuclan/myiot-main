namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 转移行为
///
/// use binary int to indicate transfer behavior between devices
/// 000000000|00000000|00000000|00000000
/// 0: TransferPathKind,2^8 = 256
/// 1: MaterialKind,2^8 = 256
/// 2: PanelKind,2^8 = 256
/// </summary>
public class SiloTransferBehavior : IEquatable<SiloTransferBehavior>
{
    public TransferPathKind TransferPathKind { get; set; }
    public MaterialKind MaterialKind { get; }
    public PanelKind PanelKind { get; }

    public static SiloTransferBehavior Noop = new SiloTransferBehavior(0);

    /// <summary>
    /// 行为名称
    /// </summary>
    public static Dictionary<int, string> BehaviorNames = new Dictionary<int, string>()
    {
        [UNDRILLED_FROM_OUTSIDE_TO_FORK] = "生料从外部到中转位",
        [UNDRILLED_FROM_OUTSIDE_TO_WIP] = "生料从外部到线边仓",
        [UNDRILLED_FROM_WIP_TO_OUTSIDE] = "生料从线边仓到外部",
        [DRILLED_FROM_OUTSIDE_TO_WIP] = "熟料从外部到线边仓",
        [DRILLED_FROM_WIP_TO_OUTSIDE] = "熟料从线边仓到外部",
        [DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN] = "熟料从线边仓到外部退pin",
        [DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN] = "熟料从中转位到外部退pin线",
        [FIRST_DRILLED_FROM_OUTSIDE_TO_WIP] = "首件从外部到线边仓",
        [FIRST_DRILLED_FROM_WIP_TO_OUTSIDE] = "首件从线边仓到外部",
        [FIRST_DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN] = "首件从线边仓到外部退pin线",
        [FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN] = "首件从中转位到外部退pin线",
        [EMPTY_BOX_FROM_OUTSIDE_TO_WIP] = "空料仓从外部到线边仓",
        [EMPTY_BOX_FROM_WIP_TO_OUTSIDE] = "空料仓从线边仓到外部",
        [EMPTY_BOX_FROM_WIP_TO_OUTSIDE_UNPIN] = "空料仓从线边仓到外部退pin线",
        [EMPTY_BOX_FROM_FORK_TO_OUTSIDE] = "空料仓从中转位到外部",
        [EMPTY_BOX_FROM_OUTSIDE_TO_FORK] = "空料仓从外部到中转位",
        [UNDRILLED_FROM_FORK_TO_OUTSIDE] = "生料从中转位到外部",
        [UNDRILLED_FROM_FORK_TO_WIP] = "生料从中转位到线边仓",
        [UNDRILLED_FROM_WIP_TO_FORK] = "生料从线边仓到中转位",
        [EMPTY_BOX_FROM_WIP_TO_FORK] = "空料仓从线边仓到中转位",
        [EMPTY_BOX_FROM_FORK_TO_WIP] = "空料仓从中转位到线边仓",
        [FIRST_DRILLED_FROM_FORK_TO_WIP] = "首件从中转位到线边仓",
        [FIRST_DRILLED_FROM_WIP_TO_FORK] = "首件从线边仓到中转位",
        [DRILLED_FROM_FORK_TO_WIP] = "熟料从中转位到线边仓",
        [DRILLED_FROM_WIP_TO_FORK] = "熟料从线边仓到中转位",
        [EMPTY_BOX_FROM_PIN_TO_FORK] = "空料仓从PIN到中转位",
        [EMPTY_BOX_FROM_FORK_TO_PIN] = "空料仓从中转位到PIN",
        [UNDRILLED_FROM_PIN_TO_FORK] = "生料从PIN到中转位",
        [EMPTY_BOX_FROM_UNPIN_TO_FORK] = "空料仓从退PIN到中转位",
        [DRILLED_FROM_FORK_TO_UNPIN] = "熟料从中转位到退PIN",
        [DRILLED_FROM_UNPIN_TO_FORK] = "熟料从退PIN到中转位",
        [FIRST_DRILLED_FROM_UNPIN_TO_FORK] = "首件从退PIN到中转位",
        [EMPTY_BOX_FROM_WIP_TO_PIN] = "空料仓从线边仓到PIN",
        [UNDRILLED_FROM_PIN_TO_WIP] = "生料从PIN到线边仓",
        [EMPTY_BOX_FROM_UNPIN_TO_WIP] = "空料仓从退pin线到线边仓",
        [DRILLED_FROM_WIP_TO_UNPIN] = "熟料从线边仓到退pin线",
    };

    /// <summary>
    /// 生料从外部到中转位
    /// </summary>
    public const int UNDRILLED_FROM_OUTSIDE_TO_FORK = ((byte)TransferPathKind.OutsideToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 生料从外部到线边仓
    /// </summary>
    public const int UNDRILLED_FROM_OUTSIDE_TO_WIP = ((byte)TransferPathKind.OutsideToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 生料从线边仓到外部
    /// </summary>
    public const int UNDRILLED_FROM_WIP_TO_OUTSIDE = ((byte)TransferPathKind.WipToOutside << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 熟料从外部到线边仓
    /// </summary>
    public const int DRILLED_FROM_OUTSIDE_TO_WIP = ((byte)TransferPathKind.OutsideToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 熟料从线边仓到外部
    /// </summary>
    public const int DRILLED_FROM_WIP_TO_OUTSIDE = ((byte)TransferPathKind.WipToOutside << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 熟料从线边仓到外部退pin
    /// </summary>
    public const int DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN = ((byte)TransferPathKind.WipToOutsideUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 熟料从中转位到外部退pin线
    /// </summary>
    public const int DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN = ((byte)TransferPathKind.ForkToOutsideUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 首件从外部到线边仓
    /// </summary>
    public const int FIRST_DRILLED_FROM_OUTSIDE_TO_WIP = ((byte)TransferPathKind.OutsideToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 首件从线边仓到外部
    /// </summary>
    public const int FIRST_DRILLED_FROM_WIP_TO_OUTSIDE = ((byte)TransferPathKind.WipToOutside << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 首件从线边仓到外部退pin线
    /// </summary>
    public const int FIRST_DRILLED_FROM_WIP_TO_OUTSIDE_UNPIN = ((byte)TransferPathKind.WipToOutsideUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 首件从中转位到外部退pin线
    /// </summary>
    public const int FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN = ((byte)TransferPathKind.ForkToOutsideUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 空料仓从外部到线边仓
    /// </summary>
    public const int EMPTY_BOX_FROM_OUTSIDE_TO_WIP = ((byte)TransferPathKind.OutsideToWip << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从线边仓到外部
    /// </summary>
    public const int EMPTY_BOX_FROM_WIP_TO_OUTSIDE = ((byte)TransferPathKind.WipToOutside << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从线边仓到外部退pin线
    /// </summary>
    public const int EMPTY_BOX_FROM_WIP_TO_OUTSIDE_UNPIN = ((byte)TransferPathKind.WipToOutsideUnPin << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从中转位到外部
    /// </summary>
    public const int EMPTY_BOX_FROM_FORK_TO_OUTSIDE = ((byte)TransferPathKind.ForkToOutside << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从外部到中转位
    /// </summary>
    public const int EMPTY_BOX_FROM_OUTSIDE_TO_FORK = ((byte)TransferPathKind.OutsideToFork << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 生料从中转位到外部
    /// </summary>
    public const int UNDRILLED_FROM_FORK_TO_OUTSIDE = ((byte)TransferPathKind.ForkToOutside << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 生料从中转位到线边仓
    /// </summary>
    public const int UNDRILLED_FROM_FORK_TO_WIP = ((byte)TransferPathKind.ForkToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 生料从线边仓到中转位
    /// </summary>
    public const int UNDRILLED_FROM_WIP_TO_FORK = ((byte)TransferPathKind.WipToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 空料仓从线边仓到中转位
    /// </summary>
    public const int EMPTY_BOX_FROM_WIP_TO_FORK = ((byte)TransferPathKind.WipToFork << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从中转位到线边仓
    /// </summary>
    public const int EMPTY_BOX_FROM_FORK_TO_WIP = ((byte)TransferPathKind.ForkToWip << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 首件从中转位到线边仓
    /// </summary>
    public const int FIRST_DRILLED_FROM_FORK_TO_WIP = ((byte)TransferPathKind.ForkToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 首件从线边仓到中转位
    /// </summary>
    public const int FIRST_DRILLED_FROM_WIP_TO_FORK = ((byte)TransferPathKind.WipToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 熟料从中转位到线边仓
    /// </summary>
    public const int DRILLED_FROM_FORK_TO_WIP = ((byte)TransferPathKind.ForkToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 熟料从线边仓到中转位
    /// </summary>
    public const int DRILLED_FROM_WIP_TO_FORK = ((byte)TransferPathKind.WipToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 空料仓从PIN到中转位
    /// </summary>
    public const int EMPTY_BOX_FROM_PIN_TO_FORK = ((byte)TransferPathKind.PinToFork << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 空料仓从中转位到PIN
    /// </summary>
    public const int EMPTY_BOX_FROM_FORK_TO_PIN = ((byte)TransferPathKind.ForkToPin << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 生料从PIN到中转位
    /// </summary>
    public const int UNDRILLED_FROM_PIN_TO_FORK = ((byte)TransferPathKind.PinToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 空料仓从退PIN到中转位
    /// </summary>
    public const int EMPTY_BOX_FROM_UNPIN_TO_FORK = ((byte)TransferPathKind.UnPinToFork << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 熟料从中转位到退PIN
    /// </summary>
    public const int DRILLED_FROM_FORK_TO_UNPIN = ((byte)TransferPathKind.ForkToUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 熟料从退PIN到中转位
    /// </summary>
    public const int DRILLED_FROM_UNPIN_TO_FORK = ((byte)TransferPathKind.UnPinToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    /// <summary>
    /// 首件从退PIN到中转位
    /// </summary>
    public const int FIRST_DRILLED_FROM_UNPIN_TO_FORK = ((byte)TransferPathKind.UnPinToFork << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.FirstDrilled;

    /// <summary>
    /// 空料仓从线边仓到PIN
    /// </summary>
    public const int EMPTY_BOX_FROM_WIP_TO_PIN = ((byte)TransferPathKind.WipToPin << 16) | ((byte)MaterialKind.EmptyPanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 生料从PIN到线边仓
    /// </summary>
    public const int UNDRILLED_FROM_PIN_TO_WIP = ((byte)TransferPathKind.PinToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Undrilled;

    /// <summary>
    /// 空料仓从退pin线到线边仓
    /// </summary>
    public const int EMPTY_BOX_FROM_UNPIN_TO_WIP = ((byte)TransferPathKind.UnPinToWip << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Unspecified;

    /// <summary>
    /// 熟料从线边仓到退pin线
    /// </summary>
    public const int DRILLED_FROM_WIP_TO_UNPIN = ((byte)TransferPathKind.WipToUnPin << 16) | ((byte)MaterialKind.PanelSilo << 8) | (byte)PanelKind.Drilled;

    private SiloTransferBehavior(int behavior)
    {
        TransferPathKind = (TransferPathKind)((0xFF0000 & behavior) >> 16);
        MaterialKind = (MaterialKind)((0xFF00 & behavior) >> 8);
        PanelKind = (PanelKind)(0xFF & behavior);
        Behavior = behavior;
    }

    public static SiloTransferBehavior Make(TransferPathKind transferPathKind,
        MaterialKind materialKind,
        PanelKind panelKind)
    {
        return new SiloTransferBehavior(((byte)transferPathKind << 16) | ((byte)materialKind << 8) | (byte)panelKind);
    }

    public int Behavior { get; }

    /// <summary>
    /// 行为名称
    /// </summary>
    public string Name => !BehaviorNames.ContainsKey(Behavior) ? string.Empty : BehaviorNames[Behavior];

    public bool Equals(SiloTransferBehavior? other)
    {
        return other != null && this.Behavior == other.Behavior;
    }

    public static implicit operator int(SiloTransferBehavior d) => d.Behavior;

    public static explicit operator SiloTransferBehavior(int b) => new SiloTransferBehavior(b);

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as SiloTransferBehavior);
    }

    public override int GetHashCode()
    {
        return this.Behavior.GetHashCode();
    }
}
