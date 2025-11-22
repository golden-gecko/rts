using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Researcher : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Research);

        foreach (string technology in Technologies)
        {
            Parent.Orders.AllowTechnology(technology);
        }

        Parent.OrderHandlers[OrderType.Research] = new OrderHandlerResearch();
    }

    public override string GetInfo()
    {
        return string.Format("Researcher: {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Technologies = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.
}
