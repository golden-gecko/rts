public class Bullet : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Damage = 1.0f;
    }
}
