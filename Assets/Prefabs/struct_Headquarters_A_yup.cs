public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Coal", 100, 1000);
        Resources.Add("Metal", 100, 1000);
        Resources.Add("Wood", 100, 1000);
    }
}
