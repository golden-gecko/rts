public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Coal", new Resource("Coal", 100, 1000));
        Resources.Add("Metal", new Resource("Metal", 100, 1000));
        Resources.Add("Wood", new Resource("Wood", 100, 1000));
    }
}
