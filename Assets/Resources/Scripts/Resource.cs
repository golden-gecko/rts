using System;
using UnityEngine;

[Serializable]
public class Resource : Counter
{
    public Resource(string name, int current = 0, int max = 0, ResourceDirection direction = ResourceDirection.None) : base(current, max)
    {
        Name = name;
        Direction = direction;
    }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public ResourceDirection Direction { get; private set; }

    public int Capacity { get => Max - Current; }

    public int Storage { get => Current; }

    public bool Empty { get => Current <= 0; }

    public bool Full { get => Current >= Max; }

    public bool In { get => Direction == ResourceDirection.Both || Direction == ResourceDirection.In; }

    public bool Out{ get => Direction == ResourceDirection.Both || Direction == ResourceDirection.Out; }
}
