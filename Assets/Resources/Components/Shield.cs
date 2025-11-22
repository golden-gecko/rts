using UnityEngine;

public class Shield : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 4.0f; // TODO: Implement.
}
