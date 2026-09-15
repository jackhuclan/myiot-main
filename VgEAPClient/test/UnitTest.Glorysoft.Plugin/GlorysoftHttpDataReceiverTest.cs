using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VgEAPClient.Common;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Inbound;

namespace UnitTest.Glorysoft.Plugin;

public class GlorysoftHttpDataReceiverTest
{
    [Fact]
    public void Test()
    {
        var json2 = "{\"header\":{\"messageName\":\"AreYouThereReply\",\"transactionID\":\"20250321102235357478\",\"userID\":\"test\"},\"body\":{\"equipmentID\":\"\",\"subEQPID\":null,\"recipeID\":null,\"fileName\":null,\"filePath\":null,\"fileCheckResult\":null,\"productNO\":null,\"panelQTY\":null,\"panelID\":null,\"stripID\":null,\"aLuminiumID\":null,\"drillID\":null,\"eqpSection\":null,\"sequenceNO\":null,\"isOK\":null,\"dateTime\":null,\"message\":null,\"alarmID\":null,\"alarmLevel\":null,\"alarmStatus\":null,\"alarmText\":null,\"communicationStatus\":null,\"totalOnTime\":null,\"totalProTime\":null,\"status\":null,\"lotID\":null,\"lotQTY\":null,\"length\":null,\"width\":null,\"thickness\":null,\"portID\":null,\"outerID\":null,\"no\":null,\"innerID\":null,\"exposureID\":null,\"action\":null,\"portStatus\":null,\"carrierID\":null,\"slotID\":null,\"recipeList\":null,\"parameterList\":null,\"pn\":null,\"pnlThickness\":null,\"pnlWidth\":null,\"pnlLength\":null,\"codeSide\":null,\"codeMatrix\":null,\"locationPointList\":null,\"codeInfoList\":null},\"result\":{\"code\":\"1\",\"messageCH\":\"\",\"messageEN\":\"\"}}";
        var obj2 = JsonSerializer.Deserialize<AreYouThereReplyModel>(json2, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        Assert.Equal("AreYouThereReply", obj2.Header.MessageName);
    }

    [Fact]
    public async Task TestAreYouThere_ShouldOk()
    {
        var obj = StartHttpDataReceiver();
        var httpDataReceiverOptions = obj.Item1;
        var httpDataReceiver = obj.Item2;

        httpDataReceiver.OnAreYouThereReplyReceived += (model) =>
        {
            return Task.FromResult(model);
        };

        var areYouThereReplyModel = new AreYouThereReplyModel()
        {
            Header = new EQPReportHeader { MessageName = "AreYouThereReply", UserID = "test", TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff") },
            Body = new EQPReportBody { },
            Result = new EQPReportResult()
        };

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(httpDataReceiverOptions.AreYouThereReplyUrl, areYouThereReplyModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = await response.Content.ReadAsStringAsync();
        var areYouThereReplyModelExpected = JsonSerializer.Deserialize<AreYouThereReplyModel>(content);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(areYouThereReplyModelExpected?.Header.TransactionID, areYouThereReplyModel.Header.TransactionID);
    }

    [Fact]
    public async Task TestDateTimeCommand_ShouldOk()
    {
        var obj = StartHttpDataReceiver();
        var httpDataReceiverOptions = obj.Item1;
        var httpDataReceiver = obj.Item2;

        httpDataReceiver.OnDateTimeCommandReceived += (model) =>
        {
            return Task.FromResult(model);
        };

        var dateTimeCommandModel = new DateTimeCommandModel()
        {
            Header = new EQPReportHeader { MessageName = "DateTimeCommand", UserID = "test", TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff") },
            Body = new DateTimeCommandBody { EquipmentID = "test", DateTime = "20250402123456" },
            Result = new EQPReportResult()
        };

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(httpDataReceiverOptions.DateTimeCommandUrl, dateTimeCommandModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = await response.Content.ReadAsStringAsync();
        var expectedModel = JsonSerializer.Deserialize<AreYouThereReplyModel>(content);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(expectedModel?.Header.TransactionID, dateTimeCommandModel.Header.TransactionID);
    }

    [Fact]
    public async Task TestCIMMessageCommand_ShouldOk()
    {
        var obj = StartHttpDataReceiver();
        var httpDataReceiverOptions = obj.Item1;
        var httpDataReceiver = obj.Item2;

        httpDataReceiver.OnCIMMessageCommandReceived += (model) =>
        {
            return Task.FromResult(model);
        };

        var CIMMessageCommandModel = new CIMMessageCommandModel()
        {
            Header = new EQPReportHeader { MessageName = "CIMMessageCommand", UserID = "test", TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff") },
            Body = new CIMMessageCommandBody { Message = "test333" },
            Result = new EQPReportResult()
        };

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(httpDataReceiverOptions.CIMMessageCommandUrl, CIMMessageCommandModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = await response.Content.ReadAsStringAsync();
        var expectedModel = JsonSerializer.Deserialize<AreYouThereReplyModel>(content);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(expectedModel?.Header.TransactionID, CIMMessageCommandModel.Header.TransactionID);
    }

    [Fact]
    public async Task TestLotInfoDownloadCommand_ShouldOk()
    {
        var obj = StartHttpDataReceiver();
        var httpDataReceiverOptions = obj.Item1;
        var httpDataReceiver = obj.Item2;

        httpDataReceiver.OnLotInfoDownloadCommandReceived += (model) =>
        {
            return Task.FromResult(model);
        };

        var LotInfoDownloadCommandModel = new LotInfoDownloadCommandModel()
        {
            Header = new EQPReportHeader { MessageName = "LotInfoDownloadCommand", UserID = "test", TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff") },
            Body = new LotInfoDownloadCommandBody
            {
                ProductNo = "123",
                LotID = "9876555",
                ItemNum = "789",
                PnlWidth = "12",
                PnlLength = "34",
                PnlThick = "56",
                PanelQTY = "78",
                RecipeID = "D:\\test.drl"
            },
            Result = new EQPReportResult()
        };

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(httpDataReceiverOptions.LotInfoDownloadCommandUrl, LotInfoDownloadCommandModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = await response.Content.ReadAsStringAsync();
        var expectedModel = JsonSerializer.Deserialize<AreYouThereReplyModel>(content);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(expectedModel?.Header.TransactionID, LotInfoDownloadCommandModel.Header.TransactionID);
    }

    [Fact]
    public async Task TestRecipeValidationResultCommand_ShouldOk()
    {
        var obj = StartHttpDataReceiver();
        var httpDataReceiverOptions = obj.Item1;
        var httpDataReceiver = obj.Item2;

        httpDataReceiver.OnRecipeValidationResultCommandReceived += (model) =>
        {
            return Task.FromResult(model);
        };

        var RecipeValidationResultCommandModel = new RecipeValidationResultCommandModel()
        {
            Header = new EQPReportHeader { MessageName = "RecipeValidationResultCommand", UserID = "test", TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff") },
            Body = new RecipeValidationResultCommandBody { EquipmentID = "test", RecipeID = "E:\\2.drl", Result = 1 },
            Result = new EQPReportResult()
        };

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync(httpDataReceiverOptions.RecipeValidationResultCommandUrl, RecipeValidationResultCommandModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        var content = await response.Content.ReadAsStringAsync();
        var expectedModel = JsonSerializer.Deserialize<RecipeValidationResultCommandModel>(content);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(expectedModel?.Header.TransactionID, RecipeValidationResultCommandModel.Header.TransactionID);
    }

    private static Tuple<HttpDataReceiverOptions, IEQPDataReceiver> StartHttpDataReceiver()
    {
        var httpDataReceiverOptions = new HttpDataReceiverOptions()
        {
            AreYouThereReplyUrl = "http://127.0.0.1:5001/restApi/AreYouThereReply",
            DateTimeCommandUrl = "http://127.0.0.1:5001/restApi/DateTimeCommand",
            CIMMessageCommandUrl = "http://127.0.0.1:5001/restApi/CIMMessageCommand",
            LotInfoDownloadCommandUrl = "http://127.0.0.1:5001/restApi/LotInfoDownloadCommand",
            RecipeValidationResultCommandUrl = "http://127.0.0.1:5001/restApi/RecipeValidationResultCommand"
        };
        var services = new ServiceCollection();
        services.AddSingleton(new MockObject<ILogger<DefaultEQPDataReceiver>>().Mock().Object);
        services.AddSingleton(new MockObject<IHostApplicationLifetime>().Mock().Object);
        services.AddSingleton(new MockOptions<HttpDataReceiverOptions>().Mock(httpDataReceiverOptions).Object);
        services.AddSingleton(new MockOptions<EAPClientOptions>().Mock(new EAPClientOptions()
        {
        }).Object);
        services.AddSingleton<IEQPDataReceiver, DefaultEQPDataReceiver>();

        var cancellationToken = new CancellationTokenSource().Token;
        var serviceProvider = services.BuildServiceProvider();
        var httpDataReceiver = serviceProvider.GetRequiredService<IEQPDataReceiver>();
        //_ = httpDataReceiver.StartAsync(cancellationToken);
        return Tuple.Create(httpDataReceiverOptions, httpDataReceiver);
    }
}
