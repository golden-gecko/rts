using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class Armour : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("Armour: {0}, Value: {1}", base.GetInfo(), Value.GetInfo());
    }

    public float Absorb(DamageType type, float damage)
    {
        float absorbed = 0.0f;

        foreach (DamageTypeItem i in ProtectionType)
        {
            if (i.Type == type)
            {
                float damageToReflect = damage * i.Ratio;
                float damageToAbsorb = damage - damageToReflect;

                absorbed += damageToReflect;
                absorbed += Value.Remove(damageToAbsorb);

                break;
            }
        }

        return absorbed;
    }

    [field: SerializeField]
    public Progress Value { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
