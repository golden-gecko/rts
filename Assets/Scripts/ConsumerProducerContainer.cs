using System.Collections.Generic;

public class ConsumerProducerContainer
{
    public ConsumerProducerContainer()
    {
        Items = new List<ConsumerProducerRequest>();
    }

    public void Add(MyGameObject myGameObject, string name, int value)
    {
        foreach (ConsumerProducerRequest request in Items)
        {
            if (request.MyGameObject == myGameObject && request.Name == name)
            {
                request.Set(value);

                return;
            }
        }

        Items.Add(new ConsumerProducerRequest(myGameObject, name, value));
    }

    public void Clear()
    {
        Items.Clear();
    }

    public void MoveToEnd()
    {
        if (Items.Count > 0)
        {
            Items.Add(Items[0]);
            Items.RemoveAt(0);
        }
    }

    public void Remove(MyGameObject myGameObject, string name)
    {
        foreach (ConsumerProducerRequest request in Items)
        {
            if (request.MyGameObject == myGameObject && request.Name == name)
            {
                Items.Remove(request);

                break;
            }
        }
    }

    public int Count { get => Items.Count; }

    public List<ConsumerProducerRequest> Items { get; }
}
