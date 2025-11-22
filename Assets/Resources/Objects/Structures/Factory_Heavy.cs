public class Factory_Heavy : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Objects/Units/Tank_Combat");
        Orders.AllowPrefab("Objects/Units/Tank_Missile");

        Resources.Add("Iron", 0, 40);

        Recipe r1 = new Recipe("Iron");

        r1.Consumes("Iron", 0);

        Recipes.Add(r1);
    }
}
