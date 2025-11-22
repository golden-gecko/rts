using UnityEngine;

public class OrderHandlerDestroyMissile : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Object.Instantiate(Resources.Load("Effects/WFXMR_Explosion StarSmoke"), myGameObject.Position, Quaternion.identity); // TODO: Move effect name to configuration.

        GameObject.Destroy(myGameObject.gameObject);

        myGameObject.Orders.Pop();
    }
}
