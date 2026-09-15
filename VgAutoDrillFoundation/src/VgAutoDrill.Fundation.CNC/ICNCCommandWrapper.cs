namespace VgAutoDrill.Fundation.CNC;

public interface ICNCCommandWrapper
{
    ICNCCommand? CNCCommand { get; }
    ICNCCommand CreateCNCCommand();
}
