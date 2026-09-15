// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VgAutoDrill.Fundation.Iot;
using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

internal class CncFileLoader : ICncFileLoader
{
    public event Action<string>? OnLoadFileFailed;

    public event Action<string>? OnParseFileFailed;

    private readonly EAPClientOptions _eAPClientOptions;

    private readonly ILogger<CncFileLoader> _logger;
    private readonly IMesAtpParser _dingTaiAtpParser;
    private readonly IGuiLogger _guiLogger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAtpFileParser _atpFileParser;
    private readonly ICNCConnector _cNCConnector;

    public CncFileLoader(IOptions<EAPClientOptions> options,
        ICNCOperatorProvider cNCOperatorProvider,
        IMesAtpParser mesAtpParser,
        IGuiLogger guiLogger,
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory)
    {
        _eAPClientOptions = options.Value;
        _logger = loggerFactory.CreateLogger<CncFileLoader>();
        _dingTaiAtpParser = mesAtpParser;
        _guiLogger = guiLogger;
        _httpClientFactory = httpClientFactory;
        _atpFileParser = cNCOperatorProvider.GetAtpFileParser();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
    }

    public MessageEntity GenerateNewAtp(string strMesAtpPath, string strDiaPath, string strGeneratePath = "")
    {
        string strFailReason = string.Empty;
        try
        {
            string LastestAtpPath = string.Empty;
            var saveatpMsg = SaveCurrentATP();
            _logger.LogInformation($"GenerateNewAtp - {saveatpMsg.Information}");
            if (saveatpMsg.IsSuccessful)
            {
                LastestAtpPath = saveatpMsg.Data?.ToString();
            }
            else
            {
                return saveatpMsg;
            }

            if (!string.IsNullOrWhiteSpace(LastestAtpPath) && File.Exists(LastestAtpPath))
            {
                AtpDTO orgAtpDto = _atpFileParser.ParseFromFile(LastestAtpPath);

                if (!string.IsNullOrWhiteSpace(strMesAtpPath) && File.Exists(strMesAtpPath))
                {
                    string strGenerateFileName = Path.GetFileName(strMesAtpPath);
                    if (!string.IsNullOrWhiteSpace(strGenerateFileName))
                    {
                        return _dingTaiAtpParser.GenerateAtpFile(strMesAtpPath, strDiaPath, orgAtpDto, strGenerateFileName, strGeneratePath);
                    }
                    else
                    {
                        strFailReason += $"获取生成文件名失败 - {strMesAtpPath}";
                    }
                }
                else
                {
                    strFailReason += $"MES ATP 文件为空或不存在 - {strMesAtpPath}";
                }
            }
            else
            {
                strFailReason += $"GetLastestATP - 获取最新的ATP文件为空或不存在 - {_eAPClientOptions.ATPFileDir}， {LastestAtpPath}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = $"生成ATP文件异常 - {ex.Message} - {strMesAtpPath}， {strDiaPath}， {strGeneratePath}",
                Timestamp = DateTime.Now,
                Data = ""
            };
        }
        return new MessageEntity
        {
            IsSuccessful = false,
            Information = $"GenerateNewAtp失败 - {strFailReason} - {strMesAtpPath}， {strDiaPath}， {strGeneratePath}",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    public MessageEntity SaveCurrentATP()
    {
        string strFailReason = string.Empty;
        int nTryCount = 0;
        try
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saveAtp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //生成路径
            string resultPath = Path.Combine(folderPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".ATP");

            while (nTryCount <= 3)
            {
                nTryCount++;

                _cNCConnector.RetrieveData($@"SaveAtpFile_{resultPath}");
                Thread.Sleep(6000);

                if (File.Exists(resultPath))
                {
                    return new MessageEntity
                    {
                        IsSuccessful = true,
                        Information = $"SaveCurrentATP 成功 - {resultPath}",
                        Timestamp = DateTime.Now,
                        Data = resultPath
                    };
                }
            }
        }
        catch (Exception ex)
        {
            strFailReason = ex.Message;
        }
        return new MessageEntity
        {
            IsSuccessful = false,
            Information = $"SaveCurrentATP 失败 - 保存刀具参数到ATP失败,请检查当前设备状态是否可以保存刀具参数",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    public async Task LoadFile(string workOrder)
    {
        _logger.LogDebug($"Cnc File Loader : Load file. work order '{workOrder}'. Op mode {_eAPClientOptions.OperationMode}.");
        if (_eAPClientOptions.OperationMode == "1")//手动模式
        {
            var resultStr = File.ReadAllText("Arrange.Json");
            _logger.LogInformation(resultStr);
            JObject jo = JObject.Parse(resultStr);
            if (jo["code"].ToString() == ErrorCodes.Sys.FAIL)
            {
                _logger.LogInformation($"{jo["info"].ToString()}，请检查工单后重试。");
                //MessageBox.Show($"{jo["info"].ToString()}，请检查工单后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnLoadFileFailed?.Invoke($"{jo["info"].ToString()}，请检查工单后重试。");
            }

            if (jo["code"].ToString() == ErrorCodes.Sys.SUCCESS)
            {
                try
                {
                    MessageEntity messageEntity = _atpFileParser.ParseToFile(resultStr, GetLastestATP(), _eAPClientOptions.DIAFile);
                    _logger.LogInformation(messageEntity.Information.ToString());
                    if (!messageEntity.IsSuccessful)
                    {
                        _guiLogger.ShowResult("产生ATP出现问题" + messageEntity.Information, "Error");
                        //MessageBox.Show("产生ATP出现问题->" + messageEntity.Information);
                        OnParseFileFailed?.Invoke($"{jo["info"].ToString()}，请检查工单后重试。");
                    }
                    else
                    {
                        string ATPFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vegaAtp") + $@"\{workOrder.Trim()}.ATP";
                        //string ATPFile = Path.Combine(Application.StartupPath, "vegaAtp") + $@"\{txtWorkOrder.Text.Trim()}.ATP";
                        _logger.LogDebug($"Load file : by cnc file loader. {LoadFileType.ATP}, '{ATPFile}'.");
                        var ack = await _cNCConnector.CncLoadFile(LoadFileType.ATP, ATPFile);

                        if (ack)
                        {
                            _logger.LogInformation("发送给CNC84 GetExecuteLoadAtp 成功。");
                            _guiLogger.ShowResult("发送给CNC84 GetExecuteLoadAtp 成功", "LoadAtp");
                        }
                        else
                        {
                            _logger.LogError("发送给CNC84 GetExecuteLoadAtp 失败。");
                            _guiLogger.ShowResult("发送给CNC84 GetExecuteLoadAtp 失败", "Error");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    _guiLogger.ShowResult("PostAsync:排布文件结果异常：" + ex.Message, "Error");
                }
            }
        }
        else//自动模式
        {
            var httpClient = _httpClientFactory.CreateClient();
            {
                var ATPFile = GetLastestATP();
                AtpDTO atpDTO = _atpFileParser.ParseFromFile(ATPFile);
                /*
                 {
                    "itemCode": "item01",
                    "macNo": "Vega0001",
                    "data": {
                        "arranges": [
                            {
                                "boxIndex": 1,
                                "site": 1,
                                "pgmDiameter": 3.0,
                                "moCount": 0,
                                "life": 5
                            },
                            {
                                "boxIndex": 1,
                                "site": 2,
                                "pgmDiameter": 4.0,
                                "moCount": 0,
                                "life": 10
                            }
                        ]
                    }
                }
                 */
                List<Dictionary<string, string>> arranges = new List<Dictionary<string, string>>();
                foreach (var dto in atpDTO.magazineDTOs)
                {
                    if (dto.magazineId < 1 || dto.magazineId > 350)
                        continue;
                    Dictionary<string, string> detail = new Dictionary<string, string>();
                    {
                        detail.Clear();
                        detail.Add("boxCode", "");
                        detail.Add("spindle", "");
                        if (dto.magazineId < 51)
                        {
                            detail.Add("boxIndex", "1"); detail.Add("site", dto.magazineId.ToString());
                        }
                        else if (dto.magazineId < 101)
                        {
                            detail.Add("boxIndex", "2"); detail.Add("site", (dto.magazineId - 50).ToString());
                        }
                        else if (dto.magazineId < 151)
                        {
                            detail.Add("boxIndex", "3"); detail.Add("site", (dto.magazineId - 100).ToString());
                        }
                        else if (dto.magazineId < 201)
                        {
                            detail.Add("boxIndex", "4"); detail.Add("site", (dto.magazineId - 150).ToString());
                        }
                        else if (dto.magazineId < 251)
                        {
                            detail.Add("boxIndex", "5"); detail.Add("site", (dto.magazineId - 200).ToString());
                        }
                        else if (dto.magazineId < 301)
                        {
                            detail.Add("boxIndex", "6"); detail.Add("site", (dto.magazineId - 250).ToString());
                        }
                        else if (dto.magazineId < 501)
                        {
                            detail.Add("boxIndex", "7"); detail.Add("site", (dto.magazineId - 300).ToString());
                        }
                        detail.Add("pgmdiameter", dto.toolDiameter.ToString());
                        //detail.Add("mocount", "0");
                        detail.Add("life", (dto.toolLife - dto.toolUseLife).ToString());
                    }
                    arranges.Add(detail);
                }
                Dictionary<string, object> data = new() { { "arranges", arranges } };
                Dictionary<string, object> dic = new() { { "itemCode", workOrder.Trim() }, { "data", data } };

                string jsonData = JsonConvert.SerializeObject(dic);
                //jsonData = "{\"itemCode\":\"item01\",\"data\":{\"arranges\":[{\"boxIndex\":1,\"site\":1,\"pgmDiameter\":3.0,\"moCount\":0,\"life\":5},{\"boxIndex\":1,\"site\":2,\"pgmDiameter\":4.0,\"moCount\":0,\"life\":10}]}}";
                using (HttpContent content = new StringContent(jsonData))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    _logger.LogInformation("PostAsync:排布文件->" + _eAPClientOptions.GetArrangeDataAddr);
                    _logger.LogInformation("FormData:jsonData->" + jsonData.ToString());
                    var httpResponseMessage = await httpClient.PostAsync($"{_eAPClientOptions.GetArrangeDataAddr}", content);

                    _logger.LogInformation("StatusCode->" + httpResponseMessage.StatusCode.ToString());
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var resultTask = httpResponseMessage.Content.ReadAsStringAsync();
                        resultTask.Wait();
                        var resultStr = resultTask.Result;
                        if (File.Exists("Arrange.Json"))
                        {
                            File.Delete("Arrange.Json");
                        }
                        File.WriteAllText("Arrange.Json", resultStr);
                        _logger.LogInformation(resultStr);
                        JObject jo = JObject.Parse(resultStr);
                        if (jo["code"].ToString() == ErrorCodes.Sys.FAIL)
                        {
                            _logger.LogInformation($"{jo["info"].ToString()}，请检查工单后重试。");
                            //MessageBox.Show($"{jo["info"].ToString()}，请检查工单后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            OnLoadFileFailed?.Invoke($"{jo["info"].ToString()}，请检查工单后重试。");
                        }
                        if (jo["code"].ToString() == ErrorCodes.Sys.SUCCESS)
                        {
                            try
                            {
                                MessageEntity messageEntity = _atpFileParser.ParseToFile(resultStr, GetLastestATP(), _eAPClientOptions.DIAFile);
                                _logger.LogInformation(messageEntity.Information.ToString());
                                if (messageEntity.IsSuccessful == false)
                                {
                                    _guiLogger.ShowResult("产生ATP出现问题" + messageEntity.Information, "Error");
                                    //MessageBox.Show("产生ATP出现问题->" + messageEntity.Information);
                                    OnParseFileFailed?.Invoke("产生ATP出现问题->" + messageEntity.Information);
                                }
                                else
                                {
                                    string atpFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _eAPClientOptions.ATPFileDir) + $@"\{workOrder.Trim()}.ATP";
                                    _logger.LogDebug($"Load file : by cnc file loader. {LoadFileType.ATP}, '{atpFile}'.");
                                    var ack = await _cNCConnector.CncLoadFile(LoadFileType.ATP, atpFile);

                                    if (ack)
                                    {
                                        string cmd = "DSP,FILE ATP LOADED SUCESSFULLY!";
                                        await _cNCConnector.LoadCNCCommand(cmd);
                                        _logger.LogInformation("发送给CNC84 GetExecuteLoadAtp 成功。");
                                        _guiLogger.ShowResult("发送给CNC84 GetExecuteLoadAtp 成功", "LoadAtp");
                                    }
                                    else
                                    {
                                        string cmd = "DSP,FILE ATP LOAD FAILED!";
                                        await _cNCConnector.LoadCNCCommand(cmd);
                                        _logger.LogError("发送给CNC84 GetExecuteLoadAtp 失败。");
                                        _guiLogger.ShowResult("发送给CNC84 GetExecuteLoadAtp 失败", "Error");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.ToString());
                                _guiLogger.ShowResult("PostAsync:排布文件结果异常：" + ex.Message, "Error");
                            }
                        }
                    }
                    else
                    {
                        httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        _guiLogger.ShowResult("请求配针排布文件失败，请检查网络后重试", "Error");
                        //请求异常
                        //MessageBox.Show("请求配针排布文件失败，请检查网络后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    private string GetLastestATP()
    {
        try
        {
            string atp = string.Empty;
            var AtpFolder = new DirectoryInfo(_eAPClientOptions.ATPFileDir);
            var atpFile = AtpFolder.GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
            _logger.LogInformation($"GetLastestATP - 最新的SVTP文件为 - {atpFile}");
            return atpFile?.FullName ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"GetLastestATP 异常 - {ex.Message}，ATPFileDir-{_eAPClientOptions.ATPFileDir}");
        }
        return string.Empty;
    }
}
