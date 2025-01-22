namespace envmonitor.Tests;

using envmonitor;

public class EnvMonitorTest
{
    [Fact]
    public void ProgressBarTestSuccess()
    {
        int paramValue = 5;
        int paramTotal = 10;
        int paramLength = 10;
        String paramLabel = "%";
        String expected = "5 % [#####-----] 10 %";
        String result = EnvMonitor.GenerateProgressBar(paramValue, paramTotal, paramLength, paramLabel);
        Assert.Equal(result, expected);
    }
}
