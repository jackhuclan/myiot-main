using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace VgAutoDrill.Fundation.Iot.Configuration;

public class DeviceDescriptorCollection : IDeviceDescriptorCollection
{
    private readonly Dictionary<string, DeviceDescriptor> _descriptors = new Dictionary<string, DeviceDescriptor>();
    private bool _isReadOnly;

    /// <inheritdoc />
    public int Count => _descriptors.Count;

    /// <inheritdoc />
    public bool IsReadOnly => _isReadOnly;

    public ICollection<string> Keys => _descriptors.Keys;

    public ICollection<DeviceDescriptor> Values => _descriptors.Values;

    /// <inheritdoc />
    public DeviceDescriptor this[string index]
    {
        get
        {
            return _descriptors[index];
        }
        set
        {
            CheckReadOnly();
            _descriptors[index] = value;
        }
    }

    /// <inheritdoc />
    public void Clear()
    {
        CheckReadOnly();
        _descriptors.Clear();
    }

    /// <summary>
    /// Makes this collection read-only.
    /// </summary>
    /// <remarks>
    /// After the collection is marked as read-only, any further attempt to modify it throws an <see cref="InvalidOperationException" />.
    /// </remarks>
    public void MakeReadOnly()
    {
        _isReadOnly = true;
    }

    private void CheckReadOnly()
    {
        if (_isReadOnly)
        {
            ThrowReadOnlyException();
        }
    }

    private static void ThrowReadOnlyException() => throw new InvalidOperationException("MachineCollection is readonly");

    public void Add(string key, DeviceDescriptor value)
    {
        _descriptors.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _descriptors.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _descriptors.Remove(key);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out DeviceDescriptor value)
    {
        return _descriptors.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<string, DeviceDescriptor> item)
    {
        _descriptors.Add(item.Key, item.Value);
    }

    public bool Contains(KeyValuePair<string, DeviceDescriptor> item)
    {
        return _descriptors.ContainsKey(item.Key);
    }

    public void CopyTo(KeyValuePair<string, DeviceDescriptor>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, DeviceDescriptor> item)
    {
        return _descriptors.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, DeviceDescriptor>> GetEnumerator()
    {
        return _descriptors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _descriptors.GetEnumerator();
    }
}
