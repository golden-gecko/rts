public class struct_Misc_Building_B_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Coal", 10, 1000);
        Resources.Add("Metal", 10, 1000);
        Resources.Add("Wood", 10, 1000);
    }
}
