using UnityEngine;

public class Storage : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Load);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Transport);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Unload);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();
    }

    public override string GetInfo()
    {
        return string.Format("{0}\nResources: {1}", base.GetInfo(), Resources.GetInfo());
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;

    public ResourceContainer Resources { get; } = new ResourceContainer();
}
