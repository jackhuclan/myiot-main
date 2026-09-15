// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Ports;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VgEAPClient.Common;

/*
装机的默认配置 ： 站号 3 ， 端口 COM4 ， 波特率 9600 ， 奇偶校验 无 ;

返回消息的CRC16校验码 是小尾序的.
收到的数值如果是单精度浮点数（4个字节），需要把顺序反转一下 才能使用  BitConverter.ToSingle ！

例子 ： 数值为 78.2
发送  03 03 10 1e 00 02 a1 2f  
收到  03 03 04 42 9c 66 66 a6 2f
备注 ： 最后两个字节是小端的  CRC16校验码；
*/

/// <summary>
/// 正泰的电表.CHNT是正泰的商标. ModelName值是型号.
/// </summary>
public class AmmeterCHNTDTSU666 : IAmmeterIO
{
    public const string ModelName = "CHNTDTSU666";
    const double ReadTimeoutSec = 4.0;

    //正向有功总电能
    const int KWHSumAddress = 0x101e;


    public class RtuInitParam
    {
        public bool AddressStartWithZero = false;

        public int ReadBuffSizeMax = 4096;
        public byte Station = 3;
        public string PortName = "";//EG : COM4
        public int BaudRate = 9600;
        public int DataBits = 8;

        public StopBits StopBits = StopBits.One;
        public Parity Parity = Parity.None;

        public bool Crc16CheckEnable = true;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    //init
    ILogger _Logger = null;
    RtuInitParam _RtuInitParam = new RtuInitParam();
    static SerialPort _serialPort = new SerialPort();

    //run
    DateTime _StartRunTime = DateTime.Now;
    DateTime _LastTickBeginTime = DateTime.Now;
    double _LastKWH = 0;//电表上累计的总用电量
    DateTime _LastKWH_ReadTime = DateTime.Now;

    object _LockForLogQueue = new object();
    Queue<string> _ImportantLogQue = new Queue<string>();

    volatile bool _LoopRunning = false;
    volatile bool _ReqStop = false;

    long _WaitTickCount = 0;

    object _RtuLock = new object();

    public AmmeterCHNTDTSU666(ILogger logger)
    {
        _Logger = logger;
    }

    public string GetAmmeterModel()
    {
        return ModelName;
    }

    public double GetConsumedKWH()
    {
        return _LastKWH;
    }

    public string PopImportantLog()
    {
        lock (_LockForLogQueue)
        {
            if (0 == _ImportantLogQue.Count)
                return "";
            else
            {
                var line = _ImportantLogQue.Dequeue();
                return line;
            }
        }
    }
    private void PushImportantLog(string line, bool log_debug = true)
    {
        lock (_LockForLogQueue)
        {
            if (log_debug) _Logger.LogDebug(line);

            _ImportantLogQue.Enqueue(line);

            if (_ImportantLogQue.Count > 512)
            {
                _ImportantLogQue.Dequeue();
            }
        }
    }


    public void StartRun(EAPClientOptions opions)
    {
        if (_LoopRunning)
        {
            _Logger.LogDebug($"Amme : {GetType()}. It is running now ! ");
            return;
        }

        _RtuInitParam.ReadBuffSizeMax = 16 * 1024;
        _RtuInitParam.PortName = opions.AmmeterPortName;
        _RtuInitParam.BaudRate = opions.AmmeterBaudRate;
        _RtuInitParam.DataBits = 8;
        _RtuInitParam.Parity = Parity.None;
        _RtuInitParam.StopBits = StopBits.One;


        _ReqStop = false;
        _LoopRunning = true;

        _Logger.LogDebug($"Amme : {GetType()}. start run ! Param : '{JsonConvert.SerializeObject(_RtuInitParam)}'");

        _StartRunTime = DateTime.Now;

        /*//03 03 00 0a 00 01    a5 ea
        string test_input_bytes_hex = "03 03 02 00 00";
        byte[] test_input = StringUtil.HexStringToByteArray(test_input_bytes_hex);
        byte[] test_input_crc = new byte[2] { 0, 0 };
        ushort crc = ModbusRtuHelper.CalculateCrc16(test_input, test_input.Length);
        test_input_crc[0] = (byte)(crc & 0xFF);       // 低位
        test_input_crc[1] = (byte)(crc >> 8);         // 高位

        string crc_hex = StringUtil.ByteArrayToHexString(test_input_crc);

        //{ 0x42, 0x9c, 0x66, 0x66 };
        byte[] single_bytes = new byte[4] { 0x66, 0x66, 0x9c, 0x42 };
        string single_bytes_hex = StringUtil.ByteArrayToHexString(single_bytes);
        float single = BitConverter.ToSingle(single_bytes, 0);

        _Logger.LogDebug($"Amme : test : CRC of {test_input_bytes_hex} : '{crc_hex}'. single_bytes : '{single_bytes_hex}' >> '{single:F2}'.");
        */
        PushImportantLog($"电表采集线程启动 ： 型号 '{GetAmmeterModel()}'.");

        TryOpenRtuConn();

        Task.Run(ThreadMain);
    }

