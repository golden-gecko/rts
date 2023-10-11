using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Producer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Produce);

        Parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();
        Parent.OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();

        foreach (string recipe in Recipes)
        {
            Parent.Orders.AllowRecipe(RecipeManager.Instance.Get(recipe));
        }
    }

    public override string GetInfo()
    {
        return string.Format("Producer: {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Recipes { get; private set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources used per second.
}
