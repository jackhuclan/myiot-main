namespace VgEAPClient.Recipe.Bomin;

internal class GetDrillRecipesManualReqest
{
    public string Lot { get; set; } = string.Empty;
    public string ProcessCode { get; set; } = string.Empty;
    public string MachineCode { get; set; } = string.Empty;
}
