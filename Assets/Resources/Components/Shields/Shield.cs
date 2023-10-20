using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class Shield : MyComponent
{
    protected override void Start()
    {
        base.Start();

        if (Mesh != null)
        {
            GameObject mesh = Instantiate(Mesh, Parent.Position, Quaternion.identity);

            mesh.transform.parent = Parent.transform;
            mesh.transform.localScale = new Vector3(Range.Total * 2.0f / Parent.Scale.x, Range.Total * 2.0f / Parent.Scale.y, Range.Total * 2.0f / Parent.Scale.z);
        }
    }

    protected override void Update()
    {
        base.Update();

        Capacity.Add(ChargeRate * Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("Shield: {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
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
                absorbed += Capacity.Remove(damageToAbsorb);

                break;
            }
        }

        return absorbed;
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
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
