using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Researcher : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Research);

        parent.OrderHandlers[OrderType.Research] = new OrderHandlerResearch();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.
}
