namespace VgAutoDrill.Fundation.Drill;

public class EmptyDrillFilePathLocator : IDrillFilePathLocator
{
    public DrillInfo GetFilePath(string itemCode) => new DrillInfo();
}
