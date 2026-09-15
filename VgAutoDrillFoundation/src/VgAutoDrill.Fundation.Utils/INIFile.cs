using System.Text;

namespace VgAutoDrill.Fundation.Utils;

public class INIFile
{
    private static string section = "AGV";
    private static string path = "D:\\AGV.ini";

    // 声明INI文件的写操作函数 WritePrivateProfileString()
    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    // 声明INI文件的读操作函数 GetPrivateProfileString()
    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


    /// 写入INI的方法
    public static void INIWrite(string key, string value)
    {
        // section=配置节点名称，key=键名，value=返回键值，path=路径
        WritePrivateProfileString(section, key, value, path);
    }

    //读取INI的方法
    public static string INIRead(string key)
    {
        // 每次从ini中读取多少字节
        StringBuilder temp = new StringBuilder(255);

        // section=配置节点名称，key=键名，temp=上面，path=路径
        GetPrivateProfileString(section, key, "", temp, 255, path);
        return temp.ToString();
    }

    //删除一个INI文件
    public void INIDelete(string FilePath)
    {
        File.Delete(FilePath);
    }
}
