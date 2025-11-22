using UnityEngine;

public class Producer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Produce);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 1;

    public RecipeContainer Recipes { get; } = new RecipeContainer();
}
