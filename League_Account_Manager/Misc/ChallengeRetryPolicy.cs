namespace League_Account_Manager.Misc;

/// <summary>Bounds replacement challenges within one authentication attempt.</summary>
internal sealed class ChallengeRetryPolicy
{
    internal const string ExhaustedMessage =
        "Riot rejected verification twice. Login stopped to avoid repeated CAPTCHA prompts. " +
        "Try signing in through Riot Client.";
    private int rejections;

    // Permit one replacement challenge; a second rejection ends this attempt.
    internal bool TryRetry() => ++rejections < 2;
}
