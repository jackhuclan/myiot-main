namespace VgAutoDrill.Fundation.CNC;

public interface IInovanceModbusIp
{
    public bool isConnected { get; }

    public bool Connect(string ip, int port);

    public void DisConnect();

    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> Y64512-  64767</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadY(string varAddress, ushort length);

    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> Y64512-  64767</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadX(string varAddress, ushort length);

    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> M0-  7679</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadM(string varAddress, ushort length);

    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> D0 -  8000</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public ushort[] ReadD(string varAddress, ushort length);

    public void WriteY(string varAddress, bool[] data);

    public void WriteM(string varAddress, bool[] data);

    public void WriteD(string varAddress, ushort[] data);
}
