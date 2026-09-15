namespace VgAutoDrill.Fundation.Pipe;

public class PipeClientOptions
{
    public bool Enabled { get; set; } = false;
    public string ServerName { get; set; } = ".";
    public string PipeName { get; set; } = "localpipe";
    public int MinBufferSize { get; set; } = 4096;
}
