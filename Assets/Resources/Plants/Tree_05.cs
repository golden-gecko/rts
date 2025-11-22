public class Tree_05 : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 80, 80);

        Recipe r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);

        Health = 10.0f;
        MaxHealth = 10.0f;
    }
}
