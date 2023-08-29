public class Crystal : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Storage>().Resources.Add("Crystal", 100, 100, ResourceDirection.Out);
    }
}
