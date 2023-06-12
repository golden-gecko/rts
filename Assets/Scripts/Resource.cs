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

        if (Value > Max)
        {
            Value = Max;
        }
    }

    public bool CanAdd(int value)
    {
        return Value + value <= Max;
    }

    public bool CanRemove(int value)
    {
        return Value - value >= 0;
    }

    public int Capacity()
    {
        return Max - Value;
    }

    public void Remove(int value)
    {
        Value -= value;

        if (Value < 0)
        {
            Value = 0;
        }
    }

    public string Name { get; }

    public int Value { get; private set; }

    // TODO: Hide setter.
    public int Max { get; set; }
}
