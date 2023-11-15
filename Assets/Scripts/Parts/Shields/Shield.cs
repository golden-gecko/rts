using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Shield : Part
{
    protected override void Update()
    {
        base.Update();

        if (ResetTime.Update(Time.deltaTime))
        {
            Capacity.Add(ChargeRate * Time.deltaTime);
        }

        GetComponent<MeshRenderer>().enabled = Capacity.Current > 0.0f;
    }

    public override string GetInfo()
    {
        return string.Format("Shield - {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
    }

    public float Absorb(DamageType type, float damage)
    {
        if (Alive == false)
        {
            return 0.0f;
        }

        float removed = 0.0f;

        foreach (DamageTypeItem i in ProtectionType)
        {
            if (i.Type == type)
            {
                float damageToReflect = damage * i.Ratio;
                float damageToAbsorb = damage - damageToReflect;

                removed += damageToReflect;
                removed += Capacity.Remove(damageToAbsorb);

                if (Capacity.Current <= 0.0f)
                {
                    ResetTime.Reset();
                }

                break;
            }
        }

        return removed;
    }

    public float Repair(float health)
    {
        return Capacity.Add(health);
    }

    [field: SerializeField]
    public Property Range { get; private set; } = new Property(1.0f);

    [field: SerializeField]
    public Progress Capacity { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float ChargeRate { get; private set; } = 0.1f;

    [field: SerializeField]
    public Timer ResetTime { get; private set; } = new Timer(3.0f, 3.0f);

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
