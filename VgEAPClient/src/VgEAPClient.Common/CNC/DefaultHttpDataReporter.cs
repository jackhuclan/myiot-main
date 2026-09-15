// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Outbound;

namespace VgEAPClient.Common.CNC;

public class DefaultHttpDataReporter : IEQPDataReporter, IDisposable
{
    public event Func<LotInfoRequestModel, Task<LotInfoRequestModel>>? OnLotInfoRequestReturn;

    public event Func<EQPCompletedReportModel, Task<EQPCompletedReportModel>>? OnEQPCompletedReportReturn;

    public event Func<Task> OnTick;


    public ILogger<DefaultHttpDataReporter> _logger;
    public IHostApplicationLifetime _hostApplicationLifetime;
    public IEAPHeartbeater _eAPHeartbeater;
    public HttpDataReporterOptions _reporterOptions;
    public IHttpRequestInvoker _httpRequestInvoker;
    public EAPClientOptions _eAPClientOptions;
    public PeriodicTimer _periodicTimer;
    public JsonSerializerOptions _jsonSerializerOptions;

    public DefaultHttpDataReporter(ILogger<DefaultHttpDataReporter> logger,
            IOptions<HttpDataReporterOptions> reporterOptions,
            IOptions<EAPClientOptions> options1,
            IHostApplicationLifetime hostApplicationLifetime,
            IEAPHeartbeater eAPHeartbeater,
            IHttpRequestInvoker httpRequestInvoker)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _eAPHeartbeater = eAPHeartbeater;
        _reporterOptions = reporterOptions.Value;
        _eAPClientOptions = options1.Value;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = null, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        _httpRequestInvoker = httpRequestInvoker;
        SetHttpRequestHeaders();
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(_reporterOptions.DataCollectionReportSeconds));
        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
        OnTick += () => Task.CompletedTask;
    }

    public virtual void SetHttpRequestHeaders()
    {
        _httpRequestInvoker.ConfigureHttpRequestHeaders(headers =>
        {
            headers.Add("Basic_base64_auth_string", _eAPClientOptions.AuthString);
            headers.Add("EqpId", _eAPClientOptions.EquipmentID);
        });
    }

    public void Dispose()
    {
        _periodicTimer.Dispose();
    }

    public void OnLotInfoRequestReturnInvoke(LotInfoRequestModel lotInfoRequestModel)
    {
        OnLotInfoRequestReturn?.Invoke(lotInfoRequestModel);
    }

    public void OnEQPCompletedReportReturnInvoke(EQPCompletedReportModel eQPCompletedReportModel)
    {
        OnEQPCompletedReportReturn?.Invoke(eQPCompletedReportModel);
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        using (_periodicTimer)
        {
            while (!cancellationToken.IsCancellationRequested
                && await _periodicTimer.WaitForNextTickAsync())
            {
                try
                {
                    await OnTick.Invoke();
                    if (_eAPHeartbeater.Connected)
                    {
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                }
            }
        }
    }

    public virtual async Task SendEQPAlarmReport(EQPAlarmReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPAlarmReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPAlarmReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPAlarmReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
                result: new EQPReportResult());

            string sendstr = data.ToJsonNull();
            _logger.LogInformation("EQPAlarmReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{sendstr}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPAlarmReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPAlarmReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPAlarmReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPDataCollectionReport(EQPDataCollectionReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPDataCollectionReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPDataCollectionReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPDataCollectionReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
                result: new EQPReportResult());

            string sendstr = data.ToJsonNull();
            _logger.LogInformation("EQPDataCollectionReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{sendstr}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPDataCollectionReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPDataCollectionReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");

        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPDataCollectionReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPStatusChangeReport(EQPStatusChangeReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPStatusChangeReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPStatusChangeReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPStatusChangeReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
            result: new EQPReportResult());

            string sendstr = data.ToJsonNull();
            _logger.LogInformation("EQPStatusChangeReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{sendstr}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPStatusChangeReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPStatusChangeReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPStatusChangeReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPDateTimeRequest(EQPDateTimeRequestBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPDateTimeRequestUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPDateTimeRequestModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPDateTimeRequest",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            string sendstr = data.ToJsonNull();
            _logger.LogInformation("EQPDateTimeRequest[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{sendstr}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPDateTimeRequestModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPDateTimeRequest[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPDateTimeRequest[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPCommunicationStatusReport(EQPCommunicationStatusReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPCommunicationStatusReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPCommunicationStatusReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPCommunicationStatusReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPCommunicationStatusReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPCommunicationStatusReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPCommunicationStatusReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPCommunicationStatusReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPRunningModeReport(EQPRunningModeReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPRunningModeReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPRunningModeReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPRunningModeReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPRunningModeReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPRunningModeReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPRunningModeReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPRunningModeReport[Error]:{ex}");
        }
    }

    public virtual async Task SendLotInfoRequest(LotInfoRequestBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.LotInfoRequestUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new LotInfoRequestModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "LotInfoRequest",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("LotInfoRequest[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<LotInfoRequestModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("LotInfoRequest[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");

            if (return_code != null)
            {
                LotInfoRequestModel lotInfoRequestModel = EntityUtil<LotInfoRequestModel>.JsonToEntity(return_code.ToStringEx());
                _logger.LogInformation("LotInfoRequest[EAP -> EQP] Format" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{lotInfoRequestModel.ToJsonNull()}" + "\r\n");

                OnLotInfoRequestReturn?.Invoke(lotInfoRequestModel);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"LotInfoRequest[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPCurrentChangeRecipeReport(EQPCurrentChangeRecipeReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPCurrentChangeRecipeReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPCurrentChangeRecipeReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPCurrentChangeRecipeReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPCurrentChangeRecipeReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPCurrentChangeRecipeReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPCurrentChangeRecipeReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");

        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPCurrentChangeRecipeReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPReceiveJobReport(EQPReceiveJobReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPReceiveJobReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPReceiveJobReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPReceiveJobReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPReceiveJobReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPReceiveJobReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPReceiveJobReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPReceiveJobReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPSendOutJobReport(EQPSendOutJobReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPSendOutJobReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new EQPSendOutJobReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPSendOutJobReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPSendOutJobReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPSendOutJobReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPSendOutJobReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPSendOutJobReport[Error]:{ex}");
        }
    }

    public virtual async Task SendEQPCompletedReport(EQPCompletedReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.EQPCompletedReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }
            var data = new EQPCompletedReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "EQPCompletedReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("EQPCompletedReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPCompletedReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("EQPCompletedReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"EQPCompletedReport[Error]:{ex}");
        }
    }

    public virtual async Task SendUserCheckCardReport(UserCheckCardReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.UserCheckCardReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new UserCheckCardReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "UserCheckCardReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("UserCheckCardReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<UserCheckCardReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("UserCheckCardReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"UserCheckCardReport[Error]:{ex}");
        }
    }

    public virtual async Task SendPanelProcessDataReport(PanelProcessDataReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.PanelProcessDataReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new PanelProcessDataReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "PanelProcessDataReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("PanelProcessDataReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<PanelProcessDataReportModel, object>(Url, data, _jsonSerializerOptions) ?? string.Empty;

            _logger.LogInformation("PanelProcessDataReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"PanelProcessDataReport[Error]:{ex}");
        }
    }

    public virtual async Task SendBrokenKnifeAlarmReport(BrokenKnifeAlarmReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.BrokenKnifeAlarmReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new BrokenKnifeAlarmReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "BrokenKnifeAlarmReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("BrokenKnifeAlarmReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<BrokenKnifeAlarmReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("BrokenKnifeAlarmReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"BrokenKnifeAlarmReport[Error]:{ex}");
        }
    }

    public virtual async Task SendIPPortReport(IPPortReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.IPPortReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new IPPortReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "IPPortReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("IPPortReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<IPPortReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("IPPortReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"IPPortReport[Error]:{ex}");
        }
    }

    public virtual async Task SendMaterialValidationRequest(MaterialValidationRequestBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.MaterialValidationRequestUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new MaterialValidationRequestModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "MaterialValidationRequest",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("MaterialValidationRequest[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<MaterialValidationRequestModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("MaterialValidationRequest[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"MaterialValidationRequest[Error]:{ex}");
        }
    }

    public virtual async Task SendMaterialUseReport(MaterialUseReportBody reportBody)
    {
        try
        {
            string Url = _reporterOptions.MaterialUseReportUrl;
            if (string.IsNullOrWhiteSpace(Url))
            {
                return;
            }

            var data = new MaterialUseReportModel
            (
                header: new EQPReportHeader
                {
                    MessageName = "MaterialUseReport",
                    UserID = _eAPClientOptions.UserID,
                    TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
                },
                body: reportBody,
                result: new EQPReportResult()
            );

            _logger.LogInformation("MaterialUseReport[EQP -> EAP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{data.ToJsonNull()}" + "\r\n");

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<MaterialUseReportModel, object>(Url, data, _jsonSerializerOptions);

            _logger.LogInformation("MaterialUseReport[EAP -> EQP]" + "\r\n"
                + $@"Url:{Url}" + "\r\n"
                + $@"Body:{return_code}" + "\r\n");

        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"MaterialUseReport[Error]:{ex}");
        }
    }
}
