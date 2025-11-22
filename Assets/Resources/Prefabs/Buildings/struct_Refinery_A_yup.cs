public class struct_Refinery_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Produce);

        Resources.Add("Coal", 10, 300);
        Resources.Add("Metal Ore", 100, 100);
        Resources.Add("Metal", 0, 100);
        Resources.Add("Wood", 60, 600);

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

        Orders.Pop();
    }
}
