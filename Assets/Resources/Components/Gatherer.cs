using UnityEngine;

public class Gatherer : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Gather Time: {1:0.}", base.GetInfo(), GatherTime);
    }

    [field: SerializeField]
    public float GatherTime { get; set; } = 2.0f;
}
