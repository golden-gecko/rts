public class IronOre : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Storage>().Resources.Add("Iron Ore", 200, 200, ResourceDirection.Out);
    }
}
