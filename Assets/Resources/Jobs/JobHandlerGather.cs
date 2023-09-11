using System.Linq;
using UnityEngine;

public class JobHandlerGather : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in Object.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
        {
            if (myResource.Working == false)
            {
                continue;
            }

            if (myResource == myGameObject)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude < distance)
            {
                if (GetStorage(myGameObject, myResource) == null)
                {
                    continue;
                }

                closest = myResource;
                distance = magnitude;
            }
        }

        if (closest == null)
        {
            return null;
        }

        return Order.Gather(closest);
    }

    private MyGameObject GetStorage(MyGameObject myGameObject, MyResource myResource)
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

            if (parent == myGameObject)
            {
                continue;
            }

            if (parent == myResource)
            {
                continue;
            }

            string[] resourcesFromStorage = myResource.GetComponent<Storage>().Resources.Items.Where(x => x.Out && x.Empty == false).Select(x => x.Name).ToArray();
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
