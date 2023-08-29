using UnityEngine;

public class PowerPlant : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1}, Range: {2}", base.GetInfo(), Power, Range);
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;
}
