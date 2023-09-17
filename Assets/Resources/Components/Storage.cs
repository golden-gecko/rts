using System.Linq;
using UnityEngine;

public class Storage : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Load);
        parent.Orders.AllowOrder(OrderType.Transport);
        parent.Orders.AllowOrder(OrderType.Unload);

        parent.OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        parent.OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        parent.OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();

        parent.ShowEntrance = Resources.In;
        parent.ShowExit = Resources.Out;
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        switch (parent.State) // TODO: Refactor.
        {
            case MyGameObjectState.Operational:
                if (parent.Working && RaiseResourceFlags)
                {
                    CreateResourceFlags(parent);
                }
                else
                {
                    RemoveResourceFlags(parent);
                }
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
                    parent.Player.RegisterConsumer(parent, resource.Name, resource.Capacity, resource.Direction);
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
                    parent.Player.RegisterProducer(parent, resource.Name, resource.Storage, resource.Direction);
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
