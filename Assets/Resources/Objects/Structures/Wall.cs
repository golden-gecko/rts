public class Wall : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Health = 300.0f;
        MaxHealth = 300.0f;
    }
}
