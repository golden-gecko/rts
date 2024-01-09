public class JobHandlerGatherObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        MyGameObject myResource = myGameObject.Player.GetResourceToGather(myGameObject);

        if (myResource == null)
        {
            return null;
        }

        return Order.GatherObject(myResource);
    }
}
