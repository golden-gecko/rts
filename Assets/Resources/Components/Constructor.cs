using UnityEngine;

public class Constructor : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Construction Time: {1:0.}", base.GetInfo(), ConstructionTime);
    }

    [field: SerializeField]
    public float ConstructionTime { get; set; } = 10.0f; // TODO: Replace with rate (resource per second).
}
