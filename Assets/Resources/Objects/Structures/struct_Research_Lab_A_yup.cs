public class struct_Research_Lab_A_yup : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Research);

        Orders.AllowTechnology("Colonization");
        Orders.AllowTechnology("Heavy Industry");
        Orders.AllowTechnology("Radar");
        Orders.AllowTechnology("Space Travels");
        Orders.AllowTechnology("Static Defences");
        Orders.AllowTechnology("Stationary Defences");

        Resources.Add("Crystal", 0, 80);

        Recipe r1 = new Recipe();

        r1.Consume("Crystal", 0);

        Recipes.Add(r1);
    }
}
