public class Factory_Light : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Objects/Units/Grav_Light");
        Orders.AllowPrefab("Objects/Units/Harvester");
        Orders.AllowPrefab("Objects/Units/Quad");
        Orders.AllowPrefab("Objects/Units/Trike");
        
        Resources.Add("Iron", 0, 40);

        Recipe r1 = new Recipe("Iron");

        r1.Consumes("Iron", 0);

        Recipes.Add(r1);
    }
}
