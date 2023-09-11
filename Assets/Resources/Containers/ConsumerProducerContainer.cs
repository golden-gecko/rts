using System.Collections.Generic;

public class ConsumerProducerContainer
{
    public void Add(MyGameObject myGameObject, string name, int value)
    {
        ConsumerProducerRequest request = Items.Find(x => x.MyGameObject == myGameObject && x.Name == name);

        if (request == null)
        {
            Items.Add(new ConsumerProducerRequest(myGameObject, name, value));
        }
        else
        {
            request.Value = value;
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

    public List<ConsumerProducerRequest> Items { get; } = new List<ConsumerProducerRequest>();
}
