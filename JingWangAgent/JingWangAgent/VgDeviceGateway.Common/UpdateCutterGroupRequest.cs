namespace VgDeviceGateway.Devices.Common;

public class UpdateCutterGroupRequest
{
    public string groupNo { get; set; }
    public int groupStatus { get; set; }
    public string atpFile { get; set; }
    public string deviceNo { get; set; }
}

public class UpdateCutterGroupResponse
{
    public int code { get; set; }
    public string message { get; set; }
    public bool data { get; set; }
}

public enum CUTTERGROUPSTATUS
{
    UnLock = 0,
    Lock = 1,
    BoxVerifyEnd = 20,
    LoadDrlFinished = 30,
    LoadAtpFinished = 40
};
