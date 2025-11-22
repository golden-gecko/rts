public class GreenTree : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Storage>().Resources.Add("Wood", 10, 10, ResourceDirection.Out);
    }
}
