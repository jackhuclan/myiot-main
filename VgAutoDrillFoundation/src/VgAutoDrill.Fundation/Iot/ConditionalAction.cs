namespace VgAutoDrill.Fundation.Iot;

public class ConditionalAction
{
    public ConditionalAction(Func<bool> predicate, Action action)
    {
        Predicate = predicate;
        Action = action;
    }

    public Func<bool> Predicate { get; }
    public Action Action { get; }
}
