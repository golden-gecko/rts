public class Factory_Heavy : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Objects/Units/Tank_Combat");
        Orders.AllowPrefab("Objects/Units/Tank_Missile");

        Resources.Add("Metal", 0, 40);

        Recipe r1 = new Recipe("Metal");

        r1.Consumes("Metal", 0);

        Recipes.Add(r1);
    }
}
