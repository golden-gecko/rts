using UnityEngine;

public class Shield : MyComponent // TODO: Shield puts health bar in wrong position.
{
    protected override void Start()
    {
        base.Start();

        if (Mesh != null)
        {
            GameObject mesh = Instantiate(Mesh, Parent.Position, Quaternion.identity);

            mesh.transform.parent = Parent.Body.transform;
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
    public GameObject Mesh { get; set; }

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public Progress Capacity { get; set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float Power { get; set; } = 0.2f; // From 0.0 to 1.0 (0.0 - no damage is absorbed, 1.0 - all damage is absorbed).

    [field: SerializeField]
    public float ChargeRate { get; set; } = 0.1f;
}
