using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;

namespace UnitTest.VgAutoDrill.Central;

internal class MockPartition : Partition
{
    public override IReadOnlyList<Location> RackLocations => new List<Location>();
}
