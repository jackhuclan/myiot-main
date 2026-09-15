// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using IWshRuntimeLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using File = System.IO.File;

namespace VgEAPClient;

public class SetuoWindowOpenRun
{
    public static string WOW6432NodeOpenRunPath = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";//计算机\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run
    public static string CreateSubKeyWOW6432NodeRunPath = "SOFTWARE//WOW6432Node//Microsoft//Windows//CurrentVersion//Run";
    public static string OpenRunPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";//【没有数据】计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
    public static string CreateSubKeyRunPath = "SOFTWARE//Microsoft//Windows//CurrentVersion//Run";
    private readonly ILogger<SetuoWindowOpenRun> _logger;

    public SetuoWindowOpenRun(ILogger<SetuoWindowOpenRun> logger)
    {
        _logger = logger;
    }

    public bool SetupStartupLnk(
      string setupPath,
      string linkname,
      string description,
      bool showMesBox,
      bool inProgramdataFolder = false,
      bool Is32 = true,
      int version = 10)
    {
        bool flag = false;
        try
        {
            string directoryName = Path.GetDirectoryName(setupPath)!;
            setupPath.Substring(setupPath.LastIndexOf("\\") + 1);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string path = "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";
            string str1 = !inProgramdataFolder || !Directory.Exists(path) ? folderPath + "\\" + linkname + ".lnk" : path + "\\" + linkname + ".lnk";
            string str2 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + linkname + ".lnk";
            if (File.Exists(str1))
                File.Delete(str1);
            if (File.Exists(str2))
                File.Delete(str2);
            string str3 = directoryName + "\\autostart.ps1";
            flag = !File.Exists(str3) ? WshShell(str1, setupPath, description, directoryName, showMesBox) : WshShell(str1, str3, description, directoryName, showMesBox);
            flag = WshShell(str2, setupPath, description, directoryName, showMesBox);
            SetAutoStart(true, linkname, Is32);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "设置开机启动失败!" + ex.Message);
        }

        return flag;
    }

    private bool WshShell(
      string startup,
      string setupPath,
      string description,
      string directoryName,
      bool showMesBox = false)
    {
        bool flag = false;
        try
        {
            IWshShortcut shortcut = (IWshShortcut)new WshShellClass().CreateShortcut(startup);
            shortcut.TargetPath = setupPath;
            shortcut.Arguments = "";
            shortcut.Description = description;
            shortcut.WorkingDirectory = directoryName;
            shortcut.IconLocation = directoryName + "\\App.ico";
            shortcut.WindowStyle = 1;
            shortcut.Save();
            flag = true;
            if (showMesBox)
            {
                int num = (int)MessageBox.Show("设置开机启动成功!", "提示");
            }
        }
        catch (Exception ex)
        {
            flag = false;
            _logger.LogError(ex, "设置开机启动失败!" + ex.Message);
        }
        finally
        {
        }
        return flag;
    }

    private bool SetAutoStart(bool onOff, string appName, bool Is32)
    {
        bool flag = true;
        string executablePath = Application.ExecutablePath;
        if (!IsExistKey(appName, Is32) & onOff)
            flag = SelfRunning(onOff, appName, executablePath, Is32);
        else if (IsExistKey(appName, Is32) && !onOff)
            flag = SelfRunning(onOff, appName, executablePath, Is32);
        return flag;
    }

    private bool IsExistKey(string keyName, bool Is32)
    {
        try
        {
            bool flag = false;
            string name = WOW6432NodeOpenRunPath;
            string subkey = CreateSubKeyWOW6432NodeRunPath;
            if (!Is32)
            {
                subkey = CreateSubKeyRunPath;
                name = OpenRunPath;
            }
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey(name, true) ?? localMachine.CreateSubKey(subkey);
            foreach (string valueName in registryKey.GetValueNames())
            {
                if (valueName.ToUpper() == keyName.ToUpper())
                    return true;
            }
            registryKey.Close();
            localMachine.Close();
            return flag;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    private bool SelfRunning(bool isStart, string exeName, string path, bool Is32)
    {
        try
        {
            string name = WOW6432NodeOpenRunPath;
            string str = CreateSubKeyWOW6432NodeRunPath;
            if (!Is32)
            {
                str = CreateSubKeyRunPath;
                name = OpenRunPath;
            }
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey(name, true)!;
            if (registryKey == null)
            {
                registryKey = localMachine.CreateSubKey(CreateSubKeyWOW6432NodeRunPath);
            }
            if (isStart)
            {
                registryKey.SetValue(exeName, path);
                registryKey.Close();
            }
            else
            {
                foreach (string valueName in registryKey.GetValueNames())
                {
                    if (valueName.ToUpper() == exeName.ToUpper())
                    {
                        registryKey.DeleteValue(exeName);
                        registryKey.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }

        return true;
    }
}
