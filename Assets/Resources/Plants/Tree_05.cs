public class Tree_05 : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Wood", 80, 80);

        var r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
