public class Resource : Counter
{
    public Resource(string name, int value, int max) : base(value, max)
    {
        Name = name;
    }

    public int Capacity()
    {
        return Max - Current;
    }

    public int Storage()
    {
        return Current;
    }

    public string Name { get; }
}
