using System.Collections.Generic;

public class ResourceContainer
{
    public ResourceContainer()
    {
        Items = new Dictionary<string, Resource>();
    }

    public Dictionary<string, Resource> Items { get; }
}
