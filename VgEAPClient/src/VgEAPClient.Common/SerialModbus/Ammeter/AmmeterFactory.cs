// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure;

namespace VgEAPClient.Common;


public interface IAmmeterFactory
{
    //注意：一个机台同型号的电表只有个一个实例。即便多次调用此方法，返回的也是同一个实例。
    IAmmeterIO? GetAmmeterIO(string model);
}
public class AmmeterFactory : IAmmeterFactory
{
    ILoggerFactory _loggerFactory;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly IObjectFactory _objectFactory;

    ILogger<AmmeterFactory> _logger;
    Dictionary<string, IAmmeterIO> _dic = new Dictionary<string, IAmmeterIO>();

    public AmmeterFactory(ILoggerFactory loggerFactory, IOptions<EAPClientOptions> options,
        IObjectFactory objectFactory)
    {
        _loggerFactory = loggerFactory;
        _eAPClientOptions = options.Value;
        _objectFactory = objectFactory;

        _logger = _loggerFactory.CreateLogger<AmmeterFactory>();
    }


    public IAmmeterIO? GetAmmeterIO(string model)
    {
        if (false == _dic.ContainsKey(model))
        {
            switch (model)
            {
                case AmmeterCHNTDTSU666.ModelName:
                    {
                        var ammeter_imp = new AmmeterCHNTDTSU666(_loggerFactory.CreateLogger("Ammeter " + model));
                        _dic[model] = ammeter_imp;
                        _logger.LogDebug($"Ammeter Factory : add '{ammeter_imp.GetType()}'. model '{model}'.");
                    }
                    break;
                default:
                    break;
            }
        }

        if (_dic.ContainsKey(model))
            return _dic[model];
        else
        {
            return null;
        }
    }
}
