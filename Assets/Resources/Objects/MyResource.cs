using System.Linq;
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

    private bool Depleted { get => GetComponents<Storage>().All(x => x.Resources.Items.All(x => x.Empty)); }
}
