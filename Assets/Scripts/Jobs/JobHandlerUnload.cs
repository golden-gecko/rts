public class JobHandlerUnload : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        Storage storage = myGameObject.GetComponent<Storage>();

        if (storage == null)
        {
            return null;
        }

        if (storage.Resources.CurrentSum <= 0)
        {
            return null;
        }

        foreach (Resource resource in storage.Resources.Items)
        {
            if (resource.Current > 0)
            {
                MyGameObject placeToStoreResources = myGameObject.Player.GetStorage(myGameObject, resource.Name, resource.Current);

                if (placeToStoreResources)
                {
                    return Order.Unload(placeToStoreResources, resource.Name, resource.Current);
                }
            }
        }

        return null;
    }
}
