// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using Opc.Ua.Client;
using OpcUaHelper;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;

public class OpcUaCNCConnector : ICNCConnector, IDisposable
{
    public event Action? OnCNCConnected;

    public event Action? OnCNCDisconnected;

    public event Action<Exception>? OnCNCConnectException;

    public event Action<Exception>? OnCNCDisonnectException;

    public event Action<Exception>? OnCNCDataReceivedException;

    public event Action? OnCNCConnectFailed;

    public event Action? OnCNCDataReceived;

    public event Action? OnCNCClosed;

    public event Action? OnCNCError;

    public event Action? OnCNCDataSent;

    public event Action<Exception>? OnCNCDataSentException;

    private volatile bool _isConnected = false;
    private readonly OpcUaClient _opcUaClient;
    private readonly OpcUaClientOptions _opcUaClientOptions;
    private readonly ILogger<OpcUaCNCConnector> _logger;
    public IHostApplicationLifetime _hostApplicationLifetime;

    //private readonly Dictionary<string, SubscribeCallback> _callbacks = new();
    private readonly IAsyncTaskWaiter _asyncTaskWaiter;

    private const string _nullTimeValue = "0001/1/1 0:00:00";
    private const int _timeDiffer = 8; //时差
    private CncErrors? _oldCncErrors;
    private string _curCncError = string.Empty;
    private string _curSAX = string.Empty;
    private string _curSAY = string.Empty;
    private string _curSAZX = string.Empty;
    private string _curSAZY = string.Empty;
    private static Dictionary<Int32, Double> ToolSpeedArray = new Dictionary<Int32, Double>();
    private string _cncBlockColor = string.Empty;
    private string _cncShowText = string.Empty;
    private string _shiftsStartTime = string.Empty;

    public bool IsConnected
    {
        get => _isConnected;
        private set => _isConnected = value;
    }
    public List<string> EcList { get; set; } = [];
    public DrillCommonDataA _drillCommonDataA { get; set; }
    public DrillStatusData _drillStatusData { get; set; }

    public OpcUaCNCConnector(IOptions<OpcUaClientOptions> options,
        ILogger<OpcUaCNCConnector> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IAsyncTaskWaiter asyncTaskWaiter)
    {
        _opcUaClient = new OpcUaClient();
        _opcUaClientOptions = options.Value;
        InitEcList();
        _opcUaClient.UserIdentity = new UserIdentity(_opcUaClientOptions.UserName, _opcUaClientOptions.Password);
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _asyncTaskWaiter = asyncTaskWaiter;

        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
    }

