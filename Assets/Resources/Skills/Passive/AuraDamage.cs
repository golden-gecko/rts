public class AuraDamage : Skill
{
    public override object Clone()
    {
        return new AuraDamage(Name, Range, Cooldown.Max, Value);
    }

    public AuraDamage(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Green hit")
    {
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
    }

    public float Value { get; } = 0.0f;
}
