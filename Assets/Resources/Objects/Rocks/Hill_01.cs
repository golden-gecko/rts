public class Hill_01 : Rock
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Metal Ore", 100, 100);

        Recipe r1 = new Recipe("Metal Ore");

        r1.Produces("Metal Ore", 0);

        Recipes.Add(r1);
    }
}
