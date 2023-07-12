public class Barracks : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Objects/Units/Infantry_Light");

        Resources.Add("Metal", 0, 40);

        Recipe r1 = new Recipe();

        r1.Consume("Metal", 0);

        Recipes.Add(r1);
    }
}
