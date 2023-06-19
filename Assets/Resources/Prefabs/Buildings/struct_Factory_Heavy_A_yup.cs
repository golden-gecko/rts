public class struct_Factory_Heavy_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);

        Orders.AllowPrefab("Prefabs/Vehicles/unit_Tank_Combat_A_yup", PrefabConstructionType.Unit);
        Orders.AllowPrefab("Prefabs/Vehicles/unit_Tank_Missile_A_yup", PrefabConstructionType.Unit);

        Resources.Add("Metal", 0, 40);

        var r1 = new Recipe();

        r1.Consume("Metal", 0);

        Recipes.Add(r1);
    }
}
