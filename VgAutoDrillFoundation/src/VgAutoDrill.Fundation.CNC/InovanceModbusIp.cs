using System.Net.Sockets;
using Modbus.Device;
using VgAutoDrill.Fundation.CNC;

namespace VgAutoDrill.Fundation.Utils;

public class InovanceModbusIp : IInovanceModbusIp
{
    public bool isConnected { get; private set; } = false;

    public bool m_bUpdataFlag = false;
    private TcpClient tcpClient = null;
    private ModbusIpMaster modbusIpMaster = null;
    #region 建立和关闭连接
    public bool Connect(string ip, int port)
    {
        tcpClient = new TcpClient();
        try
        {
            var result = tcpClient.BeginConnect(ip, port, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                return false;
            }
            tcpClient.EndConnect(result);
            modbusIpMaster = ModbusIpMaster.CreateIp(tcpClient);

        }
        catch (Exception)
        {
            return false;
        }
        isConnected = true;
        return true;
    }

    public void DisConnect()
    {
        tcpClient?.Close();
        isConnected = false;
    }
    #endregion
    #region 读数据
    #region Y
    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> Y64512-  64767</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadY(string varAddress, ushort length)
    {
        if (!isConnected) return null;
        if (varAddress.StartsWith("Y")) return ReadYArea(varAddress, length);
        if (isNumeric(varAddress))
        {
            return ReadYArea(Convert.ToUInt16(varAddress), length);
        }
        return null;
    }
    private bool[] ReadYArea(string varAddress, ushort length)
    {
        return ReadBitArea("Y", 64512, varAddress, length, 8);
    }

    private bool[] ReadYArea(int varAddress, ushort length)
    {

        if (varAddress >= 64512 && varAddress <= 64767)
        {
            return modbusIpMaster.ReadCoils(Convert.ToUInt16(varAddress), length);

        }
        return null;
    }
    #endregion
    #region X
    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> Y64512-  64767</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadX(string varAddress, ushort length)
    {
        if (!isConnected) return null;
        if (varAddress.StartsWith("X")) return ReadXArea(varAddress, length);
        if (isNumeric(varAddress))
        {
            return ReadXArea(Convert.ToUInt16(varAddress), length);
        }
        return null;
    }
    private bool[] ReadXArea(string varAddress, ushort length)
    {
        return ReadBitArea("X", 63488, varAddress, length, 8);
    }

    private bool[] ReadXArea(int varAddress, ushort length)
    {

        if (varAddress >= 63488 && varAddress <= 63743)
        {
            return modbusIpMaster.ReadInputs(Convert.ToUInt16(varAddress), length);

        }
        return null;
    }
    #endregion
    #region M
    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> M0-  7679</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public bool[] ReadM(string varAddress, ushort length)
    {
        if (!isConnected) return null;
        if (varAddress.StartsWith("M")) return ReadMArea(varAddress, length);
        if (isNumeric(varAddress))
        {
            return ReadMArea(Convert.ToUInt16(varAddress), length);
        }
        return null;
    }
    private bool[] ReadMArea(string varAddress, ushort length)
    {
        return ReadBitArea("M", 0, varAddress, length);
    }

    private bool[] ReadMArea(int varAddress, ushort length)
    {

        if (varAddress >= 0 && varAddress <= 7679)
        {
            return modbusIpMaster.ReadCoils(Convert.ToUInt16(varAddress), length);


        }
        return null;
    }
    #endregion
    #region D

    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="varAddress"> D0 -  8000</param>
    /// <param name="length">读个数</param>
    /// <returns></returns>
    public ushort[] ReadD(string varAddress, ushort length)
    {
        if (!isConnected) return null;
        if (varAddress.StartsWith("D")) return ReadDArea(varAddress, length);
        if (isNumeric(varAddress))
        {
            return ReadDArea(Convert.ToUInt16(varAddress), length);
        }
        return null;
    }
    private ushort[] ReadDArea(string varAddress, ushort length)
    {
        return ReadWArea("D", 0, varAddress, length);

    }

    private ushort[] ReadDArea(int varAddress, ushort length)
    {

        if (varAddress >= 0 && varAddress <= 8511)
        {
            return modbusIpMaster.ReadHoldingRegisters(Convert.ToUInt16(varAddress), length);

        }
        return null;
    }

    #endregion

