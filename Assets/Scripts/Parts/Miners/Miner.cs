using UnityEngine;

[DisallowMultipleComponent]
public class Miner : Part
{
    protected override void Start()
    {
        base.Start();

        Parent.Orders.AllowOrder(OrderType.MineObject);
        Parent.Orders.AllowOrder(OrderType.MineResource);

        Parent.OrderHandlers[OrderType.MineObject] = new OrderHandlerMineObject();
        Parent.OrderHandlers[OrderType.MineResource] = new OrderHandlerMineResource();
    }

    public override string GetInfo()
    {
        return string.Format("Miner - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public float Range { get; private set; } = 1.0f;

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources mined per second.
}
