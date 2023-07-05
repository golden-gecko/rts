public class struct_Factory_Light_A_yup : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Objects/Units/unit_Infantry_Light_B_yup");
        Orders.AllowPrefab("Objects/Units/unit_Grav_Light_A_yup");
        Orders.AllowPrefab("Objects/Units/unit_Harvester_A_yup");
        Orders.AllowPrefab("Objects/Units/unit_Quad_A_yup");
        Orders.AllowPrefab("Objects/Units/unit_Trike_A_yup");
        
        Resources.Add("Metal", 0, 40);

        Recipe r1 = new Recipe();

        r1.Consume("Metal", 0);

        Recipes.Add(r1);
    }
}
