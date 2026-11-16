using System.Net.Http;
using System.Text;
using League_Account_Manager.Misc;

namespace League_Account_Manager.Tests.Misc;

[TestClass]
public class LcuHeadersTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid version")]
    [DataRow("1.0\r\nInjected: true")]
    public void MissingOrInvalidVersionDoesNotPreventLocalRequests(string? version)
    {
        using var client = new HttpClient();
        Lcu.SetClientHeaders(client, "12345", Encoding.UTF8.GetBytes("riot:test"), version);
        Assert.IsTrue(client.DefaultRequestHeaders.UserAgent.Any(p => p.Product?.Name == "RiotClient"
            && p.Product.Version == null));
        Assert.IsFalse(client.DefaultRequestHeaders.Contains("Injected"));
        Assert.AreEqual("Basic", client.DefaultRequestHeaders.Authorization?.Scheme);
    }

    [TestMethod]
    public void ValidVersionIsPreserved()
    {
        using var client = new HttpClient();
        Lcu.SetClientHeaders(client, "12345", Encoding.UTF8.GetBytes("riot:test"), "143.0.5");
        Assert.IsTrue(client.DefaultRequestHeaders.UserAgent.Any(p => p.Product?.Name == "RiotClient"
            && p.Product.Version == "143.0.5"));
    }
}
