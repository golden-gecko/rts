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
                MyGameObject storage = null;

                foreach (Resource i in myResource.GetComponent<Storage>().Resources.Items)
                {
                    storage = myGameObject.Player.GetStorage(myGameObject, i.Name, i.Current);

                    if (storage != null)
                    {
                        break;
                    }
                }

                if (storage == null)
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
}