    private void InitEcList()
    {
        EcList = [];
        EcList.Add("0000393218");
        EcList.Add("0000393219");
        EcList.Add("0000393220");
        EcList.Add("0000393221");
        EcList.Add("0000393222");
        EcList.Add("0000393223");
        EcList.Add("0000393224");
        EcList.Add("0000393225");
        EcList.Add("0000393226");
        EcList.Add("0000393227");
        EcList.Add("0000393228");
        EcList.Add("0000393229");
        EcList.Add("0000393230");
        EcList.Add("0000393231");
        EcList.Add("0000393232");
        EcList.Add("0000393233");
        EcList.Add("0000393234");
        EcList.Add("0000393235");
        EcList.Add("0000393236");
        EcList.Add("0000393237");
        EcList.Add("0000393238");
        EcList.Add("0000393239");
        EcList.Add("0000393240");
        EcList.Add("0000393241");
        EcList.Add("0000393242");
        EcList.Add("0000393243");
        EcList.Add("0000397313");
        EcList.Add("0000397314");
        EcList.Add("0000397315");
        EcList.Add("0000397316");
        EcList.Add("0000397317");
        EcList.Add("0000397318");
        EcList.Add("0000397319");
        EcList.Add("0000397320");
        EcList.Add("0000397321");
        EcList.Add("0000397322");
        EcList.Add("0000397323");
        EcList.Add("0000397324");
        EcList.Add("0000458755");
        EcList.Add("0000458756");
        EcList.Add("0000458757");
        EcList.Add("0000458758");
        EcList.Add("0000458759");
        EcList.Add("0000458760");
        EcList.Add("0000458761");
        EcList.Add("0000458762");
        EcList.Add("0000458763");
        EcList.Add("0000458764");
        EcList.Add("0000458765");
        EcList.Add("0000458766");
        EcList.Add("0000458767");
        EcList.Add("0000458768");
        EcList.Add("0268435460");
        EcList.Add("0268632065");
        EcList.Add("0268632066");
        EcList.Add("0268632067");
        EcList.Add("0269090819");
        EcList.Add("0269091073");
        EcList.Add("0269094913");
        EcList.Add("0269094914");
        EcList.Add("0269094915");
        EcList.Add("0269094916");
        EcList.Add("0269094917");
        EcList.Add("0269094918");
        EcList.Add("0269094919");
        EcList.Add("0269094920");
        EcList.Add("0269094921");
        EcList.Add("0269094922");
        EcList.Add("0269094923");
        EcList.Add("0269094924");
        EcList.Add("0269094925");
        EcList.Add("0269094926");
        EcList.Add("0269094927");
        EcList.Add("0269103106");
        EcList.Add("0269111297");
        EcList.Add("0269111298");
        EcList.Add("0269111299");
        EcList.Add("0269111300");
        EcList.Add("0269111301");
        EcList.Add("0269111302");
        EcList.Add("0269111303");
        EcList.Add("0269111304");
        EcList.Add("0269157377");
        EcList.Add("0269157378");
        EcList.Add("0269157379");
        EcList.Add("0269157633");
        EcList.Add("0269157634");
        EcList.Add("0269157635");
        EcList.Add("0269157636");
        EcList.Add("0269157637");
        EcList.Add("0269157638");
        EcList.Add("0269157639");
        EcList.Add("0269157640");
        EcList.Add("0269157889");
        EcList.Add("0269157890");
        EcList.Add("0269157891");
        EcList.Add("0269157892");
        EcList.Add("0269157893");
        EcList.Add("0269157894");
        EcList.Add("0536870915");
        EcList.Add("0536870916");
        EcList.Add("0536870917");
        EcList.Add("0536870918");
        EcList.Add("0536870919");
        EcList.Add("0536870920");
        EcList.Add("0536870921");
        EcList.Add("0536870922");
        EcList.Add("0536870923");
        EcList.Add("0536870924");
        EcList.Add("0536870925");
        EcList.Add("0536936449");
        EcList.Add("0536936450");
        EcList.Add("0536936451");
        EcList.Add("0536936452");
        EcList.Add("0536936453");
        EcList.Add("0536936454");
        EcList.Add("0536936455");
        EcList.Add("0536936456");
        EcList.Add("0536936457");
        EcList.Add("0536936458");
        EcList.Add("0536936459");
        EcList.Add("0536936460");
        EcList.Add("0536936461");
        EcList.Add("0536936462");
        EcList.Add("0536936463");
        EcList.Add("0536936464");
        EcList.Add("0536936465");
        EcList.Add("0536936466");
        EcList.Add("0536936467");
        EcList.Add("0536936468");
        EcList.Add("0536936469");
        EcList.Add("0536936470");
        EcList.Add("0536936471");
        EcList.Add("0536936472");
        EcList.Add("0536936473");
        EcList.Add("0536936474");
        EcList.Add("0536936475");
        EcList.Add("0536936476");
        EcList.Add("0536936477");
        EcList.Add("0536936478");
        EcList.Add("0536936479");
        EcList.Add("0536936480");
        EcList.Add("0536936481");
        EcList.Add("0536936482");
        EcList.Add("0536936483");
        EcList.Add("0536936484");
        EcList.Add("0536936485");
        EcList.Add("0536936486");
        EcList.Add("0536936487");
        EcList.Add("0536936488");
        EcList.Add("0536936489");
        EcList.Add("0536936490");
        EcList.Add("0536936491");
        EcList.Add("0536936492");
        EcList.Add("0536936493");
        EcList.Add("0536936494");
        EcList.Add("0536936495");
        EcList.Add("0536936496");
        EcList.Add("0536936497");
        EcList.Add("0536936498");
        EcList.Add("0536936499");
        EcList.Add("0536936500");
        EcList.Add("0536936501");
        EcList.Add("0536936502");
        EcList.Add("0536936503");
        EcList.Add("0536936504");
        EcList.Add("0536936505");
        EcList.Add("0536936506");
        EcList.Add("0536936507");
        EcList.Add("0536936508");
        EcList.Add("0536936509");
        EcList.Add("0536936510");
        EcList.Add("0536936511");
        EcList.Add("0536936512");
        EcList.Add("0536936513");
        EcList.Add("0536936514");
        EcList.Add("0536936515");
        EcList.Add("0536936516");
        EcList.Add("0536936517");
        EcList.Add("0536936518");
        EcList.Add("0536936519");
        EcList.Add("0536936520");
        EcList.Add("0536936521");
        EcList.Add("0536936522");
        EcList.Add("0536936523");
        EcList.Add("0536936524");
        EcList.Add("0536936525");
        EcList.Add("0536936526");
        EcList.Add("0536936527");
        EcList.Add("0536936528");
        EcList.Add("0536936529");
        EcList.Add("0536936530");
        EcList.Add("0536936531");
        EcList.Add("0536936532");
        EcList.Add("0536936533");
        EcList.Add("0536936534");
        EcList.Add("0536936535");
        EcList.Add("0536936536");
        EcList.Add("0536936537");
        EcList.Add("0536936538");
        EcList.Add("0536936539");
        EcList.Add("0536936540");
        EcList.Add("0536936541");
        EcList.Add("0536936542");
        EcList.Add("0536936543");
        EcList.Add("0536936544");
        EcList.Add("0536936545");
        EcList.Add("0536936546");
        EcList.Add("0536936547");
        EcList.Add("0536936548");
        EcList.Add("0536936549");
        EcList.Add("0536936550");
        EcList.Add("0536936551");
        EcList.Add("0536936552");
        EcList.Add("0536936553");
        EcList.Add("0536936554");
        EcList.Add("0536936555");
        EcList.Add("0536936556");
        EcList.Add("0536936557");
        EcList.Add("0536936558");
        EcList.Add("0536936559");
        EcList.Add("0536936560");
        EcList.Add("0536936561");
        EcList.Add("0536936562");
        EcList.Add("0536936563");
        EcList.Add("0536936564");
        EcList.Add("0536936565");
        EcList.Add("0536936566");
        EcList.Add("0536936567");
        EcList.Add("0536936568");
        EcList.Add("0536936569");
        EcList.Add("0536936570");
        EcList.Add("0536936571");
        EcList.Add("0536936572");
        EcList.Add("0536936573");
        EcList.Add("0536936582");
        EcList.Add("0536936583");
        EcList.Add("0536936584");
        EcList.Add("0536936585");
        EcList.Add("0536936586");
        EcList.Add("0536936587");
        EcList.Add("0536936588");
        EcList.Add("0536936589");
        EcList.Add("0536936590");
        EcList.Add("0536936591");
        EcList.Add("0536936592");
        EcList.Add("0536936593");
        EcList.Add("0536936594");
        EcList.Add("0536936595");
        EcList.Add("0536936596");
        EcList.Add("0536936597");
        EcList.Add("0536936598");
        EcList.Add("0536936599");
        EcList.Add("0536936600");
        EcList.Add("0536936601");
        EcList.Add("0536936602");
        EcList.Add("0536936603");
        EcList.Add("0536936604");
        EcList.Add("0536936605");
        EcList.Add("0536936606");
        EcList.Add("0536936607");
        EcList.Add("0536936608");
        EcList.Add("0536936609");
        EcList.Add("0536936610");
        EcList.Add("0536936611");
        EcList.Add("0536936612");
        EcList.Add("0536936613");
        EcList.Add("0536936614");
        EcList.Add("0536936615");
        EcList.Add("0536936616");
        EcList.Add("0536936617");
        EcList.Add("0536936618");
        EcList.Add("0536936619");
        EcList.Add("0536936620");
        EcList.Add("0536936621");
        EcList.Add("0536936622");
        EcList.Add("0536936623");
        EcList.Add("0536936624");
        EcList.Add("0536936626");
        EcList.Add("0536936627");
        EcList.Add("0536936628");
        EcList.Add("0536936629");
        EcList.Add("0536936631");
        EcList.Add("0536936633");
        EcList.Add("0536936634");
        EcList.Add("0536936636");
        EcList.Add("0536936637");
        EcList.Add("0536936639");
        EcList.Add("0536936640");
        EcList.Add("0536936641");
        EcList.Add("0536936645");
        EcList.Add("0536936647");
        EcList.Add("0536936648");
        EcList.Add("0537007360");
        EcList.Add("0537007361");
        EcList.Add("0537007362");
        EcList.Add("0537007363");
        EcList.Add("0537007364");
        EcList.Add("0537007365");
        EcList.Add("0537007366");
        EcList.Add("0537007367");
        EcList.Add("0537007368");
        EcList.Add("0537007369");
        EcList.Add("0537007370");
        EcList.Add("0537007371");
        EcList.Add("0537007372");
        EcList.Add("0537007373");
        EcList.Add("0537007374");
        EcList.Add("0537007376");
        EcList.Add("0537007378");
        EcList.Add("0537007380");
        EcList.Add("0537007381");
        EcList.Add("0537007383");
        EcList.Add("0537007385");
        EcList.Add("0537007387");
        EcList.Add("0537007389");
        EcList.Add("0537007391");
        EcList.Add("0537007393");
        EcList.Add("0537007395");
        EcList.Add("0537007397");
        EcList.Add("0537007399");
        EcList.Add("0537007401");
        EcList.Add("0537007403");
        EcList.Add("0537007405");
        EcList.Add("0537027328");
        EcList.Add("0537027329");
        EcList.Add("0537027330");
        EcList.Add("0537027331");
        EcList.Add("0537027332");
        EcList.Add("0537027333");
        EcList.Add("0537027334");
        EcList.Add("0537027335");
        EcList.Add("0537027336");
        EcList.Add("0537047552");
        EcList.Add("0537047553");
        EcList.Add("0537047554");
        EcList.Add("0537067529");
        EcList.Add("0537067530");
        EcList.Add("0537067531");
        EcList.Add("0537067532");
        EcList.Add("0537067533");
        EcList.Add("0537067535");
        EcList.Add("0537067539");
        EcList.Add("0537067540");
        EcList.Add("0537067541");
        EcList.Add("0537067542");
        EcList.Add("0537067543");
        EcList.Add("0537067544");
        EcList.Add("0537067545");
        EcList.Add("0537067546");
        EcList.Add("0537067547");
        EcList.Add("0537067548");
        EcList.Add("0537067549");
        EcList.Add("0537067550");
        EcList.Add("0537067551");
        EcList.Add("0537067552");
        EcList.Add("0537067557");
        EcList.Add("0537067558");
        EcList.Add("0537067559");
        EcList.Add("0537067562");
        EcList.Add("0537067563");
        EcList.Add("0537067564");
        EcList.Add("0537067591");
        EcList.Add("0537067592");
        EcList.Add("0537067593");
        EcList.Add("0537067594");
        EcList.Add("0537067595");
        EcList.Add("0537067596");
        EcList.Add("0537067597");
        EcList.Add("0537067598");
        EcList.Add("0537067599");
        EcList.Add("0537067600");
        EcList.Add("0537067601");
        EcList.Add("0537067602");
        EcList.Add("0537067603");
        EcList.Add("0537067604");
        EcList.Add("0537067605");
        EcList.Add("0537067606");
        EcList.Add("0537067607");
        EcList.Add("0537067608");
        EcList.Add("0537067609");
        EcList.Add("0537067610");
        EcList.Add("0537067613");
        EcList.Add("0537067614");
        EcList.Add("0537067615");
        EcList.Add("0537067616");
        EcList.Add("0537067617");
        EcList.Add("0537067618");
        EcList.Add("0537067619");
        EcList.Add("0537067620");
        EcList.Add("0537067621");
        EcList.Add("0537067622");
        EcList.Add("0537067623");
        EcList.Add("0537067624");
        EcList.Add("0537067630");
        EcList.Add("0537067632");
        EcList.Add("0537067633");
        EcList.Add("0537067635");
        EcList.Add("0537067636");
        EcList.Add("0537067637");
        EcList.Add("0537067638");
        EcList.Add("0537067639");
        EcList.Add("0537067640");
        EcList.Add("0537067641");
        EcList.Add("0537067642");
        EcList.Add("0537067643");
        EcList.Add("0537067645");
        EcList.Add("0537067647");
        EcList.Add("0537067648");
        EcList.Add("0537133065");
        EcList.Add("0537133066");
        EcList.Add("0537133067");
        EcList.Add("0537133068");
        EcList.Add("0537133069");
        EcList.Add("0537133070");
        EcList.Add("0537133071");
        EcList.Add("0537133072");
        EcList.Add("0537133073");
        EcList.Add("0537133074");
        EcList.Add("0537133075");
        EcList.Add("0537133076");
        EcList.Add("0537133077");
        EcList.Add("0537133078");
        EcList.Add("0537133079");
        EcList.Add("0537133080");
        EcList.Add("0537133081");
        EcList.Add("0537133082");
        EcList.Add("0537133083");
        EcList.Add("0537133084");
        EcList.Add("0537133085");
        EcList.Add("0537133087");
        EcList.Add("0537133088");
        EcList.Add("0537133089");
        EcList.Add("0537133090");
        EcList.Add("0537133091");
        EcList.Add("0537133093");
        EcList.Add("0537133094");
        EcList.Add("0537133095");
        EcList.Add("0537133097");
        EcList.Add("0537133098");
        EcList.Add("0537133099");
        EcList.Add("0537133101");
        EcList.Add("0537133102");
        EcList.Add("0537133103");
        EcList.Add("0537133105");
        EcList.Add("0537133106");
        EcList.Add("0537133107");
        EcList.Add("0537133108");
        EcList.Add("0537133109");
        EcList.Add("0537133110");
        EcList.Add("0537133111");
        EcList.Add("0537133112");
        EcList.Add("0537133113");
        EcList.Add("0537133114");
        EcList.Add("0537133115");
        EcList.Add("0537133116");
        EcList.Add("0537133117");
        EcList.Add("0537133118");
        EcList.Add("0537133119");
        EcList.Add("0537133120");
        EcList.Add("0537133121");
        EcList.Add("0537133122");
        EcList.Add("0537133123");
        EcList.Add("0537133124");
        EcList.Add("0537133125");
        EcList.Add("0537133126");
        EcList.Add("0537133127");
        EcList.Add("0537133128");
        EcList.Add("0537198593");
        EcList.Add("0537198594");
        EcList.Add("0537198598");
        EcList.Add("0537198599");
        EcList.Add("0537198600");
        EcList.Add("0537198604");
        EcList.Add("0537198605");
        EcList.Add("0537198607");
        EcList.Add("0537198608");
        EcList.Add("0537198609");
        EcList.Add("0537198613");
        EcList.Add("0537198614");
        EcList.Add("0537198615");
        EcList.Add("0537264129");
        EcList.Add("0537264131");
        EcList.Add("0537264132");
        EcList.Add("0537264133");
        EcList.Add("0537264134");
        EcList.Add("0537264135");
        EcList.Add("0537264136");
        EcList.Add("0537264137");
        EcList.Add("0537264142");
        EcList.Add("0537264143");
        EcList.Add("0537264144");
        EcList.Add("0537264145");
        EcList.Add("0537264146");
        EcList.Add("0537264147");
        EcList.Add("0537264148");
        EcList.Add("0537264149");
        EcList.Add("0537264150");
        EcList.Add("0537264151");
        EcList.Add("0537264152");
        EcList.Add("0537264153");
        EcList.Add("0537264154");
        EcList.Add("0537264155");
        EcList.Add("0537264156");
        EcList.Add("0537264157");
        EcList.Add("0537264158");
        EcList.Add("0537264159");
        EcList.Add("0537264160");
        EcList.Add("0537264161");
        EcList.Add("0537264162");
        EcList.Add("0537264163");
        EcList.Add("0537264164");
        EcList.Add("0537264165");
        EcList.Add("0537264166");
        EcList.Add("0537264167");
        EcList.Add("0537264168");
        EcList.Add("0537264169");
        EcList.Add("0537264170");
        EcList.Add("0537264171");
        EcList.Add("0537264172");
        EcList.Add("0537264173");
        EcList.Add("0537264174");
        EcList.Add("0537264175");
        EcList.Add("0537264176");
        EcList.Add("0537264177");
        EcList.Add("0537264178");
        EcList.Add("0537264179");
        EcList.Add("0537264180");
        EcList.Add("0537264181");
        EcList.Add("0537264182");
        EcList.Add("0537264183");
        EcList.Add("0537264184");
        EcList.Add("0537264185");
        EcList.Add("0537264186");
        EcList.Add("0537264187");
        EcList.Add("0537264188");
        EcList.Add("0537264189");
        EcList.Add("0537264190");
        EcList.Add("0537264191");
        EcList.Add("0537264192");
        EcList.Add("0537264193");
        EcList.Add("0537264194");
        EcList.Add("0537264195");
        EcList.Add("0537264196");
        EcList.Add("0537264197");
        EcList.Add("0537264198");
        EcList.Add("0537264199");
        EcList.Add("0537264200");
        EcList.Add("0537264201");
        EcList.Add("0537264202");
        EcList.Add("0537264203");
        EcList.Add("0537264204");
        EcList.Add("0537264205");
        EcList.Add("0537264206");
        EcList.Add("0537264207");
        EcList.Add("0537264208");
        EcList.Add("0537264209");
        EcList.Add("0537329666");
        EcList.Add("0537329667");
        EcList.Add("0537329668");
        EcList.Add("0537329669");
        EcList.Add("0537329670");
        EcList.Add("0537329671");
        EcList.Add("0537329672");
        EcList.Add("0537329673");
        EcList.Add("0537329674");
        EcList.Add("0537329675");
        EcList.Add("0537329677");
        EcList.Add("0537329678");
        EcList.Add("0537395223");
        EcList.Add("0537395224");
        EcList.Add("0537395230");
        EcList.Add("0537395231");
        EcList.Add("0537395232");
        EcList.Add("0537395234");
        EcList.Add("0537395235");
        EcList.Add("0537395236");
        EcList.Add("0537395247");
        EcList.Add("0537395248");
        EcList.Add("0537395254");
        EcList.Add("0537395257");
        EcList.Add("0537395260");
        EcList.Add("0537395262");
        EcList.Add("0537395263");
        EcList.Add("0537395264");
        EcList.Add("0537395265");
        EcList.Add("0537395266");
        EcList.Add("0537395267");
        EcList.Add("0537395268");
        EcList.Add("0537395269");
        EcList.Add("0537395270");
        EcList.Add("0537395271");
        EcList.Add("0537395272");
        EcList.Add("0537395273");
        EcList.Add("0537395274");
        EcList.Add("0537395275");
        EcList.Add("0537395276");
        EcList.Add("0537395277");
        EcList.Add("0537395278");
        EcList.Add("0537395279");
        EcList.Add("0537395280");
        EcList.Add("0537395297");
        EcList.Add("0537395298");
        EcList.Add("0537395299");
        EcList.Add("0537395300");
        EcList.Add("0537395301");
        EcList.Add("0537395302");
        EcList.Add("0537395303");
        EcList.Add("0537395304");
        EcList.Add("0537395305");
        EcList.Add("0537395306");
        EcList.Add("0537395307");
        EcList.Add("0537395308");
        EcList.Add("0537395309");
        EcList.Add("0537395310");
        EcList.Add("0537395311");
        EcList.Add("0537395312");
        EcList.Add("0537526273");
        EcList.Add("0537526274");
        EcList.Add("0537526275");
        EcList.Add("0537526276");
        EcList.Add("0537526277");
        EcList.Add("0537526278");
        EcList.Add("0537526279");
        EcList.Add("0537526280");
        EcList.Add("0537526281");
        EcList.Add("0537526282");
        EcList.Add("0537591809");
        EcList.Add("0537591810");
        EcList.Add("0537657346");
        EcList.Add("0537657347");
        EcList.Add("0537657348");
        EcList.Add("0537657349");
        EcList.Add("0537657350");
        EcList.Add("0537657351");
        EcList.Add("0537657352");
        EcList.Add("0537657353");
        EcList.Add("0537657354");
        EcList.Add("0537657356");
        EcList.Add("0537657357");
        EcList.Add("0537657358");
        EcList.Add("0537657359");
        EcList.Add("0537657360");
        EcList.Add("0537657361");
        EcList.Add("0537657362");
        EcList.Add("0537722881");
        EcList.Add("0537722882");
        EcList.Add("0537722883");
        EcList.Add("0537722884");
        EcList.Add("0537722885");
        EcList.Add("0537722886");
        EcList.Add("0537722887");
        EcList.Add("0537722888");
        EcList.Add("0537722889");
        EcList.Add("0537722890");
        EcList.Add("0537722891");
        EcList.Add("0537722892");
        EcList.Add("0537722893");
        EcList.Add("0537722894");
        EcList.Add("0537722895");
        EcList.Add("0537722896");
        EcList.Add("0537722897");
        EcList.Add("0537722898");
        EcList.Add("0537722899");
        EcList.Add("0537722900");
        EcList.Add("0537722901");
        EcList.Add("0537722902");
        EcList.Add("0537722903");
        EcList.Add("0537722904");
        EcList.Add("0537722905");
        EcList.Add("0537722906");
        EcList.Add("0537722907");
        EcList.Add("0537722908");
        EcList.Add("0537722909");
        EcList.Add("0537722910");
        EcList.Add("0537722911");
        EcList.Add("0537722912");
        EcList.Add("0537722913");
        EcList.Add("0537722914");
        EcList.Add("0537722915");
        EcList.Add("0537722916");
        EcList.Add("0537722917");
        EcList.Add("0537722918");
        EcList.Add("0537722919");
        EcList.Add("0537722920");
        EcList.Add("0537722921");
        EcList.Add("0537722922");
        EcList.Add("0537722923");
        EcList.Add("0537722924");
        EcList.Add("0537722925");
        EcList.Add("0537722926");
        EcList.Add("0537722927");
        EcList.Add("0537722928");
        EcList.Add("0537722929");
        EcList.Add("0537722930");
        EcList.Add("0537722931");
        EcList.Add("0537722932");
        EcList.Add("0537722933");
        EcList.Add("0537722934");
        EcList.Add("0537722935");
        EcList.Add("0537722936");
        EcList.Add("0537722937");
        EcList.Add("0537722938");
        EcList.Add("0537722939");
        EcList.Add("0537722940");
        EcList.Add("0537722941");
        EcList.Add("0537722942");
        EcList.Add("0537722943");
        EcList.Add("0537722944");
        EcList.Add("0537722945");
        EcList.Add("0537722946");
        EcList.Add("0537722947");
        EcList.Add("0537722948");
        EcList.Add("0537722949");
        EcList.Add("0537722950");
        EcList.Add("0537722951");
        EcList.Add("0537722952");
        EcList.Add("0537722953");
        EcList.Add("0537722954");
        EcList.Add("0537722955");
        EcList.Add("0537722956");
        EcList.Add("0537722957");
        EcList.Add("0537722958");
        EcList.Add("0537722959");
        EcList.Add("0537722960");
        EcList.Add("0537722961");
        EcList.Add("0537722962");
        EcList.Add("0537722963");
        EcList.Add("0537722964");
        EcList.Add("0537722965");
        EcList.Add("0537722966");
        EcList.Add("0537722967");
        EcList.Add("0537722968");
        EcList.Add("0537722969");
        EcList.Add("0537722970");
        EcList.Add("0537722971");
        EcList.Add("0537722972");
        EcList.Add("0537722973");
        EcList.Add("0537722974");
        EcList.Add("0537722975");
        EcList.Add("0537722976");
        EcList.Add("0537722977");
        EcList.Add("0537722978");
        EcList.Add("0537722979");
        EcList.Add("0537722980");
        EcList.Add("0537722981");
        EcList.Add("0537722982");
        EcList.Add("0537722983");
        EcList.Add("0537722984");
        EcList.Add("0537722985");
        EcList.Add("0537722986");
        EcList.Add("0537722987");
        EcList.Add("0537722988");
        EcList.Add("0537722989");
        EcList.Add("0537722990");
        EcList.Add("0537722991");
        EcList.Add("0537722992");
        EcList.Add("0537722993");
        EcList.Add("0537722994");
        EcList.Add("0537722995");
        EcList.Add("0537722996");
        EcList.Add("0537722997");
        EcList.Add("0537722998");
        EcList.Add("0537722999");
        EcList.Add("0537723000");
        EcList.Add("0537723001");
        EcList.Add("0537723002");
        EcList.Add("0537723003");
        EcList.Add("0537723004");
        EcList.Add("0537723005");
        EcList.Add("0537723006");
        EcList.Add("0537723007");
        EcList.Add("0537723008");
        EcList.Add("0537723009");
        EcList.Add("0537723010");
        EcList.Add("0537723011");
        EcList.Add("0537723012");
        EcList.Add("0537723013");
        EcList.Add("0537723014");
        EcList.Add("0537723015");
        EcList.Add("0537723016");
        EcList.Add("0537723017");
        EcList.Add("0537723018");
        EcList.Add("0537723019");
        EcList.Add("0537723020");
        EcList.Add("0537723021");
        EcList.Add("0537723022");
        EcList.Add("0537723023");
        EcList.Add("0537723024");
        EcList.Add("0537723025");
        EcList.Add("0537723026");
        EcList.Add("0537723027");
        EcList.Add("0537723028");
        EcList.Add("0537723029");
        EcList.Add("0537723030");
        EcList.Add("0537723031");
        EcList.Add("0537723032");
        EcList.Add("0537723033");
        EcList.Add("0537723034");
        EcList.Add("0537723035");
        EcList.Add("0537723036");
        EcList.Add("0537723037");
        EcList.Add("0537723038");
        EcList.Add("0537723039");
        EcList.Add("0537723040");
        EcList.Add("0537723041");
        EcList.Add("0537723042");
        EcList.Add("0537723043");
        EcList.Add("0537723044");
        EcList.Add("0537723045");
        EcList.Add("0537723046");
        EcList.Add("0537723047");
        EcList.Add("0537723048");
        EcList.Add("0537723049");
        EcList.Add("0537723050");
        EcList.Add("0537723051");
        EcList.Add("0537723052");
        EcList.Add("0537723053");
        EcList.Add("0537723054");
        EcList.Add("0537723055");
        EcList.Add("0537723056");
        EcList.Add("0537731074");
        EcList.Add("0537731075");
        EcList.Add("0537731076");
        EcList.Add("0537731077");
        EcList.Add("0537731078");
        EcList.Add("0537731079");
        EcList.Add("0537731080");
        EcList.Add("0537731081");
        EcList.Add("0537731082");
        EcList.Add("0537731083");
        EcList.Add("0537731084");
        EcList.Add("0537731085");
        EcList.Add("0537731086");
        EcList.Add("0537731087");
        EcList.Add("0537731088");
        EcList.Add("0537731089");
        EcList.Add("0537731090");
        EcList.Add("0537731091");
        EcList.Add("0537731092");
        EcList.Add("0537788417");
        EcList.Add("0537796609");
        EcList.Add("0537804801");
        EcList.Add("0537804802");
        EcList.Add("0537804803");
        EcList.Add("0537804804");
        EcList.Add("0537804805");
        EcList.Add("0537804806");
        EcList.Add("0537804807");
        EcList.Add("0537804808");
        EcList.Add("0537804809");
        EcList.Add("0537804810");
        EcList.Add("0537804811");
        EcList.Add("0537804812");
        EcList.Add("0537804813");
        EcList.Add("0537804814");
        EcList.Add("0537804815");
        EcList.Add("0537804816");
        EcList.Add("0537804817");
        EcList.Add("0537804818");
        EcList.Add("0537804819");
        EcList.Add("0537804820");
        EcList.Add("0537804821");
        EcList.Add("0537804822");
        EcList.Add("0537804823");
        EcList.Add("0537804824");
        EcList.Add("0537804825");
        EcList.Add("0537804826");
        EcList.Add("0537804827");
        EcList.Add("0537804828");
        EcList.Add("0537804829");
        EcList.Add("0537804830");
        EcList.Add("0537804831");
        EcList.Add("0537804832");
        EcList.Add("0537804833");
        EcList.Add("0537804834");
        EcList.Add("0537804835");
        EcList.Add("0537804836");
        EcList.Add("0537804837");
        EcList.Add("0537804838");
        EcList.Add("0537804839");
        EcList.Add("0537804840");
        EcList.Add("0537804841");
        EcList.Add("0537804842");
        EcList.Add("0537804843");
        EcList.Add("0537804844");
        EcList.Add("0537804845");
        EcList.Add("0537804846");
        EcList.Add("0537804847");
        EcList.Add("0537804848");
        EcList.Add("0537804849");
        EcList.Add("0537804850");
        EcList.Add("0805306369");
        EcList.Add("0805306370");
        EcList.Add("0805306371");
        EcList.Add("0805306372");
        EcList.Add("0805306373");
        EcList.Add("0805306374");
        EcList.Add("0805306375");
        EcList.Add("0805306379");
        EcList.Add("0805306380");
        EcList.Add("0805306381");
        EcList.Add("0805343233");
        EcList.Add("0805376001");
        EcList.Add("0805380097");
        EcList.Add("0805380098");
        EcList.Add("0805384193");
        EcList.Add("0805384195");
        EcList.Add("0805384197");
        EcList.Add("0805384199");
        EcList.Add("0805384200");
        EcList.Add("0805384201");
        EcList.Add("0805388289");
        EcList.Add("0805388290");
        EcList.Add("0805388291");
        EcList.Add("0805388292");
        EcList.Add("0805388293");
        EcList.Add("0805388294");
        EcList.Add("0805388295");
        EcList.Add("0805388296");
        EcList.Add("0805388297");
        EcList.Add("0805388298");
        EcList.Add("0805388299");
        EcList.Add("0805388300");
        EcList.Add("0805388301");
        EcList.Add("0805388302");
        EcList.Add("0805502977");
        EcList.Add("0805502978");
        EcList.Add("0805502979");
        EcList.Add("0805502980");
        EcList.Add("0805502981");
        EcList.Add("0805502982");
        EcList.Add("0805502983");
        EcList.Add("0805568513");
        EcList.Add("0805568514");
        EcList.Add("0805568515");
        EcList.Add("0805568516");
        EcList.Add("0805634048");
        EcList.Add("0805634049");
        EcList.Add("0805634052");
        EcList.Add("0805634053");
        EcList.Add("0805634055");
        EcList.Add("0805634057");
        EcList.Add("0805634058");
        EcList.Add("0805634059");
        EcList.Add("0805634061");
        EcList.Add("0805634062");
        EcList.Add("0805634064");
        EcList.Add("0805634065");
        EcList.Add("0805634066");
        EcList.Add("0805634067");
        EcList.Add("0805634068");
        EcList.Add("0805634073");
        EcList.Add("0805699585");
        EcList.Add("0805703681");
        EcList.Add("0805703682");
        EcList.Add("0805703683");
        EcList.Add("0805703684");
        EcList.Add("0805703685");
        EcList.Add("0805703686");
        EcList.Add("0805703687");
        EcList.Add("0805703688");
        EcList.Add("0805703689");
        EcList.Add("0805703690");
        EcList.Add("0805703691");
        EcList.Add("0805703692");
        EcList.Add("0805703693");
        EcList.Add("0805703694");
        EcList.Add("0805703695");
        EcList.Add("0805703696");
        EcList.Add("0805703697");
        EcList.Add("0805703698");
        EcList.Add("0805703699");
        EcList.Add("0805703700");
        EcList.Add("0805703701");
        EcList.Add("0805703702");
        EcList.Add("0805703703");
        EcList.Add("0805703704");
        EcList.Add("0805703705");
        EcList.Add("0805703706");
        EcList.Add("0805703707");
        EcList.Add("0805703708");
        EcList.Add("0805703709");
        EcList.Add("0805703710");
        EcList.Add("0805703711");
        EcList.Add("0805703712");
        EcList.Add("0805703713");
        EcList.Add("0805703714");
        EcList.Add("0805703715");
        EcList.Add("0805703719");
        EcList.Add("0805703720");
        EcList.Add("0805703721");
        EcList.Add("0805703722");
        EcList.Add("0805703723");
        EcList.Add("0805703724");
        EcList.Add("0805703725");
        EcList.Add("0805703726");
        EcList.Add("0805703727");
        EcList.Add("0805703728");
        EcList.Add("0805703729");
        EcList.Add("0805703730");
        EcList.Add("0805703731");
        EcList.Add("0805703732");
        EcList.Add("0805703733");
        EcList.Add("0805703734");
        EcList.Add("0805703744");
        EcList.Add("0805703745");
        EcList.Add("0805703746");
        EcList.Add("0805765122");
        EcList.Add("0805765126");
        EcList.Add("0805765128");
        EcList.Add("0805765130");
        EcList.Add("0805765132");
        EcList.Add("0805765133");
        EcList.Add("0805765135");
        EcList.Add("0805765137");
        EcList.Add("0805765139");
        EcList.Add("0805765140");
        EcList.Add("0805765143");
        EcList.Add("0805765145");
        EcList.Add("0805765146");
        EcList.Add("0805765147");
        EcList.Add("0805765148");
        EcList.Add("0805765154");
        EcList.Add("0805765155");
        EcList.Add("0805765157");
        EcList.Add("0805765159");
        EcList.Add("0805765160");
        EcList.Add("0805765162");
        EcList.Add("0805765167");
        EcList.Add("0805765169");
        EcList.Add("0805765173");
        EcList.Add("0805765175");
        EcList.Add("0805765177");
        EcList.Add("0805765179");
        EcList.Add("0805765181");
        EcList.Add("0805765183");
        EcList.Add("0805765190");
        EcList.Add("0805765192");
        EcList.Add("0805765194");
        EcList.Add("0805765196");
        EcList.Add("0805765198");
        EcList.Add("0805765200");
        EcList.Add("0805765202");
        EcList.Add("0805765204");
        EcList.Add("0805765206");
        EcList.Add("0805765210");
        EcList.Add("0805765212");
        EcList.Add("0805765214");
        EcList.Add("0805765215");
        EcList.Add("0805765216");
        EcList.Add("0805765217");
        EcList.Add("0805765218");
        EcList.Add("0805765219");
        EcList.Add("0805765226");
        EcList.Add("0805765227");
        EcList.Add("0805961729");
        EcList.Add("0805961730");
        EcList.Add("0805961731");
        EcList.Add("0806027265");
        EcList.Add("0806027266");
        EcList.Add("0806027267");
        EcList.Add("0806027268");
        EcList.Add("0806027269");
        EcList.Add("0806027270");
        EcList.Add("0806027271");
        EcList.Add("0806027272");
        EcList.Add("0806027273");
        EcList.Add("0806027274");
        EcList.Add("0806027275");
        EcList.Add("0806027276");
        EcList.Add("0806027277");
        EcList.Add("0806027278");
        EcList.Add("0806027279");
        EcList.Add("0806027280");
        EcList.Add("0806027281");
        EcList.Add("0806027282");
        EcList.Add("0806027283");
        EcList.Add("0806027284");
        EcList.Add("0806027285");
        EcList.Add("0806027286");
        EcList.Add("0806027287");
        EcList.Add("0806027288");
        EcList.Add("0806027289");
        EcList.Add("0806027290");
        EcList.Add("0806027291");
        EcList.Add("0806027292");
        EcList.Add("0806027293");
        EcList.Add("0806027294");
        EcList.Add("0806289414");
        EcList.Add("0806486017");
        EcList.Add("0806486018");
        EcList.Add("0806486019");
        EcList.Add("0806486020");
        EcList.Add("0806486021");
        EcList.Add("0806486022");
        EcList.Add("0806486023");
        EcList.Add("0806486024");
        EcList.Add("0806486025");
        EcList.Add("0806486026");
        EcList.Add("0806486027");
        EcList.Add("0806486028");
        EcList.Add("0806486029");
        EcList.Add("0806486030");
        EcList.Add("0806486031");
        EcList.Add("0806486032");
        EcList.Add("0806486033");
        EcList.Add("0806486034");
        EcList.Add("0806486035");
        EcList.Add("0806486036");
        EcList.Add("0806551556");
        EcList.Add("0806551559");
        EcList.Add("0806551561");
        EcList.Add("0806551562");
        EcList.Add("0806551563");
        EcList.Add("0806551564");
        EcList.Add("0806617090");
        EcList.Add("0806617091");
        EcList.Add("0806617092");
        EcList.Add("0806617093");
        EcList.Add("0806682626");
        EcList.Add("0806813699");
        EcList.Add("0806813701");
        EcList.Add("0806813705");
        EcList.Add("0806813707");
        EcList.Add("0806813709");
        EcList.Add("0806813710");
        EcList.Add("0806813711");
        EcList.Add("0806813712");
        EcList.Add("0806813713");
        EcList.Add("0806813714");
        EcList.Add("0806813715");
        EcList.Add("0806813717");
        EcList.Add("0806813721");
        EcList.Add("0806813722");
        EcList.Add("0806813723");
        EcList.Add("0806813724");
        EcList.Add("0806813725");
        EcList.Add("0806813726");
        EcList.Add("0806813727");
        EcList.Add("0806813729");
        EcList.Add("0806813731");
        EcList.Add("0806813732");
        EcList.Add("0806813733");
        EcList.Add("0806813736");
        EcList.Add("0806813737");
        EcList.Add("0806813738");
        EcList.Add("0806813739");
        EcList.Add("0806813741");
        EcList.Add("0806813742");
        EcList.Add("0806813749");
        EcList.Add("0806813757");
        EcList.Add("0806813759");
        EcList.Add("0806813760");
        EcList.Add("0806813761");
        EcList.Add("0806813762");
        EcList.Add("0806813763");
        EcList.Add("0806813765");
        EcList.Add("0806813767");
        EcList.Add("0806813769");
        EcList.Add("0806813771");
        EcList.Add("0806813772");
        EcList.Add("0806813773");
        EcList.Add("0806813774");
        EcList.Add("0806813775");
        EcList.Add("0806813776");
        EcList.Add("0806813777");
        EcList.Add("0806813778");
        EcList.Add("0806813779");
        EcList.Add("0806813781");
        EcList.Add("0806813783");
        EcList.Add("0806813784");
        EcList.Add("0806813785");
        EcList.Add("0806813786");
        EcList.Add("0806813787");
        EcList.Add("0806813788");
        EcList.Add("0806813789");
        EcList.Add("0806813790");
        EcList.Add("0806813791");
        EcList.Add("0806813792");
        EcList.Add("0806813793");
        EcList.Add("0806813805");
        EcList.Add("0807534593");
        EcList.Add("0807534594");
        EcList.Add("0807534595");
        EcList.Add("0807534596");
        EcList.Add("0807534597");
        EcList.Add("0807534598");
        EcList.Add("0807534605");
        EcList.Add("0807534606");
        EcList.Add("0807534607");
        EcList.Add("0807534847");
        EcList.Add("0807534848");
        EcList.Add("0807600129");
        EcList.Add("0807600130");
        EcList.Add("0807600131");
        EcList.Add("0807600132");
        EcList.Add("0807600133");
        EcList.Add("0807600134");
        EcList.Add("0807600135");
        EcList.Add("0807600136");
        EcList.Add("0807600137");
        EcList.Add("0807600138");
        EcList.Add("0807600139");
        EcList.Clear(); //95先暂时不用
    }

