public class Resource : Counter
{
    public Resource(string name, int value, int max) : base(value, max)
    {
        Name = name;
    }

    public string Name { get; }

    public int Capacity { get => Max - Current; }

    public int Storage { get => Current; }
}
