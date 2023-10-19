public class JobHandlerGatherObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        MyResource myResource = myGameObject.Player.GetResourceToGather(myGameObject);

        return Order.GatherObject(myResource);
    }
}
