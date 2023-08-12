public class Tree_001 : Plant
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 10, 10);

        Recipe r1 = new Recipe("Wood");

        r1.Produces("Wood", 0);

        Recipes.Add(r1);
    }
}
