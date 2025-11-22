using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Storage : Part
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.GatherObject);
        Parent.Orders.AllowOrder(OrderType.GatherResource);
        Parent.Orders.AllowOrder(OrderType.Load);
        Parent.Orders.AllowOrder(OrderType.Stock);
        Parent.Orders.AllowOrder(OrderType.Transport);
        Parent.Orders.AllowOrder(OrderType.Unload);

        Parent.OrderHandlers[OrderType.GatherObject] = new OrderHandlerGatherObject();
        Parent.OrderHandlers[OrderType.GatherResource] = new OrderHandlerGatherResource();
        Parent.OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        Parent.OrderHandlers[OrderType.Stock] = new OrderHandlerStock();
        Parent.OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        Parent.OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();
    }

    protected override void Update()
    {
        base.Update();

        if (Alive == false)
        {
            return;
        }

        switch (Parent.State)
        {
            case MyGameObjectState.Operational:
                CreateResourceFlags();
                break;

            default:
                RemoveResourceFlags();
                break;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Storage - {0}\nResources: {1}", base.GetInfo(), Resources.GetInfo());
    }

    private void CreateResourceFlags()
    {
        foreach (Resource resource in Resources.Items)
        {
            if (resource.In)
            {
                if (resource.Full)
                {
                    Parent.Player.UnregisterConsumer(Parent, resource.Name);
                }
                else
                {
                    Parent.Player.RegisterConsumer(Parent, resource.Name, resource.Available, resource.Direction);
                }
            }

            if (resource.Out)
            {
                if (resource.Empty)
                {
                    Parent.Player.UnregisterProducer(Parent, resource.Name);
                }
                else
                {
                    Parent.Player.RegisterProducer(Parent, resource.Name, resource.Current, resource.Direction);
                }
            }
        }
    }

    private void RemoveResourceFlags()
    {
        foreach (string name in Resources.Items.Select(x => x.Name))
        {
            Parent.Player.UnregisterConsumer(Parent, name);
            Parent.Player.UnregisterProducer(Parent, name);
        }
    }

    [field: SerializeField]
    public ResourceContainer Resources { get; private set; } = new ResourceContainer();

    [field: SerializeField]
    public bool RaiseResourceFlags { get; private set; } = true;

    [field: SerializeField]
    public bool Gatherable { get; private set; } = false;

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources loaded or unloaded per second.
}
