public class Tree_01 : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 40, 40);

        var r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);

        Health = 10.0f;
        MaxHealth = 10.0f;
    }
}
