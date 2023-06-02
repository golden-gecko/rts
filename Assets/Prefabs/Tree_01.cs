public class Tree_01 : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Resources.Add("Wood", new Resource("Wood", 20, 20));
    }
}
