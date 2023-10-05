using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Producer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Produce);

        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();
        parent.OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();

        foreach (string recipe in Recipes)
        {
            parent.Orders.AllowRecipe(RecipeManager.Instance.Get(recipe));
        }
    }

    public override string GetInfo()
    {
        return string.Format("Producer: {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Recipes { get; set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.
}
