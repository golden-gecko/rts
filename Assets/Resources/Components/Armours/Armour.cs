using UnityEngine;

public class Armour : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Value: {1}", base.GetInfo(), Value.GetInfo());
    }

    [field: SerializeField]
    public Progress Value { get; set; } = new Progress(10.0f, 10.0f);
}
