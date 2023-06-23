public class unit_Trike_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 12;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 4;
        ReloadTimer = new Timer(2.0f);
    }
}
