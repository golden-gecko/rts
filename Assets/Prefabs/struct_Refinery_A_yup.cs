public class struct_Refinery_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Produce);

        Resources.Add("Coal", 20, 300);
        Resources.Add("Metal Ore", 100, 100);
        Resources.Add("Metal", 0, 100);

        var r1 = new Recipe();

        r1.Consume("Coal", 2);
        r1.Consume("Metal Ore", 1);
        r1.Produce("Metal", 1);

        Recipes.Add(r1);
    }

    #region Handlers
    protected override void OnOrderIdle()
    {
        Produce();

        Orders.Pop();
    }
    #endregion
}
