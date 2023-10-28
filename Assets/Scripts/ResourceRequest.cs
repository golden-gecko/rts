public class ResourceRequest
{
    public ResourceRequest(MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        MyGameObject = myGameObject;
        Name = name;
        Value = value;
        Direction = direction;
    }

    public MyGameObject MyGameObject { get; }

    public string Name { get; }

    public int Value { get; set; }

    public ResourceDirection Direction { get; set; }
}
