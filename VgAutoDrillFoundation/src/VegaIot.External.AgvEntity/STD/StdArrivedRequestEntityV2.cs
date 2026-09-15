namespace VegaIot.External.AgvEntity.STD;

public class StdArrivedRequestEntityV2
{
    public string reqCode { get; set; }

    public string reqTime { get; set; }

    public decimal? cooX { get; set; }

    public decimal? cooY { get; set; }

    public string currentPositionCode { get; set; }

    public object? data { get; set; }

    public string? mapCode { get; set; }

    public string? mapDataCode { get; set; }

    public string? stgBinCode { get; set; }

    public string method { get; set; }

    public string? podCode { get; set; }

    public string? podDir { get; set; }

    public string? materialLot { get; set; }

    public string? materialType { get; set; }

    public string robotCode { get; set; }

    public string taskCode { get; set; }

    public string? wbCode { get; set; }

    public string? ctnrCode { get; set; }

    public string? ctnrType { get; set; }

    public string? roadWayCode { get; set; }

    public string? seq { get; set; }

    public string? eqpCode { get; set; }

    public class StatusMaterialData
    {
        /// <summary>
        ///
        /// </summary>
        public string? lot { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? materialCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? qty { get; set; }
    }

    public StdArrivedRequestEntityV2()
    {
        reqCode = string.Empty;
        reqTime = string.Empty;
        cooX = 0;
        cooY = 0;
        currentPositionCode = string.Empty;
        data = string.Empty;
        mapCode = string.Empty;
        mapDataCode = string.Empty;
        stgBinCode = string.Empty;
        method = string.Empty;
        podCode = string.Empty;
        podDir = string.Empty;
        materialLot = string.Empty;
        materialType = string.Empty;
        robotCode = string.Empty;
        taskCode = string.Empty;
        wbCode = string.Empty;
        ctnrCode = string.Empty;
        ctnrType = string.Empty;
        roadWayCode = string.Empty;
        seq = string.Empty;
        eqpCode = string.Empty;
    }
}
