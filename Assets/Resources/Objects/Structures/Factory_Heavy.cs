using System.IO;

public class Factory_Heavy : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble);
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Combat"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Missile"));

        Resources.Add("Iron", 0, 40);

        Recipe r1 = new Recipe("Iron");

        r1.Consumes("Iron", 0);

        Recipes.Add(r1);
    }
}
