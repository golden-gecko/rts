using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Storage))]
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

    public override string GetInfo()
    {
        Storage storage = GetComponent<Storage>();

        return string.Format("{0}, Power: {1:0.}, Fuel: {2}/{3}/{4} Speed: {5:0.}", base.GetInfo(), Power, storage.Resources.Current("Fuel"), storage.Resources.Max("Fuel"), FuelUsage, Speed);
    }

    public bool CanDrive(float distance) // TODO: This will make unit move a bit when fuel is depleted. Remove fuel first and then move.
    {
        if (distanceDriven + distance < DistancePerUnit)
        {
            return true;
        }

        return GetComponent<Storage>().Resources.CanRemove("Fuel", 1);
    }

    public void Drive(float distance) // TODO: This will make unit move a bit when fuel is depleted. Remove fuel first and then move.
    {
        if (CanDrive(distance) == false)
        {
            return;
        }

        distanceDriven += distance;

        if (distanceDriven > DistancePerUnit)
        {
            int unitsUsed = Mathf.FloorToInt(distanceDriven / DistancePerUnit);

            GetComponent<Storage>().Resources.Remove("Fuel", unitsUsed);

            distanceDriven -= unitsUsed * DistancePerUnit;
        }
    }

    [field: SerializeField]
    public float Power { get; set; } = 50.0f;
    [field: SerializeField]
    public float FuelUsage { get; set; } = 1.0f;

    public float Speed { get => Power / GetComponent<MyGameObject>().Mass; }

    private float DistancePerUnit { get => 1.0f / FuelUsage; }

    private float distanceDriven = 0.0f;
}
