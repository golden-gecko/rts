public class Refinery : Structure
{
    protected override void Start()
    {
        base.Start();

        Producer producer = GetComponent<Producer>();

        producer.Recipes.Add(RecipeManager.Instance.Get("Charcoal"));
        producer.Recipes.Add(RecipeManager.Instance.Get("Fuel"));
        producer.Recipes.Add(RecipeManager.Instance.Get("Iron"));
    }
}
