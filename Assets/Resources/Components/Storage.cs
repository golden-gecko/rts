using UnityEngine;

public class Storage : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Gather);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Load);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Unload);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Transport);
    }

    public override string GetInfo()
    {
        return string.Format("{0}\nResources: {1}", base.GetInfo(), Resources.GetInfo());
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;

    public ResourceContainer Resources { get; } = new ResourceContainer();
}
