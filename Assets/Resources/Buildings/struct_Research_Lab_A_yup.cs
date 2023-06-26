public class struct_Research_Lab_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Research);

        Resources.Add("Crystal", 0, 20);

        Recipe r1 = new Recipe();

        r1.Consume("Crystal", 0);

        Recipes.Add(r1);

        Health = 100.0f;
        MaxHealth = 100.0f;
    }
}
