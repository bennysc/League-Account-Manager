using League_Account_Manager.Misc;

namespace League_Account_Manager.Tests.Misc;

[TestClass]
public class ChallengeRetryPolicyTests
{
    [TestMethod]
    public void RepeatedRejectionsStopAfterOneReplacement()
    {
        var policy = new ChallengeRetryPolicy();
        Assert.IsTrue(policy.TryRetry());
        for (var i = 0; i < 100; i++)
            Assert.IsFalse(policy.TryRetry(), "Rejected challenges must never restart the loop.");
    }

    [TestMethod]
    public void NewUserAttemptHasItsOwnRetryBudget()
    {
        var previous = new ChallengeRetryPolicy();
        previous.TryRetry();
        previous.TryRetry();
        Assert.IsTrue(new ChallengeRetryPolicy().TryRetry());
    }
}
