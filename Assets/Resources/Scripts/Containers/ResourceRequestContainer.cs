using System.Collections.Generic;

public class ResourceRequestContainer
{
    public void Add(MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        ResourceRequest request = Items.Find(x => x.MyGameObject == myGameObject && x.Name == name);

        if (request == null)
        {
            Items.Add(new ResourceRequest(myGameObject, name, value, direction));
        }
        else
        {
            request.Value = value;
            request.Direction = direction;
        }
    }

    public void Remove(MyGameObject myGameObject, string name)
    {
        Items.RemoveAll(x => x.MyGameObject == myGameObject && x.Name == name);
    }

    public void MoveToEnd()
    {
        if (Items.Count > 1)
        {
            Items.Add(Items[0]);
            Items.RemoveAt(0);
        }
    }

    public List<ResourceRequest> Items { get; } = new List<ResourceRequest>();
}
