public class Coal : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Storage>().Resources.Add("Coal", 100, 100, ResourceDirection.Out);
    }
}
