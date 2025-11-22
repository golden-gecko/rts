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
        
        Resources.Add("Metal", 40, 40);

        Recipe r1 = new Recipe("Metal");

        r1.Consumes("Metal", 0);

        Recipes.Add(r1);
    }
}
