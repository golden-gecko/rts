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
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1; // Number of resources used per second.

    public RecipeContainer Recipes { get; } = new RecipeContainer();
}
