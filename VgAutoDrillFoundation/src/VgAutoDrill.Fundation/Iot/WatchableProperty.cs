namespace VgAutoDrill.Fundation.Iot;

public sealed class WatchableProperty
{
    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; private set; } = string.Empty;
    public object? OldValue { get => _oldValue; private set => _oldValue = value; }
    public object? NewValue { get => _newValue; private set => _newValue = value; }
    /// <summary>
    /// 属性描述
    /// </summary>
    public string Description { get; }
    public bool IsValueChanged { get => _isValueChanged; private set => _isValueChanged = value; }
    public ActionTiggerMode TiggerMode { get; internal set; } = ActionTiggerMode.Synchronized;
    public int ActionCount { get { return actionCount; } }

    private Predicate<WatchableProperty> preCondition;
    private Predicate<WatchableProperty> postCondition;
    private Predicate<WatchableProperty>? valueChangedCondition;
    private ActionTriggerTimes triggerTimes;
    private Action? onTrigger;
    private readonly List<Task> tracingTasks = new List<Task>();
    private volatile int triggeredCount;
    private volatile int actionCount;
    private volatile object? _newValue;
    private volatile object? _oldValue;
    private readonly object _lock = new object();

    public Func<Task<object?>> TakeValue;
    private volatile bool _isValueChanged = false;

    public WatchableProperty(string propertyName, object? initValue, string description = "")
    {
        PropertyName = propertyName;
        NewValue = initValue;
        Description = description;
        preCondition = (WatchableProperty watchProperties) => true;
        postCondition = (WatchableProperty watchProperties) => true;
        TakeValue = async () => await Task.FromResult(initValue);
    }

    public WatchableProperty(string propertyName, object? initValue, Func<Task<object?>> take, string description = "")
        : this(propertyName, initValue, description)
    {
        TakeValue = take;
    }

    public void SetValue(object? newValue)
    {
        lock (_lock)
        {
            bool preConditionMatched = preCondition.Invoke(this);
            if ((NewValue == null && newValue == null) || (NewValue != null && NewValue.Equals(newValue)))
                IsValueChanged = false;
            else
                IsValueChanged = true;

            this.OldValue = NewValue;
            this.NewValue = newValue;

            bool postConditionMatched = postCondition.Invoke(this);

            if (onTrigger != null && valueChangedCondition != null && valueChangedCondition.Invoke(this))
            {
                InvokeTriggerAction();
            }
            else if (onTrigger != null && preConditionMatched && postConditionMatched && valueChangedCondition == null)
            {
                InvokeTriggerAction();
            }
        }
    }

    public WatchableProperty PreCondition(Predicate<WatchableProperty> predicate)
    {
        preCondition = predicate;
        return this;
    }

    public WatchableProperty PostCondition(Predicate<WatchableProperty> predicate)
    {
        postCondition = predicate;
        return this;
    }

    public WatchableProperty When(Predicate<WatchableProperty> predicate)
    {
        valueChangedCondition = predicate;
        return this;
    }

    public WatchableProperty WhenValueChanged()
    {
        return When(p => p.IsValueChanged);
    }

    public void TriggerOnce(Action action)
    {
        SetTriggerAction(action, ActionTriggerTimes.Once, ActionTiggerMode.Synchronized);
    }

    public void TriggerAlways(Action action)
    {
        SetTriggerAction(action, ActionTriggerTimes.Always, ActionTiggerMode.Synchronized);
    }

    public void TriggerOnceAsync(Action action)
    {
        SetTriggerAction(action, ActionTriggerTimes.Once, ActionTiggerMode.Asynchronized);
    }

    public void TriggerAlwaysAsync(Action action)
    {
        SetTriggerAction(action, ActionTriggerTimes.Always, ActionTiggerMode.Asynchronized);
    }

    private void SetTriggerAction(Action action,
        ActionTriggerTimes triggerTimes,
        ActionTiggerMode triggerMode)
    {
        this.triggerTimes = triggerTimes;
        onTrigger += action;
        Interlocked.Increment(ref actionCount);
        TiggerMode = triggerMode;
    }

    /// <summary>
    /// waiting all actions are completed!
    /// </summary>
    public void WaitAll()
    {
        try
        {
            Task.WaitAll(tracingTasks.ToArray());
            tracingTasks.Clear();
        }
        catch (Exception)
        {
            //ignore exception in every task
        }
    }

    private void InvokeTriggerAction()
    {
        if (onTrigger != null && triggeredCount == 0 && triggerTimes == ActionTriggerTimes.Once)
        {
            TriggerEachAction();
        }

        if (onTrigger != null && triggerTimes == ActionTriggerTimes.Always)
        {
            TriggerEachAction();
        }
    }

    private void TriggerEachAction()
    {
        foreach (Action trigger in onTrigger.GetInvocationList())
        {
            tracingTasks.Add(Task.Run(() => trigger.Invoke()));
        }

        Interlocked.Increment(ref triggeredCount);

        if (TiggerMode == ActionTiggerMode.Synchronized)
        {
            WaitAll();
        }
    }

    public override string ToString()
    {
        return "WatchableProperty[" + GetHashCode() + "]: {" +
            "\t\n PropertyName: " + PropertyName +
            "\t\n OldValue: " + OldValue +
            "\t\n NewValue: " + NewValue +
            "\t\n IsValueChanged: " + IsValueChanged +
            "\t\n ActionCount: " + ActionCount +
            "\n}";
    }
}
