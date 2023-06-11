public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowPrefab("Prefabs/Buildings/struct_Barracks_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Heavy_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Misc_Building_B_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Radar_Outpost_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Refinery_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Research_Lab_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Spaceport_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Turret_Gun_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Turret_Missile_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Wall_A_yup");

        Resources.Add("Coal", 100, 100);
        Resources.Add("Crystal", 100, 100);
        Resources.Add("Metal", 100, 100);
        Resources.Add("Metal Ore", 100, 100);
        Resources.Add("Wood", 100, 100);

        var r1 = new Recipe();

        r1.Consume("Coal", 0);
        r1.Consume("Crystal", 0);
        r1.Consume("Metal", 0);
        r1.Consume("Metal Ore", 0);
        r1.Consume("Wood", 0);

        r1.Produce("Coal", 0);
        r1.Produce("Crystal", 0);
        r1.Produce("Metal", 0);
        r1.Produce("Metal Ore", 0);
        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
