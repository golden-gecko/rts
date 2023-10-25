using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Armour : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("Armour - {0}, Value: {1}", base.GetInfo(), Value.GetInfo());
    }

    public float Absorb(DamageType type, float damage)
    {
        float removed = 0.0f;

        foreach (DamageTypeItem i in ProtectionType)
        {
            if (i.Type == type)
            {
                float damageToReflect = damage * i.Ratio;
                float damageToAbsorb = damage - damageToReflect;

                removed += damageToReflect;
                removed += Value.Remove(damageToAbsorb);

                break;
            }
        }

        return removed;
    }

    public float Repair(float health)
    {
        return Value.Add(health);
    }

    [field: SerializeField]
    public Progress Value { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
