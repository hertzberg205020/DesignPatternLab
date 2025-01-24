using RpgGame.Models.GameComponent;
using RpgGame.Models.GameComponent.DecisionStrategies;
using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkillHandler;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameLogic;

public class Game
{
    private readonly Dictionary<string, List<Role>> _troops = new();
    private Role? _hero;
    private bool _isEnd = false;

    public void Start()
    {
        InitializeTroop();
        ExecuteBattle();
        OnBattleEnd();
    }

    /// <summary>
    /// 讀取標準輸入，初始化軍隊
    /// </summary>
    private void InitializeTroop()
    {
        string? line;
        string currentTroop = string.Empty;

        while ((line = Console.ReadLine()) != null)
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Handle troop markers
            if (IsTroopMarker(line))
            {
                currentTroop = ProcessTroopMarker(line);
                continue;
            }

            // Process role if within a troop definition
            if (!string.IsNullOrEmpty(currentTroop))
            {
                ProcessRoleLine(line, currentTroop);
            }
        }
    }

    /// <summary>
    /// 檢查是否為軍隊標記行
    /// </summary>
    private bool IsTroopMarker(string line)
    {
        return line.StartsWith("#軍隊-");
    }

    /// <summary>
    /// 處理軍隊標記，返回當前軍隊編號或空字串
    /// </summary>
    private string ProcessTroopMarker(string line)
    {
        if (line.EndsWith("-開始"))
        {
            return line.Replace("#軍隊-", "").Replace("-開始", "");
        }
        else if (line.EndsWith("-結束"))
        {
            return string.Empty; // 清空當前軍隊標記
        }
        return string.Empty;
    }

    /// <summary>
    /// 處理角色定義行
    /// </summary>
    private void ProcessRoleLine(string line, string troopId)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
            return;

        var roleInfo = ParseRoleInfo(parts);
        var skills = CreateSkills(parts.Skip(4));
        var role = CreateRole(roleInfo, skills);
        AddRole(troopId, role);
    }

    /// <summary>
    /// 解析角色基本資訊
    /// </summary>
    /// <param name="index">the index of the role in the troop</param>
    /// <param name="parts">the parts of the line</param>
    private Role ParseRoleInfo(string[] parts)
    {
        return new Role()
        {
            Name = parts[0],
            HealthPoint = int.Parse(parts[1]),
            MagicPoint = int.Parse(parts[2]),
            Strength = int.Parse(parts[3]),
            IsHero = parts[0].Equals("英雄", StringComparison.Ordinal)
        };
    }

    /// <summary>
    /// 創建技能列表
    /// </summary>
    private List<Skill> CreateSkills(IEnumerable<string> skillNames)
    {
        var skills = new List<Skill>();
        foreach (var skillName in skillNames)
        {
            var skill = CreateSkill(skillName);
            if (skill != null)
            {
                skills.Add(skill);
            }
        }
        return skills;
    }

    /// <summary>
    /// 根據技能名稱創建對應的技能
    /// </summary>
    private Skill? CreateSkill(string skillName)
    {
        return skillName switch
        {
            "水球" => new WaterBall(),
            "火球" => new FireBall(),
            "自我治療" => new SelfHealing(),
            "石化" => new Petrochemical(),
            "下毒" => new Poison(),
            "召喚" => new Summon(),
            "自爆" => new SelfExplosion(),
            "鼓舞" => new CheerUp(),
            "詛咒" => new Curse(),
            "一拳攻擊" => CreateOnePunchSkill(),
            _ => null
        };
    }

    /// <summary>
    /// 創建一拳攻擊技能及其處理器鏈
    /// </summary>
    private OnePunch CreateOnePunchSkill()
    {
        return new OnePunch(
            new HpGreaterThanFiveHundredHandler(
                new AbnormalStateHandler(new CheerupStateHandler(new NormalStateHandler(null)))
            )
        );
    }

    /// <summary>
    /// 創建角色實例
    /// </summary>
    private Role CreateRole(Role role, List<Skill> skills)
    {
        IDecisionMaker decisionMaker = role.IsHero
            ? new HumanDecisionMaker()
            : new DefaultAiDecisionMaker();

        var normalState = new NormalState();

        role.Skills = skills;

        role.State = normalState;

        return role;
    }

    public void AddRole(string team, Role role)
    {
        if (!_troops.ContainsKey(team))
        {
            _troops.Add(team, new List<Role>());
        }
        _troops[team].Add(role);
        role.Rpg = this;
        role.Troop = team;
        role.Name = $"[{team}]{role.Name}"; // [1]英雄

        if (role.IsHero)
        {
            _hero = role;
        }
    }

    public List<Role> GetAllies(Role role)
    {
        if (role.Troop == null)
        {
            throw new ArgumentException("Role is not in any troop");
        }

        return _troops[role.Troop];
    }

    public List<Role> GetEnemies(Role role)
    {
        if (role.Troop == null)
        {
            throw new ArgumentException("Role is not in any troop");
        }

        var enemyTroopKey = _troops.Keys.FirstOrDefault(key => key != role.Troop);
        if (enemyTroopKey != null && _troops.TryGetValue(enemyTroopKey, out var enemyTroops))
        {
            return enemyTroops;
        }

        return new List<Role>();
    }

    /// <summary>
    /// 英雄死亡或是其中一方軍隊已被殲滅，遊戲結束
    /// IsHeroDead || IsAnyTroopAnnihilated
    /// </summary>
    /// <returns></returns>
    private bool CheckBattleEnd()
    {
        return IsHeroDead() || IsAnyTroopAnnihilated();
    }

    private bool IsHeroDead()
    {
        return _hero?.IsDead() ?? false;
    }

    private bool IsAnyTroopAnnihilated()
    {
        return _troops.Values.Any(troop => troop.All(role => role.IsDead()));
    }

    public void ExecuteBattle()
    {
        while (!CheckBattleEnd())
        {
            ExecuteRound();
        }
    }

    private void ExecuteRound()
    {
        foreach (var member in _troops)
        {
            var troop = member.Value;

            var index = 0;

            while (index < troop.Count)
            {
                var role = troop[index];

                if (role.IsDead())
                {
                    index++;
                    continue;
                }
                else
                {
                    ExecuteRoleTurn(role);
                }

                if (_isEnd)
                {
                    return;
                }

                index++;
            }
        }
    }

    private void ExecuteRoleTurn(Role role)
    {
        PrintRoleStatus(role);

        role.TakeTurn();

        if (CheckBattleEnd())
        {
            _isEnd = true;
        }
    }

    /// <summary>
    /// print role status
    /// </summary>
    /// <example>
    /// 輪到 [1]英雄 (HP: 300, MP: 500, STR: 100, State: 正常)。
    /// </example>
    /// <param name="role">the role</param>
    private void PrintRoleStatus(Role role)
    {
        Console.WriteLine(
            $"輪到 {role.Name} (HP: {role.HealthPoint}, MP: {role.MagicPoint}, STR: {role.Strength}, State: {role.State})。"
        );
    }

    /// <summary>
    /// 遊戲勝負揭曉 — 如果英雄仍然活著，玩家獲勝；否則玩家失敗。
    /// </summary>
    private void OnBattleEnd()
    {
        if (IsHeroDead())
        {
            Console.WriteLine("你失敗了！");
        }
        else
        {
            Console.WriteLine("你獲勝了！");
        }
    }

    public List<Role> GetRoles()
    {
        return _troops.Values.SelectMany(e => e).ToList();
    }
}
