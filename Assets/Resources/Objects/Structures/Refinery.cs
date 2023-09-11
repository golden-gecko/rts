public class Refinery : Structure
{
    protected override void Start()
    {
        base.Start();

        Producer producer = GetComponent<Producer>();
        RecipeManager recipeManager = Game.Instance.GetComponent<RecipeManager>();

        producer.Recipes.Add(recipeManager.Get("Charcoal"));
        producer.Recipes.Add(recipeManager.Get("Fuel"));
        producer.Recipes.Add(recipeManager.Get("Iron"));
    }
}
