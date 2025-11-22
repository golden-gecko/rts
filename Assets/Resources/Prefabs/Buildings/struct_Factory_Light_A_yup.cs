public class struct_Factory_Light_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);

        Orders.AllowPrefab("Prefabs/Buildings/unit_Grav_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/unit_Harvester_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/unit_Quad_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/unit_Trike_A_yup");
    }
}
