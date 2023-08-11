public class Plant : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        VisibilityRange = 0.0f;
    }

    protected override void Update()
    {
        base.Update();

        if (IsDepleted())
        {
            Destroy(0);
        }
    }

    private bool IsDepleted()
    {
        foreach (Resource resource in Resources.Items.Values)
        {
            if (resource.Storage > 0)
            {
                return false;
            }
        }

        return true;
    }
}
