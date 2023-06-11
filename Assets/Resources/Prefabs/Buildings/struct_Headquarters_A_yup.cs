public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Coal", 100, 100);
        Resources.Add("Metal", 100, 100);
        Resources.Add("Wood", 100, 100);

        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Heavy_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Misc_Building_B_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Radar_Outpost_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Refinery_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Research_Lab_A_yup");
    }
}
