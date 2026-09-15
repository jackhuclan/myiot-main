using System.Text;

namespace VgAutoDrill.Fundation.Iot;

public sealed class WatchableProperties
{
    private readonly Dictionary<string, WatchableProperties> globalWatchableProperties = new Dictionary<string, WatchableProperties>();
    private readonly List<WatchableProperties> matchedWatchableProperties = new List<WatchableProperties>();
    private readonly List<string> filledProperties = new List<string>();
    private readonly List<Task> tracingTasks = new List<Task>();
    private readonly Dictionary<string, WatchableProperty> watchProperties;
    private Predicate<WatchableProperties> preCondition;
    private Predicate<WatchableProperties> postCondition;
    private Predicate<WatchableProperties>? valueChangedCondition;
    private ActionTriggerTimes actionTriggerTimes;
    private ActionTiggerMode actionTiggerMode = ActionTiggerMode.Synchronized;
    private volatile int triggeredCount;
    private volatile int actionCount;
    private Action? onTrigger;
    private const char seperator = '@';
    private const string ROOT_PROPERTIES_NAME = "_ROOT_";
    private readonly object _lock = new object();

    public WatchableProperties? Parent { get; private set; }
    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertiesName { get; private set; }
    /// <summary>
    /// 属性描述
    /// </summary>
    public string Description { get; }

    public int ActionCount { get { return actionCount; } }

    public WatchableProperties Root
    {
        get
        {
            var _cur = this;
            var _root = this.Parent;
            while (_root != null)
            {
                _cur = _root;
                _root = _root.Parent;
            }

            return _cur;
        }
    }

    public WatchableProperties() : this(ROOT_PROPERTIES_NAME, string.Empty) { }

    private WatchableProperties(string propertiesName, string description = "")
    {
        watchProperties = new Dictionary<string, WatchableProperty>();
        preCondition = (WatchableProperties watchProperties) => true;
        postCondition = (WatchableProperties watchProperties) => true;
        PropertiesName = propertiesName;
        Description = description;
    }

    public WatchableProperties this[string propertiesName]
    {
        get { return this.Root.globalWatchableProperties[propertiesName]; }
    }

    public HashSet<string> GetPropertyKeys()
    {
        lock (_lock)
        {
            return watchProperties.Keys.ToHashSet();
        }
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object?> GetValues()
    {
        lock (_lock)
        {
            var dic = new Dictionary<string, object?>();
            foreach (var item in watchProperties)
            {
                dic.Add(item.Key, item.Value.NewValue);
            }

            return dic;
        }
    }

    /// <summary>
    /// 添加属性，Property must not contains specail char '@'
    /// </summary>
    /// <param name="key">属性名称</param>
    /// <param name="value">属性初始值</param>
    /// <param name="description">属性描述</param>
    /// <returns>属性组</returns>
    /// <exception cref="ArgumentException"></exception>
    public WatchableProperties AddProperty(string key, object? value, string description = "")
    {
        if (key.Contains(seperator)) throw new ArgumentException("Property must not contains specail char '@'");
        return AddProperty(new WatchableProperty(key, value, description));
    }

    /// <summary>
    /// 添加属性，Property must not contains specail char '@'
    /// </summary>
    /// <param name="key">属性名称</param>
    /// <param name="value">属性初始值</param>
    /// <param name="take">属性取值函数</param>
    /// <param name="description">属性描述</param>
    /// <returns>属性组</returns>
    /// <exception cref="ArgumentException"></exception>
    public WatchableProperties AddProperty(string key, object? value, Func<Task<object?>> take, string description = "")
    {
        if (key.Contains(seperator)) throw new ArgumentException("Property must not contains specail char '@'");
        return AddProperty(new WatchableProperty(key, value, take, description));
    }

    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="property">属性</param>
    /// <returns></returns>
    public WatchableProperties AddProperty(WatchableProperty property)
    {
        lock (_lock)
        {
            if (!watchProperties.ContainsKey(property.PropertyName))
            {
                watchProperties.Add(property.PropertyName, property);
            }

            return this;
        }
    }

    public WatchableProperty Property(string propertyName)
    {
        return this.Root.watchProperties[propertyName];
    }

    public bool HasProperty(string propertyName)
    {
        return this.Root.watchProperties.ContainsKey(propertyName);
    }

    public void RemoveProperty(string propertyName)
    {
        lock (_lock)
        {
            if (this.Root.watchProperties.ContainsKey(propertyName))
            {
                this.Root.watchProperties.Remove(propertyName);
            }

            foreach (var wp in this.Root.globalWatchableProperties.Values.Where(x => x.watchProperties.ContainsKey(propertyName)))
            {
                wp.watchProperties.Remove(propertyName);
            }
        }
    }

    public void RemoveProperty(WatchableProperty property)
    {
        RemoveProperty(property.PropertyName);
    }

    public WatchableProperties Properties(params string[] properties)
    {
        if (properties.Any(p => p.Contains(seperator))) throw new ArgumentException("Property must not contains specail char '@'");
        var groupKey = string.Join(seperator, properties.OrderBy(x => x));
        if (this.Root.globalWatchableProperties.ContainsKey(groupKey))
        {
            return this.Root.globalWatchableProperties[groupKey];
        }

        var wp = new WatchableProperties(groupKey);
        wp.Parent = this.Root;
        foreach (var property in properties)
        {
            wp.AddProperty(Property(property));
        }
        this.Root.globalWatchableProperties.Add(groupKey, wp);
        return wp;
    }

    public WatchableProperties PreCondition(Predicate<WatchableProperties> predicate)
    {
        preCondition = predicate;
        return this;
    }

    public WatchableProperties PostCondition(Predicate<WatchableProperties> predicate)
    {
        postCondition = predicate;
        return this;
    }

    public WatchableProperties WhenAnyValueChanged()
    {
        valueChangedCondition = (WatchableProperties wp) => wp.watchProperties.Any(p => wp.Root.HasProperty(p.Key) && p.Value.IsValueChanged);
        return this;
    }

    public WatchableProperties WhenAllValueChanged()
    {
        valueChangedCondition = (WatchableProperties wp) => wp.watchProperties.All(p => wp.Root.HasProperty(p.Key) && p.Value.IsValueChanged);
        return this;
    }

    public WatchableProperties When(Predicate<WatchableProperties> predicate)
    {
        valueChangedCondition = predicate;
        return this;
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
        actionTriggerTimes = triggerTimes;
        onTrigger += action;
        Interlocked.Increment(ref actionCount);
        SetActionTiggerMode(this, triggerMode);
    }

    private void SetActionTiggerMode(WatchableProperties that, ActionTiggerMode triggerMode)
    {
        that.actionTiggerMode = triggerMode;

        foreach (var wp in that.globalWatchableProperties.Values)
        {
            SetActionTiggerMode(wp, triggerMode);
        }

        foreach (var wp in watchProperties.Values)
        {
            wp.TiggerMode = triggerMode;
        }
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

            foreach (var wp in globalWatchableProperties.Values)
            {
                wp.WaitAll();
            }

            var list = globalWatchableProperties.Values.SelectMany(gwp => gwp.watchProperties.Values)
                .Distinct();

            foreach (var wp in watchProperties.Where(x => !list.Contains(x.Value)))
            {
                wp.Value.WaitAll();
            }
        }
        catch (Exception)
        {
            //ignore exception in every task
        }
    }

