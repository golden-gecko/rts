public class ResourceRequest
{
    public ResourceRequest(MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        MyGameObject = myGameObject;
        Name = name;
        Value = value;
        Direction = direction;
    }
    
    public string GetInfo()
    {
        return string.Format("{0} {1} {2}", Name, Value, Direction);
    }

    public MyGameObject MyGameObject { get; }

    public string Name { get; }

    public int Value { get; set; }

    public ResourceDirection Direction { get; set; }
}
