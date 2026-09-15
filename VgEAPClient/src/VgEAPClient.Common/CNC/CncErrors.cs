// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

internal class CncErrors : IEquatable<CncErrors>
{
    private readonly List<string> _cncErrorItems = new List<string>();

    public IReadOnlyList<string> ErrorItems => _cncErrorItems;

    public CncErrors(string[] errors)
    {
        if (errors == null)
            return;

        foreach (var error in errors)
        {
            _cncErrorItems.Add(error);
        }
    }

    /// <summary>
    /// 从<paramref name="other"/>找到第一个不同于原来<see cref="CncErrors"/>的错误项
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public string PickDiffOneFromOther(CncErrors other)
    {
        if (other != null)
        {
            for (int i = 0; i < other.ErrorItems.Count; i++)
            {
                if (!other.ErrorItems[i].Equals(this.ErrorItems[i]))
                {
                    return other.ErrorItems[i];
                }
            }
        }

        return other == null ? "" : other.ErrorItems[0];
    }

    public bool Equals(CncErrors? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (other != null
            && other.ErrorItems.Count == this.ErrorItems.Count)
        {
            for (int i = 0; i < other.ErrorItems.Count; i++)
            {
                if (!other.ErrorItems[i].Equals(this.ErrorItems[i]))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public override int GetHashCode() => base.GetHashCode();
}
