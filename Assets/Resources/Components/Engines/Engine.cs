using UnityEngine;

public class Engine : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Explore);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Follow);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Guard);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Move);
        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Patrol);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Guard] = new OrderHandlerGuard();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (parent.Working == false)
        {
            return;
        }

        // Grab fuel.
        // Storage storage = GetComponent<Storage>();
        // Fuel.Add(storage.Resources.Remove("Fuel", Fuel.Max - Fuel.Current));
    }

    public override string GetInfo()
    {
        // Storage storage = GetComponent<Storage>();
        // return string.Format("{0}, Power: {1:0.}, Fuel: {2}/{3}/{4} Speed: {5:0.}", base.GetInfo(), Power, storage.Resources.Current("Fuel"), storage.Resources.Max("Fuel"), FuelUsage, Speed);
        return string.Format("{0}, Power: {1:0.}, Fuel: {2}/{3} Speed: {4:0.}", base.GetInfo(), Power, Fuel.GetInfo(), FuelUsage, Speed);
    }

    public bool CanDrive(float distance)
    {
        // return distanceToDrive > distance || GetComponent<Storage>().Resources.CanDec("Fuel");
        return distanceToDrive > distance || Fuel.CanDec();
    }

    public void Drive(float distance)
    {
        if (CanDrive(distance) == false)
        {
            return;
        }

        if (distance > distanceToDrive)
        {
            // GetComponent<Storage>().Resources.Dec("Fuel");
            Fuel.CanDec();

            distanceToDrive += FuelUsage;
        }

        distanceToDrive -= distance;
    }

    [field: SerializeField]
    public float Power { get; set; } = 50.0f;

    [field: SerializeField]
    public Counter Fuel { get; } = new Counter(100, 100);

    [field: SerializeField]
    public float FuelUsage { get; set; } = 1.0f;

    public float Speed { get => Power / GetComponent<MyGameObject>().Mass; }

    private float distanceToDrive = 0.0f;
}
