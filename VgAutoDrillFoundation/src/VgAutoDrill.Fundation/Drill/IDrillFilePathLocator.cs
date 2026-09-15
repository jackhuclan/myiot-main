namespace VgAutoDrill.Fundation.Drill;

public interface IDrillFilePathLocator
{
    DrillInfo GetFilePath(string itemCode);
}
public interface IDrillFileLoadResult
{
    void WriteFileLoadResult(string itemCode);
}

public interface IDrillWriteRunInformation
{
    void WriteRunInformation(string content);
}
public interface IDrillLoadPanelToDrillComplete
{
    void PanelToDrillComplete(string content);
}

public interface IDrillLoadPanelSNToDB
{
    void LoadPanelSNToDB(string LotInfo);
}
