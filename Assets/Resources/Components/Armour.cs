using UnityEngine;

public class Armour : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Value: {1:0.}/{2:0.}", base.GetInfo(), Value, ValueMax);
    }

    [field: SerializeField]
    public float Value { get; set; } = 10.0f;

    [field: SerializeField]
    public float ValueMax { get; set; } = 10.0f;
}
