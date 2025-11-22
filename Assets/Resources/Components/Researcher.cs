using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Researcher : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Research);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Research] = new OrderHandlerResearch();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;
}
