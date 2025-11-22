public class Headquarters : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Coal", 100, 200);
        Resources.Add("Crystal", 100, 200);
        Resources.Add("Metal", 100, 200);
        Resources.Add("Metal Ore", 200, 200);
        Resources.Add("Wood", 100, 200);

        Recipe r1 = new Recipe("Storage");

        r1.Consume("Coal", 0);
        r1.Consume("Crystal", 0);
        r1.Consume("Metal", 0);
        r1.Consume("Metal Ore", 0);
        r1.Consume("Wood", 0);

        r1.Produce("Coal", 0);
        r1.Produce("Crystal", 0);
        r1.Produce("Metal", 0);
        r1.Produce("Metal Ore", 0);
        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
