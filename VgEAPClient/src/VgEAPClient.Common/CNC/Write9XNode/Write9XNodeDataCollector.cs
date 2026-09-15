// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Write9XNode;

internal class Write9XNodeDataCollector : AbstractDataCollector<Write9XNodeData>
{
    private readonly ILogger<Write9XNodeDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly Write9XNodeDataCollectorOptions _options;

    public Write9XNodeDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        Write9XNodeDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<Write9XNodeDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        try
        {
            await Data.Cnc9XWriteNodeData.LockWriteNodeValue.WaitAsync();
            try
            {
                if (Data.Cnc9XWriteNodeData.IsWriteNodeValue)
                {
                    await _cNCConnector.RetrieveData($"Cnc9XWriteNodeData_{Data.Cnc9XWriteNodeData.Node}@@@@{Data.Cnc9XWriteNodeData.ValueType}@@@@{Data.Cnc9XWriteNodeData.Value}");

                    _logger.LogInformation($"PullData Cnc9XWriteNodeData - {Data.Cnc9XWriteNodeData.Node}@@@@{Data.Cnc9XWriteNodeData.ValueType}@@@@{Data.Cnc9XWriteNodeData.Value}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cnc9SetNodeData.LockSetNodeValue inner EX - {ex.Message}");
            }
            finally
            {
                Data.Cnc9XWriteNodeData.LockWriteNodeValue.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
