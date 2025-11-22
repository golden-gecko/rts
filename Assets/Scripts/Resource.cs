public class Resource
{
    public Resource(string name, float value)
    {
        Name = name;
        Value = value;
    }

    public void Add(float value)
    {
        Value += value;
    }

    public string Name { get; }
    public float Value { get; private set; }
}
