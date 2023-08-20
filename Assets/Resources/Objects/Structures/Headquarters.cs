public class Headquarters : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Coal", 100, 100);
        Resources.Add("Crystal", 100, 100);
        Resources.Add("Iron", 100, 100);
        Resources.Add("Iron Ore", 100, 100);
        Resources.Add("Wood", 100, 100);

        Recipe r1 = new Recipe("Storage");

        r1.Consumes("Coal", 0);
        r1.Consumes("Crystal", 0);
        r1.Consumes("Iron", 0);
        r1.Consumes("Iron Ore", 0);
        r1.Consumes("Wood", 0);

        r1.Produces("Coal", 0);
        r1.Produces("Crystal", 0);
        r1.Produces("Iron", 0);
        r1.Produces("Iron Ore", 0);
        r1.Produces("Wood", 0);

        Recipes.Add(r1);
    }
}
