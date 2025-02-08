using System.Text;

namespace RpgGame.Tests.Helpers;

public static class TestHelper
{
    public static (string Input, string ExpectedOutput) LoadTestCase(string testName)
    {
        var inputPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestData", $"{testName}.in");
        var outputPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestData", $"{testName}.out");

        // 讀取所有行並使用環境換行符重新組合
        var inputLines = File.ReadAllLines(inputPath);
        var outputLines = File.ReadAllLines(outputPath);

        var input = string.Join(Environment.NewLine, inputLines) + Environment.NewLine;
        var output = string.Join(Environment.NewLine, outputLines) + Environment.NewLine;

        return (input, output);
    }
}
