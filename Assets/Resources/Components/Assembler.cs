using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Assembler : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Assemble);
        parent.Orders.AllowOrder(OrderType.Rally);

        foreach (string prefab in Prefabs)
        {
            parent.Orders.AllowPrefab(Path.Join(Config.DirectoryUnits, prefab));
        }

        parent.OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        parent.OrderHandlers[OrderType.Rally] = new OrderHandlerRally();

        RallyPoint = parent.Exit;
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Prefabs { get; set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.

    [field: SerializeField]
    public Vector3 RallyPoint { get; set; }
}
