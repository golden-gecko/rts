public class Tree_01 : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Health = 20;
    }

    protected override void Start()
    {
        base.Start();

        Resources.Add("Wood", 40, 40);

        var r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
