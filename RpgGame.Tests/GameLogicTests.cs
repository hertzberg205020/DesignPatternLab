using RpgGame.Models.GameLogic;
using RpgGame.Tests.Helpers;

namespace RpgGame.Tests;

public class GameLogicTests
{
    [Fact]
    public void BasicAttackTest()
    {
        var (input, expectedOutput) = TestHelper.LoadTestCase("only-basic-attack");

        // 確保輸入以換行符號結尾
        input = input.TrimEnd() + Environment.NewLine + Environment.NewLine;

        // 使用 StringWriter 來捕獲輸出
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // 使用 StringReader 來模擬輸入
        using var stringReader = new StringReader(input);
        Console.SetIn(stringReader);

        // 執行遊戲邏輯
        var game = new Game();
        game.Start();

        // 比較輸出
        var actualOutput = stringWriter.ToString();
        Assert.Equal(expectedOutput.TrimEnd(), actualOutput.TrimEnd());
    }
}
