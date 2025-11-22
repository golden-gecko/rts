public class ConsumerProducerRequest
{
    public ConsumerProducerRequest(MyGameObject myGameObject, string name, int value)
    {
        MyGameObject = myGameObject;
        Name = name;
        Value = value;
    }

    public MyGameObject MyGameObject { get; }

    public string Name { get; }

    public int Value { get; set; }
}
