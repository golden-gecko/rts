public class Tree_05 : Plant
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 50, 50);

        Recipe r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
