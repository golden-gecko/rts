using UnityEngine;

public class Producer : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Produce Time: {1:0.}", base.GetInfo(), ProduceTime);
    }

    [field: SerializeField]
    public float ProduceTime { get; set; } = 2.0f; // TODO: Replace with rate (resource per second).
}
