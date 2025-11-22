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

            mesh.transform.parent = parent.transform;
            mesh.transform.localScale = new Vector3(Range * 2.0f / parent.Scale.x, Range * 2.0f / parent.Scale.y, Range * 2.0f / parent.Scale.z);
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}, Power: {2:0.}", base.GetInfo(), Range, Power);
    }

    public float Absorb(float damage)
    {
        return damage * Power / 100.0f;
    }

    [field: SerializeField]
    public GameObject Mesh { get; set; }

    [field: SerializeField]
    public float Range { get; set; } = 4.0f;

    [field: SerializeField]
    public float Power { get; set; } = 50.0f; // From 0 to 100 (0 - no damage is absorbed, 100 - all damage is absorbed).
}
