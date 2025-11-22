public class Tree_01 : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Resources.Add("Wood", 10, 10);

        Recipe r1 = new Recipe();

        r1.Produce("Wood", 0);

        Recipes.Add(r1);
    }
}
