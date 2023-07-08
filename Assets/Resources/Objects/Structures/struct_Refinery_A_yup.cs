public class struct_Refinery_A_yup : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Produce);

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleProducer();

        Resources.Add("Coal", 0, 60);
        Resources.Add("Metal", 0, 60);
        Resources.Add("Metal Ore", 0, 60);

        Recipe r1 = new Recipe();
        Recipe r2 = new Recipe();

        r1.Consume("Coal", 2);
        r1.Consume("Metal Ore", 1);
        r1.Produce("Metal", 1);

        r2.Consume("Metal Ore", 1);
        r2.Consume("Wood", 4);
        r2.Produce("Metal", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
    }
}
