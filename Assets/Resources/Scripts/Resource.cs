public class Resource : Counter
{
    public Resource(string name, int value, int max = 0, ResourceDirection direction = ResourceDirection.None) : base(value, max)
    {
        Name = name;
        Direction = direction;
    }

    public string Name { get; }

    public ResourceDirection Direction { get; }

    public int Capacity { get => Max - Current; }

    public int Storage { get => Current; }

    public bool Empty { get => Current <= 0; }

    public bool Full { get => Current >= Max; }
}
