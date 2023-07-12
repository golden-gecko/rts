public class Rock_Large_01 : Rock
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Metal Ore", 200, 200);

        Recipe r1 = new Recipe();

        r1.Produce("Metal Ore", 0);

        Recipes.Add(r1);
    }
}
