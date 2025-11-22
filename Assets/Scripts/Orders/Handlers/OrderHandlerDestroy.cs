using UnityEngine;

public class OrderHandlerDestroy : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        myGameObject.OnDestroyHandler();
    }
}
