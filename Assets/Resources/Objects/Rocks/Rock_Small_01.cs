public class Rock_Small_01 : Rock
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Metal Ore", 50, 50);

        Recipe r1 = new Recipe("Metal Ore");

        r1.Produce("Metal Ore", 0);

        Recipes.Add(r1);
    }
}
