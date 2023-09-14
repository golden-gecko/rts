using UnityEngine;

public class Shield : MyComponent // TODO: Shield puts health bar in wrong position.
{
    protected override void Start()
    {
        base.Start();

        if (Mesh != null)
        {
            MyGameObject parent = GetComponent<MyGameObject>();
            GameObject mesh = Instantiate(Mesh, parent.Position, Quaternion.identity);

            mesh.transform.parent = parent.Body.transform;
            mesh.transform.localScale = new Vector3(Range * 2.0f / parent.Scale.x, Range * 2.0f / parent.Scale.y, Range * 2.0f / parent.Scale.z);
        }
    }

    protected override void Update()
    {
        base.Update();

        Capacity.Add(ChargeRate * Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("Shield: {0}, Range: {1:0.}, Power: {2:0.}", base.GetInfo(), Range, Power);
    }

    public float Absorb(float damage)
    {
        return Capacity.Remove(damage * Power);
    }

    [field: SerializeField]
    public GameObject Mesh { get; set; }

    [field: SerializeField]
    public float Range { get; set; } = 4.0f;

    [field: SerializeField]
    public Progress Capacity { get; set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float Power { get; set; } = 0.2f; // From 0.0 to 1.0 (0.0 - no damage is absorbed, 1.0 - all damage is absorbed).

    [field: SerializeField]
    public float ChargeRate { get; set; } = 0.1f;
}
