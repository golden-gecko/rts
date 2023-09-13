using System.Linq;
using UnityEngine;

public class JobHandlerUnload : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        Storage storage = myGameObject.GetComponent<Storage>();

        if (storage == null)
        {
            return null;
        }

        if (storage.Resources.StorageSum <= 0)
        {
            return null;
        }

        foreach (Resource resource in storage.Resources.Items)
        {
            if (resource.Current > 0)
            {
                MyGameObject placeToStoreResources = GetStorage(myGameObject, resource);

                if (placeToStoreResources)
                {
                    return Order.Unload(placeToStoreResources, resource.Name, resource.Current);
                }
            }
        }

        return null;
    }

    private MyGameObject GetStorage(MyGameObject myGameObject, Resource resource)
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (Storage storage in Object.FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            MyGameObject parent = storage.GetComponent<MyGameObject>();

            if (parent == null)
            {
                continue;
            }

            if (parent.Working == false)
            {
                continue;
            }

            if (parent == myGameObject)
            {
                continue;
            }

            string[] resourcesFromStorage = new string[] { resource.Name };
            string[] resourcesFromCapacity = storage.Resources.Items.Where(x => x.In && x.Full == false).Select(x => x.Name).ToArray();
            string[] match = resourcesFromStorage.Intersect(resourcesFromCapacity).ToArray();

            if (match.Length <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - parent.Position).magnitude;

            if (magnitude < distance)
            {
                closest = parent;
                distance = magnitude;
            }
        }

        return closest;
    }
}
