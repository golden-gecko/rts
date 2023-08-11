using UnityEngine;

public class Radar : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 30.0f; // TODO: Implement.
}
