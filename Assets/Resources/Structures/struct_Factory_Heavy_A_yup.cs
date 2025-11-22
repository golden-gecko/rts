public class struct_Factory_Heavy_A_yup : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab("Vehicles/unit_Tank_Combat_A_yup");
        Orders.AllowPrefab("Vehicles/unit_Tank_Missile_A_yup");

        Resources.Add("Metal", 0, 40);

        Recipe r1 = new Recipe();

        r1.Consume("Metal", 0);

        Recipes.Add(r1);
    }
}