    public void SetValues(Dictionary<string, object?> values)
    {
        lock (_lock)
        {
            if (this.Root != this)
            {
                this.Root.SetValues(values);
                return;
            }

            matchedWatchableProperties.Clear();
            filledProperties.Clear();

            matchedWatchableProperties.AddRange(from groupKey in globalWatchableProperties.Keys
                                                where groupKey.Split(seperator).All(x => values.ContainsKey(x))
                                                select globalWatchableProperties[groupKey]);

            foreach (var wp in matchedWatchableProperties)
            {
                if (wp.valueChangedCondition != null)
                {
                    DoSetValues(wp, values, filledProperties);
                    var canTrigger = wp.valueChangedCondition.Invoke(wp);
                    if (canTrigger)
                    {
                        InvokeTriggerAction(wp);
                    }
                }
                else
                {
                    var preConditionMatched = wp.preCondition.Invoke(wp);
                    DoSetValues(wp, values, filledProperties);
                    bool postConditionMatched = wp.postCondition.Invoke(wp);
                    if (preConditionMatched && postConditionMatched)
                    {
                        InvokeTriggerAction(wp);
                    }
                }
            }

            foreach (var item in values.Where(v => watchProperties.ContainsKey(v.Key) && !filledProperties.Contains(v.Key)))
            {
                if (!HasProperty(item.Key)) continue;
                Property(item.Key).SetValue(item.Value);
            }
        }
    }

    /// <summary>
    /// refresh value of all properties according its take value action.
    /// </summary>
    /// <returns></returns>
    public async Task Refresh()
    {
        var values = new Dictionary<string, object?>();
        foreach (var wp in this.Root.watchProperties.Values)
        {
            values.Add(wp.PropertyName, await wp.TakeValue.Invoke());
        }
        System.Diagnostics.Debug.WriteLine(System.Text.Json.JsonSerializer.Serialize(values));
        SetValues(values);
    }

    private void DoSetValues(WatchableProperties wp, Dictionary<string, object?> values, List<string> filledProperties)
    {
        foreach (var item in values)
        {
            if (wp.watchProperties.ContainsKey(item.Key) && !filledProperties.Contains(item.Key))
            {
                if (!HasProperty(item.Key)) continue;
                Property(item.Key).SetValue(item.Value);
                filledProperties.Add(item.Key);
            }
        }
    }

    private static void InvokeTriggerAction(WatchableProperties wp)
    {
        if (wp.onTrigger != null && wp.triggeredCount == 0 && wp.actionTriggerTimes == ActionTriggerTimes.Once)
        {
            TriggerEachAction(wp);
        }

        if (wp.onTrigger != null && wp.actionTriggerTimes == ActionTriggerTimes.Always)
        {
            TriggerEachAction(wp);
        }
    }

    private static void TriggerEachAction(WatchableProperties wp)
    {
        foreach (Action trigger in wp.onTrigger.GetInvocationList())
        {
            wp.tracingTasks.Add(Task.Run(() => trigger.Invoke()));
        }

        Interlocked.Increment(ref wp.triggeredCount);

        if (wp.actionTiggerMode == ActionTiggerMode.Synchronized)
        {
            wp.WaitAll();
        }
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("WatchableProperties[" + GetHashCode() + "]: {");
        stringBuilder.AppendLine("ActionCount: " + ActionCount);

        foreach (var wp in globalWatchableProperties)
        {
            stringBuilder.AppendLine("\tWatchableProperties[" + wp.Key + "]: {");
            stringBuilder.AppendLine("\t" + wp.ToString());
            stringBuilder.AppendLine("\t}");
        }

        var list = globalWatchableProperties.Values.SelectMany(gwp => gwp.watchProperties.Values)
            .Distinct();

        foreach (var wp in watchProperties.Where(x => !list.Contains(x.Value)))
        {
            stringBuilder.AppendLine("\t\t" + wp.ToString());
        }

        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}
