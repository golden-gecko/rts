public class Refinery : Structure
{
    protected override void Start()
    {
        base.Start();

        GetComponent<Producer>().Recipes.Add(RecipeManager.Instance.Get("Iron"));
    }
}
