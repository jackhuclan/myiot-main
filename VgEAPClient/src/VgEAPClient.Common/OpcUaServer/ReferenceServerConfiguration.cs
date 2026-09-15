// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace VgEAPClient.Common.OpcUaServer;

/// <summary>
/// Stores the configuration the data access node manager.
/// </summary>
[DataContract(Namespace = Namespaces.ReferenceApplications)]
public class ReferenceServerConfiguration
{
    #region Constructors

    /// <summary>
    /// The default constructor.
    /// </summary>
    public ReferenceServerConfiguration()
    {
        Initialize();
    }

    /// <summary>
    /// Initializes the object during deserialization.
    /// </summary>
    [OnDeserializing()]
    private void Initialize(StreamingContext context)
    {
        Initialize();
    }

    /// <summary>
    /// Sets private members to default values.
    /// </summary>
    private void Initialize()
    {
    }

    #endregion Constructors
}
