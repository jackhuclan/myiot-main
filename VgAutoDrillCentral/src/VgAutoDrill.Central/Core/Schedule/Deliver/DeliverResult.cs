namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal class DeliverResult
{
    public DeliverResult(DeliverResultCode code)
    {
        Code = code;
    }

    public DeliverResult(DeliverResultCode code,
        DeliverFailedReason failedReason)
    {
        Code = code;
        Reason = failedReason;
    }

    public DeliverResultCode Code { get; }
    public DeliverFailedReason Reason { get; }
}
