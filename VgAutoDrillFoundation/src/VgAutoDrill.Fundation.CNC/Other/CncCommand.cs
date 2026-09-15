namespace VgAutoDrill.Fundation.CNC.Other;

public class CncCommand
{
    public string id;
    public string firstCommandkey;
    public string SecondCommandkey;
    public string commndText = "";
    public bool isGetText = true; // false   获取参数值

    public CncCommand()
    {

    }

    public CncCommand(string id, string firstCommandkey, string SecondCommandkey, string commndText, bool isGetText)
    {
        this.firstCommandkey = firstCommandkey;
        this.SecondCommandkey = SecondCommandkey;
        this.commndText = commndText;
        this.isGetText = isGetText;
        this.id = id;

    }
}
