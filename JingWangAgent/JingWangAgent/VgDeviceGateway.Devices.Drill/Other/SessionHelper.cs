// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace VgDeviceGateway.Devices.Drill.Other
{
    public class SessionHelper
    {
        [DllImport("Wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSEnumerateSessions(
        IntPtr hServer,
        int Reserved,
        int Version,
        out IntPtr ppSessionInfo,
        out int pCount);

        [DllImport("Wtsapi32.dll", SetLastError = true)]
        private static extern void WTSFreeMemory(IntPtr pMemory);

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_SESSION_INFO
        {
            public int SessionId;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        public static List<WTS_SESSION_INFO> EnumerateSessions()
        {
            IntPtr ppSessionInfo;
            int pCount;
            bool success = WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1, out ppSessionInfo, out pCount);

            if (!success)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(errorCode);
            }
            List< WTS_SESSION_INFO > mm=new List< WTS_SESSION_INFO >();
            try
            {
                int size = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                for (int i = 0; i < pCount; i++)
                {
                    IntPtr current = IntPtr.Add(ppSessionInfo, i * size);
                    WTS_SESSION_INFO sessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure(current, typeof(WTS_SESSION_INFO));
                    mm.Add( sessionInfo );
                    Console.WriteLine($"Session ID: {sessionInfo.SessionId}, WinStationName: {sessionInfo.pWinStationName}, State: {sessionInfo.State}");
                }
                return mm;
            }
            finally
            {
               
                WTSFreeMemory(ppSessionInfo);
            }
        }

        private static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
    }
}
