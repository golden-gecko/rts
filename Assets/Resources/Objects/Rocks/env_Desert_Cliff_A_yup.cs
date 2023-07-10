public class env_Desert_Cliff_A_yup : Rock
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Metal Ore", 300, 300);

        Recipe r1 = new Recipe();

        r1.Produce("Metal Ore", 0);

        Recipes.Add(r1);
    }
}
