using UnityEngine;

public class Assembler : MyComponent
{
    protected override void Start()
    {
        base.Start();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Assemble);

        RallyPoint = GetComponent<MyGameObject>().Entrance;
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 2;

    [field: SerializeField]
    public Vector3 RallyPoint { get; set; }
}
