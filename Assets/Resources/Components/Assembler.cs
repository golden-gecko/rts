using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Assembler : MyComponent
{
    protected override void Start()
    {
        base.Start();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Assemble);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Rally);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Rally] = new OrderHandlerRally();

        RallyPoint = GetComponent<MyGameObject>().Exit;
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;

    [field: SerializeField]
    public Vector3 RallyPoint { get; set; }
}
