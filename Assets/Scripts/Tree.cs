using System.Collections.Generic;

public class Tree : MyGameObject
{
    void Start()
    {
        Resources = new List<Resource>();
        Resources.Add(new Resource("Wood", 100));
    }

    void Update()
    {
    }

    private List<Resource> Resources { get; set; }
}
