using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.OpenAPI;

namespace UnitTest.VgAutoDrill.OpenAPI;

public class HttpRequestInvokerTest
{
    [Fact]
    public async Task Test1()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<HttpRequestInvoker>();
        var provider = serviceCollection.BuildServiceProvider();
        var httpRequestInvoker = provider.GetRequiredService<HttpRequestInvoker>();

        var returnObj = await httpRequestInvoker.PostAsJsonAsync<string, object>("http://localhost/aa", "bb");
        Assert.Null(returnObj);
    }

    [Fact]
    public async Task Test2()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<HttpRequestInvoker>();
        var provider = serviceCollection.BuildServiceProvider();
        var httpRequestInvoker = provider.GetRequiredService<HttpRequestInvoker>();

        var returnObj = await httpRequestInvoker.PostAsJsonAsync<string, object>("http://localhost/aa", "bb", "token");
        Assert.Null(returnObj);
    }

    [Fact]
    public async Task Test3()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<HttpRequestInvoker>();
        var provider = serviceCollection.BuildServiceProvider();
        var httpRequestInvoker = provider.GetRequiredService<HttpRequestInvoker>();

        var returnObj = await httpRequestInvoker.PostAsJsonAsync<string, object>("http://localhost/aa", "bb", JsonSerializerOptions.Default);
        Assert.Null(returnObj);
    }

    [Fact]
    public async Task Test4()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<HttpRequestInvoker>();
        var provider = serviceCollection.BuildServiceProvider();
        var httpRequestInvoker = provider.GetRequiredService<HttpRequestInvoker>();

        var returnObj = await httpRequestInvoker.PostAsJsonAsync<string, object>("http://localhost/aa", "bb", JsonSerializerOptions.Default, "token");
        Assert.Null(returnObj);
    }
}
