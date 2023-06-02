public class Resource
{
    public Resource(string name, int value, int max)
    {
        Name = name;
        Value = value;
        Max = max;
    }

    public void Add(int value)
    {
        Value += value;
    }

    public void Remove(int value)
    {
        Value -= value;
    }

    public string Name { get; }

    public int Value { get; private set; }

    public int Max { get; private set; }
}
