using System.Linq;
using UnityEngine;

public class Storage : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Load);
        Parent.Orders.AllowOrder(OrderType.Stock);
        Parent.Orders.AllowOrder(OrderType.Transport);
        Parent.Orders.AllowOrder(OrderType.Unload);

        Parent.OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        Parent.OrderHandlers[OrderType.Stock] = new OrderHandlerStock();
        Parent.OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        Parent.OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();

        Parent.ShowEntrance = Resources.In;
        Parent.ShowExit = Resources.Out;
    }

    protected override void Update()
    {
        base.Update();

        switch (Parent.State)
        {
            case MyGameObjectState.Operational:
                CreateResourceFlags(Parent);
                break;

            default:
                RemoveResourceFlags(Parent);
                break;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Storage: {0}\nResources: {1}", base.GetInfo(), Resources.GetInfo());
    }

    private void CreateResourceFlags(MyGameObject parent)
    {
        foreach (Resource resource in Resources.Items)
        {
            if (resource.In)
            {
                if (resource.Full)
                {
                    parent.Player.UnregisterConsumer(parent, resource.Name);
                }
                else
                {
                    parent.Player.RegisterConsumer(parent, resource.Name, resource.Available, resource.Direction);
                }
            }

            if (resource.Out)
            {
                if (resource.Empty)
                {
                    parent.Player.UnregisterProducer(parent, resource.Name);
                }
                else
                {
                    parent.Player.RegisterProducer(parent, resource.Name, resource.Current, resource.Direction);
                }
            }
        }
    }

    private void RemoveResourceFlags(MyGameObject parent)
    {
        foreach (string name in Resources.Items.Select(x => x.Name))
        {
            parent.Player.UnregisterConsumer(parent, name);
            parent.Player.UnregisterProducer(parent, name);

        }
    }

    [field: SerializeField]
    public bool RaiseResourceFlags = true;

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources loaded or unloaded per second.

    [field: SerializeField]
    public ResourceContainer Resources { get; set; } = new ResourceContainer();
}
