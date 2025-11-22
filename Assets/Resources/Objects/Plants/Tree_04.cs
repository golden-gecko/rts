public class Tree_04 : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 40, 40);

        Recipe r1 = new Recipe("Wood");

        r1.Produces("Wood", 0);

        Recipes.Add(r1);
    }
}
