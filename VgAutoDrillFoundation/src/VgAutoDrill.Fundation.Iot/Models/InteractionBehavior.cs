namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// use binary ushort to indicate interaction behavior between devices
/// 0|-1---|-2-|-3-|-4--
/// 0|00000|000|000|0000
/// 0: InteractionMode,2^1 = 2
/// 1: DeviceKind,2^5 = 32
/// 2: InteractionSequence,2^3 = 8
/// 3: InteractionPosition,2^3 = 8
/// 4: MaterialKind,2^4 = 16
/// </summary>
public class InteractionBehavior : IEquatable<InteractionBehavior>
{
    public const ushort REAR_LOAD_PANEL_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Panel;//1
    public const ushort REAR_UNLOAD_PANEL_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Panel;//129
    public const ushort REAR_LOAD_PANEL_THEN_UNLOAD_PANEL = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Panel;//257
    public const ushort REAR_UNLOAD_PANEL_THEN_LOAD_PANEL = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Panel;//385

    public const ushort FRONT_LOAD_PANEL_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Panel;//17
    public const ushort FRONT_UNLOAD_PANEL_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Panel;//145
    public const ushort FRONT_LOAD_PANEL_THEN_UNLOAD_PANEL = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Panel;//273
    public const ushort FRONT_UNLOAD_PANEL_THEN_LOAD_PANEL = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Panel;//401

    public const ushort REAR_LOAD_SILO_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.PanelSilo;//3
    public const ushort REAR_UNLOAD_SILO_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.PanelSilo;//131
    public const ushort REAR_LOAD_SILO_THEN_UNLOAD_SILO = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.PanelSilo;//259
    public const ushort REAR_UNLOAD_SILO_THEN_LOAD_SILO = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.PanelSilo;//387

    public const ushort FRONT_LOAD_SILO_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.PanelSilo;//19
    public const ushort FRONT_UNLOAD_SILO_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.PanelSilo;//147
    public const ushort FRONT_LOAD_SILO_THEN_UNLOAD_SILO = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.PanelSilo;//275
    public const ushort FRONT_UNLOAD_SILO_THEN_LOAD_SILO = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.PanelSilo;//403

    public const ushort REAR_LOAD_CUTTER_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Cutter;//2
    public const ushort REAR_UNLOAD_CUTTER_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Cutter;//130
    public const ushort REAR_LOAD_CUTTER_THEN_UNLOAD_CUTTER = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Cutter;//258
    public const ushort REAR_UNLOAD_CUTTER_THEN_LOAD_CUTTER = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Rear << 4 | (ushort)MaterialKind.Cutter;//386

    public const ushort FRONT_LOAD_CUTTER_ONLY = (ushort)InteractionSequence.LoadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Cutter;//18
    public const ushort FRONT_UNLOAD_CUTTER_ONLY = (ushort)InteractionSequence.UnloadOnly << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Cutter;//146
    public const ushort FRONT_LOAD_CUTTER_THEN_UNLOAD_CUTTER = (ushort)InteractionSequence.LoadThenUnload << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Cutter;//274
    public const ushort FRONT_UNLOAD_CUTTER_THEN_LOAD_CUTTER = (ushort)InteractionSequence.UnloadThenLoad << 7 | (ushort)InteractionPosition.Front << 4 | (ushort)MaterialKind.Cutter;//402

    public static InteractionBehavior Noop = new InteractionBehavior(0);

    private InteractionBehavior(ushort behavior)
    {
        InteractionMode = (InteractionMode)((0x8000 & behavior) >> 15);
        DeviceKind = (DeviceKind)((0x7C00 & behavior) >> 10);
        InteractionSequence = (InteractionSequence)((0x380 & behavior) >> 7);
        InteractionPosition = (InteractionPosition)((0x70 & behavior) >> 4);
        MaterialKind = (MaterialKind)(0xF & behavior);
        Behavior = behavior;
    }

    public static InteractionBehavior Make(DeviceKind kind, ushort behavior, InteractionMode mode = InteractionMode.Passive)
    {
        return new InteractionBehavior((ushort)((ushort)mode << 15 | (ushort)kind << 10 | (behavior & 0x3ff)));
    }

    public static InteractionBehavior Make(InteractionMode mode, DeviceKind deviceKind, InteractionSequence sequence, InteractionPosition position, MaterialKind material)
    {
        return new InteractionBehavior((ushort)(((ushort)mode << 15) | ((ushort)deviceKind << 10) | ((ushort)sequence << 7) | ((ushort)position << 4) | (ushort)material));
    }

    public InteractionSequence InteractionSequence { get; }
    public InteractionMode InteractionMode { get; }
    public InteractionPosition InteractionPosition { get; }
    public MaterialKind MaterialKind { get; }
    public DeviceKind DeviceKind { get; }
    public ushort Behavior { get; }

    public bool Equals(InteractionBehavior? other) => other != null && this.Behavior == other.Behavior;

    public bool Is(InteractionBehavior interactionBehavior) => this.Behavior == interactionBehavior.Behavior;

    public static implicit operator ushort(InteractionBehavior d) => d.Behavior;

    public static explicit operator InteractionBehavior(ushort b) => new InteractionBehavior(b);

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as InteractionBehavior);
    }

    public override int GetHashCode()
    {
        return this.Behavior.GetHashCode();
    }
}
