namespace VgAutoDrill.Fundation.CNC.Model;

public class VgCncError
{
    public string DateString { get; set; }
    public string ProgrameName { get; set; }
    public DateTime CurrentTime { get; set; }
    public int ErrorID { get; set; }
    public string ErrorMessage { get; set; }

}
