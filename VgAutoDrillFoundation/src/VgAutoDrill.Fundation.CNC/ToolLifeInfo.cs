namespace VgAutoDrill.Fundation.CNC;

public class ToolLifeInfo
{

    public ToolLifeInfo(int position) : this(position, "")
    {


    }
    public ToolLifeInfo(int position, string toolUsedLife) : this(position, toolUsedLife, ToolLifeType.N)
    {


    }

    public ToolLifeInfo(int position, ToolLifeType toolLifeType) : this(position, "", toolLifeType)
    {


    }
    public ToolLifeInfo(int position, string toolUsedLife, ToolLifeType toolLifeType)
    {
        Position = position;
        ToolUsedLife = toolUsedLife;
        ToolLifeType = toolLifeType;
    }
    public int Position { get; set; }
    public string ToolUsedLife { get; set; }
    public ToolLifeType ToolLifeType { get; set; }
}

public enum ToolLifeType
{
    N,
    U,
    B,
    L,
    E,
    D,
    R,
    GN,
    GU,
    GB,
    GL,
    GE,
    GD,
    GR

}
