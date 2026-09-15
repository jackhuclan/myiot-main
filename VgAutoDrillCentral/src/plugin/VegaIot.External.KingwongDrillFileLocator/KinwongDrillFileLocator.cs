using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.KinwongDrillFileLocator;

public class KinwongDrillFileLocator : IDrillFilePathLocator
{
    private readonly DrillFilePathOptions _drillFilePathOptions;

    public KinwongDrillFileLocator(IOptions<DrillFilePathOptions> options)
    {
        _drillFilePathOptions = options == null ? new DrillFilePathOptions
        {
            StoreType = DrillFilePathKind.Local,
            StorePath = string.Empty,
        } : options.Value;
    }

    public DrillInfo GetFilePath(string itemCode)
    {
        return new DrillInfo();
    }
}
