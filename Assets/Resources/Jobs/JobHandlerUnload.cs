public class JobHandlerUnload : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        Storage storage = myGameObject.GetComponent<Storage>();

        if (storage == null)
        {
            return null;
        }

        if (storage.Resources.Sum <= 0)
        {
            return null;
        }

        // TODO: Implmenent.

        return null;
    }
}