    #region ForConn


    public int TryOpenRtuConn()
    {
        lock (_RtuLock)
        {

            try
            {
                if (false == _serialPort.IsOpen)
                {
                    _serialPort.PortName = _RtuInitParam.PortName;
                    _serialPort.BaudRate = _RtuInitParam.BaudRate;        // 设置波特率

                    _serialPort.DataBits = _RtuInitParam.DataBits;
                    _serialPort.StopBits = _RtuInitParam.StopBits;

                    _serialPort.Parity = _RtuInitParam.Parity;

                    _serialPort.ReadTimeout = 4000;
                    _serialPort.WriteTimeout = 4000;


                    _Logger.LogDebug($"Amme : Pre open rtu conn : p '{_serialPort.PortName}', br {_serialPort.BaudRate},"
                        + $" db {_serialPort.DataBits}, sb {_serialPort.StopBits}, parity {_serialPort.Parity}.");

                    _serialPort.Open();

                    string text_after = $"Amme : After open rtu conn : opened. {_RtuInitParam.PortName} .";
                    PushImportantLog(text_after);
                }
            }
            catch (Exception ex)
            {
                string err_msg = $"Amme : 电表采集 : 无法打开串口: {_RtuInitParam.PortName}. " + ex.Message;
                PushImportantLog(err_msg);

                _WaitTickCount = 2;

                return 404;
            }
        }//lock
        return 0;
    }

    #endregion


    public void StopRun(string reason)
    {
        PushImportantLog($"Amme : 电表采集 线程即将停止 ...");
        _ReqStop = true;

        while (_LoopRunning)
        {
            Thread.Sleep(1);
        }
    }


