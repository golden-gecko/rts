using UnityEngine;

public class Chassis : Part
{
    [field: SerializeField]
    public Progress Health { get; private set; } = new Progress(100.0f, 100.0f);
}
