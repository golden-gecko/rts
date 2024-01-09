public class JobHandlerMineObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        if (myGameObject.Miner == null)
        {
            return null;
        }

        MyGameObject resourceToGather = myGameObject.Player.GetResourceToGatherInRange(myGameObject, myGameObject.Miner.Range);

        if (resourceToGather == null)
        {
            return null;
        }

        return Order.MineObject(resourceToGather);
    }
}
