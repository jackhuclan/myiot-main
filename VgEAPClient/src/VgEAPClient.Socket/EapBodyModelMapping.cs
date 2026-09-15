// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public sealed class EapBodyModelMapping
{
    private static readonly Dictionary<string, Type> _modelTypes = new();
    private static readonly Dictionary<Type, Type> _replyTypes = new();

    private EapBodyModelMapping()
    { }

    static EapBodyModelMapping()
    {
        typeof(EapMessage).Assembly.GetTypes()
            .Where(x => x.IsSubclassOf(typeof(EapMessage.EapBody)))
            .ToList().ForEach(x =>
            {
                _modelTypes.Add(x.Name, x);

                if (x.CustomAttributes.Any(t => t.AttributeType == typeof(ReplyModelTypeAttribute)))
                {
                    var replyTypeAttribute = (ReplyModelTypeAttribute?)x.GetCustomAttributes(typeof(ReplyModelTypeAttribute), false).FirstOrDefault();
                    if (replyTypeAttribute != null)
                    {
                        _replyTypes.Add(x, replyTypeAttribute.ReplyType);
                    }
                }
            });
    }

    public static Dictionary<string, Type> ModelTypes => _modelTypes;

    public static Dictionary<Type, Type> ReplyTypes => _replyTypes;
}
