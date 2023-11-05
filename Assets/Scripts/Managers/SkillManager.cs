using System.Collections.Generic;

public class SkillManager : Singleton<SkillManager>
{
    protected override void Awake()
    {
        base.Awake();

        CreateSkills();
    }

    public Skill Get(string name)
    {
        if (Skills.TryGetValue(name, out Skill skill))
        {
            return Skills[name].Clone() as Skill;
        }

        return null;
    }

    private void CreateSkills()
    {
        Skills["PassiveIncreaseDamage"] = new PassiveIncreaseDamage("PassiveIncreaseDamage", 10.0f, -1.0f, 2.0f);
        Skills["PassiveIncreasePower"] = new PassiveIncreasePower("PassiveIncreasePower", 10.0f, -1.0f, 2.0f);
        Skills["PassiveIncreaseRange"] = new PassiveIncreaseRange("PassiveIncreaseRange", 10.0f, -1.0f, 2.0f);

        Skills["Damage"] = new Damage("Damage", 10.0f, 3.0f, 20.0f, new List<DamageTypeItem> { new DamageTypeItem { Type = DamageType.Kinetic, Ratio = 1.0f }, new DamageTypeItem { Type = DamageType.Laser, Ratio = 1.0f } });
        Skills["Repair"] = new Repair("Repair", 10.0f, 3.0f, 20.0f);
    }

    public Dictionary<string, Skill> Skills { get; } = new Dictionary<string, Skill>();
}
