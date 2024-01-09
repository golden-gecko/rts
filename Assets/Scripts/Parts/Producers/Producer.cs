using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Producer : Part
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Produce);

        Parent.OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();
    }

    protected override void Start()
    {
        foreach (string recipe in Recipes)
        {
            Parent.Orders.AllowRecipe(Game.Instance.RecipeManager.Get(recipe));
        }
    }

    public override string GetInfo()
    {
        return string.Format("Producer - {0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public List<string> Recipes { get; private set; } = new List<string>();

    [field: SerializeField]
    public string Recipe { get; private set; }

    [field: SerializeField]
    public int ResourceUsage { get; private set; } = 1; // Number of resources used per second.
}
