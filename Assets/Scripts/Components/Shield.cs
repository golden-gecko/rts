using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Shield : Part
{
    protected override void Start()
    {
        base.Start();

        if (Mesh != null)
        {
            ShieldMesh = Instantiate(Mesh, Parent.Position, Quaternion.identity);
            ShieldMesh.transform.parent = Parent.transform;
            ShieldMesh.transform.localScale = new Vector3(Range.Total * 2.0f / Parent.Scale.x, Range.Total * 2.0f / Parent.Scale.y, Range.Total * 2.0f / Parent.Scale.z);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (ResetTime.Update(Time.deltaTime))
        {
            ShieldMesh.SetActive(true);

            Capacity.Add(ChargeRate * Time.deltaTime);
        }
        else
        {
            ShieldMesh.SetActive(false);
        }
    }

    public override string GetInfo()
    {
        return string.Format("Shield - {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
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
    public GameObject Mesh { get; private set; }

    [field: SerializeField]
    public Property Range { get; private set; } = new Property();

    [field: SerializeField]
    public Progress Capacity { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float ChargeRate { get; private set; } = 0.1f;

    [field: SerializeField]
    public Timer ResetTime { get; private set; } = new Timer(3.0f, 3.0f);

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();

    private GameObject ShieldMesh { get; set; }
}
