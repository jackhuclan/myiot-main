using Microsoft.Management.Infrastructure;
using System;
namespace VgDeviceGateway.Devices.Drill.Other
{
    public class ProcessManager
    {
        public static CimSession CreateCimSession(string computerName = "localhost")
        {
            return CimSession.Create(computerName);
        }

        public static bool TerminateProcess(string processName, string computerName = "localhost")
        {
            try
            {
                using (CimSession session = CreateCimSession(computerName))
                {
                    string namespaceName = @"root\cimv2";
                    string query = $"SELECT * FROM Win32_Process WHERE Name='{processName}'";
                    IEnumerable<CimInstance> queryResults = session.QueryInstances(namespaceName, "WQL", query);

                    foreach (CimInstance ci in queryResults)
                    {
                        CimMethodResult result = session.InvokeMethod(namespaceName, ci, "Terminate", null);
                        if (result.ReturnValue !=null)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}

