public class Headquarters : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Coal", 100, 100);
        Resources.Add("Crystal", 100, 100);
        Resources.Add("Metal", 100, 100);
        Resources.Add("Metal Ore", 100, 100);
        Resources.Add("Wood", 100, 100);

        Recipe r1 = new Recipe("Storage");

        r1.Consumes("Coal", 0);
        r1.Consumes("Crystal", 0);
        r1.Consumes("Metal", 0);
        r1.Consumes("Metal Ore", 0);
        r1.Consumes("Wood", 0);

        r1.Produces("Coal", 0);
        r1.Produces("Crystal", 0);
        r1.Produces("Metal", 0);
        r1.Produces("Metal Ore", 0);
        r1.Produces("Wood", 0);

        Recipes.Add(r1);
    }
}
