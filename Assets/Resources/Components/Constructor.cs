using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Constructor : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Construct);

        parent.OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.
}
