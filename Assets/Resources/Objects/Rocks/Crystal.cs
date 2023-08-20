public class Crystal : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Crystal", 100, 100);

        Recipe r1 = new Recipe("Crystal");

        r1.Produces("Crystal", 0);

        Recipes.Add(r1);
    }
}
