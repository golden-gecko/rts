public class struct_Refinery_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Produce);

        var r1 = new Recipe();
        var r2 = new Recipe();

        r1.Consume("Coal", 2);
        r1.Consume("Metal Ore", 1);
        r1.Produce("Metal", 1);

        r2.Consume("Metal Ore", 1);
        r2.Consume("Wood", 4);
        r2.Produce("Metal", 1);

        Recipes.Add(r1);
        Recipes.Add(r2);
    }

    protected override void OnOrderIdle()
    {
        Produce();
    }
}
