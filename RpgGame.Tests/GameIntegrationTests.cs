using RpgGame.Tests.Helpers;

namespace RpgGame.Tests;

public class GameIntegrationTests
{
    private const string TEST_DATA_PATH = @"TestData";

    private static string NormalizeLineEndings(string text)
    {
        // 將所有可能的換行符號統一轉換為 Environment.NewLine
        return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
    }

    [Theory]
    [InlineData("only-basic-attack")]
    [InlineData("waterball-and-fireball-1v2")]
    [InlineData("cheerup")]
    [InlineData("curse")]
    [InlineData("one-punch")]
    [InlineData("petrochemical")]
    [InlineData("poison")]
    [InlineData("self-explosion")]
    [InlineData("self-healing")]
    [InlineData("summon")]
    public void Battle_WithTestData_ShouldMatchExpectedOutput(string testCase)
    {
        // Arrange
        var inputPath = Path.Combine(TEST_DATA_PATH, $"{testCase}.in");
        var expectedOutputPath = Path.Combine(TEST_DATA_PATH, $"{testCase}.out");

        // 讀取輸入檔案
        var inputLines = File.ReadAllLines(inputPath);
        var expectedOutput = File.ReadAllText(expectedOutputPath);

        var gameIO = new TestGameIO(inputLines);
        var game = new Game(gameIO);

        // Act
        game.Start();

        // Assert
        var actualOutput = string.Join(Environment.NewLine, gameIO.Outputs);

        expectedOutput = NormalizeLineEndings(expectedOutput);
        actualOutput = NormalizeLineEndings(actualOutput);

        Assert.Equal(expectedOutput.Trim(), actualOutput.Trim());
    }
}
