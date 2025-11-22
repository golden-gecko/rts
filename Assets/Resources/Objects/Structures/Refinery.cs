public class Refinery : Structure
{
    protected override void Awake()
    {
        base.Awake();

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();

        GetComponent<Storage>().Resources.Add("Coal", 0, 60, ResourceDirection.In);
        GetComponent<Storage>().Resources.Add("Iron", 0, 60, ResourceDirection.Out);
        GetComponent<Storage>().Resources.Add("Iron Ore", 0, 60, ResourceDirection.In);
    }

    protected override void Start()
    {
        base.Start();

        GetComponent<Producer>().Recipes.Add(RecipeManager.Instance.Get("Iron"));
    }
}
