using UnityEngine;

public class OrderHandlerDestroyMissile : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Object.Instantiate(Resources.Load("Effects/WFXMR_Explosion StarSmoke"), myGameObject.Position, Quaternion.identity);

        GameObject.Destroy(myGameObject.gameObject);

        myGameObject.Orders.Pop();
    }
}
