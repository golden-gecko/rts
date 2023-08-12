
public class Tree_02 : Plant
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 20, 20);

        Recipe r1 = new Recipe("Wood");

        r1.Produces("Wood", 0);

        Recipes.Add(r1);
    }
}
