public class Research_Lab : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowTechnology("Colonization");
        Orders.AllowTechnology("Infantry");
        Orders.AllowTechnology("Heavy_Industry");
        Orders.AllowTechnology("Radar");
        Orders.AllowTechnology("Space_Travels");
        Orders.AllowTechnology("Static_Defences");
        Orders.AllowTechnology("Stationary_Defences");
    }
}