    double TickIntervalSec = 10.0;//TO DO 
    void ThreadMain()
    {
        while (_LoopRunning)
        {
            if (_ReqStop)
            {
                _LoopRunning = false;
                break;
            }

            if ((DateTime.Now - _LastTickBeginTime).TotalSeconds >= TickIntervalSec)
            {
                _LastTickBeginTime = DateTime.Now;

                try
                {
                    Tick();
                }
                catch (Exception ex)
                {
                    _Logger.LogError($"Exp8900236 ex " + ex.Message);
                }
            }

            Thread.Sleep(1);
        }

        _LoopRunning = false;

        PushImportantLog($"Amme : 电表采集 : 关闭串口连接");
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    double GetElapseSecSinceLastTick()
    {
        if (false == IsValidTime(_LastTickBeginTime))
            return 0;
        else
            return (DateTime.Now - _LastTickBeginTime).TotalSeconds;

    }
    /// <summary>
    /// 读一次当前的功率值，并返回距离上一次TICK消耗掉的电量（单位：KWH）
    /// </summary>
    /// <returns></returns>
    ///
    DateTime _LastSlowPrintTime = DateTime.Now.AddSeconds(-40);
    const double _SlowPrintIntervalSec = 60;

    volatile uint _TickCount = 0;
    void Tick()
    {
        bool slow_print = false;
        if ((DateTime.Now - _LastSlowPrintTime).TotalSeconds > _SlowPrintIntervalSec || _TickCount < 3)
        {
            _LastSlowPrintTime = DateTime.Now;

            slow_print = true;
        }

        _TickCount += 1;

        if (_WaitTickCount > 0)
        {
            _WaitTickCount -= 1;

            if (slow_print) _Logger.LogDebug($"Amme : wait tick count {_WaitTickCount}. Do nothing in this tick. ");

            return;
        }


        double cur_kwh = 0;
        double tick_consume = 0;
        //Plan B :
        {
            if (false == _serialPort.IsOpen)
            {
                int fc = TryOpenRtuConn();
                if (0 != fc)
                {
                    return;
                }

                int idx_in_dummy = 0;
                int pre_read_count = TryReadAck(_Logger, _serialPort, "ClearReadBuf", 0.3, dummy_buf, ref idx_in_dummy, 256, false);//先清空READ缓冲区
                if (pre_read_count > 0)
                {
                    string pre_read_hex = StringUtil.ByteArrayToHexString(dummy_buf, 0, pre_read_count);
                    _Logger.LogDebug($"Amme : For clear read buf. Pre read byte x {pre_read_count}. '{pre_read_hex}'.  ");
                }

                Thread.Sleep(1);
            }

            try
            {
                cur_kwh = ReadCurKWH(slow_print);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Exp8900299 ex " + ex.Message);
            }
        }//lock

        var now = DateTime.Now;
        double sec_delta = GetElapseSecSinceLastTick();

        if (cur_kwh >= _LastKWH)
        {
            tick_consume = (cur_kwh - _LastKWH);
        }
        else
        {
            _Logger.LogDebug($"Amme : tick print. Cur kwh {cur_kwh:F2} < last kwh {_LastKWH:F2}.");
        }

        double elapsed_h = (DateTime.Now - _LastKWH_ReadTime).TotalHours;
        double consume_kw = 0;
        if (elapsed_h > 0.00000001)
        {
            consume_kw = tick_consume / elapsed_h;
        }

        if (slow_print) { _Logger.LogDebug($"Amme : Tick print. Ack KWH {cur_kwh:F2}. Tick consume {tick_consume:F2}({consume_kw:F2}kw)."); }

        _LastKWH = cur_kwh;
        _LastKWH_ReadTime = DateTime.Now;
    }

    void DebugPrintImp(string msg)
    {
        _Logger.LogDebug(msg);
    }


    double ReadCurKWH(bool print)
    {
        var ack_word = ReadSingle(KWHSumAddress, 0, "Read CurKWH", print);
        return (double)ack_word;
    }

    byte[] dummy_buf = new byte[512];

    /*
    以数值 78.2 为例， 收到的字节流是  { 0x42, 0x9c, 0x66, 0x66 };
    需要把顺序反转一下 才能使用  BitConverter.ToSingle
    */
    float ReadSingle(ushort rtu_address, float default_value, string reason, bool print)
    {
        int expected_ack_len = 3 + 4 + 2;
        bool success = false;
        double cur_kwh = 0;
        try
        {
            lock (_RtuLock)
            {
                byte[] read_req = ModbusRtuHelper.CreateModbusRtuRequestForRead(_RtuInitParam.Station, ModbusRtuHelper.ReadCmd, _RtuInitParam.AddressStartWithZero,
                    rtu_address, 2, DebugPrintImp, $"Read cur kwh", false);

                if (print)
                {
                    string write_bytes_hex = StringUtil.ByteArrayToHexString(read_req);
                    _Logger.LogDebug($"Amme : '{reason}'. {rtu_address}. Write '{write_bytes_hex}'. ");
                }
                _serialPort.Write(read_req, 0, read_req.Length);

                Thread.Sleep(10);

                //站号 1 ， 功能码 1 ， 数据 ?， 校验码 2 
                byte[] ack_buf = new byte[expected_ack_len * 2]; Array.Fill<byte>(ack_buf, 0);

                int ack_buf_ptr = 0;
                int read_count = TryReadAck(_Logger, _serialPort, "ReadAck", ReadTimeoutSec, ack_buf, ref ack_buf_ptr, expected_ack_len, print);

                if (print)
                {
                    string read_hex = StringUtil.ByteArrayToHexString(ack_buf, 0, expected_ack_len);
                    _Logger.LogDebug($"Amme : '{reason}'. Read count {read_count} / {expected_ack_len}. Read bytes '{read_hex}'.");
                }

                if (read_count < expected_ack_len)
                {
                    return 0;
                }

                ushort ack_crc = ModbusRtuHelper.GetWordInBufSmallEndian(ack_buf, read_count - 2);
                ushort my_crc = ModbusRtuHelper.CalculateCrc16(ack_buf, read_count - 2);
                if (ack_crc != my_crc)
                {
                    string ack_hex = StringUtil.ByteArrayToHexString(ack_buf, 0, read_count);
                    if (print) _Logger.LogDebug($"Amme : '{reason}'. Crc16 校验失败 : ack '{ack_hex}'. my crc16 {StringUtil.WordToHexString(my_crc)}.");
                    return 0;
                }
                else
                {
                    //ushort ack_word = ModbusRtuHelper.GetWordInBufBigEndian(ack_buf, read_count - 4);
                    byte[] single_bytes = new byte[4] { 0, 0, 0, 0 };
                    Array.Copy(ack_buf, read_count - 2 - 4, single_bytes, 0, 4);
                    string single_bytes_hex = StringUtil.ByteArrayToHexString(single_bytes);
                    var single_bytes_r = single_bytes.Reverse().ToArray();
                    float ack_single = BitConverter.ToSingle(single_bytes_r, 0);

                    if (print) _Logger.LogDebug($"Amme : '{reason}'.Get ack done : result v {ack_single:F2}. " + $"Read count {read_count} / {expected_ack_len} : "
                        + StringUtil.ByteArrayToHexString(ack_buf) + $"  single float part '{single_bytes_hex}'.");

                    double min_since_start = (DateTime.Now - _StartRunTime).TotalMinutes;
                    if (min_since_start < 5 && print)
                    {
                        string line = $"Amme : 电表 当前累计消耗KWH {ack_single:F2}";
                        PushImportantLog(line);
                    }

                    success = true;
                    return ack_single;
                }
            }
        }
        catch (Exception ex)
        {
            _Logger.LogDebug($"Amme : Exp48900125 " + ex.Message);
        }
        return default_value;
    }


    const int SleepMilliseconds = 10;
    int TryReadAck(ILogger logger, SerialPort port, string reason, double time_out_sec, byte[] ack_buf, ref int cur_index, int expected_ack_len, bool print = false)
    {
        DateTime begin_t = DateTime.Now;

        int read_byte_sum = 0;

        Thread.Sleep(SleepMilliseconds);

        for (int n = 0; n < 999; n++)
        {
            var elapse_sec = (DateTime.Now - begin_t).TotalSeconds;
            if (elapse_sec >= time_out_sec)
            {
                if (print) logger.LogDebug($"Amme : Read ack time out. r '{reason}'.");
                break;
            }

            try
            {
                int read_count = _serialPort.Read(ack_buf, cur_index, expected_ack_len - read_byte_sum);

                read_byte_sum += read_count;
                cur_index += read_count;

                if (read_byte_sum >= expected_ack_len)
                {
                    break;
                }
            }
            catch (Exception exp)
            {
                if (print) logger.LogDebug($"Amme : Exp33894012 r '{reason}'. exp : " + exp.Message);
            }
            Thread.Sleep(SleepMilliseconds);
        }//for

        return read_byte_sum;
    }

    #region util
    public static bool IsValidTime(DateTime dt)
    {
        return dt > DateTime.MinValue && dt < DateTime.MaxValue;
    }
    #endregion
}


public class ModbusRtuHelper
{
    public const byte ReadCmd = 0x03;//04
    public const byte WriteCmd = 0x10;


