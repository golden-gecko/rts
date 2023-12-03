public class JobHandlerUnload : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        if (myGameObject.Storage == null)
        {
            return null;
        }

        if (myGameObject.Storage.Resources.Empty)
        {
            return null;
        }

        foreach (Resource resource in myGameObject.Storage.Resources.Items)
        {
            if (resource.Empty)
            {
                continue;
            }

            MyGameObject placeToStoreResources = myGameObject.Player.GetStorage(myGameObject, resource.Name, resource.Current);

            if (placeToStoreResources == null)
            {
                return null;
            }

            return Order.Unload(placeToStoreResources, Config.Objects.MinDistance, resource.Name, resource.Current);
        }

        return null;
    }
}
