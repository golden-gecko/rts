using System.Collections.Generic;
using UnityEngine;

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
        return string.Format("Shield: {0}, Range: {1:0.}, Power: {2:0.}", base.GetInfo(), Range.Total, Power);
    }

    public float Absorb(float damage)
    {
        return Capacity.Remove(damage * Power);
    }

    [field: SerializeField]
    public GameObject Mesh { get; private set; }

    [field: SerializeField]
    public Property Range { get; private set; } = new Property();

    [field: SerializeField]
    public Progress Capacity { get; private set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float Power { get; private set; } = 0.2f; // From 0.0 to 1.0 (0.0 - no damage is absorbed, 1.0 - all damage is absorbed).

    [field: SerializeField]
    public float ChargeRate { get; private set; } = 0.1f;

    [field: SerializeField]
    public List<DamageTypeItem> ProtectionType { get; private set; } = new List<DamageTypeItem>();
}
