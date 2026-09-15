using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Calculator;

public interface ICalcutorUseAPI
{
    Task<bool> IsHaveEmptyLocation(TransportationKind transportationKind);

    Task<bool> IsHaveRawStock(TransportationKind transportationKind, string lot, string pnlStatus);

    Task<bool> IsHaveEmptySilo(TransportationKind transportationKind);
}
