namespace VgDeviceGateway.Devices.Drill.Other.Atp.Entity
{
    [Serializable]
    public class Arrange
    {
        public int TrayIndex { get; set; }
        public int Site { get; set; }
        public double PgmDiameter { get; set; }
        public int MoCount { get; set; }
        public int Life { get; set; }
    }

    [Serializable]
    public class LifeDefine
    {
        public double PgmDiameter { get; set; }
        public int MoCount { get; set; }
        public int Life { get; set; }
    }

    [Serializable]
    public class Data
    {
        public List<LifeDefine> LifeDefines { get; set; }
        public List<Arrange> Arranges { get; set; }
    }

    [Serializable]
    public class RootObject
    {
        public string Code { get; set; }
        public string Info { get; set; } //生成文件名
        public Data Data { get; set; }
    }
}
