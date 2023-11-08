public class OrderHandlerRally : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.GetComponent<Assembler>().RallyPoint = order.TargetPosition;
        myGameObject.Orders.Pop();
    }
}
