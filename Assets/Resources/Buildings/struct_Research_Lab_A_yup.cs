public class struct_Research_Lab_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Research);

        Resources.Add("Crystal", 0, 20);

        var r1 = new Recipe();

        r1.Consume("Crystal", 0);

        Recipes.Add(r1);
    }
}
