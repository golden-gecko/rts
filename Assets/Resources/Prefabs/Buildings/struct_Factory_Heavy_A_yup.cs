public class struct_Factory_Heavy_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);

        Orders.AllowPrefab("Prefabs/Buildings/unit_Tank_Combat_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/unit_Tank_Missile_A_yup");
    }
}
