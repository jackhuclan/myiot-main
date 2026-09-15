// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Ports;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VgEAPClient.Common.SerialModbus;

public class CSerialModbus
{
    public event Action<Exception>? OnOpenFailed;

    public string PortNum { get; set; } = "COM4";
    public string ModbusAddr { get; set; } = "2";
    private readonly string _BaudRate = "9600";
    private readonly string _ReadAiNum = "16";
    private SerialPort comm = new SerialPort();
    private int errrcvcnt = 0;
    private readonly ILogger<CSerialModbus> _logger;

    public List<int> GetSerialPortData(int PortCount)
    {
        try
        {
            byte[] info = CModbusDll.ReadAIInfo(Convert.ToInt16(ModbusAddr), 0, Convert.ToInt16(_ReadAiNum));
            byte[] rst = sendinfo(info);

            return analysisSerialPortData(rst, PortCount);
        }
        catch (Exception ex)
        {
            _logger.LogError("GetSerialPortData Exception - " + ex.Message);
        }

        return new List<int>();
    }

    public CSerialModbus(ILogger<CSerialModbus> logger)
    {
        _logger = logger;
    }

    private byte[] sendinfo(byte[] info)
    {
        try
        {
            if (comm == null)
            {
                comm = new SerialPort();
                return null;
            }

            if (comm.IsOpen == false)
            {
                OpenSerialPort();
                //return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("sendinfo-1 Exception - " + ex.Message);
        }
        try
        {
            byte[] data = new byte[2048];
            int len = 0;

            comm.Write(info, 0, info.Length);
            _logger.LogInformation(FormatModbusRecvInfo("发送", info, info.Length));

            try
            {
                Thread.Sleep(50);
                Stream ns = comm.BaseStream;
                ns.ReadTimeout = 50;
                len = ns.Read(data, 0, 2048);

                _logger.LogInformation(FormatModbusRecvInfo("接收", data, len));
            }
            catch (Exception ex)
            {
                _logger.LogError("sendinfo-Read Exception - " + ex.Message);
            }
            errrcvcnt = 0;
            return analysisRcv(data, len);
        }
        catch (Exception ex)
        {
            _logger.LogError("sendinfo-Write Exception - " + ex.Message);
        }
        return null;
    }

    /// <summary>
    /// 打开串口
    /// </summary>
    public bool OpenSerialPort()
    {
        //关闭时点击，则设置好端口，波特率后打开
        try
        {
            comm.PortName = PortNum; //串口名 COM1
            comm.BaudRate = int.Parse(_BaudRate); //波特率  9600
            comm.DataBits = 8; // 数据位 8
            comm.ReadBufferSize = 4096;
            comm.StopBits = StopBits.One;
            comm.Parity = Parity.None;
            comm.Open();
        }
        catch (Exception ex)
        {
            //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
            comm = new SerialPort();
            //现实异常信息给客户。
            //MessageBox.Show(ex.Message);
            _logger.LogError("OpenSerialPort Exception - " + ex.Message);
            OnOpenFailed?.Invoke(ex);
            return false;
        }
        return true;
    }

    private byte[] analysisRcv(byte[] src, int len)
    {
        try
        {
            if (len < 6) return null;
            if (src[0] != Convert.ToInt16(ModbusAddr)) return null;

            switch (src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;

                case 0x02:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;

                case 0x04:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;

                case 0x05:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[4];
                        return dst;
                    }
                    break;

                case 0x0f:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = 1;
                        return dst;
                    }
                    break;

                case 0x06:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;

                case 0x10:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("analysisRcv Exception - " + ex.Message);
        }
        return null;
    }

    public List<int> analysisSerialPortData(byte[] srcValue, int num)
    {
        List<int> listSerialPortData = new List<int>();
        try
        {
            if (srcValue == null)
            {
            }
            for (int i = 0; i < num; i++)
            {
                byte[] byteValue = { 0, 0, 0, 0 };
                byteValue[2] = srcValue[i];
                byteValue[3] = srcValue[++i];
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(byteValue);
                }

                int nValue = BitConverter.ToInt32(byteValue, 0);
                listSerialPortData.Add(nValue);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("analysisSerialPortData Exception - " + ex.Message);
        }
        return listSerialPortData;
    }

    public string FormatModbusRecvInfo(string infotxt, byte[] info, int len = 0)
    {
        string debuginfo = string.Empty;
        try
        {
            StringBuilder builder = new StringBuilder();
            if (info != null)
            {
                if (len == 0) len = info.Length;
                //判断是否是显示为16禁止
                //依次的拼接出16进制字符串
                for (int i = 0; i < len; i++)
                {
                    builder.Append(info[i].ToString("X2") + " ");
                }
            }
            debuginfo = string.Format("{0}:{1}\r\n", infotxt, builder.ToString());
            builder.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError("ModbusRecvInfo Exception - " + ex.Message);
        }
        return debuginfo;
    }
}
