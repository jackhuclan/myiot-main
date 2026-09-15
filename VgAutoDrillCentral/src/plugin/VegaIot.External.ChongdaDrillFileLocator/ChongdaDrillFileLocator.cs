using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.ChongdaDrillFileLocator;

public class ChongdaDrillFileLocator : IDrillFilePathLocator
{
    private DrillFilePathOptions _drillFilePathOptions;
    public ChongdaDrillFileLocator(IOptions<DrillFilePathOptions> options)
    {
        _drillFilePathOptions = options == null ? new DrillFilePathOptions
        {
            StoreType = DrillFilePathKind.Local,
            StorePath = string.Empty,
        } : options.Value;
    }

    public DrillInfo GetFilePath(string itemCode)
    {
        return new VgAutoDrill.Fundation.Drill.DrillInfo();
    }
}
