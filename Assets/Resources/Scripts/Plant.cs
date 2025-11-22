using System.Collections.Generic;

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
        foreach (KeyValuePair<string, Resource> resource in Resources.Items)
        {
            if (resource.Value.Storage() > 0)
            {
                return false;
            }
        }

        return true;
    }
}