    private bool isNumeric(string str)
    {
        char[] ch = new char[str.Length];
        ch = str.ToCharArray();
        for (int i = 0; i < ch.Length; i++)
        {
            if (ch[i] < 48 || ch[i] > 57)
                return false;
        }
        return true;
    }
    //进制
    private bool[] ReadBitArea(string startChar, int startAddress, string varAddress, ushort length, int fromBase = 10)
    {
        if (varAddress.Length == 0) return null;
        if (!varAddress.StartsWith(startChar)) return null;
        int addressIndex = parseMelsecBitLocation(varAddress, fromBase);
        if (addressIndex == -1) return null;
        if (startChar.Equals("X"))
        {
            return modbusIpMaster.ReadInputs(Convert.ToUInt16(startAddress + addressIndex), length);
        }
        return modbusIpMaster.ReadCoils(Convert.ToUInt16(startAddress + addressIndex), length);
    }

    private ushort[] ReadWArea(string startChar, int startAddress, string varAddress, ushort length)
    {
        if (varAddress.Length == 0) return null;
        if (!varAddress.StartsWith(startChar)) return null;
        int addressIndex = Convert.ToInt32(varAddress.Substring(1));
        if (addressIndex == -1) return null;
        return modbusIpMaster.ReadHoldingRegisters(Convert.ToUInt16(startAddress + addressIndex), length);
    }

    private void WriteBitArea(string startChar, int startAddress, string varAddress, bool[] data, int fromBase = 10)
    {
        if (varAddress.Length == 0) return;
        if (!varAddress.StartsWith(startChar)) return;
        int addressIndex = parseMelsecBitLocation(varAddress, fromBase);
        if (addressIndex == -1) return;
        modbusIpMaster.WriteMultipleCoils(Convert.ToUInt16(startAddress + addressIndex), data);
    }


    private int parseMelsecBitLocation(string varAddress, int fromBase)
    {
        if (varAddress.Length < 2)
        {
            return -1;
        }
        ;
        var count = varAddress.Substring(1, varAddress.Length - 1);
        if (isNumeric(count))
        {
            return Convert.ToInt32(count, fromBase);
        }
        else
        {
            return -1;
        }
    }

    #endregion
    #region 写数据
    private void WriteWordArea(string startChar, int startAddress, string varAddress, ushort[] data)
    {
        if (varAddress.Length == 0) return;
        if (!varAddress.StartsWith(startChar)) return;
        int addressIndex = Convert.ToInt16(varAddress.Substring(1));
        if (addressIndex == -1) return;
        modbusIpMaster.WriteMultipleRegisters(Convert.ToUInt16(startAddress + addressIndex), data);
    }
    #region 写Y
    public void WriteY(string varAddress, bool[] data)
    {
        if (!isConnected) return;
        if (varAddress.StartsWith("Y")) WriteYArea(varAddress, data);
        if (isNumeric(varAddress))
        {
            WriteYArea(Convert.ToUInt16(varAddress), data);
        }



    }
    private void WriteYArea(string varAddress, bool[] data)
    {
        WriteBitArea("Y", 64512, varAddress, data, 8);
    }
    private void WriteYArea(int varAddress, bool[] data)
    {

        if (varAddress >= 64512 && varAddress <= 64767)
        {
            modbusIpMaster.WriteMultipleCoils(Convert.ToUInt16(varAddress), data);

        }

    }
    #endregion
    #region 写M
    public void WriteM(string varAddress, bool[] data)
    {
        if (!isConnected) return;
        if (varAddress.StartsWith("M")) WriteMArea(varAddress, data);
        if (isNumeric(varAddress))
        {
            WriteMArea(Convert.ToUInt16(varAddress), data);
        }



    }
    private void WriteMArea(string varAddress, bool[] data)
    {
        WriteBitArea("M", 0, varAddress, data);
    }
    private void WriteMArea(int varAddress, bool[] data)
    {

        if (varAddress >= 0 && varAddress <= 7679)
        {
            modbusIpMaster.WriteMultipleCoils(Convert.ToUInt16(varAddress), data);

        }

    }
    #endregion
    #region 写D
    public void WriteD(string varAddress, ushort[] data)
    {
        if (!isConnected) return;
        if (varAddress.StartsWith("D")) WriteDArea(varAddress, data);
        if (isNumeric(varAddress))
        {
            WriteDArea(Convert.ToUInt16(varAddress), data);
        }



    }
    private void WriteDArea(string varAddress, ushort[] data)
    {
        WriteWordArea("D", 0, varAddress, data);
    }
    private void WriteDArea(int varAddress, ushort[] data)
    {

        if (varAddress >= 0 && varAddress <= 8511)
        {
            modbusIpMaster.WriteMultipleRegisters(Convert.ToUInt16(varAddress), data);

        }

    }
    #endregion
    #endregion
}
