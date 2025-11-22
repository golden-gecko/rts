public class Recipe
{
    public Recipe(string consume, string produce)
    {
        Consume = consume;
        Produce = produce;
    }

    public string Consume { get; }
    public string Produce { get; }
}
