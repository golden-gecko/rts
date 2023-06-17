using System.Collections.Generic;

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

public class ConsumerProducerContainer
{
    public ConsumerProducerContainer()
    {
        Items = new List<ConsumerProducerRequest>();
    }

    public void Add(MyGameObject myGameObject, string name, int value)
    {
        foreach (var i in Items)
        {
            if (i.MyGameObject == myGameObject && i.Name == name)
            {
                i.Value = value;

                return;
            }
        }

        Items.Add(new ConsumerProducerRequest(myGameObject, name, value));
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
        foreach (var i in Items)
        {
            if (i.MyGameObject == myGameObject && i.Name == name)
            {
                Items.Remove(i);

                break;
            }
        }
    }

    public void Clear()
    {
        Items.Clear();
    }

    public int Count { get => Items.Count; }

    public List<ConsumerProducerRequest> Items { get; }
}
