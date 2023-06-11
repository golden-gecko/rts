public class struct_Factory_Light_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);

        Orders.AllowPrefab("Prefabs/Vehicles/unit_Grav_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Harvester_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Quad_A_yup");
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Trike_A_yup");
    }
}
