public class Research_Lab : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Research);

        Orders.AllowTechnology("Colonization");
        Orders.AllowTechnology("Infantry");
        Orders.AllowTechnology("Heavy_Industry");
        Orders.AllowTechnology("Radar");
        Orders.AllowTechnology("Space_Travels");
        Orders.AllowTechnology("Static_Defences");
        Orders.AllowTechnology("Stationary_Defences");

        Resources.Add("Crystal", 0, 80);

        Recipe r1 = new Recipe("Crystal");

        r1.Consume("Crystal", 0);

        Recipes.Add(r1);
    }
}
