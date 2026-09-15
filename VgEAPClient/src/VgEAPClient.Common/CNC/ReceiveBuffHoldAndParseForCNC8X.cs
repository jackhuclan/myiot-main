// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace VgEAPClient.Common
{
    /* 需求分析
    假设一个完整数据包格式形如‘<HEAD>XXXXXXXX<TAIL>’，则需要处理的各种情况示意如下： 

    //第一大类 ：BUFF内的起点处就是一个包的开始；
    //<HEAD>XXXX
    //<HEAD>XXXXXXXX<TAIL>
    //<HEAD>XXXXXXXX<TAIL><HE
    //<HEAD>XXXXXXXX<TAIL><HEAD>XXXX
    //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>
    //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HE
    //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXX
    //<HE                           

    //第二大类 ：BUFF内的起点处有一个包的(后半段)残留数据;
    //IL><HEAD>XXXXXXXX<TAIL>
    //<TAIL><HEAD>XXXXXXXX<TAIL>    
    //XXXX<TAIL><HEAD>XXXXXXXX<TAIL>
    //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXX
    //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>
    //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HE
    */

    /* 正常使用步骤    
    用到的方法：
        PushData
        TryPopOnePacket

    例子代码：
            int validRecvCount = _buffHold.PushData(buffer, 0, buffer.Length, _recvBytesLogFilePath);
            while (true)
            {
                string recvMsg = _buffHold.TryPopOnePacket();
                if (string.IsNullOrEmpty(recvMsg))
                {
                    break;
                }
                else
                {
                    //TO DO : 在这里处理 recvMsg
                }
            }
    */

    /// <summary>
    /// V1.0.1 : 用于处理从 CNC8X 收到的字节流。能处理 不完整包(前半截)、粘包（BUFF里有多个包的数据）、残包(只有后半截)的情况。
    /// 注意， 如果你要用 Encoding.GetEncoding() 方法取得指定编码页， 则需要在APP入口调用 Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)；
    /// 2025-05-24 NH 创建
    /// 2025-06-24 NH 最后修改 
    /// </summary>
    public class ReceiveBuffHoldAndParseForCNC8X
    {
        public const string LogHead = "Buff hold parse : ";

        public delegate void OutputDebug(string text);

        const char leftBra = '<';
        const char rightBra = '>';

        //init 
        string _myId = "";
        Encoding _encoding = Encoding.Default;
        OutputDebug _outputLogImp = OutputDebugDummy;
        long _buffMaxCapacity = 512 * 1000;

        //run var
        object _recvBuffLock = new object();
        List<byte> _recvBuff = new List<byte>();//暂存收到的数据

        long _recvPacketCount = 0;

        DateTime _lastRecvPrintTime = DateTime.Now.AddSeconds(-30);
        long _lastRecvPacketCount = 0;

        const string packetHeadText = "<SMDNCPACKET";
        byte[] _packetHeadBytes = null;

        const string packetTailText__ = "</SMDNCPACKET>";
        byte[] _packetTailBytes__ = null;

        string _lastPopedMsg = "";
        long _totalByteCountWrittenToRecvBinLog = 0;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="encoding">字符集编码</param>
        /// <param name="max_buff_capacity">BUFF最大容量(字节数)</param>
        /// <param name="holder_id">本实例的ID,打印日志时便于区分</param>
        /// <param name="output_debug">输出日志的接口，允许为空</param>
        public ReceiveBuffHoldAndParseForCNC8X(Encoding encoding, long max_buff_capacity = 512 * 1000,
            string holder_id = "", OutputDebug? output_debug = null)
        {
            _encoding = (null != encoding) ? encoding : Encoding.Default;
            _buffMaxCapacity = max_buff_capacity;
            _myId = holder_id;
            _outputLogImp = (null != output_debug) ? output_debug : OutputDebugDummy;
        }

        static void OutputDebugDummy(string text)
        {
        }

        /// <summary>
        /// 清理BUFF中剩余数据
        /// </summary>
        public void Clear()
        {
            lock (_recvBuffLock)
            {
                _recvBuff.Clear();
            }
        }

        /// <summary>
        /// 取得BUFF中剩余数据的COPY
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffDataCopy()
        {
            lock (_recvBuffLock)
            {
                return _recvBuff.ToArray();
            }
        }

        /// <summary>
        /// 把收到的字节流（不一定是完整的包）放入
        /// </summary>
        /// <param name="srcBuf"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns>返回实际接收的有效数据的长度</returns>
        public int PushData(byte[] srcBuf, int startIndex, int count, string binLogFilePath = "")
        {
            int actualRecvCount = 0;
            lock (_recvBuffLock)
            {
                for (int kk = 0; kk <= count - 1; kk++)
                {
                    var the_char = srcBuf[startIndex + kk];
                    if (the_char == '\0' && startIndex + kk + 2 < srcBuf.Length
                        && srcBuf[startIndex + kk + 1] == '\0' && srcBuf[startIndex + kk + 2] == '\0')
                    {//遇到连续的三个0，说明这里已经是BUFF内的无效数据区域
                        break;
                    }
                    else
                    {
                        _recvBuff.Add(the_char);//以后可 块列表 代替list以提升效率
                        actualRecvCount += 1;
                    }
                }//for

                if (_totalByteCountWrittenToRecvBinLog < 1024 * 256 && actualRecvCount > 0 && false == string.IsNullOrEmpty(binLogFilePath))
                {
                    _totalByteCountWrittenToRecvBinLog += actualRecvCount;
                    AppendBytesToFile(binLogFilePath, srcBuf, startIndex, actualRecvCount);
                }
            }//lock
            return actualRecvCount;
        }

        /// <summary>
        /// 尝试取出一个完整的包；如果没有，返回空字符串;
        /// </summary>
        /// <returns></returns>
        public string TryPopOnePacket()
        {
            #region PlanB
            if (null == _packetHeadBytes || null == _packetTailBytes__)
            {
                _packetHeadBytes = _encoding.GetBytes(packetHeadText);
                _packetTailBytes__ = _encoding.GetBytes(packetTailText__);
            }

            string recvMsg = "";
            lock (_recvBuffLock)
            {
                try
                {
                    int found_head_idx = -1;
                    int found_tail___idx = -1;

                    if (_recvBuff.Count > packetHeadText.Length + _packetTailBytes__.Length)
                    {
                        //找包头.
                        for (int tt = 0; tt < _recvBuff.Count; tt++)
                        {
                            if ((char)_recvBuff[tt] == leftBra)
                            {

                                bool fit = true;
                                var compare_seg = _packetHeadBytes;
                                for (int k = 0; k < compare_seg.Length; k++)
                                {
                                    byte m = compare_seg[k];
                                    byte b = _recvBuff[tt + k];
                                    if (m != b)
                                    {
                                        fit = false;
                                    }
                                }

                                if (fit)
                                {
                                    found_head_idx = tt;
                                    break;
                                }
                            }
                        }//for

                        for (int tt = found_head_idx + 1; tt < _recvBuff.Count; tt++)
                        {//找包尾巴
                            byte recvb = _recvBuff[tt];
                            if ((char)recvb == rightBra)
                            {
                                int fit_byte_count = 0;
                                var compare_seg = _packetTailBytes__;
                                for (int k = 0; k < compare_seg.Length; k++)
                                {
                                    byte m = compare_seg[compare_seg.Length - 1 - k];
                                    if (tt - k >= 0)
                                    {
                                        byte b = _recvBuff[tt - k];
                                        if (m == b)
                                            fit_byte_count += 1;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (fit_byte_count == compare_seg.Length)
                                {
                                    found_tail___idx = tt;
                                    break;
                                }
                            }
                        }//for
                    }

                    if (found_tail___idx < 0)
                    {
                        return "";
                    }

                    if (found_tail___idx >= 0)
                    {
                        if (found_head_idx < 0)
                        {//BUFF开头的数据是残缺的'后半截'！要删掉
                            string bad_part_str = "";
                            int bad_len = found_tail___idx + 1;
                            try
                            {
                                var bad_buff = new byte[bad_len];
                                _recvBuff.CopyTo(0, bad_buff, 0, bad_len);

                                bad_part_str = _encoding.GetString(bad_buff);
                            }
                            catch (Exception exp)
                            {
                                _outputLogImp(LogHead + $"Exp5000487 " + exp.Message);
                                _outputLogImp(LogHead + $"Exp5000487 " + exp.StackTrace);
                            }

                            try
                            {
                                _recvBuff.RemoveRange(0, bad_len);

                                _outputLogImp(LogHead + $"Remove bad packet : bad byte x {bad_len} ('{bad_part_str}'). Remain part length {_recvBuff.Count}.");
                            }
                            catch (Exception exp)
                            {
                                _outputLogImp(LogHead + $"Exp5000489 " + exp.Message);
                                _outputLogImp(LogHead + $"Exp5000489 " + exp.StackTrace);
                            }
                        }
                        else if (found_tail___idx >= 0)
                        {//提取出一个完整的包
                            int copy_len = found_tail___idx + 1 - found_head_idx;
                            byte[] src = _recvBuff.ToArray();
                            byte[] pop_data = new byte[copy_len];

                            Array.Copy(src, found_head_idx, pop_data, 0, copy_len);

                            //注意， 包头 前面可能还有残缺数据//这里从0开始删除
                            _recvBuff.RemoveRange(0, found_tail___idx + 1);

                            try
                            {
                                recvMsg = _encoding.GetString(pop_data);
                                _recvPacketCount += 1;
                            }
                            catch (Exception ex)
                            {
                                string hexstr = ByteArrayToHexString(pop_data);
                                _outputLogImp(LogHead + $"Exp3000350 get string from buff failed . buf '{hexstr}'. ");
                            }
                        }
                    }//if (found_tail___idx >= 0)

                    if ((DateTime.Now - _lastRecvPrintTime).TotalSeconds > 60 * 3)
                    {
                        double elapse_min = (DateTime.Now - _lastRecvPrintTime).TotalMinutes;
                        double period_recv_count = (_recvPacketCount - _lastRecvPacketCount);
                        double speed = elapse_min > 0.1 ? (period_recv_count / elapse_min) : 0;

                        _lastRecvPrintTime = DateTime.Now;
                        _lastRecvPacketCount = _recvPacketCount;

                        _outputLogImp(LogHead + $"Recv period print : cur buff len {_recvBuff.Count}. elaps min {elapse_min}, period recv x {period_recv_count}. speed {speed} per min. me '{_myId}'.");
                    }
                    if (_recvBuff.Count > _buffMaxCapacity)
                    {
                        _outputLogImp(LogHead + $"Exp5320001 Clear recv bytes cache for cnc84 ! Cache len '{_recvBuff.Count}'.me '{_myId}'.");
                        _recvBuff.Clear();
                    }
                }
                catch (Exception ex)
                {
                    _outputLogImp(LogHead + $"Exp5111296" + ex.Message);
                    _outputLogImp(LogHead + $"Exp5111296" + ex.StackTrace);
                }
            }//lock
            #endregion

            if (false == string.IsNullOrEmpty(recvMsg))
            {
                _lastPopedMsg = recvMsg;
            }

            return recvMsg;
        }

        void AppendBytesToFile(string filePath, byte[] content, int startIndex, int count)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _outputLogImp(LogHead + $"File path is empty.");
                return;
            }

            if (content == null || content.Length == 0)
            {
                _outputLogImp(LogHead + $"Content is invalid.");
                return;
            }

            try
            {
                using FileStream stream = new FileStream(
                    filePath,
                    FileMode.Append,    // 自动创建不存在的文件并追加内容
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    useAsync: false);

                stream.Write(content, startIndex, content.Length);
            }
            catch (Exception ex)
            {
                _outputLogImp(LogHead + $"Append bytes to file : ex " + ex.Message);
            }
        }

        public static string ByteArrayToHexString(byte[] bytes, bool insert_space = true)
        {
            var s = BitConverter.ToString(bytes);
            if (insert_space)
                return s.Replace("-", " ");
            else
                return s.Replace("-", string.Empty);
        }

        #region ForTest

        public class UnitTestItem
        {
            public string InputString = "";

            public string[] PopPacketList = null;
            public string BuffAfterPop = "";
        }

        /// <summary>
        /// 返回失败案例的数量；如果一切顺利，应该返回0；
        /// </summary>
        /// <returns></returns>
        public static int BatchTest()
        {
            int fail_count = 0;
            var item_list = new List<UnitTestItem>();

            #region SetTestInput

            //<HEAD>XXXX
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VAL",
                PopPacketList = new string[]
                {
                },
                BuffAfterPop = @"<SMDNCPACKET VAL",
            });

            //<HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = @"",
            });

            //<HEAD>XXXXXXXX<TAIL><HE
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                                + "\0" + @"<SMDNCP",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                },
                BuffAfterPop = "\0" + @"<SMDNCP",
            });

            //<HEAD>XXXXXXXX<TAIL><HEAD>XXXX
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                                + "\0" + @"<SMDNCPACKET V",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                },
                BuffAfterPop = "\0" + @"<SMDNCPACKET V",
            });

            //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                    + "\0" + @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""66666""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""66666""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });

            //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HE
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                    + "\0" + @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""77711""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                    + "\0" + @"<SMDN",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""77711""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "\0" + @"<SMDN",
            });

            //<HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXX
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                    + "\0" + @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                    + "\0" + @"<SMDNCPACKET VALUE=",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""45""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "\0" + @"<SMDNCPACKET VALUE=",
            });

            //<HE
            item_list.Add(new UnitTestItem()
            {
                InputString = @"<SMDNCP",
                PopPacketList = new string[]
                {
                },
                BuffAfterPop = @"<SMDNCP",
            });
            //IL><HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = "CKET>" + "\0" + @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });
            //<TAIL><HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = @"</SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""31""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });
            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });
            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXX
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VAL",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "\0" + @"<SMDNCPACKET VAL",
            });
            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });

            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>0
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                        + "\0",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "\0",
            });

            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>000
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                        + "\0\0\0",//末尾加了多个0，应该被忽略
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });

            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL>0000000000
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>" + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                        + "\0\0\0\0\0\0\0\0\0\0",//末尾加了多个0，应该被忽略
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "",
            });

            //XXXX<TAIL><HEAD>XXXXXXXX<TAIL><HEAD>XXXXXXXX<TAIL><HE
            item_list.Add(new UnitTestItem()
            {
                InputString = @"UTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""7777""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>"
                         + "\0" + @"SMDNCPA",
                PopPacketList = new string[]
                {
                    @"<SMDNCPACKET VALUE=""551""><CNC VALUE=""1""><EXECUTE VALUE=""55555""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                    @"<SMDNCPACKET VALUE=""566""><CNC VALUE=""1""><EXECUTE VALUE=""7777""><RUNTIMEVALUE VALUE=""1"">SCREENSAVER</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>",
                },
                BuffAfterPop = "\0" + @"SMDNCPA",
            });

            #endregion

            var the_encoding = Encoding.Default;
            for (int n = 0; n < item_list.Count; n++)
            {
                var item = item_list[n];

                try
                {
                    byte[] bytes = the_encoding.GetBytes(item.InputString);
                    var hold = new ReceiveBuffHoldAndParseForCNC8X(the_encoding, 512000, $"Holder{n}", OutputDebugDummy);

                    //模拟从 源头BUF 的不同位置 取数据
                    int start_idx = n;
                    int input_data_len = bytes.Length;
                    byte[] recv_buff = new byte[input_data_len + start_idx];
                    Array.Copy(bytes, 0, recv_buff, start_idx, input_data_len);

                    hold.PushData(recv_buff, start_idx, input_data_len);

                    var popped_packets = new List<string>();
                    for (int k = 0; k < 999; k++)
                    {
                        string out_msg = hold.TryPopOnePacket();
                        if (string.IsNullOrEmpty(out_msg))
                        {
                            break;
                        }
                        else
                        {
                            popped_packets.Add(out_msg);

                        }
                    }//for

                    bool fail = false;

                    var buff_after_pop = hold.GetBuffDataCopy();
                    var buff_after_pop_str = the_encoding.GetString(buff_after_pop);
                    if (buff_after_pop_str != item.BuffAfterPop)
                    {
                        fail = true;
                    }

                    if (popped_packets.Count != item.PopPacketList.Length)
                    {
                        fail = true;
                    }
                    else
                    {
                        for (int m = 0; m < item.PopPacketList.Length; m++)
                        {
                            string expected_msg = item.PopPacketList[m];
                            string res_msg = popped_packets[m];
                            if (expected_msg != res_msg)
                            {
                                fail = true;
                            }
                        }//for
                    }

                    if (fail)
                        fail_count += 1;
                }
                catch (Exception exp)
                {
                    fail_count += 1;
                }
            }//for
            return fail_count;
        }

        #endregion
    }//class
}
