using System.Globalization;

namespace VgCNCServer.Global;

public class GlobalConfig
{
    private readonly I18n _i18n;
    private readonly CookieStorage _cookieStorage;

    private bool _expandOnHover;

    public GlobalConfig(CookieStorage cookieStorage, I18n i18n)
    {
        _i18n = i18n;
        _i18n.SetCulture(CultureInfo.InstalledUICulture);//默认当前语言
        _cookieStorage = cookieStorage;
    }

    public static string ExpandOnHoverCookieKey => "GlobalConfig_ExpandOnHover";

    public static string LangCookieKey => "GlobalConfig_Lang";

    public EventHandler? ExpandOnHoverChanged { get; set; }

    public bool ExpandOnHover
    {
        get => _expandOnHover;
        set
        {
            _expandOnHover = value;
            ExpandOnHoverChanged?.Invoke(this, EventArgs.Empty);
            _cookieStorage.SetAsync(ExpandOnHoverCookieKey, value);
        }
    }

    public CultureInfo Culture
    {
        get => _i18n.Culture;
        set
        {
            _cookieStorage.SetAsync(LangCookieKey, value.Name);//zh-CN
            _i18n.SetCulture(value);
        }
    }

    public async Task InitFromStorage()
    {
        ExpandOnHover = Convert.ToBoolean(await _cookieStorage.GetAsync(ExpandOnHoverCookieKey));

        string lang = await _cookieStorage.GetAsync(LangCookieKey);
        if (!string.IsNullOrWhiteSpace(lang))
        {
            _i18n.SetCulture(new CultureInfo(lang));
        }
    }
}
