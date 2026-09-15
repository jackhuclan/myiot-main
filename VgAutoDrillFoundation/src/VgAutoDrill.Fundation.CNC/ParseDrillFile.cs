namespace VgAutoDrill.Fundation.CNC;

public class ParseDrillFile
{
    public string LastErrorMessage { get; set; }
    public Dictionary<string, ToolEntity> ToolEntitiesDic { get; set; } = new Dictionary<string, ToolEntity>();
    public int gloablM25 = -1;
    private List<Block> BlockInf = new List<Block>();
    public bool ParseFile(string filePath)
    {
        ToolEntitiesDic = new Dictionary<string, ToolEntity>();
        LastErrorMessage = string.Empty;
        if (!File.Exists(filePath))
        {
            LastErrorMessage = "文件不存在";
            return false;
        }
        string[] content = File.ReadAllLines(filePath);
        content = content.Select(x => x.Trim()).ToArray();
        int indexSeparator = content.ToList().IndexOf("%");
        if (indexSeparator == -1)
        {
            return false;
        }
        if (!ParseHead(content, indexSeparator))
        {
            return false;
        }
        if (!ParseObj(content, indexSeparator))
        {
            return false;
        }
        return true;
    }
    private bool ParseHead(string[] content, int endIndex)
    {
        if (content.Length == 0)
        {
            return false;
        }
        int realEndIndex = Math.Min(endIndex, content.Length);
        var tmpContent = content.Take(realEndIndex)
                                .Select(x => string.Join("", x.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                                .ToList();
        int indexM48 = tmpContent.IndexOf("M48");
        if (indexM48 == -1) { return false; }

        foreach (string line in tmpContent)
        {
            if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("T"))
            {
                ParseHeadT(line);
            }
        }

        return false;
    }
    private void ParseHeadT(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return;
        int indexT = line.IndexOf("T", StringComparison.CurrentCultureIgnoreCase);
        if (indexT == -1) return;
        int indexC = line.IndexOf("C", indexT, StringComparison.CurrentCultureIgnoreCase);
        if (indexC == -1) return;
        ToolEntity toolEntity = new ToolEntity()
        {
            ToolNum = line.Substring(indexT, indexC - indexT),
            ToolDia = line.Substring(indexC),
            ToolCount = 0
        };
        ToolEntitiesDic[toolEntity.ToolNum] = toolEntity;
    }

    private bool ParseObj(string[] content, int startIndex)
    {
        if (content == null || content.Length < startIndex) return false;
        var tmpContent = content.Skip(startIndex).Select(x => x.Trim()).ToArray();


        //List<int> indexT = new List<int>();
        //Stack<int> indexM25=new Stack<int>();
        //if (content== null || content.Length <startIndex) return false;
        //var tmpContent= content.Skip(startIndex).Select(x=>x.Trim()).ToArray();
        //for (int i = 0; i < tmpContent.Length; i++)
        //{
        //    var tmpLine = content[i].Trim();
        //    if (!string.IsNullOrWhiteSpace(tmpLine)&& tmpLine.StartsWith("T"))
        //    {
        //        indexT.Add(i);
        //    }
        //    if (!string.IsNullOrWhiteSpace(tmpLine) && tmpLine.StartsWith("M25"))
        //    {
        //        indexM25.Push(i);
        //    }
        //    if (!string.IsNullOrWhiteSpace(tmpLine) && tmpLine.StartsWith("M01"))
        //    {
        //        if (indexM25.Count <= 0) continue;
        //        BlockInf.Add(new Block() { StartIndex = indexM25.Pop(), EndIndex = i });
        //    }
        //}
        //while (indexM25.Count > 0)
        //{
        //    BlockInf.Add(new Block() { StartIndex = indexM25.Pop() });
        //}

        //for (int i = 0; i < indexT.Count-1; i++)
        //{
        //    var parseTContent = tmpContent.Skip(indexT[i]).Take(indexT[i + 1] - indexT[i]).ToArray();

        //    if (parseTContent.Length >0)
        //    {
        //        ParseTContent(parseTContent);
        //    }
        //}

        return false;
    }
    private void ParseTContent(string[] content)
    {
        if (content == null || content.Length == 0) return;
        int count = content.Length;
        string key = content[0];
        if (string.IsNullOrWhiteSpace(key))
        {

        }


    }


    private bool ParseNoneGM(string line)
    {

        return false;
    }

    private bool ParseM(string line)
    {

        return false;
    }

    private bool ParseG(string line)
    {

        return false;
    }

    private bool ParseObjT(string line)
    {

        return false;
    }


    private bool ParseObjG(string line)
    {

        return false;
    }
    private bool ParseObjM(string line)
    {
        int addCount = 0;
        if (string.IsNullOrEmpty(line) || line.Length < 3) return false;
        switch (line.ToUpper().Take(3))
        {
            case "M25":
                addCount += -1;
                break;
            case "M01":
                break;
            case "M02":
                break;
        }
        return false;
    }

}