    //小端
    public static ushort GetWordInBufSmallEndian(byte[] buf, int offset)
    {
        var low = (int)(buf[offset]);
        var hi = (int)(buf[offset + 1]) << 8;
        return (ushort)(low + hi);
    }

    public static ushort GetWordInBufBigEndian(byte[] buf, int offset)
    {
        var low = (int)(buf[offset + 1]);
        var hi = (int)(buf[offset]) << 8;
        return (ushort)(low + hi);
    }

    /// <summary>
    /// 生成 Modbus RTU 请求报文。
    /// </summary>
    /// <param name="slaveAddress">从站地址</param>
    /// <param name="functionCode">功能码</param>
    /// <param name="startAddress">起始地址</param>
    /// <param name="numOfPoints">寄存器数量</param>
    /// <returns>带有 CRC 校验的 Modbus RTU 请求报文</returns>
    public static byte[] CreateModbusRtuRequestForRead(byte slaveAddress, byte functionCode, bool addressBeginFrom0, ushort startAddress, ushort numOfPoints, Action<string> debug_print, string note = "", bool print = false)
    {
        byte[] frame = new byte[8];
        if (addressBeginFrom0)
        {
            if (startAddress > 0)
                startAddress = (ushort)(startAddress - 1);
            else
            { }
        }

        // 设置从站地址
        frame[0] = slaveAddress;

        // 设置功能码
        frame[1] = functionCode;

        // 起始地址（高位在前，低位在后）
        frame[2] = (byte)(startAddress >> 8);
        frame[3] = (byte)(startAddress & 0xFF);

        // 寄存器数量（高位在前，低位在后）
        frame[4] = (byte)(numOfPoints >> 8);
        frame[5] = (byte)(numOfPoints & 0xFF);

        // 计算并设置 CRC 校验
        ushort crc = CalculateCrc16(frame, 6);
        frame[6] = (byte)(crc & 0xFF);       // 低位
        frame[7] = (byte)(crc >> 8);         // 高位

        if (null != debug_print && print) debug_print($"{note} [{startAddress}] ({numOfPoints}) send : " + StringUtil.ByteArrayToHexString(frame));

        return frame;
    }



    /// <summary>
    /// 计算 Modbus RTU 报文的 CRC16 校验。
    /// </summary>
    /// <param name="data">用于计算 CRC 的数据字节数组</param>
    /// <param name="length">用于计算的字节长度</param>
    /// <returns>CRC 校验码</returns>
    public static ushort CalculateCrc16(byte[] data, int length)
    {
        uint crcValue = 0xFFFF;  // 初始化CRC值
        int i;

        for (int index = 0; index < length; index++)
        {
            crcValue ^= data[index];  // 每次与数据进行异或操作

            for (i = 0; i < 8; i++)  // 进行8次循环
            {
                if ((crcValue & 0x0001) != 0)  // 判断最低位是否为1
                {
                    crcValue = (crcValue >> 1) ^ 0xA001;  // 右移并异或0xA001多项式
                }
                else
                {
                    crcValue >>= 1;  // 仅右移
                }
            }
        }

        return (ushort)crcValue;  // 返回计算结果
    }
}//class

