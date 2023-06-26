using UnityEngine;

public class OrderHandlerDestroy : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Object.Instantiate(Resources.Load("Effects/WFXMR_ExplosiveSmoke"), myGameObject.Position, Quaternion.identity);

        GameObject.Destroy(myGameObject.gameObject);

        myGameObject.Orders.Pop();
    }
}
