public class Refinery : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Produce);

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();

        Resources.Add("Coal", 0, 60);
        Resources.Add("Metal", 0, 60);
        Resources.Add("Metal Ore", 0, 60);
        Resources.Add("Wood", 0, 60);

        Recipe r1 = new Recipe("Metal using coal");
        Recipe r2 = new Recipe("Metal using wood");

        r1.Consumes("Coal", 2);
        r1.Consumes("Metal Ore", 1);
        r1.Produces("Metal", 1);

        r2.Consumes("Metal Ore", 1);
        r2.Consumes("Wood", 4);
        r2.Produces("Metal", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
    }
}
