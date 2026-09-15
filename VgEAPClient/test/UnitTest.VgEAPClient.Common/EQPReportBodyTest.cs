using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Utils;
using VgEAPClient.Common.Communication;

namespace UnitTest.VgEAPClient.Common;

public class EQPReportBodyTest
{
    [Fact]
    public void TestEQPReportBodySerialization()
    {
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };
        var body = new EQPReportBody();
        var json = JsonSerializer.Serialize<EQPReportBody>(body, jsonSerializerOptions);
        Assert.NotNull(json);
    }


    [Fact]
    public void TestChar()
    {
        char s = (char)2;
        char e = (char)3;

        string textjson = new CMD
        {
            Cmd = "EquipmentStatus",
            SessonId = DateTime.Now.ToString("HHmmssfff")
        }.ToJson(jsonSerializer: new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });


        string s_str = $@"{s}{textjson}{e}";
        byte[] bytes = Encoding.UTF8.GetBytes(s_str);

    }
}

public class CMD
{
    public string Cmd { get; set; } = string.Empty;
    public string SessonId { get; set; } = string.Empty;

    public string EquipmentStatus { get; set; } = string.Empty;
    public string AlarmID { get; set; } = string.Empty;
    public string AlarmLevel { get; set; } = string.Empty;
    public string AlarmStatus { get; set; } = string.Empty;
    public string AlarmText { get; set; } = string.Empty;


}
