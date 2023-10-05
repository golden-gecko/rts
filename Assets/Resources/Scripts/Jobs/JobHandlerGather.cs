using UnityEngine;

public class JobHandlerGather : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        MyResource myResource = myGameObject.Player.GetResourceToGather(myGameObject);

        return Order.Gather(myResource);
    }
}
