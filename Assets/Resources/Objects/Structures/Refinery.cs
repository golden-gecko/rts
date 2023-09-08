public class Refinery : Structure
{
    protected override void Awake()
    {
        base.Awake();

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();
    }

    protected override void Start()
    {
        base.Start();

        GetComponent<Producer>().Recipes.Add(RecipeManager.Instance.Get("Iron"));
    }
}
