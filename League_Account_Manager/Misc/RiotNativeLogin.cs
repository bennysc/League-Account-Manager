using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace League_Account_Manager.Misc;

internal static class RiotNativeLogin
{
    internal static async Task SignOutRememberedSessionAsync(CancellationToken cancellationToken = default)
    {
        var elapsed = Stopwatch.StartNew();
        var requested = false;
        while (elapsed.Elapsed < TimeSpan.FromSeconds(30))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var response = await Lcu.Connector("riot", "get", "/rso-auth/v1/session", "",
                cancellationToken) as HttpResponseMessage;
            if (response?.IsSuccessStatusCode == true)
            {
                using var session = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
                if (session.RootElement.TryGetProperty("type", out var type))
                {
                    if (type.GetString() != "authenticated") return;
                    if (!requested)
                    {
                        using var logout = await Lcu.Connector("riot", "delete", "/rso-auth/v1/session", "",
                            cancellationToken) as HttpResponseMessage;
                        if (logout == null || !logout.IsSuccessStatusCode)
                            throw new InvalidOperationException("Riot could not sign out the remembered account.");
                        requested = true;
                    }
                }
            }
            await Task.Delay(250, cancellationToken);
        }
        throw new TimeoutException("Riot did not finish signing out the remembered account.");
    }
}
