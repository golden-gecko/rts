using System.Collections.Generic;
using UnityEngine;

public class Armour : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("Armour: {0}, Value: {1}", base.GetInfo(), Value.GetInfo());
    }

    public float Absorb(float damage)
    {
        return Value.Remove(damage);
    }

    [field: SerializeField]
    public Progress Value { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
