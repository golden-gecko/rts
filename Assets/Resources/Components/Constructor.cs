using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Constructor : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Construct);

        foreach (string prefab in Prefabs)
        {
            Parent.Orders.AllowPrefab(Path.Join(Config.Asset.Structures, prefab));
        }

        Parent.OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
    }

    public override string GetInfo()
    {
        return string.Format("Constructor: {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Prefabs { get; set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.
}
