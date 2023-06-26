public class Rocket : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Damage = 10.0f;
    }
}
