using UnityEngine;

[RequireComponent(typeof(Storage))]
public class MyResource : MyGameObject
{
    protected override void Update()
    {
        base.Update();

        if (Depleted)
        {
            Destroy(0);
        }
    }

    private bool Depleted
    {
        get
        {
            foreach (Resource resource in GetComponent<Storage>().Resources.Items)
            {
                if (resource.Storage > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
