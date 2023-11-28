using UnityEngine;

[DisallowMultipleComponent]
public class Gatherer : Part
{
    protected override void Start()
    {
        base.Start();

        Parent.Orders.AllowOrder(OrderType.GatherObject);
        Parent.Orders.AllowOrder(OrderType.GatherResource);
    }

    public override string GetInfo()
    {
        return string.Format("Gatherer - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public float Range { get; private set; } = 1.0f;

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources gathered per second.
}
