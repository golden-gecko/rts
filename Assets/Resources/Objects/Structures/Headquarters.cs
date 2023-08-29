public class Headquarters : Structure
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Storage>().Resources.Add("Coal", 100, 100, ResourceDirection.Both);
        GetComponent<Storage>().Resources.Add("Crystal", 100, 100, ResourceDirection.Both);
        GetComponent<Storage>().Resources.Add("Iron", 100, 100, ResourceDirection.Both);
        GetComponent<Storage>().Resources.Add("Iron Ore", 100, 100, ResourceDirection.Both);
        GetComponent<Storage>().Resources.Add("Wood", 100, 100, ResourceDirection.Both);
    }
}
