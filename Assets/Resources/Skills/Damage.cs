public class Damage : Skill
{
    public Damage(string name, float range, float value) : base(name)
    {
        Range = range;
        Value = value;
    }

    public override void Execute()
    {
        // TODO: Find all objects in range and deal damage.
    }

    public float Range { get; }
 
    public float Value { get; }
}
