public class Plant : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        MissileRange = 0.0f;
        VisibilityRange = 0.0f;
    }
}
