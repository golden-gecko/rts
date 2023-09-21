public class AuraHealth : Skill
{
    public override object Clone()
    {
        return new AuraHealth(Name, Range, Cooldown.Max, Value);
    }

    public AuraHealth(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Healing")
    {
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
    }

    public float Value { get; } = 0.0f;
}
