public class Tree_03 : Plant
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 30, 30);

        Recipe r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
