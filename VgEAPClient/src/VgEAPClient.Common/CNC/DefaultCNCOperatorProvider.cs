// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.CNC.ATP;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;

internal class DefaultCNCOperatorProvider : ICNCOperatorProvider
{
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly AOIDbOptions _aOIDbOptions;
    private readonly IObjectFactory _objectFactory;

    public DefaultCNCOperatorProvider(IOptions<EAPClientOptions> options,
        IOptions<AOIDbOptions> aoioptions,
        IObjectFactory objectFactory)
    {
        _eAPClientOptions = options.Value;
        _aOIDbOptions = aoioptions.Value;
        _objectFactory = objectFactory;
    }

    public IAtpFileParser GetAtpFileParser()
    {
        switch (_eAPClientOptions.DeviceDescriptor.DeviceKind)
        {
            case DeviceKind.CNC95Drill:
                return _objectFactory.GetOrCreate<Cnc95AtpFileParser>();

            default:
                return _objectFactory.GetOrCreate<Cnc84AtpFileParser>();
        }
    }

    public ICNCConnector GetCNCConnector()
    {
        if (_aOIDbOptions.IsEnable)
        {
            return _objectFactory.GetOrCreate<AOIDbConnector>();
        }
        switch (_eAPClientOptions.DeviceDescriptor.DeviceKind)
        {
            case DeviceKind.CNC95Drill:
                return _objectFactory.GetOrCreate<OpcUaCNCConnector>();

            default:
                return _objectFactory.GetOrCreate<SocketCNCConnector>();
        }
    }
}
