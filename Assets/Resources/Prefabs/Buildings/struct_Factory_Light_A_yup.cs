public class struct_Factory_Light_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);

        Orders.AllowPrefab("Prefabs/Vehicles/unit_Infantry_Light_B_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Grav_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Harvester_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Quad_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Trike_A_yup");
        
        Resources.Add("Metal", 0, 40);

        var r1 = new Recipe();

        r1.Consume("Metal", 0);

        Recipes.Add(r1);
    }
}
