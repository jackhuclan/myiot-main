// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Genesis.Ensure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Pipe;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common.CNC;
using VgEAPClient.Common.CNC.Alarm;
using VgEAPClient.Common.CNC.ATP;
using VgEAPClient.Common.CNC.Buffer;
using VgEAPClient.Common.CNC.CCD;
using VgEAPClient.Common.CNC.ColletClean;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.FirstPcs;
using VgEAPClient.Common.CNC.Knife;
using VgEAPClient.Common.CNC.Options;
using VgEAPClient.Common.CNC.Status;
using VgEAPClient.Common.CNC.ToolMeasure;
using VgEAPClient.Common.CNC.ToolMeasurement;
using VgEAPClient.Common.CNC.ToolParam;
using VgEAPClient.Common.CNC.Write9XNode;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Inbound;
using VgEAPClient.Common.Communication.Outbound;
using VgEAPClient.Common.Configuration;
using VgEAPClient.Common.OpcUaServer;
using static System.Net.Mime.MediaTypeNames;
using ObjectFactory = VgAutoDrill.Infrastructure.ObjectFactory;

namespace VgEAPClient.Common;

public static class EAPClientSetup
{
    public static void AddEAPClientCore(this IServiceCollection services, IConfiguration configuration)
    {
        var clientOptions = configuration.GetSection(nameof(EAPClientOptions)).Get<EAPClientOptions>();
        services.Configure<EAPClientOptions>(configuration.GetSection(nameof(EAPClientOptions)));
        Ensure.ArgumentNotNull(clientOptions, nameof(clientOptions));

        var dataCollectorOptions = configuration.GetSection(nameof(DataCollectorOptions)).Get<DataCollectorOptions>() ?? new DataCollectorOptions { };

        services.Configure<DataCollectorOptions>(configuration.GetSection(nameof(DataCollectorOptions)));
        services.Configure<OpcUaClientOptions>(configuration.GetSection(nameof(OpcUaClientOptions)));
        services.Configure<AOIDbOptions>(configuration.GetSection(nameof(AOIDbOptions)));
        services.AddSingleton<ICNCOperatorProvider, DefaultCNCOperatorProvider>();
        services.AddSingleton<IEAPHeartbeater, HttpEAPHeartbeater>();
        services.AddSingleton<IAsyncTaskWaiter, AsyncTaskWaiter>();
        services.AddSingleton<IObjectFactory, ObjectFactory>();
        services.AddSingleton<IGuiLogger, DefaultGuiLogger>();
        services.AddSingleton<IMesAtpParser, DingTaiAtpParser>();
        services.AddSingleton<ICncFileLoader, CncFileLoader>();
        services.AddSingleton<IConfigurationFileReplacer, JsonConfigurationFileReplacer>();
        services.AddSingleton<IOpcUaServerStartup, OpcUaServerStartup>();

        services.AddSingleton<IAmmeterFactory, AmmeterFactory>();

        services.AddSingleton(dataCollectorOptions.StatusDataOptions);
        services.AddSingleton<IDataCollector<DrillStatusData>, DrillStatusDataCollector>();
        services.AddHostedService(sp => (DrillStatusDataCollector)sp.GetRequiredService<IDataCollector<DrillStatusData>>());

        services.AddSingleton(dataCollectorOptions.BrokenKnifeOptions);
        services.AddSingleton<IDataCollector<DrillBrokenKnifeData>, DrillBrokenKnifeDataCollector>();
        services.AddHostedService(sp => (DrillBrokenKnifeDataCollector)sp.GetRequiredService<IDataCollector<DrillBrokenKnifeData>>());

        services.AddSingleton(dataCollectorOptions.CommonDataOptionsA);
        services.AddSingleton<IDataCollector<DrillCommonDataA>, DrillCommonDataCollectorA>();
        services.AddHostedService(sp => (DrillCommonDataCollectorA)sp.GetRequiredService<IDataCollector<DrillCommonDataA>>());

        services.AddSingleton(dataCollectorOptions.CommonDataOptionsB);
        services.AddSingleton<IDataCollector<DrillCommonDataB>, DrillCommonDataCollectorB>();
        services.AddHostedService(sp => (DrillCommonDataCollectorB)sp.GetRequiredService<IDataCollector<DrillCommonDataB>>());

        services.AddSingleton(dataCollectorOptions.CommonDataOptionsC);
        services.AddSingleton<IDataCollector<DrillCommonDataC>, DrillCommonDataCollectorC>();
        services.AddHostedService(sp => (DrillCommonDataCollectorC)sp.GetRequiredService<IDataCollector<DrillCommonDataC>>());

        services.AddSingleton(dataCollectorOptions.ToolMeasureDataOptions);
        services.AddSingleton<IDataCollector<DrillToolMeasureData>, DrillToolMeasureDataCollector>();
        services.AddHostedService(sp => (DrillToolMeasureDataCollector)sp.GetRequiredService<IDataCollector<DrillToolMeasureData>>());

        services.AddSingleton(dataCollectorOptions.ToolParamDataOptions);
        services.AddSingleton<IDataCollector<DrillToolParamData>, DrillToolParamDataCollector>();
        services.AddHostedService(sp => (DrillToolParamDataCollector)sp.GetRequiredService<IDataCollector<DrillToolParamData>>());

        services.AddSingleton(dataCollectorOptions.ColletCleanDataOptions);
        services.AddSingleton<IDataCollector<DrillColletCleanData>, DrillColletCleanDataCollector>();
        services.AddHostedService(sp => (DrillColletCleanDataCollector)sp.GetRequiredService<IDataCollector<DrillColletCleanData>>());

        services.AddSingleton(dataCollectorOptions.Write9XNodeDataOptions);
        services.AddSingleton<IDataCollector<Write9XNodeData>, Write9XNodeDataCollector>();
        services.AddHostedService(sp => (Write9XNodeDataCollector)sp.GetRequiredService<IDataCollector<Write9XNodeData>>());

        services.AddSingleton(dataCollectorOptions.FirstPcsDataOptions);
        services.AddSingleton<IDataCollector<FirstPcsData>, FirstPcsDataCollector>();
        services.AddHostedService(sp => (FirstPcsDataCollector)sp.GetRequiredService<IDataCollector<FirstPcsData>>());

        services.AddSingleton(dataCollectorOptions.CCDOptions);
        services.AddSingleton<IDataCollector<DrillCCDData>, DrillCCDDataCollector>();
        services.AddHostedService(sp => (DrillCCDDataCollector)sp.GetRequiredService<IDataCollector<DrillCCDData>>());

        services.AddSingleton(dataCollectorOptions.BufferOptions);
        services.AddSingleton<IDataCollector<DrillBufferData>, DrillBufferDataCollector>();
        services.AddHostedService(sp => (DrillBufferDataCollector)sp.GetRequiredService<IDataCollector<DrillBufferData>>());

        services.AddSingleton(dataCollectorOptions.AlarmOptions);
        services.AddSingleton<IDataCollector<DrillAlarmData>, DrillAlarmDataCollector>();
        services.AddHostedService(sp => (DrillAlarmDataCollector)sp.GetRequiredService<IDataCollector<DrillAlarmData>>());

        services.AddSingleton<IDeviceConnectorProvider, DefaultDeviceConnectorProvider>();
        services.Configure<HttpDataReporterOptions>(configuration.GetSection(nameof(HttpDataReporterOptions)));
        services.Configure<HttpDataReceiverOptions>(configuration.GetSection(nameof(HttpDataReceiverOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IEQPDataReporter, DefaultHttpDataReporter>());
        services.AddSingleton<IEQPDataReceiver, DefaultEQPDataReceiver>();
        services.AddSingleton<IDataCollectorStarter, DataCollectorStarter>();

        services.AddDevicesCore(configuration);
        services.AddHttpRequest(configuration);
        services.AddPeriodicTimers(configuration);
        services.AddPipeServer(configuration);
        services.AddPipeClient(configuration);

        services.AddPlugin(configuration);
    }
}
