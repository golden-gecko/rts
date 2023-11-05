using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Storage))]
public class Assembler : Part
{
    protected override void Start()
    {
        base.Start();

        Parent.Orders.AllowOrder(OrderType.Assemble);
        Parent.Orders.AllowOrder(OrderType.Rally);

        foreach (GameObject prefab in ConfigPrefabs.Instance.Units)
        {
            Parent.Orders.AllowPrefab(prefab.name);
        }

        Parent.OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        Parent.OrderHandlers[OrderType.Rally] = new OrderHandlerRally();

        RallyPoint = Parent.Exit;
    }

    public override string GetInfo()
    {
        return string.Format("Assembler - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Prefabs { get; private set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources used per second.

    [field: SerializeField]
    public Vector3 RallyPoint { get; set; }
}
