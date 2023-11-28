using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Constructor : Part
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Construct);

        UpdateWhitelist();

        Parent.OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
    }

    public override string GetInfo()
    {
        return string.Format("Constructor - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    private void UpdateWhitelist()
    {
        Parent.Orders.PrefabWhitelist.Clear();

        foreach (string prefab in Prefabs)
        {
            Parent.Orders.AllowPrefab(prefab);
        }
    }

    [field: SerializeField]
    public List<string> Prefabs { get; private set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources used per second.
}