    public void Dispose()
    {
        RemoveSubscription();
        Disonnect(_hostApplicationLifetime.ApplicationStopped);
    }

    public async Task Connect(CancellationToken cancellationToken)
    {
        try
        {
#if DEBUG
            _opcUaClientOptions.ServerUrl = "opc.tcp://127.0.0.1:16664";
#endif
            await _opcUaClient.ConnectServer(_opcUaClientOptions.ServerUrl);
            _isConnected = _opcUaClient.Connected;
            await Task.Delay(1000);
            if (_isConnected)
            {
                OnCNCConnected?.Invoke();
                AddSubscription();
            }
            else
            {
                OnCNCConnectFailed?.Invoke();
            }

            //foreach (var item in _callbacks.Keys)
            //{
            //    _opcUaClient.AddSubscription(item, item,)
            //}
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OPC UA Server Connect NG\r\n" + ex.Message, "OPC UA", "warn");
            _isConnected = false;

            OnCNCConnectException?.Invoke(ex);
            await Task.Delay(5000);
        }
    }

    public void AddSubscription()
    {
        try
        {
            _opcUaClient.AddSubscription("A", "ns=4;s=UI/origin/DDETable/CncErrorTable/CncError", SubCallback);
            _opcUaClient.AddSubscription("Performance", "ns=4;s=UI/origin/DynamicInfo/Performance", SubCallback);
            _opcUaClient.AddSubscription("BlockColor", "ns=4;s=UI/origin/RosiInfo/BlockBackgroundColor", SubCallback);
            _opcUaClient.AddSubscription("ShowText", "ns=4;s=UI/normalized/new/ShowText", SubCallback);
            _opcUaClient.AddSubscription("ShiftsStartTime", "ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.StartTime", SubCallback);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public void RemoveSubscription()
    {
        try
        {
            if (_opcUaClient != null)
            {
                _opcUaClient.RemoveAllSubscription();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private static readonly object lockObject_CncError = new object();
    private static readonly object lockObject_Performance = new object();

    private void SubCallback(string key, MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args)
    {
        try
        {
            if (key == "A")
            {
                MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;

                lock (lockObject_CncError)
                {
                    string[] aryCncError = notification.Value.WrappedValue.Value as string[];

                    if (aryCncError != null)
                    {
                        var cncErrors = new CncErrors(aryCncError);
                        if (_oldCncErrors == null)
                        {
                            _oldCncErrors = cncErrors;
                            _curCncError = _oldCncErrors?.ErrorItems.FirstOrDefault() ?? string.Empty;
                        }
                        else
                        {
                            var diffItem = _oldCncErrors.PickDiffOneFromOther(cncErrors);
                            _oldCncErrors = cncErrors;
                            _curCncError = diffItem ?? string.Empty;
                        }
                    }

                    //19.06.2025 15:27:58 *  * 20030001 * COMM-SAZ1X5.678Y0.000 executed successfully
                    if (_curCncError.Contains("COMM-SAZ"))
                    {
                        string strSAZXY = _curCncError.Substring(_curCncError.IndexOf("COMM-SAZ"));
                        _curSAZX = strSAZXY.Substring(strSAZXY.IndexOf("X") + 1, strSAZXY.IndexOf("Y") - (strSAZXY.IndexOf("X") + 1));
                        _curSAZY = strSAZXY.Substring(strSAZXY.IndexOf("Y") + 1, strSAZXY.IndexOf(" ") - (strSAZXY.IndexOf("Y") + 1));
                        _logger.LogInformation($"COMM-SAZ - {strSAZXY} X {_curSAZX} Y {_curSAZY}");
                    }
                    else if (_curCncError.Contains("COMM-SA1"))
                    {
                        ///09.06.2025 11:02:48 * EBY00484 * 20030001 * COMM-SA1X1.230Y4.567 executed successfully
                        string strSAXY = _curCncError.Substring(_curCncError.IndexOf("COMM-SA1"));
                        _curSAX = strSAXY.Substring(strSAXY.IndexOf("X") + 1, strSAXY.IndexOf("Y") - (strSAXY.IndexOf("X") + 1));
                        _curSAY = strSAXY.Substring(strSAXY.IndexOf("Y") + 1, strSAXY.IndexOf(" ") - (strSAXY.IndexOf("Y") + 1));
                        _logger.LogInformation($"COMM-SA1 - {strSAXY} X {_curSAX} Y {_curSAY}");
                    }
                }
            }
            else if (key == "Performance")
            {
                MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;

                lock (lockObject_Performance)
                {
                    Double ToolSpeed = Math.Round((Double)notification.Value.WrappedValue.Value, 0);
                    Int32 CurrentToolNumber = _opcUaClient.ReadNode<Int32>("ns=4;s=UI/origin/DynamicInfo/CurrentToolNumber");
                    if (ToolSpeedArray.ContainsKey(CurrentToolNumber))
                    {
                        if (ToolSpeedArray[CurrentToolNumber] < ToolSpeed)
                        {
                            ToolSpeedArray[CurrentToolNumber] = ToolSpeed;
                        }
                    }
                    else
                    {
                        ToolSpeedArray.Add(CurrentToolNumber, ToolSpeed);
                    }
                }
            }
            else if (key == "BlockColor")
            {
                MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;
                _cncBlockColor = ((Int32)notification.Value.WrappedValue.Value).ToString();
            }
            else if (key == "ShowText")
            {
                MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;
                _cncShowText = notification.Value.WrappedValue.Value.ToString().Trim();
            }
            else if (key == "ShiftsStartTime")
            {
                MonitoredItemNotification notification = args.NotificationValue as MonitoredItemNotification;
                _shiftsStartTime = ((DateTime[])notification.Value.WrappedValue.Value)[0].ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public Task Disonnect(CancellationToken cancellationToken)
    {
        try
        {
            if (_opcUaClient != null && _opcUaClient.Connected)
            {
                _opcUaClient.Disconnect();
                _isConnected = false;
                OnCNCDisconnected?.Invoke();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DisconnectCNC Exception - " + ex.Message);

            OnCNCDisonnectException?.Invoke(ex);
        }

        return Task.CompletedTask;
    }

    public async Task<string> RetrieveData(string key, CancellationToken cancellationToken = default)
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(TimeSpan.FromSeconds(2));
        cancellationToken = tokenSource.Token;

        try
        {
            if (!_opcUaClient.Connected)
            {
                OnCNCDisconnected?.Invoke();
                return string.Empty;
            }

            if (key.Contains("_"))
            {
                string[] strInfo = key.Split("_");
                string strValue = strInfo[1];
                if (strInfo.Length > 2)
                {
                    strValue = key.Substring(key.IndexOf("_") + 1);
                }
                switch (strInfo[0])
                {
                    case "ToolD":
                        {
                            return GetToolD(strInfo[1]);
                        }
                    case "ToolS":
                        {
                            return GetToolS(strInfo[1]);
                        }
                    case "ToolF":
                        {
                            return GetToolF(strInfo[1]);
                        }
                    case "ToolR":
                        {
                            return GetToolR(strInfo[1]);
                        }
                    case "ToolN":
                        {
                            return GetToolN(strInfo[1]);
                        }
                    case "ToolB":
                        {
                            return GetToolB(strInfo[1]);
                        }
                    case "ToolZ":
                        {
                            return GetToolZ(strInfo[1]);
                        }
                    case "ToolA":
                        {
                            return GetToolA(strInfo[1]);
                        }
                    case "ToolSegM":
                        {
                            return "";
                        }
                    case "ToolChipl":
                        {
                            return GetToolChipl(strInfo[1]);
                        }
                    case "ToolZOffset":
                        {
                            return GetToolZOffset(strInfo[1]);
                        }
                    case "ToolSpeed":
                        {
                            return GetToolSpeed(strInfo[1]);
                        }
                    case "LoadPgmFile":
                        {
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.PROGRAM, strValue),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LoadDiaFile":
                        {
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.DIA, strValue),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LoadAtpFile":
                        {
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.ATP, strValue),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SpindleEnable":
                        {
                            return GetSpindleEnable(strInfo[1]);
                        }
                    case "TMeasureDia":
                        {
                            return GetTMeasureDia(strInfo[1]);
                        }
                    case "TMeasureLen":
                        {
                            return GetTMeasureLen(strInfo[1]);
                        }
                    case "TRunout":
                        {
                            return GetTRunout(strInfo[1]);
                        }
                    case "Spindle1WorkTimes":
                        {
                            return GetSpindleWorkTimes(strInfo[1]);
                        }
                    case "SpindleWorkTotalMinutes":
                        {
                            return GetSpindleWorkTotalMinutes(strInfo[1]);
                        }
                    case "Cnc9XWriteNodeData":
                        {
                            return WriteNodeData(strValue);
                        }
                    case "ToolEnalbe":
                        {
                        }
                        break;

                    case "HitEnalbe":
                        break;

                    case "ToolReqNum":
                        break;
                }
            }
            else
            {
                switch (key)
                {
                    case "CncStatus":
                        {
                            return GetCncStatusText();
                        }
                    case "BlockText":
                        {
                            return GetBlockText();
                        }
                    case "CncError":
                        {
                            return _curCncError;
                        }
                    case "CncComm":
                        {
                            return GetCncComm();
                        }
                    case "CncRunProgress":
                        {
                            return GetCncRunProgress();
                        }
                    case "PgmFilePath":
                        {
                            return GetPgmFilePath();
                        }
                    case "DiaFilePath":
                        {
                            return GetDiaFilePath();
                        }
                    case "AtpFilePath":
                        {
                            return GetAtpFilePath();
                        }
                    case "PgmRunStartTime":
                        {
                            return GetRunStartTime();
                        }
                    case "PgmRunEndTime":
                        {
                            return GetRunEndTime();
                        }
                    case "CncBlockColor":
                        {
                            return _cncBlockColor;
                        }
                    case "CurDrillOrRout":
                        {
                            return GetCurDrillOrRout();
                        }
                    case "TotalDrillOrRout":
                        {
                            return GetTotalDrillOrRout();
                        }
                    case "Duty":
                        {
                            return GetDuty();
                        }
                    case "ShiftOnlineTime":
                        {
                            return GetShiftOnlineTime();
                        }
                    case "ShiftWorkingTime":
                        {
                            return GetShiftWorkingTime();
                        }
                    case "ShiftWaitingTime":
                        {
                            return GetShiftWaitingTime();
                        }
                    case "ShiftErrorTime":
                        {
                            return GetShiftErrorTime();
                        }
                    case "XYPosition":
                        {
                            return GetXYPosition();
                        }
                    case "CurSpindleStatus":
                        {
                            ParseCncStatusText(GetCncStatusText());
                            return CncStatus.ZS;
                        }
                    case "CurToolId":
                        {
                            return GetCurToolId();
                        }
                    case "DrillH":
                        {
                            return GetDrillH();
                        }
                    case "DrillQ":
                        {
                            return GetDrillQ();
                        }
                    case "DrillK":
                        {
                            return GetDrillK();
                        }
                    case "DrillKi":
                        {
                            return GetDrillKi();
                        }
                    case "DrillZ":
                        {
                            return GetDrillZ();
                        }
                    case "Block":
                        {
                            return GetBlock();
                        }
                    case "Step":
                        {
                            return GetStep();
                        }
                    case "DrillFV":
                        {
                            return GetFV();
                        }
                    case "CncVersion":
                        {
                            return GetCNCVersion();
                        }
                    case "ShiftTotalDrillOrRout":
                        {
                            return GetShiftTotalDrillOrRout();
                        }
                    case "ShiftRunCount":
                        {
                            return GetShiftRunCount();
                        }
                    case "ShiftToolChangeTime":
                        {
                            return GetShiftToolChangeTime();
                        }
                    case "SpindleCount":
                        {
                            return GetSpindleCount();
                        }
                    case "IsCncRunning":
                        {
                            return GetIsCncRunning();
                        }
                    case "PreDuty":
                        {
                            return GetPreDuty();
                        }
                    case "OPID":
                        {
                            return GetOPID();
                        }
                    case "SAX":
                        {
                            return string.Empty;
                        }
                    case "SAY":
                        {
                            return string.Empty;
                        }
                    case "SAZX":
                        {
                            return string.Empty;
                        }
                    case "SAZY":
                        {
                            return string.Empty;
                        }
                    case "UserName":
                        {
                            return GetUserName();
                        }
                    case "UserLevel":
                        {
                            return string.Empty;
                        }
                    case "CncShowText":
                        {
                            return _cncShowText;
                        }
                    case "RunDrillHits":
                        {
                            return GetRunDrillHits();
                        }
                    case "ShiftsStartTime":
                        {
                            return _shiftsStartTime;
                        }
                    case "DiameterTolChecked":
                        {
                            return string.Empty;
                        }
                    case "DiameterTolNegValue":
                        {
                            return GetDiamterTolNeg();
                        }
                    case "DiameterTolPosValue":
                        {
                            return GetDiamterTolPos();
                        }
                    case "LengthTolChecked":
                        {
                            return string.Empty;
                        }
                    case "LengthTolNegValue":
                        {
                            return GetLengthTolNeg();
                        }
                    case "LengthTolPosValue":
                        {
                            return GetLengthTolPos();
                        }
                    case "RunoutTolChecked":
                        {
                            return string.Empty;
                        }
                    case "RunoutTolNegValue":
                        {
                            return GetRunoutTolNeg();
                        }
                    case "RunoutTolPosValue":
                        {
                            return GetRunoutTolPos();
                        }
                    case CncDataNameUnity.RecentAutoListRecords:
                        {
                            //TO DO : 返回AUTO-LIST中最后3行的内容（ List<AutoListRecord> 对应的JSON）
                            return "[]";
                        }
                    case "TParamPodCount":
                        {
                            return GetTParamPodCount();
                        }
                    case "AutoListRunNumber":
                        {
                            return GetAutoListRunNumber();
                        }
                    case "CncStart":
                        {
                            return CncStart();
                        }
                    case "CncStop":
                        {
                            return CncStop();
                        }
                    case "SpindleYaw":
                        {
                            return SpindleYaw();
                        }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return string.Empty;
    }

    private string WriteNodeData(string strWriteData)
    {
        try
        {
            string[] writeData = strWriteData.Split("@@@@");
            if (writeData.Length >= 3)
            {
                switch (writeData[1])
                {
                    case "bool":
                    case "BOOL":
                        {
                            bool bValue = bool.Parse(writeData[2]);
                            _opcUaClient.WriteNode<bool>(writeData[0], bValue);
                            _logger.LogInformation($"WriteNodeData 写入数据成功 - {writeData[0]} - {writeData[1]} - {bValue.ToString()}");
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                _logger.LogInformation($"WriteNodeData 传入参数有误 - {strWriteData}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetIsCncRunning()
    {
        bool isCncRunning = false;
        try
        {
            var Cnc9XStatus = _opcUaClient.ReadNode(new NodeId("ns=2;s=Processes/CNC95.exe"));

            if (Cnc9XStatus == null)
            {
                Cnc9XStatus = _opcUaClient.ReadNode(new NodeId("ns=2;s=Processes/CNC93.exe"));
            }

            if (Cnc9XStatus != null && Cnc9XStatus.Value != null)
            {
                var jsCnc9X = JObject.Parse(Cnc9XStatus.Value.ToString());
                if (jsCnc9X != null)
                {
                    isCncRunning = (bool)jsCnc9X["running"];
                    return isCncRunning.ToString();
                }
                else
                {
                    _logger.LogInformation("JObject.Parse is null.");
                }
            }
            else
            {
                _logger.LogInformation("GetIsCncRunning -  ns=2;s=Processes/CNC95.exe  /93 is null");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return isCncRunning.ToString();
    }

    private string GetRunDrillHits()
    {
        try
        {
            return _opcUaClient.ReadNode<UInt32>("ns=4;s=UI/origin/AutoData/AutoData.List/AutoData.RunDrillHits").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetToolSpeed(string ToolId)
    {
        try
        {
            int nToolId = 0;
            if (int.TryParse(ToolId, out nToolId))
            {
                if (ToolSpeedArray.ContainsKey(nToolId))
                {
                    return ToolSpeedArray[nToolId].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolZOffset Exception - " + ex.Message);
        }
        return "0";
    }

    private string GetToolZOffset(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ToolParameter/dnToolParameterTable/dnToolParameterParameters/TParamZPlane"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex] / 1000, 3)).ToString("F3");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolZOffset Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetTParamPodCount()
    {
        try
        {
            DataValue dataValuePodCount = _opcUaClient.ReadNode("ns=4;s=UI/origin/ToolParameter/dnToolParameterTable/dnToolParameterHitCount/TParamPodCount");
            var PodArray = (Int32[])dataValuePodCount.Value;
            return string.Join(",", PodArray);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCNCVersion()
    {
        try
        {
            return _opcUaClient.ReadNode<String>("ns=4;s=UI/origin/Parameter/Description/CncVersion").Replace(".", "");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetFV()
    {
        try
        {
            return (_opcUaClient.ReadNode<Int32>("ns=4;s=UI/origin/CncCommandTable/ProgramSettings/FV") + 1).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetRunoutTolPos()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.RunoutTolHalt");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetRunoutTolNeg()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.RunoutTolWarn");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetLengthTolPos()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.LengthTolLong");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetLengthTolNeg()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.LengthTolShort");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDiamterTolPos()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.DiameterTolPos");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDiamterTolNeg()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode("ns=4;s=UI/normalized/new/TPToolToleranceTable.DiameterTolNeg");
            return GetFormatInt2FloatString(((Double[])dataValue.Value)[0].ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetUserName()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/origin/User/User.CurrentUser/UserName").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetSpindleCount()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/Parameter/General/AxisConfiguration/WS.Number"));
            return dataValue?.Value?.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftToolChangeTime()
    {
        try
        {
            DataValue dataValueTime = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ToolChangeTime"));
            string strSeconds = ((((Double[])dataValueTime.Value)[0]) / 1000).ToString();
            return GetTimeBySeconds(strSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftRunCount()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/Shifts.RunCount"));
            return ((((Double[])dataValue.Value)[0])).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftTotalDrillOrRout()
    {
        try
        {
            DataValue dataValue = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/normalized/new/Shifts.RoutPath"));
            double dblValue = ((Double[])dataValue.Value)[0];

            if (dblValue < 0)
            {
                dataValue = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/normalized/new/Shifts.HitCount"));
                return ((Double[])dataValue.Value)[0].ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCncComm()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/origin/COMMInfos/CommStringToCnc").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCncError()
    {
        try
        {
            string[] aryCncError = _opcUaClient.ReadNode<string[]>("ns=4;s=UI/origin/DDETable/CncErrorTable/CncError");
            var cncErrors = new CncErrors(aryCncError);
            if (_oldCncErrors == null)
            {
                _oldCncErrors = cncErrors;
                return _oldCncErrors?.ErrorItems.FirstOrDefault() ?? string.Empty;
            }
            else
            {
                var diffItem = _oldCncErrors.PickDiffOneFromOther(cncErrors);
                _oldCncErrors = cncErrors;
                return diffItem ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetStep()
    {
        try
        {
            return _opcUaClient.ReadNode<UInt32>("ns=4;s=UI/normalized/new/Step").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetBlock()
    {
        try
        {
            return _opcUaClient.ReadNode<UInt32>("ns=4;s=UI/normalized/new/BlockNo").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCncRunProgress()
    {
        try
        {
            double dblPgs = _opcUaClient.ReadNode<double>("ns=4;s=UI/origin/WorkGroupSettings/WorkGroupInterface/TimeWidget/ProgressInPercent");
            return Math.Round(dblPgs * 100, 0).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetSpindleWorkTotalMinutes(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.SpindlesRuntime/Shifts.SpindleRunTime{SpindleId}"));
            if (dataValueUtilisation == null || dataValueUtilisation.Value == null)
            {
                dataValueUtilisation = _opcUaClient.ReadNode($"ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.Spindles/Shifts.SpindleRunTime{SpindleId}");
                if (dataValueUtilisation == null || dataValueUtilisation.Value == null)
                {
                    string version = _opcUaClient.ReadNode<String>("ns=4;s=UI/origin/Parameter/Description/CncVersion").Replace(".", "");
                    _logger.LogInformation($"读取SpindleRunTime{SpindleId} 失败，CNC版本{version}");
                    return string.Empty;
                }
            }

            TimeSpan ts = TimeSpan.FromMilliseconds(((Double[])dataValueUtilisation.Value)[0]);

            return ts.TotalMinutes.ToString("#0");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetSpindleWorkTimes(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.SpindlesRuntime/Shifts.SpindleRunTime{SpindleId}"));
            if (dataValueUtilisation == null || dataValueUtilisation.Value == null)
            {
                dataValueUtilisation = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.SpindlesRuntime/Shifts.SpindleRunTime{SpindleId}"));
                if (dataValueUtilisation == null || dataValueUtilisation.Value == null)
                {
                    return string.Empty;
                }
            }
            double dblValue = Math.Round(((Double[])dataValueUtilisation.Value)[0] / 1000, 0);

            return GetTimeBySeconds(int.Parse(dblValue.ToString()).ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTRunout(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TdmCalculatedToolRunout"));
            double dblValue = ((Double[])dataValueUtilisation.Value)[int.Parse(SpindleId) - 1];

            return GetFormatInt2FloatString(dblValue.ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTMeasureLen(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId($"ns=4;s=UI/normalized/new/TlmMeasuredLength"));
            double dblValue = ((Double[])dataValueUtilisation.Value)[int.Parse(SpindleId) - 1];

            return GetFormatInt2FloatString(dblValue.ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTMeasureDia(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TdmCalculatedToolDiameter"));
            double dblValue = ((Double[])dataValueUtilisation.Value)[int.Parse(SpindleId) - 1];

            return GetFormatInt2FloatString(dblValue.ToString(), 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetSpindleEnable(string SpindleId)
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/WORKSTATION.SELECTION"));
            bool bIsSpindleEnable = ((Boolean[])dataValueUtilisation.Value)[int.Parse(SpindleId) - 1];

            if (bIsSpindleEnable)
                return "1";

            return "0";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    #region GetOpcUaNodeData

    private string GetPreDuty()
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Utilisation"));
            return Math.Round(((Double[])dataValueUtilisation.Value)[1], 0).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetOPID()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/normalized/new/OPID");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDrillZ()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double>("ns=4;s=UI/normalized/new/Z").ToString();
            return GetFormatInt2FloatString(strValue, 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDrillK()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double>("ns=4;s=UI/normalized/new/K").ToString();
            return GetFormatInt2FloatString(strValue, 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDrillKi()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double[]>("ns=4;s=UI/origin/DepthControl.dnConfigurator/DepthControl.dnConfiguration/DepthControl.dnDepthControlOverview/DepthControl.dnDepthControlChapter/DepthControl.dnMonitor/DCP.Table/DepthControl.dnPlaneInfos/DCP.DepthReached")[0].ToString();
            return GetFormatInt2FloatString(strValue, 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDrillQ()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double>("ns=4;s=UI/normalized/new/QUIK").ToString();
            return GetFormatInt2FloatString(strValue, 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDrillH()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double>("ns=4;s=UI/normalized/new/H").ToString();
            return GetFormatInt2FloatString(strValue, 1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCurToolId()
    {
        try
        {
            return _opcUaClient.ReadNode<Int32>("ns=4;s=UI/origin/DynamicInfo/CurrentToolNumber").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetRunStartTime()
    {
        try
        {
            DateTime dtRunStartTime = _opcUaClient.ReadNode<DateTime>("ns=4;s=UI/normalized/new/AutoData.RunStartTime");

            if (dtRunStartTime.ToString() != _nullTimeValue)
            {
                return dtRunStartTime.AddHours(_timeDiffer).ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetRunEndTime()
    {
        try
        {
            DateTime dtRunEndTime = _opcUaClient.ReadNode<DateTime>("ns=4;s=UI/normalized/new/AutoData.RunEndTime");

            if (dtRunEndTime.ToString() != _nullTimeValue && CncStatus.EC == "0536936569")
            {
                return dtRunEndTime.AddHours(_timeDiffer).ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCncStatusText()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/origin/DDETable/CncStatus");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetBlockText()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/origin/RosiInfo/BlockText");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetPgmFilePath()
    {
        try
        {
            return _opcUaClient.ReadNode<string>("ns=4;s=UI/origin/FilesAndPaths/ProgramFiles/ProgramGroup/Program");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDiaFilePath()
    {
        try
        {
            return _opcUaClient.ReadNode<String>("ns=4;s=UI/normalized/new/DiameterTable").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetAtpFilePath()
    {
        try
        {
            return _opcUaClient.ReadNode<String>("ns=4;s=UI/normalized/new/ATP").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCurDrillOrRout()
    {
        try
        {
            return _opcUaClient.ReadNode<UInt32>("ns=4;s=UI/normalized/new/Hole").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTotalDrillOrRout()
    {
        try
        {
            return _opcUaClient.ReadNode<Int32>("ns=4;s=UI/normalized/new/TotalHitcountRequestedHits").ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetDuty()
    {
        try
        {
            DataValue dataValueUtilisation = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Utilisation"));
            return Math.Round(((Double[])dataValueUtilisation.Value)[0], 0).ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftOnlineTime()
    {
        try
        {
            DataValue dataValueTime = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ActiveTime"));
            string strSeconds = ((((Double[])dataValueTime.Value)[0]) / 1000).ToString();
            return GetTimeBySeconds(strSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftWorkingTime()
    {
        try
        {
            DataValue dataValueTime = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WorkingTime"));
            string strSeconds = ((((Double[])dataValueTime.Value)[0]) / 1000).ToString();
            return GetTimeBySeconds(strSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftWaitingTime()
    {
        try
        {
            DataValue dataValueTime = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.WaitingTime"));
            string strSeconds = ((((Double[])dataValueTime.Value)[0]) / 1000).ToString();
            return GetTimeBySeconds(strSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetShiftErrorTime()
    {
        try
        {
            DataValue dataValueTime = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/origin/ProductionTimes/Shifts/Shifts.TimesAndData/Shifts.Times/Shifts.ErrorTime"));
            string strSeconds = ((((Double[])dataValueTime.Value)[0]) / 1000).ToString();
            return GetTimeBySeconds(strSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetXYPosition()
    {
        try
        {
            DataValue dvXPos = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/XPositions"));
            string strDrlXPos = (((Double[])dvXPos.Value)[0] / 1000).ToString("#0.000");
            DataValue dvYPos = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/YPositions"));
            string strDrlYPos = (((Double[])dvYPos.Value)[0] / 1000).ToString("#0.000");
            return "X" + strDrlXPos + "Y" + strDrlYPos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetToolD(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamDiameter"));
                string strToolParm = (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
                return GetFormatInt2FloatString(strToolParm, 1000);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolD Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolS(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamSpindleSpeed"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex] * 0.06, 3)).ToString("F1");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolS Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolF(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamFeedRate"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex] * 0.00006, 3)).ToString("F3");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolF Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolR(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamRetractRate"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex] * 0.00006, 3)).ToString("F3");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolR Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolN(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamToolLife"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolN Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolB(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/TParamCurrentToolLife"));
                return (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolB Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolZ(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                var dataValueToolParm = _opcUaClient.ReadNode<double[]>("ns=4;s=UI/normalized/new/TParamZPlane");
                if (dataValueToolParm.Length > nToolIndex)
                {
                    return Math.Round(dataValueToolParm[nToolIndex] / 1000, 3).ToString("F3");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolB Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolA(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                var dataValueToolParm = _opcUaClient.ReadNode<double[]>("ns=4;s=UI/normalized/new/TParamDwellTime");
                if (dataValueToolParm.Length > nToolIndex)
                {
                    return dataValueToolParm[nToolIndex].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolB Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolChipl(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                var dataValueToolParm = _opcUaClient.ReadNode<int[]>("ns=4;s=UI/normalized/new/TParamChipload");
                if (dataValueToolParm.Length > nToolIndex)
                {
                    return dataValueToolParm[nToolIndex].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolB Exception - " + ex.Message);
        }
        return string.Empty;
    }

    /// <summary>
    /// 可用孔数
    /// </summary>
    /// <param name="ToolId"></param>
    /// <returns></returns>
    private string GetHitEnalbe(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/HitcountAvailableHits"));
                string strToolParm = (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
                return strToolParm;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolD Exception - " + ex.Message);
        }
        return string.Empty;
    }

    /// <summary>
    /// 可用刀具数
    /// </summary>
    /// <param name="ToolId"></param>
    /// <returns></returns>
    private string GetToolEnalbe(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/HitcountAvailableTools"));
                string strToolParm = (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
                return strToolParm;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolD Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolReqNum(string ToolId)
    {
        try
        {
            int nToolIndex = int.Parse(GetToolIndexById(ToolId));
            if (nToolIndex >= 0)
            {
                DataValue dataValueToolParm = _opcUaClient.ReadNode(new NodeId("ns=4;s=UI/normalized/new/HitcountRequestedTools"));
                string strToolParm = (Math.Round(((Double[])dataValueToolParm.Value)[nToolIndex], 0)).ToString();
                return strToolParm;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolD Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private int GetCommStatus()
    {
        try
        {
            return _opcUaClient.ReadNode<Int32>("ns=4;s=UI/origin/RPCTable/RPCDataTable/RPC_DATA_COMMSTATUS");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return -1;
    }

    private int GetLoadStatus()
    {
        try
        {
            return _opcUaClient.ReadNode<Int32>("ns=4;s=UI/origin/RPCTable/RPCDataTable/RPC_DATA_LOADSTATUS");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return -1;
    }

    private string GetAutoListRunNumber()
    {
        string AutoListRunNumber = "0";
        try
        {
            var rnlist = _opcUaClient.ReadNode<int[]>("ns=4;s=UI/origin/AutoList/AutoList.List/AutoList.RunNumber");
            if (rnlist != null && rnlist.Length > 0)
            {
                AutoListRunNumber = rnlist.Last().ToStringEx();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $@"GetAutoListRunNumber - {ex.Message}");
        }
        return AutoListRunNumber;
    }

    /// <summary>
    /// 开始执行程序
    /// </summary>
    /// <returns></returns>
    private string CncStart()
    {
        string msg = "";
        try
        {
            if (_opcUaClient.Connected == true)
            {
                object[] objArray = _opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_START");
                msg = objArray[0].ToStringEx();
            }
            else
            {
                msg = $@"CncStart OPCUA Connect Error";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $@"CncStart - {ex.Message}");
        }
        return msg;
    }

    /// <summary>
    /// 停止执行程序
    /// </summary>
    /// <returns></returns>
    private string CncStop()
    {
        string msg = "";
        try
        {
            if (_opcUaClient.Connected == true)
            {
                object[] objArray = _opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_STOP");
                msg = objArray[0].ToStringEx();
            }
            else
            {
                msg = $@"CncStart OPCUA Connect Error";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $@"CncStart - {ex.Message}");
        }
        return msg;
    }

    /// <summary>
    /// 主轴偏摆
    /// </summary>
    /// <returns></returns>
    private string SpindleYaw()
    {
        try
        {
            string strValue = _opcUaClient.ReadNode<double[]>("ns=4;s=UI/normalized/new/TPToolToleranceTable.RunoutTolWarn")[0].ToString();
            strValue = GetFormatInt2FloatString(strValue, 1000);
            return strValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    #endregion GetOpcUaNodeData

    private void ParseCncStatusText(string strCncStatusText)
    {
        try
        {
            if (!string.IsNullOrEmpty(strCncStatusText))
            {
                string[] aryStrStatus = strCncStatusText.Split(",");
                foreach (var item in aryStrStatus)
                {
                    if (item.StartsWith("AR")) { string AR = item.Replace("AR", ""); if (CncStatus.AR != AR) CncStatus.AR = AR; }
                    if (item.StartsWith("AP")) { string AP = item.Replace("AP", ""); if (CncStatus.AP != AP) CncStatus.AP = AP; }
                    if (item.StartsWith("ZS")) { string ZS = item.Replace("ZS", ""); if (CncStatus.ZS != ZS) CncStatus.ZS = ZS; }
                    if (item.StartsWith("MO")) { string MO = item.Replace("MO", ""); if (CncStatus.MO != MO) CncStatus.MO = MO; }
                    if (item.StartsWith("EC")) { string EC = item.Replace("EC", ""); if (CncStatus.EC != EC) CncStatus.EC = EC; }
                    if (item.StartsWith("FN")) { string FN = item.Replace("FN", ""); if (CncStatus.FN != FN) CncStatus.FN = FN; }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParseCncStatusText Exception - " + ex.Message);
        }
    }

    #region LoadFile

    private async Task<bool> IsWorkStatus()
    {
        try
        {
            int nTimes = 0;
            do
            {
                if (!_opcUaClient.ReadNode<string>("ns=4;s=UI/origin/DDETable/CncStatus").Contains("MOWORK"))
                {
                    return false;
                }
                await Task.Delay(1000);
                nTimes++;
            }
            while (nTimes <= 5);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return true;
    }

    public async Task<bool> CncLoadFile(LoadFileType loadFileType, string strFilePath, CancellationToken cancellationToken = default)
    {
        RetMsg retMsg = new RetMsg() { result = false };
        string strRetMsg = string.Empty;
        try
        {
            _logger.LogInformation($"CncLoadFile - {loadFileType} - {strFilePath}");

            if (!_opcUaClient.Connected)
            {
                strRetMsg = "NG_CNC is not connected.";
            }
            else if (!File.Exists(strFilePath))
            {
                strRetMsg = "NG_File is not exist.";
            }
            else if (!await IsWorkStatus())
            {
                string cmd = "CM@@@";
                string fileType = "PROGRAM";
                switch (loadFileType)
                {
                    case LoadFileType.PROGRAM:
                        fileType = "PROGRAM";
                        cmd = "CM@@@";
                        break;

                    case LoadFileType.DIA:
                        fileType = "DIAMETERTABLE";
                        cmd = "CD@@@";
                        break;

                    case LoadFileType.ATP:
                        fileType = "ATP";
                        cmd = "CA@@@";
                        break;
                }

                retMsg = await LoadCNCCommand(cmd);
                if (retMsg.result)
                {
                    retMsg = LoadFileWithType(strFilePath, fileType);
                    if (retMsg.result)
                    {
                        int nTryCount = 0;
                        await Task.Delay(4000);
                        do
                        {
                            await Task.Delay(1000);
                            retMsg = IsLoadSuccess(loadFileType, strFilePath);
                            if (retMsg.result)
                            {
                                strRetMsg = "OK";
                                break;
                            }
                        }
                        while (nTryCount++ <= 5);
                    }
                }
                else
                {
                    strRetMsg = "NG_" + retMsg.msg;
                }
            }
            else
            {
                strRetMsg = "NG_" + "CNC is working";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            strRetMsg = "NG_" + ex.Message;
        }

        await Task.Delay(200);
        SetLoadFileResult(loadFileType, strFilePath, strRetMsg);
        _logger.LogInformation($"CncLoadFile - {retMsg.result} - {retMsg.msg}");
        return retMsg.result;
    }

    private void SetLoadFileResult(LoadFileType loadFileType, string strFilePath, string strResult)
    {
        try
        {
            _logger.LogInformation($"SetLoadFileResult - {strFilePath} - {strResult}");

            switch (loadFileType)
            {
                case LoadFileType.PROGRAM:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadPgmFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;

                case LoadFileType.DIA:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadDiaFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;

                case LoadFileType.ATP:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadAtpFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<RetMsg> LoadCNCCommand(string cmd)
    {
        RetMsg retMsg = new RetMsg() { result = false };
        try
        {
            _logger.LogInformation($"LoadCNCCommand - {cmd}");

            if (_opcUaClient.Connected == true)
            {
                _logger.LogInformation($"LoadCNCCommand Status - {GetCommStatus().ToString()} - {GetLoadStatus().ToString()}");

                object[] objArray = _opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_CNCCOMMAND", new object[] { cmd });
                await Task.Delay(1000);

                if (objArray != null)
                {
                    string strMsg = "";
                    if (objArray.Length != 0)
                    {
                        strMsg = objArray[0].ToString();
                        if (strMsg == "success" || strMsg == "true")
                        {
                            await Task.Delay(1000);

                            retMsg.result = true; retMsg.msg = string.Format("LoadCNCCommand success." + cmd);

                            _logger.LogInformation($"LoadCNCCommand - {retMsg.msg}");
                            return retMsg;
                        }
                    }
                    retMsg.msg = "Load RPC_CNCCOMMAND Error - " + strMsg;
                }

                retMsg.msg = "Load RPC_CNCCOMMAND objArray is null." + cmd;
            }
            else
            {
                _logger.LogInformation($"LoadCNCCommand - _opcUaClient.Connected false");
                retMsg.msg = "Load RPC_CNCCOMMAND OPCUA Connect Error - " + cmd;
                await Task.Delay(200);
            }
        }
        catch (Exception ex)
        {
            retMsg.msg = "Load RPC_CNCCOMMAND ex - " + ex.Message + " - " + cmd;
            _logger.LogError(ex, ex.Message);
        }
        _logger.LogInformation($"LoadCNCCommand - {retMsg.msg}");
        return retMsg;
    }

    //不要直接调用
    private RetMsg LoadFileWithType(string fileName, string fileType)
    {
        RetMsg retMsg = new RetMsg() { result = false };
        try
        {
            _logger.LogInformation($"LoadFileWithType - {fileName} - {fileType}");

            if (_opcUaClient.Connected == true)
            {
                object[] objects = new object[] { fileName, fileType };
                object[] objArray = _opcUaClient.CallMethodByNodeId("ns=4;s=UI/Method/RPCTable/RPCCommandTable", "ns=4;s=UI/Method/RPCTable/RPCCommandTable/RPC_LOAD", objects);

                if (objArray != null)
                {
                    string str = "";
                    if (objArray.Length != 0)
                    {
                        str = objArray[0].ToString();
                        if (str == "success" || str == "true")
                        {
                            retMsg.result = true; retMsg.msg = "LoadFileWithType Success";
                            Thread.Sleep(1000);
                            _logger.LogInformation($"LoadFileWithType - {retMsg.msg}");
                            return retMsg;
                        }
                    }
                    retMsg.msg = "Load RPC_LOAD Error - " + str;
                }

                retMsg.msg = "Load RPC_LOAD objArray is null. ";
            }
            else
            {
                retMsg.msg = "Load RPC_LOAD OPCUA Connect Error - " + fileName;
            }
        }
        catch (Exception ex)
        {
            retMsg.msg = "Load RPC_LOAD ex - " + ex.Message + " - " + fileName;
            _logger.LogError(ex, ex.Message);
        }
        _logger.LogInformation($"LoadFileWithType - {retMsg.msg}");
        return retMsg;
    }

    private RetMsg IsLoadSuccess(LoadFileType loadFileType, string strFilePath)
    {
        RetMsg retMsg = new RetMsg() { result = false };
        try
        {
            _logger.LogInformation($"IsLoadSuccess - {loadFileType} - {strFilePath}");

            switch (loadFileType)
            {
                case LoadFileType.PROGRAM:
                    {
                        if (strFilePath.Equals(GetPgmFilePath(), StringComparison.OrdinalIgnoreCase))
                        {
                            retMsg.result = true;
                            _logger.LogInformation($"IsLoadSuccess - {strFilePath} 加载成功");
                            return retMsg;
                        }
                    }
                    break;

                case LoadFileType.DIA:
                    {
                        if (strFilePath.Equals(GetDiaFilePath(), StringComparison.OrdinalIgnoreCase))
                        {
                            retMsg.result = true;
                            _logger.LogInformation($"IsLoadSuccess - {strFilePath} 加载成功");
                            return retMsg;
                        }
                    }
                    break;

                case LoadFileType.ATP:
                    {
                        if (strFilePath.Equals(GetAtpFilePath(), StringComparison.OrdinalIgnoreCase))
                        {
                            retMsg.result = true;
                            _logger.LogInformation($"IsLoadSuccess - {strFilePath} 加载成功");
                            return retMsg;
                        }
                    }
                    break;
            }

            retMsg.msg = "IsLoadSuccess Failed, FileName = " + strFilePath;
        }
        catch (Exception ex)
        {
            retMsg.msg = "IsLoadSuccess Error, ex - " + ex.Message + " - " + strFilePath;
            _logger.LogError(ex, ex.Message);
        }
        _logger.LogInformation($"IsLoadSuccess - {retMsg.msg}");
        return retMsg;
    }

    #endregion LoadFile

    #region ParseData

    private string GetToolIdByIndex(string strToolIndex)
    {
        try
        {
            int nToolId = 0;
            if (int.TryParse(strToolIndex, out nToolId))
            {
                nToolId += 1;
                return nToolId.ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolIdByIndex Exception - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolIndexById(string strToolId)
    {
        try
        {
            int nToolIndex = 0;
            if (int.TryParse(strToolId, out nToolIndex))
            {
                nToolIndex -= 1;
                return nToolIndex.ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolIndexById Exception - " + ex.Message);
        }
        return string.Empty;
    }

    #endregion ParseData

    public BrokenToolData ParseBrokenToolData(BrokenToolData brokenToolData, string blockText, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"断刀的原始信息 ： {blockText}");
            brokenToolData.BrokenInfo = blockText;
            string pattern = @"(?<=\[.*?\]\s*)(.*?)(?=\s*\[.*?\]\s|$)";
            var brokenContents = Regex.Matches(blockText, pattern).Select(s => s.Groups[1].Value.Trim()).Distinct();
            List<BrokenToolData> brokenModels = new List<BrokenToolData>();
            if (brokenContents.Count() != 0)
            {
                foreach (var item in brokenContents)
                {
                    var brokenModel = ParseBrokenToolPattern1(item);
                    if (brokenModel != null)
                    {
                        brokenModels.Add(brokenModel);
                    }
                }
            }
            else
            {
                var brokenModel = ParseBrokenToolPattern2(blockText);
                if (brokenModel != null)
                {
                    brokenModels.Add(brokenModel);
                }
            }

            if (brokenModels.Count() > 0)
            {
                var brokenModel = brokenModels.FirstOrDefault();
                if (brokenModel != null)
                {
                    brokenToolData.BrkToolId = brokenModel.BrkToolId;
                    brokenToolData.BrkToolDia = brokenModel.BrkToolDia;
                    brokenToolData.BrkToolSpindle = brokenModel.BrkToolSpindle;
                }

                _logger.LogInformation($"ParseBrokenTool - T{brokenToolData.BrkToolId} D{brokenToolData.BrkToolDia} Z{brokenToolData.BrkToolSpindle}");
                return brokenToolData;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParseBrokenToolData 异常" + ex.Message);
        }
        return new BrokenToolData();
    }

    public Task<bool> CncSetXYOfProgramZero(double x, double y, CancellationToken cancellationToken = default) => throw new NotImplementedException();//TO DO

    private BrokenToolData? ParseBrokenToolPattern1(string brokenToolContent)
    {
        try
        {
            _logger.LogInformation($"解析的断刀Pat1 - {brokenToolContent}");

            if (string.IsNullOrEmpty(brokenToolContent))
            {
                return null;
            }
            var brokenArray = brokenToolContent.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (brokenArray.Length != 3)
            {
                return null;
            }
            string toolNum = brokenArray[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();
            string toolD = brokenArray[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();
            string toolS = string.Join(",", brokenArray[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1));

            return new BrokenToolData
            {
                BrkToolDia = toolD,
                BrkToolId = toolNum.Replace("T", ""),
                BrkToolSpindle = toolS,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return new BrokenToolData();
    }

    private BrokenToolData? ParseBrokenToolPattern2(string brokenToolContent)
    {
        try
        {
            _logger.LogInformation($"解析的断刀Pat2 - {brokenToolContent}");
            if (string.IsNullOrEmpty(brokenToolContent))
            {
                return null;
            }

            var brokenArray = brokenToolContent.Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (brokenArray.Length != 4)
            {
                return null;
            }
            string toolNum = brokenArray[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).First();
            string toolD = brokenArray[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();
            string toolS = string.Join(",", brokenArray[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1));
            return new BrokenToolData
            {
                BrkToolDia = toolD,
                BrkToolId = toolNum.Replace("T", ""),
                BrkToolSpindle = toolS,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return new BrokenToolData();
    }

    private string GetTimeBySeconds(string strSeconds)
    {
        string strTime = string.Empty;
        try
        {
            if (strSeconds.IndexOf(".") != -1) //如果有小数点，把小数点去掉
            {
                strSeconds = strSeconds.Substring(0, strSeconds.IndexOf("."));
            }

            TimeSpan ts = new TimeSpan(0, 0, int.Parse(strSeconds));
            return ts.ToString(@"hh\:mm\:ss");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTimeBySeconds Exception - " + ex.Message);
        }
        return strTime;
    }

    private string GetFormatInt2FloatString(string strOrg, int nDiv, string strFormat = "F3")
    {
        try
        {
            float fOrg = 0;

            if (float.TryParse(strOrg, out fOrg))
            {
                fOrg /= nDiv;
                return fOrg.ToString(strFormat);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetFormatInt2FloatString 函数异常 - " + ex.Message);
        }
        return string.Empty;
    }
}
