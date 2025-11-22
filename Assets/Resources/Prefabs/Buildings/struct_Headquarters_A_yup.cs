public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Coal", 100, 100);
        Resources.Add("Metal", 100, 100);
        Resources.Add("Wood", 100, 100);
    }
}
