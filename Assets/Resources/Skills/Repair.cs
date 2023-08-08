public class Repair : Skill
{
    public Repair(string name, float range, float value) : base(name)
    {
        Range = range;
        Value = value;
    }

    public override void Execute()
    {
        // TODO: Find all objects in range and repair.
    }

    public float Range { get; }
 
    public float Value { get; }
}
