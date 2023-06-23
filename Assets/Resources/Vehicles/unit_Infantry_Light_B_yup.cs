public class unit_Infantry_Light_B_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 2;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 5;
        ReloadTimer = new Timer(1.0f);
    }
}
