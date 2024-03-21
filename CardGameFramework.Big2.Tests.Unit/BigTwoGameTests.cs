using CardGameFramework.Big2.Models;
using CardGameFramework.Big2.Tests.Unit.Mock;
using CardGameFramework.Infra.Models;
using Xunit.Abstractions;

namespace CardGameFramework.Big2.Tests.Unit;

public class BigTwoGameTests: IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private BigTwoGame? _game; 
    private readonly string _originalNewLine;
    private readonly TextWriter _originalOut;
    private readonly TextReader _originalIn;
    private static string ProjectRoot => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "CardGameFramework.Big2.Tests.Unit"));

    public BigTwoGameTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _originalNewLine = Console.Out.NewLine;
        // 在測試初始化時保存原始的 Console 狀態
        _originalOut = Console.Out;
        _originalIn = Console.In;
    }

    public void Dispose()
    {
        Console.Out.NewLine = _originalNewLine;
        // 測試完成後恢復原始的 Console 狀態
        Console.SetOut(_originalOut);
        Console.SetIn(_originalIn);
    }
    
    private void InitializeGame()
    {
        var players = new List<CardPlayer<PokerCard>>
        {
            new BigTwoHumanPlayer(new HandCard()),
            new BigTwoHumanPlayer(new HandCard()),
            new BigTwoHumanPlayer(new HandCard()),
            new BigTwoHumanPlayer(new HandCard()),
        };
        var deck = MockDeck.CreateDeck();
        _game = new BigTwoGame(deck, players);
    }
    
    private void SetIn(string filePath)
    {
        var input = File.ReadAllText(filePath);
        var stringReader = new StringReader(input);
        // 把檔案內容導入 Console.In
        Console.SetIn(stringReader);
    }
    
    private StringWriter SetOut()
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        return stringWriter;
    }
    
    private string GetTestCaseOut(string filePath) => File.ReadAllText(filePath);

    private static string GetActual(StringWriter writer) => writer.GetStringBuilder().ToString();
    
    [Fact]
    public void always_play_first_card()
    {
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/always-play-first-card.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/always-play-first-card.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void FullHouse()
    {
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/fullhouse.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/fullhouse.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void illegal_actions()
    {
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/illegal-actions.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/illegal-actions.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void normal_no_error_play1()
    {
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/normal-no-error-play1.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/normal-no-error-play1.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void normal_no_error_play2()
    {
        
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/normal-no-error-play2.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/normal-no-error-play2.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Straight()
    {
        var inputFilePath = Path.Combine(ProjectRoot, "TestCase/straight.in");
        var outputFilePath = Path.Combine(ProjectRoot, "TestCase/straight.out");
        
        SetIn(inputFilePath);
        var writer = SetOut();
        // Console.Out.NewLine = "\n";
        InitializeGame();
        _game?.Run();
        var actual = GetActual(writer);
        var expected = GetTestCaseOut(outputFilePath);
        Assert.Equal(expected, actual);
    }
}