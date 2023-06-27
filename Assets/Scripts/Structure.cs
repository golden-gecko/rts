public class Structure : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Health = 100.0f;
        MaxHealth = 100.0f;
    }
}
