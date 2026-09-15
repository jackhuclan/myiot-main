namespace VgMySqlStore.Plugin.Models;

public class EquipmentPermitRequest
{
    public string EquCode { get; set; } = "";
    public string CardCode { get; set; } = "";
    public string OpCode { get; set; } = "";
}

public class EquipmentPermitRespones
{
    public string EmpNo { get; set; } = "";
    public string EmpName { get; set; } = "";
    public string Permit { get; set; } = "";
    public string Code { get; set; } = "";
}
public enum OpCodeEnum
{

    login,
    reset,
    alter
}
