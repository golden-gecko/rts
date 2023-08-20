public class Coal : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Coal", 100, 100);

        Recipe r1 = new Recipe("Coal");

        r1.Produces("Coal", 0);

        Recipes.Add(r1);
    }
}
