using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Storage))]
public class Constructor : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Construct);

        foreach (GameObject prefab in ConfigPrefabs.Instance.Structures)
        {
            Parent.Orders.AllowPrefab(prefab.name);
        }

        Parent.OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
    }

    public override string GetInfo()
    {
        return string.Format("Constructor - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Prefabs { get; private set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources used per second.
}
