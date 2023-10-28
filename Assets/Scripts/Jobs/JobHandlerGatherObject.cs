public class JobHandlerGatherObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        MyResource myResource = myGameObject.Player.GetResourceToGather(myGameObject);

        if (myResource != null)
        {
            return Order.GatherObject(myResource);
        }

        return null;
    }
}
