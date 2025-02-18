using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models;

public class GameTests
{
    private readonly TestGameIO _gameIO;
    private readonly Game _game;

    public GameTests()
    {
        _gameIO = new TestGameIO(Array.Empty<string>());
        _game = new Game(_gameIO);
    }

    [Fact]
    public void AddRole_ShouldAddRoleToCorrectTroop()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        const string troopId = "1";
        role.EnterState(new NormalState(_gameIO));

        // Act
        _game.AddRole(troopId, role);

        // Assert
        var allies = _game.GetAllies(role);
        Assert.Equal(troopId, role.Troop);
        Assert.Empty(allies);
    }

    [Fact]
    public void GetEnemies_ShouldReturnCorrectEnemies()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        hero.EnterState(new NormalState(_gameIO));

        var enemy = new Role(_gameIO)
        {
            Name = "Enemy",
            IsHero = false,
            HealthPoint = 80,
            MagicPoint = 40,
            Strength = 25
        };
        enemy.EnterState(new NormalState(_gameIO));

        _game.AddRole("1", hero);
        _game.AddRole("2", enemy);

        // Act
        var heroEnemies = _game.GetEnemies(hero);
        var enemyEnemies = _game.GetEnemies(enemy);

        // Assert
        Assert.Single(heroEnemies);
        Assert.Single(enemyEnemies);
        Assert.Equal(enemy, heroEnemies[0]);
        Assert.Equal(hero, enemyEnemies[0]);
    }

    [Fact]
    public void AddRole_WhenRoleIsHero_ShouldSetHeroProperty()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };

        // Act
        _game.AddRole("1", hero);

        // Assert
        Assert.True(hero.IsHero);

        // 通過反射獲取 _hero 私有欄位的值
        var heroField = typeof(Game).GetField(
            "_hero",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        Assert.NotNull(heroField);

        var gameHero = heroField.GetValue(_game) as Role;

        Assert.Equal(hero, gameHero);
    }

    [Fact]
    public void GetAllies_WhenRoleNotInTroop_ShouldThrowArgumentException()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        role.EnterState(new NormalState(_gameIO));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _game.GetAllies(role));
        Assert.Equal("Role is not in any troop", exception.Message);
    }

    [Theory]
    [InlineData("水球", typeof(WaterBall))]
    [InlineData("火球", typeof(FireBall))]
    [InlineData("自我治療", typeof(SelfHealing))]
    [InlineData("石化", typeof(Petrochemical))]
    [InlineData("下毒", typeof(Poison))]
    [InlineData("召喚", typeof(Summon))]
    [InlineData("自爆", typeof(SelfExplosion))]
    [InlineData("鼓舞", typeof(CheerUp))]
    [InlineData("詛咒", typeof(Curse))]
    [InlineData("一拳攻擊", typeof(OnePunch))]
    public void CreateSkill_ShouldReturnCorrectSkillType(string skillName, Type expectedType)
    {
        // Arrange & Act
        var createSkillMethod = typeof(Game).GetMethod(
            "CreateSkill",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        Assert.NotNull(createSkillMethod);

        var skill = createSkillMethod.Invoke(_game, new object[] { skillName }) as Skill;

        // Assert
        Assert.NotNull(skill);
        Assert.IsType(expectedType, skill);
    }

    [Fact]
    public void CreateSkill_WithInvalidName_ShouldReturnNull()
    {
        // Arrange & Act
        var createSkillMethod = typeof(Game).GetMethod(
            "CreateSkill",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        Assert.NotNull(createSkillMethod);

        var skill = createSkillMethod.Invoke(_game, new object[] { "不存在的技能" });

        // Assert
        Assert.Null(skill);
    }

    [Fact]
    public void CreateRole_ShouldCreateRoleWithCorrectAttributes()
    {
        // Arrange
        var roleInfo = new Role(_gameIO)
        {
            Name = "TestHero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        var skills = new List<Skill> { new WaterBall(_gameIO), new FireBall(_gameIO) };

        // Act
        var createRoleMethod = typeof(Game).GetMethod(
            "CreateRole",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        var role = createRoleMethod?.Invoke(_game, new object[] { roleInfo, skills }) as Role;

        // Assert
        Assert.NotNull(role);
        Assert.Equal(roleInfo.Name, role.Name);
        Assert.Equal(roleInfo.IsHero, role.IsHero);
        Assert.Equal(roleInfo.HealthPoint, role.HealthPoint);
        Assert.Equal(roleInfo.MagicPoint, role.MagicPoint);
        Assert.Equal(roleInfo.Strength, role.Strength);
        Assert.Equal(2, role.Skills.Count);
        Assert.IsType<WaterBall>(role.Skills[0]);
        Assert.IsType<FireBall>(role.Skills[1]);
        Assert.NotNull(role.State);
        Assert.IsType<NormalState>(role.State);
        Assert.IsType<HumanDecisionMaker>(role.DecisionMaker);
    }

    [Theory]
    [InlineData(true, typeof(HumanDecisionMaker))]
    [InlineData(false, typeof(DefaultAiDecisionMaker))]
    public void CreateRole_ShouldSetCorrectDecisionMaker(
        bool isHero,
        Type expectedDecisionMakerType
    )
    {
        // Arrange
        var roleInfo = new Role(_gameIO)
        {
            Name = "TestRole",
            IsHero = isHero,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        var skills = new List<Skill>();

        // Act
        var createRoleMethod = typeof(Game).GetMethod(
            "CreateRole",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        var role = createRoleMethod?.Invoke(_game, new object[] { roleInfo, skills }) as Role;

        // Assert
        Assert.NotNull(role);
        Assert.NotNull(role.DecisionMaker);
        Assert.IsType(expectedDecisionMakerType, role.DecisionMaker);
    }

    [Fact]
    public void InitializeTroop_ShouldCreateTroopsCorrectly()
    {
        // Arrange
        var input = new[]
        {
            "#軍隊-1-開始",
            "英雄 300 500 100 水球 火球",
            "#軍隊-1-結束",
            "#軍隊-2-開始",
            "怪物 200 200 50",
            "#軍隊-2-結束",
            "#初始化結束"
        };

        var game = new Game(new TestGameIO(input));

        // Act
        var initializeTroopMethod = typeof(Game).GetMethod(
            "InitializeTroops",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        initializeTroopMethod?.Invoke(game, Array.Empty<object>());

        // Assert
        var troopsField = typeof(Game).GetField(
            "_troops",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        var troops = troopsField?.GetValue(game) as Dictionary<string, List<Role>>;

        Assert.NotNull(troops);
        Assert.Equal(2, troops.Count);
        Assert.True(troops.ContainsKey("1"));
        Assert.True(troops.ContainsKey("2"));

        var troop1 = troops["1"];
        var troop2 = troops["2"];

        Assert.Single(troop1);
        Assert.Single(troop2);

        var hero = troop1[0];
        var monster = troop2[0];

        Assert.Equal("英雄", hero.Name);
        Assert.Equal(300, hero.HealthPoint);
        Assert.Equal(500, hero.MagicPoint);
        Assert.Equal(100, hero.Strength);
        Assert.Equal(2, hero.Skills.Count);
        Assert.IsType<WaterBall>(hero.Skills[0]);
        Assert.IsType<FireBall>(hero.Skills[1]);

        Assert.Equal("怪物", monster.Name);
        Assert.Equal(200, monster.HealthPoint);
        Assert.Equal(200, monster.MagicPoint);
        Assert.Equal(50, monster.Strength);
        Assert.Empty(monster.Skills);
    }

    [Fact]
    public void CheckBattleEnd_ShouldReturnTrueWhenHeroDead()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 0, // 已死亡
            MagicPoint = 50,
            Strength = 30
        };
        _game.AddRole("1", hero);

        // Act
        var checkBattleEndMethod = typeof(Game).GetMethod(
            "CheckBattleEnd",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        Assert.NotNull(checkBattleEndMethod);

        var result = (bool)checkBattleEndMethod.Invoke(_game, Array.Empty<object>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckBattleEnd_ShouldReturnTrueWhenTroopAnnihilated()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };

        hero.EnterState(new NormalState(_gameIO));

        var enemy = new Role(_gameIO)
        {
            Name = "Enemy",
            IsHero = false,
            HealthPoint = 0, // 已死亡
            MagicPoint = 40,
            Strength = 25
        };

        enemy.EnterState(new NormalState(_gameIO));

        _game.AddRole("1", hero);
        _game.AddRole("2", enemy);

        // Act
        var checkBattleEndMethod = typeof(Game).GetMethod(
            "CheckBattleEnd",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        Assert.NotNull(checkBattleEndMethod);

        var result = (bool)checkBattleEndMethod.Invoke(_game, Array.Empty<object>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExecuteRoleTurn_ShouldExecuteRoleActionCorrectly()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };

        hero.EnterState(new NormalState(_gameIO));

        var enemy = new Role(_gameIO)
        {
            Name = "Enemy",
            IsHero = false,
            HealthPoint = 80,
            MagicPoint = 40,
            Strength = 25
        };

        enemy.EnterState(new NormalState(_gameIO));

        _game.AddRole("1", hero);
        _game.AddRole("2", enemy);

        // 設置模擬輸入：選擇基本攻擊(0)和目標(0)
        _gameIO.Input("0");
        _gameIO.Input("0");
        hero.DecisionMaker = new HumanDecisionMaker(_gameIO);

        // 初始化角色狀態

        // Act
        var executeRoleTurnMethod = typeof(Game).GetMethod(
            "ExecuteRoleTurn",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        executeRoleTurnMethod.Invoke(_game, new object[] { hero });

        // Assert
        Assert.Equal(80 - hero.Strength, enemy.HealthPoint); // 驗證傷害計算
        var outputs = _gameIO.Outputs;
        Assert.Contains(outputs, output => output.Contains("輪到 [1]Hero")); // 驗證角色狀態輸出
        Assert.Contains(outputs, output => output.Contains("選擇行動")); // 驗證行動選擇提示
    }

    [Fact]
    public void ExecuteRound_ShouldSkipDeadRoles()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        hero.DecisionMaker = new HumanDecisionMaker(_gameIO);
        _game.AddRole("1", hero);
        hero.EnterState(new NormalState(_gameIO));

        var deadEnemy = new Role(_gameIO)
        {
            Name = "DeadEnemy",
            IsHero = false,
            HealthPoint = 0, // 已死亡
            MagicPoint = 40,
            Strength = 25
        };
        deadEnemy.DecisionMaker = new DefaultAiDecisionMaker();
        _game.AddRole("2", deadEnemy);
        deadEnemy.EnterState(new DeadState(_gameIO));

        var aliveEnemy = new Role(_gameIO)
        {
            Name = "AliveEnemy",
            IsHero = false,
            HealthPoint = 80,
            MagicPoint = 40,
            Strength = 25
        };
        aliveEnemy.DecisionMaker = new DefaultAiDecisionMaker();
        _game.AddRole("2", aliveEnemy);
        aliveEnemy.EnterState(new NormalState(_gameIO));

        // 設置模擬輸入
        // 基本攻擊和目標選擇
        _gameIO.Input("0");
        _gameIO.Input("0");

        // Act
        var executeRoundMethod = typeof(Game).GetMethod(
            "ExecuteRound",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        executeRoundMethod.Invoke(_game, Array.Empty<object>());

        // Assert
        IReadOnlyList<string> outputs = _gameIO.Outputs;

        Assert.Contains(outputs, output => output.Contains("輪到 [1]Hero")); // Check if any string contains "[1]Hero"
        Assert.Contains(outputs, output => output.Contains("輪到 [2]AliveEnemy")); // Check if any string contains "[2]AliveEnemy"
        Assert.DoesNotContain(outputs, output => output.Contains("輪到 [2]DeadEnemy")); // Check if no string contains "[2]DeadEnemy"
    }

    [Fact]
    public void ExecuteRound_WithPetrifiedRole_ShouldSkipAction()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30
        };
        hero.DecisionMaker = new HumanDecisionMaker(_gameIO);
        hero.EnterState(new NormalState(_gameIO));
        _game.AddRole("1", hero);

        var petrifiedEnemy = new Role(_gameIO)
        {
            Name = "PetrifiedEnemy",
            IsHero = false,
            HealthPoint = 80,
            MagicPoint = 40,
            Strength = 25
        };
        petrifiedEnemy.DecisionMaker = new DefaultAiDecisionMaker();
        petrifiedEnemy.EnterState(new PetrifiedState(_gameIO));
        _game.AddRole("2", petrifiedEnemy);

        // 設置模擬輸入
        // 基本攻擊和目標選擇
        _gameIO.Input("0");
        _gameIO.Input("0");

        // Act
        var executeRoundMethod = typeof(Game).GetMethod(
            "ExecuteRound",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        executeRoundMethod.Invoke(_game, Array.Empty<object>());

        // Assert
        var outputs = _gameIO.Outputs;
        Assert.Contains(
            outputs,
            output => output.Contains("[2]PetrifiedEnemy") && output.Contains("石化")
        ); // 確認石化狀態
        Assert.Equal(50, petrifiedEnemy.HealthPoint); // 確認石化狀態會受到攻擊傷害
    }

    [Fact]
    public void ExecuteRound_WithPoisonedRole_ShouldTakePoisonDamage()
    {
        // Arrange
        var hero = new Role(_gameIO)
        {
            Name = "Hero",
            IsHero = true,
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 0
        };
        hero.DecisionMaker = new HumanDecisionMaker(_gameIO);
        hero.EnterState(new NormalState(_gameIO));
        _game.AddRole("1", hero);

        var poisonedEnemy = new Role(_gameIO)
        {
            Name = "PoisonedEnemy",
            IsHero = false,
            HealthPoint = 80,
            MagicPoint = 40,
            Strength = 0
        };
        poisonedEnemy.DecisionMaker = new DefaultAiDecisionMaker();
        poisonedEnemy.EnterState(new PoisonedState(_gameIO));
        _game.AddRole("2", poisonedEnemy);

        // 設置模擬輸入
        // 基本攻擊和目標選擇
        _gameIO.Input("0");
        _gameIO.Input("0");

        // Act
        var executeRoundMethod = typeof(Game).GetMethod(
            "ExecuteRound",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        executeRoundMethod.Invoke(_game, Array.Empty<object>());

        // Assert
        Assert.Equal(50, poisonedEnemy.HealthPoint); // 80 - 30(中毒傷害)
        var outputs = _gameIO.Outputs;
        Assert.Contains(
            outputs,
            output => output.Contains("[2]PoisonedEnemy") && output.Contains("中毒")
        ); // 確認中毒狀態
    }
}
