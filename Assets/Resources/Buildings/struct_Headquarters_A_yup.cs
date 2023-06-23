public class struct_Headquarters_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Coal", 100, 100);
        Resources.Add("Crystal", 100, 100);
        Resources.Add("Metal", 100, 100);
        Resources.Add("Metal Ore", 100, 100);
        Resources.Add("Wood", 100, 100);

        var r1 = new Recipe();

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

        Health = 100.0f;
        MaxHealth = 100.0f;
    }
}
