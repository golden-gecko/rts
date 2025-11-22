using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Producer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();
        RecipeManager recipeManager = Game.Instance.GetComponent<RecipeManager>();

        parent.Orders.AllowOrder(OrderType.Produce);

        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();
        parent.OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();

        foreach (string recipe in RecipesNames)
        {
            Recipes.Add(recipeManager.Get(recipe));
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> RecipesNames { get; set; } = new List<string>();

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.

    public RecipeContainer Recipes { get; } = new RecipeContainer(); // TODO: Move to Orders.
}
