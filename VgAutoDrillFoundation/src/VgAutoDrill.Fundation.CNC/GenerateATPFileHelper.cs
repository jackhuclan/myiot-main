namespace VgAutoDrill.Fundation.CNC;

public class GenerateATPFileHelper
{
    public TParameter[] tParams = new TParameter[300];
    public ShortSlotInf[] shortSlotInfs = new ShortSlotInf[35];
    public LongSlotInf[] longSlotInfs = new LongSlotInf[35];
    public BParameter[] bSlotInfs = new BParameter[35];
    public ToleranceParameter[] toleranceParameters = new ToleranceParameter[51];

    public string[,] lParams = new string[200, 10];
    public string[,] wParams = new string[200, 10];
    public string[,] hParams = new string[200, 10];

    public string[,] nParams = new string[35, 10];
    public string[,] aParams = new string[35, 10];
    public string[,] bParams = new string[35, 10];

    public GenerateATPFileHelper()
    {
        var indexArray = Enumerable.Range(1, 300).ToArray();
        tParams = indexArray.Select(i => new TParameter(i)).ToArray();

        //tParams[0].Position.AddRange(new List<int>() { 2,14,6});
        //tParams[4].Position.AddRange(new List<int>() { 22, 34, 46 });
        //tParams[2].Position.Add(1);
        //tParams[2].Position.Add(3);
        //tParams[2].Position.Add(5);

        //tParams[5].Position.AddRange(new List<int>() { 122, 134, 146 });

        tParams[0].ToolMagazines.Add(new ToolLifeInfo(2, "20", ToolLifeType.U));
        tParams[0].ToolMagazines.Add(new ToolLifeInfo(14, "30", ToolLifeType.U));
        tParams[0].ToolMagazines.Add(new ToolLifeInfo(6, "30", ToolLifeType.U));

        tParams[4].ToolMagazines.Add(new ToolLifeInfo(22, "20", ToolLifeType.U));
        tParams[4].ToolMagazines.Add(new ToolLifeInfo(34, "30", ToolLifeType.U));
        tParams[4].ToolMagazines.Add(new ToolLifeInfo(46, "30", ToolLifeType.U));

        tParams[5].ToolMagazines.Add(new ToolLifeInfo(122, "20", ToolLifeType.U));
        tParams[5].ToolMagazines.Add(new ToolLifeInfo(134, "30", ToolLifeType.U));
        tParams[5].ToolMagazines.Add(new ToolLifeInfo(146, "30", ToolLifeType.U));

        tParams[2].ToolMagazines.Add(new ToolLifeInfo(1, "20", ToolLifeType.U));
        tParams[2].ToolMagazines.Add(new ToolLifeInfo(3, "30", ToolLifeType.U));
        tParams[2].ToolMagazines.Add(new ToolLifeInfo(5, "30", ToolLifeType.U));

        var indexShortArray = Enumerable.Range(1, 35).ToArray();
        shortSlotInfs = indexShortArray.Select(i => new ShortSlotInf(i)).ToArray();
        longSlotInfs = indexShortArray.Select(i => new LongSlotInf(i)).ToArray();
        bSlotInfs = indexShortArray.Select(i => new BParameter(i)).ToArray();

        var indexToleranceArray = Enumerable.Range(1, 35).ToArray();
        toleranceParameters = indexToleranceArray.Select(i => new ToleranceParameter(i)).ToArray();

    }

    public void GenerateATPfile(string path)
    {
        if (File.Exists(path)) { File.Delete(path); }
        List<string> files = new List<string>();
        files.Add("%%5001");
        files.Add("$");
        tParams.ToList().ForEach(x => files.Add(x.ToString()));
        //for (int i = 0; i < tParams.Length; i++)
        //{
        //    files.Add(tParams[i].ToString());
        //}
        //收集排刀位置
        #region old
        //var lSet = tParams.SelectMany(x => x.Position.Select(y=>new LParameterMetada(y-1,x.ToolNum))).ToList();
        //lSet.ForEach(x => {
        //   lParams[x.Index/10,x.Index%10]=x.ToolNum; 
        //});
        //for (int i = 0; i < lParams.GetLength(0); i++)
        //{
        //    var row = Enumerable.Range(0, lParams.GetLength(1))
        //                       .Select(x => lParams[i, x])
        //                       .ToArray();
        //    files.Add(new LParameter(i+1) { ToolNum=row }.ToString());

        //}
        #endregion

        #region New
        var lSet = tParams.SelectMany(x => x.ToolMagazines.Select(y => new LParameterMetada(y.Position - 1, x.ToolNum))).ToList();
        lSet.ForEach(x =>
        {
            lParams[x.Index / 10, x.Index % 10] = x.ToolNum;
        });
        for (int i = 0; i < lParams.GetLength(0); i++)
        {
            var row = Enumerable.Range(0, lParams.GetLength(1))
                               .Select(x => lParams[i, x])
                               .ToArray();
            files.Add(new LParameter(i + 1) { ToolNum = row }.ToString());

        }


        var wSet = tParams.SelectMany(x => x.ToolMagazines.Select(y => new LParameterMetada(y.Position - 1, y.ToolLifeType.ToString()))).ToList();
        wSet.ForEach(x =>
        {
            wParams[x.Index / 10, x.Index % 10] = x.ToolNum;
        });
        for (int i = 0; i < wParams.GetLength(0); i++)
        {
            var row = Enumerable.Range(0, wParams.GetLength(1))
                               .Select(x => wParams[i, x])
                               .ToArray();
            files.Add(new WParameter(i + 1) { ToolNum = row }.ToString());

        }


        var hSet = tParams.SelectMany(x => x.ToolMagazines.Select(y => new LParameterMetada(y.Position - 1, y.ToolUsedLife))).ToList();
        hSet.ForEach(x =>
        {
            hParams[x.Index / 10, x.Index % 10] = x.ToolNum;
        });
        for (int i = 0; i < hParams.GetLength(0); i++)
        {
            var row = Enumerable.Range(0, hParams.GetLength(1))
                               .Select(x => hParams[i, x])
                               .ToArray();
            files.Add(new HParameter(i + 1) { ToolNum = row }.ToString());

        }
        #endregion

        toleranceParameters.ToList().ForEach(x => files.Add(x.ToString()));
        shortSlotInfs.ToList().ForEach(x => files.Add(x.ToString()));
        longSlotInfs.ToList().ForEach(x => files.Add(x.ToString()));
        bSlotInfs.ToList().ForEach(x => files.Add(x.ToString()));

        files.Add("$");
        File.WriteAllLines(path, files);

    }

}
