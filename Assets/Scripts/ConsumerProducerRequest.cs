public class ConsumerProducerRequest
{
    public ConsumerProducerRequest(MyGameObject myGameObject, string name, int value)
    {
        MyGameObject = myGameObject;
        Name = name;
        Value = value;
    }

    public void Set(int value)
    {
        Value = value;
    }

    public MyGameObject MyGameObject { get; }

    public string Name { get; }

    public int Value { get; private set; }
}
