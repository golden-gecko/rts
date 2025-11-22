public class IronOre : MyResource
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Iron Ore", 200, 200);

        Recipe r1 = new Recipe("Iron Ore");

        r1.Produces("Iron Ore", 0);

        Recipes.Add(r1);
    }
}
