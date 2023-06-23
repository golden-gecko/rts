public class unit_Tank_Missile_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 6;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 30;
        ReloadTimer = new Timer(5.0f);
    }
}
