public class Armour : MyComponent
{
    public Armour(MyGameObject parent, string name, float mass, float value) : base(parent, name, mass)
    {
        Value = value;
        ValueMax = value;
    }

    public override string GetInfo()
    {
        return string.Format("Name: {0}, Value: {1:0.}/{2:0.}", Name, Value, ValueMax);
    }

    public float Value { get; set; }  // TODO: Hide setter.

    public float ValueMax { get; }
}
