public interface IOrderHandler
{
    bool IsValid(Order order);

    void OnExecute(MyGameObject myGameObject);
}
