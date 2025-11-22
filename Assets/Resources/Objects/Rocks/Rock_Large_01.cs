public class Rock_Large_01 : Rock
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Metal Ore", 200, 200);

        Recipe r1 = new Recipe("Metal Ore");

        r1.Produces("Metal Ore", 0);

        Recipes.Add(r1);
    }
}
